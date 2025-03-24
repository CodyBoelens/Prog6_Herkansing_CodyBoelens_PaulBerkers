using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;

namespace Prog6_Assessment_CodyBoelens.Views.ViewModels.BoekingsViewModel
{
    public class Step04ViewModel
    {
        public KlantViewModels Klant { get; set; }
        public List<BeestjeViewModels> Beestjes { get; set; }
        public DateTime Datum { get; set; }
    }
}
