using Prog6_Assessment_CodyBoelens.Data;
using Prog6_Assessment_CodyBoelens.Data.DbEntities;
using Prog6_Assessment_CodyBoelens.Interfaces;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BoekingsViewModel;

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
           

        }
    }
}
