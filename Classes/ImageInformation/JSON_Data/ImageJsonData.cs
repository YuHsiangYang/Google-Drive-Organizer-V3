using System;
using System.Collections.Generic;

namespace Google_Drive_Organizer_V3.Classes
{
    public class ImageJsonData
    {
        public Dictionary<string, int> Photo_TakenTime_Dictionary { get; set; } = new Dictionary<string, int>();

        private string phototakentime { get; set; } = "";
        public string Json_PhotoTakenTime
        {
            get
            {
                return phototakentime;
            }
            set
            {
                DateTime pre = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                pre = pre.AddSeconds(int.Parse(value));
                PhotoTakenTime_DateTime = pre;
                Photo_TakenTime_Dictionary.Add("Year", PhotoTakenTime_DateTime.Year);
                Photo_TakenTime_Dictionary.Add("Month", PhotoTakenTime_DateTime.Month);
                Photo_TakenTime_Dictionary.Add("Day", PhotoTakenTime_DateTime.Day);
                phototakentime = pre.ToString();
            }
        }
        public DateTime PhotoTakenTime_DateTime { get; set; } = new DateTime();
        public Dictionary<string, double> GPS_Longitude { get; set; } = new Dictionary<string, double>();
        public Dictionary<string, double> GPS_Latitude { get; set; } = new Dictionary<string, double>();
        public string GPS_Altitude { get; set; } = "";
        public string JsonPath { get; set; } = "";
    }
}
