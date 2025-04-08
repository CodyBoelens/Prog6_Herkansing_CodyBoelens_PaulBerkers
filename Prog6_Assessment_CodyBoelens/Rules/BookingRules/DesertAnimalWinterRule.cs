using Prog6_Assessment_CodyBoelens.Interfaces;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;

namespace Prog6_Assessment_CodyBoelens.Rules.BookingRules
{
    public class DesertAnimalWinterRule : IBookingRule
    {
        //Je mag geen beestje boeken van het type ‘Woestijn’ in de maanden oktober t/m februari

        public string? ValidateRule(KlantViewModels klant, List<BeestjeViewModels> selectedBeestjes, DateTime datum)
        {
            if (selectedBeestjes.Any(b => b.Type == "Woestijn") && (datum.Month >= 10 || datum.Month <= 2))
            {
                return "Woestijndieren kunnen niet geboekt worden in oktober t/m februari. 'Brrrr – Veel te koud'";
            }
            return null;
        }
    }
}
