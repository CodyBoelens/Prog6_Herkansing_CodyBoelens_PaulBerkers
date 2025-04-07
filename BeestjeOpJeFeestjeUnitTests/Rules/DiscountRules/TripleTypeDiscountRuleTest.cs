using Prog6_Assessment_CodyBoelens.Rules.DiscountRules;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;
using System;
using System.Collections.Generic;

namespace BeestjeOpJeFeestjeUnitTests.Rules.DiscountRules
{
    public class TripleTypeDiscountRuleTests
    {
        private readonly TripleTypeDiscountRule _rule;

        public TripleTypeDiscountRuleTests()
        {
            _rule = new TripleTypeDiscountRule();
        }

        [Fact]
        public void GetDiscount_ShouldReturn10_WhenThreeAnimalsOfSameType()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels { Type = "Jungle" },
                new BeestjeViewModels { Type = "Jungle" },
                new BeestjeViewModels { Type = "Jungle" }
            };
            var datum = DateTime.Now;

            // Act
            var discount = _rule.GetDiscount(klant, selectedBeestjes, datum);

            // Assert
            Assert.Equal(10, discount);
        }


        [Fact]
        public void GetDiscount_ShouldReturn10_WhenThreeAnimalsOfSameTypeTwice()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels { Type = "Jungle" },
                new BeestjeViewModels { Type = "Jungle" },
                new BeestjeViewModels { Type = "Jungle" },
                new BeestjeViewModels { Type = "Desert" },
                new BeestjeViewModels { Type = "Desert" },
                new BeestjeViewModels { Type = "Desert" }
            };
            var datum = DateTime.Now;

            // Act
            var discount = _rule.GetDiscount(klant, selectedBeestjes, datum);

            // Assert
            Assert.Equal(10, discount);
        }

        [Fact]
        public void GetDiscount_ShouldReturn10_WhenFourAnimalsOfSameType()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels { Type = "Jungle" },
                new BeestjeViewModels { Type = "Jungle" },
                new BeestjeViewModels { Type = "Jungle" },
                new BeestjeViewModels { Type = "Jungle" }
            };
            var datum = DateTime.Now;

            // Act
            var discount = _rule.GetDiscount(klant, selectedBeestjes, datum);

            // Assert
            Assert.Equal(10, discount);
        }

        [Fact]
        public void GetDiscount_ShouldReturn0_WhenLessThanThreeAnimalsOfSameType()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels { Type = "Jungle" },
                new BeestjeViewModels { Type = "Jungle" }
            };
            var datum = DateTime.Now;

            // Act
            var discount = _rule.GetDiscount(klant, selectedBeestjes, datum);

            // Assert
            Assert.Equal(0, discount);
        }

        [Fact]
        public void GetDiscount_ShouldReturn0_WhenNoAnimalsOfSameType()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels { Type = "Jungle" },
                new BeestjeViewModels { Type = "Desert" },
                new BeestjeViewModels { Type = "Sea" }
            };
            var datum = DateTime.Now;

            // Act
            var discount = _rule.GetDiscount(klant, selectedBeestjes, datum);

            // Assert
            Assert.Equal(0, discount);
        }

        [Fact]
        public void GetDiscount_ShouldReturn10_WhenThreeOutOfFourAnimalsOfSameType()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels { Type = "Jungle" },
                new BeestjeViewModels { Type = "Jungle" },
                new BeestjeViewModels { Type = "Jungle" },
                new BeestjeViewModels { Type = "desert" }
            };
            var datum = DateTime.Now;

            // Act
            var discount = _rule.GetDiscount(klant, selectedBeestjes, datum);

            // Assert
            Assert.Equal(10, discount);
        }
    }
}
