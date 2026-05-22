namespace Application.LogComponent
{
	using System;
	using System.Collections.Concurrent;
    using System.Collections.Generic;
	using System.IO;
	using System.Text;
	using System.Threading;

    public class AsyncLogger : ILogger
	{
		private Thread _runThread;

		private ConcurrentQueue<LogLine> _lines = new ConcurrentQueue<LogLine>();

		private StreamWriter _writer;

		private bool _exit;

		private bool _QuitWithFlush = false;

		private DateTime _curDate = DateTime.Now;

        /// <summary>
        /// An asynchronous logger that writes log lines to a file. The logger runs in a separate thread and uses a concurrent queue 
        /// to store log lines until they are written to the file. The logger can be stopped with or without flushing the remaining log lines to the file.
        /// </summary>
        public AsyncLogger()
		{
            if (!Directory.Exists(@"./LogTest"))
				Directory.CreateDirectory(@"./LogTest");

			this._writer = File.AppendText(@"./LogTest/Log" + DateTime.Now.ToString("yyyyMMdd HHmmss fff") + ".log");

			this._writer.Write("Timestamp".PadRight(25, ' ') + "\t" + "Data".PadRight(15, ' ') + "\t" + Environment.NewLine);

			this._writer.AutoFlush = true;

			this._runThread = new Thread(this.MainLoop);
			this._runThread.Start();
		}

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

							if ((DateTime.Now - _curDate).Days != 0)
							{
								_curDate = DateTime.Now;

								this._writer = File.AppendText(@"./LogTest/Log" + DateTime.Now.ToString("yyyyMMdd HHmmss fff") + ".log");

								this._writer.Write("Timestamp".PadRight(25, ' ') + "\t" + "Data".PadRight(15, ' ') + "\t" + Environment.NewLine);

								stringBuilder.Append(Environment.NewLine);

								this._writer.Write(stringBuilder.ToString());

								this._writer.AutoFlush = true;
							}

							stringBuilder.Append(logLine.Timestamp.ToString("yyyy-MM-dd HH:mm:ss:fff"));
							stringBuilder.Append("\t");
							stringBuilder.Append(logLine.LineText());
							stringBuilder.Append("\t");

							stringBuilder.Append(Environment.NewLine);

							this._writer.Write(stringBuilder.ToString());
						}
					}

					if (this._QuitWithFlush == true && this._lines.IsEmpty) { 
						this._exit = true;
					} 
					else if (!this._exit)
					{
                        Thread.Sleep(50);
                    }
				}
			}

            this._writer?.Dispose(); // Ensure the StreamWriter is properly disposed when the logger is stopped.
        }

		public void Stop_Without_Flush()
		{
			this._exit = true;
		}

		public void Stop_With_Flush()
		{
			this._QuitWithFlush = true;
		}

		public void WriteLog(string s)
		{
			this._lines.Enqueue(new LogLine() {Text = s, Timestamp = DateTime.Now});
		}
	}
}