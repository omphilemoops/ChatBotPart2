
using System.Collections.Generic;

namespace ChatBotPart2
{
    public class respond
    {
        public respond(List<string> reply, List<string> ignore)
        {
            answers(reply);
            words(ignore);
        }

        private void words(List<string> ignoring)
        {
            ignoring.Add("the");
            ignoring.Add("and");
            ignoring.Add("is");
            ignoring.Add("am");
            ignoring.Add("i");
            ignoring.Add("you");
            ignoring.Add("are");
            ignoring.Add("a");
            ignoring.Add("an");
            ignoring.Add("to");
            ignoring.Add("of");
            ignoring.Add("in");
            ignoring.Add("on");
            ignoring.Add("for");
            ignoring.Add("with");
            ignoring.Add("that");
            ignoring.Add("this");
            ignoring.Add("it");


        }

        public void answers(List<string> add_answers)
        {
            // PASSWORD
            add_answers.Add("password Use strong passwords with symbols and numbers.");
            add_answers.Add("password Avoid using your birth date as a password.");
            add_answers.Add("password Change your passwords regularly.");
            add_answers.Add("password a password is used to secure access to your accounts or devices.");
            add_answers.Add("password it should be strong, long and not easy to guess.");
            add_answers.Add("password avoid using personal details when creating one.");

            // PHISHING
            add_answers.Add("phishing Never click suspicious email links.");
            add_answers.Add("phishing Scammers pretend to be trusted companies.");
            add_answers.Add("phishing Verify email addresses carefully.");
            add_answers.Add("phishing phishing is a scam where attackers pretend to be trusted sources to steal information.");
            add_answers.Add("phishing it uses fake messages or websites to trick users into revealing sensitive data.");
            add_answers.Add("phishing attackers use deception to make users believe they are legitimate.");

            // FIREWALL
            add_answers.Add("firewall A firewall blocks harmful network traffic.");
            add_answers.Add("firewall Firewalls protect your computer from attackers.");
            add_answers.Add("firewall a firewall controls network traffic based on security rules.");
            add_answers.Add("firewall it helps block unwanted access to your device or network.");
            add_answers.Add("firewall it acts as a protective barrier between trusted and untrusted networks.");

            // VPN
            add_answers.Add("vpn VPNs protect your privacy online.");
            add_answers.Add("vpn VPNs encrypt your internet traffic.");

            // MALWARE
            add_answers.Add("malware Malware can damage your computer.");
            add_answers.Add("malware Install antivirus software for protection.");

            // RANSOMWARE
            add_answers.Add("ransomware Ransomware locks your files until payment.");
            add_answers.Add("ransomware Always backup important files.");

            // PRIVACY
            add_answers.Add("privacy Review your account privacy settings regularly.");
            add_answers.Add("privacy Never overshare personal information online.");

            // 2FA
            add_answers.Add("2fa Two-factor authentication improves security.");
            add_answers.Add("2fa Use authentication apps instead of SMS.");

            // SCAM
            add_answers.Add("scam Ignore messages asking for banking details.");
            add_answers.Add("scam Scammers create urgency to trick victims.");

            // ENCRYPTION
            add_answers.Add("encryption Encryption protects sensitive data.");
            add_answers.Add("encryption Encrypted apps improve privacy.");

            // SENTIMENTS
            add_answers.Add("worried It's okay to feel worried. Let me help you stay safe online.");
            add_answers.Add("frustrated I understand your frustration. Let's solve it together.");
            add_answers.Add("happy That's great to hear!");
            add_answers.Add("sad I'm here for you.");
            add_answers.Add("angry Take a moment and let's solve the issue calmly.");
            add_answers.Add("confused Let me explain it more clearly.");
        }
    }
}