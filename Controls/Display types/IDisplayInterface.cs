using Google_Drive_Organizer_V3.Pages.MatchItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Google_Drive_Organizer_V3.Classes
{
    /// <summary>
    /// This interface serves as the standard template to display the images in different forms (list and icon)
    /// </summary>
    public interface IDisplayInterface
    {
        string PanelName { get;}
        object Panel { get;}
        void ShowPage(int page, List<ImageExif> input_items);
        void InitializePage(System.Windows.Controls.ScrollViewer scrollViewer);
    }
}
