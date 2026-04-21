using Marketly.Core.Interfaces;
using Marketly.Core.Models;
using Marketly.Core.Services;
using MockQueryable.Moq;
using Moq;

[TestFixture]
public class UserServiceTests
{
    private Mock<IApplicationRepository> _mockRepo;
    private UserService _userService;

    [SetUp]
    public void SetUp()
    {
        _mockRepo = new Mock<IApplicationRepository>();
        _userService = new UserService(_mockRepo.Object);
    }

    [Test]
    public async Task AddToFavoritesAsync_DoesNotDuplicateEntries()
    {
        var userId = "user-1";
        var adId = 1;
        var existing = new List<UserAd> { new UserAd { UserId = userId, AdId = adId } }.AsQueryable().BuildMock();

        _mockRepo.Setup(r => r.All<UserAd>()).Returns(existing);

        await _userService.AddToFavoritesAsync(userId, adId);

        // Verify AddAsync was never called because it already exists
        _mockRepo.Verify(r => r.AddAsync(It.IsAny<UserAd>()), Times.Never);
    }
}