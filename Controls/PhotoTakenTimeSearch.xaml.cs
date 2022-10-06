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

namespace Google_Drive_Organizer_V3.Pages.MatchItem
{
    /// <summary>
    /// Search_Date_UserControl.xaml 的互動邏輯
    /// </summary>
    public partial class Search_Date_UserControl : UserControl
    {
        public Search_Date_UserControl()
        {
            InitializeComponent();
        }
        public event Action<Dictionary<string, string>> Input_Changed;
        public string[] Inputs { get; set; } = { "全部", "全部", "全部" };

        private void Search_Input_Year_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Dictionary<string, string> inputs = new Dictionary<string, string>();
            if (Search_Input_Year.Items.Count > 0 && Search_Input_Month.Items.Count > 0 && Search_Input_Day.Items.Count > 0)
            {

                try
                {
                    inputs.Add(Search_Input_Year.Name, Search_Input_Year.SelectedItem.ToString());
                    inputs.Add(Search_Input_Month.Name, Search_Input_Month.SelectedItem.ToString());
                    inputs.Add(Search_Input_Day.Name, Search_Input_Day.SelectedItem.ToString());
                    Input_Changed(inputs);
                }
                catch (System.NullReferenceException)
                {
                    Console.WriteLine("Loading...");
                }
            }
        }
    }
}
