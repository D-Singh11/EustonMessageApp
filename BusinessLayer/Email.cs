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
        private string messageText;
        private string centreCode = "n/a";
        private string incidentNature = "n/a";

        public string Sender
        {
            get { return sender; }
            set
            {
                //a = @"sender+\:"
                string patternSender = @"\b[\w._%+-]+@[\w.]+\.[A-Za-z]{2,}\b";
                if (!Regex.IsMatch(value, patternSender))
                {
                    throw new Exception("Invalid sender input.\nSender's email must be in correct format.");
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
                //a = @"sender+\:"
                string pattern = @"\s{0,}subject+\:";
                if (!Regex.IsMatch(value, pattern))
                {
                    throw new Exception("Invalid subject label.\nSender label be on first line in \"subject:\" format");
                }
                Match rewwsult = Regex.Match(value, pattern);
                value = value.Replace("subject:", "");

                string patternSubject = @"\b[\+\D\w\s]{4,20}\b";
                if (!Regex.IsMatch(value, patternSubject))
                {
                    throw new Exception("Invalid subject input.\nEmail subject must be on 2nd line and between 1-20 characters");
                }
                Match result = Regex.Match(value, patternSubject);
                subject = result.Value;
            }
        }

        public string MessageText
        {
            get { return messageText; }
            set
            {
                string pattern = @"\s{0,}Text+\:";
                if (!Regex.IsMatch(value, pattern))
                {
                    throw new Exception("Invalid Message Text label.\nIt must be in \"Text:\" format");
                }
                value = value.Replace("Text:", "");

                string patternText = @"[\+\D\w\s]{1,1028}";
                if (!Regex.IsMatch(value, patternText))
                {
                    throw new Exception("Invalid message text input.\n Email text must be between 1-1028 characters.");
                }
                Match result = Regex.Match(value, patternText);
                messageText = result.Value;
            }
        }

        public string CentreCode
        {
            get { return centreCode; }
            set
            {
                //a = @"sender+\:"
                string pattern = @"\s{0,}sport\s{1}centre\s{1}code+\:";
                if (!Regex.IsMatch(value, pattern))
                {
                    throw new Exception("Invalid sport centre code label.\nIt must be on like in \"sport centre code:\" format");
                }
                value = value.Replace("sport centre code:", "").Trim();
                
                string patternText = @"^[\d]{2}\-[\d]{3}\-[\d]{2}";
                if (!Regex.IsMatch(value, patternText))
                {
                    throw new Exception("Invalid sport centre code input.\n SIR email must have a centre code in 00-000-00 format.");
                }
                Match result = Regex.Match(value, patternText);
                centreCode = result.Value;
            }
        }

        public string IncidentNature
        {
            get { return incidentNature; }
            set
            {
                string pattern = @"\s{0,}nature\s{1}of\s{1}incident+\:";
                if (!Regex.IsMatch(value, pattern))
                {
                    throw new Exception("Invalid centre code label.\nIt must be on like in \"nature of incident:\" format");
                }

                value = value.Replace("nature of incident:","");

                string patternText = @"[\+\D\w\s]{4,20}";
                if (!Regex.IsMatch(value, patternText))
                {
                    throw new Exception("Invalid nature of incident input.\n SIR email must specify a nature of incident in message text.");
                }
                Match result = Regex.Match(value, patternText);
                incidentNature = result.Value.Trim();
            }
        }
    }
}
