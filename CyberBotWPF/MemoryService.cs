namespace CyberBotWPF.Services;

/// <summary>
/// Simple in-session memory store.
/// Remembers user name, favourite topic, and any key facts they share.
/// Uses a Dictionary (generic collection) as required by the assessment.
/// </summary>
public class MemoryService
{
    // Generic collection — Dictionary<string, string>
    private readonly Dictionary<string, string> _memory = new();

    public void Store(string key, string value)
    {
        _memory[key.ToLowerInvariant()] = value;
    }

    public string? Recall(string key)
    {
        _memory.TryGetValue(key.ToLowerInvariant(), out string? value);
        return value;
    }

    public bool Has(string key) => _memory.ContainsKey(key.ToLowerInvariant());

    /// <summary>
    /// Scans user input for interest statements like
    /// "I'm interested in X" or "I like X" and stores the topic.
    /// </summary>
    public string? DetectAndStoreInterest(string input)
    {
        string lower = input.ToLowerInvariant();

        string[] triggers = [
            "interested in", "i like", "i love", "favourite topic",
            "care about", "want to learn about", "focus on"
        ];

        foreach (string trigger in triggers)
        {
            int idx = lower.IndexOf(trigger);
            if (idx >= 0)
            {
                string topic = input[(idx + trigger.Length)..].Trim().TrimEnd('.', '!', '?');
                if (topic.Length > 2)
                {
                    Store("favourite_topic", topic);
                    return topic;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Returns a personalised contextual nudge based on stored memory,
    /// or empty string if nothing is remembered.
    /// </summary>
    public string GetMemoryHint()
    {
        if (Has("favourite_topic"))
            return $"As someone interested in {Recall("favourite_topic")}, you might find this especially relevant.";
        return string.Empty;
    }
}
