using Prog6_Assessment_CodyBoelens.Rules.BookingRules;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;

namespace BeestjeOpJeFeestjeUnitTests.Rules.BookingRules
{
    namespace BeestjeOpJeFeestjeUnitTests.Rules.BookingRules
    {
        public class DesertAnimalWinterRuleTest
        {
            private readonly DesertAnimalWinterRule _rule;

            public DesertAnimalWinterRuleTest()
            {
                _rule = new DesertAnimalWinterRule();
            }

            [Theory]
            [InlineData(10)] // October
            [InlineData(11)] // November
            [InlineData(12)] // December
            [InlineData(1)]  // January
            [InlineData(2)]  // February
            public void ValidateRule_ShouldReturnError_WhenWoestijnAnimalInWinterMonths(int month)
            {
                // Arrange
                var klant = new KlantViewModels();
                var selectedBeestjes = new List<BeestjeViewModels>
                {
                    new BeestjeViewModels { Type = "Woestijn" }
                };
                var datum = new DateTime(DateTime.Now.Year, month, 1);

                // Act
                var result = _rule.ValidateRule(klant, selectedBeestjes, datum);

                // Assert
                Assert.Equal("Woestijndieren kunnen niet geboekt worden in oktober t/m februari. 'Brrrr – Veel te koud'", result);
            }

            [Theory]
            [InlineData(3)]  // March
            [InlineData(4)]  // April
            [InlineData(5)]  // Mei
            [InlineData(6)]  // Juni
            [InlineData(7)]  // Juli
            [InlineData(8)]  // Augustus
            [InlineData(9)]  // September
            public void ValidateRule_ShouldReturnNull_WhenWoestijnAnimalInAllowedMonths(int month)
            {
                // Arrange
                var klant = new KlantViewModels();
                var selectedBeestjes = new List<BeestjeViewModels>
                {
                    new BeestjeViewModels { Type = "Woestijn" }
                };
                var datum = new DateTime(DateTime.Now.Year, month, 1);

                // Act
                var result = _rule.ValidateRule(klant, selectedBeestjes, datum);

                // Assert
                Assert.Null(result);
            }

            [Fact]
            public void ValidateRule_ShouldReturnNull_WhenNoWoestijnAnimalRegardlessOfMonth()
            {
                // Arrange
                var klant = new KlantViewModels();
                var selectedBeestjes = new List<BeestjeViewModels>
                {
                    new BeestjeViewModels { Type = "Jungle" },
                    new BeestjeViewModels { Type = "Boerderij" }
                };
                var datum = new DateTime(DateTime.Now.Year, 12, 1);

                // Act
                var result = _rule.ValidateRule(klant, selectedBeestjes, datum);

                // Assert
                Assert.Null(result);
            }
        }
    }

}
