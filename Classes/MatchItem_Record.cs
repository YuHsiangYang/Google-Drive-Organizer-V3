using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Google_Drive_Organizer_V3.Pages.MatchItem;
using System.Diagnostics;
using Google_Drive_Organizer_V3.Pages.SelectFolder;

namespace Google_Drive_Organizer_V3
{
    public static class MatchItem_Record
    {
        public static List<MatchItem_Class> Matched_Items { get; set; } = new List<MatchItem_Class>();
        public static async Task<List<MatchItem_Class>> LoadMatch(List<Folder_Item> folder_locations, IProgress<LoadEXIFRecord_ProgressReportModule> progression)
        {
            List<MatchItem_Class> Matches = new List<MatchItem_Class>();
            try
            {
                List<string> Jsons_FullPath = new List<string>();
                List<string> Images_FullPath = new List<string>();
                List<string> Jsons_Name = new List<string>();
                List<string> Images_Name = new List<string>();
                foreach (var item in folder_locations)
                {
                    Jsons_FullPath.AddRange(await Task.Run(() => Directory.GetFiles(item.FolderPath, "*.json", SearchOption.AllDirectories).ToList()));
                    Images_FullPath.AddRange(await Task.Run(() => Directory.GetFiles(item.FolderPath, "*.*", SearchOption.AllDirectories).Where(file => file.EndsWith(".jpeg") || file.EndsWith(".jpg")).ToList()));
                }
                Jsons_FullPath.ForEach(item =>
                Jsons_Name.Add(Path.GetFileName(item))
                );
                Images_FullPath.ForEach(item => Images_Name.Add(Path.GetFileName(item)));
                List<Task> match_process_parallel = new List<Task>();
                Action<object> find_name_action = delegate(object input){
                    string image = Images_FullPath.Find(x => x.Contains(input.ToString().Substring(0, input.ToString().Length - 5)));//substring the json to image
                    if (image != null)
                    {
                        MatchItem_Class matched = new MatchItem_Class(image, Jsons_FullPath.Find(x => x.Contains(input.ToString())));
                        Matches.Add(matched);
                        LoadEXIFRecord_ProgressReportModule progress_report = new LoadEXIFRecord_ProgressReportModule()
                        {
                            CurrentItem = Matches.Count,
                            TotalItems = Jsons_FullPath.Count
                        };
                        progression.Report(progress_report);
                        //Console.WriteLine(image);
                    }
                };
                await Task.Run(() => Parallel.ForEach<string>(Jsons_Name, find_name_action));
                Matches = Matches.Distinct<MatchItem_Class>().ToList();
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("From MatchItem_Record");
            }
            return Matches;
        }
    }
    public class MatchItem_Class
    {
        public MatchItem_Class(string image_location, string json_location)
        {
            ImagePath = image_location;
            JsonPath = json_location;
        }
        public string ImagePath { get; set; }
        public string JsonPath { get; private set; }
    }
}
