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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Google_Drive_Organizer_V3.Controls
{
    /// <summary>
    /// DragEnterIcon_With_Animation.xaml 的互動邏輯
    /// </summary>
    public partial class DragEnterIcon_With_Animation : UserControl
    {
        public DragEnterIcon_With_Animation()
        {
            InitializeComponent();
        }

        private void Grid_Initialized(object sender, EventArgs e)
        {
            BeginStoryboard(Resources["Animation"] as Storyboard);
            BeginStoryboard(Resources["TextAnimation"] as Storyboard);
        }
    }
}
