using Prog6_Assessment_CodyBoelens.Interfaces;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;

namespace Prog6_Assessment_CodyBoelens.Rules.BookingRules
{
    public class DonkeyCantBeAloneRule : IBookingRule
    {
        //Verzin zelf een leuke regel!

        public string? ValidateRule(KlantViewModels klant, List<BeestjeViewModels> selectedBeestjes, DateTime datum)
        {
            if (selectedBeestjes.Any(b => b.Name.ToLower() == "ezel")) 
            {
                if (selectedBeestjes.Count < 2) 
                {
                    return "Boek een ezel met een extra dier  'vriendschap verplicht!'";
                }
            }

            return null;
        }
    }
}
