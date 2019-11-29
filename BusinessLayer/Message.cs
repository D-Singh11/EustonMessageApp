using System;

namespace BusinessLayer
{

    /// <summary>
    /// Message class is base class which is inherited by SMS, Tweet and email classes.
    /// it has one private messageId property
    /// It is also used to validate the user input for messageId to assure input data is correct.
    /// </summary>
    public class Message
    {
        private string messageID;       // private variable to store messageId


        //************************* 
        // PROPERTIES 
        //*************************//  

        // this property is used to get and set  the value of messageId, it returns a string value
        public string pMessageId
        {
            get { return messageID; }
            set
            {
                string header = value;
                if (header.Length == 10)            //checks if length of input is lon characters long
                {
                    validate_MessageType(header);  
                }
                else                                // if input text length not 10 characters long throw exception
                {
                    throw new ArgumentException("Incorrect Header input.\n Enter 10 charcters meassage code.");
                }
                messageID = value;              // otherwise assign value to property

            }
        }

         //************************* 
         // Methods
         // ***************************

        // private message used to validate the message type using first character of input
        private void validate_MessageType(string message)
        {
            string messageType = message.Substring(0, 1).ToLower();
            //int code = (int)messageType;
            if (messageType != "s" && messageType != "m" && messageType != "t")
            {
                throw new ArgumentException("Incorrect Header input.\nMessage code must begin with M,S or T only.");
            }
            else
            {
                validate_message_code(message);
            }
        }


        // private message validates if the user input after first letter is all digits or not
        private void validate_message_code(string message)
        {
            Boolean valid;
            string idNumber = message.Substring(1, message.Length - 1);
            foreach (var item in idNumber)
            {
                int code = (int)item;
                valid = code < 48 || code > 57 ? false : true;
                if (valid == false)
                {
                    throw new ArgumentException("Incorrect Header input.\nOnly first character of message code can be alphabet.");
                }
            }
        }
    }
}
