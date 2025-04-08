using Microsoft.EntityFrameworkCore;
using Prog6_Assessment_CodyBoelens.Data.DbEntities;
using Prog6_Assessment_CodyBoelens.Data;
using Prog6_Assessment_CodyBoelens.Services;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BeestjeViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeestjeOpJeFeestjeUnitTests.Services
{
    public class BeestjeServiceTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique name per test
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetAllBeestjesAsync_ShouldReturnAllBeestjes()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Types.Add(new Types { Id = 1, TypeName = "Boerderij" });
            context.Beestjes.Add(new Beestje { Id = 1, Name = "Cow", TypeId = 1, Price = 10, Picture = "cow.jpg" });
            await context.SaveChangesAsync();
            var service = new BeestjeService(context);

            // Act
            var result = await service.GetAllBeestjesAsync();

            // Assert
            Assert.Single(result);
            var beestje = result[0];
            Assert.Equal(1, beestje.Id);
            Assert.Equal("Cow", beestje.Name);
            Assert.Equal("Boerderij", beestje.Type);
            Assert.Equal(10, beestje.Price);
            Assert.Equal("cow.jpg", beestje.Picture);
        }

        [Fact]
        public async Task GetAvailableBeestjesAsync_ShouldReturnAvailableBeestjes()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var date = new DateTime(2025, 5, 1);
            context.Types.Add(new Types { Id = 1, TypeName = "Boerderij" });
            context.Beestjes.Add(new Beestje { Id = 1, Name = "Cow", TypeId = 1, Price = 10, Picture = "cow.jpg" });
            context.Beestjes.Add(new Beestje { Id = 2, Name = "Sheep", TypeId = 1, Price = 8, Picture = "sheep.jpg" });
            context.Boekingen.Add(new Boeking
            {
                Id = 1,
                Date = date,
                Name = "Test",
                Adress = "Test Address",
                Email = "test@example.com", // Required field
                PhoneNumber = "1234567890", // Required field
                Is_Confirmed = true
            });
            context.BeestjeBoekingen.Add(new BeestjeBoeking { BeestjeID = 1, BoekingID = 1 });
            await context.SaveChangesAsync();
            var service = new BeestjeService(context);

            // Act
            var result = await service.GetAvailableBeestjesAsync(date);

            // Assert
            Assert.Single(result);
            var beestje = result[0];
            Assert.Equal(2, beestje.Id); // Only Sheep is available
            Assert.Equal("Sheep", beestje.Name);
            Assert.Equal("Boerderij", beestje.Type);
        }

        [Fact]
        public async Task GetBeestjeByIdAsync_ShouldReturnBeestje_WhenExists()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Types.Add(new Types { Id = 1, TypeName = "Boerderij" });
            context.Beestjes.Add(new Beestje { Id = 1, Name = "Cow", TypeId = 1, Price = 10, Picture = "cow.jpg" });
            await context.SaveChangesAsync();
            var service = new BeestjeService(context);

            // Act
            var result = await service.GetBeestjeByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Cow", result.Name);
            Assert.Equal("Boerderij", result.Type);
            Assert.Equal(1, result.TypeId);
            Assert.Equal(10, result.Price);
            Assert.Equal("cow.jpg", result.Picture);
            Assert.Single(result.allTypes);
        }

        [Fact]
        public async Task GetBeestjeByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var service = new BeestjeService(context);

            // Act
            var result = await service.GetBeestjeByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateBeestjeAsync_ShouldAddBeestjeAndReturnTrue()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var viewModel = new BeestjeViewModels { Name = "Cow", TypeId = 1, Price = 10, Picture = "cow.jpg" };
            var service = new BeestjeService(context);

            // Act
            var result = await service.CreateBeestjeAsync(viewModel);

            // Assert
            Assert.True(result);
            var beestje = await context.Beestjes.SingleOrDefaultAsync(b => b.Name == "Cow");
            Assert.NotNull(beestje);
            Assert.Equal("Cow", beestje.Name);
            Assert.Equal(1, beestje.TypeId);
            Assert.Equal(10, beestje.Price);
            Assert.Equal("cow.jpg", beestje.Picture);
        }

        [Fact]
        public async Task UpdateBeestjeAsync_ShouldUpdateBeestjeAndReturnTrue()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Beestjes.Add(new Beestje { Id = 1, Name = "Cow", TypeId = 1, Price = 10, Picture = "cow.jpg" });
            await context.SaveChangesAsync();
            var viewModel = new BeestjeViewModels { Id = 1, Name = "Updated Cow", TypeId = 2, Price = 15, Picture = "updated.jpg" };
            var service = new BeestjeService(context);

            // Act
            var result = await service.UpdateBeestjeAsync(viewModel);

            // Assert
            Assert.True(result);
            var beestje = await context.Beestjes.FindAsync(1);
            Assert.Equal("Updated Cow", beestje.Name);
            Assert.Equal(2, beestje.TypeId);
            Assert.Equal(15, beestje.Price);
            Assert.Equal("updated.jpg", beestje.Picture);
        }

        [Fact]
        public async Task UpdateBeestjeAsync_ShouldReturnFalse_WhenBeestjeNotFound()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var viewModel = new BeestjeViewModels { Id = 1, Name = "Cow", TypeId = 1, Price = 10 };
            var service = new BeestjeService(context);

            // Act
            var result = await service.UpdateBeestjeAsync(viewModel);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteBeestjeAsync_ShouldRemoveBeestjeAndReturnTrue()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Beestjes.Add(new Beestje { Id = 1, Name = "Cow", TypeId = 1, Price = 10, Picture = "cow.jpg" });
            await context.SaveChangesAsync();
            var service = new BeestjeService(context);

            // Act
            var result = await service.DeleteBeestjeAsync(1);

            // Assert
            Assert.True(result);
            Assert.Null(await context.Beestjes.FindAsync(1));
        }

        [Fact]
        public async Task DeleteBeestjeAsync_ShouldReturnFalse_WhenBeestjeNotFound()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var service = new BeestjeService(context);

            // Act
            var result = await service.DeleteBeestjeAsync(1);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetAllTypesAsync_ShouldReturnAllTypes()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Types.Add(new Types { Id = 1, TypeName = "Boerderij" });
            context.Types.Add(new Types { Id = 2, TypeName = "Jungle" });
            await context.SaveChangesAsync();
            var service = new BeestjeService(context);

            // Act
            var result = await service.GetAllTypesAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, t => t.TypeName == "Boerderij");
            Assert.Contains(result, t => t.TypeName == "Jungle");
        }

        [Fact]
        public async Task GetAllImageNamesAsync_ShouldReturnImageNames_WhenDirectoryExists()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var service = new BeestjeService(context);
            var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/beestjes");
            Directory.CreateDirectory(directory);
            File.WriteAllText(Path.Combine(directory, "cow.jpg"), "test");
            File.WriteAllText(Path.Combine(directory, "sheep.jpg"), "test");

            // Act
            var result = await service.GetAllImageNamesAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains("cow.jpg", result);
            Assert.Contains("sheep.jpg", result);

            // Cleanup
            Directory.Delete(directory, true);
        }

        [Fact]
        public async Task GetAllImageNamesAsync_ShouldReturnEmptyList_WhenDirectoryDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var service = new BeestjeService(context);
            var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/beestjes");
            if (Directory.Exists(directory)) Directory.Delete(directory, true);

            // Act
            var result = await service.GetAllImageNamesAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetBeestjesByIdsAsync_ShouldReturnBeestjesForGivenIds()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Types.Add(new Types { Id = 1, TypeName = "Boerderij" });
            context.Beestjes.Add(new Beestje { Id = 1, Name = "Cow", TypeId = 1, Price = 10, Picture = "cow.jpg" });
            context.Beestjes.Add(new Beestje { Id = 2, Name = "Sheep", TypeId = 1, Price = 8, Picture = "sheep.jpg" });
            await context.SaveChangesAsync();
            var service = new BeestjeService(context);
            var ids = new List<int> { 1 };

            // Act
            var result = await service.GetBeestjesByIdsAsync(ids);

            // Assert
            Assert.Single(result);
            var beestje = result[0];
            Assert.Equal(1, beestje.Id);
            Assert.Equal("Cow", beestje.Name);
            Assert.Equal("Boerderij", beestje.Type);
        }
    }
}