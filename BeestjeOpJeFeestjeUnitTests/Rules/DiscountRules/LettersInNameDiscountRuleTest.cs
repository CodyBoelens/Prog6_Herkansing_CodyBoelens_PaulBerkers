using Prog6_Assessment_CodyBoelens.Rules.DiscountRules;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;
using System;
using System.Collections.Generic;

namespace BeestjeOpJeFeestjeUnitTests.Rules.DiscountRules
{
    public class LettersInNameDiscountRuleTests
    {
        private readonly LettersInNameDiscountRule _rule;

        public LettersInNameDiscountRuleTests()
        {
            _rule = new LettersInNameDiscountRule();
        }

        [Fact]
        public void GetDiscount_ShouldReturn2_WhenNameContainsA()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels { Name = "Aap" }
            };
            var datum = DateTime.Now;

            // Act
            var discount = _rule.GetDiscount(klant, selectedBeestjes, datum);

            // Assert
            Assert.Equal(2, discount);
        }

        [Fact]
        public void GetDiscount_ShouldReturn4_WhenNameContainsAB()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels { Name = "Zebra" }
            };
            var datum = DateTime.Now;

            // Act
            var discount = _rule.GetDiscount(klant, selectedBeestjes, datum);

            // Assert
            Assert.Equal(4, discount); 
        }

        [Fact]
        public void GetDiscount_ShouldReturn6_WhenNameContainsABC()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels { Name = "Cobra" }
            };
            var datum = DateTime.Now;

            // Act
            var discount = _rule.GetDiscount(klant, selectedBeestjes, datum);

            // Assert
            Assert.Equal(6, discount);
        }

        [Fact]
        public void GetDiscount_ShouldReturn2_WhenNameContainsAAndCButNotB()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels { Name = "Alpaca" }
            };
            var datum = DateTime.Now;

            // Act
            var discount = _rule.GetDiscount(klant, selectedBeestjes, datum);

            // Assert
            Assert.Equal(2, discount);
        }

        [Fact]
        public void GetDiscount_ShouldReturn2_WhenNameContainsAllLetters()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels { Name = "qjkmprzwoxifbvghsdlteyucna" }
            };
            var datum = DateTime.Now;

            // Act
            var discount = _rule.GetDiscount(klant, selectedBeestjes, datum);

            // Assert
            Assert.Equal(52, discount);
        }

        [Fact]
        public void GetDiscount_ShouldReturn0_WhenNameContainsNoA()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels { Name = "Boktor" }
            };
            var datum = DateTime.Now;

            // Act
            var discount = _rule.GetDiscount(klant, selectedBeestjes, datum);

            // Assert
            Assert.Equal(0, discount);
        }
    }
}
