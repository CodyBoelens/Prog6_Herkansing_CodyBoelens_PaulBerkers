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
    public class DonkeyCantBeAloneRuleTest
    {
        private readonly DonkeyCantBeAloneRule _rule;

        public DonkeyCantBeAloneRuleTest()
        {
            _rule = new DonkeyCantBeAloneRule();
        }

        [Fact]
        public void ValidateRule_ShouldReturnError_WhenOnlyDonkeyIsBooked()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels { Name = "Ezel" }
            };
            var datum = DateTime.Now;

            // Act
            var result = _rule.ValidateRule(klant, selectedBeestjes, datum);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Boek een ezel met een extra dier  'vriendschap verplicht!'", result);
        }

        [Fact]
        public void ValidateRule_ShouldReturnNull_WhenDonkeyIsWithAnotherAnimal()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels { Name = "Ezel" },
                new BeestjeViewModels { Name = "Kameel" }
            };
            var datum = DateTime.Now;

            // Act
            var result = _rule.ValidateRule(klant, selectedBeestjes, datum);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ValidateRule_ShouldReturnNull_WhenNoDonkeyIsBooked()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels { Name = "Kameel" }
            };
            var datum = DateTime.Now;

            // Act
            var result = _rule.ValidateRule(klant, selectedBeestjes, datum);

            // Assert
            Assert.Null(result);
        }
    }
}
