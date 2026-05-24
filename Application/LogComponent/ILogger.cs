using System.Threading.Tasks;

namespace Application.LogComponent
{
	public interface ILogger
	{
		/// <summary>
		/// Stop the logging. If there are any outstanding logs, these will not be written to the Log.
		/// </summary>
		void StopWithoutFlush();

		/// <summary>
		/// Stop the logging. The call will not return until all logs have been written to the Log.
		/// </summary>
		Task StopWithFlush();

		/// <summary>
		/// Write a message to the Log.
		/// </summary>
		/// <param name="s">The string to be written to the Log.</param>
		void WriteLog(string s);
	}
}