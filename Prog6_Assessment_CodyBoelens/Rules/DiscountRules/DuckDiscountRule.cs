using Prog6_Assessment_CodyBoelens.Interfaces;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;
using System;

namespace Prog6_Assessment_CodyBoelens.Rules.DiscountRules
{
    public class DuckDiscountRule : IDiscountRule
    {
        //Een boeking met een beestje met de naam ‘Eend’ heeft een kans van 1 op 6, op 50% korting. 

        private readonly Random _random;

        public DuckDiscountRule(Random? random = null)
        {
            _random = random ?? new Random();
        }

        public int GetDiscount(KlantViewModels klant, List<BeestjeViewModels> selectedBeestjes, DateTime datum)
        {
            if (selectedBeestjes.Any(b => b.Name.ToUpper() == "EEND") && _random.Next(1, 7) == 1)
            {
                return 50; 
            }
            return 0;
        }
    }
}
