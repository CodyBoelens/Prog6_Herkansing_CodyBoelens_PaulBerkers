using Prog6_Assessment_CodyBoelens.Data.DbEntities;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;

namespace Prog6_Assessment_CodyBoelens.Interfaces
{
    public interface IKlantService
    {
        List<KlantViewModels> GetKlantViewModels();
        List<Klantkaart> GetAllKlantkaarten();
        Task<string> CreateKlantAsync(KlantViewModels klantViewModel);
        KlantViewModels GetKlantById(int id);
        KlantViewModels GetKlantByApplicationUserId(string applicationUserId);
        Task<bool> UpdateKlantAsync(KlantViewModels viewModel);
    }
}
