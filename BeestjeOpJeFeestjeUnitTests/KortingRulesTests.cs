using Moq;
using Prog6_Assessment_CodyBoelens.Interfaces;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeestjeOpJeFeestjeUnitTests
{
    public class KortingServiceTests
    {
        private readonly Mock<IKortingService> _mockKortingService;
        private readonly Mock<Random> _mockRandom;

        public KortingServiceTests()
        {
            _mockKortingService = new Mock<IKortingService>();
            _mockRandom = new Mock<Random>();
        }

        [Fact]
        public void KortingDrieDierenVanZelfdeType_ShouldReturn10Percent_When3OfTheSameType()
        {
            // Arrange
            var selectedBeestjes = new List<BeestjeViewModels>
        {
            new BeestjeViewModels { Type = "A" },
            new BeestjeViewModels { Type = "A" },
            new BeestjeViewModels { Type = "A" }
        };
            double totaalPrijs = 100.0;

            // Act
            _mockKortingService.Setup(s => s.KortingDrieDierenVanZelfdeType(totaalPrijs, selectedBeestjes)).Returns(totaalPrijs * 0.10);
            double korting = _mockKortingService.Object.KortingDrieDierenVanZelfdeType(totaalPrijs, selectedBeestjes);

            // Assert
            Assert.Equal(10.0, korting); // Verwacht 10% korting
        }

        [Fact]
        public void KortingDrieDierenVanZelfdeType_ShouldReturn0_WhenLessThan3OfTheSameType()
        {
            // Arrange
            var selectedBeestjes = new List<BeestjeViewModels>
        {
            new BeestjeViewModels { Type = "A" },
            new BeestjeViewModels { Type = "B" }
        };
            double totaalPrijs = 100.0;

            // Act
            _mockKortingService.Setup(s => s.KortingDrieDierenVanZelfdeType(totaalPrijs, selectedBeestjes)).Returns(0.0);
            double korting = _mockKortingService.Object.KortingDrieDierenVanZelfdeType(totaalPrijs, selectedBeestjes);

            // Assert
            Assert.Equal(0.0, korting); // Geen korting als er minder dan 3 dezelfde dieren zijn
        }

        [Fact]
        public void KortingEendKans_ShouldReturn50Percent_WhenEendIsPresentAndRandomIs1()
        {
            // Arrange
            var selectedBeestjes = new List<BeestjeViewModels>
        {
            new BeestjeViewModels { Name = "Eend" }
        };
            double totaalPrijs = 100.0;
            _mockRandom.Setup(r => r.Next(1, 7)).Returns(1); // Mock de random return waarde naar 1

            // Act
            _mockKortingService.Setup(s => s.KortingEendKans(totaalPrijs, selectedBeestjes)).Returns(totaalPrijs * 0.50);
            double korting = _mockKortingService.Object.KortingEendKans(totaalPrijs, selectedBeestjes);

            // Assert
            Assert.Equal(50.0, korting); // Verwacht 50% korting
        }

        [Fact]
        public void KortingEendKans_ShouldReturn0_WhenEendIsNotPresent()
        {
            // Arrange
            var selectedBeestjes = new List<BeestjeViewModels>
        {
            new BeestjeViewModels { Name = "Kat" }
        };
            double totaalPrijs = 100.0;

            // Act
            _mockKortingService.Setup(s => s.KortingEendKans(totaalPrijs, selectedBeestjes)).Returns(0.0);
            double korting = _mockKortingService.Object.KortingEendKans(totaalPrijs, selectedBeestjes);

            // Assert
            Assert.Equal(0.0, korting); // Geen korting als er geen "Eend" is
        }

        [Fact]
        public void KortingOpWeekdag_ShouldReturn15Percent_WhenBookingIsOnMonday()
        {
            // Arrange
            var datum = new DateTime(2025, 4, 6); // Een maandag
            double totaalPrijs = 100.0;

            // Act
            _mockKortingService.Setup(s => s.KortingOpWeekdag(totaalPrijs, datum)).Returns(totaalPrijs * 0.15);
            double korting = _mockKortingService.Object.KortingOpWeekdag(totaalPrijs, datum);

            // Assert
            Assert.Equal(15.0, korting); // Verwacht 15% korting
        }

        [Fact]
        public void KortingOpWeekdag_ShouldReturn0_WhenBookingIsNotOnMondayOrTuesday()
        {
            // Arrange
            var datum = new DateTime(2025, 4, 4); // Vrijdag
            double totaalPrijs = 100.0;

            // Act
            _mockKortingService.Setup(s => s.KortingOpWeekdag(totaalPrijs, datum)).Returns(0.0);
            double korting = _mockKortingService.Object.KortingOpWeekdag(totaalPrijs, datum);

            // Assert
            Assert.Equal(0.0, korting); // Geen korting op andere dagen dan maandag of dinsdag
        }

        [Fact]
        public void KortingVoorLettersInNaam_ShouldReturnCorrectDiscount_ForConsecutiveLetters()
        {
            // Arrange
            var selectedBeestjes = new List<BeestjeViewModels>
        {
            new BeestjeViewModels { Name = "ABC" },
            new BeestjeViewModels { Name = "AC" },
            new BeestjeViewModels { Name = "ADE" }
        };

            // Act
            _mockKortingService.Setup(s => s.KortingVoorLettersInNaam(selectedBeestjes)).Returns(0.06); // Verwacht 6% korting
            double korting = _mockKortingService.Object.KortingVoorLettersInNaam(selectedBeestjes);

            // Assert
            Assert.Equal(0.06, korting); // Verwacht 6% korting (3 letters A, B, C)
        }

        [Fact]
        public void KortingVoorKlantenkaart_ShouldReturn10Percent_WhenKlantHasCard()
        {
            // Arrange
            var klant = new KlantViewModels { KlantkaartId = 1 }; // Klant heeft een klantenkaart
            double totaalPrijs = 100.0;

            // Act
            _mockKortingService.Setup(s => s.KortingVoorKlantenkaart(totaalPrijs, klant)).Returns(totaalPrijs * 0.10);
            double korting = _mockKortingService.Object.KortingVoorKlantenkaart(totaalPrijs, klant);

            // Assert
            Assert.Equal(10.0, korting); // Verwacht 10% korting
        }

        [Fact]
        public void KortingVoorKlantenkaart_ShouldReturn0_WhenKlantDoesNotHaveCard()
        {
            // Arrange
            var klant = new KlantViewModels { KlantkaartId = 0 }; // Klant heeft geen klantenkaart
            double totaalPrijs = 100.0;

            // Act
            _mockKortingService.Setup(s => s.KortingVoorKlantenkaart(totaalPrijs, klant)).Returns(0.0);
            double korting = _mockKortingService.Object.KortingVoorKlantenkaart(totaalPrijs, klant);

            // Assert
            Assert.Equal(0.0, korting); // Geen korting zonder klantenkaart
        }
    }

}
