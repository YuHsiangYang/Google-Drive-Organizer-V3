using Google_Drive_Organizer_V3.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Google_Drive_Organizer_V3.Controls
{
    /// <summary>
    /// SearchType_Control.xaml 的互動邏輯
    /// </summary>
    public partial class SearchControl : UserControl
    {
        private Search_By_FileName Search_By_FileName { get; set; } = new Search_By_FileName();
        private Search_By_PhotoTakenTime Search_By_PhotoTakenTime { get; set; } = new Search_By_PhotoTakenTime();

        public event EventHandler<Dictionary<DateTypes, int>> SearchDate_Event;
        public event EventHandler<string> SearchFileName_Event;

        public object CurrentFilter;


        private enum SearchTypes
        {
            FileName,
            DateTaken,
        }
        public SearchControl()
        {
            InitializeComponent();
            Search_By_FileName.SearchFileName += Search_By_FileName_SearchFileName; ;
            Search_By_PhotoTakenTime.SelectedDateChanged += Search_By_PhotoTakenTime_SelectedDateChanged; ;
            SearchType.ItemsSource = Enum.GetValues(typeof(SearchTypes));
            SearchType.SelectedIndex = 0;
        }

        private void Search_By_FileName_SearchFileName(object sender, string e)
        {
            CurrentFilter = e;
            SearchFileName_Event?.Invoke(sender, e);
        }
        private void Search_By_PhotoTakenTime_SelectedDateChanged(object sender, Dictionary<DateTypes, int> e)
        {
            CurrentFilter = e;
            SearchDate_Event?.Invoke(sender, e);
        }

        public void LoadSelectionDates(List<ImageExif> exifs)
        {
            Search_By_PhotoTakenTime.LoadPhotoTakenTimeComboboxItem(exifs);
        }

        private void SearchType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadSearchControl(SearchType.SelectedValue);
        }

        private async void LoadSearchControl(object selected_type)
        {
            SearchTypes selected = (SearchTypes)Enum.Parse(typeof(SearchTypes), selected_type.ToString());
            TimeSpan AnimationDuration = TimeSpan.FromSeconds(.2);
            switch (selected)
            {
                case SearchTypes.DateTaken:
                    await SwitchElement(Search_By_FileName, Search_By_PhotoTakenTime, AnimationDuration);
                    break;
                case SearchTypes.FileName:
                    await SwitchElement(Search_By_PhotoTakenTime, Search_By_FileName, AnimationDuration);
                    break;
                default:
                    break;
            }
        }
        private async Task SwitchElement(UIElement from, UIElement to, TimeSpan duration)
        {
            GlobalScripts.Disappear_Element(from, duration);
            await Task.Delay(duration);
            SearchControlGrid.Children.Clear();
            SearchControlGrid.Children.Add(to);
            GlobalScripts.Appear_Element(to, duration);
        }
    }
}
