using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Converters;

namespace Google_Drive_Organizer_V3.Classes
{
    public static class ImageInfo_Functions
    {

        /// <summary>
        /// Use the GetEXIF.exe script from "Scripts" to retrieve EXIF data using the image path.
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>

        public static async Task<Dictionary<string, string>> ExtractEXIFDataFrom_GetEXIF_Script(string imagePath)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            List<string> result = new List<string>();
            ProcessStartInfo exif_script = new ProcessStartInfo()
            {
                FileName = Path.GetFullPath(@".\Scripts\GetEXIF.exe"),
                Arguments = string.Format("\"{0}\"", imagePath),//Set up the argument
                UseShellExecute = false,
                RedirectStandardOutput = true, //Get the output when there when error occurred
                CreateNoWindow = true, //To prevent the default command-line window.
            };
            ImageExif exif = new ImageExif();
            await Task.Run(async () =>
            {
                using (Process process = await Task.Run(() => Process.Start(exif_script)))
                {
                    using (StreamReader read = process.StandardOutput)
                    {

                        try
                        {
                            dictionary = read.ReadToEnd().ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList().Distinct().ToDictionary(x => x.Substring(0, x.IndexOf(':')).Trim(), x => x.Substring(x.IndexOf('[') + 1, Math.Abs(x.IndexOf('[') - x.IndexOf(']') + 1)).Trim());
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Caught the same key"); //When there are two identical keys shown.
                        }
                    }
                }
            });
            return dictionary;
        }

        public static async Task<ImageExif> LoadImageInfo_Async(MatchItem_Class input_item)
        {
            ImageExif detail = new ImageExif();
            Dictionary<string, string> exif_dictionary = await ExtractEXIFDataFrom_GetEXIF_Script(input_item.ImagePath);

            //process image exif
            detail.EXIFData.phototakentime = exif_dictionary.ContainsKey("DateTime") == true ? exif_dictionary["DateTime"] : "";
            var exif_value = "";
            detail.EXIFData.GPS_Latitude = String_To_GPSCoordinatesDictionary(exif_dictionary.TryGetValue("GPSLatitude", out exif_value) ? exif_value : "");
            detail.EXIFData.GPS_Longitude = String_To_GPSCoordinatesDictionary(exif_dictionary.TryGetValue("GPSLongitude", out exif_value) ? exif_value : "");
            detail.EXIFData.ImagePath = input_item.ImagePath;
            detail.EXIFData.CameralModel = exif_dictionary.TryGetValue("Model", out exif_value) ? exif_value : "";
            detail.EXIFData.Artist = exif_dictionary.TryGetValue("Artist", out exif_value) ? exif_value : "";
            detail.EXIFData.UserComment = exif_dictionary.TryGetValue("UserComment", out exif_value) ? exif_value : "";

            //process json
            JObject json_object = await Task.Run(() => JObject.Parse(File.ReadAllText(input_item.JsonPath)));
            detail.JsonData.JsonPath = input_item.JsonPath;
            detail.JsonData.GPS_Latitude = Double_To_DMS((double)json_object["geoDataExif"]["latitude"]);
            detail.JsonData.GPS_Longitude = Double_To_DMS((double)json_object["geoDataExif"]["longitude"]);
            detail.JsonData.GPS_Altitude = json_object["geoDataExif"]["altitude"].ToString();
            detail.JsonData.Json_PhotoTakenTime = json_object["photoTakenTime"]["timestamp"].ToString();
            return detail;
        }

