using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotPart2
{
    // Class to manage the quiz questions and scoring
    public class QuizManager
    {
        public List<QuizQuestion> Questions =
            new List<QuizQuestion>();

        public int CurrentQuestion = 0;

        public int Score = 0;

        public QuizManager()
        {
            LoadQuestions();
        }

        private void LoadQuestions()
        {
            Questions.Add(new QuizQuestion
            {
                Question = "What does VPN stand for?",
                A = "Virtual Private Network",
                B = "Verified Private Network",
                C = "Virtual Public Network",
                D = "Visual Private Network",
                Correct = "A",
                Explanation = "VPN stands for Virtual Private Network."
            });

            Questions.Add(new QuizQuestion
            {
                Question = "Which attack uses fake emails?",
                A = "Malware",
                B = "Firewall",
                C = "Phishing",
                D = "Encryption",
                Correct = "C",
                Explanation = "Phishing attacks often use fake emails."
            });

            Questions.Add(new QuizQuestion
            {
                Question = "What is 2FA?",
                A = "Two Factor Authentication",
                B = "Two File Access",
                C = "Two Firewall Access",
                D = "Two Fast Applications",
                Correct = "A",
                Explanation = "2FA adds an additional layer of security."
            });

            Questions.Add(new QuizQuestion
            {
                Question = "What protects a network?",
                A = "Wallpaper",
                B = "Firewall",
                C = "Browser",
                D = "Keyboard",
                Correct = "B",
                Explanation = "Firewalls help protect networks."
            });

            Questions.Add(new QuizQuestion
            {
                Question = "Malware is:",
                A = "Safe software",
                B = "Security software",
                C = "Malicious software",
                D = "VPN software",
                Correct = "C",
                Explanation = "Malware means malicious software."
            });

            Questions.Add(new QuizQuestion
            {
                Question = "Strong passwords should:",
                A = "Use birthdays",
                B = "Be short",
                C = "Contain symbols and numbers",
                D = "Be reused",
                Correct = "C",
                Explanation = "Strong passwords use symbols, numbers and complexity."
            });

            Questions.Add(new QuizQuestion
            {
                Question = "Ransomware:",
                A = "Encrypts files",
                B = "Improves speed",
                C = "Blocks ads",
                D = "Creates backups",
                Correct = "A",
                Explanation = "Ransomware often encrypts files for ransom."
            });

            Questions.Add(new QuizQuestion
            {
                Question = "Encryption is used to:",
                A = "Delete data",
                B = "Protect data",
                C = "Damage data",
                D = "Move data",
                Correct = "B",
                Explanation = "Encryption protects information."
            });

            Questions.Add(new QuizQuestion
            {
                Question = "Public Wi-Fi is:",
                A = "Always secure",
                B = "Always encrypted",
                C = "Potentially risky",
                D = "Private",
                Correct = "C",
                Explanation = "Public Wi-Fi can expose your data."
            });

            Questions.Add(new QuizQuestion
            {
                Question = "Privacy means:",
                A = "Protecting personal information",
                B = "Sharing everything",
                C = "Disabling passwords",
                D = "Ignoring security",
                Correct = "A",
                Explanation = "Privacy protects personal information."
            });
        }
    }
}