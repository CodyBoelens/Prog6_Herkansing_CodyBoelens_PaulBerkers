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
        Task<List<string>> BoekingValidation(Step03ViewModel viewModel);
        string CheckLeeuwIjsbeerWithBoerderijdier(List<(Beestje Beestje, Types Type)> beestjesList);
        string CheckPinguinNietInWeekend(List<(Beestje Beestje, Types Type)> beestjesList, DateTime datum);
        string CheckWoestijndierenNietWinter(List<(Beestje Beestje, Types Type)> beestjesList, DateTime datum);
        string CheckSneeuwdierenNietZomer(List<(Beestje Beestje, Types Type)> beestjesList, DateTime datum);
        string CheckMaxDierenPerKlant(List<int> beestjesId, int? klantkaartId);
        string CheckVIPAlleenPlatina(List<(Beestje Beestje, Types Type)> beestjesList, int? klantkaartId);
    }
}
