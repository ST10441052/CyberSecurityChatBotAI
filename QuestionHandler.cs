using MiNET.Plugins;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CyberSecurityChatBotAI
{
   
    public class QuestionHandler // Handles user questions and provides responses based on keywords, sentiment, or predefined answers
    {
        private ArrayList singleResponses = new ArrayList();
        private Dictionary<string, List<string>> keywordResponses = new Dictionary<string, List<string>>();
        private Dictionary<string, Dictionary<string, string>> sentimentalResponses = new Dictionary<string, Dictionary<string, string>>();
        private Random random = new Random();
        private string? currentTopic = null; // Nullable
        private Memory memory;

        public QuestionHandler(Memory memory)
        {
            this.memory = memory;
            StoreReplies();
        }
        /// Handles user questions and provides responses based on keywords, sentiment, or predefined answers
        public void HandleQuestions()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine($"Chat AI-> {memory.UserName}, enter your question (or 'exit' to go back to the main menu):");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"{memory.UserName} -> ");
                    string? question = Console.ReadLine()?.ToLower();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    string? response = null;

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
                        memory.RecordConversation(question, response);
                        return;
                    }

                    string? sentiment = DetectSentiment(question);

                    string? sentimentalResponse = GetSentimentalResponse(question, sentiment);
                    if (sentimentalResponse != null)
                    {
                        response = sentimentalResponse;
                        Console.WriteLine($"Chat AI -> {response}");
                        memory.RecordConversation(question, response);
                        currentTopic = GetKeywordFromInput(question);
                        continue;
                    }

                    string? singleResponse = GetSingleResponse(question);
                    if (singleResponse != null)
                    {
                        response = singleResponse;
                        Console.WriteLine($"Chat AI -> {response}");
                        memory.RecordConversation(question, response);
                        currentTopic = null;
                        continue;
                    }

                    string? randomResponse = GetRandomKeywordResponse(question);
                    if (randomResponse != null)
                    {
                        response = randomResponse;
                        Console.WriteLine($"Chat AI -> {response}");
                        memory.RecordConversation(question, response);
                        currentTopic = GetKeywordFromInput(question);
                        continue;
                    }

                    response = "I'm not sure I understand. Can you try rephrasing your cybersecurity question?";
                    Console.WriteLine($"Chat AI -> {response}");
                    memory.RecordConversation(question, response);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Chat AI -> Oops! Something went wrong. Please try again.");
                    Console.WriteLine($"[Error Details: {ex.Message}]");
                }
            }
        }

        private void StoreReplies()
        {
            singleResponses.Add(new DictionaryEntry("how are you", "I'm just a bot, but I'm running smoothly and ready to help with cybersecurity!"));
            singleResponses.Add(new DictionaryEntry("what is your purpose", "I provide cybersecurity knowledge to help keep you safe online."));
            singleResponses.Add(new DictionaryEntry("hello", "Hello there! I'm here to help you with cybersecurity questions."));
            singleResponses.Add(new DictionaryEntry("hi", "Hi! Ready to learn about staying safe online?"));
            singleResponses.Add(new DictionaryEntry("thank you", "You're welcome! Stay safe out there!"));
            singleResponses.Add(new DictionaryEntry("help", "I can help you with cybersecurity topics like phishing, malware, passwords, and much more!"));

            //add keyword responses for cybersecurity topics 
            keywordResponses["spoofing"] = new List<string>
            {
                "Avoid talking to people you don't know, especially online.",
                "Verify the identity of the person contacting you before sharing information.",
                "Be cautious of emails or calls claiming to be from trusted organizations.",
                "Check email headers to identify spoofed email addresses.",
                "Use anti-spoofing tools or software to protect your devices.",
                "Look for inconsistencies in email addresses and domain names.",
                "Never provide personal information through unsolicited communications."
            };

            keywordResponses["password safety"] = new List<string>
            {
                "Use long, unique passwords and a password manager!",
                "Avoid using personal details in your passwords.",
                "Change your passwords regularly to enhance security.",
                "Enable two-factor authentication for added protection.",
                "Never share your passwords with anyone.",
                "Use a combination of uppercase, lowercase, numbers, and symbols.",
                "Avoid using the same password across multiple accounts.",
                "Consider using passphrases instead of traditional passwords."
            };

            keywordResponses["phishing"] = new List<string>
            {
                "Beware of emails or messages asking for personal information. Always verify the sender.",
                "Never click on suspicious links or download unknown attachments.",
                "Look for spelling errors or generic greetings in phishing emails.",
                "Enable two-factor authentication to protect your accounts.",
                "Report phishing attempts to your email provider or IT department.",
                "Check the sender's email address carefully for inconsistencies.",
                "When in doubt, contact the organization directly through official channels.",
                "Hover over links to see the actual destination before clicking."
            };

            keywordResponses["encryption"] = new List<string>
            {
                "Encryption helps protect your sensitive data from unauthorized access.",
                "Always use encrypted communication channels like HTTPS or VPNs.",
                "Encrypt sensitive files before sharing them online.",
                "Use end-to-end encryption for messaging apps.",
                "Ensure your Wi-Fi network is encrypted with WPA3 or WPA2.",
                "Consider using full-disk encryption on your devices.",
                "Use encrypted cloud storage services for sensitive data."
            };

            keywordResponses["malware"] = new List<string>
            {
                "Keep your antivirus software updated and run regular scans.",
                "Avoid downloading software from untrusted sources.",
                "Be cautious when clicking on ads or pop-ups online.",
                "Keep your operating system and software up to date.",
                "Use a firewall to block unauthorized network access.",
                "Don't open email attachments from unknown senders.",
                "Backup your data regularly to protect against ransomware.",
                "Use application whitelisting when possible."
            };

            keywordResponses["vpn"] = new List<string>
            {
                "VPNs encrypt your internet traffic and hide your IP address.",
                "Use a VPN when connecting to public Wi-Fi networks.",
                "Choose a reputable VPN provider with a no-logs policy.",
                "VPNs can help protect your privacy and bypass geo-restrictions.",
                "Make sure your VPN uses strong encryption protocols.",
                "Avoid free VPN services as they may compromise your privacy.",
                "Consider using a VPN for all your internet activities."
            };

            keywordResponses["firewall"] = new List<string>
            {
                "Firewalls act as a barrier between your device and potential threats.",
                "Enable your operating system's built-in firewall.",
                "Configure your firewall to block unnecessary incoming connections.",
                "Consider using both hardware and software firewalls.",
                "Regularly review and update your firewall rules.",
                "A firewall is your first line of defense against network attacks.",
                "Don't disable your firewall unless absolutely necessary."
            };

            keywordResponses["social engineering"] = new List<string>
            {
                "Be skeptical of unsolicited requests for information or access.",
                "Verify the identity of anyone requesting sensitive information.",
                "Don't let urgency or authority pressure you into hasty decisions.",
                "Be cautious about sharing personal information on social media.",
                "Train yourself to recognize manipulation techniques.",
                "When in doubt, consult with a trusted colleague or supervisor.",
                "Social engineers often exploit human psychology rather than technology."
            };

            keywordResponses["data breach"] = new List<string>
            {
                "Monitor your accounts regularly for unauthorized activity.",
                "Change passwords immediately if you suspect a breach.",
                "Enable account notifications for login attempts and changes.",
                "Consider using identity monitoring services.",
                "Report suspected breaches to the relevant authorities.",
                "Understand what data was compromised and take appropriate action.",
                "Learn from breaches to improve your security practices."
            };

            keywordResponses["wifi security"] = new List<string>
            {
                "Always use WPA3 or WPA2 encryption on your wireless networks.",
                "Avoid connecting to open, unsecured Wi-Fi networks.",
                "Change default router passwords and network names.",
                "Regularly update your router's firmware.",
                "Use a VPN when connecting to public Wi-Fi.",
                "Disable WPS (Wi-Fi Protected Setup) if not needed.",
                "Consider hiding your network name (SSID) for added security."
            };

            foreach (var keyword in keywordResponses.Keys)
            {
                sentimentalResponses[keyword] = new Dictionary<string, string>
                {
                    { "worried", $"It's completely normal to feel worried about {keyword}. Let me help you understand how to protect yourself." },
                    { "unsure", $"If you're unsure about {keyword}, don't worry - I'll explain it in simple terms." },
                    { "overwhelmed", $"{char.ToUpper(keyword[0]) + keyword.Substring(1)} can feel overwhelming, but we'll take it step by step." },
                    { "confused", $"Feeling confused about {keyword} is understandable. Let me break it down for you." },
                    { "scared", $"It's okay to feel scared about {keyword}. Knowledge is power - let me help you feel more confident." },
                    { "frustrated", $"I understand your frustration with {keyword}. Let's work through this together." },
                    { "anxious", $"Feeling anxious about {keyword} is natural. I'm here to help ease your concerns." },
                    { "curious", $"Great question about {keyword}! I love when people are curious about cybersecurity." },
                    { "excited", $"I'm excited to help you learn about {keyword}! It's an important topic." },
                    { "stressed", $"Don't let {keyword} stress you out. With the right knowledge, you can handle this confidently." }
                };
            }
        }

        private string? GetSingleResponse(string input)
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

        private string? GetRandomKeywordResponse(string input)
        {
            foreach (var keyword in keywordResponses.Keys)
            {
                if (input.Contains(keyword))
                {
                    var responses = keywordResponses[keyword];
                    return responses[random.Next(responses.Count)];
                }
            }
            return null;
        }

        private string? GetKeywordFromInput(string input)
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

        private string? DetectSentiment(string input)
        {
            if (input.Contains("worried") || input.Contains("worry") || input.Contains("concern")) return "worried";
            if (input.Contains("unsure") || input.Contains("uncertain") || input.Contains("not sure")) return "unsure";
            if (input.Contains("overwhelmed") || input.Contains("too much") || input.Contains("overwhelming")) return "overwhelmed";
            if (input.Contains("confused") || input.Contains("confusing") || input.Contains("don't understand")) return "confused";
            if (input.Contains("scared") || input.Contains("afraid") || input.Contains("frightened")) return "scared";
            if (input.Contains("frustrated") || input.Contains("annoyed") || input.Contains("irritated")) return "frustrated";
            if (input.Contains("anxious") || input.Contains("nervous")) return "anxious";
            if (input.Contains("curious") || input.Contains("interested") || input.Contains("want to know")) return "curious";
            if (input.Contains("excited") || input.Contains("eager") || input.Contains("looking forward")) return "excited";
            if (input.Contains("stressed") || input.Contains("pressure")) return "stressed";
            return null;
        }

        private string? GetSentimentalResponse(string input, string? sentiment)
        {
            string? keyword = GetKeywordFromInput(input);
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
        // / Main method to get a response based on user input ( now  method checks for more than 1 keyword response )
        // added NLP response check to handle more complex queries
        internal string GetResponse(string userInput)
        {
            string? nlpResponse = GetNlpResponse(userInput);
            if (nlpResponse != null)
                return nlpResponse;

            string? sentiment = DetectSentiment(userInput);
            string? sentimentalResponse = GetSentimentalResponse(userInput, sentiment);
            if (sentimentalResponse != null)
                return sentimentalResponse;

            string? singleResponse = GetSingleResponse(userInput);
            if (singleResponse != null)
                return singleResponse;

            string? keywordResponse = GetRandomKeywordResponse(userInput);
            if (keywordResponse != null)
                return keywordResponse;

            return "I'm not sure I understand. Can you try rephrasing?";
        }

        // ... inside QuestionHandler class

        // This method uses simple NLP techniques to identify tasks, reminders, quizzes, and cybersecurity topics
        private string? GetNlpResponse(string input)
        {

            if (!input.Contains("show activity log") && !input.Contains("what have you done") && !Input.Contains("recent actions") && !input.Contains("activity log"))
            {
                if (input.Contains("show activity log") || input.Contains("what have you done") || input.Contains("recent actions") || input.Contains("activity log"))

                    input = input.ToLower();

                // Add Task or Reminder
                if (Regex.IsMatch(input, @"(add|create|set)\s+(a\s+)?(task|reminder)", RegexOptions.IgnoreCase) ||
                    input.Contains("remind me to") || input.Contains("reminder"))
                {
                    // Extract the task/reminder description
                    var match = Regex.Match(input, @"remind me to (.+?)( on| at|$)", RegexOptions.IgnoreCase);
                    string description = match.Success ? match.Groups[1].Value.Trim() : null;

                    if (description != null)
                    {
                        return $"Reminder set for '{description}'.";
                    }

                    match = Regex.Match(input, @"add (a )?task to (.+)", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        description = match.Groups[2].Value.Trim();
                        return $"Task added: '{description}'. Would you like to set a reminder for this task?";
                    }

                    return "What would you like to be reminded about?";
                }

                // Quiz
                if (input.Contains("quiz") || Regex.IsMatch(input, @"(play|start|begin).*(quiz|game)", RegexOptions.IgnoreCase))
                {
                    return "Would you like to start the cybersecurity quiz?";
                }

                // Summary
                if (input.Contains("what have you done") || input.Contains("recent actions") || input.Contains("summary"))
                {
                    // This is a placeholder. You can enhance this to pull from a real action log.
                    return "Here's a summary of recent actions:\n1. Reminder set for 'Update my password' on tomorrow.\n2. Task added: 'Enable two-factor authentication' (no reminder set).";
                }

                // Cybersecurity keywords
                string[] securityKeywords = { "password", "phishing", "2fa", "encryption", "malware", "vpn", "firewall", "social engineering", "data breach", "wifi security" };
                foreach (var keyword in securityKeywords)
                {
                    if (input.Contains(keyword))
                    {
                        return $"Would you like to learn more about {keyword}?";
                    }
                }

                return null;
            }
            var recent = memory.GetRecentActivity();
            if (recent.Count == 0)
                return "No recent actions yet.";
            return "Here's a summary of recent actions:\n" + string.Join("\n", recent.Select((a, i) => $"{i + 1}. {a}"));
        }
    }
}