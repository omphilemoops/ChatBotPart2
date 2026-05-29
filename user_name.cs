using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace ChatBotPart2
{
    public class user_name
    {
        public string submit_name(
            TextBox user_name,
            TextBlock username_error,
            ListView chats)
        {
            string filename =
                "user_names.txt";

            if (!File.Exists(filename))
            {
                File.Create(filename).Close();
            }

            string name =
                user_name.Text.Trim();

            // CLEAR ERROR
            username_error.Text = "";

            // EMPTY
            if (string.IsNullOrWhiteSpace(name))
            {
                username_error.Text =
                    "Please enter a username.";

                return "";
            }

            // LETTERS ONLY
            if (!Regex.IsMatch(
                name,
                @"^[a-zA-Z]+$"))
            {
                username_error.Text =
                    "Username must contain letters only.";

                return "";
            }

            bool found = check_name(name);

            if (!found)
            {
                File.AppendAllText(
                    filename,
                    name +
                    Environment.NewLine
                );

                error_method(
                    "ChatBot",
                    "Welcome " + name + "!",
                    chats
                );
            }
            else
            {
                error_method(
                    "ChatBot",
                    "Welcome back " + name + "!",
                    chats
                );
            }

            return name;
        }

        // CHECK USER
        private bool check_name(string name)
        {
            string filename =
                "user_names.txt";

            if (!File.Exists(filename))
            {
                return false;
            }

            string[] names =
                File.ReadAllLines(filename);

            foreach (string stored_name in names)
            {
                if (stored_name
                    .Trim()
                    .ToLower() ==
                    name.ToLower())
                {
                    return true;
                }
            }

            return false;
        }

        // CHAT DISPLAY
        private void error_method(
            string name,
            string message,
            ListView chats)
        {
            Border messageBorder =
                new Border
                {
                    Margin =
                        new Thickness(5),

                    Padding =
                        new Thickness(10),

                    CornerRadius =
                        new CornerRadius(10),

                    BorderThickness =
                        new Thickness(1)
                };

            messageBorder.Background =
                new SolidColorBrush(
                    Color.FromRgb(
                        15,
                        25,
                        35
                    )
                );

            messageBorder.BorderBrush =
                new SolidColorBrush(
                    Color.FromRgb(
                        0,
                        255,
                        255
                    )
                );

            TextBlock text =
                new TextBlock
                {
                    TextWrapping =
                        TextWrapping.Wrap,

                    FontSize = 14,

                    Foreground =
                        Brushes.White
                };

            text.Inlines.Add(new Run
            {
                Text = name + ": ",

                FontWeight =
                    FontWeights.Bold,

                Foreground =
                    Brushes.Cyan
            });

            text.Inlines.Add(new Run
            {
                Text = message
            });

            messageBorder.Child = text;

            chats.Items.Add(messageBorder);
        }
    }
}