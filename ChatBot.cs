using System;
using System.Text.RegularExpressions;

namespace CyberSecurityChatBotAI
{
    // Handles user interaction and chatbot operation
    public class ChatBot
    {
        private string name;
        private AudioImageHandler mediaHandler;
        private QuestionHandler questionHandler;
        private Memory memory;

        public ChatBot()
        {
            memory = new Memory(); // Initialize Memory
            mediaHandler = new AudioImageHandler();
            questionHandler = new QuestionHandler(memory); // Pass memory to QuestionHandler
        }

        public void Run()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                ShowLoading();
                mediaHandler.DisplayLogo();
                mediaHandler.PlayWelcomeAudio();
                WelcomeUser();
                Menu();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
                Console.ResetColor();
            }
        }

        private void ShowLoading(int seconds = 3)
        {
            Console.Write("\n Starting up chatbot AI");
            for (int i = 0; i < seconds; i++)
            {
                Console.Write(".");
                System.Threading.Thread.Sleep(1000);
            }
            Console.WriteLine();
        }

        private void WelcomeUser()
        {
            Console.WriteLine("\n===================================================================================================");
            Console.WriteLine("CHAT AI-> Hello! Welcome to the Cybersecurity Awareness Bot. I'm here to help you stay safe online.");
            Console.WriteLine("===================================================================================================");
            Console.WriteLine("Chat AI-> Please enter your full name:");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("User-> ");
            name = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(name) || Regex.IsMatch(name, @"\d"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input! Name cannot be empty or contain numbers. Please enter a valid name:");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("User-> ");
                name = Console.ReadLine();
            }
            memory.UserName = name; // Set the name in memory
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n===================================================================================================");
            Console.WriteLine("Chat AI-> Well hello there: " + name);
            Console.WriteLine("===================================================================================================");
        }

        // Update the Menu method to call HandleQuestions without arguments
        private void Menu()
        {
            while (true)
            {
                Console.WriteLine("\n===================================================================================================");
                Console.WriteLine("Chat AI-> Would you like to ask any questions? (yes/no)");
                Console.WriteLine("===================================================================================================");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(name + "-> ");
                string answer = Console.ReadLine()?.ToLower();
                Console.ForegroundColor = ConsoleColor.Cyan;

                switch (answer)
                {
                    case "yes":
                        questionHandler.HandleQuestions();
                        break;
                    case "no":
                        Console.WriteLine("\n===================================================================================================");
                        Console.WriteLine("That's a shame. Feel free to come back if you have questions.");
                        Console.WriteLine("===================================================================================================");
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n===================================================================================================");
                        Console.WriteLine("Invalid input! Please enter 'yes' or 'no'.");
                        Console.WriteLine("===================================================================================================");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        break;
                }
            }
        }
    }
}
