namespace CyberBotWPF.Models;

/// <summary>
/// Represents a single message in the chat history.
/// Sender = "user" | "bot" | "system"
/// </summary>
public class ChatMessage
{
    public string Sender    { get; set; } = string.Empty;
    public string Text      { get; set; } = string.Empty;
    public string Topic     { get; set; } = string.Empty;
    public string Sentiment { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.Now;

    public ChatMessage(string sender, string text, string topic = "", string sentiment = "")
    {
        Sender    = sender;
        Text      = text;
        Topic     = topic;
        Sentiment = sentiment;
        Timestamp = DateTime.Now;
    }
}
