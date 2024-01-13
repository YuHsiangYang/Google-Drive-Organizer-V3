using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Google_Drive_Organizer_V3
{
    public static class ApplicationVariables
    {
        public static string NoData { get; } = "No data";
        public static int PageSize { get;} = 70;
        public static int PageNumber { get; set; } = 0;
        public static double RowHeight { get; set; } = 0;
        public static double ColumnWidth { get; set; } = 0;
        public static string AllDate { get; } = "All";
    }
}
