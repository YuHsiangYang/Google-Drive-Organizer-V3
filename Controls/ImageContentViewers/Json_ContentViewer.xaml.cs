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
    /// Json_ContentViewer.xaml 的互動邏輯
    /// </summary>
    public partial class Json_ContentViewer : UserControl
    {
        public Json_ContentViewer()
        {
            InitializeComponent();
        }
        public System.Windows.Controls.Button Apply { get; set; } = new System.Windows.Controls.Button()
        {
            Content = "Apply"
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

        public void DisplayJSONData(ImageJsonData jsonData)
        {
            //Dispaly the phototaken time.
            PhotoTakenTimeDate.Content = jsonData.PhotoTakenTime_DateTime.ToString();

            //Display the GPS cordinates
            Longitude.Content = ImageInfo_Functions.GPSDictionary_To_String(jsonData.GPS_Longitude);
            Latitude.Content = ImageInfo_Functions.GPSDictionary_To_String(jsonData.GPS_Latitude);
            Altitude.Content = jsonData.GPS_Altitude;

            JsonName.Content = System.IO.Path.GetFileName(jsonData.JsonPath);
        }
    }

}
