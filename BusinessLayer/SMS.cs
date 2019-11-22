using System;
using System.Text.RegularExpressions;

namespace BusinessLayer
{
    public class SMS : Message
    {
        private string sender;
        private string message;
        private string pattern = @"^(?<phoneNumber>[\+][\d]{9,15})\s+(?<text>[\w]{1,40})?$";
        string patternNumber = @"^[\+][\d]{9,15}\s";
        string patternMessage = @"\s{1}[\+\D\w\s]{1,140}";

        public string Sender
        {

            get { return sender; }
            set
            {
                if (!Regex.IsMatch(value, patternNumber))
                {
                    throw new Exception("Invalid sender's phone number.\nPhone number must start with\"+\" and should be between 9-15 digits.");
                }
                Match result = Regex.Match(value, patternNumber);
                sender = result.Value;
            }
        }

        public string Message
        {
            get { return message; }
            set
            {
                if (!Regex.IsMatch(value, patternMessage))
                {
                    throw new Exception("Invalid SMS text.\nMessage Text must be between 1-140 characters only.");
                }
                Match result = Regex.Match(value, patternMessage);
                message = result.Value;
            }
        }
    }
}
