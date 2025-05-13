using System.Collections.Generic;

namespace CyberSecurityChatBotAI
{
    public class Memory
    {
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
    }
}
