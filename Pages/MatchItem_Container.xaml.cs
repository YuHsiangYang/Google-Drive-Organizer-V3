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
using Google_Drive_Organizer_V3;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Google_Drive_Organizer_V3.SelectedFolder_Record;
using static Google_Drive_Organizer_V3.MatchItem_Record;
using Google_Drive_Organizer_V3.Controls;
using Newtonsoft.Json;
using System.Windows.Media.Animation;
using System.Reflection;
using System.Windows.Forms;
using Google_Drive_Organizer_V3.Classes;
using Google_Drive_Organizer_V3.Classes.Display_types;

namespace Google_Drive_Organizer_V3.Pages.MatchItem
{
    /// <summary>
    /// MatchItem_Container.xaml 的互動邏輯
    /// </summary>
    public partial class MatchItem_Container : System.Windows.Controls.UserControl
    {
        public List<MatchItem_Child> MatchItems = new List<MatchItem_Child>();
        public event Action NextStepTriggered;
        public CancellationTokenSource Cancel_LoadDetailRecord { get; set; } = new CancellationTokenSource();
        public CancellationTokenSource CancelAllTasks { get; set; } = new CancellationTokenSource();
        public List<string> Years_With_Duplicates { get; set; } = new List<string>();
        public List<string> Years_Without_Duplicates { get; set; } = new List<string>();
        public List<string> Months_With_Duplicates { get; set; } = new List<string>();
        public List<string> Months_Without_Duplicates { get; set; } = new List<string>();
        public List<string> Days_With_Duplicates { get; set; } = new List<string>();
        public List<string> Day_Without_Duplicates { get; set; } = new List<string>();
        public Search_Date_UserControl SearchDate = new Search_Date_UserControl()
        {
            Name = "SearchDate_Element",
            Height = 26,
        };
        public ResourceDictionary ApplicationResources { get; set; } = new ResourceDictionary();
        public Search_By_FileName SearchFileName { get; set; } = new Search_By_FileName();
        CancellationTokenSource search_cancel_cts = new CancellationTokenSource();
        public List<ImageExif> ItemsForDisplay = new List<ImageExif>();
        IDisplayInterface displayInterface;

        /// <summary>
        /// 排序的類型
        /// </summary>
        public enum SortType
        {
            拍攝日期,
            檔案名稱
        }
        /// <summary>
        /// 排序的方式
        /// </summary>
        public enum SortManner
        {
            遞增,
            遞減
        }


