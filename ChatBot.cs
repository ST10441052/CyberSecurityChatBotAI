using System;
using System.Text.RegularExpressions;

namespace CyberSecurityChatBotAI
{
    // Updated ChatBot class for GUI integration  
    public class ChatBot
    {
        private string name;
        private string userName;
        //private AudioImageHandler mediaHandler;
        private QuestionHandler questionHandler;
        private Memory memory;

        public ChatBot(string botName = "CyberBot")
        {
            name = botName;
           // mediaHandler = new AudioImageHandler();
            memory = new Memory();
            questionHandler = new QuestionHandler(memory); // Pass 'memory' to the QuestionHandler constructor  
        }

        public string ProcessMessage(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
                return "I didn't receive any input. Please ask me something about cybersecurity!";

            // Store user input and get response  
            string response = questionHandler.GetResponse(userInput);

            // Record conversation in memory  
            if (!string.IsNullOrWhiteSpace(userName))
            {
                memory.UserName = userName;
                memory.RecordConversation(userInput, response);
            }

            return response;
        }

        public void SetUserName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                userName = name.Trim();
                memory.UserName = userName;

                // Store in memory for personalization  
                memory.SetDetail("UserName", userName);
                memory.SetDetail("FirstContact", DateTime.Now.ToString());
            }
        }

        public string GetUserName()
        {
            return userName ?? "User";
        }

        public void SetUserFavoriteTopic(string topic)
        {
            if (!string.IsNullOrWhiteSpace(topic))
            {
                memory.FavoriteTopic = topic.Trim();
                memory.SetDetail("FavoriteTopic", topic.Trim());
            }
        }

        public string GetUserFavoriteTopic()
        {
            return memory.FavoriteTopic ?? "General Cybersecurity";
        }


        public string GetConversationHistory()
        {
            return memory.GetConversationHistory();
        }

        public Memory GetMemory()
        {
            return memory;
        }

        public string GetPersonalizedGreeting()
        {
            if (string.IsNullOrWhiteSpace(userName))
                return "Hello! I'm your cybersecurity assistant. What would you like to know?";

            string greeting = $"Hello {userName}! ";

            // Add personalized content based on memory  
            string favTopic = GetUserFavoriteTopic();
            if (favTopic != "General Cybersecurity")
            {
                greeting += $"Ready to discuss more about {favTopic}? ";
            }

            greeting += "How can I help you stay secure today?";
            return greeting;
        }

        public string GetTaskRelatedResponse(string input)
        {
            string lowerInput = input.ToLower();

            if (lowerInput.Contains("task") || lowerInput.Contains("reminder") || lowerInput.Contains("todo"))
            {
                return "I see you're interested in task management! You can use the task panel to add cybersecurity-related tasks with reminders. This helps you stay on top of important security practices!";
            }

            return ProcessMessage(input);
        }

        // Method to get contextual responses based on conversation history  
        public string GetContextualResponse(string userInput)
        {
            string response = questionHandler.GetResponse(userInput);

            // Add personal touches if we know the user  
            if (!string.IsNullOrWhiteSpace(userName))
            {
                // Add user's name occasionally for personalization  
                Random rand = new Random();
                if (rand.Next(1, 4) == 1) // 33% chance  
                {
                    response = response.Replace("you", userName).Replace("You", userName);
                }
            }

            return response;
        }

        // Get bot statistics for display  
        public string GetBotStats()
        {
            return $"Bot Name: {name}\nUser: {GetUserName()}\nFavorite Topic: {GetUserFavoriteTopic()}";
        }
    }
}
