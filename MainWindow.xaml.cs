using Newtonsoft.Json.Linq;
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
            History.SelectFolder.Select_Folder_Finish += SelectFolder_Control_Select_Folder_Finish;
        }

        private void SelectFolder_Control_Select_Folder_Finish()
        {
            MainWindow_Grid.Children.Clear();
            History.MatchedItem = new Pages.MatchItem.MatchItem_Container();
            History.MatchedItem.NextStepTriggered += MatchedItem_NextStepTriggered;
            MainWindow_Grid.Children.Add(History.MatchedItem);
        }

        private void MatchedItem_NextStepTriggered()
        {
            MainWindow_Grid.Children.Clear();
            MainWindow_Grid.Children.Add(History.OutputDistination);

        }
    }
}
