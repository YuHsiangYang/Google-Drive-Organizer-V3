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
    /// DisplayRange.xaml 的互動邏輯
    /// </summary>
    public partial class DisplayRange : UserControl
    {
        //Items per page = 30
        int pages = 0;
        public event Action<int> PageChanged;
        public DisplayRange()
        {
            InitializeComponent();
        }
        public void LoadRange(int quantities)
        {
            DisplayPanel.Children.Clear();
            pages = (int)Math.Ceiling((double)quantities / 30);
            int total = pages++;
            for (int i = 1; i < total; i++)
            {
                Button PageBTN = new Button()
                {
                    Content = i,
                    Width = 15,
                    Height = 15,
                    Margin = new Thickness(2.5)
                };
                PageBTN.Click += PageBTN_Click;
                ButtonCustomProperties.SetCornerRadius(PageBTN, 7.5);
                DisplayPanel.Children.Add(PageBTN);
            }

        }

        private void PageBTN_Click(object sender, RoutedEventArgs e)
        {
            int page = (int)((Button)sender).Content;
            PageChanged(page);
            Properties.Settings.Default["page_number"] = page;
        }
    }
}
