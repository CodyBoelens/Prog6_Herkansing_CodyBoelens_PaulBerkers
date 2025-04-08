using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Prog6_Assessment_CodyBoelens.Controllers;
using Prog6_Assessment_CodyBoelens.Interfaces;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BoekingsViewModel;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BeestjeOpJeFeestjeUnitTests
{
    public class BoekingControllerTests
    {
        private readonly Mock<IBoekingService> _mockBoekingService;
        private readonly Mock<IKlantService> _mockKlantService;
        private readonly Mock<IBeestjeService> _mockBeestjeService;
        private readonly Mock<IKortingService> _mockKortingService;
        private readonly Mock<IBookingRulesService> _mockBookingRulesService;
        private readonly BoekingController _controller;

        public BoekingControllerTests()
        {
            _mockBoekingService = new Mock<IBoekingService>();
            _mockKlantService = new Mock<IKlantService>();
            _mockBeestjeService = new Mock<IBeestjeService>();
            _mockKortingService = new Mock<IKortingService>();
            _mockBookingRulesService = new Mock<IBookingRulesService>();

            _controller = new BoekingController(
                _mockBoekingService.Object,
                _mockKlantService.Object,
                _mockBeestjeService.Object,
                _mockKortingService.Object,
                _mockBookingRulesService.Object
            );
        }

        [Fact]
        public void Index_ReturnsRedirectToHome()
        {
            // Act
            var result = _controller.Index();

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Home", redirectToActionResult.ControllerName);
        }



        [Fact]
        public void Step01_ValidDate_SetsSessionAndRedirects()
        {
            // Arrange
            var eventDate = DateTime.Today.AddDays(1);  // Valid future date
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.Session).Returns(new Mock<ISession>().Object);
            _controller.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = _controller.Step01(eventDate);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Step02", redirectToActionResult.ActionName);
        }



        [Fact]
        public async Task Step03_Post_InvalidBeestjes_ValidatesAndReturnsView()
        {
            // Arrange
            var model = new Step03ViewModel();
            List<BeestjeViewModels> beestjeViewModels = new List<BeestjeViewModels>();

            _mockBeestjeService.Setup(s => s.GetBeestjesByIdsAsync(model.SelectedBeestjesIds)).ReturnsAsync(new List<BeestjeViewModels>());
            _mockBookingRulesService.Setup(s => s.ValidateBookingSelection(model.Klant, beestjeViewModels, model.Datum)).Returns(new List<string> { "Error" });

            // Act
            var result = await _controller.Step03(model);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var returnedModel = Assert.IsType<Step03ViewModel>(viewResult.Model);
            Assert.Equal(model, returnedModel);
            Assert.Contains("Error", viewResult.ViewData.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        }

    }

}
