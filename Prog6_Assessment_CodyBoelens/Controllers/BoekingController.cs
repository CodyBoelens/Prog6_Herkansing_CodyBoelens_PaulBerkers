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

        public BoekingController(IBoekingService boekingService, IKlantService klantService, IBeestjeService beestjeService)
        {
            _boekingService = boekingService;
            _klantService = klantService;
            _beestjeService = beestjeService;
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

            var beestjes = await _beestjeService.GetAllBeestjesAsync();

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
            // Validate the selected beestjes using the BoekingValidation method
            var errors = await _boekingService.BoekingValidation(model);

            // If there are validation errors, return the view with those errors
            if (errors.Any())
            {
                // Re-fetch Beestjes for the view to display again
                model.Beestjes = await _beestjeService.GetAllBeestjesAsync();

                // Add the errors to the ModelState to display them in the view
                foreach (var error in errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return View(model);
            }

            HttpContext.Session.SetString("SelectedBeestjes", JsonConvert.SerializeObject(model.SelectedBeestjesIds));

            // Proceed with further logic after validation is successful (e.g., save data, move to next step)
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

            //calculate total price

            // Create the ViewModel
            var model = new Step04ViewModel
            {
                Klant = klant,
                Beestjes = selectedBeestjes,
                Datum = eventDate
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmBooking()
        {
            DateTime eventDate = getEventDateFromSession();
            if (eventDate == default) return RedirectToAction("Index", "Home");

            List<int> selectedBeestjesIds = getSelectedBeestjesIdsFromSession();
            if (!selectedBeestjesIds.Any()) return RedirectToAction("Step03");
            var selectedBeestjes = await _beestjeService.GetBeestjesByIdsAsync(getSelectedBeestjesIdsFromSession());

            KlantViewModels klant = getKlantFromSession();
            if (klant == null) return RedirectToAction("Step02");

            // Create the ViewModel
            var model = new Step04ViewModel
            {
                Klant = klant,
                Beestjes = selectedBeestjes,
                Datum = getEventDateFromSession()
            };

            //save booking

            // Clear the session after booking
            HttpContext.Session.Remove("selectedEventDate");
            HttpContext.Session.Remove("KlantInfo");
            HttpContext.Session.Remove("SelectedBeestjes");

            // Redirect to a confirmation page
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
