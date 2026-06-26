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
        // PART 3 OBJECTS
        DatabaseManager database =
            new DatabaseManager();

        ActivityLogger logger =
            new ActivityLogger();

        QuizManager quiz =
            new QuizManager();

        private QuizQuestion currentQuestion;
        private bool waitingForReminder = false;

        private CyberTask pendingTask = null;

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
            RefreshTaskGrid();

            LoadQuestion();

            logger.Add("Application Started");
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
                    "Please enter a valid question.");
// ADD TASK USING NATURAL LANGUAGE
if (questions.StartsWith("add task"))
                {
                    string title = questions.Replace("add task", "")
                                            .Replace("-", "")
                                            .Trim();

                    if (string.IsNullOrWhiteSpace(title))
                    {
                        error_method("ChatBot",
                            "Please tell me the task name.");

                        return;
                    }

                    pendingTask = new CyberTask()
                    {
                        Title = title,
                        Description = "Review account privacy settings to ensure your data is protected.",
                        ReminderDate = DateTime.Now,
                        Completed = false
                    };

                    waitingForReminder = true;

                    error_method("ChatBot",
                        "Task added with the description \"" +
                        pendingTask.Description +
                        "\"\n\nWould you like a reminder?");
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
                if (questions.Contains(
        "add task"))
                {
                    error_method(
                        "ChatBot",
                        "Please use the Task Assistant tab."
                    );

                    logger.Add(
                        "NLP Task Request"
                    );

                    return;
                }

                if (questions.Contains(
                    "show tasks"))
                {
                    error_method(
                        "ChatBot",
                        "Open the Task Assistant tab to view tasks."
                    );

                    logger.Add(
                        "NLP View Tasks"
                    );

                    return;
                }

                if (questions.Contains(
                    "quiz"))
                {
                    error_method(
                        "ChatBot",
                        "Open the Quiz tab."
                    );

                    logger.Add(
                        "NLP Quiz Request"
                    );

                    return;
                }

                if (questions.Contains(
                    "activity"))
                {
                    error_method(
                        "ChatBot",
                        "Open the Activity Log tab."
                    );

                    logger.Add(
                        "NLP Activity Log Request"
                    );

                    return;
                }
                if (waitingForReminder)
                {
                    waitingForReminder = false;

                    if (questions.Contains("yes"))
                    {
                        int days = 1;

                        Match match = Regex.Match(questions, @"\d+");

                        if (match.Success)
                            days = Convert.ToInt32(match.Value);

                        pendingTask.ReminderDate =
                            DateTime.Now.AddDays(days);

                        database.AddTask(pendingTask);

                        error_method("ChatBot",
                            "Got it! I'll remind you in " +
                            days +
                            " day(s).");
                    }
                    else
                    {
                        database.AddTask(pendingTask);

                        error_method("ChatBot",
                            "Task saved without a reminder.");
                    }

                    RefreshTaskGrid();

                    pendingTask = null;

                    return;
                } }
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
        private void RefreshTaskGrid()
        {
            try
            {
                taskGrid.ItemsSource = null;
                taskGrid.ItemsSource =
                    database.GetTasks();
            }
            catch
            {
            }
        }
        private void RefreshTasks_Click(
    object sender,
    RoutedEventArgs e)
        {
            RefreshTaskGrid();

            logger.Add(
                "Task List Viewed"
            );
        }
        private void CompleteTask_Click(
           object sender,
           RoutedEventArgs e)
        {
            if (taskGrid.SelectedItem == null)
            {
                MessageBox.Show(
                    "Please select a task first."
                );

                return;
            }

            CyberTask task =
                (CyberTask)taskGrid.SelectedItem;

            database.CompleteTask(task.Id);

            logger.Add(
                "Task Completed: " +
                task.Title
            );

            RefreshTaskGrid();

            MessageBox.Show(
                "Task marked as completed."
            );
        }
        private void DeleteTask_Click(
       object sender,
       RoutedEventArgs e)
        {
            if (taskGrid.SelectedItem == null)
            {
                MessageBox.Show(
                    "Please select a task first."
                );

                return;
            }

            CyberTask task =
                (CyberTask)taskGrid.SelectedItem;

            MessageBoxResult result =
                MessageBox.Show(
                    "Delete '" + task.Title + "' ?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

            if (result == MessageBoxResult.Yes)
            {
                database.DeleteTask(task.Id);

                logger.Add(
                    "Task Deleted: " +
                    task.Title
                );

                RefreshTaskGrid();

                MessageBox.Show(
                    "Task deleted successfully."
                );
            }
        }
        private void LoadQuestion()
        {
            if (quiz.CurrentQuestion
                >= quiz.Questions.Count)
            {
                quizQuestion.Text =
                    "Quiz Complete";

                btnA.Visibility =
                    Visibility.Hidden;

                btnB.Visibility =
                    Visibility.Hidden;

                btnC.Visibility =
                    Visibility.Hidden;

                btnD.Visibility =
                    Visibility.Hidden;

                MessageBox.Show(
                    "Final Score: "
                    + quiz.Score
                    + "/"
                    + quiz.Questions.Count
                );

                logger.Add(
                    "Quiz Completed"
                );

                return;
            }

            currentQuestion =
                quiz.Questions[
                    quiz.CurrentQuestion
                ];

            quizQuestion.Text =
                currentQuestion.Question;

            btnA.Content =
                "A. "
                + currentQuestion.A;

            btnB.Content =
                "B. "
                + currentQuestion.B;

            btnC.Content =
                "C. "
                + currentQuestion.C;

            btnD.Content =
                "D. "
                + currentQuestion.D;

            quizScore.Text =
                "Score: "
                + quiz.Score;
        }
        private void CheckAnswer(string answer)
        {
            if (answer ==
                currentQuestion.Correct)
            {
                quiz.Score++;

                MessageBox.Show(
                    "Correct!\n\n"
                    + currentQuestion.Explanation
                );
            }
            else
            {
                MessageBox.Show(
                    "Incorrect.\n\n"
                    + currentQuestion.Explanation
                );
            }

            quiz.CurrentQuestion++;

            logger.Add(
                "Quiz Question Answered"
            );

            LoadQuestion();
        }
        private void AnswerA_Click( object sender,RoutedEventArgs e)
        {
            CheckAnswer("A");
        }
        private void AnswerB_Click( object sender,RoutedEventArgs e)
        {
            CheckAnswer("B");
        }
        private void AnswerC_Click(
    object sender,
    RoutedEventArgs e)
        {
            CheckAnswer("C");
        }
        private void AnswerD_Click(
    object sender,
    RoutedEventArgs e)
        {
            CheckAnswer("D");
        }
        private void RefreshLog_Click(
    object sender,
    RoutedEventArgs e)
        {
            activityList.ItemsSource =
                null;

            activityList.ItemsSource =
                logger.GetRecent();
        }

        private void AddTask_Click(
    object sender,
    RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(
                    taskTitle.Text))
                {
                    MessageBox.Show(
                        "Enter task title."
                    );

                    return;
                }

                CyberTask task =
                    new CyberTask
                    {
                        Title =
                            taskTitle.Text,

                        Description =
                            taskDescription.Text,

                        ReminderDate =
                            taskDate.SelectedDate
                            ?? DateTime.Now,

                        Completed =
                            false
                    };

                database.AddTask(task);

                logger.Add(
                    "Task Added: "
                    + task.Title
                );

                RefreshTaskGrid();

                MessageBox.Show(
                    "Task added successfully."
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message
                );
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