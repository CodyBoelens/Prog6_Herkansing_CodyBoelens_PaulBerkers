using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prog6_Assessment_CodyBoelens.Data.DbEntities;
using Prog6_Assessment_CodyBoelens.Interfaces;
using Prog6_Assessment_CodyBoelens.Services;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BoekingsViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;

namespace Prog6_Assessment_CodyBoelens.Controllers
{
    public class BoekingController : Controller
    {
        private readonly IBoekingService _boekingService;
        private readonly IKlantService _klantService;
        private readonly IBeestjeService _beestjeService;
        private readonly IKortingService _kortingService; 
        private readonly IBookingRulesService _bookingRulesService;

        public BoekingController(IBoekingService boekingService, IKlantService klantService, IBeestjeService beestjeService, IKortingService kortingService, IBookingRulesService bookingRulesService)
        {
            _boekingService = boekingService;
            _klantService = klantService;
            _beestjeService = beestjeService;
            _kortingService = kortingService;
            _bookingRulesService = bookingRulesService;
            _bookingRulesService = bookingRulesService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Step01(DateTime eventDate)
        {
            if (eventDate <= DateTime.Today)
            {
                TempData["EventDateErrorStep01"] = "Selecteer een geldige datum in de toekomst.";
                return RedirectToAction("Index", "Home");
            }

            HttpContext.Session.SetString("selectedEventDate", eventDate.ToString("yyyy-MM-dd"));

            // Redirect to Step 2, passing eventDate as a query string
            return RedirectToAction("Step02");
        }

        [HttpGet]
        public async Task<IActionResult> Step02()
        {
            DateTime eventDate = getEventDateFromSession();
            if (eventDate == default) return RedirectToAction("Index", "Home");

            ViewBag.EventDate = eventDate;

            if (User.Identity.IsAuthenticated)
            {
                // Get the current user's ApplicationUserId
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId != null)
                {
                    // Fetch the klant using the ApplicationUserId
                    var klant = _klantService.GetKlantByApplicationUserId(userId);

                    return View(klant);
                }
            }

            string klantJson = HttpContext.Session.GetString("KlantInfo");
            KlantViewModels sessionKlant = string.IsNullOrEmpty(klantJson) ? null : JsonConvert.DeserializeObject<KlantViewModels>(klantJson);

            return View(sessionKlant);
        }

        [HttpPost]
        public async Task<IActionResult> Step02(KlantViewModels klant)
        {
            DateTime eventDate = getEventDateFromSession();
            if (eventDate == default) return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
            {
                return View(klant);
            }

            if (klant.KlantkaartId == null) klant.KlantkaartId = 0;

            HttpContext.Session.SetString("KlantInfo", JsonConvert.SerializeObject(klant));

            return RedirectToAction("Step03");
        }


