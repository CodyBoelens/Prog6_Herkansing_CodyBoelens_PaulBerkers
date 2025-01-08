using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prog6_Assessment_CodyBoelens.Data;
using Prog6_Assessment_CodyBoelens.Data.DbEntities;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;

namespace Prog6_Assessment_CodyBoelens.Controllers
{
    [Authorize(Roles = "Boerderij")]
    public class BeestjeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BeestjeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var beestjes = _context.Beestjes.ToList();
            List<BeestjeViewModel> beestjeList = new();
            if (beestjes != null)
            {
                foreach (var beestje in beestjes)
                {
                    var beestjeType = _context.Types
                        .Where(type => type.Id == beestje.TypeId)
                        .Select(type => type.TypeName).FirstOrDefault();

                    var BeestjeViewModel = new BeestjeViewModel()
                    {
                        Id = beestje.Id,
                        Name = beestje.Name,
                        Type = beestjeType,
                        Price = beestje.Price,
                        Picture = beestje.Picture,
                    };

                    beestjeList.Add(BeestjeViewModel);
                }
                return View(beestjeList);
            }
            return View("Index", beestjeList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var allBeestjeTypes = _context.Types.ToList();

            var beestjeviewmodel = new BeestjeViewModel()
            {
                allTypes = allBeestjeTypes
            };

            return View(beestjeviewmodel);
        }

        [HttpPost]
        public IActionResult Create(BeestjeViewModel beestjeViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var allBeestjeTypes = _context.Types.ToList();
                    var beestje = new Beestje()
                    {
                        Name = beestjeViewModel.Name,
                        Price = beestjeViewModel.Price,
                        TypeId = beestjeViewModel.TypeId,
                        Picture = beestjeViewModel.Picture,
                    };

                    _context.Add(beestje);
                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    var types = _context.Types.ToList();

                    var errors1 = ModelState.Values.SelectMany(v => v.Errors)
                      .Select(e => e.ErrorMessage)
                      .ToList();

                    var viewmodel = new BeestjeViewModel
                    {
                        allTypes = types
                    };
                    TempData["errorMessage"] = "Vul de juiste gegevens in.";
                    return View("Create", viewmodel);
                }
            }
            catch (Exception ex)
            {
                var types = _context.Types.ToList();

                var viewmodel = new BeestjeViewModel
                {
                    allTypes = types
                };
                TempData["errorMessage"] = ex.Message;
                return View("Create", viewmodel);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var beestje = _context.Beestjes.SingleOrDefault(b => b.Id == id);
            var types = _context.Types.ToList();

            if (beestje != null)
            {
                var viewModel = new BeestjeViewModel
                {
                    Id = beestje.Id,
                    Name = beestje.Name,
                    Price = beestje.Price,
                    TypeId = beestje.TypeId,
                    Picture = beestje.Picture,
                    allTypes = types
                };

                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Edit(BeestjeViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var allBeestjeTypes = _context.Types.ToList();
                    var beestje = new Beestje()
                    {
                        Id = viewModel.Id,
                        Name = viewModel.Name,
                        Price = viewModel.Price,
                        TypeId = viewModel.TypeId,
                        Picture = viewModel.Picture,
                    };

                    _context.Update(beestje);
                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    var types = _context.Types.ToList();

                    var errors1 = ModelState.Values.SelectMany(v => v.Errors)
                      .Select(e => e.ErrorMessage)
                      .ToList();

                    var viewmodel = new BeestjeViewModel
                    {
                        allTypes = types
                    };
                    TempData["errorMessage"] = "Vul de juiste gegevens in.";
                    return View("Create", viewmodel);
                }
            }
            catch (Exception ex)
            {
                var types = _context.Types.ToList();

                var viewmodel = new BeestjeViewModel
                {
                    allTypes = types
                };
                TempData["errorMessage"] = ex.Message;
                return View("Create", viewmodel);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var beestje = _context.Beestjes.SingleOrDefault(b => b.Id == id);
            var beestjeType = _context.Types
            .Where(type => type.Id == beestje.TypeId)
            .Select(type => type.TypeName)
            .FirstOrDefault();
            var types = _context.Types.ToList();

            if (beestje != null)
            {
                var viewModel = new BeestjeViewModel
                {
                    Id = beestje.Id,
                    Name = beestje.Name,
                    Price = beestje.Price,
                    Type = beestjeType,
                    Picture = beestje.Picture,
                    allTypes = types
                };

                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Delete(BeestjeViewModel viewModel)
        {
            try
            {
                var beestje = _context.Beestjes.SingleOrDefault(x => x.Id == viewModel.Id);

                if (beestje != null)
                {
                    _context.Beestjes.Remove(beestje);
                    _context.SaveChanges();
                    TempData["successMessage"] = $"Beestje {beestje.Name} has been deleted";

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["errorMessage"] = $"Beestje details not availabe with the Id: {viewModel.Id}";

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;

                return View();
            }
        }
    }
}
