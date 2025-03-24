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

            // Retrieve the beestjesList along with their Types using a join
            var beestjesList = await _context.Beestjes
                .Where(b => beestjesId.Contains(b.Id))
                .Join(_context.Types,
                      b => b.TypeId,
                      t => t.Id,
                      (b, t) => new { Beestje = b, Type = t })
                .ToListAsync();

            // Regel 1: Leeuw/IJsbeer niet samen met Boerderijdier
            if (beestjesList.Any(b => b.Beestje.Name == "Leeuw" || b.Beestje.Name == "IJsbeer") && beestjesList.Any(b => b.Type.TypeName == "Boerderij"))
            {
                errors.Add("Je kunt geen Leeuw of IJsbeer boeken samen met een Boerderijdier. 'Nom nom nom'");
            }

            // Regel 2: Pinguïn niet in het weekend
            if (beestjesList.Any(b => b.Beestje.Name == "Pinguin") && (datum.DayOfWeek == DayOfWeek.Saturday || datum.DayOfWeek == DayOfWeek.Sunday))
            {
                errors.Add("Pinguïns werken alleen doordeweeks. 'Dieren in pak werken alleen doordeweeks'");
            }

            // Regel 3: Woestijndieren niet van oktober t/m februari
            if (beestjesList.Any(b => b.Type.TypeName == "Woestijn") && (datum.Month >= 10 || datum.Month <= 2))
            {
                errors.Add("Woestijndieren kunnen niet geboekt worden in oktober t/m februari. 'Brrrr – Veel te koud'");
            }

            // Regel 4: Sneeuwdieren niet van juni t/m augustus
            if (beestjesList.Any(b => b.Type.TypeName == "Sneeuw") && (datum.Month >= 6 && datum.Month <= 8))
            {
                errors.Add("Sneeuwdieren kunnen niet geboekt worden in juni t/m augustus. 'Some People Are Worth Melting For ~ Olaf'");
            }

            // Regel 5: Maximaal aantal beestjes per klantenkaart type
            int maxDieren = klant.KlantkaartId switch
            {
                0 => 3,
                1 => 4,
                2 => int.MaxValue,
                3 => int.MaxValue, // Platina kan ook VIP dieren boeken
                _ => 3
            };

            if (beestjesId.Count > maxDieren)
            {
                errors.Add($"Je mag maximaal {maxDieren} dieren boeken met jouw klantenkaart.");
            }

            //Regel 6: Alleen klanten met een Platina kaart kunnen VIP Beestjes boeken
            if (beestjesList.Any(b => b.Type.TypeName == "VIP") && (klant.KlantkaartId != 3))
            {
                errors.Add("Alleen klanten met een Platina kaart kunnen VIP beestjes boeken");
            }

            return errors;
        }

    }
}
