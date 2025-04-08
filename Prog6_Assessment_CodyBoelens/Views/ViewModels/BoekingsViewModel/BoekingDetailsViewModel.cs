using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using System.Diagnostics.CodeAnalysis;

namespace Prog6_Assessment_CodyBoelens.Views.ViewModels.BoekingsViewModel
{
    [ExcludeFromCodeCoverage]
    public class BoekingDetailsViewModel
    {
        public BoekingViewModel Boeking { get; set; }
        public List<BeestjeViewModels> BoekedBeestjes { get; set; }
    }
}
