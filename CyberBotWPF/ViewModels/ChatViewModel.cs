using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CyberBotWPF.Models;
using CyberBotWPF.Services;
using CyberBotWPF.Views;

namespace CyberBotWPF.ViewModels;

public class ChatViewModel : INotifyPropertyChanged
{
    private readonly MemoryService _memory = new();
    private readonly TaskService _tasks = new();
    private readonly ActivityLogService _log = new();
    private User? _user;
    private string _currentTopic = string.Empty;

    public ObservableCollection<ChatMessage> Messages { get; } = [];

    private string _inputText = string.Empty;
    private string _statusText = "Type a message to begin...";
    private bool _isNamePrompt = true;
    private int _taskCount = 0;

    public string InputText
    {
        get => _inputText;
        set { _inputText = value; OnPropertyChanged(); }
    }

    public string StatusText
    {
        get => _statusText;
        set { _statusText = value; OnPropertyChanged(); }
    }

    public bool IsNamePrompt
    {
        get => _isNamePrompt;
        set { _isNamePrompt = value; OnPropertyChanged(); }
    }

    public int TaskCount
    {
        get => _taskCount;
        set { _taskCount = value; OnPropertyChanged(); }
    }

    public void Initialise()
    {
        AudioService.PlayGreeting();
        TaskCount = _tasks.GetAll().Count;

        var reminders = _tasks.GetUpcomingReminders();
        string reminderNote = reminders.Count > 0
            ? $"\n\n⏰ You have {reminders.Count} upcoming reminder(s)!"
            : string.Empty;

        AddBotMessage(
            "👋 Welcome to the Cybersecurity Awareness Bot!\n\n" +
            "I can help you with:\n" +
            "  🔑 Cybersecurity topics\n" +
            "  📋 Task management\n" +
            "  🎮 Quiz game\n" +
            "  📊 Activity log\n\n" +
            "Before we begin — what is your name?" + reminderNote,
            "Welcome");
        StatusText = "Please enter your name to get started.";
    }

