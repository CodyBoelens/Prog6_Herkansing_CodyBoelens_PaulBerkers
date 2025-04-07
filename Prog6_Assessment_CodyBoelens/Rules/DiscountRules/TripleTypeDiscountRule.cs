using Prog6_Assessment_CodyBoelens.Interfaces;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;

namespace Prog6_Assessment_CodyBoelens.Rules.DiscountRules
{
    public class TripleTypeDiscountRule : IDiscountRule
    {
        //Een boeking met 3 dieren van het zelfde type krijgt 10% korting

        public int GetDiscount(KlantViewModels klant, List<BeestjeViewModels> selectedBeestjes, DateTime datum)
        {
            if (selectedBeestjes.GroupBy(b => b.Type).Any(g => g.Count() >= 3))
            {
                return 10;
            }

            return 0;
        }
    }
}
