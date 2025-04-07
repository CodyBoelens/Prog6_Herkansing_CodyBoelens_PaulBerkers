using Prog6_Assessment_CodyBoelens.Rules.DiscountRules;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;
using System;
using System.Collections.Generic;

namespace BeestjeOpJeFeestjeUnitTests.Rules.DiscountRules
{
    public class CustomerCardDiscountRuleTests
    {
        private CustomerCardDiscountRule _rule;

        public CustomerCardDiscountRuleTests()
        {
            _rule = new CustomerCardDiscountRule();
        }

        [Fact]
        public void GetDiscount_ShouldReturn10_WhenKlantkaartIdIsNotZero()
        {
            // Arrange
            var klant = new KlantViewModels
            {
                KlantkaartId = 1
            };

            var selectedBeestjes = new List<BeestjeViewModels>();
            var datum = DateTime.Now;

            // Act
            var discount = _rule.GetDiscount(klant, selectedBeestjes, datum);

            // Assert
            Assert.Equal(10, discount);
        }

        [Fact]
        public void GetDiscount_ShouldReturn0_WhenKlantkaartIdIsZero()
        {
            // Arrange
            var klant = new KlantViewModels
            {
                KlantkaartId = 0
            };

            var selectedBeestjes = new List<BeestjeViewModels>(); 
            var datum = DateTime.Now;

            // Act
            var discount = _rule.GetDiscount(klant, selectedBeestjes, datum);

            // Assert
            Assert.Equal(0, discount);
        }
    }
}
