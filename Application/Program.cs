using System;
using System.Threading;
using Application.LogComponent;

namespace Application
{
	class Program
	{
		static void Main(string[] args)
		{
			TimeProvider timeProvider = TimeProvider.System;

			ILogger logger = new AsyncLogger(timeProvider);

			for (int i = 0; i < 15; i++)
			{
				logger.WriteLog("Number with flush: " + i.ToString());
				Thread.Sleep(50);
			}

			logger.Stop_With_Flush();

			ILogger logger2 = new AsyncLogger(timeProvider);

			for (int i = 50; i > 0; i--)
			{
				logger2.WriteLog("Number with no flush: " + i.ToString());
				Thread.Sleep(20);
			}

			logger2.Stop_Without_Flush();

			//Console.ReadLine();
		}
	}
}