        [HttpGet]
        public async Task<IActionResult> Step03()
        {
            DateTime eventDate = getEventDateFromSession();
            if (eventDate == default) return RedirectToAction("Index", "Home");

            List<int> selectedBeestjesIds = getSelectedBeestjesIdsFromSession();

            KlantViewModels klant = getKlantFromSession();
            if (klant == null) return RedirectToAction("Step02");

            var beestjes = await _beestjeService.GetAvailableBeestjesAsync(eventDate);

            var model = new Step03ViewModel
            {
                Klant = klant,
                Beestjes = beestjes,
                Datum = eventDate,
                SelectedBeestjesIds = selectedBeestjesIds
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Step03(Step03ViewModel model)
        {
            List<BeestjeViewModels> beestjeViewModels = await _beestjeService.GetBeestjesByIdsAsync(model.SelectedBeestjesIds);

            List<string> errors = _bookingRulesService.ValidateBookingSelection(model.Klant, beestjeViewModels, model.Datum);

            if (errors.Count > 0)
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                model.Beestjes = await _beestjeService.GetAvailableBeestjesAsync(model.Datum);

                return View(model);
            }

            HttpContext.Session.SetString("SelectedBeestjes", JsonConvert.SerializeObject(model.SelectedBeestjesIds));

            return RedirectToAction("Step04");
        }

        [HttpGet]
        public async Task<IActionResult> Step04()
        {
            DateTime eventDate = getEventDateFromSession();
            if (eventDate == default) return RedirectToAction("Index", "Home");

            List<int> selectedBeestjesIds = getSelectedBeestjesIdsFromSession();
            if (!selectedBeestjesIds.Any()) return RedirectToAction("Step03");
            var selectedBeestjes = await _beestjeService.GetBeestjesByIdsAsync(getSelectedBeestjesIdsFromSession());

            KlantViewModels klant = getKlantFromSession();
            if (klant == null) return RedirectToAction("Step02");

            // Calculate total price
            double totaalPrijs = selectedBeestjes.Sum(b => b.Price);

            // Use the injected KortingService to calculate the discount
            int discount = _kortingService.GetTotalDiscountPercentage(klant, selectedBeestjes, eventDate);
            double totalPrice = Math.Round(totaalPrijs * (1 - (discount / 100.0)), 2);

            HttpContext.Session.SetString("TotalPrice", totalPrice.ToString());

            // Create the ViewModel
            var model = new Step04ViewModel
            {
                Klant = klant,
                Beestjes = selectedBeestjes,
                Datum = eventDate,
                TotaalPrijs = totaalPrijs,
                TotaalPrijsMetKorting = totalPrice, // Adding the total price after discount
                Discount = discount
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> ConfirmBooking()
        {
            DateTime eventDate = getEventDateFromSession();
            if (eventDate == default) return RedirectToAction("Index", "Home");

            double totaalPrijsMetKorting = Convert.ToDouble(HttpContext.Session.GetString("TotalPrice"));

            List<int> selectedBeestjesIds = getSelectedBeestjesIdsFromSession();
            if (!selectedBeestjesIds.Any()) return RedirectToAction("Step03");
            var selectedBeestjes = await _beestjeService.GetBeestjesByIdsAsync(getSelectedBeestjesIdsFromSession());

            KlantViewModels klant = getKlantFromSession();
            if (klant == null) return RedirectToAction("Step02");

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                klant = _klantService.GetKlantByApplicationUserId(userId);
            }

            // Map data to BoekingViewModel and save the booking
            var boekingViewModel = new BoekingViewModel
            {
                Date = eventDate,
                Name = klant.Name,
                Adress = klant.Adres,
                PhoneNumber = klant.PhoneNumber,
                Email = klant.Email, 
                Is_Confirmed = true,
                TotaalPrijs = totaalPrijsMetKorting,
                KlantId = klant.Id > 0 ? klant.Id : null, 
                BeestjeIds = selectedBeestjesIds
            };

            await _boekingService.AddBoekingAsync(boekingViewModel);

            // Clear the session after booking
            HttpContext.Session.Remove("selectedEventDate");
            HttpContext.Session.Remove("KlantInfo");
            HttpContext.Session.Remove("SelectedBeestjes");
            HttpContext.Session.Remove("TotalPrice");

            // Redirect to a confirmation page
            TempData["orderSucces"] = "De boeking is gelukt!";
            return RedirectToAction("Index", "Home");
        }


        private DateTime getEventDateFromSession()
        {
            // Retrieve the date from session
            var eventDateString = HttpContext.Session.GetString("selectedEventDate");

            DateTime eventDate = default;

            // Convert the stored string back to DateTime
            if (!string.IsNullOrEmpty(eventDateString) && DateTime.TryParse(eventDateString, out DateTime parsedDate))
            {
                eventDate = parsedDate;
            }

            ViewBag.EventDate = eventDate;

            return eventDate;
        }

        private KlantViewModels getKlantFromSession()
        {
            string klantJson = HttpContext.Session.GetString("KlantInfo");
            KlantViewModels klant = string.IsNullOrEmpty(klantJson) ? null : JsonConvert.DeserializeObject<KlantViewModels>(klantJson);

            return klant;
        }

        private List<int> getSelectedBeestjesIdsFromSession()
        {
            string selectedBeestjesJson = HttpContext.Session.GetString("SelectedBeestjes");
            List<int> selectedBeestjesIds = string.IsNullOrEmpty(selectedBeestjesJson) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(selectedBeestjesJson);

            return selectedBeestjesIds;
        }
    }

}
