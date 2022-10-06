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
using System.Diagnostics;
using SharpVectors;
using System.Globalization;

using System.IO;
using Newtonsoft.Json.Linq;
using System.Windows.Media.Animation;
using static Google_Drive_Organizer_V3.Pages.MatchItem.ImageExif_Record;
using static Google_Drive_Organizer_V3.Pages.MatchItem.ImageExif_Function;
using System.Threading;

namespace Google_Drive_Organizer_V3.Pages.MatchItem
{
    /// <summary>
    /// MatchItem_Main_Control.xaml 的互動邏輯
    /// </summary>
    public partial class MatchItem_Child : UserControl
    {

        public MatchItem_Child()
        {
            InitializeComponent();
            MainControl.Height = 35;
            Delete_Item_Grid.Width = 0;
        }
        public ImageExif_Class EXIF;
        public event Action<LoadImageDetail_ProgressModule> ImageEXIFData_Loaded;
        public MatchItem_Class match_import { get; set; }
        public CancellationTokenSource CTS { get; set; } = new CancellationTokenSource();
        bool Height_Expanded = false;
        bool Delete_Icon_Expanded = false;
        double Original_Height = 35;
        TimeSpan Height_Animation_Duration = TimeSpan.FromSeconds(.2);
        public string ImageLocation { get; set; }
        public string JsonLocation { get; set; }
        public MatchItem_Class match { get; set; }
        private Task<ImageExif_Class> GetEXIF;
        Action<ImageExif_Class> add;
        Task Completed;
        LoadImageDetail_ProgressModule image_loaded = new LoadImageDetail_ProgressModule();
        public MatchItem_Child(MatchItem_Class MatchInput)
        {
            Action completed = delegate
            {
                Console.WriteLine("Add to record completed");
            };
            Completed = new Task(completed);
            Initialized += async delegate
            {
                JsonLocation = MatchInput.Json_Location;
                ImageLocation = MatchInput.Image_Location;
                match_import = MatchInput;
                JsonName.Content = System.IO.Path.GetFileName(JsonLocation);
                ImageName.Content = System.IO.Path.GetFileName(ImageLocation);
                MainControl.Height = Initial_Height.Height.Value;
                //ImageDetailRecord.record.Add(Detail);
                Func<MatchItem_Class, CancellationToken, Task<ImageExif_Class>> LoadImageEXIF = Load_Image_EXIF_Async;
                GetEXIF = LoadImageEXIF(MatchInput, CTS.Token);
                //await Task.WhenAll(GetEXIF);
                //image_loaded = new LoadImageDetail_ProgressModule()
                //{
                //    EXIFFromJsonFile = GetEXIF.Result,
                //    AddToRecordCompleted = AddToRecord_Completed
                //};
                add = delegate (ImageExif_Class exif)
                {
                    //if (ImageExif_Record.Images.Find(item => item.Image_Location == exif.Image_Location) == null)
                    //{
                    //}
                    ImageExif_Record.Images.Add(exif);
                    History.MatchedItem.MatchingProgress.percentage = ImageExif_Record.Images.Count / History.MatchedItem.Matched_Item_Stackpanel.Children.Count;
                    //if (ImageExif_Record.Images.Count == History.MatchedItem.Matched_Item_Stackpanel.Children.Count)
                    //{
                    //    History.MatchedItem.SortInputDateComboBox();

                    //}

                    Completed.Start();
                };
                await Task.WhenAll(GetEXIF);
                add(GetEXIF.Result);


                EXIF = GetEXIF.Result;
                //EXIF = IsCompletedImage_Detail_Async(MatchInput.Image_Location, MatchInput.Json_Location, CTS.Token);
                //ImageEXIFData_Loaded(new LoadImageDetail_ProgressModule
                //{
                //    EXIFFromJsonFile = EXIF
                //});
                Apply_Image_Detail(EXIF);
            };

            InitializeComponent();
            match_import = MatchInput;
        }
        public MatchItem_Child(ImageExif_Class exif_input)
        {
            Action completed = delegate
            {
                Console.WriteLine("Add to record completed using exif");
            };
            Completed = new Task(completed);
            Initialized += async delegate
            {
                match_import = new MatchItem_Class(exif_input.Image_Location, exif_input.Json_Location);
                JsonName.Content = System.IO.Path.GetFileName(JsonLocation);
                ImageName.Content = System.IO.Path.GetFileName(ImageLocation);
                MainControl.Height = Initial_Height.Height.Value;
                //await Task.WhenAll(GetEXIF);
                //image_loaded = new LoadImageDetail_ProgressModule()
                //{
                //    EXIFFromJsonFile = GetEXIF.Result,
                //    AddToRecordCompleted = AddToRecord_Completed
                //};
                add = delegate (ImageExif_Class exif)
                {
                    if (ImageExif_Record.Images.Find(item => item.MD5 == exif.MD5) == null)
                    {
                        ImageExif_Record.Images.Add(exif);
                        History.MatchedItem.MatchingProgress.percentage = ImageExif_Record.Images.Count / History.MatchedItem.Matched_Item_Stackpanel.Children.Count;
                        if (ImageExif_Record.Images.Count == History.MatchedItem.Matched_Item_Stackpanel.Children.Count)
                        {
                            History.MatchedItem.SortInputDateComboBox();

                        }
                    }

                    Completed.Start();
                };


                EXIF = exif_input;
                //EXIF = IsCompletedImage_Detail_Async(MatchInput.Image_Location, MatchInput.Json_Location, CTS.Token);
                //ImageEXIFData_Loaded(new LoadImageDetail_ProgressModule
                //{
                //    EXIFFromJsonFile = EXIF
                //});
                Apply_Image_Detail(EXIF);
            };
            InitializeComponent();
            JsonLocation = exif_input.Json_Location;
            ImageLocation = exif_input.Image_Location;
            JsonName.Content = System.IO.Path.GetFileName(exif_input.Json_Location);
            ImageName.Content = System.IO.Path.GetFileName(exif_input.Image_Location);
            MainControl.Height = Initial_Height.Height.Value;
            //Apply_Image_Detail(detail);
        }


