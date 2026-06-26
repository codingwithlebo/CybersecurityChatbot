namespace CyberBotWPF.Models;

/// <summary>
/// Represents the user session — stores name, favourite topic (memory),
/// question count, and session start time. Uses automatic properties.
/// </summary>
public class User
{
    public string Name           { get; set; } = string.Empty;
    public string FavouriteTopic { get; set; } = string.Empty;
    public int    QuestionCount  { get; set; } = 0;
    public DateTime SessionStart { get; set; } = DateTime.Now;

    public User(string name)
    {
        Name         = name;
        SessionStart = DateTime.Now;
    }

    public string GetSessionDuration()
    {
        TimeSpan d = DateTime.Now - SessionStart;
        return d.TotalMinutes < 1
            ? "less than a minute"
            : $"{(int)d.TotalMinutes} minute(s)";
    }
}
