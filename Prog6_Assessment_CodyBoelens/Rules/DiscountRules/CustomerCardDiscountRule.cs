using Prog6_Assessment_CodyBoelens.Interfaces;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;

namespace Prog6_Assessment_CodyBoelens.Rules.DiscountRules
{
    public class CustomerCardDiscountRule : IDiscountRule
    {
        //Klanten met een klantenkaart krijgen 10% korting.

        public int GetDiscount(KlantViewModels klant, List<BeestjeViewModels> selectedBeestjes, DateTime datum)
        { 
            if (klant.KlantkaartId != 0)
            {
                return 10;
            }

            return 0;
        }
    }
}
