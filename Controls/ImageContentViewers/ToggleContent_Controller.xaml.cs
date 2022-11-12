using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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

namespace Google_Drive_Organizer_V3.Controls.ImageContentViewers
{
    /// <summary>
    /// ToggleContent_Controller.xaml 的互動邏輯
    /// </summary>

    public partial class ToggleContent_Controller : UserControl
    {
        private Thickness JsonContent = new Thickness(0, 0, 0, 0);
        private Thickness EXIFContent = new Thickness(50, 0, 0, 0);
        public ContentType PreviousContentType;
        private Dictionary<ContentType, Thickness> animation_dictionary = new Dictionary<ContentType, Thickness>();

        public event EventHandler<ContentType> ContentType_Changed;

        public ToggleContent_Controller()
        {
            Initialized += ToggleContent_Controller_Initialized;
            InitializeComponent();
            animation_dictionary.Add(ContentType.Json, JsonContent);
            animation_dictionary.Add(ContentType.EXIF, EXIFContent);

        }

        private void ToggleContent_Controller_Initialized(object sender, EventArgs e)
        {
        }

        private void ContentToggled(object sender, RoutedEventArgs e)
        {
            Enum.TryParse((sender as Rectangle).Name.ToString(), out ContentType CurrentContentType);
            ThicknessAnimation selection_animation = new ThicknessAnimation()
            {
                From = animation_dictionary[PreviousContentType],
                To = animation_dictionary[CurrentContentType],
                Duration = TimeSpan.FromSeconds(.2),
            };
            ContentSelection_Rectangle.BeginAnimation(MarginProperty, selection_animation);
            (MainControl.FindName(PreviousContentType.ToString() + "Label") as Label).FontWeight = FontWeights.Regular;
            (MainControl.FindName(CurrentContentType.ToString() + "Label") as Label).FontWeight = FontWeights.Bold;

            if (PreviousContentType != CurrentContentType)
            {
                ContentType_Changed?.Invoke(this, CurrentContentType);
            }
            PreviousContentType = CurrentContentType;
        }
    }

    public enum ContentType
    {
        Json,
        EXIF,
    }
}
