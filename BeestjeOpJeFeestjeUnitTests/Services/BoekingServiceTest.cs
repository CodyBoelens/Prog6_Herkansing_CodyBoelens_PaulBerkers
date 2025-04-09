using Microsoft.EntityFrameworkCore;
using Prog6_Assessment_CodyBoelens.Data.DbEntities;
using Prog6_Assessment_CodyBoelens.Data;
using Prog6_Assessment_CodyBoelens.Services;
using Prog6_Assessment_CodyBoelens.Views.ViewModels.BoekingsViewModel;
using System;

namespace BeestjeOpJeFeestjeUnitTests.Services
{
    public class BoekingServiceTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique name per test
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetAllBoekingsAsync_ShouldReturnAllBookings()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Boekingen.Add(new Boeking
            {
                Id = 1,
                Date = new DateTime(2025, 5, 1),
                Name = "Test1",
                Adress = "Address1",
                Email = "test1@example.com",
                PhoneNumber = "1234567890"
            });
            context.Boekingen.Add(new Boeking
            {
                Id = 2,
                Date = new DateTime(2025, 5, 2),
                Name = "Test2",
                Adress = "Address2",
                Email = "test2@example.com",
                PhoneNumber = "0987654321"
            });
            await context.SaveChangesAsync();
            var service = new BoekingService(context);

            // Act
            var result = await service.GetAllBoekingsAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, b => b.Id == 1 && b.Name == "Test1" && b.Email == "test1@example.com");
            Assert.Contains(result, b => b.Id == 2 && b.Name == "Test2" && b.Email == "test2@example.com");
        }

        [Fact]
        public async Task GetBookingByIdAsync_ShouldReturnBooking_WhenExists()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Boekingen.Add(new Boeking
            {
                Id = 1,
                Date = new DateTime(2025, 5, 1),
                Name = "Test",
                Adress = "Test Address",
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                TotaalPrijs = 50.0
            });
            await context.SaveChangesAsync();
            var service = new BoekingService(context);

            // Act
            var result = await service.GetBookingByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Test", result.Name);
            Assert.Equal("Test Address", result.Adress);
            Assert.Equal("test@example.com", result.Email);
            Assert.Equal("1234567890", result.PhoneNumber);
            Assert.Equal(50.0, result.TotaalPrijs);
        }

        [Fact]
        public async Task GetBookingByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var service = new BoekingService(context);

            // Act
            var result = await service.GetBookingByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetBookedBeestjesByIdAsync_ShouldReturnBookedBeestjes()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Types.Add(new Types { Id = 1, TypeName = "Boerderij" });
            context.Beestjes.Add(new Beestje { Id = 1, Name = "Cow", TypeId = 1, Price = 10, Picture = "cow.jpg" });
            context.Boekingen.Add(new Boeking
            {
                Id = 1,
                Date = new DateTime(2025, 5, 1),
                Name = "Test",
                Adress = "Test Address",
                Email = "test@example.com",
                PhoneNumber = "1234567890"
            });
            context.BeestjeBoekingen.Add(new BeestjeBoeking { BeestjeID = 1, BoekingID = 1 });
            await context.SaveChangesAsync();
            var service = new BoekingService(context);

            // Act
            var result = await service.GetBookedBeestjesByIdAsync(1);

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
        public async Task GetBookedBeestjesByIdAsync_ShouldReturnEmptyList_WhenNoBeestjesBooked()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Boekingen.Add(new Boeking
            {
                Id = 1,
                Date = new DateTime(2025, 5, 1),
                Name = "Test",
                Adress = "Test Address",
                Email = "test@example.com",
                PhoneNumber = "1234567890"
            });
            await context.SaveChangesAsync();
            var service = new BoekingService(context);

            // Act
            var result = await service.GetBookedBeestjesByIdAsync(1);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task DeleteBoekingAsync_ShouldRemoveBookingAndReturnTrue()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Boekingen.Add(new Boeking
            {
                Id = 1,
                Date = new DateTime(2025, 5, 1),
                Name = "Test",
                Adress = "Test Address",
                Email = "test@example.com",
                PhoneNumber = "1234567890"
            });
            await context.SaveChangesAsync();
            var service = new BoekingService(context);

            // Act
            var result = await service.DeleteBoekingAsync(1);

            // Assert
            Assert.True(result);
            Assert.Null(await context.Boekingen.FindAsync(1));
        }

        [Fact]
        public async Task DeleteBoekingAsync_ShouldReturnFalse_WhenBookingNotFound()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var service = new BoekingService(context);

            // Act
            var result = await service.DeleteBoekingAsync(1);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AddBoekingAsync_ShouldAddBookingWithoutBeestjes()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var boekingViewModel = new BoekingViewModel
            {
                Date = new DateTime(2025, 5, 1),
                Name = "Test",
                Adress = "Test Address",
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                Is_Confirmed = true,
                TotaalPrijs = 50.0
            };
            var service = new BoekingService(context);

            // Act
            await service.AddBoekingAsync(boekingViewModel);

            // Assert
            var boeking = await context.Boekingen.SingleOrDefaultAsync(b => b.Name == "Test");
            Assert.NotNull(boeking);
            Assert.Equal("Test", boeking.Name);
            Assert.Equal("Test Address", boeking.Adress);
            Assert.Equal("test@example.com", boeking.Email);
            Assert.Equal("1234567890", boeking.PhoneNumber);
            Assert.True(boeking.Is_Confirmed);
            Assert.Equal(50.0, boeking.TotaalPrijs);
        }

        [Fact]
        public async Task AddBoekingAsync_ShouldAddBookingWithBeestjes()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Beestjes.Add(new Beestje { Id = 1, Name = "Cow", TypeId = 1, Price = 10, Picture = "cow.jpg" });
            await context.SaveChangesAsync();
            var boekingViewModel = new BoekingViewModel
            {
                Date = new DateTime(2025, 5, 1),
                Name = "Test",
                Adress = "Test Address",
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                Is_Confirmed = true,
                TotaalPrijs = 10.0,
                BeestjeIds = new List<int> { 1 }
            };
            var service = new BoekingService(context);

            // Act
            await service.AddBoekingAsync(boekingViewModel);

            // Assert
            var boeking = await context.Boekingen.SingleOrDefaultAsync(b => b.Name == "Test");
            Assert.NotNull(boeking);
            var beestjeBoeking = await context.BeestjeBoekingen.SingleOrDefaultAsync(bb => bb.BoekingID == boeking.Id);
            Assert.NotNull(beestjeBoeking);
            Assert.Equal(1, beestjeBoeking.BeestjeID);
        }
    }
}
