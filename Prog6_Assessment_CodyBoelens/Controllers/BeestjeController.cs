using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prog6_Assessment_CodyBoelens.Services;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using System.Threading.Tasks;

namespace Prog6_Assessment_CodyBoelens.Controllers
{
    [Authorize(Roles = "Boerderij")]
    public class BeestjeController : Controller
    {
        private readonly IBeestjeService _beestjeService;

        public BeestjeController(IBeestjeService beestjeService)
        {
            _beestjeService = beestjeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var beestjes = await _beestjeService.GetAllBeestjesAsync();
            return View(beestjes);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var beestjeViewModel = new BeestjeViewModel
            {
                allTypes = await _beestjeService.GetAllTypesAsync(),
                allImageNames = await _beestjeService.GetAllImageNamesAsync()
            };

            return View(beestjeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BeestjeViewModel beestjeViewModel)
        {
            if (!ModelState.IsValid)
            {
                beestjeViewModel.allTypes = await _beestjeService.GetAllTypesAsync();
                beestjeViewModel.allImageNames = await _beestjeService.GetAllImageNamesAsync();
                TempData["errorMessage"] = "Vul de juiste gegevens in.";
                return View(beestjeViewModel);
            }

            try
            {
                await _beestjeService.CreateBeestjeAsync(beestjeViewModel);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                beestjeViewModel.allTypes = await _beestjeService.GetAllTypesAsync();
                beestjeViewModel.allImageNames = await _beestjeService.GetAllImageNamesAsync();
                return View(beestjeViewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var beestjeViewModel = await _beestjeService.GetBeestjeByIdAsync(id);
            if (beestjeViewModel == null)
            {
                return RedirectToAction("Index");
            }

            beestjeViewModel.allTypes = await _beestjeService.GetAllTypesAsync();
            beestjeViewModel.allImageNames = await _beestjeService.GetAllImageNamesAsync();
            return View(beestjeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BeestjeViewModel beestjeViewModel)
        {
            if (!ModelState.IsValid)
            {
                beestjeViewModel.allTypes = await _beestjeService.GetAllTypesAsync();
                beestjeViewModel.allImageNames = await _beestjeService.GetAllImageNamesAsync();
                TempData["errorMessage"] = "Vul de juiste gegevens in.";
                return View(beestjeViewModel);
            }

            try
            {
                await _beestjeService.UpdateBeestjeAsync(beestjeViewModel);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                beestjeViewModel.allTypes = await _beestjeService.GetAllTypesAsync();
                beestjeViewModel.allImageNames = await _beestjeService.GetAllImageNamesAsync();
                return View(beestjeViewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var beestjeViewModel = await _beestjeService.GetBeestjeByIdAsync(id);
            if (beestjeViewModel == null)
            {
                return RedirectToAction("Index");
            }

            return View(beestjeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(BeestjeViewModel beestjeViewModel)
        {
            try
            {
                await _beestjeService.DeleteBeestjeAsync(beestjeViewModel.Id);
                TempData["successMessage"] = $"Beestje {beestjeViewModel.Name} is verwijderd.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(beestjeViewModel);
            }
        }
    }
}
