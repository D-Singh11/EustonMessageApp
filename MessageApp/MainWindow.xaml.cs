using BusinessLayer;
using DataLayer;
using Microsoft.Win32;
using System;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataOperations dataOps;
        private Dictionary<string, string> testSpeak;
        private List<string> incidentList = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
            //tb_body.Text = "Sender:\nSubject:\nText:";
            dataOps = new DataOperations();
            testSpeak = dataOps.AbberviationList;
            incidentList = dataOps.IncidentList;
             //tb_body.Text = "Sender: \nSubject: \nText: ";
        }

        private void Bt_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                processMessage();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message,"Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            

        }

        private void processMessage()
        {
            
            Message msg = new Message();
            msg.MessageId = tb_header.Text;
            if (String.IsNullOrWhiteSpace(tb_body.Text))
            {
                throw new Exception("Message body must not be blank.");
            }
            else
            {
                string messageBody = Regex.Replace(tb_body.Text, @"\s+", " ");  // removes extra space
                if (msg.MessageType == "s")
                {
                    build_SMS(messageBody);
                }
                if (msg.MessageType == "m") { build_Email_Message(); }
                if (msg.MessageType == "t") { build_Twitter_Message(messageBody); }
            }

        }

        private void build_SMS(string body)
        {
            //string pattern = @"^(?<phoneNumber>[\+][\d]{9,15})\s+(?<text>[\w]{1,40})?$";
            SMS sms = new SMS();
            sms.MessageId = tb_header.Text;
            sms.Sender = body;
            string message = this.filterTextSpeak(body);
            sms.Message = message;
            tblock_message_output.Text = "Message id: " + sms.MessageId + "\nSender: " + sms.Sender + "\nText: " + sms.Message;
            dataOps.saveMessageToFile(sms);
            MessageBox.Show(sms.Message);
            
        }

        private void build_Twitter_Message(string body)
        {
            Tweet tweet = new Tweet();
            tweet.MessageId = tb_header.Text;
            tweet.Sender = body;
            string message = this.filterTextSpeak(body);
            tweet.Message = message;

            buildTrendingList(tweet.Message);
            buildMentionList(tweet.Message);
            dataOps.saveMessageToFile(tweet);
            tblock_message_output.Text = "Message id: " + tweet.MessageId + "\nSender: " + tweet.Sender + "\nText: " + tweet.Message;
            MessageBox.Show(tweet.Message);

        }

        private void build_Email_Message()
        {
            Email email = parseEmail();

            if (email.Subject.ToUpper().Contains("SIR"))
            {
                buildSirList(email.CentreCode, email.IncidentNature);;
            }
            
            tblock_message_output.Text = ("Message id: " + email.MessageId + "\nSender: " + email.Sender + " Subject: " + email.Subject
                + "Sport centre code: "+ email.CentreCode + "Nature of incident: " + email.IncidentNature + "\nText: " + email.MessageText);
            
            dataOps.saveMessageToFile(email);
            MessageBox.Show(email.MessageText);

        }

        private string filterTextSpeak(string text)
        {
            string[] data = text.Trim().Split(' ');
            string[] message = text.Trim().Split(' ');

            for (int i = 0; i<data.Length; i++)
            {
                string key = data[i].ToUpper();
                if (testSpeak.ContainsKey(key))
                {
                    string word = key + " <" + this.testSpeak[key] + ">";
                    message.SetValue(word, i );
                }     
            }
            return String.Join(" ", message);
        }

        private void buildTrendingList(string text)
        {
            Dictionary<string, int> trendingList = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            string patternHashtag = @"[\#][\w]{1,35}";
            MatchCollection result = Regex.Matches(text, patternHashtag);

            foreach (var item in result)
            {
                string key = item.ToString();
                int val;
                if (trendingList.TryGetValue(key, out val))
                {
                    trendingList[key] = val + 1;
                } else
                {
                    trendingList.Add(key, 1);
                }
            }
            foreach (var item in trendingList)
            {
                lb_trendList.Items.Add(item);
            }

        }

        private void buildMentionList(string text)
        {
            string pattern = @"[\@][\w]{1,15}";
            List<string> mentionList = new List<string>();
            MatchCollection result = Regex.Matches(text, pattern);
            foreach (var item in result)
            {
                if (!mentionList.Contains(item.ToString().ToLower()))
                {
                    mentionList.Add(item.ToString());
                }
            }
            lb_mentionList.ItemsSource = mentionList;

        }

        private string filterURL(string text)
        {
            //string patternSender = @"\.[\w\D]{2,}$";
            //List<string> urls = Regex.Matches(text, patternSender).Cast<Match>().Select(item => item.Value).ToList();
            string patternUrlEnd = @"\.[\w\D]{2,}$";

            List<string> quarantinedList = new List<string>();
            string[] data = text.Trim().Split(' ');

            for (int i = 0; i < data.Length; i++)
            {
                string key = data[i].ToLower();
                if (key.StartsWith("www") || key.StartsWith("http") && Regex.IsMatch(text, patternUrlEnd))
                {
                    data.SetValue("<URL Quarantined>", i);
                    if (!quarantinedList.Contains(key))
                    {
                        quarantinedList.Add(key);
                    }
                }
            }
            lb_quarantinedList.ItemsSource = quarantinedList;
            return String.Join(" ", data);
        }

        //private void validateInputLabel(string label, string pattern, string input, string pos)
        //{
        //    if (!Regex.IsMatch(label, pattern))
        //    {
        //        throw new Exception("Invalid "+ label + "label.\n"+ label + "label must be on " +pos+ " line in '"+ label +":' format");
        //    }
        //}

        private Email parseEmail()
        {
            Email email = new Email();
            email.MessageId = tb_header.Text;
            email.Sender = tb_body.GetLineText(0);
            email.Subject = tb_body.GetLineText(1);
            int index = 2;
            string text = "";
            if (email.Subject.StartsWith("SIR", StringComparison.OrdinalIgnoreCase))
            {
                email.CentreCode = tb_body.GetLineText(2);
                email.IncidentNature = tb_body.GetLineText(3).Trim();
                index = 4;
            }
            for (int i =index; i < tb_body.LineCount; i++)
            {
                text = tb_body.GetLineText(i);
            }
            email.MessageText = this.filterURL(text);
            
            

            //string messageBody = tb_body.Text.ToLower();
            //string senderLb = "sender:";
            //string subjectLb = "subject:";
            //string textLb = "text:";
            //int a = messageBody.IndexOf(senderLb);
            //int b = messageBody.IndexOf(subjectLb);
            //int c = messageBody.IndexOf(textLb);
            //string sender, subject;
            //try
            //{
            //    sender = messageBody.Substring(a + senderLb.Length, b - senderLb.Length);
            //    subject = messageBody.Substring(b + subjectLb.Length, c - subjectLb.Length - b);
            //    text = messageBody.Substring(c + textLb.Length);

            //}
            //catch
            //{
            //    throw new Exception("Message body invlaid input.\nLabels must be correctly spelt and in following order:\nsender:\nsubject:\ntext:");
            //}
            //Email email = new Email();
            //email.MessageId = tb_header.Text;
            //email.Sender = sender;
            //email.Subject = subject;
            //email.MessageText = text;
            return email;
        }

        private void parseSirEmailText(string text)
        {
            //string messageBody = text.ToLower().Trim();
            //string scLb = "sport centre code:";
            //string incidentLb = "nature of incident:";
            //int a = messageBody.IndexOf(scLb);
            //int b = messageBody.IndexOf(incidentLb);
            //string centreCode, incidentType;
            //try
            //{
            //    centreCode = messageBody.Substring(a + scLb.Length, b - scLb.Length);
            //    incidentType = messageBody.Substring(b + incidentLb.Length, 20);
            //    buildSirList(centreCode, incidentType);

            //}
            //catch
            //{
            //    throw new Exception("Missing sports centre or nature of incident.\nLabels must be correctly spelt and in following order:\nsport centre code:\nnature of incident");
            //}
        }

        private void buildSirList(string centreCode, string incidentType)
        {
            Dictionary<string, string> SIRList = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            string result = "No";

            this.incidentList.ForEach(incident =>
            {
                if (incidentType.Equals(incident, StringComparison.OrdinalIgnoreCase))
                {
                    result = incident;
                    SIRList.Add(centreCode, incident);
                    lb_sirList.Items.Add(centreCode + " " + incident);
                }
            });
            if (result == "No")
            {
                throw new Exception("Unknown incident nature.\nEmail text must have a valid incident nature.");
            }
        }

        private void clearInputs()
        {
            tb_header.Clear();
            tb_body.Clear();
            tblock_message_output.Text = "";

        }

        private void Button_OpenFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] fileData = dataOps.InputFromFile;
                lb_inputData.Items.Clear();
                foreach (var message in fileData)
                {
                    lb_inputData.Items.Add(message);
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void Lb_InputData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lb_inputData.SelectedValue == null)
            {
                MessageBox.Show("No value Selected", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                try
                {
                    string selctedMessage = lb_inputData.SelectedItem.ToString();
                    tb_header.Text = selctedMessage.Trim().Substring(0, 10);
                    tb_body.Text = selctedMessage.Replace(tb_header.Text, "").Replace(",","\n").Trim();
                    //processMessage();                                         //only use if want to process on selction, otherwise click save
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }  
            }
        }

        private void Bt_Clear_Click(object sender, RoutedEventArgs e)
        {
            this.clearInputs();
        }
    }
}