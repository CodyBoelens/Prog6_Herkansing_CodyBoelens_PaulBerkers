using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;
using System.Diagnostics.CodeAnalysis;

namespace Prog6_Assessment_CodyBoelens.Views.ViewModels.BoekingsViewModel
{
    [ExcludeFromCodeCoverage]
    public class Step04ViewModel
    {
        public KlantViewModels Klant { get; set; }
        public List<BeestjeViewModels> Beestjes { get; set; }
        public DateTime Datum { get; set; }
        public double TotaalPrijs { get; set; }
        public double TotaalPrijsMetKorting { get; set; }
        public int Discount { get; set; }
    }
}
