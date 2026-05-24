using System;
using System.Threading;
using System.Threading.Tasks;
using Application.LogComponent;

namespace Application
{
	class Program
	{
		static async Task Main(string[] args)
		{
			TimeProvider timeProvider = TimeProvider.System;

			ILogger logger1 = new AsyncLogger(timeProvider);

			for (int i = 0; i < 15; i++)
			{
				logger1.WriteLog("Number with flush: " + i.ToString());
				Thread.Sleep(50);
			}

			var logger1Thread = Task.Run(() => logger1.StopWithFlush());

			ILogger logger2 = new AsyncLogger(timeProvider);

			for (int i = 50; i > 0; i--)
			{
				logger2.WriteLog("Number with no flush: " + i.ToString());
				Thread.Sleep(20);
			}

			logger2.StopWithoutFlush();

			await logger1Thread;

			//Console.ReadLine();
		}
	}
}