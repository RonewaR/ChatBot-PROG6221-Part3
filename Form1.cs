using System;
using System.Speech.Recognition;
using System.Collections.Generic;
using System.Speech.Synthesis;
using System.Windows.Forms;

namespace ChatbotApp
{
    public partial class Form1 : Form
    {
        private SpeechRecognitionEngine recognizer;
        private SpeechSynthesizer voice;
        private string userName = "";
        private List<string> activityLog = new List<string>();
        private List<string> tasks = new List<string>();
        private bool quizActive = false;
        private int quizQuestion = 0;
        private int quizScore = 0;

        private Dictionary<string, string> responses = new Dictionary<string, string>()
        {
            { "hello", "Hello 👋 How can I help you today?" },
            { "hi", "Hi there 👋" },
            { "hey", "Hey! 😊" },
            { "how are you", "I'm doing great 😊 Thanks for asking!" },
            { "your name", "I'm your AI chatbot assistant 🤖" },
            { "password", "Tip: Never share your password with anyone 🔐" },
            { "phishing", "Phishing is when attackers try to trick you into giving personal info." },
            { "virus", "Always use antivirus software and avoid unknown downloads." },
            { "help", "You can ask me about time, date, cybersecurity tips, or greetings." }
        };

        private void AddToLog(string action)
        {
            string entry = DateTime.Now.ToString("HH:mm:ss") + " - " + action;
            activityLog.Add(entry);

            if (activityLog.Count > 10)
            {
                activityLog.RemoveAt(0);
            }
        }

        public Form1()
        {
            InitializeComponent();
            this.AcceptButton = btnSend;

            // 🔊 Voice setup
            try
            {
                voice = new SpeechSynthesizer();
                voice.SetOutputToDefaultAudioDevice();
                voice.Volume = 100;
                voice.Rate = 0;
                voice.SelectVoiceByHints(VoiceGender.Female);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Voice setup failed: " + ex.Message);
            }

            // 🎤 Speech recognition setup
            try
            {
                recognizer = new SpeechRecognitionEngine();
                recognizer.SetInputToDefaultAudioDevice();
                recognizer.LoadGrammar(new DictationGrammar());
                recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Voice input setup failed: " + ex.Message);
            }
        }

        // ✅ KEEP EMPTY
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        // 🔊 VOICE GREETING (IMPORTANT)
        private void Form1_Shown(object sender, EventArgs e)
        {
            try
            {
                voice.SpeakAsync("Hello! Welcome to your chatbot application. You can start chatting now.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Voice greeting failed: " + ex.Message);
            }
        }

        // 💬 SEND BUTTON
        private void btnSend_Click(object sender, EventArgs e)
        {
            string userMessage = txtUserInput.Text.Trim();
            AddToLog("User sent message: " + userMessage);
            MessageBox.Show("Log count = " + activityLog.Count); 

            if (string.IsNullOrWhiteSpace(userMessage))
                return;

            rtbChat.SelectionColor = System.Drawing.Color.Cyan;
            rtbChat.AppendText("You: " + userMessage + Environment.NewLine);

            string botReply = GetBotResponse(userMessage);

            rtbChat.SelectionColor = System.Drawing.Color.LimeGreen;
            rtbChat.AppendText("Bot: " + botReply + Environment.NewLine + Environment.NewLine);

            txtUserInput.Clear();

            rtbChat.SelectionStart = rtbChat.Text.Length;
            rtbChat.ScrollToCaret();

            try
            {
                voice.SpeakAsyncCancelAll();
                voice.SpeakAsync(botReply);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Voice error: " + ex.Message);
            }
        }

        // 🎤 VOICE INPUT BUTTON
        private void btnVoice_Click(object sender, EventArgs e)
        {
            try
            {
                if (recognizer != null)
                {
                    recognizer.RecognizeAsyncCancel();
                    System.Threading.Thread.Sleep(150);
                    recognizer.RecognizeAsync(RecognizeMode.Single);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Voice input error: " + ex.Message);
            }
        }

        // 🎤 SPEECH RESULT
        private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result == null || string.IsNullOrWhiteSpace(e.Result.Text))
                return;

            txtUserInput.Text = e.Result.Text;
            btnSend.PerformClick();
        }

