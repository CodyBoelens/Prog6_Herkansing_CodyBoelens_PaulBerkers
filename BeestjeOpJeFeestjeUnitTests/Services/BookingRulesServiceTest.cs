using Moq;
using Prog6_Assessment_CodyBoelens.Interfaces;
using Prog6_Assessment_CodyBoelens.Services;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeestjeOpJeFeestjeUnitTests.Services
{
    namespace BeestjeOpJeFeestjeUnitTests.Services
    {
        public class BookingRulesServiceTest
        {
            [Fact]
            public void ValidateBookingSelection_ShouldReturnMessages_WhenRulesAreViolated()
            {
                // Arrange:
                var mockRule1 = new Mock<IBookingRule>();
                mockRule1.Setup(r => r.ValidateRule(It.IsAny<KlantViewModels>(), It.IsAny<List<BeestjeViewModels>>(), It.IsAny<DateTime>()))
                    .Returns("Maximaal 3 dieren toegestaan.");

                var mockRule2 = new Mock<IBookingRule>();
                mockRule2.Setup(r => r.ValidateRule(It.IsAny<KlantViewModels>(), It.IsAny<List<BeestjeViewModels>>(), It.IsAny<DateTime>()))
                    .Returns("Sneeuwdieren kunnen niet geboekt worden in juni t/m augustus.");

                var bookingRules = new List<IBookingRule> { mockRule1.Object, mockRule2.Object };
                var service = new BookingRulesService(bookingRules);

                var klant = new KlantViewModels();
                var selectedBeestjes = new List<BeestjeViewModels>
                {
                    new BeestjeViewModels { Type = "Sneeuw" }
                };
                var bookingDate = DateTime.Now;

                // Act:
                var validationMessages = service.ValidateBookingSelection(klant, selectedBeestjes, bookingDate);

                // Assert:
                Assert.Equal(2, validationMessages.Count);
                Assert.Contains("Maximaal 3 dieren toegestaan.", validationMessages);
                Assert.Contains("Sneeuwdieren kunnen niet geboekt worden in juni t/m augustus.", validationMessages);
            }

            [Fact]
            public void ValidateBookingSelection_ShouldReturnEmpty_WhenNoRulesAreViolated()
            {
                // Arrange: Mock a rule that returns null (no violation).
                var mockRule1 = new Mock<IBookingRule>();
                mockRule1.Setup(r => r.ValidateRule(It.IsAny<KlantViewModels>(), It.IsAny<List<BeestjeViewModels>>(), It.IsAny<DateTime>()))
                    .Returns((string)null); // No violation

                var bookingRules = new List<IBookingRule> { mockRule1.Object };
                var service = new BookingRulesService(bookingRules);

                var klant = new KlantViewModels();
                var selectedBeestjes = new List<BeestjeViewModels>
                {
                    new BeestjeViewModels { Type = "Farm" }
                };
                var bookingDate = DateTime.Now;

                // Act:
                var validationMessages = service.ValidateBookingSelection(klant, selectedBeestjes, bookingDate);

                // Assert:
                Assert.Empty(validationMessages);
            }

            [Fact]
            public void ValidateBookingSelection_ShouldReturnCorrectMessage_WhenOnlyOneRuleIsViolated()
            {
                // Arrange:
                var mockRule1 = new Mock<IBookingRule>();
                mockRule1.Setup(r => r.ValidateRule(It.IsAny<KlantViewModels>(), It.IsAny<List<BeestjeViewModels>>(), It.IsAny<DateTime>()))
                    .Returns("Maximaal 3 dieren toegestaan.");

                var mockRule2 = new Mock<IBookingRule>();
                mockRule2.Setup(r => r.ValidateRule(It.IsAny<KlantViewModels>(), It.IsAny<List<BeestjeViewModels>>(), It.IsAny<DateTime>()))
                    .Returns((string)null);

                var bookingRules = new List<IBookingRule> { mockRule1.Object, mockRule2.Object };
                var service = new BookingRulesService(bookingRules);

                var klant = new KlantViewModels();
                var selectedBeestjes = new List<BeestjeViewModels>
                {
                    new BeestjeViewModels { Type = "Farm" }
                };
                var bookingDate = DateTime.Now;

                // Act:
                var validationMessages = service.ValidateBookingSelection(klant, selectedBeestjes, bookingDate);

                // Assert:
                Assert.Single(validationMessages);
                Assert.Contains("Maximaal 3 dieren toegestaan.", validationMessages);
            }
        }
    }
}
