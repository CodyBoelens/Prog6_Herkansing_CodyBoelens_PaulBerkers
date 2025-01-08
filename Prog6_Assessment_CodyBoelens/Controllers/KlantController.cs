using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prog6_Assessment_CodyBoelens.Data;
using Prog6_Assessment_CodyBoelens.Data.DbEntities;
using Prog6_Assessment_CodyBoelens.Data.Migrations;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;
using System.Security.Cryptography;
using System.Text;

namespace Prog6_Assessment_CodyBoelens.Controllers
{
    [Authorize(Roles = "Boerderij")]
    public class KlantController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public KlantController(ApplicationDbContext appDbContext, UserManager<IdentityUser> userManager)
        {
            _context = appDbContext;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var klanten = _context.Klanten.ToList();
            List<KlantViewModel> klantList = new();
            if (klanten != null)
            {
                foreach (var klant in klanten)
                {
                    var klantkaart = _context.Klantkaarten
                        .Where(k => k.Id == klant.KlantkaartId)
                        .Select(k => k.Rank).FirstOrDefault();

                    if (klantkaart == null)
                    {
                        klantkaart = "Geen Klantkaart";
                    }

                    var KlantViewModel = new KlantViewModel()
                    {
                        Id = klant.Id,
                        Name = klant.Name,
                        Adres = klant.Adres,
                        Rank = klantkaart

                    };

                    klantList.Add(KlantViewModel);
                }
                return View(klantList);
            }
            return View("Index", klantList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var allKlantRanks = _context.Klantkaarten.ToList();

            var klantViewModel = new KlantViewModel()
            {
                allRanks = allKlantRanks
            };

            return View(klantViewModel);
        }

        [HttpPost]
        public IActionResult Create(KlantViewModel klantViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string password = GeneratePassword();

                    var klantUser = new IdentityUser
                    {
                        Email = klantViewModel.Email,
                        UserName = klantViewModel.Email,
                        PhoneNumber = klantViewModel.PhoneNumber,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                    };

                    var result = _userManager.CreateAsync(klantUser, password).Result;

                    if (result.Succeeded)
                    {
                        if (klantViewModel.KlantkaartId == null)
                        {
                            klantViewModel.KlantkaartId = 0;
                        }
                        var klant = new Klant()
                        {
                            Name = klantViewModel.Name,
                            Adres = klantViewModel.Adres,
                            KlantkaartId = (int)klantViewModel.KlantkaartId,
                            ApplicationUserId = klantUser.Id
                        };

                        _context.Add(klant);
                        _context.SaveChanges();
                        _userManager.AddToRoleAsync(klantUser, "Klant").Wait();
                        TempData["successMessage"] = $"Klant {klant.Name} aangemaakt met het volgende wachtwoord: {password}";

                        return RedirectToAction("Index");

                    }
                    else
                    {
                        var errors = result.Errors;

                        foreach (var error in errors)
                        {
                            if (error.Code == "DuplicateEmail")
                            {
                                // Handle the case where the email is already in use
                                ModelState.AddModelError("", "The email address is already in use.");
                                // You may choose to redirect to a specific view or take other actions.
                            }
                            else
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }

                var allKlantRanks = _context.Klantkaarten.ToList();

                var viewModel = new KlantViewModel()
                {
                    allRanks = allKlantRanks
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                var allKlantRanks = _context.Klantkaarten.ToList();

                var viewmodel = new KlantViewModel
                {
                    allRanks = allKlantRanks
                };

                return View("Create",viewmodel);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var klant = _context.Klanten.SingleOrDefault(b => b.Id == id);
            var klantkaarten = _context.Klantkaarten.ToList();
            var userEmployee = _userManager.FindByIdAsync(klant.ApplicationUserId);
            var userEmail = userEmployee.Result.Email;
            var userPhoneNumber = userEmployee.Result.PhoneNumber;
            if (klant != null)
            {
                var viewModel = new KlantViewModel()
                {
                    Id = id,
                    Name = klant.Name,
                    Adres = klant.Adres,
                    Email = userEmail,
                    PhoneNumber = userPhoneNumber,
                    KlantkaartId = klant.KlantkaartId,
                    allRanks = klantkaarten
                };
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Index");
            }
            
        }

        [HttpPost]
        public IActionResult Edit(KlantViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var klant = _context.Klanten.SingleOrDefault(b => b.Id == viewModel.Id);
                if (viewModel.KlantkaartId == null)
                {
                    viewModel.KlantkaartId = 0;
                }
                klant.Name = viewModel.Name;
                klant.Adres = viewModel.Adres;
                klant.KlantkaartId = (int)viewModel.KlantkaartId;

                _context.Update(klant);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            var allKlantRanks = _context.Klantkaarten.ToList();

            var klantViewModel = new KlantViewModel()
            {
                allRanks = allKlantRanks
            };

            return View(klantViewModel);
        }



        public static string GeneratePassword(int length = 6)
        {
            string UppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string LowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
            string Numbers = "0123456789";
            string SpecialCharacters = "!@#$%^&*()-_=+[]{}|;:'\",.<>/?";
            // Make sure the length is at least 6
            length = Math.Max(length, 6);

            // Create a character pool based on the requirements
            string charPool = UppercaseLetters + LowercaseLetters + Numbers + SpecialCharacters;

            // Use RNGCryptoServiceProvider for secure random generation
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                // Ensure at least one uppercase letter, one number, and one special character
                StringBuilder password = new StringBuilder();
                password.Append(GetRandomCharacter(rng, UppercaseLetters));
                password.Append(GetRandomCharacter(rng, Numbers));
                password.Append(GetRandomCharacter(rng, SpecialCharacters));
                password.Append(GetRandomCharacter(rng, LowercaseLetters));

                // Fill the rest of the password with random characters
                for (int i = 3; i < length; i++)
                {
                    password.Append(charPool[GetRandomIndex(rng, charPool.Length)]);
                }

                // Convert the StringBuilder to a string
                return password.ToString();
            }
        }

        private static char GetRandomCharacter(RNGCryptoServiceProvider rng, string charPool)
        {
            // Get a random character from the specified character pool
            int index = GetRandomIndex(rng, charPool.Length);
            return charPool[index];
        }

        private static int GetRandomIndex(RNGCryptoServiceProvider rng, int max)
        {
            // Buffer storage for a random number
            byte[] data = new byte[4];

            // Generate a random number within the range of the character pool
            rng.GetBytes(data);
            int value = BitConverter.ToInt32(data, 0);
            return Math.Abs(value % max);
        }
    }
}
