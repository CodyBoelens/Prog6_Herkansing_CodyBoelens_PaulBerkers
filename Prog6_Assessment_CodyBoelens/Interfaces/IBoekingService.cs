
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BoekingsViewModel;

namespace Prog6_Assessment_CodyBoelens.Interfaces
{
    public interface IBoekingService
    {
        Task AddBoekingAsync(BoekingViewModel boekingViewModel);
        Task<List<string>> BoekingValidation(Step03ViewModel viewModel);
    }
}
