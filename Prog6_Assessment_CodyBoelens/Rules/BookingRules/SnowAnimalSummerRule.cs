using Prog6_Assessment_CodyBoelens.Interfaces;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;

namespace Prog6_Assessment_CodyBoelens.Rules.BookingRules
{
    public class SnowAnimalSummerRule : IBookingRule
    {
        //Je mag geen beestje boeken van het type ‘Sneeuw’ in de maanden juni t/m augustus

        public string? ValidateRule(KlantViewModels klant, List<BeestjeViewModels> selectedBeestjes, DateTime datum)
        {
            if (selectedBeestjes.Any(b => b.Type == "Sneeuw") && (datum.Month >= 6 && datum.Month <= 8))
            {
                return "Sneeuwdieren kunnen niet geboekt worden in juni t/m augustus. 'Some People Are Worth Melting For ~ Olaf'";
            }
            return null;
        }
    }
}
