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
    public class AmountOfAnimalsRuleTest
    {
        private readonly AmountOfAnimalsRule _rule;

        public AmountOfAnimalsRuleTest()
        {
            _rule = new AmountOfAnimalsRule();
        }

        [Fact]
        public void ValidateRule_ShouldReturnError_WhenNoAnimalsSelected()
        {
            // Arrange
            var klant = new KlantViewModels { KlantkaartId = 0 };
            var selectedBeestjes = new List<BeestjeViewModels>();
            var datum = DateTime.Now;

            // Act
            var result = _rule.ValidateRule(klant, selectedBeestjes, datum);

            // Assert
            Assert.Equal("Je moet minimaal 1 dier boeken.", result);
        }

        [Fact]
        public void ValidateRule_ShouldReturnError_WhenTooManyAnimalsWithoutCard()
        {
            // Arrange
            var klant = new KlantViewModels { KlantkaartId = 0 };
            var selectedBeestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels(),
                new BeestjeViewModels(),
                new BeestjeViewModels(),
                new BeestjeViewModels()
            };
            var datum = DateTime.Now;

            // Act
            var result = _rule.ValidateRule(klant, selectedBeestjes, datum);

            // Assert
            Assert.Equal("Je mag maximaal 3 dieren boeken met jouw klantenkaart.", result);
        }

        [Fact]
        public void ValidateRule_ShouldReturnError_WhenTooManyAnimalsWithSilverCard()
        {
            // Arrange
            var klant = new KlantViewModels { KlantkaartId = 1 }; // Silver card
            var selectedBeestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels(),
                new BeestjeViewModels(),
                new BeestjeViewModels(),
                new BeestjeViewModels(),
                new BeestjeViewModels()
            };
            var datum = DateTime.Now;

            // Act
            var result = _rule.ValidateRule(klant, selectedBeestjes, datum);

            // Assert
            Assert.Equal("Je mag maximaal 4 dieren boeken met jouw klantenkaart.", result);
        }

        [Fact]
        public void ValidateRule_ShouldReturnNull_WhenValidAmountOfAnimalsWithoutCard()
        {
            // Arrange
            var klant = new KlantViewModels { KlantkaartId = 0 };
            var selectedBeestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels(),
                new BeestjeViewModels(),
                new BeestjeViewModels()
            };
            var datum = DateTime.Now;

            // Act
            var result = _rule.ValidateRule(klant, selectedBeestjes, datum);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ValidateRule_ShouldReturnNull_WhenUnlimitedAnimalsWithGoldCard()
        {
            // Arrange
            var klant = new KlantViewModels { KlantkaartId = 2 }; // Gold card
            var selectedBeestjes = new List<BeestjeViewModels>();
            for (int i = 0; i < 10; i++) selectedBeestjes.Add(new BeestjeViewModels());
            var datum = DateTime.Now;

            // Act
            var result = _rule.ValidateRule(klant, selectedBeestjes, datum);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ValidateRule_ShouldReturnNull_WhenUnlimitedAnimalsWithPlatinumCard()
        {
            // Arrange
            var klant = new KlantViewModels { KlantkaartId = 3 }; // Platinum card
            var selectedBeestjes = new List<BeestjeViewModels>();
            for (int i = 0; i < 10; i++) selectedBeestjes.Add(new BeestjeViewModels());
            var datum = DateTime.Now;

            // Act
            var result = _rule.ValidateRule(klant, selectedBeestjes, datum);

            // Assert
            Assert.Null(result);
        }
    }
}
