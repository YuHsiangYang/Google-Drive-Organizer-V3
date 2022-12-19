using Newtonsoft.Json.Linq;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Google_Drive_Organizer_V3
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainWindow_Grid.Children.Clear();
            MainWindow_Grid.Children.Add(History.SelectFolder);
            History.SelectFolder.Select_Folder_Finish += SelectFolder_Select_Folder_Finish;
        }

        private void SelectFolder_Select_Folder_Finish(object sender, EventArgs e)
        {
            NavController.CanContinue = true;
            NavController.ProceedToPrevious_Event += async delegate
            {
                await ShowSelectFolderPage();
                NavController.ProceedToNext_Event += async delegate
                {
                    await ShowDisplayPanel();
                };
            };
            NavController.ProceedToNext_Event += async delegate
            {
                await ShowDisplayPanel();
            };
        }

        private async Task ShowDisplayPanel()
        {
            GlobalScripts.Disappear_Element(SelectFolder_Control, TimeSpan.FromSeconds(0.2));
            await Task.Delay(TimeSpan.FromSeconds(0.2));
            MainWindow_Grid.Children.Clear();
            Progress<LoadEXIFRecord_ProgressReportModule> progress = new Progress<LoadEXIFRecord_ProgressReportModule>();
            History.DisplayMatchPanel = new Controls.DisplayPanel(await MatchItem_Record.LoadMatch(History.SelectFolder.ImportedFolders, progress));
            //History.MatchedItem.NextStepTriggered += MatchedItem_NextStepTriggered;
            History.DisplayMatchPanel.StageFinished += DisplayMatchPanel_StageFinished;
            MainWindow_Grid.Children.Add(History.DisplayMatchPanel);
            GlobalScripts.Appear_Element(History.DisplayMatchPanel, TimeSpan.FromSeconds(0.2));
            NavController.CanGoBack = true;
            NavController.ProceedToNext_Event = null;
        }

        private async Task ShowSelectFolderPage()
        {
            GlobalScripts.Disappear_Element(MainWindow_Grid.Children[0], TimeSpan.FromSeconds(0.2));
            await Task.Delay(TimeSpan.FromSeconds(0.2));
            MainWindow_Grid.Children.Clear();
            MainWindow_Grid.Children.Add(History.SelectFolder);
            GlobalScripts.Appear_Element(History.SelectFolder, TimeSpan.FromSeconds(0.2));
        }

        private void DisplayMatchPanel_StageFinished(object sender, List<Classes.ImageExif> e)
        {
            NavController.ProceedToNext_Event += delegate
            {
                MainWindow_Grid.Children.Clear();
                MainWindow_Grid.Children.Add(History.OutputDistination);
            };
        }
    }
}
