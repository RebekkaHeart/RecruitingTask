namespace Application.LogComponent
{
	using System;
	using System.Collections.Concurrent;
	using System.IO;
	using System.Text;
	using System.Threading;
    using System.Threading.Tasks;

    public class AsyncLogger : ILogger
	{
		private Thread _runThread;

		private ConcurrentQueue<LogLine> _lines = new ConcurrentQueue<LogLine>();

		private StreamWriter _writer;

		private bool _exit;

		private bool _QuitWithFlush = false;

		private TimeProvider _timeProvider;

		private DateTimeOffset _curDate;

		private readonly string _logFolderPath = @"./../../../LogTest"; // if run in Visual Studio

        //private readonly string _logFolderPath = @"./LogTest"; // if run in command line

        /// <summary>
        /// An asynchronous logger that writes log lines to a file. The logger runs in a separate thread and uses a concurrent queue to store
        /// log lines until they are written to the file. The logger can be stopped with or without flushing the remaining log lines to the file.
        /// </summary>
        public AsyncLogger(TimeProvider timeProvider)
		{
            if (!Directory.Exists(this._logFolderPath))
				Directory.CreateDirectory(this._logFolderPath);

			this._timeProvider = timeProvider;
			this._curDate = this._timeProvider.GetLocalNow();

			SetUpWriter();

			this._runThread = new Thread(this.MainLoop);
			this._runThread.Start();
		}

        /// <summary>
        /// The main loop of the logger that runs in a separate thread. It continuously checks for new log lines in the concurrent queue and writes them to the file.
        /// </summary>
        private void MainLoop()
		{
			while (!this._exit)
			{
				if (!this._lines.IsEmpty)
				{
					while (this._lines.TryDequeue(out LogLine logLine))
					{
						if (!this._exit || this._QuitWithFlush)
						{
							StringBuilder stringBuilder = new StringBuilder();

							if ((this._timeProvider.GetLocalNow().Day - this._curDate.Day) != 0)
							{
								this._curDate = this._timeProvider.GetLocalNow();

								this._writer?.Dispose();
								SetUpWriter();

								this._writer.Write(stringBuilder.ToString());
							}

							stringBuilder.Append(logLine.Timestamp.ToString("yyyy-MM-dd HH:mm:ss:fff"));
							stringBuilder.Append("\t");
							stringBuilder.Append(logLine.GetLineText());
							stringBuilder.Append("\t");

							stringBuilder.Append(Environment.NewLine);

							this._writer.Write(stringBuilder.ToString());
						}
					}

					Thread.Sleep(50); // Sleep for a short time so that new log lines can be enqueued if there are more

					if (this._QuitWithFlush == true && this._lines.IsEmpty)
					{
						this._exit = true;
					}
				}
			}

            this._writer?.Dispose(); // Ensure the StreamWriter is properly disposed when the logger is stopped.
        }

		/// <summary>
		/// Sets up the StreamWriter for a new log file and writes the first line which is a header that names the columns.
		/// </summary>
		private void SetUpWriter()
		{
            this._writer = File.AppendText(this._logFolderPath + "/Log" + this._timeProvider.GetLocalNow().ToString("yyyyMMdd HHmmss fff") + ".log");

            this._writer.Write("Timestamp".PadRight(25, ' ') + "\t" + "Text".PadRight(15, ' ') + "\t" + Environment.NewLine);

            this._writer.AutoFlush = true;
        }

		public void StopWithoutFlush()
		{
			this._exit = true;
		}

		public async Task StopWithFlush()
		{
			this._QuitWithFlush = true;

			await (this._runThread != null ? Task.Run(() => this._runThread.Join()) : Task.CompletedTask);
        }

		public void WriteLog(string s)
		{
			this._lines.Enqueue(new LogLine() { Text = s, Timestamp = this._timeProvider.GetLocalNow() });
		}
    }
}