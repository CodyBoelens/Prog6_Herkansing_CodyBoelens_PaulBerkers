using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using Prog6_Assessment_CodyBoelens.Data;
using Prog6_Assessment_CodyBoelens.Data.DbEntities;
using Prog6_Assessment_CodyBoelens.Services;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.KlantViewModel;

namespace BeestjeOpJeFeestjeUnitTests.Services
{
    public class KlantServiceTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        private Mock<UserManager<IdentityUser>> GetMockUserManager()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            return new Mock<UserManager<IdentityUser>>(
                store.Object, null, null, null, null, null, null, null, null);
        }

        [Fact]
        public void GetKlantViewModels_ShouldReturnAllKlantenWithRanks()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var userManager = GetMockUserManager();
            context.Klanten.Add(new Klant { Id = 1, Name = "John", Adres = "Address1", KlantkaartId = 1, ApplicationUserId = "user1" });
            context.Klantkaarten.Add(new Klantkaart { Id = 1, Rank = "Gold" });
            context.SaveChanges();
            var service = new KlantService(context, userManager.Object);

            // Act
            var result = service.GetKlantViewModels();

            // Assert
            Assert.Single(result);
            var klant = result[0];
            Assert.Equal(1, klant.Id);
            Assert.Equal("John", klant.Name);
            Assert.Equal("Address1", klant.Adres);
            Assert.Equal("Gold", klant.Rank);
        }

        [Fact]
        public void GetKlantViewModels_ShouldReturnGeenKlantkaart_WhenNoKlantkaart()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var userManager = GetMockUserManager();
            context.Klanten.Add(new Klant { Id = 1, Name = "John", Adres = "Address1", KlantkaartId = 999, ApplicationUserId = "user1" });
            context.SaveChanges();
            var service = new KlantService(context, userManager.Object);

            // Act
            var result = service.GetKlantViewModels();

            // Assert
            Assert.Single(result);
            Assert.Equal("Geen Klantkaart", result[0].Rank);
        }

        [Fact]
        public void GetAllKlantkaarten_ShouldReturnAllKlantkaarten()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var userManager = GetMockUserManager();
            context.Klantkaarten.Add(new Klantkaart { Id = 1, Rank = "Gold" });
            context.Klantkaarten.Add(new Klantkaart { Id = 2, Rank = "Silver" });
            context.SaveChanges();
            var service = new KlantService(context, userManager.Object);

            // Act
            var result = service.GetAllKlantkaarten();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, k => k.Rank == "Gold");
            Assert.Contains(result, k => k.Rank == "Silver");
        }

        [Fact]
        public async Task CreateKlantAsync_ShouldCreateKlantAndUser_ReturnPassword()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var userManager = GetMockUserManager();
            userManager.Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            userManager.Setup(um => um.AddToRoleAsync(It.IsAny<IdentityUser>(), "Klant"))
                .ReturnsAsync(IdentityResult.Success);
            var klantViewModel = new KlantViewModels
            {
                Name = "John",
                Adres = "Address1",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                KlantkaartId = 1
            };
            var service = new KlantService(context, userManager.Object);

            // Act
            var password = await service.CreateKlantAsync(klantViewModel);

            // Assert
            Assert.NotNull(password);
            Assert.True(password.Length >= 6);
            var klant = context.Klanten.SingleOrDefault(k => k.Name == "John");
            Assert.NotNull(klant);
            Assert.Equal("Address1", klant.Adres);
            Assert.Equal(1, klant.KlantkaartId);
            userManager.Verify(um => um.CreateAsync(It.Is<IdentityUser>(u => u.Email == "john@example.com"), It.IsAny<string>()), Times.Once());
            userManager.Verify(um => um.AddToRoleAsync(It.IsAny<IdentityUser>(), "Klant"), Times.Once());
        }

        [Fact]
        public async Task CreateKlantAsync_ShouldThrowException_WhenUserCreationFails()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var userManager = GetMockUserManager();
            userManager.Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Email already taken" }));
            var klantViewModel = new KlantViewModels
            {
                Name = "John",
                Email = "john@example.com"
            };
            var service = new KlantService(context, userManager.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => service.CreateKlantAsync(klantViewModel));
            Assert.Equal("Email already taken", exception.Message);
        }

        [Fact]
        public void GetKlantById_ShouldReturnKlant_WhenExists()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var userManager = GetMockUserManager();
            var user = new IdentityUser { Id = "user1", Email = "john@example.com", PhoneNumber = "1234567890" };
            userManager.Setup(um => um.FindByIdAsync("user1")).ReturnsAsync(user);
            context.Klanten.Add(new Klant { Id = 1, Name = "John", Adres = "Address1", KlantkaartId = 1, ApplicationUserId = "user1" });
            context.Klantkaarten.Add(new Klantkaart { Id = 1, Rank = "Gold" });
            context.SaveChanges();
            var service = new KlantService(context, userManager.Object);

            // Act
            var result = service.GetKlantById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("John", result.Name);
            Assert.Equal("Address1", result.Adres);
            Assert.Equal("john@example.com", result.Email);
            Assert.Equal("1234567890", result.PhoneNumber);
            Assert.Equal(1, result.KlantkaartId);
            Assert.Single(result.allRanks);
        }

        [Fact]
        public void GetKlantById_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var userManager = GetMockUserManager();
            var service = new KlantService(context, userManager.Object);

            // Act
            var result = service.GetKlantById(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetKlantByApplicationUserId_ShouldReturnKlant_WhenExists()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var userManager = GetMockUserManager();
            var user = new IdentityUser { Id = "user1", Email = "john@example.com", PhoneNumber = "1234567890" };
            userManager.Setup(um => um.FindByIdAsync("user1")).ReturnsAsync(user);
            context.Klanten.Add(new Klant { Id = 1, Name = "John", Adres = "Address1", KlantkaartId = 1, ApplicationUserId = "user1" });
            context.Klantkaarten.Add(new Klantkaart { Id = 1, Rank = "Gold" });
            context.SaveChanges();
            var service = new KlantService(context, userManager.Object);

            // Act
            var result = service.GetKlantByApplicationUserId("user1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("John", result.Name);
            Assert.Equal("Address1", result.Adres);
            Assert.Equal("john@example.com", result.Email);
            Assert.Equal("1234567890", result.PhoneNumber);
            Assert.Equal(1, result.KlantkaartId);
            Assert.Single(result.allRanks);
        }

        [Fact]
        public void GetKlantByApplicationUserId_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var userManager = GetMockUserManager();
            var service = new KlantService(context, userManager.Object);

            // Act
            var result = service.GetKlantByApplicationUserId("user1");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateKlantAsync_ShouldUpdateKlantAndReturnTrue()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var userManager = GetMockUserManager();
            context.Klanten.Add(new Klant { Id = 1, Name = "John", Adres = "Address1", KlantkaartId = 1, ApplicationUserId = "user1" });
            context.SaveChanges();
            var viewModel = new KlantViewModels { Id = 1, Name = "Updated John", Adres = "Updated Address", KlantkaartId = 2 };
            var service = new KlantService(context, userManager.Object);

            // Act
            var result = await service.UpdateKlantAsync(viewModel);

            // Assert
            Assert.True(result);
            var klant = await context.Klanten.FindAsync(1);
            Assert.Equal("Updated John", klant.Name);
            Assert.Equal("Updated Address", klant.Adres);
            Assert.Equal(2, klant.KlantkaartId);
        }

        [Fact]
        public async Task UpdateKlantAsync_ShouldReturnFalse_WhenKlantNotFound()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var userManager = GetMockUserManager();
            var viewModel = new KlantViewModels { Id = 1, Name = "John", Adres = "Address1" };
            var service = new KlantService(context, userManager.Object);

            // Act
            var result = await service.UpdateKlantAsync(viewModel);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GeneratePassword_ShouldReturnPasswordWithMinimumLengthAndRequirements()
        {
            // Act
            var password = KlantService.GeneratePassword(8);

            // Assert
            Assert.Equal(9, password.Length);
            Assert.True(password.Any(char.IsUpper));
            Assert.True(password.Any(char.IsLower));
            Assert.True(password.Any(char.IsDigit));
            Assert.True(password.Any(c => "!@#$%^&*()-_=+[]{}|;:'\",.<>/?".Contains(c)));
        }
    }
}