        public static async Task<List<ImageExif>> LoadImageInfo_Record(List<MatchItem_Class> matches_input, IProgress<LoadEXIFRecord_ProgressReportModule> progress)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<ImageExif> ImageExifs = new List<ImageExif>();
            foreach (MatchItem_Class item in matches_input)
            {
                ImageExif exif = await LoadImageInfo_Async(item);
                ImageExifs.Add(exif);
                LoadEXIFRecord_ProgressReportModule CurrentProgress = new LoadEXIFRecord_ProgressReportModule()
                {
                    CurrentItem = ImageExifs.Count,
                    EXIFData = exif,
                    TotalItems = matches_input.Count - 1,
                };
                progress.Report(CurrentProgress);
            }
            stopwatch.Stop();
            Console.WriteLine(String.Format("Total timespan on loading {0}", stopwatch.Elapsed.TotalSeconds));
            return ImageExifs;
        }
        public static async Task<List<ImageExif>> SortResult(List<ImageExif> inputs, SortManner sortManner, SortTypes sortType)
        {
            List<ImageExif> sorted = new List<ImageExif>();
            switch (sortType)
            {
                case SortTypes.DateTaken:
                    await Task.Run(() => inputs.Sort(SortByDate));
                    if (sortManner == SortManner.Descending)
                    {
                        inputs.Reverse();
                    }
                    break;
                case SortTypes.FileName:
                    await Task.Run(() => inputs.Sort(SortByFileName));
                    if (sortManner == SortManner.Descending)
                    {
                        inputs.Reverse();
                    }
                    break;
                default:
                    break;
            }
            sorted = inputs;
            return inputs;
        }
        private static int SortByDate(ImageExif x, ImageExif y)
        {
            if (x.JsonData.PhotoTakenTime_DateTime == null)
            {
                if (y.JsonData.PhotoTakenTime_DateTime == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (y.JsonData.PhotoTakenTime_DateTime == null)
                {
                    return -1;
                }
                else
                {
                    return x.JsonData.PhotoTakenTime_DateTime.CompareTo(y.JsonData.PhotoTakenTime_DateTime);
                }
            }
        }
        private static int SortByFileName(ImageExif x, ImageExif y)
        {
            int retval = 0;
            try
            {
                retval = x.EXIFData.ImagePath.CompareTo(y.EXIFData.ImagePath);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "錯誤");
            }
            return retval;
        }
        public static List<ImageExif> SearchByPhotoTakenTime(List<ImageExif> imageExifs, Dictionary<DateTypes, int> searchDate)
        {
            IEnumerable<ImageExif> exif_query = imageExifs;
            if (searchDate[DateTypes.Year] != 0)
            {
                exif_query = exif_query.Where(item => item.JsonData.PhotoTakenTime_DateTime.Year == searchDate[DateTypes.Year]);
            }
            if (searchDate[DateTypes.Month] != 0)
            {
                exif_query = exif_query.Where(item => item.JsonData.PhotoTakenTime_DateTime.Month == searchDate[DateTypes.Month]);
            }
            if (searchDate[DateTypes.Day] != 0)
            {
                exif_query = exif_query.Where(item => item.JsonData.PhotoTakenTime_DateTime.Day == searchDate[DateTypes.Day]);
            }
            return exif_query.ToList();
        }
        public static List<ImageExif> SearchByFileName(List<ImageExif> imageExifs, string filename)
        {
            IEnumerable<ImageExif> query = imageExifs;
            if (filename != "")
            {
                query = query.Where(item => item.EXIFData.ImagePath.Contains(filename));
            }
            //Console.WriteLine("Result count " + query.Count());
            return query.ToList();
        }
        public static Dictionary<string, double> String_To_GPSCoordinatesDictionary(string input)
        {
            string[] seperator = { "), (" };
            string[] inputs = input
                .Split(seperator, StringSplitOptions.RemoveEmptyEntries)
                .Select(item => item
                .Replace("(", "")
                .Replace(")", "")
                .Trim()
                )
                .Select(item => item.Substring(0, item.IndexOf(",")))
                .ToArray();
            var convert = new Tuple<double, double, double>(0, 0, 0);
            string[] dms = new string[] { "d", "m", "s" };
            Dictionary<string, double> dictionary = dms.Zip(inputs, (k, v) => new { k, v }).ToDictionary(x => x.k, x => double.Parse(x.v));
            return dictionary;
        }

        public static string GPSDictionary_To_String(Dictionary<string, double> input)
        {
            string output = string.Join(";", input.Select(x => x.Value)) == ApplicationVariables.NoData ? "" : String.Join(";", input.Select(x => x.Value));
            if (input.Count() == 0)
            {
                output = ApplicationVariables.NoData;
            }
            return output;
        }

        /// <summary>
        /// Converts the decimal gps coordinate to dictionary with key: d, m, s
        /// </summary>
        /// <param name="input_decimal"></param>
        /// <returns></returns>
        public static Dictionary<string, double> Double_To_DMS(double input_decimal)
        {
            Dictionary<string, double> output = new Dictionary<string, double>();
            output.Add("d", Math.Floor(input_decimal));
            output.Add("m", Math.Floor((input_decimal - Math.Floor(input_decimal)) * 60.0));
            output.Add("s", Math.Round((((input_decimal - Math.Floor(input_decimal)) * 60.0 - output["m"])) * 60.0, 2));

            return output;
        }
        public static double DMS_To_Double(Dictionary<string, double> input)
        {
            double output = input["d"] + input["m"]  / 60 + input["s"] / 3600;
            return output;
        }
    }
    public enum DateTypes
    {
        Year,
        Month,
        Day,
    }
    public enum SortTypes
    {
        FileName,
        DateTaken
    }
    public enum SortManner
    {
        Ascending,
        Descending
    }
}
