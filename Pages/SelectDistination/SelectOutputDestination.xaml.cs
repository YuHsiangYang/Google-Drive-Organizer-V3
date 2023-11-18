using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Google_Drive_Organizer_V3.Classes;
using Google_Drive_Organizer_V3.Pages.SelectFolder;
using Google_Drive_Organizer_V3.Scripts;
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
        string output_dir = "";
        private async void StartProcess_Click(object sender, RoutedEventArgs e)
        {
            if (output_dir.Length > 0)
            {
                string log = "";
                List<ImageExif> exif_data = History.DisplayMatchPanel.CompleteExifs;
                MergingProgress.Visibility = Visibility.Visible;
                GlobalScripts.Appear_Element(MergingProgress, TimeSpan.FromSeconds(0.2));
                //Time how long will it take for the process to finish
                Stopwatch timer = new Stopwatch();
                timer.Start();
                for (int i = 0; i < exif_data.Count; i++)
                {
                    await ModifyEXIFData.Modify(exif_data[i], output_dir);
                    log += string.Format("Image: '{0}' is processed and copied to {1} at {2}\n", exif_data[i].EXIFData.ImagePath, output_dir, DateTime.Now.ToString("yyyy:MM:dd HH:mm:ss"));
                    log_panel.Text = log;
                    log_panel_textBox.ScrollToEnd();
                    MergingProgress.percentage = Convert.ToDouble(i + 1) / Convert.ToDouble(exif_data.Count);
                }
                log += string.Format("Completed at {0}\n\n", DateTime.Now.ToString("yyyy:MM:dd HH:mm:ss"));

                timer.Stop();
                log += string.Format("Total time used: {0} \nTotal Images processed: {1}", timer.Elapsed, exif_data.Count);
                log_panel.Text = log;


                //write the log to a txt file in the output 
                string file_name = System.IO.Path.Combine(output_dir, string.Format("{0} log file.txt", DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")));
                using (StreamWriter log_file = new StreamWriter(file_name))
                {
                    log_file.WriteLine(log);
                }
            }
            else
            {
                MessageBox.Show("Please select an output destination", "Warning",MessageBoxButton.OK, MessageBoxImage.Warning);
            }
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
                output_dir = selectdestination_dialog.FileName;
                SelectedDestination_label.Content = output_dir;
            }
        }
    }
}
