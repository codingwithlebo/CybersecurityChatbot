using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using CyberBotWPF.Models;

namespace CyberBotWPF.Services;

public class TaskService
{
    private readonly string _filePath = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory, "tasks.json");

    private List<CyberTask> _tasks = [];

    public TaskService()
    {
        Load();
    }

    public List<CyberTask> GetAll() => _tasks;

    public List<CyberTask> GetPending() =>
        _tasks.Where(t => !t.IsCompleted).ToList();

    public void AddTask(CyberTask task)
    {
        _tasks.Add(task);
        Save();
    }

    public void CompleteTask(string id)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        if (task != null)
        {
            task.IsCompleted = true;
            Save();
        }
    }

    public void DeleteTask(string id)
    {
        _tasks.RemoveAll(t => t.Id == id);
        Save();
    }

    public List<CyberTask> GetUpcomingReminders()
    {
        DateTime now = DateTime.Now;
        return _tasks
            .Where(t => !t.IsCompleted &&
                        t.ReminderDate.HasValue &&
                        t.ReminderDate.Value.Date <= now.AddDays(3).Date)
            .ToList();
    }

    public CyberTask? DetectTaskRequest(string input)
    {
        string lower = input.ToLowerInvariant();

        string[] addTriggers = [
            "add task", "create task", "new task", "add a task",
            "remind me to", "set a reminder", "remind me about",
            "add reminder", "i need to", "task to"
        ];

        if (!addTriggers.Any(t => lower.Contains(t)))
            return null;

        string title = input;
        foreach (string trigger in addTriggers)
        {
            int idx = lower.IndexOf(trigger);
            if (idx >= 0)
            {
                title = input[(idx + trigger.Length)..].Trim().TrimEnd('.', '!', '?');
                break;
            }
        }

        if (title.Length < 3) return null;

        DateTime? reminderDate = null;
        if (lower.Contains("tomorrow"))
            reminderDate = DateTime.Now.AddDays(1);
        else if (lower.Contains("next week"))
            reminderDate = DateTime.Now.AddDays(7);
        else if (lower.Contains("in 3 days"))
            reminderDate = DateTime.Now.AddDays(3);
        else if (lower.Contains("in 7 days"))
            reminderDate = DateTime.Now.AddDays(7);

        return new CyberTask(
            title.Length > 60 ? title[..60] : title,
            $"Added via chatbot on {DateTime.Now:dd MMM yyyy}",
            reminderDate
        );
    }

    private void Save()
    {
        try
        {
            string json = JsonSerializer.Serialize(_tasks,
                new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
        catch { }
    }

    private void Load()
    {
        try
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                _tasks = JsonSerializer.Deserialize<List<CyberTask>>(json) ?? [];
            }
        }
        catch { _tasks = []; }
    }
}
