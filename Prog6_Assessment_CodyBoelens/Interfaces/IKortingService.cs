using Prog6_Assessment_CodyBoelens.Data.DbEntities;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;

namespace Prog6_Assessment_CodyBoelens.Interfaces
{
    public interface IKortingService
    {
        double BerekenKorting(KlantViewModels klant, double totaalPrijs, List<BeestjeViewModels> selectedBeestjes, DateTime datum);
        double KortingDrieDierenVanZelfdeType(double totaalPrijs, List<BeestjeViewModels> selectedBeestjes);
        double KortingEendKans(double totaalPrijs, List<BeestjeViewModels> selectedBeestjes);
        double KortingOpWeekdag(double totaalPrijs, DateTime datum);
        double KortingVoorLettersInNaam(List<BeestjeViewModels> selectedBeestjes);
        double KortingVoorKlantenkaart(double totaalPrijs, KlantViewModels klant);
    }

}