        // 🤖 BOT LOGIC
        private string GetBotResponse(string message)
        {
            message = message.ToLower().Trim();

            // NLP Greeting Detection
            if (message.Contains("hello") ||
                message.Contains("hi") ||
                message.Contains("hey") ||
                message.Contains("good morning") ||
                message.Contains("good afternoon"))
            {
                AddToLog("Greeting detected");
                return "Hello! How can I help you today?";
            }

            if (message.StartsWith("add task"))
            {
                string task = message.Substring(9);
                tasks.Add(task);
                AddToLog("Task added: " + task);
                return "Task added: " + task;
            }
            if (message.Contains("show tasks") ||
                message.Contains("my tasks") ||
                message.Contains("task list") ||
                message.Contains("show task list"))
            {
                if (tasks.Count == 0)
                    return "You have no tasks.";

                string result = "Your Tasks:\n\n";

                for (int i = 0; i < tasks.Count; i++)
                {
                    result += (i + 1) + ". " + tasks[i] + "\n";
                }

                return result;
            }
            if (message.StartsWith("complete task "))
            {
                try
                {
                    int taskNumber = Convert.ToInt32(message.Substring(14));

                    if (taskNumber < 1 || taskNumber > tasks.Count)
                        return "Invalid task number.";

                    string completedTask = tasks[taskNumber - 1];

                    tasks.RemoveAt(taskNumber - 1);

                    AddToLog("Task completed: " + completedTask);

                    return "Task completed: " + completedTask;
                }
                catch
                {
                    return "Please enter a valid task number.";
                }
            }
            if (message == "clear tasks")
            {
                tasks.Clear();
                AddToLog("All tasks cleared");
                return "All tasks have been removed.";
            }

            // Activity log First 
            if (message.Contains("activity"))
            {
                if (activityLog.Count == 0)
                    return "No activity has been recorded yet.";
                return "Recent Activity:\n\n" +
                     string.Join("\n", activityLog);
            }

            // Start Quiz 
            if (message.Contains("quiz") &&
               (message.Contains("start") ||
                message.Contains("begin") ||
                message.Contains("play")))
            {
                quizActive = true;
                quizQuestion = 1;
                quizScore = 0;

                AddToLog("Quiz started");

                return "Cybersecurity Quiz Started!\n\nQuestion 1:\nWhat does 2FA stand for?\nA) Two Factor Authentication\nB) Two File Access\nC) Two Fast Accounts";
            }

            if (message == "test")
            {
                return "SUCESS - NEW CODE IS RUNNING"; 
            }
            
            if (quizActive)
            {
                switch (quizQuestion)
                {
                    case 1:
                        if (message == "a")
                            quizScore++;

                        quizQuestion = 2;

                        return "Question 2:\nWhich is a strong password?\nA) 123456\nB) Password\nC) P@ssw0rd!";

                    case 2:
                        if (message == "c")
                            quizScore++;

                        quizQuestion = 3;

                        return "Question 3:\nWhat is phishing?\nA) A type of fish\nB) A scam to steal information\nC) A computer game";

                    case 3:
                        if (message == "b")
                            quizScore++;

                        quizActive = false;

                        return "Quiz Complete!\nYour Score: " + quizScore + "/3";
                }
            }
            if (message.StartsWith("what is"))
            {
                AddToLog("Knowledge question asked");

                if (message.Contains("phishing"))
                    return "Phishing is a scam that tricks users into revealing personal information.";

                if (message.Contains("malware"))
                    return "Malware is malicious software designed to damage or access systems.";

                if (message.Contains("2fa"))
                    return "Two-factor authentication adds an extra layer of security.";

                return "I don't have information about that topic yet.";
            }
            if (message.Contains("i am"))
            {
                if (message.Contains("happy"))
                    return "I'm glad you're feeling happy! 😊";

                if (message.Contains("sad"))
                    return "I'm sorry you're feeling sad. 💙";

                if (message.Contains("stressed"))
                    return "Take a short break and remember to look after yourself.";
            }

            if (message.StartsWith("my name is"))
            {
                userName = message.Replace("my name is", "").Trim();
                AddToLog("User name set to " + userName);
                return "Nice to meet you " + userName + " 😊";
            }

            if (message.Contains("my name"))
            {
                return string.IsNullOrEmpty(userName)
                    ? "I don't know your name yet."
                    : "Your name is " + userName;
            }

            if (message.Contains("time"))
            {
                AddToLog("Time requested");
                return "Current time is " + DateTime.Now.ToShortTimeString();
            }

            if (message.Contains("date"))
            {
                AddToLog("Date requested");
                return "Today's date is " + DateTime.Now.ToShortDateString();
            }

            foreach (var pair in responses)
            {
                if (message.Contains(pair.Key))
                    return pair.Value;
            }

            return "I'm not fully sure, but I'm learning from your inputs 🤖";
        }
    }
}
