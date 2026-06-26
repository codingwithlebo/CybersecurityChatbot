using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CyberBotWPF.Services;

public static class NlpService
{
    private static readonly Dictionary<string, string[]> IntentMap = new()
    {
        ["quiz"] = [
            "quiz", "test me", "test my knowledge", "play game",
            "start game", "mini game", "challenge me", "question me",
            "let's play", "play quiz", "start quiz", "game"
        ],
        ["tasks"] = [
            "show tasks", "my tasks", "view tasks", "list tasks",
            "what tasks", "pending tasks", "task list", "show my tasks",
            "what do i need to do", "my to do", "todo"
        ],
        ["add_task"] = [
            "add task", "create task", "new task", "add a task",
            "remind me", "set reminder", "add reminder",
            "i need to", "task to", "can you remind"
        ],
        ["activity_log"] = [
            "activity log", "show log", "what have you done",
            "recent actions", "show history", "view log",
            "what have you done for me", "show activity"
        ],
        ["password"] = [
            "password", "passwords", "passphrase", "strong password",
            "how to make password", "create password", "secure password"
        ],
        ["phishing"] = [
            "phishing", "phish", "fake email", "scam email",
            "suspicious email", "spam", "email scam"
        ],
        ["malware"] = [
            "malware", "virus", "ransomware", "trojan",
            "spyware", "antivirus", "infected"
        ],
        ["privacy"] = [
            "privacy", "private", "personal data", "data protection",
            "popia", "gdpr", "personal information"
        ],
        ["2fa"] = [
            "two factor", "two-factor", "2fa", "mfa",
            "multi factor", "authenticator", "verification code"
        ],
        ["wifi"] = [
            "wifi", "wi-fi", "public wifi", "hotspot",
            "free wifi", "open network", "wireless"
        ],
        ["browsing"] = [
            "browsing", "browse", "safe browsing", "https",
            "secure website", "website safety", "url"
        ],
        ["social_engineering"] = [
            "social engineering", "manipulation", "pretexting",
            "vishing", "impersonation", "baiting", "scam call"
        ],
        ["help"] = [
            "help", "menu", "topics", "what can you do",
            "options", "commands", "what can i ask"
        ]
    };

    public static string DetectIntent(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return "unknown";
        string lower = input.ToLowerInvariant();

        foreach (var (intent, keywords) in IntentMap)
        {
            if (keywords.Any(k => lower.Contains(k)))
                return intent;
        }

        return "unknown";
    }

    public static bool IsQuizRequest(string input) =>
        DetectIntent(input) == "quiz";

    public static bool IsTaskViewRequest(string input) =>
        DetectIntent(input) == "tasks";

    public static bool IsAddTaskRequest(string input) =>
        DetectIntent(input) == "add_task";
}