using Microsoft.EntityFrameworkCore;
using Prog6_Assessment_CodyBoelens.Data;
using Prog6_Assessment_CodyBoelens.Data.DbEntities;
using Prog6_Assessment_CodyBoelens.Interfaces;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BoekingsViewModel;
using Prog6_Assessment_CodyBoelens.Models;

namespace Prog6_Assessment_CodyBoelens.Services
{
    public class BoekingService : IBoekingService
    {

        private readonly ApplicationDbContext _context;

        public BoekingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<BoekingViewModel>> GetAllBoekingsAsync()
        {
            return await _context.Boekingen
                .Select(boeking => new BoekingViewModel
                {
                    Id = boeking.Id,
                    Date = boeking.Date,
                    Name = boeking.Name,
                    Email = boeking.Email
                })
                .ToListAsync();
        }

        public async Task<BoekingViewModel> GetBookingByIdAsync(int id)
        {
            var boeking = await _context.Boekingen
                .Where(b => b.Id == id)
                .Select(b => new BoekingViewModel
                {
                    Id = b.Id,
                    Date = b.Date,
                    Name = b.Name,
                    Adress = b.Adress,
                    PhoneNumber = b.PhoneNumber,
                    Email = b.Email
                })
                .FirstOrDefaultAsync();

            return boeking;
        }

        public async Task<List<BeestjeViewModels>> GetBookedBeestjesByIdAsync(int id)
        {
            return await _context.BeestjeBoekingen
                .Where(bb => bb.BoekingID == id)
                .Select(bb => new BeestjeViewModels
                {
                    Id = bb.Beestje.Id,
                    Name = bb.Beestje.Name,
                    Price = bb.Beestje.Price,
                    Picture = bb.Beestje.Picture,
                    Type = bb.Beestje.Type.TypeName
                })
                .ToListAsync();
        }

        public async Task<bool> DeleteBoekingAsync(int id)
        {
            try
            {
                var boeking = await _context.Boekingen.SingleOrDefaultAsync(b => b.Id == id);
                if (boeking == null) return false;

                _context.Boekingen.Remove(boeking);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task AddBoekingAsync(BoekingViewModel boekingViewModel)
        {
            var boeking = new Boeking
            {
                Date = boekingViewModel.Date,
                Name = boekingViewModel.Name,
                Adress = boekingViewModel.Adress,
                PhoneNumber = boekingViewModel.PhoneNumber,
                Email = boekingViewModel.Email,
                Is_Confirmed = boekingViewModel.Is_Confirmed,
                KlantId = boekingViewModel.KlantId 
            };

            _context.Boekingen.Add(boeking);
            await _context.SaveChangesAsync();

            if (boekingViewModel.BeestjeIds != null && boekingViewModel.BeestjeIds.Any())
            {
                foreach (var beestjeId in boekingViewModel.BeestjeIds)
                {
                    var beestjeBoeking = new BeestjeBoeking
                    {
                        BoekingID = boeking.Id,
                        BeestjeID = beestjeId
                    };
                    _context.BeestjeBoekingen.Add(beestjeBoeking);
                }
                await _context.SaveChangesAsync();
            }

        }

        public async Task<List<string>> BoekingValidation(Step03ViewModel viewModel)
        {
            List<string> errors = new List<string>();

            var beestjesId = viewModel.SelectedBeestjesIds;
            var klant = viewModel.Klant;
            var datum = viewModel.Datum;

            // Haal de beestjes met hun types op
            var beestjesRawList = await _context.Beestjes
            .Where(b => beestjesId.Contains(b.Id))
            .Join(_context.Types,
                  b => b.TypeId,
                  t => t.Id,
                  (b, t) => new { Beestje = b, Type = t })
            .ToListAsync(); // 🚀 Data is nu in memory

            // Stap 2: Gebruik LINQ in memory
            var beestjesList = beestjesRawList
                .Select(b => (Beestje: b.Beestje, Type: b.Type)) // 🚀 Nu als tuple
                .ToList();

            // **Alle validatieregels toepassen**
            var validations = new List<string?>
            {
                CheckLeeuwIjsbeerWithBoerderijdier(beestjesList),
                CheckPinguinNietInWeekend(beestjesList, datum),
                CheckWoestijndierenNietWinter(beestjesList, datum),
                CheckSneeuwdierenNietZomer(beestjesList, datum),
                CheckMaxDierenPerKlant(beestjesId, klant.KlantkaartId),
                CheckVIPAlleenPlatina(beestjesList, klant.KlantkaartId)
            };

            // Alleen niet-lege foutmeldingen toevoegen
            errors.AddRange(validations.Where(error => !string.IsNullOrEmpty(error)));

            return errors.Any() ? errors : null;
        }


        public string CheckLeeuwIjsbeerWithBoerderijdier(List<(Beestje Beestje, Types Type)> beestjesList)
        {
            if (beestjesList.Any(b => b.Beestje.Name == "Leeuw" || b.Beestje.Name == "IJsbeer") &&
                beestjesList.Any(b => b.Type.TypeName == "Boerderij"))
            {
                return "Je kunt geen Leeuw of IJsbeer boeken samen met een Boerderijdier.";
            }
            return null;
        }

        public string CheckPinguinNietInWeekend(List<(Beestje Beestje, Types Type)> beestjesList, DateTime datum)
        {
            if (beestjesList.Any(b => b.Beestje.Name == "Pinguin") &&
                (datum.DayOfWeek == DayOfWeek.Saturday || datum.DayOfWeek == DayOfWeek.Sunday))
            {
                return "Pinguïns werken alleen doordeweeks. 'Dieren in pak werken alleen doordeweeks'";
            }
            return null;
        }

        public string CheckWoestijndierenNietWinter(List<(Beestje Beestje, Types Type)> beestjesList, DateTime datum)
        {
            if (beestjesList.Any(b => b.Type.TypeName == "Woestijn") && (datum.Month >= 10 || datum.Month <= 2))
            {
                return "Woestijndieren kunnen niet geboekt worden in oktober t/m februari. 'Brrrr – Veel te koud'";
            }
            return null;
        }

        public string CheckSneeuwdierenNietZomer(List<(Beestje Beestje, Types Type)> beestjesList, DateTime datum)
        {
            if (beestjesList.Any(b => b.Type.TypeName == "Sneeuw") && (datum.Month >= 6 && datum.Month <= 8))
            {
                return "Sneeuwdieren kunnen niet geboekt worden in juni t/m augustus. 'Some People Are Worth Melting For ~ Olaf'";
            }
            return null;
        }


        public string CheckMaxDierenPerKlant(List<int> beestjesId, int? klantkaartId)
        {
            int maxDieren = klantkaartId switch
            {
                0 => 3,
                1 => 4,
                2 => int.MaxValue,
                3 => int.MaxValue, // Platina kan ook VIP dieren boeken
                _ => 3
            };

            if (beestjesId.Count > maxDieren)
            {
                return $"Je mag maximaal {maxDieren} dieren boeken met jouw klantenkaart.";
            }
            return null;
        }

        public string CheckVIPAlleenPlatina(List<(Beestje Beestje, Types Type)> beestjesList, int? klantkaartId)
        {
            if (beestjesList.Any(b => b.Type.TypeName == "VIP") && (klantkaartId != 3))
            {
                return "Alleen klanten met een Platina kaart kunnen VIP beestjes boeken";
            }
            return null;
        }


    }
}
