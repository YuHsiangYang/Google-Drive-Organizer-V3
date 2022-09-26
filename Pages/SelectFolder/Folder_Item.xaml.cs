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
using System.IO;
using static Google_Drive_Organizer_V3.UniversalFunctions;
using System.Windows.Media.Animation;

namespace Google_Drive_Organizer_V3.Pages.SelectFolder
{
    /// <summary>
    /// Folder_Item.xaml 的互動邏輯
    /// </summary>
    public partial class Folder_Item : UserControl
    {
        public string FolderLocation = "";
        FolderRecordItem position;
        public Folder_Item()
        {
            InitializeComponent();
            //foreach (Window window in Application.Current.Windows)
            //{
            //    if (window.GetType() == typeof(MainWindow))
            //    {
            //        MainWindow s = window as MainWindow;
            //        s.Select_F.FolderItem_StackPanel.Children.Clear();
            //    }
            //}
        }
        public Folder_Item(string folder_location)
        {
            FolderLocation = folder_location;
            InitializeComponent();
        }

        private async Task<FolderRecordItem> GetFolderInformation(string folder)
        {
            FolderName.Content = await Task.Run(() => new DirectoryInfo(folder).Name);
            int json = await Task.Run(() => Directory.GetFiles(folder, "*.json", SearchOption.AllDirectories).Count());
            Progress.percentage = .5;
            JsonCount_Label.Content += json.ToString();
            int image = await Task.Run(() => Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories).Where(file => file.EndsWith(".jpeg") || file.EndsWith(".jpg")).Count());
            //string MD5 = await Task.Run(() => CalculateMD5(MD5Type.Folder, FolderLocation));
            string MD5 = "just a test of performance.";
            ImageCount_Label.Content += image.ToString();
            Progress.percentage = 1;

            return new FolderRecordItem(FolderLocation, MD5, json, image);
        }

        private void Delete_Icon_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            History.SelectFolder.FolderItem_StackPanel.Children.Remove(this);
            SelectedFolder_Record.Folders.Remove(position);
            CheckNextStep_Btn_Visibility();            //foreach (Window window in Application.Current.Windows)
            //{
            //    if (window.GetType() == typeof(MainWindow))
            //    {
            //        MainWindow main_window = window as MainWindow;
            //        Folder_Item Imported_item = new Folder_Item();
            //        Imported_item.Height = 75;
            //        main_window.SelectFolder_Control.FolderItem_StackPanel.Children.Remove(this);
            //        SelectedFolder_Record.Folders.Remove(position);
            //    }
            //}
        }


        //private async void Main_Loaded(object sender, RoutedEventArgs e)
        //{
            
        //}

        private void CheckNextStep_Btn_Visibility()
        {
            if (SelectedFolder_Record.Folders.Count > 0)
            {
                History.SelectFolder.Nxt_Btn.Visibility = Visibility.Visible;
            }
            else
            {
                History.SelectFolder.Nxt_Btn.Visibility = Visibility.Hidden;
            }
        }

        private void Delete_Icon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
           
            MessageBoxResult result = MessageBox.Show("是否要取消選取 '" + new DirectoryInfo(FolderLocation).Name + "'", "警告", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                //執行取消選取動作
                History.SelectFolder.FolderItem_StackPanel.Children.Remove(this);
                SelectedFolder_Record.Folders.Remove(position);
                CheckNextStep_Btn_Visibility();
            }
            //Old code
            //foreach (Window window in Application.Current.Windows)
            //{
            //    if (window.GetType() == typeof(MainWindow))
            //    {
            //         MainWindow main_window = window as MainWindow;
            //        MessageBoxResult result = MessageBox.Show("是否要取消選取 '" + new DirectoryInfo(FolderLocation).Name + "'", "警告", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            //        if (result == MessageBoxResult.Yes)
            //        {
            //            //執行取消選取動作
            //            main_window.SelectFolder_Control.FolderItem_StackPanel.Children.Remove(this);
            //            SelectedFolder_Record.Folders.Remove(position);
            //            CheckNextStep_Btn_Visibility();
            //        }
            //    }
            //}
        }

        private async void Main_Initialized(object sender, EventArgs e)
        {
            if (Directory.Exists(FolderLocation))
            {
                FolderRecordItem item_to_find = await GetFolderInformation(FolderLocation);
                FolderRecordItem ItemFounded = await Task.Run(() => SelectedFolder_Record.Folders.Find(x => x.FolderLocation == item_to_find.FolderLocation));
                if (ItemFounded != null)
                {
                    MessageBox.Show("已經添加過 '" + new DirectoryInfo(FolderLocation).Name + "' 。", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Console.WriteLine("找到資料");
                    History.SelectFolder.FolderItem_StackPanel.Children.Remove(this);
                    CheckNextStep_Btn_Visibility();
                    //foreach (Window window in Application.Current.Windows)
                    //{
                    //    if (window.GetType() == typeof(MainWindow))
                    //    {
                    //        MainWindow main_window = window as MainWindow;
                    //        main_window.SelectFolder_Control.FolderItem_StackPanel.Children.Remove(this);
                    //        CheckNextStep_Btn_Visibility();
                    //    }
                    //}
                }
                ItemFounded = await Task.Run(() => SelectedFolder_Record.Folders.Find(x => x.MD5 != item_to_find.MD5 && x.FolderLocation == item_to_find.FolderLocation));
                if (ItemFounded != null)
                {
                    Console.WriteLine("資料夾有更新");
                    CheckNextStep_Btn_Visibility();
                    //MessageBox.Show("資料夾有更新");
                }
                ItemFounded = await Task.Run(() => SelectedFolder_Record.Folders.Find(x => x.FolderLocation == item_to_find.FolderLocation));
                if (ItemFounded == null)
                {
                    //MessageBox.Show("資料未被找到");
                    Console.WriteLine("資料未被找到");
                    //foreach (Window window in Application.Current.Windows)
                    //{
                    //    if (window.GetType() == typeof(MainWindow))
                    //    {
                    //        MainWindow main_window = window as MainWindow;
                    //        //Folder_Item Imported_item = new Folder_Item();
                    //        //Imported_item.Height = 75;
                    //        //main_window.SelectFolder_Control.FolderItem_StackPanel.Children.Add(Imported_item);
                    //        Allow_Visible();
                    //        main_window.SelectFolder_Control.FolderItem_StackPanel.Children.Add(data_item);
                    //    }
                    //}
                    SelectedFolder_Record.Folders.Add(item_to_find);
                    position = item_to_find;
                    CheckNextStep_Btn_Visibility();
                }
            }
            //foreach (var item in SelectedFolderRecord.Folders)  
            //{
            //    if (item.MD5 == GetMD5Hash(FolderLocation))
            //    {
            //        MessageBox.Show("已發現相同資料夾要覆蓋嗎?", "警告", MessageBoxButton.YesNo);
            //    }
            //    else if (item.MD5 != GetMD5Hash(FolderLocation) && item.FolderLocation == FolderLocation)
            //    {
            //        MessageBox.Show("已發現相同資料夾，並且此資料夾有新的變更，要使用新的變更嗎?", "警告", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            //    }
            //}
            //if (FolderLocation.Length > 0)
            //{
            //    await GetFolderInformation(FolderLocation);
            //}
        }

        //public Folder_Item(string FolderLocation)
        //{
        //    InitializeComponent();
        //    foreach (Window window in Application.Current.Windows)
        //    {
        //        if(window.GetType() == typeof(SelectFolderPage))
        //        {
        //            (window as SelectFolderPage).FolderItem_StackPanel.Items.Add(FolderLocation);
        //        }
        //    }
        //}
    }
}
