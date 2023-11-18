using Google_Drive_Organizer_V3.Pages.SelectFolder;
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

namespace Google_Drive_Organizer_V3.Pages
{
    /// <summary>
    /// StageNavigationController.xaml 的互動邏輯
    /// </summary>
    public partial class StageNavigationController : UserControl
    {
        public StageNavigationController()
        {
            InitializeComponent();
            RefreshNavigationStatus();
        }
        public EventHandler ProceedToNext_Event;
        public EventHandler ProceedToPrevious_Event;
        public UIElement PreviousPage { get; set; } = new UIElement();
        public UIElement NextPage { get; set; } = new UIElement();
        private bool cancontinue { get; set; } = false;
        private bool cangoback { get; set; } = false;
        public bool CanGoBack
        {
            get
            {
                return cangoback;
            }
            set
            {
                cangoback = value;
                RefreshNavigationStatus();
            }
        }

        public bool CanContinue
        {
            get { return cancontinue; }
            set
            {
                cancontinue = value;
                RefreshNavigationStatus();
            }
        }

        //Variables for the next and previous step
        private async void RefreshNavigationStatus()
        {
            switch (cancontinue)
            {
                case true:
                    Go_Next.Visibility = Visibility.Visible;
                    GlobalScripts.Appear_Element(Go_Next, TimeSpan.FromSeconds(0.2));
                    break;
                case false:
                    GlobalScripts.Disappear_Element(Go_Next, TimeSpan.FromSeconds(0.2));
                    await Task.Delay(TimeSpan.FromSeconds(0.2));
                    Go_Next.Visibility = Visibility.Hidden;
                    break;
                default:
                    break;
            }
            switch (cangoback)
            {
                case true:
                    Go_Previous.Visibility = Visibility.Visible;
                    GlobalScripts.Appear_Element(Go_Previous, TimeSpan.FromSeconds(0.2));
                    break;
                case false:
                    GlobalScripts.Disappear_Element(Go_Previous, TimeSpan.FromSeconds(0.2));
                    await Task.Delay(TimeSpan.FromSeconds(0.2));
                    Go_Previous.Visibility = Visibility.Hidden;
                    break;
                default:
                    break;
            }
        }
        private void StageButtons_Click(object sender, RoutedEventArgs e)
        {
            Button clicked_btn = sender as Button;
            if (clicked_btn.Name.ToString() == "Go_Next")
            {
                ProceedToNext_Event?.Invoke(this, e);
            }
            else if (clicked_btn.Name.ToString() == "Go_Previous")
            {
                ProceedToPrevious_Event?.Invoke(this, e);
            }
        }
    }
}
