using Marketly.Core.Interfaces;
using Marketly.Core.Models;
using Marketly.Core.Services;
using MockQueryable.Moq;
using Moq;

[TestFixture]
public class CategoryServiceTests
{
    private Mock<IApplicationRepository> _mockRepo;
    private CategoryService _categoryService;

    [SetUp]
    public void SetUp()
    {
        _mockRepo = new Mock<IApplicationRepository>();
        _categoryService = new CategoryService(_mockRepo.Object);
    }

    [Test]
    public async Task DeleteAsync_ReturnsFalse_WhenAdsExistInCategory()
    {
        var category = new Category { Id = 1, Ads = new List<Ad> { new Ad() } };
        var categories = new List<Category> { category }.AsQueryable().BuildMock();
        _mockRepo.Setup(r => r.All<Category>()).Returns(categories);

        var result = await _categoryService.DeleteAsync(1);

        Assert.IsFalse(result);
        _mockRepo.Verify(r => r.Delete(It.IsAny<Category>()), Times.Never);
    }

    [Test]
    public async Task GetAllForAdminAsync_MapsAdCountCorrectly()
    {
        var categories = new List<Category>
        {
            new Category { Name = "Tech", Ads = new List<Ad> { new Ad(), new Ad() } }
        }.AsQueryable().BuildMock();
        _mockRepo.Setup(r => r.All<Category>()).Returns(categories);

        var result = await _categoryService.GetAllForAdminAsync();

        Assert.That(result.First().AdCount, Is.EqualTo(2));
    }
}