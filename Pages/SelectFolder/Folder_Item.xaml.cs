using System;
using System.Collections.Generic;
using System.Linq;
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
using static Google_Drive_Organizer_V3.GlobalScripts;
using System.Windows.Media.Animation;

namespace Google_Drive_Organizer_V3.Pages.SelectFolder
{
    /// <summary>
    /// Folder_Item.xaml 的互動邏輯
    /// </summary>
    public partial class Folder_Item : UserControl
    {
        public string FolderPath = "";
        public int JsonCount { get; private set; } = 0;
        public int ImageCount { get; private set; } = 0;
        public event EventHandler<Folder_Item> Folde_Item_DeleteEvent;
        public event EventHandler<Folder_Item> Loaded;
        public Folder_Item()
        {
            InitializeComponent();
        }
        private async void Main_Initialized(object sender, EventArgs e)
        {
            await GetFolderInformation(FolderPath);
            Loaded?.Invoke(this, this);
        }
        public Folder_Item(string folder_location)
        {
            Initialized += Main_Initialized;
            FolderPath = folder_location;
            InitializeComponent();
        }

        private async Task GetFolderInformation(string folder)
        {
            FolderName.Content = await Task.Run(() => new DirectoryInfo(folder).Name);
            JsonCount = await Task.Run(() => Directory.GetFiles(folder, "*.json", SearchOption.AllDirectories).Count());
            Progress.percentage = .5;
            JsonCount_Label.Content = JsonCount.ToString();
            ImageCount = await Task.Run(() => Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories).Where(file => file.EndsWith(".jpeg") || file.EndsWith(".jpg")).Count());
            ImageCount_Label.Content = ImageCount.ToString();
            Progress.percentage = 1;
        }


        private void Delete_Icon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to deselect '" + new DirectoryInfo(FolderPath).Name + "'?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                //執行取消選取動作
                Folde_Item_DeleteEvent?.Invoke(this, this);
            }
        }
    }
}