        public MatchItem_Container()
        {
            InitializeComponent();
            Search_Type.Items.Add("Json 拍攝日期");
            Search_Type.Items.Add("檔案名稱");
            //Expand_Collaps.On_Text = "展開";
            //Expand_Collaps.Off_Text = "收起";
            //Expand_Collaps_Label.Content = Expand_Collaps.Off_Text;
            SearchDate.Input_Changed += Input_Date_Input_Changed;
            SearchFileName.SearchFileName += SearchFileName_SearchFileActivated;
            Input_Search_Grid.Children.Add(SearchDate);
            //ApplicationResources.Source = new Uri("ApplicationResources.xaml", UriKind.Relative);
            //Search_Btn = new Button()
            //{
            //    Margin = new Thickness(10, 0, 0, 0),
            //    VerticalAlignment = VerticalAlignment.Center,
            //    Height = 24,
            //    Width = 52,
            //    BorderBrush = null,
            //    Cursor = Cursors.Hand,
            //    Padding = new Thickness(4),
            //    Content = "搜尋",
            //    Template = ApplicationResources["CustomButtonTemplate"] as ControlTemplate
            //};
            //ButtonCustomProperties.SetCornerRadius(Search_Btn, 4);
            //Search_Btn.Click += Search_Click;
            //Search_Panel.Children.Add(Search_Btn);
        }
        public async void UserControl_Main_Loaded(object sender, RoutedEventArgs e)
        {
            //Add search inputs start
            //Input_Search_Grid.Children.Add(SearchDate);
            Input_Search_Grid.UpdateLayout();
            display_type.DisplayTypeChanged += Display_type_DisplayTypeChanged;
            //SearchFileName.SetBinding(HeightProperty, new Binding()
            //{
            //    Source = Input_Search_Grid,
            //    Path = new PropertyPath(Grid.ActualHeightProperty)
            //});
            //SearchDate.HorizontalAlignment = HorizontalAlignment.Left;
            SearchDate.Width = SearchDate.RenderSize.Width;
            //Add Search inputs end

            Search_Type.SelectedIndex = 0;
            //Creating Text change event start
            //SearchFileName.TextChanged += SearchFileName_TextChanged;
            //SearchFileName.KeyDown += SearchFileName_KeyDown;
            //Creating Text change event end
            //creating input change event for search date

            Sort_Type.ItemsSource = Enum.GetValues(typeof(SortType)).Cast<SortType>();
            Sort_Manner.ItemsSource = Enum.GetValues(typeof(SortManner)).Cast<SortManner>();
            Sort_Type.SelectedIndex = 0;
            Sort_Manner.SelectedIndex = 0;
            Sort_Type.SelectionChanged += SortInput_SelectionChanged;
            Sort_Manner.SelectionChanged += SortInput_SelectionChanged;

            SearchDate.IsEnabled = false;
            Progress<LoadEXIFRecord_ProgressReportModule> loadmatch_progress = new Progress<LoadEXIFRecord_ProgressReportModule>();
            loadmatch_progress.ProgressChanged += Loadmatch_progress_ProgressChanged;
            Matched_Items = await LoadMatch(Folders, loadmatch_progress, CancelAllTasks.Token);
            Progress<LoadEXIFRecord_ProgressReportModule> progress = new Progress<LoadEXIFRecord_ProgressReportModule>();
            progress.ProgressChanged += EXIF_ProgressChanged;
            await ImageExif_Functions.Load_Image_EXIF_Record(CancelAllTasks.Token, progress);
            ShowAllItems();
            //Progress<MatchingProgress_Report> progression = new Progress<MatchingProgress_Report>();
            //progression.ProgressChanged += Matching_ProgressChanged;
            //await UniversalFunctions.Load_Image_Detail_Record_Async(progression, Cancel_LoadDetailRecord.Token);
            //Console.WriteLine(Matched_Item_Stackpanel.Children.Count);
            Pages.PageChanged += page_changed;
            switch (display_type.Display_Type)
            {
                case TypeOfDisplay.ListView:
                    displayInterface = new Display_List();
                    break;
                case TypeOfDisplay.ImageView:
                    displayInterface = new Display_Icon();
                    break;
                default:
                    break;
            }
            //displayInterface.InitializePage(Matched_Item_ScrollViewer);
        }

        private void Loadmatch_progress_ProgressChanged(object sender, LoadEXIFRecord_ProgressReportModule e)
        {
            MatchingProgress.percentage = (double)(e.CurrentItem / e.TotalItems);
        }

        private void EXIF_ProgressChanged(object sender, LoadEXIFRecord_ProgressReportModule e)
        {
            MatchingProgress.percentage = ((double)e.CurrentItem + 1) / (double)e.TotalItems;
            ItemsForDisplay.Add(e.EXIFData);
            ImageExif_Record.Images.Add(e.EXIFData);
            if (ImageExif_Record.Images.Count % (int)Properties.Settings.Default["Page_Size"] == 0)
            {
                Pages.LoadRange(ImageExif_Record.Images.Count);
            }
            if (Matched_Item_Stackpanel.Children.Count < 30)
            {
                Matched_Item_Stackpanel.Children.Add(new MatchItem_Child(e.EXIFData));
            }
            if (e.CurrentItem == e.TotalItems)
            {
                SortInputDateComboBox();
            }
        }

        private void page_changed(int number)
        {
            displayInterface.ShowPage(number);
        }

        private void Display_type_DisplayTypeChanged(TypeOfDisplay obj)
        {
            switch (obj)
            {
                case TypeOfDisplay.ListView:
                    displayInterface = new Display_List();
                    break;
                case TypeOfDisplay.ImageView:
                    displayInterface = new Display_Icon();
                    break;
                default:
                    break;
            }
        }

