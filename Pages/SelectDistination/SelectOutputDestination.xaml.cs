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
using Google_Drive_Organizer_V3.Pages.SelectFolder;
using Microsoft.WindowsAPICodePack.Dialogs;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Google_Drive_Organizer_V3.Pages.SelectDistination
{
    /// <summary>
    /// SelectOutputDistination.xaml 的互動邏輯
    /// </summary>
    public partial class SelectOutputDistination : UserControl
    {
        public SelectOutputDistination()
        {
            InitializeComponent();
        }

        private void StartProcess_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void SelectedDestination_label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CommonOpenFileDialog selectdestination_dialog = new CommonOpenFileDialog()
            {
                IsFolderPicker = true,
                InitialDirectory = @"C:\Users\" + Environment.UserName + "Yu - Hsiang Folder",
            };
            if (selectdestination_dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string destination = selectdestination_dialog.FileName;
                SelectedDestination_label.Content = destination;
            }
        }
    }
}
