using Prog6_Assessment_CodyBoelens.Data;
using Prog6_Assessment_CodyBoelens.Interfaces;
using Prog6_Assessment_CodyBoelens.Rules.DiscountRules;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;

namespace Prog6_Assessment_CodyBoelens.Services
{
    public class KortingService : IKortingService
    {
        private readonly List<IDiscountRule> _discountRules;
        private const int _discountCap = 60;

        public KortingService()
        {
            _discountRules = new List<IDiscountRule> {
                new CustomerCardDiscountRule(),
                new DuckDiscountRule(),
                new LettersInNameDiscountRule(),
                new MoTueDiscountRule(),
                new TripleTypeDiscountRule(),
            };
        }

        public KortingService(List<IDiscountRule> discountRules)
        {
            _discountRules = discountRules;
        }

        public int GetTotalDiscountPercentage(KlantViewModels klant, List<BeestjeViewModels> selectedBeestjes, DateTime datum)
        {
            int discount = 0;

            foreach (var rule in _discountRules) {
                discount += rule.GetDiscount(klant, selectedBeestjes, datum);
            }

            if (discount > _discountCap) {
                discount = _discountCap;
            }

            return discount;
        }

    }

}
