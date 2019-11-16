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
                     this.buildIncidentList();
                    _dataOperations = new DataOperations();
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
            get {
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
            
    }
}
