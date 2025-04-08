using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Newtonsoft.Json;
using Prog6_Assessment_CodyBoelens.Controllers;
using Prog6_Assessment_CodyBoelens.Data.DbEntities;
using Prog6_Assessment_CodyBoelens.Interfaces;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;
using System.Security.Claims;
using Xunit;

namespace BeestjeOpJeFeestjeUnitTests
{
    public class KlantControllerTests
    {
        private readonly Mock<IKlantService> _klantServiceMock;
        private readonly KlantController _controller;

        public KlantControllerTests()
        {
            _klantServiceMock = new Mock<IKlantService>();

            _controller = new KlantController(_klantServiceMock.Object);

            // Setup HttpContext with Session and TempData
            var httpContext = new DefaultHttpContext
            {
                Session = new MockHttpSession()
            };
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            _controller.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            // Setup an authenticated user with "Boerderij" role to satisfy [Authorize(Roles = "Boerderij")]
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Role, "Boerderij")
            }, "TestAuthType"); // Authenticated identity
            _controller.HttpContext.User = new ClaimsPrincipal(identity);
        }

        [Fact]
        public void Index_ReturnsViewWithKlantList()
        {
            // Arrange
            var klantList = new List<KlantViewModels>
            {
                new KlantViewModels { Id = 1, Name = "Klant1" },
                new KlantViewModels { Id = 2, Name = "Klant2" }
            };
            _klantServiceMock.Setup(x => x.GetKlantViewModels()).Returns(klantList);

            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<List<KlantViewModels>>(result.Model);
            Assert.Equal(klantList, model);
        }

        [Fact]
        public void Create_Get_ReturnsViewWithKlantViewModel()
        {
            // Arrange
            var ranks = new List<Klantkaart>
            {
                new Klantkaart { Id = 1, Rank = "Zilver" },
                new Klantkaart { Id = 2, Rank = "Goud" }
            };
            _klantServiceMock.Setup(x => x.GetAllKlantkaarten()).Returns(ranks);

            // Act
            var result = _controller.Create() as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<KlantViewModels>(result.Model);
            Assert.Equal(ranks, model.allRanks);
        }

        [Fact]
        public async Task Create_Post_ValidModel_RedirectsToIndexWithSuccessMessage()
        {
            // Arrange
            var klantViewModel = new KlantViewModels { Id = 1, Name = "NewKlant" };
            _klantServiceMock.Setup(x => x.CreateKlantAsync(klantViewModel)).ReturnsAsync("password123");

            // Act
            var result = await _controller.Create(klantViewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal($"Klant {klantViewModel.Name} aangemaakt met wachtwoord: password123", _controller.TempData["successMessage"]);
            _klantServiceMock.Verify(x => x.CreateKlantAsync(klantViewModel), Times.Once());
        }

        [Fact]
        public async Task Create_Post_InvalidModel_ReturnsViewWithError()
        {
            // Arrange
            var klantViewModel = new KlantViewModels { Id = 1, Name = "" }; // Invalid: Name required
            var ranks = new List<Klantkaart> { new Klantkaart { Id = 1, Rank = "Zilver" } };
            _klantServiceMock.Setup(x => x.GetAllKlantkaarten()).Returns(ranks);
            _controller.ModelState.AddModelError("Name", "The Name field is required.");

            // Act
            var result = await _controller.Create(klantViewModel) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
            var model = Assert.IsType<KlantViewModels>(result.Model);
            Assert.Equal(klantViewModel, model);
            Assert.Equal(ranks, model.allRanks);
            Assert.Equal("Vul de juiste gegevens in.", _controller.TempData["errorMessage"]);
        }

        [Fact]
        public async Task Create_Post_Exception_ReturnsViewWithError()
        {
            // Arrange
            var klantViewModel = new KlantViewModels { Id = 1, Name = "NewKlant" };
            var ranks = new List<Klantkaart> { new Klantkaart { Id = 1, Rank = "Zilver" } };
            _klantServiceMock.Setup(x => x.CreateKlantAsync(klantViewModel)).ThrowsAsync(new Exception("Test error"));
            _klantServiceMock.Setup(x => x.GetAllKlantkaarten()).Returns(ranks);

            // Act
            var result = await _controller.Create(klantViewModel) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<KlantViewModels>(result.Model);
            Assert.Equal(klantViewModel, model);
            Assert.Equal(ranks, model.allRanks);
            Assert.True(_controller.ModelState.ContainsKey(""));
            Assert.Equal("Test error", _controller.ModelState[""].Errors[0].ErrorMessage);
        }

        [Fact]
        public void Edit_Get_ValidId_ReturnsViewWithKlant()
        {
            // Arrange
            var klantViewModel = new KlantViewModels { Id = 1, Name = "Klant1" };
            _klantServiceMock.Setup(x => x.GetKlantById(1)).Returns(klantViewModel);

            // Act
            var result = _controller.Edit(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<KlantViewModels>(result.Model);
            Assert.Equal(klantViewModel, model);
        }

        [Fact]
        public void Edit_Get_InvalidId_RedirectsToIndex()
        {
            // Arrange
            _klantServiceMock.Setup(x => x.GetKlantById(999)).Returns((KlantViewModels)null);

            // Act
            var result = _controller.Edit(999) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Edit_Post_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var klantViewModel = new KlantViewModels { Id = 1, Name = "UpdatedKlant" };
            _klantServiceMock.Setup(x => x.UpdateKlantAsync(klantViewModel)).ReturnsAsync(true);

            // Act
            var result = await _controller.Edit(klantViewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _klantServiceMock.Verify(x => x.UpdateKlantAsync(klantViewModel), Times.Once());
        }

        [Fact]
        public async Task Edit_Post_InvalidModel_ReturnsViewWithError()
        {
            // Arrange
            var klantViewModel = new KlantViewModels { Id = 1, Name = "" }; // Invalid: Name required
            var ranks = new List<Klantkaart> { new Klantkaart { Id = 1, Rank = "Zilver" } };
            _klantServiceMock.Setup(x => x.GetAllKlantkaarten()).Returns(ranks);
            _controller.ModelState.AddModelError("Name", "The Name field is required.");

            // Act
            var result = await _controller.Edit(klantViewModel) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
            var model = Assert.IsType<KlantViewModels>(result.Model);
            Assert.Equal(klantViewModel, model);
            Assert.Equal(ranks, model.allRanks);
            Assert.Equal("Vul de juiste gegevens in.", _controller.TempData["errorMessage"]);
        }

        [Fact]
        public async Task Edit_Post_UpdateFails_RedirectsToIndex()
        {
            // Arrange
            var klantViewModel = new KlantViewModels { Id = 1, Name = "UpdatedKlant" };
            _klantServiceMock.Setup(x => x.UpdateKlantAsync(klantViewModel)).ReturnsAsync(false);

            // Act
            var result = await _controller.Edit(klantViewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _klantServiceMock.Verify(x => x.UpdateKlantAsync(klantViewModel), Times.Once());
        }

        // MockHttpSession implementation
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
            public bool TryGetValue(string key, out byte[] value) => _sessionStorage.TryGetValue(key, out value);
            public string GetString(string key) => TryGetValue(key, out byte[] value) ? System.Text.Encoding.UTF8.GetString(value) : null;
        }
    }
}
