using Game;
using Moq;
using NUnit.Framework;
using UserCommunication;

namespace DiceRollGameTests;

[TestFixture]
public class GuessingGamesTests{
    private Mock<IDice> _diceMock;
    private Mock<IUserCommunication> _userCommunicationMock;
    private GuessingGame _cut;

    [SetUp]
    public void SetUp(){
        _userCommunicationMock = new Mock<IUserCommunication>();
        _diceMock = new Mock<IDice>();
        _cut = new GuessingGame(
            _diceMock.Object,
            _userCommunicationMock.Object
        );
    }

    [Test]
    public void Play_ReturnsVictory_IfUserGuessesRightOnFirstTry(){
        const int rollReturnValue = 3;
        _diceMock
        .Setup( mock => mock.Roll())
        .Returns(rollReturnValue);
        _userCommunicationMock
        .Setup(mock => mock.ReadInteger(It.IsAny<string>()))
        .Returns(rollReturnValue);

        var gameResult = _cut.Play();

        Assert.AreEqual(GameResult.Victory, gameResult);
    }

    [Test]
    public void Play_ReturnsLoss_IfUserNeverGuessesRight(){
        const int rollReturnValue = 3;
        _diceMock
        .Setup( mock => mock.Roll())
        .Returns(rollReturnValue);
        _userCommunicationMock
        .Setup(mock => mock.ReadInteger(It.IsAny<string>()))
        .Returns(1);

        var gameResult = _cut.Play();

        Assert.AreEqual(GameResult.Loss, gameResult);
    }

    [Test]
        public void Play_ReturnsVictory_IfUserGuessesRightOnThirdTry(){
        const int rollReturnValue = 3;
        _diceMock
        .Setup( mock => mock.Roll())
        .Returns(rollReturnValue);
        _userCommunicationMock
        .SetupSequence(mock => mock.ReadInteger(It.IsAny<string>()))
        .Returns(rollReturnValue-2)
        .Returns(rollReturnValue-1)
        .Returns(rollReturnValue);
        var gameResult = _cut.Play();

        Assert.AreEqual(GameResult.Victory, gameResult);
    }
    [Test]
        public void Play_ReturnsLoss_IfUserGuessesRightOnFourthTry(){
        const int rollReturnValue = 3;
        _diceMock
        .Setup( mock => mock.Roll())
        .Returns(rollReturnValue);
        _userCommunicationMock
        .SetupSequence(mock => mock.ReadInteger(It.IsAny<string>()))
        .Returns(rollReturnValue-2)
        .Returns(rollReturnValue-1)
        .Returns(rollReturnValue+1)
        .Returns(rollReturnValue);
        var gameResult = _cut.Play();

        Assert.AreEqual(GameResult.Loss, gameResult);
    }

    [TestCase(GameResult.Victory, "You win!")]
    [TestCase(GameResult.Loss, "You lose :(")]
    public void PrintResult_ReturnsCorrespondingMessage_ForGameResult(
        GameResult gameResult, string expectedMessage)
    {
        _cut.PrintResult(gameResult);
        _userCommunicationMock
        .Verify( mock=> mock.ShowMessage(expectedMessage)); 
    }

}
