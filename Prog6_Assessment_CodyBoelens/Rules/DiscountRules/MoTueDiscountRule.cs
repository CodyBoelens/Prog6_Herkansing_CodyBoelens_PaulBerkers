using Prog6_Assessment_CodyBoelens.Interfaces;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;

namespace Prog6_Assessment_CodyBoelens.Rules.DiscountRules
{
    public class MoTueDiscountRule : IDiscountRule
    {
        //Een boeking op maandag of dinsdag ontvangt 15% korting.

        public int GetDiscount(KlantViewModels klant, List<BeestjeViewModels> selectedBeestjes, DateTime datum)
        {
            if (datum.DayOfWeek == DayOfWeek.Monday || datum.DayOfWeek == DayOfWeek.Tuesday)
            {
                return 15;
            }

            return 0;
        }
    }
}
