using Prog6_Assessment_CodyBoelens.Data.DbEntities;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;

namespace Prog6_Assessment_CodyBoelens.Interfaces
{
    public interface IKortingService
    {
        int GetTotalDiscountPercentage(KlantViewModels klant, List<BeestjeViewModels> selectedBeestjes, DateTime datum);
    }

}
