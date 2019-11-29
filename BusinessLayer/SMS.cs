using System;
using System.Text.RegularExpressions;

namespace BusinessLayer
{
    /// <summary>
    /// SMS class holds all the SMS properties and validation.
    /// It is also used to validate the user inputs to assure input data is correct.
    /// It also inherits Message class.
    /// </summary>
    public class SMS : Message
    {
        private string sender;          // private variable to store SMS sender
        private string messageText;     // private variable to store SMS's messageText


        //************************* 
        // PROPERTIES 
        //*************************//  

        // this property is used to get and set  the value of sender in other classes, it returns a string value
        public string pSender
        {
            get { return sender; }
            set
            {
                string patternNumber = @"^[\+][\d]{9,15}$";     //regex pattern to validate sender
                if (!Regex.IsMatch(value, patternNumber))       // if pattern does not match then throw exception with message
                {
                    throw new Exception("Invalid sender's phone number.\nPhone number must start with\"+\" and should be between 9-15 digits.");
                }
                Match result = Regex.Match(value, patternNumber);       // otherwise extract sender
                sender = result.Value;                                  // assign value to property
            }
        }


        // this property is used to get and set  the value of tweet' message text in other classes, it returns a string value
        public string pMessageText
        {
            get { return messageText; }
            set
            {
                string patternMessage = @"^[\+\D\w\s]{1,140}$";     //regex pattern to validate messageText
                if (!Regex.IsMatch(value, patternMessage))          // pattern does not match then throw exception with message
                {
                    throw new Exception("Invalid SMS text.\nMessage Text must be between 1-140 characters only.");
                }
                Match result = Regex.Match(value, patternMessage);      // otherwise extract messageText
                messageText = result.Value;                         // assign value to property
            }
        }
    }
}
