using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Google_Drive_Organizer_V3
{
    public static class History
    {
        //This class is mainly use for staging purposes. For recording the current state of the application.
        public static Controls.DisplayPanel DisplayMatchPanel = new Controls.DisplayPanel();
        public static Pages.SelectFolderPage SelectFolder = new Pages.SelectFolderPage();
        public static Pages.SelectDistination.SelectOutputDistination OutputDistination = new Pages.SelectDistination.SelectOutputDistination();
    }
}
