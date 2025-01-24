using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prog6_Assessment_CodyBoelens.Data.DbEntities;
using Prog6_Assessment_CodyBoelens.Interfaces;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;
using System.Security.Claims;

namespace Prog6_Assessment_CodyBoelens.Controllers
{
    public class BoekingController : Controller
    {
        private readonly IBoekingService _boekingService;
        private readonly IKlantService _klantService;

        public BoekingController(IBoekingService boekingService, IKlantService klantService)
        {
            _boekingService = boekingService;
            _klantService = klantService;
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
        public async Task<IActionResult> Step02(KlantViewModel klant, DateTime eventDate)
        {
            if (eventDate == default)
            {
                // Redirect if no event date is provided
                return RedirectToAction("Step01");
            }

            ViewBag.EventDate = eventDate;

            return RedirectToAction("Step03",  klant);
        }

        [HttpGet]
        public IActionResult Step03(KlantViewModel klant)
        {
            // Process the klant model for Step03
            return View("~/Views/Boekingen/Step03.cshtml", klant);
        }




    }
}
