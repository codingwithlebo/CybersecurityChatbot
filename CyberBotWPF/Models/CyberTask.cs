using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CyberBotWPF.Models;

public class CyberTask
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ReminderDate { get; set; } = null;

    public CyberTask() { }

    public CyberTask(string title, string description, DateTime? reminderDate = null)
    {
        Title = title;
        Description = description;
        ReminderDate = reminderDate;
        CreatedAt = DateTime.Now;
    }

    public string ReminderText => ReminderDate.HasValue
        ? $"⏰ Reminder: {ReminderDate.Value:dd MMM yyyy}"
        : "No reminder set";

    public string StatusText => IsCompleted ? "✅ Completed" : "⏳ Pending";
}