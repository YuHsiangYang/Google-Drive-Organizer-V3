using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Google_Drive_Organizer_V3.Pages.MatchItem
{
    public static class ImageExif_Function
    {
        static ProcessStartInfo start_info = new ProcessStartInfo
        {
            FileName = @".\Read_EXIF_PY.exe",
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            CreateNoWindow = true,
        };

        public static async Task<ImageExif_Class> Load_Image_EXIF_Async(MatchItem_Class input_item, CancellationToken cancellationToken)
        {
            ImageExif_Class detail = new ImageExif_Class();

            try
            {
                await Task.Run(async () =>
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            detail = new ImageExif_Class();
                            cancellationToken.ThrowIfCancellationRequested();
                        }
                        start_info.Arguments = string.Format("\"{0}\" \"{1}\"", input_item.Image_Location, Properties.Settings.Default["No_Data_Message"]);
                        using (Process process = Process.Start(start_info))
                        {
                            using (StreamReader read = process.StandardOutput)
                            {

                                string[] result = read.ReadToEnd().ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                                detail.Image_Location = input_item.Image_Location;
                                detail.phototakentime = result[0];
                                detail.CameraManufactor = result[1];
                                detail.GPS_Longitude = result[3];
                                detail.GPS_Latitude = result[5];
                                detail.GPS_Altitude = result[6];
                                //detail.MD5 = UniversalFunctions.CalculateMD5(MD5Type.File, image_location);
                            }
                        }
                        Console.WriteLine(input_item.Json_Location);
                        string json_text = await Task.Run(() => File.ReadAllText(input_item.Json_Location));
                        JObject json = await Task.Run(() => JObject.Parse(json_text));
                        detail.Json_GPS_Longitude = json["geoDataExif"]["longitude"].ToString();
                        detail.Json_GPS_Latitude = json["geoDataExif"]["latitude"].ToString();
                        detail.Json_GPS_Altitude = json["geoDataExif"]["altitude"].ToString();
                        detail.Json_PhotoTakenTime = json["photoTakenTime"]["timestamp"].ToString();
                        detail.Json_Location = input_item.Json_Location;
                    });
            }
            catch (OperationCanceledException OCE)
            {
                Console.WriteLine("LoadImageDetail cancelled");
            }
            return detail;
        }
        public static async Task Load_Image_EXIF_Record(CancellationToken ct, IProgress<LoadEXIFRecord_ProgressReportModule> progress)
        {
            List<Task<ImageExif_Class>> GetEXIF = new List<Task<ImageExif_Class>>();
            List<ImageExif_Class> ImageExif = new List<ImageExif_Class>();
            Action<object> get_exif_individual = async delegate (object input)
            {
                var exif = await Load_Image_EXIF_Async(input as MatchItem_Class, ct);
                ImageExif.Add(exif);
                LoadEXIFRecord_ProgressReportModule progression = new LoadEXIFRecord_ProgressReportModule()
                {
                    TotalItems = MatchItem_Record.Matched_Items.Count(),
                    CurrentItem = ImageExif.Count(),
                    EXIFData = exif
                };
                progress.Report(progression);
            };
            await Task.Run(() => Parallel.ForEach(MatchItem_Record.Matched_Items, item => get_exif_individual(item)));
            //foreach (MatchItem_Class item in MatchItem_Record.Matched_Items)
            //{

            //    ImageExif.Add(await Load_Image_EXIF_Async(item, ct));
            //    LoadEXIFRecord_ProgressReportModule progression = new LoadEXIFRecord_ProgressReportModule()
            //    {
            //        TotalItems = MatchItem_Record.Matched_Items.Count(),
            //        CurrentItem = MatchItem_Record.Matched_Items.ToList().IndexOf(item),
            //    };
            //    progress.Report(progression);
            //}
            //var result = await Task.WhenAll(GetEXIF);


            foreach (ImageExif_Class item in ImageExif)
            {
                ImageExif_Record.Images.Add(item);
            }
        }
    }
}
