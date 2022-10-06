using Google_Drive_Organizer_V3.Pages.MatchItem;
using Google_Drive_Organizer_V3.Pages.MatchItem.Display_types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Google_Drive_Organizer_V3.Classes.Display_types
{
    class Display_Icon : IDisplayInterface
    {
        public void ShowPage(int page)
        {
            foreach (ImageExif_Class item in History.MatchedItem.ItemsForDisplay)
            {
                History.MatchedItem.Matched_Item_Stackpanel.Children.Add(new Match_Display_Icon(item)
                {
                    Height = 130,
                    Width = 120,
                });
            }
        }
    }
}
