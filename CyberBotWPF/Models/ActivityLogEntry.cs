using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CyberBotWPF.Models;

public class ActivityLogEntry
{
    public string Action { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.Now;

    public ActivityLogEntry(string action, string detail)
    {
        Action = action;
        Detail = detail;
        Timestamp = DateTime.Now;
    }

    public string FormattedEntry =>
        $"[{Timestamp:HH:mm:ss}] {Action}: {Detail}";
}