using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DataLayer
{
    public class DataOperations
    {
        private DataOperations dataOps;
        private Dictionary<string, string> abberviationList = new Dictionary<string, string>();
        private List<string> incidentList = new List<string>();
        private List<Object> messageList = new List<Object>();
        private string[] inputFromFile;

        public DataOperations() { }


        public DataOperations pDataOps
        {
            get
            {
                if (dataOps == null)
                {
                    dataOps = new DataOperations();
                }
                return dataOps;
            }
        }

        public Dictionary<string, string> pAbberviationList
        {
            get {
                this.retrieveTextSpeak();
                return abberviationList;
            }
        }

        private void retrieveTextSpeak()
        {
            string[] data = File.ReadAllLines("textwords.csv");
            string key, value;

            foreach (var item in data)
            {
                key = item.Split(',')[0];
                value = item.Split(',')[1];
                this.abberviationList.Add(key, value);
            }
        }

        public List<string> pIncidentList
        {
            get {
                buildIncidentList();
                return incidentList;
            }
        }

        private void buildIncidentList()
        {
            this.incidentList.Add("Theft of Properties");
            this.incidentList.Add("Staff Attack");
            this.incidentList.Add("Device Damage");
            this.incidentList.Add("Raid");
            this.incidentList.Add("Customer Attack");
            this.incidentList.Add("Staff Abuse");
            this.incidentList.Add("Terrorism");
            this.incidentList.Add("Suspicious Incident");
            this.incidentList.Add("Sport Injury");
            this.incidentList.Add("Personal Info Leak");
        }

        public string[] pInputFromFile
        {
            get {
                    OpenFileDialog openFile = new OpenFileDialog();
                    openFile.Title = "Choose input text file";
                    openFile.Filter = "TXT files|*.txt";  //TXT files|*.txt
                    if (openFile.ShowDialog() == true)
                    {
                        inputFromFile = File.ReadAllLines(openFile.FileName);
                    }
                    else if (inputFromFile == null)
                    {
                        throw new Exception("Error occured during file access.\n Try again.");
                    }
                
                return inputFromFile;
            }
           
        }

        public void saveMessageToFile(Object obj)
        {
            this.messageList.Add(obj);
            StreamWriter writetoFile = new StreamWriter("people.json", true);
            string messageToSave = JsonConvert.SerializeObject(obj);
            writetoFile.WriteLine(messageToSave);
            writetoFile.Close();

        }
            
    }
}
