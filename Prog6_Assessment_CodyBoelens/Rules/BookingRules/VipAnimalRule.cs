using Prog6_Assessment_CodyBoelens.Data.DbEntities;
using Prog6_Assessment_CodyBoelens.Interfaces;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;

namespace Prog6_Assessment_CodyBoelens.Rules.BookingRules
{
    public class VipAnimalRule : IBookingRule
    {
        //Klanten met een platina kaart mogen de VIP dieren boeken. 

        public string? ValidateRule(KlantViewModels klant, List<BeestjeViewModels> selectedBeestjes, DateTime datum)
        {
            if (selectedBeestjes.Any(b => b.Type == "VIP") && (klant.KlantkaartId != 3))
            {
                return "Alleen klanten met een Platina kaart kunnen VIP beestjes boeken";
            }
            return null;
        }
    }
}
