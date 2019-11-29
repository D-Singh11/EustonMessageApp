using DataLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BusinessLayer
{
    public class MessageOperationsFacade
    {
        /// <summary>
        /// This class allow all layers to communicate with each other.
        /// It uses facade design beacuse it hides the complexity of all three SMS, Tweet and Email classes from user.
        /// It is connects all all three Presentation, Business and data layers together.
        /// It has an instance of DataOperations class used to deal with data layer of application.
        /// </summary>
        /// 
        private DataOperations dataOps;                     // private instance of data operations to deal with data storage.
        private Dictionary<string, string> textSpeak;           // private dictionary used for text speak abbreviations
        private List<string> quarantinedList = new List<string>();



        public MessageOperationsFacade()
        {
            dataOps = new DataOperations();                 // initialise instace of data operations class
            textSpeak = dataOps.pAbberviationList;          // load abbreviations to list
        }


        //************************* 
        // Methods
        //*************************//

        // this method is used to process the raw SMS message. take message body as list of strings (Each line) and id as input
        // return SMS object
        public SMS buildSmsMessage(List<string> body, string messageId)
        {
            string patternSender = @"\s{0,}sender+\:";
            string patternText = @"\s{0,}Text+\:";

            SMS sms = new SMS();
            sms.pMessageId = messageId;
            sms.pSender = extractInput("Sender", patternSender, body[0].Trim(), "1");       // extract the actual subject and removes the sender label from it
            string messagetext = "";
            for (int i = 1; i < body.Count; i++)        // builtd message text because it can me on more than 1 line
            {
                messagetext = messagetext + body[i];
            }
            messagetext = extractInput("Text", patternText, messagetext, "2");          // extract the actual message text and removes label from it
            sms.pMessageText = this.filterTextSpeak(messagetext);               // filter the text speak words and assign new messageText to property
            dataOps.saveMessageToFile(sms);                                 // save processed message to file.
            return sms;
        }

        // this method is used to process the raw Tweet message. take message body as list of strings (Each line) and id as input
        // return Tweet object
        public Tweet buildTwitterMessage(List<string> body, string messageId)
        {
            string patternSender = @"\s{0,}sender+\:";
            string patternText = @"\s{0,}Text+\:";
            Tweet tweet = new Tweet();
            tweet.pMessageId = messageId;
            tweet.pSender = extractInput("Sender", patternSender, body[0].Trim(), "1");   // extract the actual subject and removes the sender label from it
            string messagetext = "";
            for (int i = 1; i < body.Count; i++)                                        // built message text because it can me on more than 1 line
            {
                messagetext = messagetext + body[i];
            }
            messagetext = extractInput("Text", patternText, messagetext, "2");      // extract the actual message text and removes the label from it
            tweet.pMessageText = this.filterTextSpeak(messagetext);                 // filter the text speak words and assign new messageText to property
            dataOps.saveMessageToFile(tweet);                                       // save processed message to file.
            return tweet;
        }

        // this public method is used to process the raw Email message. take message body as list of strings (Each line) and id as input
        // return Email object
        public Email buildEmailMessage(List<string> body, string messageId)
        {
            string patternSender = @"\s{0,}sender+\:";
            string patternSubject = @"\s{0,}subject+\:";
            string patternText = @"\s{0,}Text+\:";
            string patternCentreCode = @"\s{0,}sport\s{1}centre\s{1}code+\:";
            string patternNature = @"\s{0,}nature\s{1}of\s{1}incident+\:";

            Email email = new Email();
            email.pMessageId = messageId;

            email.pSender = extractInput("Sender", patternSender, body[0].Trim(), "1");         // extract the email sender and removes the sender label from it

            email.pSubject = extractInput("Subject", patternSubject, body[1].Trim(), "2");      // extract the email sender and removes the subject label from it

            int index = 2;
            string text = "";
            if (email.pSubject.StartsWith("SIR", StringComparison.OrdinalIgnoreCase))           // checks if Email's subject is of SIR type
            {
                email.pCentreCode = extractInput("Sport centre code", patternCentreCode, body[2].Trim(), "3");          // then extract the sport centre code from input message body
                string incidentNature = extractInput("Nature of incident", patternNature, body[3].Trim(), "4");         // extract the nature of incident from input message body
                validateIncidentList(incidentNature);                                                                // method validates if nature of incident is the one from provided list
                email.pIncidentNature = incidentNature;                                            // assign nature of incident to property
                index = 4;
            }
            for (int i = index; i < body.Count; i++)                                       // built message text because it can me on more than 1 line
            {
                text = text + body[i];
            }
            text = extractInput("Text", patternText, text, "2");                    // extract the email's message text and removes the text label from it        
            email.pMessageText = this.filterURL(text);
            dataOps.saveMessageToFile(email);                                       // save processed email message to file
            return email;
        }

        // this private method is used to find the URLs in the text and replace them with <Url Quarantined> message
        // takes message text as input and return string
        private string filterURL(string text)
        {
            //string patternSender = @"\.[\w\D]{2,}$";
            //List<string> urls = Regex.Matches(text, patternSender).Cast<Match>().Select(item => item.Value).ToList();
            string patternUrlEnd = @"\.[\w\D]{2,}$";

            string[] data = text.Trim().Split(' ');                                             // split the string on white space and convert it to array. needed for URL finding

            for (int i = 0; i < data.Length; i++)
            {
                string key = data[i].ToLower();
                if (key.StartsWith("www") || key.StartsWith("http") && Regex.IsMatch(text, patternUrlEnd))      // finds the URL in message text
                {
                    data.SetValue("<URL Quarantined>", i);                                                      // replace URL with <URL Quarantined> text
                    if (!quarantinedList.Contains(key))                                                     // if URL does not exist on quaranlied list used to display removed URLs on user interface
                    {
                        quarantinedList.Add(key);                                              // add URL to quarantined list 
                    }
                }
            }
            return String.Join(" ", data);                                  // convert array of message text to one single string separated by white spaces.
        }


        // this private method is used to extract the actual meaningful text and from the input message body.
        // it is used for all type of properties. It removes the label of property from raw text.
        // takes label name, pattern to identify label, user input and position of line as string parameters
        // return extracted property text as string
        private string extractInput(string label, string pattern, string input, string pos)
        {
            if (!Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase))        // checks if input has label in the text
            {                                                                           // if not then throw error with a meaningful error message specifying what went wrong and how to fix it
                throw new Exception("Invalid " + label + " label.\n" + label + " label must be on " + pos + " line in '" + label + ":' format");
            }
            string result = input.ToLower().Replace(label.ToLower() + ":", "").Trim();                    // removes the label from input text
            return result.Trim();                                                                       // return the extracted text
        }

        // this private method is used to find the abbreviations in the message text and replace them using the data from a abbreviations file
        // takes message text as input and return string
        private string filterTextSpeak(string text)
        {
            string[] data = text.Trim().Split(' ');
            string[] message = text.Trim().Split(' ');

            for (int i = 0; i < data.Length; i++)
            {
                string word = data[i].ToUpper();
                if (this.textSpeak.ContainsKey(word))                // checks if word from input text exist on the text speak abbreviations list
                {
                    string expandedWord = word + " <" + this.textSpeak[word] + ">";
                    message.SetValue(expandedWord, i);                              // add expanded word to the message text
                }
            }
            return String.Join(" ", message);                         // convert array of message text to one single string separated by white spaces.
        }

        // this public method is used to return the list of quarantined URLs
        // It is used by presentation layer to display it on user interface
        public List<string> retrieveQuarantinedList()
        {
            return quarantinedList;
        }

        // this method is used to retrieve message inputs from file
        // it returns each message as separate element of string array
        public string[] retrieveFileInput()
        {
            string[] filedata = dataOps.pInputFromFile;             // calls the pInputFromFile property of data operations class from data layer to get file input data
            return filedata;
        }

        // this private method is used to check if provided nature of incident is valid or not
        // takes message text as input and return string
        private void validateIncidentList(string incidentType)
        {
            string result = "No";
            List<string> incidentList = new List<string>();
            incidentList = dataOps.pIncidentList;                       // access incidentList from data layer's class

            incidentList.ForEach(incident =>
            {
                if (incidentType.Equals(incident, StringComparison.OrdinalIgnoreCase))              // checks if provided nature of incident exist on the permitted inident natures list
                {
                    result = incident;                                                          // if correct incident nature then assigns its value to property
                }
            });
            if (result == "No")                                                                 // checks if incident nature is invalid
            {
                throw new Exception("Unknown incident nature.\nEmail text must have a valid incident nature.");             // show exception
            }
        }
    }
}
