using Microsoft.AspNetCore.Mvc;
using Moq;
using Prog6_Assessment_CodyBoelens.Controllers;
using Prog6_Assessment_CodyBoelens.Interfaces;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BoekingsViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeestjeOpJeFeestjeUnitTests
{
    public class BoekingManagementControllerTests
    {
        private readonly Mock<IBoekingService> _mockBoekingService;
        private readonly Mock<IKlantService> _mockKlantService;
        private readonly Mock<IBeestjeService> _mockBeestjeService;
        private readonly BoekingManagementController _controller;

        public BoekingManagementControllerTests()
        {
            _mockBoekingService = new Mock<IBoekingService>();
            _mockKlantService = new Mock<IKlantService>();
            _mockBeestjeService = new Mock<IBeestjeService>();
            _controller = new BoekingManagementController(_mockBoekingService.Object, _mockKlantService.Object, _mockBeestjeService.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewWithBookings()
        {
            // Arrange
            var bookings = new List<BoekingViewModel>
        {
            new BoekingViewModel { Id = 1, Name = "Test Booking" },
            new BoekingViewModel { Id = 2, Name = "Another Booking" }
        };
            _mockBoekingService.Setup(s => s.GetAllBoekingsAsync()).ReturnsAsync(bookings);

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<BoekingViewModel>>(viewResult.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenBookingDoesNotExist()
        {
            // Arrange
            _mockBoekingService.Setup(s => s.GetBookingByIdAsync(It.IsAny<int>())).ReturnsAsync((BoekingViewModel)null);

            // Act
            var result = await _controller.Details(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsViewWithBookingDetails_WhenBookingExists()
        {
            // Arrange
            var booking = new BoekingViewModel { Id = 1, Name = "Test Booking" };
            var bookedBeestjes = new List<BeestjeViewModels>
        {
            new BeestjeViewModels { Id = 1, Name = "Beestje 1" }
        };
            _mockBoekingService.Setup(s => s.GetBookingByIdAsync(1)).ReturnsAsync(booking);
            _mockBoekingService.Setup(s => s.GetBookedBeestjesByIdAsync(1)).ReturnsAsync(bookedBeestjes);

            // Act
            var result = await _controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<BoekingDetailsViewModel>(viewResult.Model);
            Assert.Equal(1, model.BoekedBeestjes.Count);
            Assert.Equal("Test Booking", model.Boeking.Name);
        }

        [Fact]
        public async Task Delete_Get_ReturnsViewWithBooking_WhenBookingExists()
        {
            // Arrange
            var booking = new BoekingViewModel { Id = 1, Name = "Test Booking" };
            _mockBoekingService.Setup(s => s.GetBookingByIdAsync(1)).ReturnsAsync(booking);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<BoekingViewModel>(viewResult.Model);
            Assert.Equal("Test Booking", model.Name);
        }

        [Fact]
        public async Task Delete_Get_RedirectsToIndex_WhenBookingDoesNotExist()
        {
            // Arrange
            _mockBoekingService.Setup(s => s.GetBookingByIdAsync(1)).ReturnsAsync((BoekingViewModel)null);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }
    }

}
