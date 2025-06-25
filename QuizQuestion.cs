using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSecurityChatBotAI_poe
{
    public class QuizQuestion
    {
        public string QuestionText { get; set; }
        public string[] Options { get; set; }
        public int CorrectOptionIndex { get; set; }
        public string Explanation { get; set; }
        public bool IsMultipleChoice { get; set; }
    }
}
