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
        public IDisplayInterface BaseDisplayInterface;
        public List<ImageExif> Exifs = new List<ImageExif>();
        List<MatchItem_Class> Matches = new List<MatchItem_Class>();
        private CancellationTokenSource cts = new CancellationTokenSource();
        public DisplayPanel()
        {
            InitializeComponent();
            Loaded += DisplayPanel_Loaded;
        }
        public DisplayPanel(List<MatchItem_Class> matches)
        {
            Matches = matches;
            InitializeComponent();
            Loaded += DisplayPanel_Loaded;
        }

        private async void DisplayPanel_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDisplayBase(DisplayManner.Display_Type);
            Progress<LoadEXIFRecord_ProgressReportModule> exif_progress = new Progress<LoadEXIFRecord_ProgressReportModule>();
            Exifs = await ImageExif_Functions.Load_Image_EXIF_Record(Matches, cts.Token, exif_progress);
            BaseDisplayInterface.ShowPage((int)Properties.Settings.Default["page_number"], Exifs);
        }
        private void DisplayManner_DisplayTypeChanged(object sender, TypeOfDisplay e)
        {
            LoadDisplayBase(e);
            BaseDisplayInterface.ShowPage(0, Exifs);
        }
        private void LoadDisplayBase(TypeOfDisplay obj)
        {
            switch (obj)
            {
                case TypeOfDisplay.ListView:
                    BaseDisplayInterface = new Display_List();
                    break;
                case TypeOfDisplay.ImageView:
                    BaseDisplayInterface = new Display_Icon();
                    break;
                default:
                    break;
            }
            BaseDisplayInterface.InitializePage(Viewer);
        }

    }
}
