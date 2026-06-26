using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CyberBotWPF.Models;
using CyberBotWPF.Services;

namespace CyberBotWPF.Views;

public partial class QuizWindow : Window
{
    private readonly QuizService _quiz;
    private readonly ActivityLogService _log;
    private string _selectedAnswer = string.Empty;
    private bool _answered = false;

    public event Action<int, int>? QuizCompleted;

    public QuizWindow(ActivityLogService log)
    {
        InitializeComponent();
        _quiz = new QuizService();
        _log = log;
        _quiz.StartQuiz();
        _log.Log("Quiz", "Quiz started");
        LoadQuestion();
    }

    private void LoadQuestion()
    {
        _answered = false;
        _selectedAnswer = string.Empty;
        FeedbackBorder.Visibility = Visibility.Collapsed;
        ResultLabel.Text = string.Empty;
        OptionsPanel.Children.Clear();

        QuizQuestion? q = _quiz.GetCurrentQuestion();

        if (q == null || _quiz.IsFinished)
        {
            ShowFinalScore();
            return;
        }

        ProgressBar.Value = _quiz.CurrentIndex;
        ProgressLabel.Text = $"{_quiz.CurrentIndex + 1} / {_quiz.TotalQuestions}";
        ScoreLabel.Text = $"Score: {_quiz.Score} / {_quiz.TotalQuestions}";
        QuestionText.Text = q.QuestionText;

        if (_quiz.CurrentIndex == _quiz.TotalQuestions - 1)
            NextButton.Content = "Finish Quiz";
        else
            NextButton.Content = "Next Question";

        foreach (string option in q.Options)
        {
            Button btn = new Button();
            btn.Content = option;
            btn.Margin = new Thickness(0, 5, 0, 5);
            btn.Padding = new Thickness(15, 10, 15, 10);
            btn.Background = new SolidColorBrush(Color.FromRgb(19, 29, 53));
            btn.Foreground = new SolidColorBrush(Color.FromRgb(232, 244, 253));
            btn.BorderBrush = new SolidColorBrush(Color.FromRgb(30, 58, 95));
            btn.BorderThickness = new Thickness(1);
            btn.HorizontalContentAlignment = HorizontalAlignment.Left;
            btn.Cursor = System.Windows.Input.Cursors.Hand;
            btn.Tag = option;
            btn.Click += OptionButton_Click;
            OptionsPanel.Children.Add(btn);
        }
    }

    private void OptionButton_Click(object sender, RoutedEventArgs e)
    {
        if (_answered) return;
        Button btn = (Button)sender;
        _selectedAnswer = btn.Tag.ToString();

        foreach (Button b in OptionsPanel.Children)
        {
            b.Background = new SolidColorBrush(Color.FromRgb(19, 29, 53));
            b.BorderBrush = new SolidColorBrush(Color.FromRgb(30, 58, 95));
        }

        btn.Background = new SolidColorBrush(Color.FromRgb(0, 68, 136));
        btn.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 212, 255));
    }

    private void NextButton_Click(object sender, RoutedEventArgs e)
    {
        if (!_answered)
        {
            if (string.IsNullOrEmpty(_selectedAnswer))
            {
                ResultLabel.Text = "Please select an answer!";
                return;
            }

            var result = _quiz.SubmitAnswer(_selectedAnswer);
            bool correct = result.IsCorrect;
            string explan = result.Explanation;
            _answered = true;

            FeedbackBorder.Visibility = Visibility.Visible;

            if (correct)
            {
                FeedbackBorder.Background = new SolidColorBrush(
                    Color.FromRgb(0, 40, 20));
                FeedbackText.Text = "Correct! " + explan;
                FeedbackText.Foreground = new SolidColorBrush(
                    Color.FromRgb(0, 255, 136));
                ResultLabel.Text = "Correct!";
            }
            else
            {
                FeedbackBorder.Background = new SolidColorBrush(
                    Color.FromRgb(40, 0, 10));
                FeedbackText.Text = "Incorrect. " + explan;
                FeedbackText.Foreground = new SolidColorBrush(
                    Color.FromRgb(255, 68, 102));
                ResultLabel.Text = "Wrong";
            }

            ScoreLabel.Text = "Score: " + _quiz.Score + " / " + _quiz.TotalQuestions;

            if (_quiz.IsFinished)
                NextButton.Content = "See Results";
            else
                NextButton.Content = "Next Question";
        }
        else
        {
            if (_quiz.IsFinished)
                ShowFinalScore();
            else
                LoadQuestion();
        }
    }

    private void ShowFinalScore()
    {
        string feedback = _quiz.GetFinalFeedback();
        _log.Log("Quiz Completed",
            _quiz.Score + "/" + _quiz.TotalQuestions);
        QuizCompleted?.Invoke(_quiz.Score, _quiz.TotalQuestions);
        MessageBox.Show(feedback, "Quiz Complete!",
            MessageBoxButton.OK, MessageBoxImage.Information);
        Close();
    }
}