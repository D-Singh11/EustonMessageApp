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

        string patternSender = @"\b[\w._%+-]+@[\w.]+\.[A-Za-z]{2,}\b";
        string patternText = @"\s{1}[\+\D\w\s]{1,1028}";

        public string Sender
        {
            get { return sender; }
            set
            {
                if (!Regex.IsMatch(value, patternSender))
                {
                    throw new Exception("Message must have sender's email in correct format.");
                }
                Match result = Regex.Match(value, patternSender);
                sender = result.Value;
            }
        }

        public string Message
        {
            get { return message; }
            set
            {
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
