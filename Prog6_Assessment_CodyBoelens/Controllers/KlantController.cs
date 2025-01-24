using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prog6_Assessment_CodyBoelens.Data;
using Prog6_Assessment_CodyBoelens.Data.DbEntities;
using Prog6_Assessment_CodyBoelens.Data.Migrations;
using Prog6_Assessment_CodyBoelens.Interfaces;
using Prog6_Assessment_CodyBoelens.Services;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;
using System.Security.Cryptography;
using System.Text;

namespace Prog6_Assessment_CodyBoelens.Controllers
{
    [Authorize(Roles = "Boerderij")]
    public class KlantController : Controller
    {
        private readonly IKlantService _klantService;

        public KlantController(IKlantService klantService)
        {
            _klantService = klantService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var klantList = _klantService.GetKlantViewModels();
            return View(klantList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var klantViewModel = new KlantViewModel
            {
                allRanks = _klantService.GetAllKlantkaarten()
            };

            return View(klantViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(KlantViewModel klantViewModel)
        {
            if (!ModelState.IsValid) return View(klantViewModel);

            try
            {
                var password = await _klantService.CreateKlantAsync(klantViewModel);
                TempData["successMessage"] = $"Klant {klantViewModel.Name} aangemaakt met wachtwoord: {password}";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                klantViewModel.allRanks = _klantService.GetAllKlantkaarten();
                return View(klantViewModel);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var klantViewModel = _klantService.GetKlantById(id);
            if (klantViewModel == null) return RedirectToAction("Index");

            return View(klantViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(KlantViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var success = await _klantService.UpdateKlantAsync(viewModel);
            if (!success) return RedirectToAction("Index");

            return RedirectToAction("Index");
        }
    }





}
