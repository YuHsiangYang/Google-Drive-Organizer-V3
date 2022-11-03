using Google_Drive_Organizer_V3.Classes;
using Google_Drive_Organizer_V3.Pages.MatchItem.Display_types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MessageBox = System.Windows.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace Google_Drive_Organizer_V3.Controls
{
    /// <summary>
    /// EXIFDataViewer.xaml 的互動邏輯
    /// </summary>
    public partial class EXIFDataViewer : System.Windows.Controls.UserControl
    {
        public EXIFDataViewer()
        {
            Initialized += EXIFDataViewer_Initialized;
            InitializeComponent();
        }

        private void EXIFDataViewer_Initialized(object sender, EventArgs e)
        {
            //Load the previous height Set by the user
            Row_Height.Height = new GridLength(double.Parse(Properties.Settings.Default["EXIF_Height"].ToString()), GridUnitType.Pixel);

            //add event listener to the click event of the apply button
            Apply.Click += Apply_Click;
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            caller.exif.PhotoTakenTime_DateTime_Json = (DateTime)CalanderControl.SelectedDate;
            GlobalScripts.Disappear_Element(Apply, TimeSpan.FromSeconds(.15));
            PhotoTakenTimeStackPanel.Children.Remove(Apply);
        }
        private Match_Display_Icon caller = new Match_Display_Icon();
        public ImageExif EXIF { get; set; }
        public System.Windows.Controls.Button Apply { get; set; } = new System.Windows.Controls.Button()
        {
            Content = "套用"
        };
        public void DisplayImage(object activator, ImageExif exif)
        {
            EXIF = exif;
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.DecodePixelHeight = (int)Math.Round(ImageArea.RenderSize.Height * 1.5);
            bitmap.UriSource = new Uri(exif.ImagePath, UriKind.Absolute);
            bitmap.EndInit();
            ImageBrush brush = new ImageBrush(bitmap);
            brush.ImageSource = bitmap;
            brush.Stretch = Stretch.Uniform;
            ImagePreview.Background = brush;
            PhotoTakenTimeDate.Content = exif.PhotoTakenTime_DateTime_Json;
            Longitude.Content = exif.GPS_Longitude;
            Latitude.Content = exif.GPS_Latitude;
            Altitude.Content = exif.GPS_Altitude;
            caller = activator as Match_Display_Icon;
        }

        private void PhtoTakenTime_CalendarClosed(object sender, RoutedEventArgs e)
        {
        }
        private void ShowApplyButton()
        {

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

        private void RowSplitter_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            Properties.Settings.Default["EXIF_Height"] = EXIFArea.ActualHeight;
            Properties.Settings.Default.Save();
            //Sets the row height and save it as user preferences which will be loaded in the next launch
        }
    }
}
