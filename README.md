# ChatBot-Part1-PROG6221
Part 1 of the PROG6221 POE
# Chatbot Assignment

## Project Description
This is a simple C# chatbot application that responds to user input using keyword-based logic.

## Features
- User input handling
- Keyword-based responses
- Greeting and exit commands
- Input validation
- Improved chatbot interaction

## How to run
Open the project in Visual Studio and run the program.

# 🤖 WinForms Chatbot Application (C#)

## 📌 Project Overview
This is a Windows Forms chatbot application built using C#.  
The chatbot provides interactive responses to user input using a dictionary-based logic system and includes voice input/output features for a more engaging user experience.

---

## ✨ Features

### 💬 Chat System
- User can type messages in a chat box
- Bot responds instantly using predefined logic
- Supports common greetings and simple questions

### 📚 Dictionary-Based Intelligence
- Uses a `Dictionary<string, string>` for faster and cleaner response handling
- Easier to expand compared to traditional if-else logic

### 🎨 User Interface (GUI)
- Built using Windows Forms (WinForms)
- Clean chat layout with message display area
- Send button + input textbox
- Improved formatting for messages

### 🔊 Voice Features
- Text-to-Speech (TTS) for bot responses
- Speech recognition for voice input (optional feature depending on system support)

---

## 🧠 How It Works

1. User types a message or uses voice input  
2. Input is processed and converted to lowercase  
3. The chatbot checks the dictionary for a matching response  
4. If found → returns response  
5. If not found → returns default message  
6. Response is displayed in the chat window and spoken aloud

---

## 🧾 Example Code Logic

```csharp
Dictionary<string, string> responses = new Dictionary<string, string>()
{
    { "hello", "Hi there! How can I help you?" },
    { "how are you", "I'm doing great! Thanks for asking." },
    { "bye", "Goodbye! Have a nice day." }
};

## Author
Ronewa Ramphabana
## CI Workflow

The project uses GitHub Actions for Continuous Integration.  
Below is a successful workflow run:

![CI Workflow Screenshot](ci-screenshot.png)
