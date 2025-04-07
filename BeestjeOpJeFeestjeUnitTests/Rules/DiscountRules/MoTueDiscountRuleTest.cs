using Prog6_Assessment_CodyBoelens.Rules.DiscountRules;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;
using System;
using System.Collections.Generic;

namespace BeestjeOpJeFeestjeUnitTests.Rules.DiscountRules
{
    public class MoTueDiscountRuleTest
    {
        private readonly MoTueDiscountRule _rule;

        public MoTueDiscountRuleTest()
        {
            _rule = new MoTueDiscountRule();
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
        public void GetDiscount_ShouldReturn15_WhenBookingIsOnMonday()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>();
            var datum = GetDateForDayOfWeek(DayOfWeek.Monday);

            // Act
            var discount = _rule.GetDiscount(klant, selectedBeestjes, datum);

            // Assert
            Assert.Equal(15, discount);
        }

        [Fact]
        public void GetDiscount_ShouldReturn15_WhenBookingIsOnTuesday()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>();
            var datum = GetDateForDayOfWeek(DayOfWeek.Tuesday);

            // Act
            var discount = _rule.GetDiscount(klant, selectedBeestjes, datum);

            // Assert
            Assert.Equal(15, discount);
        }

        [Fact]
        public void GetDiscount_ShouldReturn0_WhenBookingIsOnWednesday()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>();
            var datum = GetDateForDayOfWeek(DayOfWeek.Wednesday);

            // Act
            var discount = _rule.GetDiscount(klant, selectedBeestjes, datum);

            // Assert
            Assert.Equal(0, discount);
        }

        [Fact]
        public void GetDiscount_ShouldReturn0_WhenBookingIsOnThursday()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>();
            var datum = GetDateForDayOfWeek(DayOfWeek.Thursday);

            // Act
            var discount = _rule.GetDiscount(klant, selectedBeestjes, datum);

            // Assert
            Assert.Equal(0, discount);
        }

        [Fact]
        public void GetDiscount_ShouldReturn0_WhenBookingIsOnFriday()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>();
            var datum = GetDateForDayOfWeek(DayOfWeek.Friday);

            // Act
            var discount = _rule.GetDiscount(klant, selectedBeestjes, datum);

            // Assert
            Assert.Equal(0, discount);
        }

        [Fact]
        public void GetDiscount_ShouldReturn0_WhenBookingIsOnSaturday()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>();
            var datum = GetDateForDayOfWeek(DayOfWeek.Saturday);

            // Act
            var discount = _rule.GetDiscount(klant, selectedBeestjes, datum);

            // Assert
            Assert.Equal(0, discount);
        }

        [Fact]
        public void GetDiscount_ShouldReturn0_WhenBookingIsOnSunday()
        {
            // Arrange
            var klant = new KlantViewModels();
            var selectedBeestjes = new List<BeestjeViewModels>();
            var datum = GetDateForDayOfWeek(DayOfWeek.Sunday);

            // Act
            var discount = _rule.GetDiscount(klant, selectedBeestjes, datum);

            // Assert
            Assert.Equal(0, discount);
        }
    }
}