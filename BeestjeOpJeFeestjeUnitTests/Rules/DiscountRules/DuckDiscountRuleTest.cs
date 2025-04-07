using Moq;
using Prog6_Assessment_CodyBoelens.Rules.DiscountRules;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;
using System;
using System.Collections.Generic;

namespace BeestjeOpJeFeestjeUnitTests.Rules.DiscountRules
{
    public class DuckDiscountRuleTests
    {
        [Fact]
        public void GetDiscount_ShouldReturn50_WhenDuckIsPresentAndRandomIs1()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels { Name = "Other" },
                new BeestjeViewModels { Name = "Eend" }
            };

            var mockRandom = new Mock<Random>();
            mockRandom.Setup(r => r.Next(1, 7)).Returns(1);

            var discountRule = new DuckDiscountRule(mockRandom.Object);

            // Act
            var discount = discountRule.GetDiscount(klant, selectedBeestjes, DateTime.Now);

            // Assert
            Assert.Equal(50, discount);
        }

        [Fact]
        public void GetDiscount_ShouldReturn0_WhenDuckIsPresentButRandomIsNot1()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels { Name = "Other" },
                new BeestjeViewModels { Name = "Eend" }
            };

            var mockRandom = new Mock<Random>();
            mockRandom.Setup(r => r.Next(1, 7)).Returns(2);

            var discountRule = new DuckDiscountRule(mockRandom.Object);

            // Act
            var discount = discountRule.GetDiscount(klant, selectedBeestjes, DateTime.Now);

            // Assert
            Assert.Equal(0, discount);
        }

        [Fact]
        public void GetDiscount_ShouldReturn0_WhenDuckIsNotPresent()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels { Name = "Other" }
            };

            var mockRandom = new Mock<Random>();
            mockRandom.Setup(r => r.Next(1, 7)).Returns(1);

            var discountRule = new DuckDiscountRule(mockRandom.Object);

            // Act
            var discount = discountRule.GetDiscount(klant, selectedBeestjes, DateTime.Now);

            // Assert
            Assert.Equal(0, discount);
        }
    }
}
