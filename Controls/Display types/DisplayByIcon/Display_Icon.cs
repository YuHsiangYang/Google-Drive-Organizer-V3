using Google_Drive_Organizer_V3.Pages.MatchItem;
using Google_Drive_Organizer_V3.Pages.MatchItem.Display_types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Google_Drive_Organizer_V3.Classes.Display_types
{
    public class Display_Icon : IDisplayInterface
    {
        public string PanelName { get; } = "DisplayPanel";
        public object Panel { get; private set; }

        public void InitializePage(ScrollViewer scrollViewer)
        {
            WrapPanel DisplayWrapPanel = new WrapPanel()
            {
                Name = PanelName,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
            };
            Panel = DisplayWrapPanel;
            scrollViewer.Content = Panel;
        }

        public void ShowPage(int page, List<ImageExif> input_items)
        {
            ((WrapPanel)Panel).Children.Clear();
            List<ImageExif> selected_range = new List<ImageExif>();
            try
            {
                selected_range = input_items.GetRange(page * ApplicationVariables.PageSize, ApplicationVariables.PageSize);
            }
            catch (ArgumentException)
            {
                selected_range = input_items.GetRange(page * ApplicationVariables.PageSize, input_items.Count - page * ApplicationVariables.PageSize);
            }
            foreach (ImageExif children_item in selected_range)
            {
                ((WrapPanel)Panel).Children.Add(new Match_Display_Icon(children_item));
            }
        }
    }
}