        private async void SortInput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ItemsForDisplay = await ImageExif_Functions.SortResult(ImageExif_Record.Images, (SortManner)Sort_Manner.SelectedItem, (SortType)Sort_Type.SelectedItem);
            displayInterface.ShowPage((int)Properties.Settings.Default["page_number"]);
        }
        
        private async void SearchFileName_SearchFileActivated(string obj)
        {
            ItemsForDisplay = await SearchByFileName(obj, search_cancel_cts.Token);
            ShowPage((int)Properties.Settings.Default["page_number"]);
        }

        private async void Input_Date_Input_Changed(Dictionary<string, string> obj)
        {
            if (obj.ToList().FindAll(input => input.Value == "全部").Count < 3)
            {
                ItemsForDisplay = await SearchByPhotoTakenTime(obj);
                ShowPage((int)Properties.Settings.Default["page_number"]);
            }
            //List<string> inputs = ((string[])obj).ToList();
            //if (inputs.FindAll(input => input == "全部").Count != 3)
            //{
            //    ShowResult()
            //}
        }

        public void SortInputDateComboBox()
        {
            CancellationToken ct = CancelAllTasks.Token;
            Years_With_Duplicates = new List<string>();
            Months_With_Duplicates = new List<string>();
            Days_With_Duplicates = new List<string>();
            try
            {
                ImageExif_Record.Images.ForEach(image_exif => Years_With_Duplicates.Add(image_exif.Json_PhotoTakenTime_DateTime.Year.ToString()));
                ImageExif_Record.Images.ForEach(image_exif => Months_With_Duplicates.Add(image_exif.Json_PhotoTakenTime_DateTime.Month.ToString()));
                ImageExif_Record.Images.ForEach(image_exif => Days_With_Duplicates.Add(image_exif.Json_PhotoTakenTime_DateTime.Day.ToString()));
                //foreach (var item in ImageExif_Record.Images)
                //{
                //    Years_With_Duplicates.Add(item.Json_PhotoTakenTime_DateTime.Year.ToString());
                //    ct.ThrowIfCancellationRequested();
                //    Months_With_Duplicates.Add(item.Json_PhotoTakenTime_DateTime.Month.ToString());
                //    ct.ThrowIfCancellationRequested();
                //    Days_With_Duplicates.Add(item.Json_PhotoTakenTime_DateTime.Day.ToString());
                //    ct.ThrowIfCancellationRequested();
                //    MatchingProgress.percentage += (ImageExif_Record.Images.IndexOf(item) + 1) / ImageExif_Record.Images.Count;
                //    ct.ThrowIfCancellationRequested();
                //}
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("所有作業已被取消");
            }

            Years_Without_Duplicates = Years_With_Duplicates.Distinct().ToList();
            Years_Without_Duplicates.Sort();
            Years_Without_Duplicates.Insert(0, "全部");
            Months_Without_Duplicates = Months_With_Duplicates.Distinct().ToList();
            Months_Without_Duplicates.Sort();
            Months_Without_Duplicates.Insert(0, "全部");
            Day_Without_Duplicates = Days_With_Duplicates.Distinct().ToList();
            Day_Without_Duplicates.Sort();
            Day_Without_Duplicates.Insert(0, "全部");

            SearchDate.Search_Input_Year.ItemsSource = Years_Without_Duplicates;
            SearchDate.Search_Input_Year.SelectedIndex = 0;
            SearchDate.Search_Input_Month.ItemsSource = Months_Without_Duplicates;
            SearchDate.Search_Input_Month.SelectedIndex = 0;
            SearchDate.Search_Input_Day.ItemsSource = Day_Without_Duplicates;
            SearchDate.Search_Input_Day.SelectedIndex = 0;
            //MatchingProgress.percentage = 1;
            SearchDate.IsEnabled = true;
            SearchDate.Input_Changed += SearchDate_Input_Changed;
        }


