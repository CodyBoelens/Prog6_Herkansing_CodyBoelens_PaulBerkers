using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;

namespace Prog6_Assessment_CodyBoelens.Interfaces
{
    public interface IBookingRule
    {
        string? ValidateRule(KlantViewModels klant, List<BeestjeViewModels> selectedBeestjes, DateTime datum);
    }
}
