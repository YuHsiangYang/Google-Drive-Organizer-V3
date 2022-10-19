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

        private string json_phototakentime { get; set; } = "";
        public string Json_PhotoTakenTime
        {
            get
            {
                return json_phototakentime;
            }
            set
            {
                DateTime pre = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                pre = pre.AddSeconds(int.Parse(value));
                Json_PhotoTakenTime_DateTime = pre;
                Photo_TakenTime_Dictionary.Add("Year", Json_PhotoTakenTime_DateTime.Year);
                Photo_TakenTime_Dictionary.Add("Month", Json_PhotoTakenTime_DateTime.Month);
                Photo_TakenTime_Dictionary.Add("Day", Json_PhotoTakenTime_DateTime.Day);
                json_phototakentime = pre.ToString();
            }
        }
        public DateTime Json_PhotoTakenTime_DateTime { get; set; } = new DateTime();
        public string Json_GPS_Longitude { get; set; } = "";
        public string Json_GPS_Latitude { get; set; } = "";
        public string Json_GPS_Altitude { get; set; } = "";
        public string Json_Location { get; set; } = "";
        public string MD5 { get; set; } = "";
    }
}
