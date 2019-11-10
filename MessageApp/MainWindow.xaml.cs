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
        public MainWindow()
        {
            InitializeComponent();
            dataOps = new DataOperations();
            testSpeak = dataOps.AbberviationList;
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
                    if (msg.MessageType =="s")
                    {
                        build_SMS(tb_body.Text);
                    }
                    if (msg.MessageType =="m") {}
                    if (msg.MessageType =="t") { build_Twitter_Message(tb_body.Text); }
                }
                
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message,"Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            

        }

        //private void validate_MessaeBody(string botbdy)
        //{
        //    if (messageType == "s")
        //    {
        //        validate_SMS(body.ToLower());
        //    }
        //}

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
            List<string> distinct = new List<string>();
            MatchCollection result = Regex.Matches(text, pattern);
            foreach (var item in result)
            {
                if (!distinct.Contains(item.ToString().ToLower()))
                {
                    distinct.Add(item.ToString());
                }
            }
            lb_mentionList.ItemsSource = distinct;

        }
    }
}
