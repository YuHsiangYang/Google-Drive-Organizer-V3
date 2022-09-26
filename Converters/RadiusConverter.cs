using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Google_Drive_Organizer_V3.Converters
{
    internal class RadiusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Console.WriteLine((double)parameter);
            try
            {
                if (parameter != null)
                {

                    Console.WriteLine((double)parameter);
                    return (double)value * .5;
                }
                else
                {
                    //Console.WriteLine("None");
                    return 0;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("NO");
                return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
