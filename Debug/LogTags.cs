

/// <summary>
/// Static class containing formatted strings from tagging log files to make them easier to filter in debug console.
/// Usage: Debug.Log(LogTags.GAMEPLAY_EVENT + "Player activated chest");
/// </summary>
public static class LogTags
{
	public static string SYSTEM { get { return "<b><COLOR=#0033cc>[SYSTEM]</COLOR></b> ";  } }
	public static string SYSTEM_ERROR { get { return "<b><COLOR=#ff0000>[SYSTEM_ERROR]</COLOR></b> "; } }
	public static string GAMEPLAY_EVENT { get { return "<b><COLOR=#006600>[GAMEPLAY_EVENT]</COLOR></b> "; } }
	public static string CONSOLE { get { return "<b><COLOR=#006600>[CONSOLE]</COLOR></b> "; } }

}
