namespace Application.LogComponent
{
    using System;
    using System.Text;

    /// <summary>
    /// An object representing a line of a log. It contains the text to be logged and the timestamp of when the log line was created.
    /// </summary>
    public class LogLine
    {
    	/// <summary>
    	/// The text to be displayed in a log line.
    	/// </summary>
    	public string Text { get; set; }

    	/// <summary>
    	/// The time of creation of the log line.
    	/// </summary>
    	public required DateTimeOffset Timestamp { get; set; }

    	public LogLine(string text, DateTimeOffset timestamp)
    	{
    		this.Text = text;
    		this.Timestamp = timestamp;
    	}

        public LogLine() { }

    	/// <summary>
    	/// Get the logged text.
    	/// </summary>
    	/// <returns>A formatted log line</returns>
    	public string GetLineText()
    	{
    		StringBuilder sb = new StringBuilder();

    		if (this.Text.Length > 0)
    		{
    			sb.Append(this.Text);
    			sb.Append(".");
    		}
            else
            {
                sb.Append("No text.");
            }

    		return sb.ToString();
    	}
    }
}