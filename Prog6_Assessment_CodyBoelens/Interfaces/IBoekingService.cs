
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BoekingsViewModel;

namespace Prog6_Assessment_CodyBoelens.Interfaces
{
    public interface IBoekingService
    {
        Task AddBoekingAsync(BoekingViewModel boekingViewModel);
    }
}
