using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BusinessLayer
{
    public class Tweet: Message
    {
        private string sender;
        private string message;

        string patternSender = @"^[\@][\w]{1,15}\s";
        string patternText = @"\s{1}[\+\D\w\s]{1,140}";
        public string Sender
        {
            get { return sender; }
            set
            {
                if (!Regex.IsMatch(value, patternSender))
                {
                    throw new Exception("Message must have sender's Twitter ID between 1-15 characters predcedded by @");
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
                    throw new Exception("Message must have tweet text between 1-140 characters");
                }
                Match result = Regex.Match(value, patternText);
                message = result.Value;
            }
        }
    }
}
