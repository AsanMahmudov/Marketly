using Marketly.Core.Interfaces;
using Marketly.Core.Models;
using Marketly.Core.Services;
using Marketly.Core.ViewModels;
using Microsoft.AspNetCore.Hosting;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;

namespace Tests.UnitTests
{
    [TestFixture]
    public class AdServiceTests
    {
        private Mock<IApplicationRepository> _mockRepo;
        private Mock<IWebHostEnvironment> _mockEnv;
        private AdService _adService;

        [SetUp]
        public void SetUp()
        {
            _mockRepo = new Mock<IApplicationRepository>();
            _mockEnv = new Mock<IWebHostEnvironment>();
            _adService = new AdService(_mockRepo.Object, _mockEnv.Object);
        }

        [Test]
        public async Task AllAsync_FiltersBySearchTerm_InTitleAndDescription()
        {
            var ads = new List<Ad>
            {
                new Ad { Id = 1, Title = "Gaming Laptop", Description = "Great condition", IsActive = true, Category = new Category(), Seller = new ApplicationUser(), Images = new List<Image>() },
                new Ad { Id = 2, Title = "Office Chair", Description = "Black color", IsActive = true, Category = new Category(), Seller = new ApplicationUser(), Images = new List<Image>() }
            }.AsQueryable().BuildMock();

            _mockRepo.Setup(r => r.All<Ad>()).Returns(ads);

            // Test searching in Title
            var resultTitle = await _adService.AllAsync(null, "Gaming", 1, 10);
            // Test searching in Description
            var resultDesc = await _adService.AllAsync(null, "color", 1, 10);

            Assert.That(resultTitle.TotalAds, Is.EqualTo(1));
            Assert.That(resultDesc.TotalAds, Is.EqualTo(1));
        }

        [Test]
        public async Task AllAsync_CalculatesPaginationCorrectly()
        {
            // Arrange
            var data = new List<Ad>
    {
        new Ad
        {
            IsActive = true,
            Category = new Category { Name = "Tech" },
            Seller = new ApplicationUser { FirstName = "John", LastName = "Doe" },
            Images = new List<Image>(),
            CreatedOn = DateTime.Now
        },
        new Ad
        {
            IsActive = true,
            Category = new Category { Name = "Tech" },
            Seller = new ApplicationUser { FirstName = "Jane", LastName = "Smith" }, 
            Images = new List<Image>(),
            CreatedOn = DateTime.Now
        },
        new Ad
        {
            IsActive = true,
            Category = new Category { Name = "Tech" },
            Seller = new ApplicationUser { FirstName = "Bob", LastName = "Jones" }, 
            Images = new List<Image>(),
            CreatedOn = DateTime.Now
        }
    };

            var mockAds = data.AsQueryable().BuildMock();
            _mockRepo.Setup(r => r.All<Ad>()).Returns(mockAds);

            // Act
            var result = await _adService.AllAsync(null, null, 1, 2);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.TotalPages, Is.EqualTo(2));
                Assert.That(result.Ads.Count(), Is.EqualTo(2));
            });
        }

        [Test]
        public async Task DeleteAsync_SetsIsActiveToFalse_InsteadOfRemovingFromDb()
        {
            var ad = new Ad { Id = 1, IsActive = true };
            var ads = new List<Ad> { ad }.AsQueryable().BuildMock();
            _mockRepo.Setup(r => r.All<Ad>()).Returns(ads);

            await _adService.DeleteAsync(1);

            Assert.IsFalse(ad.IsActive);
            _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}