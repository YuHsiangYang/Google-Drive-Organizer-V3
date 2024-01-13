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
    /// Use combobox to search the images based on the take time retrieved from the JSON files.
    /// </summary>
    public partial class Search_By_PhotoTakenTime : UserControl
    {
        public Search_By_PhotoTakenTime()
        {
            //Add the option to search for all dates
            InitializeComponent();
            year.Add(ApplicationVariables.AllDate);
            Year.SelectedIndex = 0;

            month.Add(ApplicationVariables.AllDate);
            Month.SelectedIndex = 0;


            day.Add(ApplicationVariables.AllDate);
            Day.SelectedIndex = 0;
        }
        List<string> year = new List<string>();
        List<string> month = new List<string>();
        List<string> day = new List<string>();
        Dictionary<DateTypes, int> SelectedDate = new Dictionary<DateTypes, int>();
        public event EventHandler<Dictionary<DateTypes, int>> SelectedDateChanged;
        private void DateInput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //convert the "all date" text to the format that is readable by the program. 0 if all date is selected. Else if the other dates is selected.
            SelectedDate = new Dictionary<DateTypes, int>
            {
                { DateTypes.Year, (int)(Year.SelectedValue.ToString() == ApplicationVariables.AllDate && Year.SelectedValue != null ? 0 : int.Parse(Year.SelectedValue.ToString())) },
                { DateTypes.Month, (int)(Month.SelectedValue.ToString() == ApplicationVariables.AllDate && Month.SelectedValue != null ? 0 : int.Parse(Month.SelectedValue.ToString())) },
                { DateTypes.Day, (int)(Day.SelectedValue.ToString() == ApplicationVariables.AllDate && Day.SelectedValue != null ? 0 : int.Parse(Day.SelectedValue.ToString())) }
            };
            SelectedDateChanged?.Invoke(this, SelectedDate);
        }
        public async Task LoadPhotoTakenTimeComboboxItem(List<ImageExif> Exifs)
        {


            await Task.Run(() =>
            {
                //Get the year from the input
                Exifs.ForEach(item => year.Add(item.JsonData.PhotoTakenTime_DateTime.Year.ToString()));
                year = year.Distinct().ToList();
                year.Sort();
                year.Reverse();

                //Get the month from the input
                Exifs.ForEach(item => month.Add(item.JsonData.PhotoTakenTime_DateTime.Month.ToString()));
                month = month.Distinct().ToList();
                month.Sort();
                month.Reverse();

                //Get the days from the input
                Exifs.ForEach(item => day.Add(item.JsonData.PhotoTakenTime_DateTime.Day.ToString()));
                day = day.Distinct().ToList();
                day.Sort();
                day.Reverse();
            });


            Year.ItemsSource = year;
            Year.SelectionChanged += DateInput_SelectionChanged;

            Month.ItemsSource = month;
            Month.SelectionChanged += DateInput_SelectionChanged;

            Day.ItemsSource = day;
            Day.SelectionChanged += DateInput_SelectionChanged;

            Console.WriteLine(
                        "Year count: {0} \n" +
                        "Month count: {1} \n" +
                        "Day count: {2}", year.Count, month.Count, day.Count);
        }
    }
}