        private async void SearchDate_Input_Changed(Dictionary<string, string> obj)
        {
            if (obj.Values.ToList().FindAll(value => value == "全部").Count != 3)
            {
                ItemsForDisplay.Clear();
                ItemsForDisplay = await SearchByPhotoTakenTime(obj);
                displayInterface.ShowPage((int)Properties.Settings.Default["page_number"]);
            }
            else
            {
            }
            GC.Collect();
            //RoutedEventArgs routedEventArgs = new RoutedEventArgs();
            //Search_Click(this, routedEventArgs);
        }

        private void Matching_ProgressChanged(object sender, MatchingProgress_Report e)
        {
            MatchingProgress.percentage = (double)e.Current_Item / (double)e.Total_Items * 0.75;
            Console.WriteLine((double)e.Current_Item / (double)e.Total_Items);
            Console.WriteLine(e.Total_Items);
        }
        public async Task<List<ImageExif>> SearchByFileName(string FileName, CancellationToken cts)
        {
            List<ImageExif> founded = new List<ImageExif>();
            founded = await Task.Run(() => ImageExif_Record.Images.FindAll(x => System.IO.Path.GetFileName(x.ImagePath).Contains(FileName)).ToList());
            List<MatchItem_Child> children = new List<MatchItem_Child>();
            //foreach (ImageExif_Class item in founded)
            //{
            //    children.Add(new MatchItem_Child(item));
            //}
            //Console.WriteLine("Found " + founded.Count);
            return founded;
        }
        public async Task<List<ImageExif>> SearchByPhotoTakenTime(Dictionary<string, string> inputs)
        {
            List<ImageExif> founded = new List<ImageExif>();
            List<MatchItem_Child> children = new List<MatchItem_Child>();
            List<MatchItem_Child> children_return = new List<MatchItem_Child>();
            //List<string> Inputs = ((string[])inputs).ToList();
            Console.WriteLine(inputs.Values.ToList());
            if (inputs.Values.ToList().FindAll(item => item == "全部").Count == 3)
            {
                foreach (ImageExif item in ImageExif_Record.Images)
                {
                    children.Add(new MatchItem_Child(item));
                }
            }
            foreach (KeyValuePair<string, string> item in inputs)
            {
                Console.WriteLine(item.Key + "值" + item.Value);
            }
            int search_count = 0;
            foreach (KeyValuePair<string, string> item in inputs)
            {
                if (item.Value != "全部")
                {
                    string year_month_day = item.Key.Substring(item.Key.IndexOf("t_") + 2, item.Key.Length - (item.Key.IndexOf("t_") + 2));
                    if (search_count == 0)
                    {
                        //foreach (var itemm in Image_Detail_Record.Images)
                        //{
                        //    Console.WriteLine(itemm.Photo_TakenTime_Dictionary[year_month_day]);
                        //}
                        List<ImageExif> temp_found = await Task.Run(() => ImageExif_Record.Images.FindAll(images => images.Photo_TakenTime_Dictionary[year_month_day] == int.Parse(item.Value)));

                        founded.AddRange(temp_found);
                    }
                    else
                    {
                        var found_temp = founded.FindAll(images => images.Photo_TakenTime_Dictionary[year_month_day] == int.Parse(item.Value));
                        founded = found_temp;
                    }

                    search_count++;
                }
            }
            //founded.Distinct().ToList().ForEach(item => children.Add(new MatchItem_Child(item)));
            return founded;
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            switch (Search_Type.SelectedIndex)
            {
                case 0://json photo taken time
                    List<ImageExif> founded = new List<ImageExif>();
                    if (SearchDate.Search_Input_Year.SelectedItem.ToString() != "全部")
                    {
                        if (founded.Count == 0)
                        {
                            founded = ImageExif_Record.Images.FindAll(x => x.Json_PhotoTakenTime_DateTime.Year == int.Parse(SearchDate.Search_Input_Year.SelectedItem.ToString()));
                        }
                    }
                    if (SearchDate.Search_Input_Month.SelectedItem.ToString() != "全部")
                    {
                        if (founded.Count == 0)
                        {
                            founded = ImageExif_Record.Images.FindAll(x => x.Json_PhotoTakenTime_DateTime.Month == int.Parse(SearchDate.Search_Input_Month.SelectedItem.ToString()));

                        }
                        else if (founded.Count > 0)
                        {

                            founded = founded.FindAll(x => x.Json_PhotoTakenTime_DateTime.Month == int.Parse(SearchDate.Search_Input_Month.SelectedItem.ToString()));
                        }
                    }
                    if (SearchDate.Search_Input_Day.SelectedItem.ToString() != "全部")
                    {
                        if (founded.Count == 0)
                        {

                            founded = ImageExif_Record.Images.FindAll(x => x.Json_PhotoTakenTime_DateTime.Day == int.Parse(SearchDate.Search_Input_Day.SelectedItem.ToString()));
                        }
                        else if (founded.Count > 0)
                        {
                            founded = founded.FindAll(x => x.Json_PhotoTakenTime_DateTime.Day == int.Parse(SearchDate.Search_Input_Day.SelectedItem.ToString()));

                        }
                    }

                    if (founded.Count > 0)
                    {

                        Console.WriteLine("Item founded in input date" + founded.Count);
                        Matched_Item_Stackpanel.Children.Clear();
                        foreach (ImageExif item in founded)
                        {
                            Matched_Item_Stackpanel.Children.Add(new MatchItem_Child(item));
                        }

                    }
                    else if (founded.Count == 0)
                    {
                        if (SearchDate.Inputs.ToList().FindAll(x => x == "全部").Count != 3)
                        {
                            NoItemFounded();
                        }
                        else
                        {
                            ShowAllItems();
                        }
                    }
                    break;
                case 1://file name
                    search_cancel_cts.Cancel();
                    search_cancel_cts = new CancellationTokenSource();
                    //ShowResult(await Find_Item_Using_FileName(SearchFileName.Text, search_cancel_cts.Token));
                    break;
                default:
                    break;
            }
        }

