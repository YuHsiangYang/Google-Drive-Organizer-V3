using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Google_Drive_Organizer_V3.Classes
{
    public static class ImageExif_Functions
    {
        static ProcessStartInfo start_info = new ProcessStartInfo
        {
            FileName = System.IO.Path.GetFullPath(@"C:\Users\yuhsi\Home\Drive\Yu-Hsiang Folder\Self written Application\Google Drive Organizer V3\Scripts\Read_EXIF.exe"),
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            CreateNoWindow = true,
        };

        public static async Task<ImageExif> Load_Image_EXIF_Async(MatchItem_Class input_item, CancellationToken cancellationToken)
        {
            ImageExif detail = new ImageExif();

            try
            {
                await Task.Run(async () =>
                    {
                        //if (cancellationToken.IsCancellationRequested)
                        //{
                        //    detail = new ImageExif();
                        //    cancellationToken.ThrowIfCancellationRequested();
                        //}
                        start_info.Arguments = string.Format("\"{0}\" \"{1}\"", input_item.Image_Location, ApplicationVariables.NoData);
                        using (Process process = Process.Start(start_info))
                        {
                            using (StreamReader read = process.StandardOutput)
                            {

                                string[] result = read.ReadToEnd().ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                                detail.ImagePath = input_item.Image_Location;
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
            catch (OperationCanceledException)
            {
                Console.WriteLine("LoadImageDetail cancelled");
            }
            return detail;
        }
        
        public static async Task<List<ImageExif>> Load_Image_EXIF_Record(List<MatchItem_Class> matches_input, CancellationToken ct, IProgress<LoadEXIFRecord_ProgressReportModule> progress)
        {

            List<ImageExif> ImageExif = new List<ImageExif>();
            //Action<object> get_exif_individual = async delegate (object input)
            //{
            //    var exif = await Load_Image_EXIF_Async(input as MatchItem_Class, ct);
            //    ImageExif.Add(exif);
            //    LoadEXIFRecord_ProgressReportModule progression = new LoadEXIFRecord_ProgressReportModule()
            //    {
            //        TotalItems = matches_input.Count(),
            //        CurrentItem = ImageExif.Count(),
            //        EXIFData = exif
            //    };
            //    progress.Report(progression);
            //};
            //await Task.Run(() => Parallel.ForEach(matches_input, item => get_exif_individual(item)));
            foreach (MatchItem_Class item in matches_input)
            {
                ImageExif.Add(await Load_Image_EXIF_Async(item, ct));
            }
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


            //foreach (ImageExif item in ImageExif)
            //{
            //    ImageExif_Record.Images.Add(item);
            //}
            return ImageExif;
        }
        public static async Task<List<ImageExif>> SortResult(List<ImageExif> inputs, SortManner sortManner, SortType sortType)
        {
            List<ImageExif> sorted = new List<ImageExif>();
            switch (sortType)
            {
                case SortType.拍攝日期:
                    await Task.Run(() => inputs.Sort(SortByDate));
                    if (sortManner == SortManner.遞減)
                    {
                        inputs.Reverse();
                    }
                    break;
                case SortType.檔案名稱:
                    await Task.Run(() => inputs.Sort(SortByFileName));
                    if (sortManner == SortManner.遞減)
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
            if (x.Json_PhotoTakenTime_DateTime == null)
            {
                if (y.Json_PhotoTakenTime_DateTime == null)
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
                if (y.Json_PhotoTakenTime_DateTime == null)
                {
                    return -1;
                }
                else
                {
                    return x.Json_PhotoTakenTime_DateTime.CompareTo(y.Json_PhotoTakenTime_DateTime);
                }
            }
        }
        private static int SortByFileName(ImageExif x, ImageExif y)
        {
            int retval = 0;
            try
            {
                retval = x.ImagePath.CompareTo(y.ImagePath);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "錯誤");
            }
            return retval;
        }
    }
    public enum SortType
    {
        檔案名稱,
        拍攝日期
    }
    public enum SortManner
    {
        遞增,
        遞減
    }
}
