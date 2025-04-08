using Prog6_Assessment_CodyBoelens.Data.DbEntities;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BoekingsViewModel;

namespace Prog6_Assessment_CodyBoelens.Interfaces
{
    public interface IBoekingService
    {
        Task AddBoekingAsync(BoekingViewModel boekingViewModel);
        Task<List<BoekingViewModel>> GetAllBoekingsAsync();
        Task<BoekingViewModel> GetBookingByIdAsync(int id);
        Task<bool> DeleteBoekingAsync(int id);
        Task<List<BeestjeViewModels>> GetBookedBeestjesByIdAsync(int id);
    }
}
