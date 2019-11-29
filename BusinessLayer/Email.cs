using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BusinessLayer
{
    /// <summary>
    /// Email class holds all the email properties and validation.
    /// It is also used to validate the user inputs to assure input data is correct.
    /// It also inherits Message class.
    /// </summary>
    public class Email:Message
    {
        private string sender;                      // private variable to store email sender
        private string subject;                     // private variable to store email subject
        private string messageText;                 // private variable to store email emailText
        private string centreCode = "n/a";          // private variable to store email sport centre code, deafult value assigned n/a
        private string incidentNature = "n/a";      // private variable to store email nature of incident, deafult value assigned n/a


        //************************* 
        // PROPERTIES 
        //*************************//  

        // this property is used to get and set the value of sender, it returns a string value
        public string pSender
        {
            get { return sender; }
            set
            {
                string patternSender = @"^\b[\w._%+-]+@[\w.]+\.[A-Za-z]{2,}\b$";        //regex pattern to validate email sender  
                if (!Regex.IsMatch(value, patternSender))                               // pattern does not match then throw exception with message
                {
                    throw new Exception("Invalid sender input.\nSender's email must be in correct format.");
                }
                Match result = Regex.Match(value, patternSender);                       // otherwise extract email address of sender
                sender = result.Value;                                                  // assigns its value to property
            }
        }

        // this property is used to get and set the value of email subject, it returns a string value
        public string pSubject
        {
            get { return subject; }
            set
            {
                string sirPttern = @"^sir\s[\d]{2}\/[\d]{2}\/[\d]{4}$";                 //regex pattern to check if email has subject of SIR type in correct format
                if (value.StartsWith("SIR", StringComparison.OrdinalIgnoreCase) && !Regex.IsMatch(value, sirPttern, RegexOptions.IgnoreCase))
                {
                    throw new Exception("Invalid subject.\nSIR subject must be in \"SIR dd/mm/yyyy\" format");          // throw  error id SIR subject format is incorrect
                }
                if (value.Length > 20 || value.Length < 1)                                                          /// check if length of subject is bwetween permitted length
                {
                    throw new Exception("Invalid subject input.\nEmail subject must be on 2nd line and between 1-20 characters");
                }
                subject = value;                                                                    // assigns input value to property
            }
        }


        // this property is used to get and set the value of email text, it returns a string value
        public string pMessageText
        {
            get { return messageText; }
            set
            {
                string patternText = @"^[\+\D\w\s]{1,1028}$";            //regex pattern to check if email text is in correct format
                if (!Regex.IsMatch(value, patternText))                     // throw  error text is empty or text length 1028 characters
                {
                    throw new Exception("Invalid message text input.\n Email text must be between 1-1028 characters.");
                }
                Match result = Regex.Match(value, patternText);                 // extract text from input
                messageText = result.Value;                                     // assigns input to property
            }
        }


        // this property is used to get and set the value of Special incident report (SIR) email's sport centre code, it returns a string value
        public string pCentreCode
        {
            get { return centreCode; }
            set
            {
                string patternText = @"^[\d]{2}\-[\d]{3}\-[\d]{2}$";                //regex pattern to check if sport centre code is in correct format
                if (!Regex.IsMatch(value, patternText))                              // throw  error text if incorrect format
                {
                    throw new Exception("Invalid sport centre code input.\n SIR email must have a centre code in 00-000-00 format.");
                }
                Match result = Regex.Match(value, patternText);                 // extract centre code from input
                centreCode = result.Value;                                      // assigns input to property
            }
        }


        // this property is used to get and set  the value of Special incident report (SIR) email's nature of incident , it returns a string value
        public string pIncidentNature
        {
            get { return incidentNature; }
            set
            {
                string patternText = @"^[\+\D\w\s]{4,20}$";                 //regex pattern to check if nature of incident is in correct format
                if (!Regex.IsMatch(value, patternText))                     // throw  error text if incorrect format
                {
                    throw new Exception("Invalid nature of incident input.\n SIR email must specify a nature of incident in message text.");
                }
                Match result = Regex.Match(value, patternText);             // extract nature of incident from input
                incidentNature = result.Value.Trim();                       // assigns input to property
            }
        }
    }
}
