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

        public void DisplayEXIF(ImageEXIFData exif)
        {
            //Set the content of EXIF viewer
            Longitude.Content = ImageInfo_Functions.GPSDictionary_To_String(exif.GPS_Longitude);
            Latitude.Content = ImageInfo_Functions.GPSDictionary_To_String(exif.GPS_Latitude);
            Altitude.Content = exif.GPS_Altitude == "" ? ApplicationVariables.NoData : exif.GPS_Altitude;
            ImageName.Content = System.IO.Path.GetFileName(exif.ImagePath); // Get the name and set the image name from the path
        }
    }
}
