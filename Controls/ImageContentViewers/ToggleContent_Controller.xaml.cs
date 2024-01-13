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
    /// This component is to switch between JSON data and the metadata of the image.
    /// </summary>

    public partial class ToggleContent_Controller : UserControl
    {
        //Set the position of the highlighting element, the black one.
        private Thickness JsonContent = new Thickness(0, 0, 0, 0);
        private Thickness EXIFContent = new Thickness(50, 0, 0, 0);
        public ContentType PreviousContentType = ContentType.EXIF; //This is used as the reference for the highlighting element above.
        private Dictionary<ContentType, Thickness> animation_dictionary = new Dictionary<ContentType, Thickness>(); //This dictionary stores 2 types of animation (from EXIF to JSON, from JSON to EXIF)
        public event EventHandler<ContentType> ContentType_Changed; //Evoked when the user clicks on one of the two types.

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

            //Change the font weight (make it bold or regular) as the display type (EXIF, JSON) changes.
            (MainControl.FindName(PreviousContentType.ToString() + "Label") as Label).FontWeight = FontWeights.Regular;
            (MainControl.FindName(PreviousContentType.ToString() + "Label") as Label).Foreground = Resources["ColorPrimary"] as SolidColorBrush;
            (MainControl.FindName(CurrentContentType.ToString() + "Label") as Label).FontWeight = FontWeights.Bold;
            (MainControl.FindName(CurrentContentType.ToString() + "Label") as Label).Foreground = Resources["ColorSecondary"] as SolidColorBrush;

            if (PreviousContentType != CurrentContentType)
            {
                ContentType_Changed?.Invoke(this, CurrentContentType);//Trigger the eventhandler when the user clicks on the other type.
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
