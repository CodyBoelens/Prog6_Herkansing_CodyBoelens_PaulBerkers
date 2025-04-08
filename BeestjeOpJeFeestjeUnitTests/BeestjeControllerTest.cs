using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Prog6_Assessment_CodyBoelens.Controllers;
using Prog6_Assessment_CodyBoelens.Interfaces;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using Prog6_Assessment_CodyBoelens.Data.DbEntities;

namespace BeestjeOpJeFeestjeUnitTests
{
    public class BeestjeControllerTests
    {
        private readonly Mock<IBeestjeService> _mockBeestjeService;
        private readonly BeestjeController _controller;

        public BeestjeControllerTests()
        {
            _mockBeestjeService = new Mock<IBeestjeService>();
            _controller = new BeestjeController(_mockBeestjeService.Object);

            // Setup TempData (required for error/success messages)
            _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
        }

        [Fact]
        public async Task Index_ShouldReturnViewWithBeestjes()
        {
            // Arrange
            var beestjes = new List<BeestjeViewModels>
            {
                new BeestjeViewModels { Id = 1, Name = "Cow", Type = "Boerderij", Price = 10 }
            };
            _mockBeestjeService.Setup(s => s.GetAllBeestjesAsync()).ReturnsAsync(beestjes);

            // Act
            var result = await _controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(beestjes, result.Model);
        }

        [Fact]
        public async Task Create_Get_ShouldReturnViewWithModel()
        {
            // Arrange
            var types = new List<Types> { new Types { Id = 1, TypeName = "Boerderij" } };
            var imageNames = new List<string> { "cow.jpg" };
            _mockBeestjeService.Setup(s => s.GetAllTypesAsync()).ReturnsAsync(types);
            _mockBeestjeService.Setup(s => s.GetAllImageNamesAsync()).ReturnsAsync(imageNames);

            // Act
            var result = await _controller.Create() as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<BeestjeViewModels>(result.Model);
            Assert.Equal(types, model.allTypes);
            Assert.Equal(imageNames, model.allImageNames);
        }

