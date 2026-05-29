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
            message = message.ToLower();

            if (message.StartsWith("my name is"))
            {
                userName = message.Replace("my name is", "").Trim();
                return "Nice to meet you " + userName + " 😊";
            }

            if (message.Contains("my name"))
            {
                return string.IsNullOrEmpty(userName)
                    ? "I don't know your name yet."
                    : "Your name is " + userName;
            }

            if (message.Contains("time"))
                return "Current time is " + DateTime.Now.ToShortTimeString();

            if (message.Contains("date"))
                return "Today's date is " + DateTime.Now.ToShortDateString();

            foreach (var pair in responses)
            {
                if (message.Contains(pair.Key))
                    return pair.Value;
            }

            return "I'm not fully sure, but I'm learning from your inputs 🤖";
        }
    }
}