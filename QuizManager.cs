using CyberSecurityChatBotAI_poe;
using System.Collections.Generic;

namespace CyberSecurityChatBotAI
{
    public class QuizManager
    {
        private readonly List<QuizQuestion> questions;
        private int currentIndex;
        public int Score { get; private set; }

        public QuizManager()
        {
            questions = new List<QuizQuestion>
            {
                new QuizQuestion {
                    QuestionText = "What should you do if you receive an email asking for your password?",
                    Options = new[] { "A) Reply with your password", "B) Delete the email", "C) Report the email as phishing", "D) Ignore it" },
                    CorrectOptionIndex = 2,
                    Explanation = "Correct! Reporting phishing emails helps prevent scams.",
                    IsMultipleChoice = true
                },
                new QuizQuestion {
                    QuestionText = "True or False: Using the same password for multiple accounts is safe.",
                    Options = new[] { "True", "False" },
                    CorrectOptionIndex = 1,
                    Explanation = "False! Reusing passwords increases your risk if one account is compromised.",
                    IsMultipleChoice = false
                },
                new QuizQuestion {
                    QuestionText = "Which of the following is a strong password?",
                    Options = new[] { "A) password123", "B) Qw!7x$2Lp#", "C) 123456", "D) mydogname" },
                    CorrectOptionIndex = 1,
                    Explanation = "A strong password uses a mix of letters, numbers, and symbols.",
                    IsMultipleChoice = true
                },
                new QuizQuestion {
                    QuestionText = "True or False: You should click links in emails from unknown senders.",
                    Options = new[] { "True", "False" },
                    CorrectOptionIndex = 1,
                    Explanation = "Never click links from unknown senders—they may be phishing attempts.",
                    IsMultipleChoice = false
                },
                new QuizQuestion {
                    QuestionText = "What is phishing?",
                    Options = new[] { "A) A type of malware", "B) A scam to trick you into giving info", "C) A firewall", "D) A password manager" },
                    CorrectOptionIndex = 1,
                    Explanation = "Phishing is a scam to trick you into revealing sensitive information.",
                    IsMultipleChoice = true
                },
                new QuizQuestion {
                    QuestionText = "Which is the safest way to connect to public Wi-Fi?",
                    Options = new[] { "A) Without protection", "B) Using a VPN", "C) With Bluetooth on", "D) Using default passwords" },
                    CorrectOptionIndex = 1,
                    Explanation = "A VPN encrypts your data on public Wi-Fi.",
                    IsMultipleChoice = true
                },
                new QuizQuestion {
                    QuestionText = "True or False: Two-factor authentication adds extra security.",
                    Options = new[] { "True", "False" },
                    CorrectOptionIndex = 0,
                    Explanation = "2FA adds an extra layer of security to your accounts.",
                    IsMultipleChoice = false
                },
                new QuizQuestion {
                    QuestionText = "What is social engineering?",
                    Options = new[] { "A) Hacking computers", "B) Manipulating people", "C) Building networks", "D) Encrypting data" },
                    CorrectOptionIndex = 1,
                    Explanation = "Social engineering manipulates people to gain confidential info.",
                    IsMultipleChoice = true
                },
                new QuizQuestion {
                    QuestionText = "Which of these is a sign of a phishing website?",
                    Options = new[] { "A) HTTPS in the URL", "B) Misspelled domain", "C) Padlock icon", "D) Professional design" },
                    CorrectOptionIndex = 1,
                    Explanation = "Misspelled domains are a common phishing sign.",
                    IsMultipleChoice = true
                },
                new QuizQuestion {
                    QuestionText = "True or False: You should update your software regularly.",
                    Options = new[] { "True", "False" },
                    CorrectOptionIndex = 0,
                    Explanation = "Regular updates patch security vulnerabilities.",
                    IsMultipleChoice = false
                }
            };
            currentIndex = 0;
            Score = 0;
        }

        public QuizQuestion GetCurrentQuestion() => currentIndex < questions.Count ? questions[currentIndex] : null;

        public bool CheckAnswer(int selectedIndex, out string explanation)
        {
            var question = GetCurrentQuestion();
            if (question == null)
            {
                explanation = "";
                return false;
            }
            bool correct = selectedIndex == question.CorrectOptionIndex;
            if (correct) Score++;
            explanation = question.Explanation;
            return correct;
        }

        public bool NextQuestion()
        {
            currentIndex++;
            return currentIndex < questions.Count;
        }

        public void Reset()
        {
            currentIndex = 0;
            Score = 0;
        }

        public int TotalQuestions => questions.Count;
        public int CurrentQuestionNumber => currentIndex + 1;
    }
}
