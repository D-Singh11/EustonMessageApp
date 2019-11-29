using BusinessLayer;
using DataLayer;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MessageApp
{
    /// <summary>
    /// This class is used to hold the logic for main window (WPF User interface) of application
    /// It is used to take user input and display appropriate outputs on appropriate wpf controls.
    /// It uses the Facade class to interact with the bussiness logic of this application
    /// </summary>
    public partial class MainWindow : Window
    {
        private MessageOperationsFacade msgOps = new MessageOperationsFacade();                 // private instance of facade class
        private Dictionary<string, int> mentionList = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);      // private dictionary used to store and display mention list(embedded twitter handles) on UI
        private Dictionary<string, int> trendingList = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);       // private dictionary used to store and display trend list(hashtags) on UI
        private List<string> quarantinedList = new List<string>();                                                   // private list used to store and display removed URLs on UI

        public MainWindow()
        {
            InitializeComponent();
        }


        // This event handler is used to process and save the raw input message.
        // It takes all the values from the form and process the them to build correct message

        private void Bt_Save_Click(object sender, RoutedEventArgs e)
        {
            try                                             // uses try block to catch any exception thrown by application
            {
                processMessage();                           // local method used to process the raw message
            }
            catch (Exception error)                             // catch block to catch the exceptions thrown by appplication and prevent application to go into error mode
            {
                MessageBox.Show(error.Message,"Error", MessageBoxButton.OK, MessageBoxImage.Error);         // show error message using message box
            }
            

        }

        // this method is used to process the raw input of messsage to sanitised and structured message.
        // It is used for both user input and finput from file
        private void processMessage()
        { 
            Message msg = new Message();
            msg.pMessageId = tb_header.Text;                        // assigns user input from header to property, property of message class validates the input
            if (String.IsNullOrWhiteSpace(tb_body.Text))                    // if blank message body then throw eexception
            {
                throw new Exception("Message body must not be blank.");
            }
            else                                                                    // otherwise process the message
            {
                string messageType = tb_header.Text.Substring(0, 1).ToLower();
                List<string> bodyMessage = new List<string>();                      // list of string used to store message body input line by line
                for (int i = 0; i < tb_body.LineCount; i++)
                {
                    bodyMessage.Add(tb_body.GetLineText(i).Trim());                 // get each line of text from message body and add it to list
                }
                if (messageType == "s")                                                 // checks if message type is SMS
                {
                    SMS sms = msgOps.buildSmsMessage(bodyMessage, msg.pMessageId);          // builds the and process the SMS message
                    tblock_message_output.Text = "Message id: " + sms.pMessageId + "\nSender: " + sms.pSender + "\nText: " + sms.pMessageText;          // display processed message on UI
                    MessageBox.Show("Message has been sucessfully processed.", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);             // message box updates the user on sucessful message processing
                }
                if (messageType == "m")                                                             // checks if message type is Email
                {
                    Email email = msgOps.buildEmailMessage(bodyMessage, msg.pMessageId);            // builds the and process the Email message
                    if (email.pSubject.ToUpper().Contains("SIR"))
                    {
                        lb_sirList.Items.Add(email.pCentreCode + ": " + email.pIncidentNature.ToUpper());    // build SIR list
                    }
                    displayQuarantinedList();                                                   // show removed URLs in list
                    
                    tblock_message_output.Text = ("Message id: " + email.pMessageId + "\nSender: " + email.pSender + "\nSubject: " + email.pSubject         // display processed message on UI
                    + "\nSport centre code: " + email.pCentreCode + "\nNature of incident: " + email.pIncidentNature.ToUpper() + "\nText: " + email.pMessageText);

                    MessageBox.Show(email.pMessageText, "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);                        // message box updates the user on sucessful message processing
                }
                if (messageType == "t")                                                                     // checks if message type is Tweet
                {
                    Tweet tweet = msgOps.buildTwitterMessage(bodyMessage, msg.pMessageId);                  // builds the and process the Tweet message
                    buildTrendingList(tweet.pMessageText);                                                  // builds and display trend list  (hastags)                            
                    buildMentionList(tweet.pMessageText);                                                   // build and display mention list (embedded twitter handle in messageText)
                    tblock_message_output.Text = "Message id: " + tweet.pMessageId + "\nSender: " + tweet.pSender + "\nText: " + tweet.pMessageText;        // display processed message on UI
                    MessageBox.Show(tweet.pMessageText, "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);                     // message box updates the user on sucessful message processing

                }
            }

        }

        // this method is used to build the trend list - a list of hashtags used in the message text
        // it takes message text as string input'
        // display the build list on UI using listbox control
        private void buildTrendingList(string text)
        {
            string patternHashtag = @"[\#][\w]{1,35}";          // regex pattern to detect hashtag in message text
            MatchCollection result = Regex.Matches(text, patternHashtag);

            foreach (var item in result)
            {
                string key = item.ToString();
                int val;
                if (trendingList.TryGetValue(key, out val))                 // checks if hashtag already exist on list
                {
                    trendingList[key] = val + 1;                            // it does then add 1 to previous value to count the times it has been used
                }
                else                                                        // otherwise add it to trending list using key value pair
                {
                    trendingList.Add(key, 1);
                }
            }
            lb_trendList.Items.Clear();
            foreach (var item in trendingList)
            {
                lb_trendList.Items.Add(item);                   // add items to UI listbox control
            }

        }

        private void buildMentionList(string text)
        {
            string pattern = @"[\@][\w]{1,15}";                      // regex pattern to detect embedded twitter handles in message text
            MatchCollection result = Regex.Matches(text, pattern);

            foreach (var item in result)
            {
                string key = item.ToString();
                int val;
                if (mentionList.TryGetValue(key, out val))          // checks if twitter id already exist on list
                {
                    mentionList[key] = val + 1;                     // it does then add 1 to previous value to count the times it has been used
                }
                else                                                // otherwise add it to trending list using key value pair
                {
                    mentionList.Add(key, 1);
                }
            }
            lb_mentionList.Items.Clear();
            foreach (var item in mentionList)
            {
                lb_mentionList.Items.Add(item);                     // add items to UI listbox control
            }

        }


        // this method is used to retrieve list of quarantined URLs from the business layer and
        // display them on user interface using listbox  control
        private void displayQuarantinedList()
        {
            quarantinedList = msgOps.retrieveQuarantinedList();             //retrieve list of removed URLs
            lb_quarantinedList.ItemsSource = quarantinedList;               // add them to the UI listbox
            lb_quarantinedList.Items.Refresh();
        }


        // this method is used to clear the values from the form
        private void clearInputs()
        {
            tb_header.Clear();
            tb_body.Clear();
            tblock_message_output.Text = "";

        }


        // This event hadnler is used to handle the mousedoubleClick event of input data listbox.
        // Once user double click on the uploaded message in the list box, it is assigned to the input header and message bosy controls
        // so that user can inspect it before processing
        private void Lb_InputData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lb_inputData.SelectedValue == null)             // checks if selected item has null value and show error message if it is null
            {
                MessageBox.Show("No value Selected", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else                                                                    // otherwise load the selected item/message to header and message body controls
            {
                try
                {
                    string selectedMessage = lb_inputData.SelectedItem.ToString();
                    tb_header.Text = selectedMessage.Trim().Substring(0, 10);                        // assign the messageid from selected message to header textbox
                    tb_body.Text = selectedMessage.Replace(tb_header.Text, "").Replace(",","\n").Trim();                // assign the message body from selected message to body textbox
                }
                catch (Exception error)                     // catch block to catch exceptions and stop application to go into break mode
                {
                    MessageBox.Show(error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);            // show error message
                }  
            }
        }

        // This event hadnler is used to upload a data file from file system when user click on "upload file button
        // Once user double click on the uploaded message in the list box, it is assigned to the input header and message bosy controls
        // so that user can inspect it before processing
        private void Button_OpenFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] fileData = msgOps.retrieveFileInput();             // uses method of data operation class to read input file's data and store to array of string
                lb_inputData.Items.Clear();                                 // clear previous entries from the uploaded lixt box control to display recently uploaded messages
                foreach (var message in fileData)
                {
                    lb_inputData.Items.Add(message);                        // add input message to listbox control
                }
            }
            catch (Exception error)                                          // catch block to catch exceptions and stop application to go into break mode
            {
                MessageBox.Show(error.Message, "File not foud", MessageBoxButton.OK, MessageBoxImage.Error);                // show error message
            }

        }

        // this event handler is raised when user clicks on the "clear inputs button"
        // it is used to clear thethe form
        private void Bt_Clear_Click(object sender, RoutedEventArgs e)
        {
            this.clearInputs();                 // calls local method to clear input form
        }

    }
}