using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;

namespace CyberSecurityChatBotAI
{
    // Handles user questions and responses
    public class QuestionHandler
    {
        private ArrayList singleResponses = new ArrayList();
        private Dictionary<string, List<string>> keywordResponses = new Dictionary<string, List<string>>();
        private Dictionary<string, Dictionary<string, string>> sentimentalResponses = new Dictionary<string, Dictionary<string, string>>();
        private Random random = new Random();
        private string currentTopic = null; // Tracks the current topic
        private Memory memory; // Memory instance to store user details

        public QuestionHandler(Memory memory)
        {
            this.memory = memory;
            StoreReplies();
        }
        /// Handles user questions and provides responses
        public void HandleQuestions()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine($"Chat AI-> {memory.UserName}, enter your question (or 'exit' to go back to the main menu):");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"{memory.UserName} -> ");
                    string question = Console.ReadLine()?.ToLower();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    string response = null;

                    // Check for empty input
                    if (string.IsNullOrWhiteSpace(question))
                    {
                        response = "It seems you didn't enter anything. Could you try again?";
                        Console.WriteLine($"Chat AI -> {response}");
                        continue;
                    }

                    if (question == "exit")
                    {
                        response = $"Goodbye, {memory.UserName}! Feel free to come back anytime.";
                        Console.WriteLine($"Chat AI -> {response}");
                        memory.RecordConversation(question, response); // Record exit conversation
                        return;
                    }

                    // Detect sentiment in the user's input
                    string sentiment = DetectSentiment(question);

                    // Check for keyword match with sentimental response
                    string sentimentalResponse = GetSentimentalResponse(question, sentiment);
                    if (sentimentalResponse != null)
                    {
                        response = sentimentalResponse;
                        Console.WriteLine($"Chat AI -> {response}");
                        memory.RecordConversation(question, response); // Record conversation
                        currentTopic = GetKeywordFromInput(question); // Update current topic
                        continue;
                    }

                    // Check for exact match in single responses
                    string singleResponse = GetSingleResponse(question);
                    if (singleResponse != null)
                    {
                        response = singleResponse;
                        Console.WriteLine($"Chat AI -> {response}");
                        memory.RecordConversation(question, response); // Record conversation
                        currentTopic = null; // Reset topic for general questions
                        continue;
                    }

                    // Check for keyword match with random response
                    string randomResponse = GetRandomKeywordResponse(question);
                    if (randomResponse != null)
                    {
                        response = randomResponse;
                        Console.WriteLine($"Chat AI -> {response}");
                        memory.RecordConversation(question, response); // Record conversation
                        currentTopic = GetKeywordFromInput(question); // Update current topic
                        continue;
                    }

