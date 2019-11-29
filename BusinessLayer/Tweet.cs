using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BusinessLayer
{
    /// <summary>
    /// Tweet class holds all the tweet properties and validation.
    /// It is also used to validate the user inputs to assure input data is correct.
    /// It also inherits Message class.
    /// </summary>
    public class Tweet: Message
    {
        private string sender;          // private variable to store tweet sender
        private string messageText;     // private variable to store tweet's messageText


        //************************* 
        // PROPERTIES 
        //*************************//  

        // this property is used to get and set  the value of sender in other classes, it returns a string value
        public string pSender
        {
            get { return sender; }
            set
            {
                string patternSender = @"^[\@][\w]{1,15}$";             //regex pattern to validate sender
                if (!Regex.IsMatch(value, patternSender))               // pattern does not match then throw exception with message
                {
                    throw new Exception("Invalid sender's twitter handle.\nMessage must have sender's Twitter ID between 1-15 characters predcedded by @.\nOnly letter, digits or _ is allowed");
                }
                Match result = Regex.Match(value, patternSender);       // otherwise extract sender
                sender = result.Value;                                  // assign value to property
            }
        }



        // this property is used to get and set  the value of tweet' message text in other classes, it returns a string value
        public string pMessageText
        {
            get { return messageText; }
            set
            {
                string patternText = @"^[\+\D\w\s]{1,140}$";        //regex pattern to validate message text
                if (!Regex.IsMatch(value, patternText))             // pattern does not match then throw exception with message
                {
                    throw new Exception("Invalid tweet text.\nMessage must have tweet text between 1-140 characters.");
                }
                Match result = Regex.Match(value, patternText);     // otherwise extract messageText
                messageText = result.Value;                         // assign value to property
            }
        }
    }
}
