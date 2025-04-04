using System;
using System.Collections;
using System.Collections.Generic;

namespace CyberSecurityChatBotAI
{
    // Handles user questions and responses
    public class QuestionHandler
    {
        private ArrayList responses = new ArrayList();

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

                bool found = false;
                foreach (DictionaryEntry entry in responses)
                {
                    if ((string)entry.Key == question)
                    {
                        Console.WriteLine("Chat AI -> " + entry.Value);
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    Console.WriteLine("Chat AI-> I couldn't find an answer. Try rephrasing your question.");
                }
            }
        }

        private void StoreReplies()
        {
            responses.Add(new DictionaryEntry("how are you", "I'm just a bot, but I'm running smoothly!"));
            responses.Add(new DictionaryEntry("what is your purpose", "I provide cybersecurity knowledge to help keep you safe online."));
            responses.Add(new DictionaryEntry("password safety", "Use long, unique passwords and a password manager!"));
            responses.Add(new DictionaryEntry("phishing", "Beware of emails or messages asking for personal information. Always verify the sender."));
            responses.Add(new DictionaryEntry("safe browsing", "Use secure websites (HTTPS) and avoid downloading from unknown sources."));
            responses.Add(new DictionaryEntry("firewall", "Firewalls help block unauthorized access to your network."));
            responses.Add(new DictionaryEntry("vpn", "A VPN encrypts your internet traffic, keeping your online activity private."));
            responses.Add(new DictionaryEntry("encryption", "Encryption helps protect your sensitive data from unauthorized access."));
            responses.Add(new DictionaryEntry("ransomware", "Never open suspicious attachments, and always back up your important files."));
            responses.Add(new DictionaryEntry("antivirus", "Keep your antivirus software updated to protect against threats."));
            responses.Add(new DictionaryEntry("spoofing", "Avoid talking to people you don't know, especially online and about your personal information."));
        }
    }
}
