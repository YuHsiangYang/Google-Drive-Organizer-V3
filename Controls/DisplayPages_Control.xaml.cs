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
        int total_pages = 0;//This is the inital value of the total pages.
        public event EventHandler<int> PageChanged;//This event will need to be catched by the parent container
        public DisplayPages_Control()
        {
            InitializeComponent();
        }
        public void LoadRange(int quantities)
        {
            ///This method is to load the page navigation buttons based on the specified number of images per page.
            ///When the number of images exceed the limit, the remaining images will be displayed on the next page.
            if ((int)Math.Ceiling((double)quantities / ApplicationVariables.PageSize) > total_pages)
            {
                PagesPanel.Children.Clear();//Clear all the images on the page
                total_pages = (int)Math.Ceiling((double)quantities / ApplicationVariables.PageSize);//Get the total number of pages needed by rounding up
                for (int current_page = 0; current_page < total_pages; current_page++)
                {
                    
                    Button PageBTN = new Button()
                    {
                        Content = current_page +1 ,
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
            ApplicationVariables.PageNumber = page-1;//The minus one is to fit the indexing of the record. The record is 0-based.
            PageChanged?.Invoke(this, page); //Trigger the page change event.
        }
    }
}
