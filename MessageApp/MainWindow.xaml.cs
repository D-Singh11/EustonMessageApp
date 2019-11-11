using BusinessLayer;
using DataLayer;
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
        string messageType = "";
        private DataOperations dataOps;
        private Dictionary<string, string> testSpeak;
        private List<string> incidentList;
        public MainWindow()
        {
            InitializeComponent();
            dataOps = new DataOperations();
            testSpeak = dataOps.AbberviationList;
            incidentList = dataOps.IncidentList;
        }

        private void Bt_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string header = tb_header.Text;
                Message msg = new Message();
                msg.MessageId = header;
                if (String.IsNullOrWhiteSpace(tb_body.Text))
                {
                    throw new Exception("Message body must not be blank.");
                }
                else
                {
                    string messageBody = Regex.Replace(tb_body.Text, @"\s+", " ");
                    if (msg.MessageType =="s")
                    {
                        build_SMS(messageBody);
                    }
                    if (msg.MessageType =="m") { build_Email_Message(messageBody); }
                    if (msg.MessageType =="t") { build_Twitter_Message(messageBody); }
                }
                
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message,"Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

            tb_id.Text = sms.MessageId;
            tb_sender.Text = sms.Sender;
            tb_message.Text = sms.Message;
            MessageBox.Show(sms.Message);
            
        }

        private void build_Twitter_Message(string body)
        {
            Tweet tweet = new Tweet();
            tweet.MessageId = tb_header.Text;
            tweet.Sender = body;
            string message = this.filterTextSpeak(body);
            tweet.Message = message;

            tb_id.Text = tweet.MessageId;
            tb_sender.Text = tweet.Sender;
            tb_message.Text = tweet.Message;

            buildTrendingList(tweet.Message);
            buildMentionList(tweet.Message);
            MessageBox.Show(tweet.Message);

        }

        private void build_Email_Message(string body)
        {
            string[] msg = body.Split(',');
            if (msg.Count() != 3)
            {
                throw new Exception("Enter message comprising sender, subject and message separated by comma.");
            }
            Email email = new Email();
            email.MessageId = tb_header.Text;
            email.Sender = msg[0];
            email.Subject = msg[1];
            string messageText = msg[2];
            if (email.Subject.Contains("SIR"))
            {
                messageText = build_SIR_Message(messageText);
            }
            email.Message = this.filterURL(messageText);

            tb_id.Text = email.MessageId;
            tb_sender.Text = email.Sender;
            tb_message.Text = " Subject: " + email.Subject + "\n Text : " + email.Message;

            //buildTrendingList(email.Message);
            //buildMentionList(email.Message);
            MessageBox.Show(email.Message);

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

        private string build_SIR_Message(string text)
        {
            Dictionary<string, string> SIRList = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            string patternCentreCode = @"^[\d]{2}\-[\d]{3}\-[\d]{2}";
            string result = "No";
            string centreCode = Regex.Match(text.Trim(), patternCentreCode).Value;
            if (!Regex.IsMatch(text.Trim(), patternCentreCode))
            {
                throw new Exception("Invalid centre code.\nIt must be in 00-000-00 format.");
            }
            string incidentType = text.Substring(centreCode.Length, 20).ToLower();
            incidentList.ForEach(incident =>
            {
                if (incidentType.Contains(incident.ToLower()))
                {
                    result = incident;
                    SIRList.Add(centreCode, incident);
                    lb_sirList.Items.Add(centreCode +" "+ incident);
                }    
            });
            if (result =="No")
            {
                throw new Exception("Unknown incident nature.\nEnter a valid incident nature.");
            }
            
            return text.Replace(centreCode + " " + result, "");
        }
    }
}
