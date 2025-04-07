using Moq;
using Prog6_Assessment_CodyBoelens.Interfaces;
using Prog6_Assessment_CodyBoelens.Services;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;
using System;
using System.Collections.Generic;

namespace BeestjeOpJcFeestjeUnitTests.Services
{
    public class KortingServiceTests
    {
        [Fact]
        public void GetTotalDiscountPercentage_ShouldSumDiscounts_WhenBelowCap()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>();
            var datum = DateTime.Now;

            var mockRule1 = new Mock<IDiscountRule>();
            mockRule1.Setup(r => r.GetDiscount(klant, selectedBeestjes, datum)).Returns(10);

            var mockRule2 = new Mock<IDiscountRule>();
            mockRule2.Setup(r => r.GetDiscount(klant, selectedBeestjes, datum)).Returns(20);

            var mockRule3 = new Mock<IDiscountRule>();
            mockRule3.Setup(r => r.GetDiscount(klant, selectedBeestjes, datum)).Returns(15);

            var discountRules = new List<IDiscountRule>
            {
                mockRule1.Object,
                mockRule2.Object,
                mockRule3.Object
            };

            var service = new KortingService(discountRules);

            // Act
            var totalDiscount = service.GetTotalDiscountPercentage(klant, selectedBeestjes, datum);

            // Assert
            Assert.Equal(45, totalDiscount); // 10 + 20 + 15 = 45
        }

        [Fact]
        public void GetTotalDiscountPercentage_ShouldCapAt60_WhenSumExceedsCap()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>();
            var datum = DateTime.Now;

            var mockRule1 = new Mock<IDiscountRule>();
            mockRule1.Setup(r => r.GetDiscount(klant, selectedBeestjes, datum)).Returns(30);

            var mockRule2 = new Mock<IDiscountRule>();
            mockRule2.Setup(r => r.GetDiscount(klant, selectedBeestjes, datum)).Returns(40);

            var mockRule3 = new Mock<IDiscountRule>();
            mockRule3.Setup(r => r.GetDiscount(klant, selectedBeestjes, datum)).Returns(10);

            var discountRules = new List<IDiscountRule>
            {
                mockRule1.Object,
                mockRule2.Object,
                mockRule3.Object
            };

            var service = new KortingService(discountRules);

            // Act
            var totalDiscount = service.GetTotalDiscountPercentage(klant, selectedBeestjes, datum);

            // Assert
            Assert.Equal(60, totalDiscount); // 30 + 40 + 10 = 80, capped at 60
        }

        [Fact]
        public void GetTotalDiscountPercentage_ShouldReturn0_WhenNoDiscountsApply()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>();
            var datum = DateTime.Now;

            var mockRule1 = new Mock<IDiscountRule>();
            mockRule1.Setup(r => r.GetDiscount(klant, selectedBeestjes, datum)).Returns(0);

            var mockRule2 = new Mock<IDiscountRule>();
            mockRule2.Setup(r => r.GetDiscount(klant, selectedBeestjes, datum)).Returns(0);

            var discountRules = new List<IDiscountRule>
            {
                mockRule1.Object,
                mockRule2.Object
            };

            var service = new KortingService(discountRules);

            // Act
            var totalDiscount = service.GetTotalDiscountPercentage(klant, selectedBeestjes, datum);

            // Assert
            Assert.Equal(0, totalDiscount); // 0 + 0 = 0
        }

        [Fact]
        public void GetTotalDiscountPercentage_ShouldHandleSingleRule()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>();
            var datum = DateTime.Now;

            // Create a single mock rule
            var mockRule = new Mock<IDiscountRule>();
            mockRule.Setup(r => r.GetDiscount(klant, selectedBeestjes, datum)).Returns(25);

            var discountRules = new List<IDiscountRule> { mockRule.Object };
            var service = new KortingService(discountRules);

            // Act
            var totalDiscount = service.GetTotalDiscountPercentage(klant, selectedBeestjes, datum);

            // Assert
            Assert.Equal(25, totalDiscount); // Single rule returns 25
        }

        [Fact]
        public void GetTotalDiscountPercentage_ShouldHandleNoRule()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>();
            var datum = DateTime.Now;

            var discountRules = new List<IDiscountRule> {};
            var service = new KortingService(discountRules);

            // Act
            var totalDiscount = service.GetTotalDiscountPercentage(klant, selectedBeestjes, datum);

            // Assert
            Assert.Equal(0, totalDiscount); // No rules returns 0
        }
    }
}