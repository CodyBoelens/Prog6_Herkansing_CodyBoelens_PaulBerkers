using Prog6_Assessment_CodyBoelens.Interfaces;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;

namespace Prog6_Assessment_CodyBoelens.Rules.DiscountRules
{
    public class LettersInNameDiscountRule : IDiscountRule
    {
        //Een boeking met daarin een beestje met in de naam de letter ‘A’ krijgt 2% extra korting.
        //      Als er ook een letter B in zit krijgt hij 2% korting extra;
        //      Als er dan ook nog een letter C in zit weer 2% extra;
        //      Etc.

        public int GetDiscount(KlantViewModels klant, List<BeestjeViewModels> selectedBeestjes, DateTime datum)
        {
            int korting = 0;

            foreach (var beestje in selectedBeestjes)
            {
                string naam = beestje.Name.ToUpper();
                var letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

                // Kijk voor opeenvolgende letters
                foreach (char letter in letters)
                {
                    if (naam.Contains(letter))
                    {
                        korting += 2;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return korting;
        }
    }
}
