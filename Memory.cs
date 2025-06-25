using System;
using System.Collections.Generic;
using System.IO;

namespace CyberSecurityChatBotAI
{
    public class Memory
    {
        public string UserName { get; set; }
        public string FavoriteTopic { get; set; }
        private List<(string UserInput, string Response)> conversationHistory;

        public Memory()
        {
            conversationHistory = new List<(string UserInput, string Response)>();
        }

        public void RecordConversation(string userInput, string response)
        {
            conversationHistory.Add((userInput, response));
        }

        public string GetConversationHistory()
        {
            if (conversationHistory.Count == 0)
                return "No conversation history available.";

            var history = new System.Text.StringBuilder();
            foreach (var entry in conversationHistory)
            {
                history.AppendLine($"User: {entry.UserInput}");
                history.AppendLine($"Bot: {entry.Response}");
            }

            return history.ToString();
        }

        private List<string> activityLog = new List<string>();

        public void LogActivity(string description)
        {
            activityLog.Add($"[{DateTime.Now:HH:mm}] {description}");
            if (activityLog.Count > 50) // Keep log size manageable
                activityLog.RemoveAt(0);
        }

        public List<string> GetRecentActivity(int count = 10)
        {
            int skip = Math.Max(0, activityLog.Count - count);
            return activityLog.Skip(skip).ToList();
        }
        public void SetDetail(string key, string value)
        {
            // Implementation for storing additional details  
        }
      

    }
}
    
    