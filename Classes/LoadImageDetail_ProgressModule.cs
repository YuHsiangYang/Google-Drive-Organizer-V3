using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Google_Drive_Organizer_V3.Classes
{
    public class LoadImageDetail_ProgressModule
    {
        public ImageExif EXIFFromJsonFile { get; set; } = new ImageExif();
        public Task AddToRecordCompleted;

    }
}
