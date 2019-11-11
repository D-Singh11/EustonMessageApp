using System;
using System.Collections.Generic;
using System.IO;

namespace DataLayer
{
    public class DataOperations
    {
        private DataOperations _dataOperations;
        private Dictionary<string, string> abberviationList = new Dictionary<string, string>();
        private List<string> incidentList = new List<string>();

        public DataOperations pDataOperations
        {
            get
            {
                if (_dataOperations == null)
                {
                    _dataOperations = new DataOperations();
                    incidentList.Add("Theft of Properties");
                    incidentList.Add("Staff Attack");
                    incidentList.Add("Device Damage");
                    incidentList.Add("Raid");
                    incidentList.Add("Customer Attack");
                    incidentList.Add("Staff Abuse");
                    incidentList.Add("Terrorism");
                    incidentList.Add("Suspicious Incident");
                    incidentList.Add("Sport Injury");
                    incidentList.Add("Personal Info Leak");
                }
                return _dataOperations;
            }
        }

        public Dictionary<string, string> AbberviationList
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

        public List<string> IncidentList
        {
            get { return incidentList; }
        }
            
    }
}
