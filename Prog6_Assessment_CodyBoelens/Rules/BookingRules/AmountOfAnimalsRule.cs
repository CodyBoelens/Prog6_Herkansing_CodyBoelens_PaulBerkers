using Prog6_Assessment_CodyBoelens.Interfaces;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;

namespace Prog6_Assessment_CodyBoelens.Rules.BookingRules
{
    public class AmountOfAnimalsRule : IBookingRule
    {
        //Klanten zonder klantenkaart mogen maximaal 3 dieren boeken
        //Klanten met een zilveren klantenkaart mogen 1 dier extra boeken
        //Klanten met een gouden kaart mogen zoveel dieren boeken als ze willen

        public string? ValidateRule(KlantViewModels klant, List<BeestjeViewModels> selectedBeestjes, DateTime datum)
        {
            if (selectedBeestjes.Count == 0)
            {
                return $"Je moet minimaal 1 dier boeken.";
            }

            int maxDieren = klant.KlantkaartId switch
            {
                1 => 4,
                2 => int.MaxValue,
                3 => int.MaxValue,
                _ => 3
            };

            if (selectedBeestjes.Count > maxDieren)
            {
                return $"Je mag maximaal {maxDieren} dieren boeken met jouw klantenkaart.";
            }

            return null;
        }
    }
}
