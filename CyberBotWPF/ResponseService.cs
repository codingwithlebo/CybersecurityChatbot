namespace CyberBotWPF.Services;

/// <summary>
/// Knowledge base with keyword matching and random response selection.
/// Uses Dictionary and List (generic collections) as required.
/// Delegate is used for the random selector.
/// </summary>
public static class ResponseService
{
    private static readonly Random Rng = new();

    // Delegate for selecting a random response — uses delegates as required
    private delegate string RandomPicker(List<string> responses);
    private static readonly RandomPicker PickRandom = responses =>
        responses[Rng.Next(responses.Count)];

    // ---------------------------------------------------------------
    // Knowledge base: keyword → multiple possible responses (random)
    // Dictionary<string, (string Topic, List<string> Responses)>
    // ---------------------------------------------------------------
    private static readonly Dictionary<string, (string Topic, List<string> Responses)> KB = new()
    {
        ["password"] = ("Password Safety", [
            "🔑 Use at least 12 characters mixing uppercase, lowercase, numbers and symbols.\n\nNever reuse passwords across different accounts — use a password manager like Bitwarden or KeePass to keep them organised.",
            "🔑 A strong password looks like: 'PurpleCoffee#Rain42'\n\nAvoid names, birthdays, or 'password123'. Enable two-factor authentication on top of your password for extra protection.",
            "🔑 Password tip: Think of a memorable phrase and take the first letter of each word.\n\n'I love eating pizza on Fridays!' → 'IlEp0F!' — strong and easy to remember."
        ]),

        ["phishing"] = ("Phishing", [
            "🎣 Phishing emails create urgency — 'Your account will be CLOSED!' — to panic you into clicking.\n\nAlways hover over links to preview the real URL before clicking. When in doubt, go directly to the website.",
            "🎣 Check the sender's email address carefully. 'support@paypa1.com' is NOT PayPal.\n\nLegitimate companies never ask for your password, PIN, or OTP via email.",
            "🎣 If you receive a suspicious email, do NOT click any links or download attachments.\n\nReport it to your IT department or forward it to phishing@reportfraud.fia.gov.za in South Africa."
        ]),

        ["scam"] = ("Scam Awareness", [
            "🚨 Common SA scams include fake SASSA payment notifications, lottery wins, and job offers.\n\nIf it sounds too good to be true — it is. Never pay upfront fees to receive a prize.",
            "🚨 Bank scams: Your bank will NEVER call asking for your card PIN, OTP, or full card number.\n\nHang up and call your bank directly using the number on the back of your card.",
            "🚨 WhatsApp scams are rising in South Africa. Be wary of messages from unknown numbers claiming to be family in an emergency asking for money."
        ]),

        ["privacy"] = ("Online Privacy", [
            "🔒 Review your social media privacy settings — limit who can see your posts, location, and personal details.\n\nNever share your ID number, banking details, or home address publicly.",
            "🔒 Use a VPN (Virtual Private Network) to encrypt your internet traffic, especially on public Wi-Fi.\n\nThis prevents attackers from intercepting your data.",
            "🔒 Think before you post! Once something is online it can be very hard to remove.\n\nBe especially careful with photos that reveal your location, workplace, or daily routine."
        ]),

        ["malware"] = ("Malware", [
            "🦠 Keep your antivirus software updated and run regular scans.\n\nNever open email attachments from unknown senders — even .pdf files can contain malware.",
            "🦠 Ransomware encrypts your files and demands payment. Protect yourself by backing up data regularly to an external drive or cloud service.",
            "🦠 Only download software from official websites. Pirated software is a common malware delivery method."
        ]),

        ["browsing"] = ("Safe Browsing", [
            "🌐 Always check for HTTPS (the padlock icon) before entering any personal or payment information.\n\nHTTP sites are not encrypted — your data can be intercepted.",
            "🌐 Keep your browser updated. Browser updates patch security vulnerabilities that attackers exploit.",
            "🌐 Be cautious of pop-ups claiming 'Your PC is infected! Call this number!'\n\nThese are scareware — close the tab immediately."
        ]),

        ["social engineering"] = ("Social Engineering", [
            "🕵️ Attackers often impersonate IT support, bank officials, or government workers.\n\nAlways verify identity independently — call back on an official number you find yourself.",
            "🕵️ 'Baiting' involves leaving infected USB drives in public spaces hoping someone plugs them in.\n\nNever use a USB drive you find or receive unexpectedly.",
            "🕵️ Pretexting is when an attacker creates a fake scenario to manipulate you.\n\nTrust your instincts — if something feels off, verify before sharing any information."
        ]),

        ["2fa"] = ("Two-Factor Authentication", [
            "🔐 Enable 2FA on all important accounts — banking, email, and social media.\n\nUse an authenticator app (Google Authenticator or Authy) rather than SMS for stronger protection.",
            "🔐 2FA means even if a hacker steals your password, they still cannot access your account without the second factor.",
            "🔐 Store your 2FA backup codes somewhere safe and offline.\n\nNever share a 2FA code with anyone — no legitimate service will ever ask for it."
        ]),

        ["wifi"] = ("Public Wi-Fi", [
            "📶 Never do online banking or shopping on public Wi-Fi — attackers can intercept your traffic.\n\nUse your mobile data or a VPN for sensitive tasks.",
            "📶 Beware of 'Evil Twin' hotspots — fake Wi-Fi networks named after real ones (e.g. 'OR Tambo Airport Free WiFi').\n\nAlways confirm the official network name with staff.",
            "📶 Turn off Wi-Fi auto-connect on your phone to prevent it joining unknown networks automatically."
        ]),

        ["how are you"] = ("Status", [
            "I'm fully operational and ready to keep you cyber-safe! 🛡️",
            "Running smoothly! Every conversation makes South Africa a little safer online. 🇿🇦",
            "All systems go! I'm here and ready to help you navigate the cyber world safely. 💻"
        ]),

        ["purpose"] = ("About Me", [
            "I'm your Cybersecurity Awareness Assistant 🛡️\n\nI help South African citizens identify and avoid cyber threats like phishing, malware, and social engineering. Ask me anything!"
        ]),

        ["help"] = ("Topics", [
            "Here are topics I can help with:\n\n🔑 password  |  🎣 phishing  |  🚨 scam\n🔒 privacy   |  🦠 malware   |  🌐 browsing\n🕵️ social engineering  |  🔐 2fa  |  📶 wifi\n\nJust type any topic or ask a question!"
        ])
    };

