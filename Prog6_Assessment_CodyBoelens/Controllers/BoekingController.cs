using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prog6_Assessment_CodyBoelens.Data.DbEntities;
using Prog6_Assessment_CodyBoelens.Interfaces;
using Prog6_Assessment_CodyBoelens.Services;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BoekingsViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;
using System.Security.Claims;

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
            return View();
        }

        [HttpPost]
        public IActionResult Step01(DateTime eventDate)
        {
            if (eventDate == default)
            {
                ModelState.AddModelError("eventDate", "Selecteer een geldige datum.");
                return View();
            }

            // Redirect to Step 2, passing eventDate as a query string
            return RedirectToAction("Step02", new { eventDate });
        }

        [HttpGet]
        public async Task<IActionResult> Step02(DateTime eventDate)
        {
            if (eventDate == default)
            {
                // Redirect if no event date is provided
                return RedirectToAction("Step01");
            }

            ViewBag.EventDate = eventDate;

            if (User.Identity.IsAuthenticated)
            {
                // Get the current user's ApplicationUserId
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId != null)
                {
                    // Fetch the klant using the ApplicationUserId
                    var klant = _klantService.GetKlantByApplicationUserId(userId);

                    return View("~/Views/Boekingen/Step02.cshtml", klant);
                }
            }

            return View("~/Views/Boekingen/Step02.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Step02(KlantViewModels klant, DateTime eventDate)
        {
            if (eventDate == default)
            {
                // Redirect if no event date is provided
                return RedirectToAction("Step01");
            }

            return RedirectToAction("Step03", new { klant.Name, klant.Email, klant.PhoneNumber, klant.Adres, klant.KlantkaartId, eventDate });
        }


        [HttpGet]
        public async Task<IActionResult> Step03(KlantViewModels klant, DateTime eventDate)
        {
            var beestjes = await _beestjeService.GetAllBeestjesAsync();

            var model = new Step03ViewModel
            {
                Klant = klant,
                Beestjes = beestjes,
                Datum = eventDate 
            };

            return View("~/Views/Boekingen/Step03.cshtml", model);
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

                return View("~/Views/Boekingen/Step03.cshtml", model);
            }

            // Proceed with further logic after validation is successful (e.g., save data, move to next step)
            return RedirectToAction("Step04");
        }








    }
}