        private void MainControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public class JsonObj
        {
            public string PhotoTakenTime_String { get; set; }
            public DateTime PhotoTakenTime_DateTime { get; set; }
            public double GPS_Longitude { get; set; }
            public double GPS_Latitude { get; set; }
            public double Altitude { get; set; }
        }
        public async Task Toggle_Expand()
        {
            if (Height_Expanded == false)
            {
                await Expand_Height();
            }
            else if (Height_Expanded == true)
            {
                await Collaps_Height();
                //Console.WriteLine("Collaps");
            }
        }

        private async void Expend_Info_MouseUP(object sender, MouseButtonEventArgs e)
        {

            //BitmapImage image = new BitmapImage(new Uri(ImageLocation, UriKind.Absolute));
            //ImageBrush imageBrush = new ImageBrush(image);
            //imageBrush.Stretch = Stretch.Uniform;
            //Image_Match.Background = imageBrush;
            //await Toggle_Expand();
            if (Height_Expanded == false)
            {
                await Expand_Height();
            }
            else if (Height_Expanded == true)
            {
                await Collaps_Height();
                //Console.WriteLine("Collaps");
            }
            //Console.WriteLine("Toggle");
        }

        private void MainControl_MouseMove(object sender, MouseEventArgs e)
        {
            double TriggerWidth = 50;
            //Console.WriteLine(Mouse.GetPosition(MainControl));
            if (Mouse.GetPosition(MainControl).X < TriggerWidth)
            {
                if (Delete_Icon_Expanded == false)
                {

                    Expand_Delete_Button(TriggerWidth);
                }
                MainGrid.MouseLeave += delegate
                {
                    Collaps_Delete_Button();
                };
                Delete_Item_Grid.MouseLeave += delegate
                {
                    Collaps_Delete_Button();
                };
                //MainControl.Height = (double)MainControl.Resources["Expanded_Height"];
                //Expand_Height((double)MainControl.Resources["Expanded_Height"]);
                //await Load_Detail();
            }
            else if (Mouse.GetPosition(MainControl).X > TriggerWidth && Delete_Icon_Expanded == true)
            {
                Collaps_Delete_Button();
            }
        }

