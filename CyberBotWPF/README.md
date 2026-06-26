# 🛡️ CyberBot WPF — Part 2

Cybersecurity Awareness Chatbot with a full WPF GUI, sentiment detection, memory, random responses, and conversation flow.

---

## Requirements

- .NET 8 SDK (Windows)
- Visual Studio 2022 **or** VS Code with C# Dev Kit

---

## Run

```bash
cd CyberBotWPF
dotnet run
```

Or open in Visual Studio and press **F5**.

---

## Features

| Feature | Implementation |
|---|---|
| WPF GUI | Dark cyber-themed chat window with sidebar |
| Keyword Recognition | Dictionary-based knowledge base, 10+ topics |
| Random Responses | `List<string>` per topic + delegate `RandomPicker` |
| Conversation Flow | Follow-up detection ("tell me more", "explain more") |
| Memory & Recall | `MemoryService` using `Dictionary<string,string>` |
| Sentiment Detection | Detects worried / curious / frustrated / positive |
| Error Handling | Graceful fallback, no crashes |
| OOP | Models, Services, ViewModels, Views — clean separation |

---

## Project Structure

```
CyberBotWPF/
├── App.xaml / App.xaml.cs
├── Models/
│   ├── User.cs           — Automatic properties, session tracking
│   └── ChatMessage.cs    — Message model
├── Services/
│   ├── AudioService.cs   — WAV greeting
│   ├── ResponseService.cs— Knowledge base + delegate random picker
│   ├── SentimentService.cs — Sentiment detection
│   └── MemoryService.cs  — Dictionary-based memory
├── ViewModels/
│   └── ChatViewModel.cs  — INotifyPropertyChanged, ObservableCollection
├── Views/
│   ├── MainWindow.xaml   — WPF UI
│   └── MainWindow.xaml.cs— Event handlers
└── .github/workflows/ci.yml
```

---

## Voice Greeting

Place `greeting.wav` next to the `.exe` (`bin/Release/net8.0-windows/`).

---

## GitHub Commits (minimum 6)

```
feat: add WPF project structure and App.xaml
feat: add Models (User, ChatMessage)
feat: add SentimentService and MemoryService
feat: add ResponseService with random responses and delegate
feat: add ChatViewModel with INotifyPropertyChanged
feat: add MainWindow WPF UI with dark cyber theme
ci: add GitHub Actions workflow for WPF build
```