        [Fact]
        public async Task Create_Post_ShouldRedirectToIndex_WhenModelIsValid()
        {
            // Arrange
            var beestjeViewModel = new BeestjeViewModels { Name = "Cow", TypeId = 1, Price = 10, Picture = "cow.jpg" };
            _mockBeestjeService.Setup(s => s.CreateBeestjeAsync(beestjeViewModel)).ReturnsAsync(true);

            // Act
            var result = await _controller.Create(beestjeViewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Create_Post_ShouldReturnViewWithErrors_WhenModelStateIsInvalid()
        {
            // Arrange
            var beestjeViewModel = new BeestjeViewModels { Name = "", TypeId = 1 }; // Invalid: Name is required
            var types = new List<Types> { new Types { Id = 1, TypeName = "Boerderij" } };
            var imageNames = new List<string> { "cow.jpg" };
            _mockBeestjeService.Setup(s => s.GetAllTypesAsync()).ReturnsAsync(types);
            _mockBeestjeService.Setup(s => s.GetAllImageNamesAsync()).ReturnsAsync(imageNames);
            _controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _controller.Create(beestjeViewModel) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(beestjeViewModel, result.Model);
            Assert.Equal("Vul de juiste gegevens in.", _controller.TempData["errorMessage"]);
            var model = Assert.IsType<BeestjeViewModels>(result.Model);
            Assert.Equal(types, model.allTypes);
            Assert.Equal(imageNames, model.allImageNames);
        }

        [Fact]
        public async Task Create_Post_ShouldReturnViewWithException_WhenServiceThrows()
        {
            // Arrange
            var beestjeViewModel = new BeestjeViewModels { Name = "Cow", TypeId = 1, Price = 10 };
            var types = new List<Types> { new Types { Id = 1, TypeName = "Boerderij" } };
            var imageNames = new List<string> { "cow.jpg" };
            _mockBeestjeService.Setup(s => s.CreateBeestjeAsync(beestjeViewModel)).ThrowsAsync(new Exception("Create failed"));
            _mockBeestjeService.Setup(s => s.GetAllTypesAsync()).ReturnsAsync(types);
            _mockBeestjeService.Setup(s => s.GetAllImageNamesAsync()).ReturnsAsync(imageNames);

            // Act
            var result = await _controller.Create(beestjeViewModel) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(beestjeViewModel, result.Model);
            Assert.Equal("Create failed", _controller.TempData["errorMessage"]);
            var model = Assert.IsType<BeestjeViewModels>(result.Model);
            Assert.Equal(types, model.allTypes);
            Assert.Equal(imageNames, model.allImageNames);
        }

        [Fact]
        public async Task Edit_Get_ShouldReturnViewWithModel_WhenBeestjeExists()
        {
            // Arrange
            var beestje = new BeestjeViewModels { Id = 1, Name = "Cow", TypeId = 1, Price = 10 };
            var types = new List<Types> { new Types { Id = 1, TypeName = "Boerderij" } };
            var imageNames = new List<string> { "cow.jpg" };
            _mockBeestjeService.Setup(s => s.GetBeestjeByIdAsync(1)).ReturnsAsync(beestje);
            _mockBeestjeService.Setup(s => s.GetAllTypesAsync()).ReturnsAsync(types);
            _mockBeestjeService.Setup(s => s.GetAllImageNamesAsync()).ReturnsAsync(imageNames);

            // Act
            var result = await _controller.Edit(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<BeestjeViewModels>(result.Model);
            Assert.Equal(beestje, model);
            Assert.Equal(types, model.allTypes);
            Assert.Equal(imageNames, model.allImageNames);
        }

        [Fact]
        public async Task Edit_Get_ShouldRedirectToIndex_WhenBeestjeNotFound()
        {
            // Arrange
            _mockBeestjeService.Setup(s => s.GetBeestjeByIdAsync(1)).ReturnsAsync((BeestjeViewModels)null);

            // Act
            var result = await _controller.Edit(1) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Edit_Post_ShouldRedirectToIndex_WhenModelIsValid()
        {
            // Arrange
            var beestjeViewModel = new BeestjeViewModels { Id = 1, Name = "Cow", TypeId = 1, Price = 10 };
            _mockBeestjeService.Setup(s => s.UpdateBeestjeAsync(beestjeViewModel)).ReturnsAsync(true);

            // Act
            var result = await _controller.Edit(beestjeViewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Edit_Post_ShouldReturnViewWithErrors_WhenModelStateIsInvalid()
        {
            // Arrange
            var beestjeViewModel = new BeestjeViewModels { Id = 1, Name = "", TypeId = 1 }; // Invalid: Name is required
            var types = new List<Types> { new Types { Id = 1, TypeName = "Boerderij" } };
            var imageNames = new List<string> { "cow.jpg" };
            _mockBeestjeService.Setup(s => s.GetAllTypesAsync()).ReturnsAsync(types);
            _mockBeestjeService.Setup(s => s.GetAllImageNamesAsync()).ReturnsAsync(imageNames);
            _controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _controller.Edit(beestjeViewModel) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(beestjeViewModel, result.Model);
            Assert.Equal("Vul de juiste gegevens in.", _controller.TempData["errorMessage"]);
            var model = Assert.IsType<BeestjeViewModels>(result.Model);
            Assert.Equal(types, model.allTypes);
            Assert.Equal(imageNames, model.allImageNames);
        }

        [Fact]
        public async Task Delete_Get_ShouldReturnViewWithModel_WhenBeestjeExists()
        {
            // Arrange
            var beestje = new BeestjeViewModels { Id = 1, Name = "Cow" };
            _mockBeestjeService.Setup(s => s.GetBeestjeByIdAsync(1)).ReturnsAsync(beestje);

            // Act
            var result = await _controller.Delete(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(beestje, result.Model);
        }

        [Fact]
        public async Task Delete_Get_ShouldRedirectToIndex_WhenBeestjeNotFound()
        {
            // Arrange
            _mockBeestjeService.Setup(s => s.GetBeestjeByIdAsync(1)).ReturnsAsync((BeestjeViewModels)null);

            // Act
            var result = await _controller.Delete(1) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Delete_Post_ShouldRedirectToIndex_WhenDeleteSucceeds()
        {
            // Arrange
            var beestjeViewModel = new BeestjeViewModels { Id = 1, Name = "Cow" };
            _mockBeestjeService.Setup(s => s.DeleteBeestjeAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(beestjeViewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Beestje Cow is verwijderd.", _controller.TempData["successMessage"]);
        }

        [Fact]
        public async Task Delete_Post_ShouldReturnViewWithException_WhenDeleteFails()
        {
            // Arrange
            var beestjeViewModel = new BeestjeViewModels { Id = 1, Name = "Cow" };
            _mockBeestjeService.Setup(s => s.DeleteBeestjeAsync(1)).ThrowsAsync(new Exception("Delete failed"));

            // Act
            var result = await _controller.Delete(beestjeViewModel) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(beestjeViewModel, result.Model);
            Assert.Equal("Delete failed", _controller.TempData["errorMessage"]);
        }
    }
}