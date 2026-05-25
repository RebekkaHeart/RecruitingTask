using Application.LogComponent;
using Microsoft.Extensions.Time.Testing;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

namespace Application.Tests.LogComponent;

[TestClass]
public sealed class AsyncLoggerTests
{
    private readonly string _logFolderPath = @"./../../../LogTest";

    [TestMethod]
    public async Task StopWithoutFlush_ShouldNotWriteRemainingLogs()
    {
        // Arrange
        FakeTimeProvider fakeTimeProvider = new FakeTimeProvider(DateTimeOffset.Now.AddHours(1));
        ILogger logger = new AsyncLogger(fakeTimeProvider);
        string message1 = "Test Log Message 1";
        string message2 = "Test Log Message 2";

        // Act
        logger.WriteLog(message1);
        Thread.Sleep(30);
        logger.WriteLog(message2);
        logger.StopWithoutFlush();
        Thread.Sleep(30);

        // Assert
        var file = Directory.EnumerateFiles(_logFolderPath, $"Log{fakeTimeProvider.GetLocalNow():yyyyMMdd HHmmss}*.log").LastOrDefault(); // Get the most recent log file
        Assert.IsNotNull(file);
        var lines = File.ReadLines(file).ToList();
        Assert.IsTrue(lines.Any(line => line.Contains(message1)));
        Assert.IsFalse(lines.Any(line => line.Contains(message2)));
    }

    [TestMethod]
    public async Task StopWithFlush_WaitsForLoggingToFinish()
    {
        // Arrange
        FakeTimeProvider fakeTimeProvider = new FakeTimeProvider(DateTimeOffset.Now.AddHours(2));
        ILogger logger = new AsyncLogger(fakeTimeProvider);
        string message = "Test Log Message";

        // Act
        logger.WriteLog(message);
        var stopTask = logger.StopWithFlush();

        // Assert
        Assert.IsFalse(stopTask.IsCompleted);
        await stopTask;
        Assert.IsTrue(stopTask.IsCompleted);
    }

    [TestMethod]
    public async Task WriteToLog_ShouldAddLogFile()
    {
        // Arrange
        FakeTimeProvider fakeTimeProvider = new FakeTimeProvider(DateTimeOffset.Now.AddHours(3));
        DateTimeOffset now = fakeTimeProvider.GetLocalNow();
        ILogger logger = new AsyncLogger(fakeTimeProvider);
        string message = "Test Log Message";

        // Act
        logger.WriteLog(message);
        await logger.StopWithFlush();

        // Assert
        var file = Directory.EnumerateFiles(_logFolderPath, $"Log{now:yyyyMMdd HHmmss}*.log").LastOrDefault(); // Get the most recent log file
        Assert.IsNotNull(file);
        var lines = File.ReadLines(file).ToList();
        Assert.IsTrue(lines.Any(line => line.Contains(message)));
    }

    [TestMethod]
    public async Task CrossingMidnight_ShouldCreateNewLogFile()
    {
        // Arrange
        string messageBeforeMidnight = "TestLogMessageBeforeMidnight"; // They have to be without spaces to avoid issues with splitting the log line on spaces further down
        string messageAfterMidnight = "TestLogMessageAfterMidnight";
        DateTimeOffset beforeMidnight = DateTimeOffset.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);
        DateTimeOffset afterMidnight = beforeMidnight.AddMilliseconds(2);
        FakeTimeProvider fakeTimeProvider = new FakeTimeProvider(beforeMidnight);
        ILogger logger = new AsyncLogger(fakeTimeProvider);

        // Act
        logger.WriteLog(messageBeforeMidnight);
        Thread.Sleep(10); // Wait a bit to ensure the log is written before advancing time

        fakeTimeProvider.Advance(TimeSpan.FromMilliseconds(2)); // Advance time to just after midnight
        logger.WriteLog(messageAfterMidnight);

        await logger.StopWithFlush();

        // Assert
        var fileBeforeMidnight = Directory.EnumerateFiles(_logFolderPath, $"Log{beforeMidnight:yyyyMMdd HHmmss}*.log").LastOrDefault();
        var fileAfterMidnight = Directory.EnumerateFiles(_logFolderPath, $"Log{afterMidnight:yyyyMMdd HHmmss}*.log").FirstOrDefault();
        Assert.IsNotNull(fileBeforeMidnight);
        Assert.IsNotNull(fileAfterMidnight);
        var lineBeforeMidnight = File.ReadLines(fileBeforeMidnight).LastOrDefault();
        var lineAfterMidnight = File.ReadLines(fileAfterMidnight).LastOrDefault();
        Assert.AreEqual(messageBeforeMidnight + ".", lineBeforeMidnight?.Split(' ', '\t').Skip(2).FirstOrDefault());
        Assert.AreEqual(messageAfterMidnight + ".", lineAfterMidnight?.Split(' ', '\t').Skip(2).FirstOrDefault());
    }
}
