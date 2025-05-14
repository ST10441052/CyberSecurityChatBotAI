using System.Collections.Generic;

namespace CyberSecurityChatBotAI
{
    public class Memory
    {
        private const string ConversationLogFile = "ConversationLog.txt";

        public string UserName { get; set; }
        public string FavoriteTopic { get; set; }

        private Dictionary<string, string> additionalDetails = new Dictionary<string, string>();

        public void SetDetail(string key, string value)
        {
            additionalDetails[key] = value;
        }

        public string GetDetail(string key)
        {
            return additionalDetails.ContainsKey(key) ? additionalDetails[key] : null;
        }

        /// <summary>
        /// Records a conversation entry with the user's question and the AI's response
        /// </summary>
        /// <param name="question">The question asked by the user</param>
        /// <param name="response">The response provided by the AI</param>
        public void RecordConversation(string question, string response)
        {
            try
            {
                // Create a timestamped entry
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string entry = $"[{timestamp}] User: {UserName}\r\nQuestion: {question}\r\nResponse: {response}\r\n";

                // Append to the conversation log file
                File.AppendAllText(ConversationLogFile, entry + "\r\n");
            }
            catch (Exception ex)
            {
                // Log the error (could be enhanced with proper logging)
                Console.WriteLine($"Error recording conversation: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all recorded conversations from the log file
        /// </summary>
        /// <returns>The contents of the conversation log file</returns>
        public string GetConversationHistory()
        {
            try
            {
                if (File.Exists(ConversationLogFile))
                {
                    return File.ReadAllText(ConversationLogFile);
                }
                return "No conversation history found.";
            }
            catch (Exception ex)
            {
                return $"Error retrieving conversation history: {ex.Message}";
            }
        }
    }
}
