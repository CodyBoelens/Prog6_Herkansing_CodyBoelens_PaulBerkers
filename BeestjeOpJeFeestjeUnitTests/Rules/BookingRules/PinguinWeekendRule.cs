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
    namespace BeestjeOpJeFeestjeUnitTests.Rules.BookingRules
    {
        public class PinguinWeekendRuleTest
        {
            private readonly PinguinWeekendRule _rule;

            public PinguinWeekendRuleTest()
            {
                _rule = new PinguinWeekendRule();
            }

            // Helper method to get a date for a specific day of the week
            private DateTime GetDateForDayOfWeek(DayOfWeek day)
            {
                DateTime today = DateTime.Today;
                int diff = day - today.DayOfWeek;

                if (diff < 0) diff += 7;

                return today.AddDays(diff);
            }

            [Fact]
            public void ValidateRule_ShouldReturnError_WhenPinguinIsBookedOnSaturday()
            {
                // Arrange
                var klant = new KlantViewModels();
                var selectedBeestjes = new List<BeestjeViewModels>
                {
                    new BeestjeViewModels { Name = "Pinguin" }
                };
                var saturday = GetDateForDayOfWeek(DayOfWeek.Saturday);

                // Act
                var result = _rule.ValidateRule(klant, selectedBeestjes, saturday);

                // Assert
                Assert.Equal("Pinguïns werken alleen doordeweeks. 'Dieren in pak werken alleen doordeweeks'", result);
            }

            [Fact]
            public void ValidateRule_ShouldReturnError_WhenPinguinIsBookedOnSunday()
            {
                // Arrange
                var klant = new KlantViewModels();
                var selectedBeestjes = new List<BeestjeViewModels>
                {
                    new BeestjeViewModels { Name = "Pinguin" }
                };
                var sunday = GetDateForDayOfWeek(DayOfWeek.Sunday);

                // Act
                var result = _rule.ValidateRule(klant, selectedBeestjes, sunday);

                // Assert
                Assert.Equal("Pinguïns werken alleen doordeweeks. 'Dieren in pak werken alleen doordeweeks'", result);
            }

            [Theory]
            [InlineData(DayOfWeek.Monday)]
            [InlineData(DayOfWeek.Tuesday)]
            [InlineData(DayOfWeek.Wednesday)]
            [InlineData(DayOfWeek.Thursday)]
            [InlineData(DayOfWeek.Friday)]
            public void ValidateRule_ShouldReturnNull_WhenPinguinIsBookedOnWeekday(DayOfWeek dayOfWeek)
            {
                // Arrange
                var klant = new KlantViewModels();
                var selectedBeestjes = new List<BeestjeViewModels>
                {
                    new BeestjeViewModels { Name = "Pinguin" }
                };
                var date = GetDateForDayOfWeek(dayOfWeek);

                // Act
                var result = _rule.ValidateRule(klant, selectedBeestjes, date);

                // Assert
                Assert.Null(result);
            }

            [Fact]
            public void ValidateRule_ShouldReturnNull_WhenNoPinguinIsBooked()
            {
                // Arrange
                var klant = new KlantViewModels();
                var selectedBeestjes = new List<BeestjeViewModels>
                {
                    new BeestjeViewModels { Name = "Leeuw" }
                };
                var sunday = GetDateForDayOfWeek(DayOfWeek.Sunday);

                // Act
                var result = _rule.ValidateRule(klant, selectedBeestjes, sunday);

                // Assert
                Assert.Null(result);
            }
        }
    }
}
