using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberBotWPF.Models;

public class QuizQuestion
{
    public string QuestionText { get; set; } = string.Empty;
    public List<string> Options { get; set; } = [];
    public string CorrectAnswer { get; set; } = string.Empty;
    public string Explanation { get; set; } = string.Empty;
    public string Type { get; set; } = "multiple";

    public QuizQuestion(string question, List<string> options,
                        string correct, string explanation,
                        string type = "multiple")
    {
        QuestionText = question;
        Options = options;
        CorrectAnswer = correct;
        Explanation = explanation;
        Type = type;
    }
}