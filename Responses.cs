using System;

public static class Responses
{
    public static string GetResponse(string input, string name)
    {
        input = input.ToLower();

        if (input.Contains("hi") || input.Contains("hello"))
        {
            return "Hello! How can I help you today?";
        }
        else if (input.Contains("password"))
        {
            return name + ", use strong passwords with letters, numbers, and symbols.";
        }
        else if (input.Contains("phishing"))
        {
            return "Be careful of suspicious emails and links. Always verify the sender.";
        }
        else if (input.Contains("privacy"))
        {
            return "Never share personal information like your ID or banking details online.";
        }
        else if (input.Contains("virus") || input.Contains("malware"))
        {
            return "Install antivirus software and keep it updated.";
        }
        else if (input.Contains("wifi"))
        {
            return "Avoid using public WiFi for sensitive transactions.";
        }
        else if (input.Contains("2fa"))
        {
            return "Enable two-factor authentication for extra security.";
        }
        else if (input == "exit")
        {
            return "Goodbye " + name + "! Stay safe online.";
        }
        else
        {
            return "Sorry, I don't understand. Try asking about passwords, phishing, or privacy.";
        }
    }
}
