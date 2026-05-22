using Application.LogComponent;

namespace Application.Tests.LogComponent;

[TestClass]
public sealed class AsyncLoggerTests
{
    [TestMethod]
    public void WritingToLogger_ShouldAddLogFile()
    {
        // Arrange
        DateTime now = DateTime.Now;
        ILogger logger = new AsyncLogger();
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
}
