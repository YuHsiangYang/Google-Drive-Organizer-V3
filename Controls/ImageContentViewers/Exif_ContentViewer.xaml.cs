using Google_Drive_Organizer_V3.Classes;
using Google_Drive_Organizer_V3.Pages.MatchItem.Display_types;
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

namespace Google_Drive_Organizer_V3.Controls.ImageContentViewers
{
    /// <summary>
    /// Exif_ContentViewer.xaml 的互動邏輯
    /// </summary>
    public partial class Exif_ContentViewer : UserControl
    {
        public Exif_ContentViewer()
        {
            InitializeComponent();
        }
        public System.Windows.Controls.Button Apply { get; set; } = new System.Windows.Controls.Button()
        {
            Content = "套用"
        };
        private void CalanderControl_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!CalanderPopup.IsOpen)
            {
                CalanderPopup.IsOpen = true;
            }
            else
            {
                CalanderPopup.IsOpen = false;
            }

            PhotoTakenTimeDate.Content = CalanderControl.SelectedDate;
            if (PhotoTakenTimeStackPanel.Children.IndexOf(Apply) == -1)
            {
                PhotoTakenTimeStackPanel.Children.Insert(2, Apply);
                GlobalScripts.Appear_Element(Apply, TimeSpan.FromSeconds(.2));
            }
        }

        private void PhotoTakenTime_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!CalanderPopup.IsOpen)
            {
                CalanderPopup.IsOpen = true;
            }
            else
            {
                CalanderPopup.IsOpen = false;
            }
        }
        /// <summary>
        /// Use Match_Display_Icon class as input.
        /// Display the data by using the EXIF data stored in the record.
        /// </summary>
        /// <param name="sender"></param>

        public void DisplayEXIF(object sender)
        {
            try
            {
                //Set the content of EXIF viewer
                ImageExif exif = (sender as Match_Display_Icon).exif;
                Longitude.Content = ImageInfo_Functions.GPSDictionary_To_String(exif.EXIFData.GPS_Longitude);
                Latitude.Content = ImageInfo_Functions.GPSDictionary_To_String(exif.EXIFData.GPS_Latitude);
                Altitude.Content = exif.EXIFData.GPS_Altitude == "" ? ApplicationVariables.NoData : exif.EXIFData.GPS_Altitude;
                ImageName.Content = System.IO.Path.GetFileName(exif.EXIFData.ImagePath); // Get the name and set the image name from the path
                CameraManufactor.Content = exif.EXIFData.CameralModel;
                Artist.Content = exif.EXIFData.Artist;
                UserComment.Content = exif.EXIFData.UserComment;
            }
            catch (Exception)
            {
                Console.WriteLine("Error while converting sender to Match_Display_Icon");
            }
        }
    }
}
