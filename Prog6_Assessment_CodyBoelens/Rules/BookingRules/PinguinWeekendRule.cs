using Prog6_Assessment_CodyBoelens.Interfaces;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;

namespace Prog6_Assessment_CodyBoelens.Rules.BookingRules
{
    public class PinguinWeekendRule : IBookingRule
    {
        //Je mag geen beestje boeken met de naam ‘Pinguïn’ in het weekend

        public string? ValidateRule(KlantViewModels klant, List<BeestjeViewModels> selectedBeestjes, DateTime datum)
        {
            if (selectedBeestjes.Any(b => b.Name.ToLower() == "pinguin"))
            {
                if (datum.DayOfWeek == DayOfWeek.Saturday || datum.DayOfWeek == DayOfWeek.Sunday)
                {
                    return "Pinguïns werken alleen doordeweeks. 'Dieren in pak werken alleen doordeweeks'";
                }                
            }
            return null;
        }
    }
}
