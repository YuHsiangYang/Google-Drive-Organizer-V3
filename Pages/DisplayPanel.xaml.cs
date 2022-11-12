using Google_Drive_Organizer_V3.Classes;
using Google_Drive_Organizer_V3.Classes.Display_types;
using Google_Drive_Organizer_V3.Pages.MatchItem;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<ImageExif> Exifs = new List<ImageExif>();
        List<MatchItem_Class> Matches = new List<MatchItem_Class>();
        private CancellationTokenSource cts = new CancellationTokenSource();
        public DisplayPanel()
        {
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
            Exifs = await ImageInfo_Functions.LoadImageInfo_Record(Matches, cts.Token, exif_progress);

            GlobalScripts.Disappear_Element(Loading_Progress, TimeSpan.FromSeconds(.2));
            await Task.Delay(TimeSpan.FromSeconds(.2));
            DisplayInterface = LoadDisplayBase(TypeOfDisplay.ImageView);
            DisplayInterface.InitializePage(Viewer);
            DisplayInterface.ShowPage(ApplicationVariables.PageNumber, Exifs);



            //Add event listener to the "Search" element
            Search.SearchDate_Event += Search_SearchDate_Event;
            Search.SearchFileName_Event += Search_SearchFileName_Event;
        }

        private void Search_SearchFileName_Event(object sender, string e)
        {
            List<ImageExif> images = ImageInfo_Functions.SearchByFileName(Exifs, e);
            DisplayInterface.ShowPage(ApplicationVariables.PageNumber, images);
        }

        private void Search_SearchDate_Event(object sender, Dictionary<DateTypes, int> e)
        {
            DisplayInterface.ShowPage(ApplicationVariables.PageNumber, ImageInfo_Functions.SearchByPhotoTakenTime(Exifs, e));
        }

        private void ExifLoading_Progress(object sender, LoadEXIFRecord_ProgressReportModule e)
        {
            Loading_Progress.percentage = (double)((double)e.CurrentItem / (double)e.TotalItems);
        }

        public DisplayPanel(List<MatchItem_Class> matches)
        {
            Matches = matches;
            Initialized += DisplayPanel_Initialized;
            InitializeComponent();
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
    }
}
