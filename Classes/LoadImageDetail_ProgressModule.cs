using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Google_Drive_Organizer_V3.Pages.MatchItem
{
    public class LoadImageDetail_ProgressModule
    {
        public ImageExif_Class EXIFFromJsonFile { get; set; } = new ImageExif_Class();
        public Task AddToRecordCompleted;

    }
}
