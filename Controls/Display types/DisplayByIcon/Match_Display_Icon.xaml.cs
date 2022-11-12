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
using Google_Drive_Organizer_V3.Classes;

namespace Google_Drive_Organizer_V3.Pages.MatchItem.Display_types
{
    /// <summary>
    /// Match_Display_Icon.xaml 的互動邏輯
    /// </summary>
    public partial class Match_Display_Icon : UserControl
    {
        public Match_Display_Icon()
        {
            Initialized += Match_Display_Icon_Initialized;
            InitializeComponent();
        }
        public ImageExif exif = new ImageExif();
        private void Match_Display_Icon_Initialized(object sender, EventArgs e)
        {
            try
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.DecodePixelHeight = 300;
                image.UriSource = new Uri(exif.EXIFData.ImagePath);
                image.EndInit();
                ImageBrush image_brush = new ImageBrush();
                image_brush.ImageSource = image;
                image_brush.Stretch = Stretch.Uniform;
                DisplayImage.Background = image_brush;
                ImageFileName.Content = System.IO.Path.GetFileName(exif.EXIFData.ImagePath);
            }
            catch (Exception)
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.DecodePixelHeight = 300;
                image.UriSource = new Uri(@"..\..\..\icons\Empty Image.png", UriKind.Relative);
                image.EndInit();
                ImageBrush image_brush = new ImageBrush();
                image_brush.ImageSource = image;
                image_brush.Stretch = Stretch.Uniform;
                DisplayImage.Background = image_brush;
                ImageFileName.Content = "錯誤";
            }
        }

        public Match_Display_Icon(ImageExif exif_input)
        {
            exif = exif_input;
            Initialized += Match_Display_Icon_Initialized;
            InitializeComponent();
        }

        private void ClickTrigger_Click(object sender, RoutedEventArgs e)
        {
            History.DisplayMatchPanel.EXIFViewer.DisplayImage(this, exif);
        }
    }
}
