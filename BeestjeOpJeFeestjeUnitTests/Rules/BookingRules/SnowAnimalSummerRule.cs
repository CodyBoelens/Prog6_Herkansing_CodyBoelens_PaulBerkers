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
    public class SnowAnimalSummerRuleTest
    {
        private readonly SnowAnimalSummerRule _rule;

        public SnowAnimalSummerRuleTest()
        {
            _rule = new SnowAnimalSummerRule();
        }

        [Theory]
        [InlineData(6)]  // June
        [InlineData(7)]  // July
        [InlineData(8)]  // August
        public void ValidateRule_ShouldReturnError_WhenSnowAnimalInSummerMonths(int month)
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels { Type = "Sneeuw" }
            };
            var datum = new DateTime(DateTime.Now.Year, month, 1);

            // Act
            var result = _rule.ValidateRule(klant, selectedBeestjes, datum);

            // Assert
            Assert.Equal("Sneeuwdieren kunnen niet geboekt worden in juni t/m augustus. 'Some People Are Worth Melting For ~ Olaf'", result);
        }

        [Theory]
        [InlineData(1)]  // January
        [InlineData(2)]  // February
        [InlineData(3)]  // March
        [InlineData(4)]  // April
        [InlineData(5)]  // May
        [InlineData(9)]  // September
        [InlineData(10)] // October
        [InlineData(11)] // November
        [InlineData(12)] // December
        public void ValidateRule_ShouldReturnNull_WhenSnowAnimalInAllowedMonths(int month)
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels { Type = "Sneeuw" }
            };
            var datum = new DateTime(DateTime.Now.Year, month, 1);

            // Act
            var result = _rule.ValidateRule(klant, selectedBeestjes, datum);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ValidateRule_ShouldReturnNull_WhenNoSnowAnimalRegardlessOfMonth()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels { Type = "Jungle" },
                new BeestjeViewModels { Type = "Woestijn" }
            };
            var datum = new DateTime(DateTime.Now.Year, 7, 1); // July

            // Act
            var result = _rule.ValidateRule(klant, selectedBeestjes, datum);

            // Assert
            Assert.Null(result);
        }
    }
}
