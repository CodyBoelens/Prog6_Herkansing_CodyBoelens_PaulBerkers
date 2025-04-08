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
                    TotaalPrijs = b.TotaalPrijs,
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
                TotaalPrijs = boekingViewModel.TotaalPrijs,
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

    }
}
