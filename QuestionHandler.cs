using System;
using System.Collections.Generic;

namespace CyberSecurityChatBotAI
{
    // Handles user questions and responses
    public class QuestionHandler
    {
        private Dictionary<string, string> responses = new Dictionary<string, string>();

        public QuestionHandler()
        {
            StoreReplies();
        }

        public void HandleQuestions(string userName)
        {
            while (true)
            {
                Console.WriteLine("Chat AI-> Enter your question (or 'exit' to go back to the main menu):");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(userName + " -> ");
                string question = Console.ReadLine()?.ToLower();
                Console.ForegroundColor = ConsoleColor.Cyan;

                if (question == "exit")
                {
                    Console.WriteLine("Goodbye! " + userName);
                    return;
                }

                if (responses.ContainsKey(question))
                {
                    Console.WriteLine("Chat AI -> " + responses[question]);
                }
                else
                {
                    Console.WriteLine("Chat AI-> I couldn't find an answer. Try rephrasing your question.");
                }
            }
        }

        private void StoreReplies()
        {
            responses["how are you"] = "I'm just a bot, but I'm running smoothly!";
            responses["what is your purpose"] = "I provide cybersecurity knowledge to help keep you safe online.";
            responses["password safety"] = "Use long, unique passwords and a password manager!";
            responses["phishing"] = "Beware of emails or messages asking for personal information. Always verify the sender.";
            responses["safe browsing"] = "Use secure websites (HTTPS) and avoid downloading from unknown sources.";
            responses["firewall"] = "Firewalls help block unauthorized access to your network.";
            responses["vpn"] = "A VPN encrypts your internet traffic, keeping your online activity private.";
            responses["encryption"] = "Encryption helps protect your sensitive data from unauthorized access.";
            responses["ransomware"] = "Never open suspicious attachments, and always back up your important files.";
            responses["antivirus"] = "Keep your antivirus software updated to protect against threats.";

            responses["Spoofing"] = "avoid talking to people you dont know especially online and about your personal information";
        }
    }
}
