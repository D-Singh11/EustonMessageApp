using System;

namespace BusinessLayer
{
    public class Message
    {
        private string messageID;
        private string mMessageType;


        public string MessageId
        {
            get { return messageID; }
            set
            {
                string header = value;
                if (header.Length == 10)
                {
                    validate_MessageType(header);
                }
                else
                {
                    throw new ArgumentException("Incorrect Header input.\n Enter 10 charcters meassage code.");
                }
                messageID = value;

            }
        }

        public string MessageType
        {
            get { return mMessageType; }
        }


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
            mMessageType = message.Substring(0, 1).ToLower();
            //tb_id.Text = message;
        }
    }
}
