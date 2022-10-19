using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Google_Drive_Organizer_V3.Controls.Display_types.DisplayByList;
using Google_Drive_Organizer_V3.Pages.MatchItem;

namespace Google_Drive_Organizer_V3.Classes.Display_types
{
    public  class Display_List : IDisplayInterface
    {
        public string PanelName { get; } = "DisplayStackPanel";

        public object Panel { get; private set; }

        public void InitializePage(ScrollViewer scrollViewer)
        {
            StackPanel display_panel = new StackPanel()
            {
                Name = PanelName,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            Panel = display_panel;
            scrollViewer.Content = Panel;
        }

        public void ShowPage(int page, List<ImageExif> input)
        {
            List<Match_Display_List> children = new List<Match_Display_List>();
            int from = (page - 1) * 30;
            int to = page * 30;
            foreach (ImageExif item in input)
            {
                children.Add(new Match_Display_List(item)
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch
                });
            }
            //foreach (ImageExif_Class item in ItemsForDisplay)
            //{
            //    children.Add(new MatchItem_Child(item)); 
            //}
            ((StackPanel)Panel).Children.Clear();
            foreach (var item in children)
            {
                if (((StackPanel)Panel).Children.IndexOf(item) == -1)
                {
                    ((StackPanel)Panel).Children.Add(item);
                    UniversalFunctions.Appear_Element(item, new Duration(TimeSpan.FromSeconds(0.15)));
                }
            }
            //if (input.Count == 0)
            //{
            //    NoItemFounded();
            //}
            //History.MatchedItem.Pages.LoadRange(History.MatchedItem.ItemsForDisplay.Count);
        }
    }
}
