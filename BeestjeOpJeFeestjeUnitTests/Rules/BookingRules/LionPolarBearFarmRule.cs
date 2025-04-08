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
    public class LionPolarBearFarmRuleTest
    {
        private readonly LionPolarBearFarmRule _rule;

        public LionPolarBearFarmRuleTest()
        {
            _rule = new LionPolarBearFarmRule();
        }

        [Fact]
        public void ValidateRule_ShouldReturnError_WhenLionAndFarmAnimalAreBookedTogether()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels { Name = "Leeuw", Type = "Jungle" },
                new BeestjeViewModels { Name = "Koe", Type = "Boerderij" }
            };
            var datum = DateTime.Now;

            // Act
            var result = _rule.ValidateRule(klant, selectedBeestjes, datum);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Je kunt geen Leeuw of IJsbeer boeken samen met een Boerderijdier. 'Nom nom nom'", result);
        }

        [Fact]
        public void ValidateRule_ShouldReturnError_WhenPolarBearAndFarmAnimalAreBookedTogether()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels { Name = "IJsbeer", Type = "Sneuw" },
                new BeestjeViewModels { Name = "Varken", Type = "Boerderij" }
            };
            var datum = DateTime.Now;

            // Act
            var result = _rule.ValidateRule(klant, selectedBeestjes, datum);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Je kunt geen Leeuw of IJsbeer boeken samen met een Boerderijdier. 'Nom nom nom'", result);
        }

        [Fact]
        public void ValidateRule_ShouldReturnNull_WhenOnlyFarmAnimalsAreBooked()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels { Name = "Koe", Type = "Boerderij" },
                new BeestjeViewModels { Name = "Varken", Type = "Boerderij" }
            };
            var datum = DateTime.Now;

            // Act
            var result = _rule.ValidateRule(klant, selectedBeestjes, datum);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ValidateRule_ShouldReturnNull_WhenOnlyLionIsBookedWithoutFarmAnimals()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels { Name = "Leeuw", Type = "Jungle" }
            };
            var datum = DateTime.Now;

            // Act
            var result = _rule.ValidateRule(klant, selectedBeestjes, datum);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ValidateRule_ShouldReturnNull_WhenOnlyPolarBearIsBookedWithoutFarmAnimals()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels { Name = "IJsbeer", Type = "Sneuw" }
            };
            var datum = DateTime.Now;

            // Act
            var result = _rule.ValidateRule(klant, selectedBeestjes, datum);

            // Assert
            Assert.Null(result);
        }



        [Fact]
        public void ValidateRule_ShouldReturnNull_WhenOnlyPolarBearAndLionIsBookedWithoutOtherAnimals()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels { Name = "IJsbeer", Type = "Sneuw" },
                new BeestjeViewModels { Name = "Leeuw", Type = "Jungle" },
                new BeestjeViewModels { Name = "Kameel", Type = "Woestijn" }
            };
            var datum = DateTime.Now;

            // Act
            var result = _rule.ValidateRule(klant, selectedBeestjes, datum);

            // Assert
            Assert.Null(result);
        }
    }
}
