using System;
using System.Collections.Generic;

namespace Google_Drive_Organizer_V3.Classes
{
    public class ImageJsonData
    {
        /// <summary>
        /// A ImageJsonData class containing the JSON file implemented in object form.
        /// Important notes:
        /// When setting the Json_PhotoTakenTime, the input should be in unix timestamp
        /// </summary>
        public Dictionary<string, int> Photo_TakenTime_Dictionary { get; set; } = new Dictionary<string, int>(); //This dictionary is used for getting the JSON photo taken time using key (Year, Month, Day)

        private string phototakentime { get; set; } = "";

        //This is the class implementing the json photo taken time.
        public string Json_PhotoTakenTime
        {
            get
            {
                return phototakentime;
                //Return the phototakentime in string format
            }
            set
            {
                DateTime pre = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc); //Convert the time using the unix timestamp
                pre = pre.AddSeconds(int.Parse(value));
                pre = pre.ToLocalTime();

                //TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
                //DateTimeOffset utcOffset = new DateTimeOffset(pre, TimeSpan.Zero);
                //DateTimeOffset localTime = utcOffset.ToOffset(localTimeZone.GetUtcOffset(pre))

                PhotoTakenTime_DateTime = new DateTimeOffset(pre, TimeZoneInfo.Local.GetUtcOffset(pre)).DateTime;
                Photo_TakenTime_Dictionary.Add("Year", PhotoTakenTime_DateTime.Year);
                Photo_TakenTime_Dictionary.Add("Month", PhotoTakenTime_DateTime.Month);
                Photo_TakenTime_Dictionary.Add("Day", PhotoTakenTime_DateTime.Day);
                phototakentime = pre.ToString();
            }
        }
        public DateTime PhotoTakenTime_DateTime { get; set; } = new DateTime(); //This is the JSON photo taken time in a DateTime object.
        public Dictionary<string, double> GPS_Longitude { get; set; } = new Dictionary<string, double>(); //A record for the GPS Longitude
        public Dictionary<string, double> GPS_Latitude { get; set; } = new Dictionary<string, double>(); //A record for the GPS Latitude
        public string GPS_Altitude { get; set; } = ""; // Record of how high was the photo taken
        public string JsonPath { get; set; } = ""; //Path of the json file
    }
}
