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

namespace Google_Drive_Organizer_V3.Controls.SortController
{
    /// <summary>
    /// Controlles the sorting type of the images (by file name or photo taken date)
    /// </summary>
    public partial class MainSortController : UserControl
    {
        public event EventHandler<Sort> SortChanged;
        public MainSortController()
        {
            Initialized += MainSortController_Initialized;
            InitializeComponent();
        }

        private void MainSortController_Initialized(object sender, EventArgs e)
        {
            SortTypeController.ItemsSource = Enum.GetValues(typeof(SortTypes)).Cast<SortTypes>();
            SortTypeController.SelectedIndex = 0;
            SortTypeController.SelectionChanged += delegate
            {
                SortChanged?.Invoke(this, new Sort()
                {
                    SortType = (SortTypes)Enum.Parse(typeof(SortTypes), SortTypeController.SelectedItem.ToString()),//This is the combobox control so parsing seleteditem is needed
                    SortManner = SortMannerController.current_manner
                });
            };

            SortMannerController.SortMannerChanged_Event += delegate
            {
                SortChanged?.Invoke(this, new Sort()
                {
                    SortType = (SortTypes)Enum.Parse(typeof(SortTypes), SortTypeController.SelectedItem.ToString()),//This is the combobox control so parsing seleteditem is needed
                    SortManner = SortMannerController.current_manner
                });
            };
        }
    }
    public class Sort
    {
        public SortTypes SortType = new SortTypes();
        public SortManner SortManner = new SortManner();
    }
}
