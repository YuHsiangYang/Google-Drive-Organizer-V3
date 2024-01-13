using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Diagnostics;

namespace Google_Drive_Organizer_V3.Classes
{
    //This is the class to store the EXIF data retrieved from the script.
    public class ImageExif
    {
        public ImageJsonData JsonData = new ImageJsonData();
        public ImageEXIFData EXIFData = new ImageEXIFData();
    }
    public class ImageEXIFData
    {
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
                if (value != "")
                {
                    CultureInfo culture = CultureInfo.CurrentCulture;
                    phototakentime_DateTime = DateTime.ParseExact(value, "yyyy:MM:dd HH:mm:ss", culture);
                    PhotoTakemTime = phototakentime_DateTime.ToString();

                }
                else if (value == "")
                {
                    PhotoTakemTime = "";
                }
            }
        }
        public string CameralModel { get; set; } = "";
        public string ImagePath { get; set; } = "";

        public Dictionary<string, double> GPS_Longitude { get; set; } = new Dictionary<string, double>();
        public Dictionary<string, double> GPS_Latitude { get; set; } = new Dictionary<string, double>();
        public string GPS_Altitude { get; set; } = "";
        public string Artist { get; set; } = "";
        public string UserComment { get; set; } = "";
    }
}
