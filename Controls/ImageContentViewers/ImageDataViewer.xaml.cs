using Google_Drive_Organizer_V3.Classes;
using Google_Drive_Organizer_V3.Controls.ImageContentViewers;
using Google_Drive_Organizer_V3.Pages.MatchItem.Display_types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public partial class ImageDataViewer : UserControl
    {
        public ImageDataViewer()
        {
            Initialized += EXIFDataViewer_Initialized;
            InitializeComponent();
        }
        private Json_ContentViewer JsonViewer = new Json_ContentViewer()
        {

        };

        private Exif_ContentViewer EXIFViewer = new Exif_ContentViewer()
        {

        };

        private void EXIFDataViewer_Initialized(object sender, EventArgs e)
        {
            //Load the previous height Set by the user
            Row_Height.Height = new GridLength(double.Parse(Properties.Settings.Default["EXIF_Height"].ToString()), GridUnitType.Pixel);

            //add event listener to the click event of the apply button
            Apply.Click += Apply_Click;


            //Add the viewers
            EXIF_Json_Grid.Children.Add(EXIFViewer);
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            //caller.exif.JsonData.PhotoTakenTime_DateTime = (DateTime)CalanderControl.SelectedDate;
            //GlobalScripts.Disappear_Element(Apply, TimeSpan.FromSeconds(.15));
            //PhotoTakenTimeStackPanel.Children.Remove(Apply);
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

            //Load the preview of the image
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.DecodePixelHeight = 500;
            bitmap.UriSource = new Uri(exif.EXIFData.ImagePath, UriKind.Absolute);
            bitmap.EndInit();
            ImageBrush preview_image_brush = new ImageBrush(bitmap);
            preview_image_brush.ImageSource = bitmap;
            preview_image_brush.Stretch = Stretch.Uniform;
            ImagePreview.Background = preview_image_brush;

            //Load the info for JSON
            JsonViewer.DisplayJSONData(EXIF.JsonData);

            //Load the exif
            EXIFViewer.DisplayEXIF(EXIF.EXIFData);
            caller = activator as Match_Display_Icon; //Sets the caller of the method
        }

        private void PhtoTakenTime_CalendarClosed(object sender, RoutedEventArgs e)
        {
        }
        private void ShowApplyButton()
        {

        }

        private void RowSplitter_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            Properties.Settings.Default["EXIF_Height"] = EXIFArea.ActualHeight;
            Properties.Settings.Default.Save();
            //Sets the row height and save it as user preferences which will be loaded in the next launch
        }

        private async void ContentController_ContentType_Changed(object sender, ContentType e)
        {
            TimeSpan duration = TimeSpan.FromSeconds(.2);


            //Disappear the previous element
            switch (e)
            {
                case ContentType.Json:
                    GlobalScripts.SwipeTransition(EXIF_Json_Grid.Children[1], duration, GlobalScripts.SwipeDirection.RightToLeft, GlobalScripts.TransitionType.Disappear);
                    break;
                case ContentType.EXIF:
                    GlobalScripts.SwipeTransition(EXIF_Json_Grid.Children[1], duration, GlobalScripts.SwipeDirection.LeftToRight, GlobalScripts.TransitionType.Disappear);
                    break;
                default:
                    break;
            }
            await Task.Delay(duration);
            EXIF_Json_Grid.Children.Remove(EXIF_Json_Grid.Children[1]);

            //Animation for the selected content type
            switch (e)
            {
                case ContentType.Json:
                    EXIF_Json_Grid.Children.Add(JsonViewer);
                    GlobalScripts.SwipeTransition(JsonViewer, duration, GlobalScripts.SwipeDirection.LeftToRight, GlobalScripts.TransitionType.Appear);
                    break;
                case ContentType.EXIF:
                    EXIF_Json_Grid.Children.Add(EXIFViewer);
                    GlobalScripts.SwipeTransition(EXIFViewer, duration, GlobalScripts.SwipeDirection.RightToLeft, GlobalScripts.TransitionType.Appear);
                    break;
                default:
                    break;
            }
        }
    }
}
