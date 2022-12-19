using Google_Drive_Organizer_V3.Classes;
using Google_Drive_Organizer_V3.Classes.Display_types;
using Google_Drive_Organizer_V3.Pages.MatchItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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
    /// DisplayPanel.xaml 的互動邏輯
    /// </summary>
    public partial class DisplayPanel : UserControl
    {
        public IDisplayInterface DisplayInterface;
        public List<ImageExif> CompleteExifs = new List<ImageExif>();
        private List<ImageExif> PartialExifs = new List<ImageExif>();
        private List<ImageExif> FilteredExifs = new List<ImageExif>();
        List<MatchItem_Class> Matches = new List<MatchItem_Class>();
        public event EventHandler<List<ImageExif>> StageFinished;
        public DisplayPanel()
        {
            Initialized += DisplayPanel_Initialized;
            InitializeComponent();
        }

        public DisplayPanel(List<MatchItem_Class> matches)
        {
            Matches = matches;
            Initialized += DisplayPanel_Initialized;
            InitializeComponent();
        }

        private async void DisplayPanel_Initialized(object sender, EventArgs e)
        {

            //load the width from setting
            if ((double)Properties.Settings.Default["EXIF_Width"] != EXIFViewer.DesiredSize.Width)
            {
                EXIFViewer_Column.Width = new GridLength((double)Properties.Settings.Default["EXIF_Width"], GridUnitType.Pixel);
            }
            Progress<LoadEXIFRecord_ProgressReportModule> exif_progress = new Progress<LoadEXIFRecord_ProgressReportModule>();
            exif_progress.ProgressChanged += ExifLoading_Progress;

            //Add event listener to the "Search" element
            Search.SearchDate_Event += Search_SearchDate_Event;
            Search.SearchFileName_Event += Search_SearchFileName_Event;

            //Add event listener to sort type
            SortController.SortChanged += SortController_SortChanged;
            DisplayInterface = LoadDisplayBase(TypeOfDisplay.ImageView);
            DisplayInterface.InitializePage(Viewer);

            CompleteExifs = await ImageInfo_Functions.LoadImageInfo_Record(Matches, exif_progress);
            
        }

        private async void SortController_SortChanged(object sender, SortController.Sort e)
        {
            FilteredExifs = await ImageInfo_Functions.SortResult(FilteredExifs, e.SortManner, e.SortType);
            DisplayInterface.ShowPage(ApplicationVariables.PageNumber, FilteredExifs);
        }

        private void Search_SearchFileName_Event(object sender, string e)
        {
            FilteredExifs = ImageInfo_Functions.SearchByFileName(PartialExifs, e);
            DisplayInterface.ShowPage(ApplicationVariables.PageNumber, FilteredExifs);
        }

        private void Search_SearchDate_Event(object sender, Dictionary<DateTypes, int> e)
        {
            FilteredExifs = ImageInfo_Functions.SearchByPhotoTakenTime(PartialExifs, e);
            DisplayInterface.ShowPage(ApplicationVariables.PageNumber, FilteredExifs);
        }

        private void ExifLoading_Progress(object sender, LoadEXIFRecord_ProgressReportModule e)
        {
            Loading_Progress.percentage = (double)((double)e.CurrentItem / (double)e.TotalItems);
            PartialExifs.Add(e.EXIFData);
            FilteredExifs = PartialExifs;
            Search.LoadSelectionDates(PartialExifs);

            if (Search.CurrentFilter != null)
            {
                try
                {
                    Dictionary<DateTypes, int> date = (Dictionary<DateTypes, int>)Search.CurrentFilter;
                    FilteredExifs = ImageInfo_Functions.SearchByPhotoTakenTime(PartialExifs, date);
                }
                catch (Exception)
                {
                    string filename = Search.CurrentFilter.ToString();
                    FilteredExifs = ImageInfo_Functions.SearchByFileName(PartialExifs, filename);
                }
            }

           
            DisplayInterface.ShowPage(ApplicationVariables.PageNumber, FilteredExifs);
            //Load the pages
            PageNavigation.LoadRange(FilteredExifs.Count());
        }
        private IDisplayInterface LoadDisplayBase(TypeOfDisplay obj)
        {
            IDisplayInterface display = new Display_List();
            switch (obj)
            {
                case TypeOfDisplay.ListView:
                    display = new Display_List();
                    break;
                case TypeOfDisplay.ImageView:
                    display = new Display_Icon();
                    break;
                default:
                    break;
            }
            return display;
        }

        private void EXIFWidthSplitter_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            Properties.Settings.Default["EXIF_Width"] = EXIFViewer.RenderSize.Width;
            Properties.Settings.Default.Save();
        }

        private void NextStep_Btn_Click(object sender, RoutedEventArgs e)
        {
            StageFinished?.Invoke(this, CompleteExifs);
        }

        private void PageNavigation_PageChanged(object sender, int e)
        {
            DisplayInterface.ShowPage(ApplicationVariables.PageNumber, FilteredExifs);
        }
    }
}
