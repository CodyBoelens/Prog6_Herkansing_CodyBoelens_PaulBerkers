using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prog6_Assessment_CodyBoelens.Data.DbEntities;
using Prog6_Assessment_CodyBoelens.Interfaces;
using Prog6_Assessment_CodyBoelens.Services;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BoekingsViewModel;

namespace Prog6_Assessment_CodyBoelens.Controllers
{
    [Authorize(Roles = "Boerderij")]
    public class BoekingManagementController : Controller
    {
        private readonly IBoekingService _boekingService;
        private readonly IKlantService _klantService;
        private readonly IBeestjeService _beestjeService;

        public BoekingManagementController(IBoekingService boekingService, IKlantService klantService, IBeestjeService beestjeService)
        {
            _boekingService = boekingService;
            _klantService = klantService;
            _beestjeService = beestjeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var bookings = await _boekingService.GetAllBoekingsAsync();
            return View(bookings);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var Boeking = await _boekingService.GetBookingByIdAsync(id);

            if (Boeking == null)
            {
                return NotFound();
            }

            List<BeestjeViewModels> bookedBeestjes = await _boekingService.GetBookedBeestjesByIdAsync(id);

            var boekingDetail = new BoekingDetailsViewModel
            {
                Boeking = Boeking,
                BoekedBeestjes = bookedBeestjes,
            };

            return View(boekingDetail);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var boekingViewModel = await _boekingService.GetBookingByIdAsync(id);
            if (boekingViewModel == null)
            {
                return RedirectToAction("Index");
            }

            return View(boekingViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(BoekingViewModel boekingViewModel)
        {
            try
            {
                await _boekingService.DeleteBoekingAsync(boekingViewModel.Id); 
                TempData["successMessage"] = $"Boeking {boekingViewModel.Id} van {boekingViewModel.Name} is verwijderd.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(boekingViewModel);
            }
        }
    }
}