    // Follow-up keywords that continue the current topic
    private static readonly string[] FollowUpKeywords =
        ["more", "tell me more", "explain more", "elaborate", "another tip",
         "give me more", "continue", "go on", "and", "what else", "expand"];

    /// <summary>
    /// Main response lookup. Returns (topic, responseText) or null if unrecognised.
    /// Uses the RandomPicker delegate to select from multiple responses.
    /// </summary>
    public static (string Topic, string Text)? GetResponse(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;
        string lower = input.ToLowerInvariant();

        foreach (var (keyword, (topic, responses)) in KB)
        {
            if (lower.Contains(keyword))
                return (topic, PickRandom(responses));
        }

        return null;
    }

    /// <summary>Returns true if the input is a follow-up request.</summary>
    public static bool IsFollowUp(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;
        string lower = input.ToLowerInvariant();
        return FollowUpKeywords.Any(k => lower.Contains(k));
    }

    /// <summary>Returns a new random tip for the given topic (for follow-ups).</summary>
    public static string GetFollowUpResponse(string topic)
    {
        string lowerTopic = topic.ToLowerInvariant();
        foreach (var (keyword, (kbTopic, responses)) in KB)
        {
            if (lowerTopic.Contains(keyword) || keyword.Contains(lowerTopic))
                return PickRandom(responses);
        }
        return "Here's a general tip: always keep your software updated and use strong, unique passwords for every account. 🛡️";
    }

    public static bool IsExitCommand(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;
        string[] exits = ["exit", "quit", "bye", "goodbye", "close"];
        return exits.Any(e => input.ToLowerInvariant().Contains(e));
    }
}
