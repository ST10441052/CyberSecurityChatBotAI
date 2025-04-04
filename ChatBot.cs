using System;

namespace CyberSecurityChatBotAI
{
    // Handles user interaction and chatbot operation
    public class ChatBot
    {
        private string name;
        private AudioImageHandler mediaHandler;
        private QuestionHandler questionHandler;

        public ChatBot()
        {
            mediaHandler = new AudioImageHandler();
            questionHandler = new QuestionHandler();
        }

        public void Run()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            ShowLoading();
            mediaHandler.DisplayLogo();
            mediaHandler.PlayWelcomeAudio();
            WelcomeUser();
            Menu();
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
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n===================================================================================================");
            Console.WriteLine("Chat AI-> Well hello there: " + name);
            Console.WriteLine("===================================================================================================");
        }

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

                //used a case here instead of using a do while loop to have an eaaier user interaction 
                switch (answer)
                {
                    case "yes":
                        questionHandler.HandleQuestions(name);
                        break;
                    case "no":
                        Console.WriteLine("\n===================================================================================================");
                        Console.WriteLine("That's a shame. Feel free to come back if you have questions.");
                        Console.WriteLine("===================================================================================================");
                        return;
                    default:
                        Console.WriteLine("\n===================================================================================================");
                        Console.WriteLine("Invalid input! Please enter 'yes' or 'no'.");
                        Console.WriteLine("===================================================================================================");
                        break;
                }
            }
        }
    }
}
