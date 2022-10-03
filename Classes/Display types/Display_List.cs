using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Google_Drive_Organizer_V3.Pages.MatchItem;

namespace Google_Drive_Organizer_V3.Classes.Display_types
{
    internal class Display_List : IDisplayInterface
    {
        public void ShowPage(int page)
        {
            List<MatchItem_Child> children = new List<MatchItem_Child>();
            int from = (page - 1) * 30;
            int to = page * 30;
            for (int i = from; i < to; i++)
            {
                try
                {
                    children.Add(new MatchItem_Child(History.MatchedItem.ItemsForDisplay[i]));

                }
                catch (Exception)
                {
                    Console.WriteLine("items not enough");
                }
            }
            //foreach (ImageExif_Class item in ItemsForDisplay)
            //{
            //    children.Add(new MatchItem_Child(item)); 
            //}
            History.MatchedItem.Matched_Item_Stackpanel.Children.Clear();
            foreach (var item in children)
            {
                if (History.MatchedItem.Matched_Item_Stackpanel.Children.IndexOf(item) == -1)
                {
                    History.MatchedItem.Matched_Item_Stackpanel.Children.Add(item);
                    UniversalFunctions.Appear_Element(item, new Duration(TimeSpan.FromSeconds(0.15)));
                }
            }
            if (History.MatchedItem.ItemsForDisplay.Count == 0)
            {
                History.MatchedItem.NoItemFounded();
            }
            History.MatchedItem.Pages.LoadRange(History.MatchedItem.ItemsForDisplay.Count);
        }
    }
}
