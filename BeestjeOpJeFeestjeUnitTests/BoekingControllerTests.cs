using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
        private readonly Mock<IBoekingService> _boekingServiceMock;
        private readonly Mock<IKlantService> _klantServiceMock;
        private readonly Mock<IBeestjeService> _beestjeServiceMock;
        private readonly Mock<IKortingService> _kortingServiceMock;
        private readonly Mock<IBookingRulesService> _bookingRulesServiceMock;
        private readonly BoekingController _controller;

        public BoekingControllerTests()
        {
            _boekingServiceMock = new Mock<IBoekingService>();
            _klantServiceMock = new Mock<IKlantService>();
            _beestjeServiceMock = new Mock<IBeestjeService>();
            _kortingServiceMock = new Mock<IKortingService>();
            _bookingRulesServiceMock = new Mock<IBookingRulesService>();

            _controller = new BoekingController(
                _boekingServiceMock.Object,
                _klantServiceMock.Object,
                _beestjeServiceMock.Object,
                _kortingServiceMock.Object,
                _bookingRulesServiceMock.Object
            );

            var httpContext = new DefaultHttpContext
            {
                Session = new MockHttpSession()
            };
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            _controller.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            _controller.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());
        }

        [Fact]
        public void Step01_WithPastDate_ReturnsRedirectToHomeWithError()
        {
            // Arrange
            var pastDate = DateTime.Today.AddDays(-1);

            // Act
            var result = _controller.Step01(pastDate) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Home", result.ControllerName);
            Assert.Equal("Selecteer een geldige datum in de toekomst.", _controller.TempData["EventDateErrorStep01"]);
        }

        [Fact]
        public async Task Step02_Get_AuthenticatedUser_ReturnsViewWithKlant()
        {
            // Arrange
            var userId = "test-user-id";
            var klant = new KlantViewModels { Id = 1, Name = "Test" };
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId) };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(identity);
            _controller.HttpContext.Session.SetString("selectedEventDate", DateTime.Today.AddDays(1).ToString("yyyy-MM-dd"));
            _klantServiceMock.Setup(x => x.GetKlantByApplicationUserId(userId)).Returns(klant);

            // Act
            var result = await _controller.Step02() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(klant, result.Model);
        }

        [Fact]
        public async Task Step03_Post_WithValidationErrors_ReturnsViewWithErrors()
        {
            // Arrange
            var model = new Step03ViewModel
            {
                Datum = DateTime.Today.AddDays(1),
                SelectedBeestjesIds = new List<int> { 1, 2 },
                Klant = new KlantViewModels { Id = 1 }
            };
            var beestjes = new List<BeestjeViewModels> { new BeestjeViewModels { Id = 1 } };
            var errors = new List<string> { "Validation error" };
            
            _controller.HttpContext.Session.SetString("selectedEventDate", model.Datum.ToString("yyyy-MM-dd"));
            _controller.HttpContext.Session.SetString("KlantInfo", JsonConvert.SerializeObject(model.Klant));
            _beestjeServiceMock.Setup(x => x.GetBeestjesByIdsAsync(model.SelectedBeestjesIds)).ReturnsAsync(beestjes);
            _bookingRulesServiceMock.Setup(x => x.ValidateBookingSelection(model.Klant, beestjes, model.Datum)).Returns(errors);
            _beestjeServiceMock.Setup(x => x.GetAvailableBeestjesAsync(model.Datum)).ReturnsAsync(beestjes);

            // Act
            var result = await _controller.Step03(model) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
            Assert.Equal(model, result.Model);
        }

        [Fact]
        public async Task Step04_Get_WithValidSession_ReturnsViewWithCalculatedPrice()
        {
            // Arrange
            var eventDate = DateTime.Today.AddDays(1);
            var klant = new KlantViewModels { Id = 1 };
            var beestjes = new List<BeestjeViewModels> { new BeestjeViewModels { Id = 1, Price = 100 } };
            var selectedIds = new List<int> { 1 };
            var discount = 20;

            _controller.HttpContext.Session.SetString("selectedEventDate", eventDate.ToString("yyyy-MM-dd"));
            _controller.HttpContext.Session.SetString("KlantInfo", JsonConvert.SerializeObject(klant));
            _controller.HttpContext.Session.SetString("SelectedBeestjes", JsonConvert.SerializeObject(selectedIds));

            _beestjeServiceMock.Setup(x => x.GetBeestjesByIdsAsync(It.Is<List<int>>(ids => ids.SequenceEqual(selectedIds))))
                .ReturnsAsync(beestjes);

            _kortingServiceMock.Setup(x => x.GetTotalDiscountPercentage(klant, beestjes, eventDate))
                .Returns(discount)
                .Verifiable();

            // Act
            var result = await _controller.Step04() as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = result.Model as Step04ViewModel;
            Assert.NotNull(model);
            Assert.Equal(100.0, model.TotaalPrijs);
        }

        [Fact]
        public async Task ConfirmBooking_WithValidData_CreatesBookingAndClearsSession()
        {
            // Arrange
            var eventDate = DateTime.Today.AddDays(1);
            var klant = new KlantViewModels
            {
                Id = 1,
                Name = "Test",
                Adres = "Test Address",
                PhoneNumber = "123456789",
                Email = "test@example.com"
            };
            var beestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels { Id = 1, Price = 100 }
            };
            var selectedIds = new List<int> { 1 };
            var totalPrice = "80.0";

            // Setup HttpContext more thoroughly
            var httpContext = new DefaultHttpContext();
            httpContext.Session = new MockHttpSession();

            // Explicitly set an unauthenticated user (mimicking ASP.NET Core default)
            var identity = new ClaimsIdentity(); 
            var principal = new ClaimsPrincipal(identity);
            httpContext.User = principal;

            // Set session data
            httpContext.Session.SetString("selectedEventDate", eventDate.ToString("yyyy-MM-dd"));
            httpContext.Session.SetString("KlantInfo", JsonConvert.SerializeObject(klant));
            httpContext.Session.SetString("SelectedBeestjes", JsonConvert.SerializeObject(selectedIds));
            httpContext.Session.SetString("TotalPrice", totalPrice);

            // Setup controller with proper context
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Setup TempData (might be needed for the success message)
            _controller.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            // Setup mocks
            _beestjeServiceMock.Setup(x => x.GetBeestjesByIdsAsync(selectedIds)).ReturnsAsync(beestjes);
            _boekingServiceMock.Setup(x => x.AddBoekingAsync(It.IsAny<BoekingViewModel>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.ConfirmBooking() as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Home", result.ControllerName);

            _boekingServiceMock.Verify(x => x.AddBoekingAsync(It.Is<BoekingViewModel>(b =>
                b.Date == eventDate &&
                b.Name == klant.Name &&
                b.Adress == klant.Adres &&
                b.PhoneNumber == klant.PhoneNumber &&
                b.Email == klant.Email &&
                b.TotaalPrijs == Convert.ToDouble(totalPrice) &&
                b.KlantId == klant.Id &&
                b.BeestjeIds.SequenceEqual(selectedIds)
            )), Times.Once());

            Assert.Null(httpContext.Session.GetString("selectedEventDate"));
            Assert.Null(httpContext.Session.GetString("KlantInfo"));
            Assert.Null(httpContext.Session.GetString("SelectedBeestjes"));
            Assert.Null(httpContext.Session.GetString("TotalPrice"));

            Assert.Equal("De boeking is gelukt!", _controller.TempData["orderSucces"]);
        }
    }

    // Mock HttpSession implementation for testing
    public class MockHttpSession : ISession
    {
        private readonly Dictionary<string, byte[]> _sessionStorage = new();

        public bool IsAvailable => true;
        public string Id => "test-session-id";
        public IEnumerable<string> Keys => _sessionStorage.Keys;

        public void Clear() => _sessionStorage.Clear();

        public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public void Remove(string key) => _sessionStorage.Remove(key);

        public void Set(string key, byte[] value) => _sessionStorage[key] = value;

        public bool TryGetValue(string key, out byte[] value)
        {
            return _sessionStorage.TryGetValue(key, out value);
        }

        public string GetString(string key)
        {
            if (TryGetValue(key, out byte[] value))
            {
                return System.Text.Encoding.UTF8.GetString(value);
            }
            return null;
        }
    }

}
