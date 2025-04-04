using System;
using System.IO;
using System.Media;
using System.Drawing;

namespace CyberSecurityChatBotAI
{
    //This class will handle the audio and image files for the CyberBot.
    public class AudioImageHandler
    {
        //This method plays the welcome audio of the CyberBot.
        public void PlayWelcomeAudio()
        {
            //Getting the path of the audio file.
            string fullLocation = AppDomain.CurrentDomain.BaseDirectory;
            string newPath = fullLocation.Replace("bin\\Debug\\", "");

            try
            {
                //Combining the path with the audio file name.
                string fullPath = Path.Combine(newPath, "BotGreeting.wav");
                using (SoundPlayer play = new SoundPlayer(fullPath))
                {
                    play.PlaySync();
                }
            }

            //Catching any exceptions that may occur.
            catch (Exception error)
            {
                Console.WriteLine("Error playing audio: " + error.Message);
            }

        }//End of PlayWelcomeAudio method.

        //This method displays the logo of the CyberBot as ASCII art.
        public void DisplayLogo()
        {
            //Getting the path of the image.
            string paths = AppDomain.CurrentDomain.BaseDirectory;

            //Removing the bin\Debug\ part of the path.
            string newPath = paths.Replace("bin\\Debug\\", "");

            //Combining the path with the image name.
            string fullPath = Path.Combine(newPath, "Ascii-image.jpg");

            try
            {
                Bitmap logo = new Bitmap(fullPath);
                logo = new Bitmap(logo, new Size(120, 170));

                for (int height = 0; height < logo.Height; height++)
                {
                    for (int width = 0; width < logo.Width; width++)
                    {
                        Color pixelColor = logo.GetPixel(width, height);
                        int gray = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
                        char asciiChar = gray > 250 ? '.' : gray > 150 ? '*' : gray > 100 ? 'o' : gray > 50 ? '#' : '@';
                        Console.Write(asciiChar);
                    }
                    Console.WriteLine();

                }//End of for loop.

            }//End of try block.

            //Catching any exceptions that may occur.
            catch (Exception error)
            {
                Console.WriteLine("Error displaying image: " + error.Message);
            }

        }//End of DisplayLogo method.

    }//End of AudioImageHandler class.

}// End of the namespace POE.
