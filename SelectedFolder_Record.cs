using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Google_Drive_Organizer_V3
{
    public static class SelectedFolder_Record
    {
        public static List<FolderRecordItem> Folders = new List<FolderRecordItem>();
    }
    public class FolderRecordItem
    {
        public FolderRecordItem(string folderlocation, string MD5Hash, int json_count, int image_count)
        {
            FolderLocation = folderlocation;
            MD5 = MD5Hash;
            JsonCount = json_count;
            ImageCount = image_count;
        }
        public string FolderLocation { get; set; }
        public string MD5 { get; set; }
        public int JsonCount { get; set; }
        public int ImageCount { get; set; }
    }
}
