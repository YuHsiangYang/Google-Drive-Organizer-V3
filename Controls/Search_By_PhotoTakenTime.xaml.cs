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
            SelectedDate = new Dictionary<DateTypes, int>();
            SelectedDate.Add(DateTypes.Year, (int)(Year.SelectedValue.ToString() == ApplicationVariables.AllDate && Year.SelectedValue != null ? 0 : int.Parse(Year.SelectedValue.ToString())));
            SelectedDate.Add(DateTypes.Month, (int)(Month.SelectedValue.ToString() == ApplicationVariables.AllDate && Month.SelectedValue != null ? 0 : int.Parse(Month.SelectedValue.ToString())));
            SelectedDate.Add(DateTypes.Day, (int)(Day.SelectedValue.ToString() == ApplicationVariables.AllDate && Day.SelectedValue != null ? 0 : int.Parse(Day.SelectedValue.ToString())));
            SelectedDateChanged?.Invoke(this, SelectedDate);
        }
        public void LoadPhotoTakenTimeComboboxItem(List<ImageExif> Exifs)
        {
            //Get the year from the input
            List<string> year = new List<string>();
            Exifs.ForEach(item => year.Add(item.JsonData.PhotoTakenTime_DateTime.Year.ToString()));
            year = year.Distinct().ToList();
            year.Sort();
            year.Insert(0, ApplicationVariables.AllDate);

            //Get the month from the input
            List<string> month = new List<string>();
            Exifs.ForEach(item => month.Add(item.JsonData.PhotoTakenTime_DateTime.Month.ToString()));
            month = month.Distinct().ToList();
            month.Sort();
            month.Insert(0, ApplicationVariables.AllDate);

            //Get the days from the input
            List<string> day = new List<string>();
            Exifs.ForEach(item => day.Add(item.JsonData.PhotoTakenTime_DateTime.Day.ToString()));
            day = day.Distinct().ToList();
            day.Sort();
            day.Insert(0, ApplicationVariables.AllDate);

            Year.ItemsSource = year;
            Year.SelectedIndex = 0;
            Year.SelectionChanged += DateInput_SelectionChanged;

            Month.ItemsSource = month;
            Month.SelectedIndex = 0;
            Month.SelectionChanged += DateInput_SelectionChanged;

            Day.ItemsSource = day;
            Day.SelectedIndex = 0;
            Day.SelectionChanged += DateInput_SelectionChanged;

            Console.WriteLine(
                "Year count: {0} \n" +
                "Month count: {1} \n" +
                "Day count: {2}", year.Count, month.Count, day.Count);
        }
    }
}
