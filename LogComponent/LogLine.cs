namespace LogComponent
{
    using System;
    using System.Text;

    /// <summary>
    /// This is the object that the diff. loggers (filelogger, consolelogger etc.) will operate on. The LineText() method will be called to get the text (formatted) to log
    /// </summary>
    public class LogLine
    {
    	/// <summary>
    	/// The text to be displayed in a log line
    	/// </summary>
    	public string Text { get; set; }

    	/// <summary>
    	/// The Timestamp of a log. It is initialized when the log is added.
    	/// </summary>
    	public virtual DateTime Timestamp { get; set; }

    	public LogLine()
    	{
    		this.Text = "";
    	}

    	/// <summary>
    	/// Get the logged text.
    	/// </summary>
    	/// <returns>A formatted log line</returns>
    	public virtual string LineText()
    	{
    		StringBuilder sb = new StringBuilder();

    		if (this.Text.Length > 0)
    		{
    			sb.Append(this.Text);
    			sb.Append(". ");
    		}

    		sb.Append(this.CreateLineText());

    		return sb.ToString();
    	}

    	public virtual string CreateLineText()
    	{
    		return "";
    	}
    }
}