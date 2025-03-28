using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;

namespace Prog6_Assessment_CodyBoelens.Views.ViewModels.BoekingsViewModel
{
    public class BoekingDetailsViewModel
    {
        public BoekingViewModel Boeking { get; set; }
        public List<BeestjeViewModels> BoekedBeestjes { get; set; }
    }
}
