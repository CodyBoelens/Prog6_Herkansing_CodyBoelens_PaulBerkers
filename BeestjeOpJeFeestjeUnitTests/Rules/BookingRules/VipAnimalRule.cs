using Prog6_Assessment_CodyBoelens.Rules.BookingRules;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeestjeOpJeFeestjeUnitTests.Rules.BookingRules
{
    public class VipAnimalRuleTest
    {
        private readonly VipAnimalRule _rule;

        public VipAnimalRuleTest()
        {
            _rule = new VipAnimalRule();
        }

        [Theory]
        [InlineData(1)] //Zilver
        [InlineData(2)] //Goud
        [InlineData(0)] //Geen
        public void ValidateRule_ShouldReturnError_WhenVipAnimalWithNonPlatinaCard(int klantkaartId)
        {
            // Arrange
            var klant = new KlantViewModels { KlantkaartId = klantkaartId };
            var selectedBeestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels { Type = "VIP" }
            };
            var datum = DateTime.Now;

            // Act
            var result = _rule.ValidateRule(klant, selectedBeestjes, datum);

            // Assert
            Assert.Equal("Alleen klanten met een Platina kaart kunnen VIP beestjes boeken", result);
        }

        [Fact]
        public void ValidateRule_ShouldReturnNull_WhenVipAnimalWithPlatinaCard()
        {
            // Arrange
            var klant = new KlantViewModels { KlantkaartId = 3 }; // Platina card
            var selectedBeestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels { Type = "VIP" }
            };
            var datum = DateTime.Now;

            // Act
            var result = _rule.ValidateRule(klant, selectedBeestjes, datum);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(1)] //Zilver
        [InlineData(2)] //Goud
        [InlineData(3)] //Platinum
        [InlineData(0)] //Geen
        public void ValidateRule_ShouldReturnNull_WhenNoVipAnimalRegardlessOfCard(int klantkaartId)
        {
            // Arrange
            var klant = new KlantViewModels { KlantkaartId = klantkaartId };
            var selectedBeestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels { Type = "Jungle" },
                new BeestjeViewModels { Type = "Sneeuw" }
            };
            var datum = DateTime.Now;

            // Act
            var result = _rule.ValidateRule(klant, selectedBeestjes, datum);

            // Assert
            Assert.Null(result);
        }
    }
}