    public void SendMessage()
    {
        string input = InputText.Trim();
        if (string.IsNullOrWhiteSpace(input)) return;
        InputText = string.Empty;

        if (IsNamePrompt)
        {
            _user = new User(input);
            IsNamePrompt = false;
            _memory.Store("name", input);
            _log.Log("Session Started", $"User: {input}");
            AddUserMessage(input);
            AddBotMessage(
                $"🎉 Welcome, {_user.Name}!\n\n" +
                $"Here is what I can do:\n" +
                $"  🔑 Ask about cybersecurity topics\n" +
                $"  📋 Type 'add task' to manage tasks\n" +
                $"  🎮 Type 'quiz' to start the quiz game\n" +
                $"  📊 Type 'show activity log' to see recent actions\n" +
                $"  ❓ Type 'help' for all topics",
                "Welcome");
            StatusText = $"Session started · {_user.Name}";
            return;
        }

        if (_user == null) return;
        _user.QuestionCount++;
        AddUserMessage(input);

        if (ResponseService.IsExitCommand(input))
        {
            _log.Log("Session Ended", $"Questions: {_user.QuestionCount}");
            AddBotMessage(
                $"👋 Goodbye, {_user.Name}! Stay cyber-safe!\n\n" +
                $"📊 Questions asked: {_user.QuestionCount}\n" +
                $"📋 Tasks created: {_tasks.GetAll().Count}\n" +
                $"⏱️ Duration: {_user.GetSessionDuration()}",
                "Farewell");
            return;
        }

        string sentiment = SentimentService.Detect(input);
        string empathy = SentimentService.GetEmpathyPrefix(sentiment, _user.Name);
        string intent = NlpService.DetectIntent(input);

        if (ActivityLogService.IsLogRequest(input))
        {
            _log.Log("Activity Log", "User viewed log");
            AddBotMessage(_log.GetFormattedLog(), "Activity Log");
            return;
        }

        if (intent == "quiz")
        {
            _log.Log("Quiz", "Quiz window opened");
            AddBotMessage("🎮 Starting the Cybersecurity Quiz! Good luck!", "Quiz");
            OpenQuizWindow();
            return;
        }

        if (intent == "tasks")
        {
            var allTasks = _tasks.GetAll();
            if (allTasks.Count == 0)
            {
                AddBotMessage(
                    "📋 You have no tasks yet.\n\n" +
                    "Type 'add task' followed by a description!\n" +
                    "Example: 'add task Enable two-factor authentication'",
                    "Tasks");
            }
            else
            {
                string taskList = string.Join("\n", allTasks.Select((t, i) =>
                    $"{i + 1}. {t.StatusText} {t.Title} — {t.ReminderText}"));
                AddBotMessage(
                    $"📋 YOUR TASKS\n──────────────────\n{taskList}\n\n" +
                    "Type 'open tasks' to manage them.", "Tasks");
            }
            _log.Log("Tasks Viewed", $"{allTasks.Count} tasks");
            return;
        }

        if (input.ToLowerInvariant().Contains("open tasks") ||
            input.ToLowerInvariant().Contains("manage tasks"))
        {
            OpenTaskWindow();
            return;
        }

        if (intent == "add_task")
        {
            CyberTask? detectedTask = _tasks.DetectTaskRequest(input);
            if (detectedTask != null)
            {
                _tasks.AddTask(detectedTask);
                TaskCount = _tasks.GetAll().Count;
                _log.Log("Task Added", detectedTask.Title);
                AddBotMessage(
                    $"✅ Task added: '{detectedTask.Title}'\n" +
                    (detectedTask.ReminderDate.HasValue
                        ? $"⏰ Reminder: {detectedTask.ReminderDate.Value:dd MMM yyyy}\n"
                        : "No reminder set.\n") +
                    "\nType 'open tasks' to view and manage all tasks.",
                    "Task Added");
                return;
            }
            else
            {
                AddBotMessage(
                    "📋 To add a task, try:\n\n" +
                    "  • 'Add task Enable two-factor authentication'\n" +
                    "  • 'Remind me to update my password tomorrow'\n" +
                    "  • 'Add task Review privacy settings in 7 days'",
                    "Task Help");
                return;
            }
        }

        string? interest = _memory.DetectAndStoreInterest(input);
        if (interest != null)
        {
            _user.FavouriteTopic = interest;
            _log.Log("Memory", $"Stored interest: {interest}");
            AddBotMessage(
                $"💡 Great! I'll remember you're interested in {interest}.",
                "Memory");
            return;
        }

        if (ResponseService.IsFollowUp(input) && !string.IsNullOrEmpty(_currentTopic))
        {
            string followUp = ResponseService.GetFollowUpResponse(_currentTopic);
            string hint = _memory.GetMemoryHint();
            _log.Log("Follow-up", _currentTopic);
            AddBotMessage(
                empathy + followUp + (hint.Length > 0 ? $"\n\n💡 {hint}" : ""),
                _currentTopic, sentiment);
            return;
        }

        var result = ResponseService.GetResponse(input);
        if (result.HasValue)
        {
            _currentTopic = result.Value.Topic;
            string hint = _memory.GetMemoryHint();
            _log.Log("Topic", result.Value.Topic);
            AddBotMessage(
                empathy + result.Value.Text +
                (hint.Length > 0 ? $"\n\n💡 {hint}" : ""),
                result.Value.Topic, sentiment);
            StatusText = $"{_user.Name} · {result.Value.Topic}";
            return;
        }

        _log.Log("Fallback", input[..Math.Min(30, input.Length)]);
        AddBotMessage(
            $"🤔 I'm not sure I understood that, {_user.Name}.\n\n" +
            $"Try: passwords, phishing, malware, privacy, 2fa, wifi\n" +
            $"Or: 'quiz', 'add task', 'show tasks', 'show activity log'",
            "Fallback", sentiment);
    }

    public void OpenQuizWindow()
    {
        QuizWindow quiz = new(_log);
        quiz.QuizCompleted += (score, total) =>
        {
            AddBotMessage(
                $"🎮 Quiz complete! You scored {score}/{total}!",
                "Quiz Result");
            _log.Log("Quiz Completed", $"{score}/{total}");
        };
        quiz.Show();
    }

    public void OpenTaskWindow()
    {
        TaskWindow taskWin = new(_tasks, _log);
        taskWin.TasksChanged += () => TaskCount = _tasks.GetAll().Count;
        taskWin.Show();
    }

    public void SendQuickTopic(string topic)
    {
        InputText = topic;
        SendMessage();
    }

    private void AddBotMessage(string text, string topic = "", string sentiment = "")
        => Messages.Add(new ChatMessage("bot", text, topic, sentiment));

    private void AddUserMessage(string text)
        => Messages.Add(new ChatMessage("user", text));

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}