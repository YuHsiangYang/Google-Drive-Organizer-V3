using System;
using System.Collections.Generic;
using System.Globalization;

namespace Google_Drive_Organizer_V3.Classes
{
    public class ImageExif
    {
        private string NoData = ApplicationVariables.NoData;
        public DateTime phototakentime_DateTime { get; set; } = new DateTime();
        public Dictionary<string, int> Photo_TakenTime_Dictionary { get; set; } = new Dictionary<string, int>();
        private string PhotoTakemTime { get; set; } = "";
        public string phototakentime
        {
            get
            {
                return PhotoTakemTime;
            }
            set
            {
                if (value != NoData)
                {
                    CultureInfo culture = CultureInfo.CurrentCulture;
                    phototakentime_DateTime = DateTime.ParseExact(value, "yyyy:MM:dd HH:mm:ss", culture);
                    PhotoTakemTime = phototakentime_DateTime.ToString();

                }
                else if (value == NoData)
                {
                    PhotoTakemTime = NoData;
                }
            }
        }
        public string CameraManufactor { get; set; } = "";
        public string ImagePath { get; set; } = "";

        public string GPS_Longitude { get; set; } = "";
        public string GPS_Latitude { get; set; } = "";
        public string GPS_Altitude { get; set; } = "";

        private string phototakentime_json { get; set; } = "";
        public string Json_PhotoTakenTime
        {
            get
            {
                return phototakentime_json;
            }
            set
            {
                DateTime pre = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                pre = pre.AddSeconds(int.Parse(value));
                PhotoTakenTime_DateTime_Json = pre;
                Photo_TakenTime_Dictionary.Add("Year", PhotoTakenTime_DateTime_Json.Year);
                Photo_TakenTime_Dictionary.Add("Month", PhotoTakenTime_DateTime_Json.Month);
                Photo_TakenTime_Dictionary.Add("Day", PhotoTakenTime_DateTime_Json.Day);
                phototakentime_json = pre.ToString();
            }
        }
        public DateTime PhotoTakenTime_DateTime_Json { get; set; } = new DateTime();
        public string Json_GPS_Longitude { get; set; } = "";
        public string Json_GPS_Latitude { get; set; } = "";
        public string Json_GPS_Altitude { get; set; } = "";
        public string JsonPath { get; set; } = "";
    }
}
