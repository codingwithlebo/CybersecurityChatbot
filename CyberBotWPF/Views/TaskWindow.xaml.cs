using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CyberBotWPF.Models;
using CyberBotWPF.Services;

namespace CyberBotWPF.Views;

public partial class TaskWindow : Window
{
    private readonly TaskService _taskService;
    private readonly ActivityLogService _log;
    public event Action? TasksChanged;

    public TaskWindow(TaskService taskService, ActivityLogService log)
    {
        InitializeComponent();
        _taskService = taskService;
        _log = log;
        RefreshTaskList();
    }

    private void AddTask_Click(object sender, RoutedEventArgs e)
    {
        string title = TitleBox.Text.Trim();
        if (string.IsNullOrWhiteSpace(title))
        {
            MessageBox.Show("Please enter a task title.", "Missing Title",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        string desc = DescBox.Text.Trim();
        DateTime? reminder = ReminderPicker.SelectedDate;
        CyberTask task = new(title,
            desc.Length > 0 ? desc : "No description", reminder);
        _taskService.AddTask(task);
        _log.Log("Task Added", title +
            (reminder.HasValue
                ? $" (reminder: {reminder.Value:dd MMM})" : ""));
        TitleBox.Text = string.Empty;
        DescBox.Text = string.Empty;
        ReminderPicker.SelectedDate = null;
        RefreshTaskList();
        TasksChanged?.Invoke();
        MessageBox.Show($"✅ Task added: '{title}'", "Task Added",
            MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void RefreshTaskList()
    {
        TasksList.Items.Clear();
        var tasks = _taskService.GetAll();
        if (tasks.Count == 0)
        {
            TasksList.Items.Add(new TextBlock
            {
                Text = "No tasks yet. Add your first task above!",
                Foreground = new SolidColorBrush(Color.FromRgb(122, 155, 181)),
                FontSize = 12,
                Padding = new Thickness(0, 5, 0, 5)
            });
            return;
        }
        foreach (CyberTask task in tasks)
            TasksList.Items.Add(CreateTaskCard(task));
    }

    private Border CreateTaskCard(CyberTask task)
    {
        Border card = new()
        {
            Background = new SolidColorBrush(Color.FromRgb(19, 29, 53)),
            BorderBrush = new SolidColorBrush(task.IsCompleted
                ? Color.FromRgb(0, 255, 136)
                : Color.FromRgb(30, 58, 95)),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(8),
            Padding = new Thickness(12, 8, 12, 8),
            Margin = new Thickness(0, 4, 0, 4)
        };

        StackPanel panel = new();
        Grid titleRow = new();
        titleRow.ColumnDefinitions.Add(new ColumnDefinition
        { Width = new GridLength(1, GridUnitType.Star) });
        titleRow.ColumnDefinitions.Add(new ColumnDefinition
        { Width = GridLength.Auto });
        titleRow.ColumnDefinitions.Add(new ColumnDefinition
        { Width = GridLength.Auto });

        TextBlock titleBlock = new()
        {
            Text = $"{task.StatusText}  {task.Title}",
            Foreground = new SolidColorBrush(Color.FromRgb(232, 244, 253)),
            FontSize = 13,
            FontWeight = FontWeights.SemiBold
        };
        Grid.SetColumn(titleBlock, 0);

        Button completeBtn = new()
        {
            Content = "✅",
            FontSize = 12,
            Background = Brushes.Transparent,
            BorderThickness = new Thickness(0),
            Cursor = System.Windows.Input.Cursors.Hand,
            Margin = new Thickness(5, 0, 0, 0),
            Visibility = task.IsCompleted
                ? Visibility.Collapsed : Visibility.Visible
        };
        completeBtn.Click += (_, _) =>
        {
            _taskService.CompleteTask(task.Id);
            _log.Log("Task Completed", task.Title);
            RefreshTaskList();
            TasksChanged?.Invoke();
        };
        Grid.SetColumn(completeBtn, 1);

        Button deleteBtn = new()
        {
            Content = "🗑️",
            FontSize = 12,
            Background = Brushes.Transparent,
            BorderThickness = new Thickness(0),
            Cursor = System.Windows.Input.Cursors.Hand,
            Margin = new Thickness(5, 0, 0, 0)
        };
        deleteBtn.Click += (_, _) =>
        {
            _taskService.DeleteTask(task.Id);
            _log.Log("Task Deleted", task.Title);
            RefreshTaskList();
            TasksChanged?.Invoke();
        };
        Grid.SetColumn(deleteBtn, 2);

        titleRow.Children.Add(titleBlock);
        titleRow.Children.Add(completeBtn);
        titleRow.Children.Add(deleteBtn);
        panel.Children.Add(titleRow);

        if (task.ReminderDate.HasValue)
            panel.Children.Add(new TextBlock
            {
                Text = task.ReminderText,
                Foreground = new SolidColorBrush(Color.FromRgb(255, 215, 0)),
                FontSize = 11,
                Margin = new Thickness(0, 4, 0, 0)
            });

        card.Child = panel;
        return card;
    }

    private void Close_Click(object sender, RoutedEventArgs e) => Close();
}