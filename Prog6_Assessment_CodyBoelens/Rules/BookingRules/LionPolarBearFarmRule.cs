using Prog6_Assessment_CodyBoelens.Interfaces;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;

namespace Prog6_Assessment_CodyBoelens.Rules.BookingRules
{
    public class LionPolarBearFarmRule : IBookingRule
    {
        //Je mag geen beestje boeken met het type ‘Leeuw’ of ‘IJsbeer’ als je ook een beestje boekt van het type ‘Boerderijdier’

        public string? ValidateRule(KlantViewModels klant, List<BeestjeViewModels> selectedBeestjes, DateTime datum)
        {
            if (selectedBeestjes.Any(b => b.Name.ToLower() == "leeuw" || b.Name.ToLower() == "ijsbeer"))
            {
                if(selectedBeestjes.Any(b => b.Type == "Boerderij"))
                {
                    return "Je kunt geen Leeuw of IJsbeer boeken samen met een Boerderijdier. 'Nom nom nom'";
                }
            }
            return null;
        }
    }
}
