using Marketly.Core.Interfaces;
using Marketly.Core.Models;
using Marketly.Core.Services;
using MockQueryable.Moq;
using Moq;

[TestFixture]
public class MessageServiceTests
{
    private Mock<IApplicationRepository> _mockRepo;
    private MessageService _messageService;

    [SetUp]
    public void SetUp()
    {
        _mockRepo = new Mock<IApplicationRepository>();
        _messageService = new MessageService(_mockRepo.Object);
    }

    [Test]
    public async Task GetUserMessagesAsync_GroupsByConversationAndLabelsSenderAsMe()
    {
        var myId = "user-1";
        var otherId = "user-2";
        var messages = new List<Message>
        {
            new Message { Id = 1, SenderId = myId, ReceiverId = otherId, AdId = 10, SentOn = DateTime.Now.AddMinutes(-5) },
            new Message { Id = 2, SenderId = otherId, ReceiverId = myId, AdId = 10, SentOn = DateTime.Now }
        }.AsQueryable().BuildMock();

        _mockRepo.Setup(r => r.All<Message>()).Returns(messages);

        var result = await _messageService.GetUserMessagesAsync(myId);

        Assert.That(result.Count(), Is.EqualTo(1)); // Should group into one conversation
        Assert.That(result.First().SenderName, Is.EqualTo("User")); // Latest message was from 'otherId'
    }
}