                    // Default response for unknown inputs
                    response = "I'm not sure I understand. Can you try rephrasing?";
                    Console.WriteLine($"Chat AI -> {response}");
                    memory.RecordConversation(question, response); // Record conversation
                }
                catch (Exception ex)
                {
                    // Log the error (optional) and provide a user-friendly message
                    Console.WriteLine("Chat AI -> Oops! Something went wrong. Please try again.");
                    Console.WriteLine($"[Error Details: {ex.Message}]"); // For debugging purposes (can be removed in production)
                }
            }
        }

        // Rest of the QuestionHandler class remains unchanged
        private void StoreReplies()
        {
            // Add single responses
            singleResponses.Add(new DictionaryEntry("how are you", "I'm just a bot, but I'm running smoothly!"));
            singleResponses.Add(new DictionaryEntry("what is your purpose", "I provide cybersecurity knowledge to help keep you safe online."));

            // Add keyword responses
            keywordResponses["spoofing"] = new List<string>
                {
                    "Avoid talking to people you don't know, especially online.",
                    "Verify the identity of the person contacting you before sharing information.",
                    "Be cautious of emails or calls claiming to be from trusted organizations.",
                    "Check email headers to identify spoofed email addresses.",
                    "Use anti-spoofing tools or software to protect your devices."
                };

            keywordResponses["password safety"] = new List<string>
                {
                    "Use long, unique passwords and a password manager!",
                    "Avoid using personal details in your passwords.",
                    "Change your passwords regularly to enhance security.",
                    "Enable two-factor authentication for added protection.",
                    "Never share your passwords with anyone."
                };

            keywordResponses["phishing"] = new List<string>
                {
                    "Beware of emails or messages asking for personal information. Always verify the sender.",
                    "Never click on suspicious links or download unknown attachments.",
                    "Look for spelling errors or generic greetings in phishing emails.",
                    "Enable two-factor authentication to protect your accounts.",
                    "Report phishing attempts to your email provider or IT department."
                };

            keywordResponses["encryption"] = new List<string>
                {
                    "Encryption helps protect your sensitive data from unauthorized access.",
                    "Always use encrypted communication channels like HTTPS or VPNs.",
                    "Encrypt sensitive files before sharing them online.",
                    "Use end-to-end encryption for messaging apps.",
                    "Ensure your Wi-Fi network is encrypted with WPA3 or WPA2."
                };

            // Add sentimental responses for each keyword
            sentimentalResponses["spoofing"] = new Dictionary<string, string>
                {
                    { "worried", "It's okay to feel worried about spoofing. Let me guide you on how to identify and avoid spoofing attempts." },
                    { "unsure", "If you're unsure about spoofing, it's when someone pretends to be someone else to gain your trust. Let me explain further." },
                    { "overwhelmed", "Spoofing can feel overwhelming, but don't worry. Start by verifying the identity of anyone contacting you." }
                };

            sentimentalResponses["password safety"] = new Dictionary<string, string>
                {
                    { "worried", "It's okay to feel worried about password safety. Let me guide you on creating strong, unique passwords for each account." },
                    { "unsure", "If you're unsure about password safety, it involves using strong, unique passwords and enabling two-factor authentication. Let me explain more." },
                    { "overwhelmed", "Password safety can seem overwhelming, but it's manageable. Start with a password manager to simplify the process." }
                };

            sentimentalResponses["phishing"] = new Dictionary<string, string>
                {
                    { "worried", "It's completely understandable to feel worried about phishing. Scammers can be very convincing. Let me share some tips to help you stay safe." },
                    { "unsure", "If you're unsure about phishing, it's when attackers try to trick you into giving personal information. Let me explain further." },
                    { "overwhelmed", "I know phishing can feel overwhelming. Take it one step at a time. Start by verifying the sender of emails and avoiding suspicious links." }
                };

            sentimentalResponses["encryption"] = new Dictionary<string, string>
                {
                    { "worried", "It's okay to feel worried about encryption. Let me explain how it protects your sensitive data from unauthorized access." },
                    { "unsure", "If you're unsure about encryption, it's a way to scramble your data so only authorized parties can read it. Let me explain more." },
                    { "overwhelmed", "Encryption can seem overwhelming, but it's essential for security. Start by using encrypted communication channels like HTTPS or VPNs." }
                };
        }

        private string GetSingleResponse(string input)
        {
            foreach (DictionaryEntry entry in singleResponses)
            {
                if ((string)entry.Key == input)
                {
                    return (string)entry.Value;
                }
            }
            return null;
        }

        private string GetRandomKeywordResponse(string input)
        {
            foreach (var keyword in keywordResponses.Keys)
            {
                if (input.Contains(keyword))
                {
                    var possibleResponses = keywordResponses[keyword];
                    return possibleResponses[random.Next(possibleResponses.Count)];
                }
            }
            return null;
        }

        private string GetKeywordFromInput(string input)
        {
            foreach (var keyword in keywordResponses.Keys)
            {
                if (input.Contains(keyword))
                {
                    return keyword;
                }
            }
            return null;
        }

        private string DetectSentiment(string input)
        {
            if (input.Contains("worried") || input.Contains("scared") || input.Contains("anxious"))
            {
                return "worried";
            }
            if (input.Contains("unsure") || input.Contains("confused") || input.Contains("uncertain"))
            {
                return "unsure";
            }
            if (input.Contains("overwhelmed") || input.Contains("stressed") || input.Contains("frustrated"))
            {
                return "overwhelmed";
            }
            return null;
        }

        private string GetSentimentalResponse(string input, string sentiment)
        {
            string keyword = GetKeywordFromInput(input);
            if (keyword != null && sentiment != null && sentimentalResponses.ContainsKey(keyword))
            {
                var responses = sentimentalResponses[keyword];
                if (responses.ContainsKey(sentiment))
                {
                    return responses[sentiment];
                }
            }
            return null;
        }
    }
}