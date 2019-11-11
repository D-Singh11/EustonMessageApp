using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BusinessLayer
{
    public class Email:Message
    {
        private string sender;
        private string subject;
        private string message;

        public string Sender
        {
            get { return sender; }
            set
            {
                string patternSender = @"\b[\w._%+-]+@[\w.]+\.[A-Za-z]{2,}\b";
                if (!Regex.IsMatch(value, patternSender))
                {
                    throw new Exception("Message must have sender's email in correct format.");
                }
                Match result = Regex.Match(value, patternSender);
                sender = result.Value;
            }
        }

        public string Subject
        {
            get { return subject; }
            set
            {
                //string patternSubject = @"\b\s{1}[\+\D\w\s]{1,20}\s\b";
                if (String.IsNullOrWhiteSpace(value) || value.Length > 20)
                {
                    throw new Exception("Message must a subject between 1-20 characters");
                }
                //Match result = Regex.Match(value, patternSubject);
                subject = value;
            }
        }

        public string Message
        {
            get { return message; }
            set
            {
                string patternText = @"[\+\D\w\s]{1,1028}";
                if (!Regex.IsMatch(value, patternText))
                {
                    throw new Exception("Email must have message text between 1-1028 characters");
                }
                Match result = Regex.Match(value, patternText);
                message = result.Value;
            }
        }
    }
}
