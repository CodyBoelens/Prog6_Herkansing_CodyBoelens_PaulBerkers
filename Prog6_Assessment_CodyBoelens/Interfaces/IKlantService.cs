using Prog6_Assessment_CodyBoelens.Data.DbEntities;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;

namespace Prog6_Assessment_CodyBoelens.Interfaces
{
    public interface IKlantService
    {
        List<KlantViewModel> GetKlantViewModels();
        List<Klantkaart> GetAllKlantkaarten();
        Task<string> CreateKlantAsync(KlantViewModel klantViewModel);
        KlantViewModel GetKlantById(int id);
        KlantViewModel GetKlantByApplicationUserId(string applicationUserId);
        Task<bool> UpdateKlantAsync(KlantViewModel viewModel);
    }
}
