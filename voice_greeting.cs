
using System;
using System.IO;
using System.Media;
using System.Windows;

namespace ChatBotPart2
{
    public class voice_greeting
    {
        // METHOD TO PLAY GREETING SOUND
        public void greet()
        {
            try
            {
         //star of greet method

            

            string auto_path = AppDomain.CurrentDomain.BaseDirectory.Replace(@"\bin\Debug\", @"\greeet.wav");

           

                // CHECK IF FILE EXISTS
                if (File.Exists(auto_path))

                {
                    //create an instance for the soundPlayer class
                    SoundPlayer greetMe = new SoundPlayer(auto_path);

                    // LOAD SOUND
                    greetMe.Load();

                    // PLAY SOUND
                    greetMe.Play();
                }
                else
                {
                    MessageBox.Show(
                        "Greeting sound file was not found.",
                        "Voice Greeting Error"
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error playing greeting sound:\n" + ex.Message,
                    "Voice Greeting Error"
                );
            }
        }
    }
}