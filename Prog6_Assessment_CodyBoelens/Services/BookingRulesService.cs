using Prog6_Assessment_CodyBoelens.Interfaces;
using Prog6_Assessment_CodyBoelens.Rules.BookingRules;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;

namespace Prog6_Assessment_CodyBoelens.Services
{
    public class BookingRulesService : IBookingRulesService
    {
        private readonly List<IBookingRule> _bookingRules;

        public BookingRulesService()
        {
            _bookingRules = new List<IBookingRule> {
                new AmountOfAnimalsRule(),
                new DesertAnimalWinterRule(),
                new DonkeyCantBeAloneRule(),
                new LionPolarBearFarmRule(),
                new PinguinWeekendRule(),
                new SnowAnimalSummerRule(),
                new VipAnimalRule(),
            };
        }

        public BookingRulesService(List<IBookingRule> bookingRules)
        {
            _bookingRules = bookingRules;
        }


        public List<string> ValidateBookingSelection(KlantViewModels klant, List<BeestjeViewModels> selectedBeestjes, DateTime datum)
        {
            List<string> validationMessages = new List<string>();

            foreach (var rule in _bookingRules)
            {
                var result = rule.ValidateRule(klant, selectedBeestjes, datum);
                if(result != null) validationMessages.Add(result);
            }

            return validationMessages;
        }
    }
}
