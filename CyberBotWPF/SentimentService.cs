namespace CyberBotWPF.Services;

/// <summary>
/// Detects the user's sentiment from their input text.
/// Returns: "worried" | "curious" | "frustrated" | "positive" | "neutral"
/// </summary>
public static class SentimentService
{
    private static readonly Dictionary<string, string[]> SentimentKeywords = new()
    {
        ["worried"] = [
            "worried", "scared", "afraid", "anxious", "nervous", "concern",
            "panic", "fear", "terrified", "unsafe", "danger", "hack", "hacked",
            "breach", "stolen", "compromised", "attacked"
        ],
        ["curious"] = [
            "curious", "wonder", "how does", "what is", "tell me", "explain",
            "learn", "understand", "interesting", "know more", "find out",
            "want to know", "what about", "how can", "teach me"
        ],
        ["frustrated"] = [
            "frustrated", "annoyed", "confused", "don't understand", "doesn't work",
            "not working", "useless", "stupid", "why", "still don't", "again",
            "keep getting", "can't figure", "lost", "impossible"
        ],
        ["positive"] = [
            "thanks", "thank you", "great", "awesome", "helpful", "love",
            "amazing", "good", "nice", "perfect", "excellent", "appreciate",
            "brilliant", "cool", "wonderful"
        ]
    };

    /// <summary>Analyses input text and returns a sentiment label.</summary>
    public static string Detect(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return "neutral";

        string lower = input.ToLowerInvariant();

        foreach (var (sentiment, keywords) in SentimentKeywords)
        {
            if (keywords.Any(k => lower.Contains(k)))
                return sentiment;
        }

        return "neutral";
    }

    /// <summary>
    /// Returns an empathetic prefix to prepend to the bot's response
    /// based on the detected sentiment.
    /// </summary>
    public static string GetEmpathyPrefix(string sentiment, string userName) => sentiment switch
    {
        "worried"    => $"I completely understand your concern, {userName}. It's smart to take cybersecurity seriously. ",
        "curious"    => $"Great question, {userName}! I love your curiosity about staying safe online. ",
        "frustrated" => $"I hear you, {userName} — cybersecurity can feel overwhelming at first. Let me help clarify. ",
        "positive"   => $"Glad you're enjoying this, {userName}! ",
        _            => string.Empty
    };
}
