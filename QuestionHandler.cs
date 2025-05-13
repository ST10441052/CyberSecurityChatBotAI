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
        private Dictionary<string, string> clarificationResponses = new Dictionary<string, string>();
        private Random random = new Random();
        private string currentTopic = null; // so this is what tracks the current topic
        private Memory memory; // this is a Memory instance for me to store the  user details

        //Constrructor that I use to initialize the memory instance
        public QuestionHandler(Memory memory)
        {
            this.memory = memory;
            StoreReplies();
        }
        // This method handles user questions and provides responses based on keywords or single responses.
        public void HandleQuestions()
        {
            while (true)
            {
                Console.WriteLine($"Chat AI-> {memory.UserName}, enter your question (or 'exit' to go back to the main menu):");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{memory.UserName} -> ");
                string question = Console.ReadLine()?.ToLower();
                Console.ForegroundColor = ConsoleColor.Cyan;

                if (question == "exit")
                {
                    Console.WriteLine($"Goodbye, {memory.UserName}! Feel free to come back anytime.");
                    return;
                }

                // Handle follow-up questions or confusion
                if (IsFollowUpQuestion(question))
                {
                    HandleFollowUp();
                    continue;
                }

                // Check for exact match in single responses
                string singleResponse = GetSingleResponse(question);
                if (singleResponse != null)
                {
                    Console.WriteLine($"Chat AI -> {singleResponse}");
                    currentTopic = null; // Reset topic for general questions
                    continue;
                }

                // Check for keyword match with random response
                string randomResponse = GetRandomKeywordResponse(question);
                if (randomResponse != null)
                {
                    Console.WriteLine($"Chat AI -> {randomResponse}");
                    currentTopic = GetKeywordFromInput(question); // Update current topic
                    continue;
                }

                // Ask for favorite topic if not set
                if (memory.FavoriteTopic == null)
                {
                    Console.WriteLine("Chat AI -> By the way, do you have a favorite cybersecurity topic? (e.g., phishing, password safety)");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"{memory.UserName} -> ");
                    memory.FavoriteTopic = Console.ReadLine()?.ToLower();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"Chat AI -> Got it! I'll remember that your favorite topic is {memory.FavoriteTopic}.");
                    continue;
                }

                // Default response if no match is found
                Console.WriteLine("Chat AI-> I couldn't find an answer. Try rephrasing your question.");
            }
        }

        private void StoreReplies()
        {
            // Add single responses
            singleResponses.Add(new DictionaryEntry("how are you", "I'm just a bot, but I'm running smoothly!"));
            singleResponses.Add(new DictionaryEntry("what is your purpose", "I provide cybersecurity knowledge to help keep you safe online."));

            // Add keyword responses
            keywordResponses["safe browsing"] = new List<string>
            {
                "Use secure websites (HTTPS) and avoid downloading from unknown sources.",
                "Keep your browser updated to the latest version.",
                "Avoid clicking on pop-ups or suspicious ads.",
                "Use browser extensions that enhance security, like ad blockers.",
                "Clear your browser cache and cookies regularly."
            };

            keywordResponses["encryption"] = new List<string>
            {
                "Encryption helps protect your sensitive data from unauthorized access.",
                "Always use encrypted communication channels like HTTPS or VPNs.",
                "Encrypt sensitive files before sharing them online.",
                "Use end-to-end encryption for messaging apps.",
                "Ensure your Wi-Fi network is encrypted with WPA3 or WPA2."
            };

            keywordResponses["phishing"] = new List<string>
            {
                "Beware of emails or messages asking for personal information. Always verify the sender.",
                "Never click on suspicious links or download unknown attachments.",
                "Look for spelling errors or generic greetings in phishing emails.",
                "Enable two-factor authentication to protect your accounts.",
                "Report phishing attempts to your email provider or IT department."
            };

            keywordResponses["password safety"] = new List<string>
            {
                "Use long, unique passwords and a password manager!",
                "Avoid using personal details in your passwords.",
                "Change your passwords regularly to enhance security.",
                "Enable two-factor authentication for added protection.",
                "Never share your passwords with anyone."
            };

            keywordResponses["spoofing"] = new List<string>
            {
                "Avoid talking to people you don't know, especially online.",
                "Verify the identity of the person contacting you before sharing information.",
                "Be cautious of emails or calls claiming to be from trusted organizations.",
                "Check email headers to identify spoofed email addresses.",
                "Use anti-spoofing tools or software to protect your devices."
            };

            // Add clarification responses for each topic
            clarificationResponses["safe browsing"] = "Safe browsing means using secure websites and avoiding risky online behavior. Would you like more tips?";
            clarificationResponses["encryption"] = "Encryption scrambles your data to protect it from unauthorized access. Do you need more details?";
            clarificationResponses["phishing"] = "Phishing is a scam where attackers trick you into giving personal information. Should I explain further?";
            clarificationResponses["password safety"] = "Password safety involves using strong, unique passwords for each account. Want to know more?";
            clarificationResponses["spoofing"] = "Spoofing is when someone pretends to be someone else to gain your trust. Need more clarification?";
        }

        // This method checks if the input matches any of the single responses and returns the first one found.
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
        // This method checks if the input contains any of the keywords and returns a random response from the list.
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
        // This method checks if the input contains any of the keywords and returns the first one found.
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
        // This method checks if the input is a follow-up question or indicates confusion.

        private bool IsFollowUpQuestion(string input)
        {
            return input.Contains("more") || input.Contains("explain") || input.Contains("details") || input.Contains("i don't understand");
        }

        // This method handles follow-up questions or confusion.
        private void HandleFollowUp()
        {
            if (currentTopic != null)
            {
                if (keywordResponses.ContainsKey(currentTopic))
                {
                    var possibleResponses = keywordResponses[currentTopic];
                    Console.WriteLine($"Chat AI -> {possibleResponses[random.Next(possibleResponses.Count)]}");
                }
                else if (clarificationResponses.ContainsKey(currentTopic))
                {
                    Console.WriteLine($"Chat AI -> {clarificationResponses[currentTopic]}");
                }
            }
            else
            {
                Console.WriteLine("Chat AI -> I'm not sure what you're referring to. Could you clarify?");
            }
        }
    }


    //User Input:
//how are you?
//chatbot response:
//i'm just a bot, but i'm running smoothly!
//user input:
//tell me about phishing.
//chatbot response:
//beware of emails or messages asking for personal information. always verify the sender.
//user input:
//can you explain more?
//chatbot response:
//phishing is a scam where attackers trick you into giving personal information. should i explain further?
//user input:
//i like password safety.
//chatbot response:
//got it! i'll remember that your favorite topic is password safety.