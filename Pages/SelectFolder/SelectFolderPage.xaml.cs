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
using System.Windows.Media.Animation;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Diagnostics;
using System.Windows.Shapes;
using Google_Drive_Organizer_V3.Pages.SelectFolder;
using Microsoft.WindowsAPICodePack.Dialogs;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Google_Drive_Organizer_V3.Pages
{
    /// <summary>
    /// SelectFolderPage.xaml 的互動邏輯
    /// </summary>
    public partial class SelectFolderPage : UserControl
    {
        public SelectFolderPage()
        {
            InitializeComponent();
        }

        public event EventHandler Select_Folder_Finish;
        public List<Folder_Item> ImportedFolders { get; set; } = new List<Folder_Item>();
        private void DragTrigger_Grid_Drop(object sender, DragEventArgs e)
        {
            List<string> directory_imported = ((string[])e.Data.GetData(DataFormats.FileDrop, false)).ToList();
            List<string> valid_imports = new List<string>();
            List<string> invalid_import = new List<string>();
            string message = "";
            foreach (string sub_directory in directory_imported)
            {
                FileAttributes attribute = File.GetAttributes(sub_directory);
                if (attribute.HasFlag(FileAttributes.Directory))
                {
                    valid_imports.Add(sub_directory);
                }
                else
                {
                    invalid_import.Add(System.IO.Path.GetFileName(sub_directory));
                    message += System.IO.Path.GetFileName(sub_directory) + Environment.NewLine;
                }
            }
            if (invalid_import.Count > 0)
            {
                MessageBox.Show("無法添加 " + Environment.NewLine + message + "因為" + (invalid_import.Count > 1 ? "它們" : "它") + "不是資料夾", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            ImportFolders(valid_imports);
            DragTrigger_Grid_DragLeave(sender, e);
        }
        private void Select_Folder_Btn_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            dialog.InitialDirectory = @"C:\Users\" + Environment.UserName + "Documents";
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                ImportFolders(new List<string> { dialog.FileName });
                //Select_Folder_Finish?.Invoke(this, e);
            }
        }

        private void Folder_Item_Loaded(object sender, Folder_Item e)
        {
            ImportedFolders.Add(e);
            CheckFolderCount();
        }

        private void FolderDeleted(object sender, Folder_Item e)
        {
            ImportedFolders.Remove(e);
            FolderItem_StackPanel.Children.Remove(e);
            CheckFolderCount();
        }

        private async void Drop_Container_DragEnter(object sender, DragEventArgs e)
        {
            GlobalScripts.Disappear_Element(stackPanel, TimeSpan.FromSeconds(.2));
            stackPanel.Visibility = Visibility.Hidden;
            await Task.Delay(TimeSpan.FromSeconds(.1));
            dragEnterIcon.Name = "DragEnterIcon";
            Drop_Area_Grid.Children.Insert(1, dragEnterIcon);
            Grid.SetColumn(dragEnterIcon, 1);
            GlobalScripts.Appear_Element(dragEnterIcon, TimeSpan.FromSeconds(.15));
        }
        Controls.DragEnterIcon_With_Animation dragEnterIcon = new Controls.DragEnterIcon_With_Animation()
        {
            Height = 200,
        };

        private async void DragTrigger_Grid_DragLeave(object sender, DragEventArgs e)
        {
            GlobalScripts.Disappear_Element(dragEnterIcon, TimeSpan.FromSeconds(.2));
            await Task.Delay(TimeSpan.FromSeconds(.1));
            Drop_Area_Grid.Children.Remove(dragEnterIcon);
            stackPanel.Visibility = Visibility.Visible;
            GlobalScripts.Appear_Element(stackPanel, TimeSpan.FromSeconds(.2));
        }
        private void CheckFolderCount()
        {
            if (ImportedFolders.Count() > 0)
            {
                Select_Folder_Finish?.Invoke(this, new EventArgs());
            }
            else if (ImportedFolders.Count() == 0)
            {
                //NavController.CanContinue = false;
            }
        }
        public void ImportFolders(List<string> paths)
        {
            foreach (string path in paths)
            {
                if (ImportedFolders.FindAll(item => item.FolderPath == path).Count == 0)
                {
                    Folder_Item folder = new Folder_Item(path);
                    folder.Height = 75;
                    folder.Loaded += Folder_Item_Loaded;
                    folder.Folde_Item_DeleteEvent += FolderDeleted;
                    FolderItem_StackPanel.Children.Add(folder);
                }
                else
                {
                    MessageBox.Show("已經添加過 '" + new DirectoryInfo(path).Name + "' 。", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Console.WriteLine("找到資料");
                }
            }
        }
    }
}
