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
    public partial class DisplayPages_Control : UserControl
    {
        //Items per page = 30
        int pages = 0;
        public event EventHandler<int> PageChanged;
        public DisplayPages_Control()
        {
            InitializeComponent();
        }
        public void LoadRange(int quantities)
        {
            if ((int)Math.Ceiling((double)quantities / ApplicationVariables.PageSize) > pages)
            {
                PagesPanel.Children.Clear();
                pages = (int)Math.Ceiling((double)quantities / ApplicationVariables.PageSize);
                for (int i = 0; i < pages; i++)
                {
                    
                    Button PageBTN = new Button()
                    {
                        Content = i +1 ,
                        Width = 20,
                        Height = 20,
                        Margin = new Thickness(2.5),
                        BorderBrush = null,
                        Background = Resources["ColorPrimary"] as SolidColorBrush,
                        Foreground = Resources["ColorSecondary"] as SolidColorBrush,
                    };
                    Viewbox BTNViewbox = new Viewbox()
                    {
                        Child = PageBTN,
                    };

                    PageBTN.Click += PageBTN_Click;
                    ButtonCustomProperties.SetCornerRadius(PageBTN, PageBTN.Width / 2);
                    PagesPanel.Children.Add(BTNViewbox);
                }

            }

        }

        private void PageBTN_Click(object sender, RoutedEventArgs e)
        {
            int page = (int)((Button)sender).Content;
            ApplicationVariables.PageNumber = page-1;
            PageChanged?.Invoke(this, page);
        }
    }
}
