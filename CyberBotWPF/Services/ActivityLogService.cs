using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CyberBotWPF.Models;

namespace CyberBotWPF.Services;

public class ActivityLogService
{
    private readonly List<ActivityLogEntry> _log = [];

    public void Log(string action, string detail)
    {
        _log.Add(new ActivityLogEntry(action, detail));
    }

    public string GetFormattedLog()
    {
        if (_log.Count == 0)
            return "📋 No activity recorded yet.";

        var recent = _log.TakeLast(10).ToList();
        string entries = string.Join("\n",
            recent.Select((e, i) => $"{i + 1}. {e.FormattedEntry}"));

        return $"📋 RECENT ACTIVITY LOG\n" +
               $"──────────────────────────────────\n" +
               entries;
    }

    public int Count => _log.Count;

    public static bool IsLogRequest(string input)
    {
        string lower = input.ToLowerInvariant();
        string[] triggers = [
            "show activity log", "activity log", "what have you done",
            "show log", "recent actions", "what have you done for me",
            "show history", "view log"
        ];
        return triggers.Any(t => lower.Contains(t));
    }
}