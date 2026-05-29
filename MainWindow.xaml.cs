using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace ChatBotPart2
{
    public partial class MainWindow : Window
    {
        // LISTS
        List<string> reply = new List<string>();
        List<string> ignore = new List<string>();

        // OBJECTS
        user_name check_name = new user_name();

        // VARIABLES
        string username = string.Empty;
        string currentStatus = "Online";
        string last_topic = string.Empty;

        DateTime lastMessageDate;

        int counting = 0;

        // CONSTRUCTOR
        public MainWindow()
        {
            InitializeComponent();

            // LOAD RESPONSES
            new respond(reply, ignore);

            // PLAY GREETING SOUND
            voice_greeting greet =
                new voice_greeting();

            greet.greet();
        }

        // =====================================
        // PROCEED BUTTON
        // =====================================

        private void proceed(
            object sender,
            RoutedEventArgs e)
        {
            home_grid.Visibility =
                Visibility.Hidden;

            username_grid.Visibility =
                Visibility.Visible;
        }

        // =====================================
        // SUBMIT USERNAME
        // =====================================

        private void submit_name(
            object sender,
            RoutedEventArgs e)
        {
            username =
                check_name.submit_name(
                    usernames_input,
                    username_error,
                    chats
                );

            // STOP IF INVALID
            if (string.IsNullOrWhiteSpace(username))
            {
                return;
            }

            // OPEN CHAT PAGE
            username_grid.Visibility =
                Visibility.Hidden;

            chat_grid.Visibility =
                Visibility.Visible;
        }

        // =====================================
        // SEND MESSAGE
        // =====================================

        private async void send(
            object sender,
            RoutedEventArgs e)
        {
            string rawQuestion =
                question.Text.Trim();

            // EMPTY VALIDATION
            if (string.IsNullOrWhiteSpace(rawQuestion))
            {
                error_method(
                    "ChatBot",
                    "Please enter a question."
                );

                return;
            }

            // SAVE DATE
            lastMessageDate =
                DateTime.Now;

            // UPDATE STATUS
            currentStatus = "Active";

            // CLEAN INPUT
            string questions =
                RemoveSpecialCharacters(
                    rawQuestion
                );

            // SHOW USER MESSAGE
            error_method(
                username +
                " (" + currentStatus + ")",
                rawQuestion
            );

            // SMALL DELAY
            await Task.Delay(500);

            // MEMORY RECALL
            auto_show_interest();

            // BOT RESPONSE
            ai_check(questions);

            // UPDATE STATUS BAR
            status_text.Text =
                "Last Message: " +
                lastMessageDate.ToString(
                    "dd MMM yyyy HH:mm:ss"
                );
        }

        // =====================================
        // CLEAR CHAT
        // =====================================

        private void clear_chat(
            object sender,
            RoutedEventArgs e)
        {
            // CLEAR ALL MESSAGES
            chats.Items.Clear();

            // UPDATE STATUS
            status_text.Text =
                "Chat cleared successfully.";

            // BOT MESSAGE
            error_method(
                "ChatBot",
                "Chat history has been cleared."
            );
        }

        // =====================================
        // CHATBOT LOGIC
        // =====================================

        private void ai_check(
            string questions)
        {
            if (string.IsNullOrWhiteSpace(questions))
            {
                error_method(
                    "ChatBot",
                    "Please enter a valid question."
                );

                return;
            }

            questions =
                questions.ToLower();

            // FOLLOW-UP QUESTIONS
            if (questions.Contains("tell me more") ||
                questions.Contains("more") ||
                questions.Contains("explain"))
            {
                if (!string.IsNullOrWhiteSpace(last_topic))
                {
                    ai_check(last_topic);
                    return;
                }
            }

            string[] words =
                questions.Split(
                    new char[]
                    {
                        ' ',
                        ',',
                        '.',
                        '?',
                        '!',
                        ';',
                        ':'
                    },

                    StringSplitOptions
                    .RemoveEmptyEntries
                );

            bool found = false;

            Random random =
                new Random();

            List<string> answers_found =
                new List<string>();

            foreach (string word in words)
            {
                // IGNORE SMALL WORDS
                if (word.Length < 3 ||
                    ignore.Contains(word))
                {
                    continue;
                }

                List<string> per_word =
                    new List<string>();

                // SEARCH RESPONSES
                foreach (string answer in reply)
                {
                    if (answer
                        .ToLower()
                        .Contains(word))
                    {
                        found = true;

                        last_topic = word;

                        per_word.Add(answer);
                    }
                }

                // RANDOM ANSWER
                if (per_word.Count > 0)
                {
                    int index =
                        random.Next(
                            per_word.Count
                        );

                    answers_found.Add(
                        per_word[index]
                    );
                }
            }

            // REMOVE DUPLICATES
            answers_found =
                answers_found
                .Distinct()
                .ToList();

            // SHOW ANSWERS
            if (found &&
                answers_found.Count > 0)
            {
                string finalMessage =
                    string.Empty;

                foreach (string ans
                    in answers_found)
                {
                    finalMessage +=
                        ans + "\n";
                }

                error_method(
                    "ChatBot",
                    finalMessage
                );
            }
            else
            {
                string[] fallback =
                {
                    "I didn't understand that.",
                    "Please ask cybersecurity questions.",
                    "Can you rephrase your question?",
                    "I'm still learning cybersecurity topics."
                };

                error_method(
                    "ChatBot",
                    fallback[random.Next(
                        fallback.Length
                    )]
                );
            }

            // CLEAR INPUT
            question.Clear();
        }

        // =====================================
        // REMOVE SPECIAL CHARACTERS
        // =====================================

        private string RemoveSpecialCharacters(
            string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return "";
            }

            StringBuilder clean =
                new StringBuilder();

            foreach (char c in input)
            {
                if (char.IsLetterOrDigit(c) ||
                    char.IsWhiteSpace(c))
                {
                    clean.Append(c);
                }
                else
                {
                    clean.Append(' ');
                }
            }

            return Regex.Replace(
                clean.ToString(),
                @"\s+",
                " "
            ).Trim();
        }

        // =====================================
        // MEMORY RECALL
        // =====================================

        private void auto_show_interest()
        {
            if (counting == 3)
            {
                string filename =
                    "interested_topic.txt";

                if (File.Exists(filename))
                {
                    string[] lines =
                        File.ReadAllLines(
                            filename
                        );

                    foreach (string line
                        in lines)
                    {
                        if (line.StartsWith(username))
                        {
                            int index =
                                line.IndexOf(
                                    "interested in:"
                                );

                            if (index >= 0)
                            {
                                string interests =
                                    line.Substring(
                                        index + 14
                                    );

                                error_method(
                                    "ChatBot",
                                    "Reminder: you are interested in "
                                    + interests
                                );

                                ai_check(interests);

                                break;
                            }
                        }
                    }
                }

                counting = 0;
            }
            else
            {
                counting++;
            }
        }

        // =====================================
        // DISPLAY CHAT
        // =====================================

        private void error_method(
            string name,
            string message)
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

            // BOT STYLE
            if (name.ToLower().Contains("chat"))
            {
                messageBorder.Background =
                    new SolidColorBrush(
                        Color.FromRgb(
                            15,
                            25,
                            35
                        )
                    );

                messageBorder.BorderBrush =
                    Brushes.Cyan;
            }
            else
            {
                // USER STYLE
                messageBorder.Background =
                    new SolidColorBrush(
                        Color.FromRgb(
                            45,
                            45,
                            45
                        )
                    );

                messageBorder.BorderBrush =
                    Brushes.Gray;
            }

            TextBlock text =
                new TextBlock
                {
                    TextWrapping =
                        TextWrapping.Wrap,

                    Foreground =
                        Brushes.White,

                    FontSize = 14
                };

            // NAME
            text.Inlines.Add(
                new Run
                {
                    Text = name + ": ",

                    FontWeight =
                        FontWeights.Bold,

                    Foreground =
                        Brushes.Cyan
                }
            );

            // MESSAGE
            text.Inlines.Add(
                new Run
                {
                    Text = message
                }
            );

            messageBorder.Child = text;

            chats.Items.Add(messageBorder);

            chats.ScrollIntoView(
                chats.Items[
                    chats.Items.Count - 1
                ]
            );
        }
    }
}