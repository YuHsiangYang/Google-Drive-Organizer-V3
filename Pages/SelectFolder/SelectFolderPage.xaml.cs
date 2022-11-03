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
            Nxt_Btn.Visibility = Visibility.Hidden;
            //var temp = (Button)template;
        }
        public event Action Select_Folder_Finish;
        private void Select_Folder_Btn_Click(object sender, RoutedEventArgs e)
        {
            var template = Nxt_Btn.Template;
            Border d = (Border)Nxt_Btn.Template.FindName("Front", Nxt_Btn);
            //d.CornerRadius = new CornerRadius(10);
            //Nxt_Btn.UpdateDefaultStyle();
            //Nxt_Btn.UpdateLayout();
            //Progress_Bar.percentage += 0.1;
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            dialog.InitialDirectory = @"C:\Users\" + Environment.UserName + "Yu - Hsiang Folder";
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                //if(place_holder != null)
                //{
                //    FolderItem_StackPanel.Children.Clear();
                //}
                Folder_Item folder = new Folder_Item(dialog.FileName);
                folder.Height = 75;
                FolderItem_StackPanel.Children.Add(folder);
                //MessageBox.Show(dialog.FileName);
            }
        }

        private void MainWindow_Initialized(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Nxt_Btn_Click(object sender, RoutedEventArgs e)
        {
            Select_Folder_Finish();
        }

        //Canvas DragImage = new Canvas();

        private void Main_Grid_MouseMove(object sender, MouseEventArgs e)
        {
            //Thickness margin = new Thickness(e.GetPosition(Main_Grid).X, e.GetPosition(Main_Grid).Y, Main_Grid.ActualHeight - e.GetPosition(Main_Grid).X, Main_Grid.ActualHeight - e.GetPosition(Main_Grid).Y);
            //DragImage.Margin = margin;
            //Console.WriteLine("Mouse position" + e.GetPosition(Main_Grid).X);

        }
        private async void Drop_Container_DragEnter(object sender, DragEventArgs e)
        {
            //if (Main_Grid.Children.Contains(DragImage) == false)
            //{

            //    BitmapImage image = new BitmapImage(new Uri(@".\icons\Drag\folder.png", UriKind.Relative));
            //    ImageBrush imageBrush = new ImageBrush(image);
            //    DragImage.Background = imageBrush;
            //    DragImage.Height = 100;
            //    DragImage.Width = 100;
            //    Grid.SetColumnSpan(DragImage, 2);
            //    Grid.SetRowSpan(DragImage, 2);
            //    Main_Grid.Children.Add(DragImage);
            //}
            //Main_Grid.MouseMove += Main_Grid_MouseMove;
            //DoubleAnimation animation = new DoubleAnimation(0, 100, new Duration(TimeSpan.FromSeconds(.2)));
            //TranslateTransform transform = new TranslateTransform();
            //stackPanel.RenderTransform = transform;
            //transform.BeginAnimation(TranslateTransform.YProperty, animation);
            //Storyboard.SetTargetProperty(animation, new PropertyPath(TranslateTransform.YProperty));
            //Storyboard.SetTarget(animation, stackPanel);
            //Storyboard storyboard = new Storyboard();
            //storyboard.Children.Add(animation);
            //storyboard.Duration = new Duration(TimeSpan.FromSeconds(.2));
            //storyboard.Begin();
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

        private void DragTrigger_Grid_Drop(object sender, DragEventArgs e)
        {
            List<string> directory_imported = ((string[])e.Data.GetData(DataFormats.FileDrop, false)).ToList();
            List<string> valid_import = new List<string>();
            List<string> invalid_import = new List<string>();
            string message = "";
            foreach (string sub_directory in directory_imported)
            {
                FileAttributes attribute = File.GetAttributes(sub_directory);
                if (attribute.HasFlag(FileAttributes.Directory))
                {
                    valid_import.Add(sub_directory);
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
            foreach (string folder_location in valid_import)
            {
                Folder_Item folder_item = new Folder_Item(folder_location);
                folder_item.Height = 75;
                FolderItem_StackPanel.Children.Add(folder_item);
            }
            DragTrigger_Grid_DragLeave(sender, e);
        }
    }
}
