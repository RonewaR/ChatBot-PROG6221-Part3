using System.Speech.Synthesis;


public static class VoiceGreeting
{
    private static SpeechSynthesizer speaker = new SpeechSynthesizer();

    public static void Speak(string text)
    {
        speaker.Volume = 100;
        speaker.Rate = 0;
        speaker.SelectVoiceByHints(VoiceGender.Female);

        speaker.SpeakAsyncCancelAll();
        speaker.SpeakAsync(text);
    }

    public static void PlayGreeting(string name)
    {
        Speak("Hello " + name + ", welcome to your chatbot application.");
    }
}