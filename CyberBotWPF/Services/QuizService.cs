using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CyberBotWPF.Models;

namespace CyberBotWPF.Services;

public class QuizService
{
    private readonly List<QuizQuestion> _allQuestions =
    [
        new QuizQuestion(
            "What should you do if you receive an email asking for your password?",
            ["A) Reply with your password", "B) Delete the email", "C) Report it as phishing", "D) Ignore it"],
            "C", "Reporting phishing emails helps prevent scams and protects others.", "multiple"),

        new QuizQuestion(
            "How many characters should a strong password have at minimum?",
            ["A) 6 characters", "B) 8 characters", "C) 12 characters", "D) 4 characters"],
            "C", "A strong password should have at least 12 characters.", "multiple"),

        new QuizQuestion(
            "What does HTTPS mean in a website URL?",
            ["A) The website is fast", "B) The connection is encrypted and secure", "C) The website is popular", "D) The website is free"],
            "B", "HTTPS means the connection between your browser and the website is encrypted.", "multiple"),

        new QuizQuestion(
            "What is phishing?",
            ["A) A type of antivirus software", "B) A secure way to browse", "C) A scam to trick you into giving personal info", "D) A strong password technique"],
            "C", "Phishing tricks users into revealing sensitive information through fake emails or websites.", "multiple"),

        new QuizQuestion(
            "Which is the safest way to store passwords?",
            ["A) Write them in a notebook", "B) Use the same password everywhere", "C) Use a password manager", "D) Save them in a text file"],
            "C", "A password manager securely stores and encrypts all your passwords.", "multiple"),

        new QuizQuestion(
            "What is two-factor authentication (2FA)?",
            ["A) Using two passwords", "B) A second verification step beyond your password", "C) Logging in twice", "D) Having two email accounts"],
            "B", "2FA adds a second layer of security to your accounts.", "multiple"),

        new QuizQuestion(
            "What should you do before connecting to public Wi-Fi?",
            ["A) Share your location", "B) Use a VPN to encrypt your connection", "C) Log into your bank account", "D) Download unknown apps"],
            "B", "A VPN encrypts your traffic on public Wi-Fi.", "multiple"),

        new QuizQuestion(
            "What is ransomware?",
            ["A) Software that speeds up your computer", "B) A free antivirus tool", "C) Malware that encrypts files and demands payment", "D) A type of firewall"],
            "C", "Ransomware encrypts your files and demands payment. Always backup your data!", "multiple"),

        new QuizQuestion(
            "TRUE or FALSE: You should use the same password for all accounts.",
            ["True", "False"],
            "False", "Never reuse passwords! If one account is breached, all accounts become vulnerable.", "truefalse"),

        new QuizQuestion(
            "TRUE or FALSE: A padlock icon means the website is completely safe.",
            ["True", "False"],
            "False", "HTTPS means encrypted connection but the site could still be malicious.", "truefalse"),

        new QuizQuestion(
            "TRUE or FALSE: Your bank will call and ask for your full PIN number.",
            ["True", "False"],
            "False", "Banks will NEVER ask for your full PIN over the phone. This is always a scam.", "truefalse"),

        new QuizQuestion(
            "TRUE or FALSE: Keeping your software updated helps protect against cyberattacks.",
            ["True", "False"],
            "True", "Software updates patch security vulnerabilities that attackers could exploit.", "truefalse"),

        new QuizQuestion(
            "TRUE or FALSE: It is safe to plug in a USB drive you found in a public place.",
            ["True", "False"],
            "False", "Found USB drives could contain malware — this is called baiting.", "truefalse"),
    ];

    private List<QuizQuestion> _sessionQuestions = [];
    private int _currentIndex = 0;
    private int _score = 0;

    public int TotalQuestions => _sessionQuestions.Count;
    public int CurrentIndex => _currentIndex;
    public int Score => _score;
    public bool IsFinished => _currentIndex >= _sessionQuestions.Count;

    public void StartQuiz()
    {
        _sessionQuestions = _allQuestions
            .OrderBy(_ => Guid.NewGuid())
            .Take(10)
            .ToList();
        _currentIndex = 0;
        _score = 0;
    }

    public QuizQuestion? GetCurrentQuestion() =>
        _currentIndex < _sessionQuestions.Count
            ? _sessionQuestions[_currentIndex]
            : null;

    public (bool IsCorrect, string Explanation) SubmitAnswer(string answer)
    {
        if (_currentIndex >= _sessionQuestions.Count)
            return (false, string.Empty);

        QuizQuestion q = _sessionQuestions[_currentIndex];
        bool correct = answer.Trim().Equals(q.CorrectAnswer.Trim(),
                            StringComparison.OrdinalIgnoreCase);
        if (correct) _score++;
        _currentIndex++;

        return (correct, q.Explanation);
    }

    public string GetFinalFeedback()
    {
        double percent = (double)_score / TotalQuestions * 100;

        return percent switch
        {
            >= 90 => $"🏆 Outstanding! {_score}/{TotalQuestions} — You're a Cybersecurity Pro!",
            >= 70 => $"🎉 Great job! {_score}/{TotalQuestions} — You know your cybersecurity well!",
            >= 50 => $"👍 Good effort! {_score}/{TotalQuestions} — Keep learning to stay safe online!",
            _ => $"📚 Keep practising! {_score}/{TotalQuestions} — Review the topics and try again!"
        };
    }
}