        private void Apply_Image_Detail(ImageExif_Class detail)
        {
            try
            {
                PhotoTakenTime.Content = detail.phototakentime;
                PhotoTakenTime.ToolTip = detail.phototakentime;
                CameraManufactor.Content = detail.CameraManufactor;
                Photo_GPS_Longitude.Content = detail.GPS_Longitude;
                Photo_GPS_Latitude.Content = detail.GPS_Latitude;
                Photo_GPS_Altitude.Content = detail.GPS_Altitude;
                Json_GPS_Longitude.Content = detail.Json_GPS_Longitude;
                Json_GPS_Latitude.Content = detail.Json_GPS_Latitude;
                Json_GPS_Altitude.Content = detail.Json_GPS_Altitude;
                Json_PhotoTakenTime.Content = detail.Json_PhotoTakenTime;
                SharpVectors.Converters.SvgViewbox canva = new SharpVectors.Converters.SvgViewbox
                {
                    UriSource = new Uri(@".\icons\multiple.svg", UriKind.Relative),

                };

                //SVG_Grid.Children.Add(canva);
                //Console.WriteLine(canva.RenderSize);

            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Apply image detail cancelled");
            }
            //Image_Match.Background = new ImageBrush(new BitmapImage(new Uri(detail.image_location))) { Stretch = Stretch.Uniform};
        }
        private void Expand_Delete_Button(double width)
        {
            DoubleAnimation delete_icon_expand = new DoubleAnimation(Delete_Item_Grid.RenderSize.Width, width, TimeSpan.FromSeconds(.15));
            delete_icon_expand.DecelerationRatio = .1;
            delete_icon_expand.AccelerationRatio = .1;
            Delete_Item_Grid.BeginAnimation(WidthProperty, delete_icon_expand);
            Delete_Icon_Expanded = true;
        }
        private void Collaps_Delete_Button()
        {
            DoubleAnimation delete_icon_collaps = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(.15)));
            delete_icon_collaps.DecelerationRatio = .1;
            delete_icon_collaps.AccelerationRatio = .1;
            Delete_Item_Grid.BeginAnimation(WidthProperty, delete_icon_collaps);
            Delete_Icon_Expanded = false;
        }
        public async Task Expand_Height()
        {
            if (EXIF == null)
            {
                EXIF = await Task.Run(() => ImageExif_Record.Images.Find(item => item.Image_Location == ImageLocation));
                MatchItem_Class json = await Task.Run(() => MatchItem_Record.Matched_Items.Find(item => item.Image_Location == ImageLocation));
                if (EXIF != null)
                {

                    Apply_Image_Detail(EXIF);

                }
                //animation start
                DoubleAnimation expand_height_animation = new DoubleAnimation(MainControl.RenderSize.Height, (double)MainControl.Resources["Expanded_Height"], Height_Animation_Duration);
                //Original_Height = MainControl.RenderSize.Height;
                expand_height_animation.DecelerationRatio = .1;
                expand_height_animation.AccelerationRatio = .1;
                MainControl.BeginAnimation(HeightProperty, expand_height_animation);
                //animation end
                await Task.Delay(TimeSpan.FromSeconds(.3));
                if (EXIF == null)
                {
                    EXIF = await Load_Image_EXIF_Async(match_import, CTS.Token);
                    Apply_Image_Detail(EXIF);
                }
            }
            else if (EXIF != null)
            {
                Apply_Image_Detail(EXIF);
                //animation start
                DoubleAnimation expand_height_animation = new DoubleAnimation(MainControl.RenderSize.Height, (double)MainControl.Resources["Expanded_Height"], Height_Animation_Duration);
                //Original_Height = MainControl.RenderSize.Height;
                expand_height_animation.DecelerationRatio = .1;
                expand_height_animation.AccelerationRatio = .1;
                MainControl.BeginAnimation(HeightProperty, expand_height_animation);
                //animation end
            }
            await Task.Delay(Height_Animation_Duration);
            BitmapImage display_image = new BitmapImage();
            display_image.BeginInit();
            display_image.UriSource = new Uri(EXIF.Image_Location);
            display_image.DecodePixelHeight = (int)Image_Match.RenderSize.Height * 2;
            //display_image.DecodePixelWidth = (int)Image_Match.RenderSize.Width;
            display_image.EndInit();
            ImageBrush image = new ImageBrush();
            image.ImageSource = display_image;
            image.Stretch = Stretch.Uniform;
            Image_Match.Background = image;
            //Image_Match.Background = new ImageBrush(new BitmapImage(new Uri(EXIF.Image_Location)) { DecodePixelHeight = 1, DecodePixelWidth = 1})
            //{
            //    Stretch = Stretch.Uniform,
            //};
            Height_Expanded = true;
        }
        public async Task Collaps_Height()
        {
            Image_Match.Background = null;
            GC.Collect();
            DoubleAnimation collaps_height_animation = new DoubleAnimation(MainControl.RenderSize.Height, Initial_Height.Height.Value, Height_Animation_Duration);
            collaps_height_animation.DecelerationRatio = .1;
            collaps_height_animation.AccelerationRatio = .1;
            MainControl.BeginAnimation(HeightProperty, collaps_height_animation);
            Height_Expanded = false;
        }

        private void Open_Image_Trigger_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(System.IO.Path.GetFullPath(ImageLocation));
        }

        private async void Delete_Item_Trigger_Click(object sender, RoutedEventArgs e)
        {
            CTS.Cancel();
            //await Task.WhenAll(Completed);
            //EXIF = GetEXIF.Result;
            Console.WriteLine("File Name" + System.IO.Path.GetFileName(EXIF.Image_Location));

            if (MatchItem_Record.Matched_Items.Count == 1)
            {
                MessageBoxResult delete_last_match = MessageBox.Show("請問您確定要將最後一張照片從此清單移除嗎?您將需要重新選擇資料夾以繼續之後的步驟。", "警告", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (delete_last_match == MessageBoxResult.Yes)
                {

                    History.MatchedItem.Matched_Item_Stackpanel.Children.Remove(this);
                    Images.Remove(EXIF);
                    MatchItem_Record.Matched_Items.Remove(match_import);
                    History.MatchedItem.NoItemFounded();
                }
            }
            else
            {
                History.MatchedItem.Matched_Item_Stackpanel.Children.Remove(this);
                Images.Remove(EXIF);
                Console.WriteLine("EXIF MD5" + EXIF.MD5);
                MatchItem_Record.Matched_Items.Remove(match_import);
            }
            History.MatchedItem.SortInputDateComboBox();
        }
    }
}
