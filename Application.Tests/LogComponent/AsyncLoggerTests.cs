using Application.LogComponent;
using Microsoft.Extensions.Time.Testing;

namespace Application.Tests.LogComponent;

[TestClass]
public sealed class AsyncLoggerTests
{
    [TestMethod]
    public void WritingToLogger_ShouldAddLogFile()
    {
        // Arrange
        TimeProvider timeProvider = TimeProvider.System;
        DateTimeOffset now = timeProvider.GetLocalNow();
        ILogger logger = new AsyncLogger(timeProvider);
        string message = "TestLogMessage"; // it has to be without spaces, because the file data gets split by spaces

        // Act
        logger.WriteLog(message);
        logger.Stop_With_Flush();

        Thread.Sleep(1000);

        // Assert
        string logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "LogTest");
        var file = Directory.EnumerateFiles(logFilePath, $"Log{now:yyyyMMdd HHmmss}*.log").LastOrDefault(); // Get the most recent log file
        Assert.IsNotNull(file);
        var line = File.ReadLines(file).LastOrDefault();
        Assert.AreEqual(message + ".", line?.Split(' ', '\t').Skip(2).FirstOrDefault());
    }

    [TestMethod]
    public void CrossingMidnight_ShouldCreateNewLogFile()
    {
        // Arrange
        string messageBeforeMidnight = "TestLogMessageBeforeMidnight";
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

        logger.Stop_With_Flush();
        Thread.Sleep(1000);

        // Assert
        string logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "LogTest");
        var fileBeforeMidnight = Directory.EnumerateFiles(logFilePath, $"Log{beforeMidnight:yyyyMMdd HHmmss}*.log").LastOrDefault();
        var fileAfterMidnight = Directory.EnumerateFiles(logFilePath, $"Log{afterMidnight:yyyyMMdd HHmmss}*.log").FirstOrDefault();
        Assert.IsNotNull(fileBeforeMidnight);
        Assert.IsNotNull(fileAfterMidnight);
        var lineBeforeMidnight = File.ReadLines(fileBeforeMidnight).LastOrDefault();
        var lineAfterMidnight = File.ReadLines(fileAfterMidnight).LastOrDefault();
        Assert.AreEqual(messageBeforeMidnight + ".", lineBeforeMidnight?.Split(' ', '\t').Skip(2).FirstOrDefault());
        Assert.AreEqual(messageAfterMidnight + ".", lineAfterMidnight?.Split(' ', '\t').Skip(2).FirstOrDefault());
    }
}
