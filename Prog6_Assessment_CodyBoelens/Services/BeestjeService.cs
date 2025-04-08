using Microsoft.EntityFrameworkCore;
using Prog6_Assessment_CodyBoelens.Data;
using Prog6_Assessment_CodyBoelens.Data.DbEntities;
using Prog6_Assessment_CodyBoelens.Interfaces;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prog6_Assessment_CodyBoelens.Services
{

    public class BeestjeService : IBeestjeService
    {
        private readonly ApplicationDbContext _context;

        public BeestjeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<BeestjeViewModels>> GetAllBeestjesAsync()
        {
            var beestjes = await _context.Beestjes.ToListAsync();

            var beestjeList = new List<BeestjeViewModels>();
            foreach (var beestje in beestjes)
            {
                var beestjeType = await _context.Types
                    .Where(type => type.Id == beestje.TypeId)
                    .Select(type => type.TypeName)
                    .FirstOrDefaultAsync();

                beestjeList.Add(new BeestjeViewModels
                {
                    Id = beestje.Id,
                    Name = beestje.Name,
                    Type = beestjeType,
                    Price = beestje.Price,
                    Picture = beestje.Picture
                });
            }

            return beestjeList;
        }

        public async Task<List<BeestjeViewModels>> GetAvailableBeestjesAsync(DateTime date)
        {
            // Get Beestje IDs that are booked on the given date
            var bookedBeestjeIds = await _context.BeestjeBoekingen
                .Join(_context.Boekingen,
                      bb => bb.BoekingID,
                      b => b.Id,
                      (bb, b) => new { bb.BeestjeID, b.Date })
                .Where(x => x.Date.Date == date.Date)
                .Select(x => x.BeestjeID)
                .Distinct()
                .ToListAsync();

            // Get all Beestjes that are NOT in the booked list
            var availableBeestjes = await _context.Beestjes
                .Where(b => !bookedBeestjeIds.Contains(b.Id))
                .ToListAsync();

            var beestjeList = new List<BeestjeViewModels>();
            foreach (var beestje in availableBeestjes)
            {
                var beestjeType = await _context.Types
                    .Where(type => type.Id == beestje.TypeId)
                    .Select(type => type.TypeName)
                    .FirstOrDefaultAsync();

                beestjeList.Add(new BeestjeViewModels
                {
                    Id = beestje.Id,
                    Name = beestje.Name,
                    Type = beestjeType,
                    Price = beestje.Price,
                    Picture = beestje.Picture
                });
            }

            return beestjeList;
        }

        public async Task<BeestjeViewModels> GetBeestjeByIdAsync(int id)
        {
            var beestje = await _context.Beestjes.SingleOrDefaultAsync(b => b.Id == id);
            if (beestje == null) return null;

            var beestjeType = await _context.Types
                .Where(type => type.Id == beestje.TypeId)
                .Select(type => type.TypeName)
                .FirstOrDefaultAsync();

            return new BeestjeViewModels
            {
                Id = beestje.Id,
                Name = beestje.Name,
                TypeId = beestje.TypeId,
                Type = beestjeType,
                Price = beestje.Price,
                Picture = beestje.Picture,
                allTypes = await _context.Types.ToListAsync()
            };
        }

        public async Task<bool> CreateBeestjeAsync(BeestjeViewModels viewModel)
        {
            try
            {
                var beestje = new Beestje
                {
                    Name = viewModel.Name,
                    Price = viewModel.Price,
                    TypeId = viewModel.TypeId,
                    Picture = viewModel.Picture
                };

                await _context.Beestjes.AddAsync(beestje);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateBeestjeAsync(BeestjeViewModels viewModel)
        {
            try
            {
                var beestje = await _context.Beestjes.SingleOrDefaultAsync(b => b.Id == viewModel.Id);
                if (beestje == null) return false;

                beestje.Name = viewModel.Name;
                beestje.Price = viewModel.Price;
                beestje.TypeId = viewModel.TypeId;
                beestje.Picture = viewModel.Picture;

                _context.Beestjes.Update(beestje);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteBeestjeAsync(int id)
        {
            try
            {
                var beestje = await _context.Beestjes.SingleOrDefaultAsync(b => b.Id == id);
                if (beestje == null) return false;

                _context.Beestjes.Remove(beestje);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Types>> GetAllTypesAsync()
        {
            return await _context.Types.ToListAsync();
        }

        public async Task<List<String>> GetAllImageNamesAsync()
        {
            string imagesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/beestjes");
            List<string> imageNameList = new List<string>();

            if (Directory.Exists(imagesDirectory))
            {
                var imageNames = Directory.GetFiles(imagesDirectory)
                                           .Select(Path.GetFileName)
                                           .ToList();

                imageNameList.AddRange(imageNames);
            }

            return imageNameList;
        }

        public async Task<List<BeestjeViewModels>> GetBeestjesByIdsAsync(List<int> ids)
        {
            var beestjes = await _context.Beestjes
                .Where(b => ids.Contains(b.Id))
                .ToListAsync();

            var beestjeList = new List<BeestjeViewModels>();
            foreach (var beestje in beestjes)
            {
                var beestjeType = await _context.Types
                    .Where(type => type.Id == beestje.TypeId)
                    .Select(type => type.TypeName)
                    .FirstOrDefaultAsync();

                beestjeList.Add(new BeestjeViewModels
                {
                    Id = beestje.Id,
                    Name = beestje.Name,
                    Type = beestjeType,
                    Price = beestje.Price,
                    Picture = beestje.Picture
                });
            }

            return beestjeList;
        }

    }
}
