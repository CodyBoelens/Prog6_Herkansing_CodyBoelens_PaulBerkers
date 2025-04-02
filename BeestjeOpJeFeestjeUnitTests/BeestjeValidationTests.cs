namespace BeestjeOpJeFeestjeUnitTests
{
    using Moq;
    using Prog6_Assessment_CodyBoelens.Data.DbEntities;
    using Prog6_Assessment_CodyBoelens.Interfaces;
    using System;
    using System.Collections.Generic;
    using Xunit;

    public class BeestjeValidationTests
    {
        private readonly Mock<IBoekingService> _mockBoekingService;

        public BeestjeValidationTests()
        {
            _mockBoekingService = new Mock<IBoekingService>();
        }

        [Fact]
        public void CheckLeeuwIjsbeerWithBoerderijdier_ReturnsErrorMessage()
        {
            var testBeestjes = new List<(Beestje Beestje, Types Type)>
        {
            (new Beestje { Name = "Leeuw" }, new Types { TypeName = "Jungle" }),
            (new Beestje { Name = "Koe" }, new Types { TypeName = "Boerderij" })
        };

            _mockBoekingService
                .Setup(s => s.CheckLeeuwIjsbeerWithBoerderijdier(testBeestjes))
                .Returns("Je kunt geen Leeuw of IJsbeer boeken samen met een Boerderijdier.");

            var result = _mockBoekingService.Object.CheckLeeuwIjsbeerWithBoerderijdier(testBeestjes);

            Assert.Equal("Je kunt geen Leeuw of IJsbeer boeken samen met een Boerderijdier.", result);
        }

        [Fact]
        public void CheckPinguinNietInWeekend_ReturnsErrorMessage_OnSaturday()
        {
            var testBeestjes = new List<(Beestje Beestje, Types Type)>
        {
            (new Beestje { Name = "Pinguin" }, new Types { TypeName = "Pool" })
        };

            var testDatum = new DateTime(2025, 3, 29); // Zaterdag

            _mockBoekingService
                .Setup(s => s.CheckPinguinNietInWeekend(testBeestjes, testDatum))
                .Returns("Pinguïns werken alleen doordeweeks. 'Dieren in pak werken alleen doordeweeks'");

            var result = _mockBoekingService.Object.CheckPinguinNietInWeekend(testBeestjes, testDatum);

            Assert.Equal("Pinguïns werken alleen doordeweeks. 'Dieren in pak werken alleen doordeweeks'", result);
        }

        [Fact]
        public void CheckWoestijndierenNietWinter_ReturnsErrorMessage()
        {
            var testBeestjes = new List<(Beestje Beestje, Types Type)>
        {
            (new Beestje { Name = "Kameel" }, new Types { TypeName = "Woestijn" })
        };

            var testDatum = new DateTime(2025, 12, 15); // December

            _mockBoekingService
                .Setup(s => s.CheckWoestijndierenNietWinter(testBeestjes, testDatum))
                .Returns("Woestijndieren kunnen niet geboekt worden in oktober t/m februari. 'Brrrr – Veel te koud'");

            var result = _mockBoekingService.Object.CheckWoestijndierenNietWinter(testBeestjes, testDatum);

            Assert.Equal("Woestijndieren kunnen niet geboekt worden in oktober t/m februari. 'Brrrr – Veel te koud'", result);
        }

        [Fact]
        public void CheckSneeuwdierenNietZomer_ReturnsErrorMessage()
        {
            var testBeestjes = new List<(Beestje Beestje, Types Type)>
        {
            (new Beestje { Name = "IJsbeer" }, new Types { TypeName = "Sneeuw" })
        };

            var testDatum = new DateTime(2025, 7, 10); // Juli

            _mockBoekingService
                .Setup(s => s.CheckSneeuwdierenNietZomer(testBeestjes, testDatum))
                .Returns("Sneeuwdieren kunnen niet geboekt worden in juni t/m augustus. 'Some People Are Worth Melting For ~ Olaf'");

            var result = _mockBoekingService.Object.CheckSneeuwdierenNietZomer(testBeestjes, testDatum);

            Assert.Equal("Sneeuwdieren kunnen niet geboekt worden in juni t/m augustus. 'Some People Are Worth Melting For ~ Olaf'", result);
        }

        [Fact]
        public void CheckMaxDierenPerKlant_ReturnsErrorMessage_IfTooManyAnimals()
        {
            var testBeestjesId = new List<int> { 1, 2, 3, 4 };
            int klantkaartId = 0; // Standaard kaart (max 3 dieren)

            _mockBoekingService
                .Setup(s => s.CheckMaxDierenPerKlant(testBeestjesId, klantkaartId))
                .Returns("Je mag maximaal 3 dieren boeken met jouw klantenkaart.");

            var result = _mockBoekingService.Object.CheckMaxDierenPerKlant(testBeestjesId, klantkaartId);

            Assert.Equal("Je mag maximaal 3 dieren boeken met jouw klantenkaart.", result);
        }

        [Fact]
        public void CheckVIPAlleenPlatina_ReturnsErrorMessage_IfNotPlatina()
        {
            var testBeestjes = new List<(Beestje Beestje, Types Type)>
        {
            (new Beestje { Name = "Draak" }, new Types { TypeName = "VIP" })
        };

            int klantkaartId = 1; // Geen Platina kaart

            _mockBoekingService
                .Setup(s => s.CheckVIPAlleenPlatina(testBeestjes, klantkaartId))
                .Returns("Alleen klanten met een Platina kaart kunnen VIP beestjes boeken");

            var result = _mockBoekingService.Object.CheckVIPAlleenPlatina(testBeestjes, klantkaartId);

            Assert.Equal("Alleen klanten met een Platina kaart kunnen VIP beestjes boeken", result);
        }

        [Fact]
        public void CheckLeeuwIjsbeerWithBoerderijdier_ReturnsNull_IfNoConflict()
        {
            var testBeestjes = new List<(Beestje Beestje, Types Type)>
        {
            (new Beestje { Name = "Leeuw" }, new Types { TypeName = "Savanne" }),
            (new Beestje { Name = "Tijger" }, new Types { TypeName = "Jungle" })
        };

            _mockBoekingService
                .Setup(s => s.CheckLeeuwIjsbeerWithBoerderijdier(testBeestjes))
                .Returns((string)null);

            var result = _mockBoekingService.Object.CheckLeeuwIjsbeerWithBoerderijdier(testBeestjes);

            Assert.Null(result);
        }

        [Fact]
        public void CheckPinguinNietInWeekend_ReturnsNull_IfOnWeekday()
        {
            var testBeestjes = new List<(Beestje Beestje, Types Type)>
        {
            (new Beestje { Name = "Pinguin" }, new Types { TypeName = "Pool" })
        };

            var testDatum = new DateTime(2025, 3, 26); // Woensdag

            _mockBoekingService
                .Setup(s => s.CheckPinguinNietInWeekend(testBeestjes, testDatum))
                .Returns((string)null);

            var result = _mockBoekingService.Object.CheckPinguinNietInWeekend(testBeestjes, testDatum);

            Assert.Null(result);
        }

        [Fact]
        public void CheckWoestijndierenNietWinter_ReturnsNull_IfNotWinter()
        {
            var testBeestjes = new List<(Beestje Beestje, Types Type)>
        {
            (new Beestje { Name = "Kameel" }, new Types { TypeName = "Woestijn" })
        };

            var testDatum = new DateTime(2025, 6, 15); // Juni (zomer)

            _mockBoekingService
                .Setup(s => s.CheckWoestijndierenNietWinter(testBeestjes, testDatum))
                .Returns((string)null);

            var result = _mockBoekingService.Object.CheckWoestijndierenNietWinter(testBeestjes, testDatum);

            Assert.Null(result);
        }

        [Fact]
        public void CheckSneeuwdierenNietZomer_ReturnsNull_IfNotSummer()
        {
            var testBeestjes = new List<(Beestje Beestje, Types Type)>
        {
            (new Beestje { Name = "IJsbeer" }, new Types { TypeName = "Sneeuw" })
        };

            var testDatum = new DateTime(2025, 12, 10); // December (winter)

            _mockBoekingService
                .Setup(s => s.CheckSneeuwdierenNietZomer(testBeestjes, testDatum))
                .Returns((string)null);

            var result = _mockBoekingService.Object.CheckSneeuwdierenNietZomer(testBeestjes, testDatum);

            Assert.Null(result);
        }

        [Fact]
        public void CheckMaxDierenPerKlant_ReturnsNull_IfWithinLimit()
        {
            var testBeestjesId = new List<int> { 1, 2, 3 };
            int klantkaartId = 0; // Standaard kaart (max 3 dieren)

            _mockBoekingService
                .Setup(s => s.CheckMaxDierenPerKlant(testBeestjesId, klantkaartId))
                .Returns((string)null);

            var result = _mockBoekingService.Object.CheckMaxDierenPerKlant(testBeestjesId, klantkaartId);

            Assert.Null(result);
        }

        [Fact]
        public void CheckVIPAlleenPlatina_ReturnsNull_IfPlatinaCustomer()
        {
            var testBeestjes = new List<(Beestje Beestje, Types Type)>
        {
            (new Beestje { Name = "Draak" }, new Types { TypeName = "VIP" })
        };

            int klantkaartId = 3; // Platina kaart

            _mockBoekingService
                .Setup(s => s.CheckVIPAlleenPlatina(testBeestjes, klantkaartId))
                .Returns((string)null);

            var result = _mockBoekingService.Object.CheckVIPAlleenPlatina(testBeestjes, klantkaartId);

            Assert.Null(result);
        }
    }

}