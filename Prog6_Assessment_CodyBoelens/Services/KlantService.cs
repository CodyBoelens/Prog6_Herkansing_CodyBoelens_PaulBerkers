using Microsoft.AspNetCore.Identity;
using Prog6_Assessment_CodyBoelens.Controllers;
using Prog6_Assessment_CodyBoelens.Data;
using Prog6_Assessment_CodyBoelens.Data.DbEntities;
using Prog6_Assessment_CodyBoelens.Interfaces;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;
using System.Security.Cryptography;
using System.Text;

namespace Prog6_Assessment_CodyBoelens.Services
{
    

    public class KlantService : IKlantService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public KlantService(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<KlantViewModels> GetKlantViewModels()
        {
            var klanten = _context.Klanten.ToList();
            var klantList = new List<KlantViewModels>();

            foreach (var klant in klanten)
            {
                var klantkaart = _context.Klantkaarten
                    .Where(k => k.Id == klant.KlantkaartId)
                    .Select(k => k.Rank)
                    .FirstOrDefault() ?? "Geen Klantkaart";

                klantList.Add(new KlantViewModels
                {
                    Id = klant.Id,
                    Name = klant.Name,
                    Adres = klant.Adres,
                    Rank = klantkaart
                });
            }

            return klantList;
        }

        public List<Klantkaart> GetAllKlantkaarten()
        {
            return _context.Klantkaarten.ToList();
        }

        public async Task<string> CreateKlantAsync(KlantViewModels klantViewModel)
        {
            var password = GeneratePassword();

            var klantUser = new IdentityUser
            {
                Email = klantViewModel.Email,
                UserName = klantViewModel.Email,
                PhoneNumber = klantViewModel.PhoneNumber,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            var result = await _userManager.CreateAsync(klantUser, password);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            var klant = new Klant
            {
                Name = klantViewModel.Name,
                Adres = klantViewModel.Adres,
                KlantkaartId = klantViewModel.KlantkaartId ?? 0,
                ApplicationUserId = klantUser.Id
            };

            _context.Add(klant);
            await _context.SaveChangesAsync();
            await _userManager.AddToRoleAsync(klantUser, "Klant");

            return password;
        }

        public KlantViewModels GetKlantById(int id)
        {
            var klant = _context.Klanten.SingleOrDefault(b => b.Id == id);
            var klantkaarten = _context.Klantkaarten.ToList();

            if (klant == null) return null;

            var user = _userManager.FindByIdAsync(klant.ApplicationUserId).Result;

            return new KlantViewModels
            {
                Id = id,
                Name = klant.Name,
                Adres = klant.Adres,  
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                KlantkaartId = klant.KlantkaartId,
                allRanks = klantkaarten
            };
        }

        public KlantViewModels GetKlantByApplicationUserId(string applicationUserId)
        {
            // Find the klant using the ApplicationUserId
            var klant = _context.Klanten.SingleOrDefault(b => b.ApplicationUserId == applicationUserId);
            var klantkaarten = _context.Klantkaarten.ToList();

            if (klant == null) return null;

            // Retrieve the associated user using the UserManager
            var user = _userManager.FindByIdAsync(applicationUserId).Result;

            return new KlantViewModels
            {
                Id = klant.Id,
                Name = klant.Name,
                Adres = klant.Adres,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                KlantkaartId = klant.KlantkaartId,
                allRanks = klantkaarten
            };
        }


        public async Task<bool> UpdateKlantAsync(KlantViewModels viewModel)
        {
            var klant = _context.Klanten.SingleOrDefault(b => b.Id == viewModel.Id);
            if (klant == null) return false;

            klant.Name = viewModel.Name;
            klant.Adres = viewModel.Adres;
            klant.KlantkaartId = viewModel.KlantkaartId ?? 0;

            _context.Update(klant);
            await _context.SaveChangesAsync();

            return true;
        }

        public static string GeneratePassword(int length = 6)
        {
            string UppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string LowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
            string Numbers = "0123456789";
            string SpecialCharacters = "!@#$%^&*()-_=+[]{}|;:'\",.<>/?";

            length = Math.Max(length, 6);

            string charPool = UppercaseLetters + LowercaseLetters + Numbers + SpecialCharacters;

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                StringBuilder password = new StringBuilder();
                password.Append(GetRandomCharacter(rng, UppercaseLetters));
                password.Append(GetRandomCharacter(rng, Numbers));
                password.Append(GetRandomCharacter(rng, SpecialCharacters));
                password.Append(GetRandomCharacter(rng, LowercaseLetters));

                for (int i = 3; i < length; i++)
                {
                    password.Append(charPool[GetRandomIndex(rng, charPool.Length)]);
                }

                return password.ToString();
            }
        }

        private static char GetRandomCharacter(RNGCryptoServiceProvider rng, string charPool)
        {
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
