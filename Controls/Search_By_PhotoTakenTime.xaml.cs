using Google_Drive_Organizer_V3.Classes;
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

namespace Google_Drive_Organizer_V3.Controls
{
    /// <summary>
    /// Search_By_PhotoTakenTime.xaml 的互動邏輯
    /// </summary>
    public partial class Search_By_PhotoTakenTime : UserControl
    {
        public Search_By_PhotoTakenTime()
        {
            InitializeComponent();
        }
        Dictionary<DateTypes, int> SelectedDate = new Dictionary<DateTypes, int>();
        public event EventHandler<Dictionary<DateTypes, int>> SelectedDateChanged;
        private void DateInput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedDate.Add(DateTypes.Year, (int)(Year.SelectedValue.ToString() == "全部" ? 0 : Year.SelectedValue));
            SelectedDate.Add(DateTypes.Month, (int)(Month.SelectedValue.ToString() == "全部" ? 0 : Month.SelectedValue));
            SelectedDate.Add(DateTypes.Day, (int)(Day.SelectedValue.ToString() == "全部" ? 0 : Day.SelectedValue));
            SelectedDateChanged?.Invoke(this, SelectedDate);
        }
    }
}