        private async void Search_Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TimeSpan animation_duration = TimeSpan.FromSeconds(.15);
            if (Search_Type.SelectedIndex == 1)//input for file name
            {
                UniversalFunctions.Disappear_Element(SearchDate, new Duration(animation_duration));
                await Task.Delay(animation_duration);
                //Search_Panel.Children.Add(Search_Btn);
                Input_Search_Grid.Children.Remove(SearchDate);
                Input_Search_Grid.Children.Add(SearchFileName);
                //UniversalFunctions.Appear_Element(Search_Btn, animation_duration);
                UniversalFunctions.Appear_Element(SearchFileName, new Duration(animation_duration));
                UpdateLayout();
                //await Task.Delay(animation_duration);
                DoubleAnimation animation = new DoubleAnimation(SearchDate.RenderSize.Width, SearchFileName.RenderSize.Width, new Duration(animation_duration));
                animation.DecelerationRatio = .2;
                animation.AccelerationRatio = animation.DecelerationRatio;
                //Input_Search_Grid.BeginAnimation(WidthProperty, animation);
            }
            else if (Search_Type.SelectedIndex == 0 && Input_Search_Grid.Children.Contains(SearchDate) == false)
            {
                UniversalFunctions.Disappear_Element(SearchFileName, new Duration(animation_duration));
                //UniversalFunctions.Disappear_Element(Search_Btn, animation_duration);
                await Task.Delay(animation_duration);
                Input_Search_Grid.Children.Remove(SearchFileName);
                Input_Search_Grid.Children.Add(SearchDate);
                //Search_Panel.Children.Remove(Search_Btn);
                UniversalFunctions.Appear_Element(SearchDate, new Duration(animation_duration));
                UpdateLayout();
                await Task.Delay(animation_duration);
                DoubleAnimation animation = new DoubleAnimation(SearchFileName.RenderSize.Width, SearchDate.RenderSize.Width, new Duration(animation_duration));
                Console.WriteLine(SearchDate.RenderSize.Width);
                animation.DecelerationRatio = .2;
                animation.AccelerationRatio = animation.DecelerationRatio;
                //Input_Search_Grid.BeginAnimation(WidthProperty, animation);
            }
        }

        private void ShowPage(int page)
        {
            List<MatchItem_Child> children = new List<MatchItem_Child>();
            int from = (page - 1) * 30;
            int to = page * 30;
            for (int i = from; i < to; i++)
            {
                try
                {
                    children.Add(new MatchItem_Child(ItemsForDisplay[i]));

                }
                catch (Exception)
                {
                    Console.WriteLine("items not enough");
                }
            }
            //foreach (ImageExif_Class item in ItemsForDisplay)
            //{
            //    children.Add(new MatchItem_Child(item)); 
            //}
            Matched_Item_Stackpanel.Children.Clear();
            foreach (var item in children)
            {
                if (Matched_Item_Stackpanel.Children.IndexOf(item) == -1)
                {
                    Matched_Item_Stackpanel.Children.Add(item);
                    UniversalFunctions.Appear_Element(item, new Duration(TimeSpan.FromSeconds(0.15)));
                }
            }
            if (ItemsForDisplay.Count == 0)
            {
                NoItemFounded();
            }
            Pages.LoadRange(ItemsForDisplay.Count);
        }
        private void ShowAllItems()
        {
            List<MatchItem_Child> childs = new List<MatchItem_Child>();
            //Pages.LoadRange(ImageExif_Record.Images.Count);
            Matched_Item_Stackpanel.Children.Clear();
            for (int i = ((int)Properties.Settings.Default["page_number"] - 1) * 30; i < (int)Properties.Settings.Default["page_number"] * 30; i++)
            {
                try
                {
                    MatchItem_Child matchItem_Child = new MatchItem_Child(ImageExif_Record.Images[i]);
                    matchItem_Child.ImageEXIFData_Loaded += MatchItem_Child_ImageEXIFData_Loaded;
                    Matched_Item_Stackpanel.Children.Add(matchItem_Child);
                    Matched_Item_Stackpanel.UpdateLayout();
                }
                catch (Exception)
                {
                    Console.WriteLine("Notification: 超出資料範圍");
                }
            }
            ItemsForDisplay.AddRange(ImageExif_Record.Images);
        }

        private void MatchItem_Child_ImageEXIFData_Loaded(LoadImageDetail_ProgressModule obj)
        {
            //if (ImageExif_Record.Images.Find(item => item.Image_Location == obj.EXIFFromJsonFile.Image_Location) == null)
            //{

            //}
            ImageExif_Record.Images.Add(obj.EXIFFromJsonFile);
            MatchingProgress.percentage = ImageExif_Record.Images.Count / Matched_Item_Stackpanel.Children.Count;
            if (ImageExif_Record.Images.Count == Matched_Item_Stackpanel.Children.Count)
            {
                SortInputDateComboBox();
                obj.AddToRecordCompleted.Start();
            }
        }

        public void NoItemFounded()
        {
            Matched_Item_Stackpanel.Children.Clear();
            Controls.Cannot_Find_Search_Result cannot_find_result = new Controls.Cannot_Find_Search_Result();
            cannot_find_result.Width = 300;
            System.Windows.Data.Binding binding = new System.Windows.Data.Binding();
            binding.Source = Matched_Item_ScrollViewer;
            binding.Path = new PropertyPath(ScrollViewer.ActualHeightProperty);
            cannot_find_result.SetBinding(HeightProperty, binding);
            Matched_Item_Stackpanel.Children.Add(cannot_find_result);
            Console.WriteLine("Height" + cannot_find_result.RenderSize.Height);
        }

        private void Previous_Step_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in System.Windows.Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    CancelAllTasks.Cancel();
                    MainWindow main_window = window as MainWindow;
                    main_window.MainWindow_Grid.Children.Clear();
                    main_window.MainWindow_Grid.Children.Add(History.SelectFolder);
                    ImageExif_Record.Images.Clear();
                    MatchItem_Record.Matched_Items.Clear();
                    History.MatchedItem = new MatchItem_Container();
                }
            }
        }

        private void Expand_Collaps_On_Status_Changed(bool arg1, string arg2)
        {

        }

        private void Next_Step_Click(object sender, RoutedEventArgs e)
        {
            if (ImageExif_Record.Images.Count > 0)
            {
                NextStepTriggered();
            }
            else
            {
                System.Windows.MessageBox.Show("無法前往下一步");
            }

        }
    }
}
