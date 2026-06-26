---

## 🖥️ Part 1 — Console Chatbot

A command-line cybersecurity awareness chatbot.

### Features
- ASCII art CYBER BOT logo
- Voice greeting on startup
- Colour coded console UI with typing effect
- Topics: passwords, phishing, malware, safe browsing, social engineering, 2FA, public wifi
- Input validation and graceful fallback responses
- Session stats on exit

### How to Run
```bash
cd CybersecurityChatbot
dotnet run
```

---

## 🖼️ Part 2 — WPF GUI Chatbot

A dark cyber themed graphical chatbot interface.

### Features
- Dark cyber themed WPF window
- Keyword recognition for 10 plus cybersecurity topics
- Random responses using List and delegate
- Sentiment detection — worried, curious, frustrated, positive
- Memory and recall using Dictionary
- Conversation flow with follow-up detection
- Quick topic buttons in sidebar

### How to Run
```bash
cd CyberBotWPF
dotnet run
```

---

## 🎮 Part 3 — Full POE

Extended WPF app with advanced interactive features.

### Features
- Task Assistant — add, view, complete and delete cybersecurity tasks with reminders stored in JSON
- Cybersecurity Quiz — 13 questions, multiple choice and true/false, score tracking
- NLP Simulation — flexible keyword detection understands differently worded requests
- Activity Log — records all chatbot actions, view last 10 on request

### How to Use
Type these commands in the chat:
- `quiz` — start the quiz game
- `add task Enable two-factor authentication` — add a task
- `show tasks` — view all tasks
- `open tasks` — open task management window
- `show activity log` — view recent actions
- `remind me to update my password tomorrow` — add task with reminder

---

## 🛠️ Technologies Used

- C# .NET 8
- WPF (Windows Presentation Foundation)
- XAML
- JSON file storage (System.Text.Json)
- GitHub Actions CI/CD

---

## 📁 Code Structure
