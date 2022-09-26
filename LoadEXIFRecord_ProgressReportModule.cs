using Google_Drive_Organizer_V3.Pages.MatchItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Google_Drive_Organizer_V3
{
    public class LoadEXIFRecord_ProgressReportModule
    {
        public ImageExif_Class EXIFData { get; set; }
        public int CurrentItem { get; set; }
        public int TotalItems { get; set; }
    }
}
