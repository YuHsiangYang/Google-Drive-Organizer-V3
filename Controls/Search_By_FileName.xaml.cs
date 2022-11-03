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
using System.Windows.Media.Animation;
using System.Threading;

namespace Google_Drive_Organizer_V3.Controls
{
    /// <summary>
    /// Search_By_FileName.xaml 的互動邏輯
    /// </summary>
    public partial class Search_By_FileName : UserControl
    {
        public CancellationTokenSource CancellationToken { get; set; } = new CancellationTokenSource();
        public event EventHandler<string> SearchFileName;
        public Search_By_FileName()
        {
            InitializeComponent();
            Grid.SetColumn(ClearSearch, 1);
            ClearSearch = Resources["Clear"] as Button;
            //ClearSearch.Template = ApplicationResource["CustomButtonTemplate"] as ControlTemplate;
            ClearSearch.ApplyTemplate();
            ButtonCustomProperties.SetCornerRadius(ClearSearch, 10);
            ClearSearch.Click += ClearSearch_Click;
        }

        //ResourceDictionary ApplicationResource = new ResourceDictionary()
        //{
        //    Source = new Uri(@"./ApplicationResources.xaml", UriKind.Relative)
        //};

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            //List<Pages.MatchItem.MatchItem_Child> matched = 
            SearchFileName?.Invoke(this, Input.Text);
        }

        private void ClearSearch_Click(object sender, RoutedEventArgs e)
        {
            Input.Text = "";
        }

        public Button ClearSearch { get; set; } = new Button()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Margin = new Thickness(0),
            Content = "清除搜尋",
            Width = 70,
            Cursor = Cursors.Hand,
            BorderBrush = null,
            Background = new SolidColorBrush(Color.FromArgb(100, 255, 159, 159))
        };
    private async void Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            //ClearSearch.Width = ClearSearch.RenderSize.Width;
            TimeSpan duration = TimeSpan.FromSeconds(.2);
            try
            {
                if (Input.Text.Length > 0)
                {
                    ClearSearch_PlaceHolder.Children.Add(ClearSearch);
                    GlobalScripts.Appear_Element(ClearSearch, duration);
                    ClearSearch_PlaceHolder.UpdateLayout();
                    DoubleAnimation clearsearch_grid_animation = new DoubleAnimation(0, ClearSearch.Width, duration);
                    ClearSearch_PlaceHolder.BeginAnimation(WidthProperty, clearsearch_grid_animation);
                }
                else
                {
                    SearchFileName(this, Input.Text);
                    GlobalScripts.Disappear_Element(ClearSearch, duration);
                    DoubleAnimation clearsearch_grid_animation = new DoubleAnimation(ClearSearch.Width, 0, duration);
                    ClearSearch_PlaceHolder.BeginAnimation(WidthProperty, clearsearch_grid_animation);
                    await Task.Delay(duration);
                    ClearSearch_PlaceHolder.Children.Remove(ClearSearch);
                    ClearSearch_PlaceHolder.UpdateLayout();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ClearSearch_PlaceHolder.Children.Contains(ClearSearch));
            }

        }

        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter && Input.Text.Length != 0)
            {
                SearchFileName(this, Input.Text);
            }
        }
    }
}
