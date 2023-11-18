using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using Google_Drive_Organizer_V3.Pages.MatchItem;
using System.Security.Cryptography;
using System.IO;
using SharpVectors.Scripting;

namespace Google_Drive_Organizer_V3
{
    public static class GlobalScripts
    {
        private static double scale_ratio = .75;

        //To make the element disappear by using scale and transparency
        public static void Disappear_Element(UIElement uIElement, Duration animation_duration)
        {
            DoubleAnimation disappear_scale = new DoubleAnimation(1, scale_ratio, animation_duration);
            DoubleAnimation disappear_opacity = new DoubleAnimation(1, 0, animation_duration);
            ScaleTransform transform = new ScaleTransform();
            uIElement.RenderTransform = transform; //Turn the transform into a property that can be animated
            uIElement.RenderTransformOrigin = new Point(0.5, 0.5); //Sets the Trnasform origin (pivot point of the scale animation)

            //Begin the animation.
            transform.BeginAnimation(ScaleTransform.ScaleXProperty, disappear_scale);
            transform.BeginAnimation(ScaleTransform.ScaleYProperty, disappear_scale);
            uIElement.BeginAnimation(UIElement.OpacityProperty, disappear_opacity);
        }
        public static void Appear_Element(UIElement uIElement, Duration animation_duration)
        {
            DoubleAnimation appear_scale = new DoubleAnimation(scale_ratio, 1, animation_duration);
            DoubleAnimation appear_opacity = new DoubleAnimation(0, 1, animation_duration);
            ScaleTransform transform = new ScaleTransform();
            uIElement.RenderTransform = transform;
            uIElement.RenderTransformOrigin = new Point(0.5, 0.5);
            transform.BeginAnimation(ScaleTransform.ScaleXProperty, appear_scale);
            transform.BeginAnimation(ScaleTransform.ScaleYProperty, appear_scale);
            uIElement.BeginAnimation(UIElement.OpacityProperty, appear_opacity);
        }
        //Swipe the chosen control to switch to another. This is for animation purpose
        private static void SwipeGeneric(UIElement uIElement, Duration duration, SwipeDirection direction, bool to_origin)
        {
            double displacement = 20;
            displacement = direction == SwipeDirection.LeftToRight ? displacement : -displacement; //Sets the displacement to either to the right or to the left.
            DoubleAnimation displacement_animation = new DoubleAnimation(displacement, duration);
            //Checks if it is from the other place to the origin or from the origin to the other place.
            if (to_origin)
            {
                displacement_animation.From = displacement;
                displacement_animation.To = 0;
            }


            TranslateTransform translateTransform = new TranslateTransform();
            uIElement.RenderTransform = translateTransform;
            translateTransform.BeginAnimation(TranslateTransform.XProperty, displacement_animation);
        }
        public static void SwipeTransition(UIElement uIElement, Duration duration, SwipeDirection direction, TransitionType transitionType)
        {
            DoubleAnimation opacity_animation = new DoubleAnimation();

            switch (transitionType)
            {
                case TransitionType.Appear:
                    SwipeGeneric(uIElement, duration, direction, true);
                    opacity_animation = new DoubleAnimation(0, 1, duration);
                    break;
                case TransitionType.Disappear:
                    SwipeGeneric(uIElement, duration, direction, false);
                    opacity_animation = new DoubleAnimation(1, 0, duration);
                    break;
                default:
                    break;
            }
            //Set up the opacity animation
            uIElement.BeginAnimation(UIElement.OpacityProperty, opacity_animation);
        }
        public enum SwipeDirection
        {
            LeftToRight,
            RightToLeft
        }

        public enum TransitionType
        {
            Appear,
            Disappear
        }
        public static string CalculateMD5(MD5Type Type, string InputValue)
        {
            string output = "";
            if (Type == MD5Type.Folder)
            {
                Console.WriteLine("Folder");
                output = CalculateFolderMD5(InputValue);
            }
            else if (Type == MD5Type.File)
            {
                Console.WriteLine("File MD5");
                output = CalculateFileMD5(InputValue);
            }
            return output;
        }
        public static string CalculateFileMD5(string filename)
        {
            string output = "";
            var hash = MD5.Create().ComputeHash(File.OpenRead(filename));
            output = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            return output;
        }
        public static string CalculateFolderMD5(string FolderLocation)
        {
            // assuming you want to include nested folders
            var files = Directory.GetFiles(FolderLocation, "*.*", SearchOption.AllDirectories)
                                 .OrderBy(p => p).ToList();

            MD5 md5 = MD5.Create();

            for (int i = 0; i < files.Count; i++)
            {
                string file = files[i];

                // hash path
                string relativePath = file.Substring(FolderLocation.Length + 1);
                byte[] pathBytes = Encoding.UTF8.GetBytes(relativePath.ToLower());
                md5.TransformBlock(pathBytes, 0, pathBytes.Length, pathBytes, 0);

                // hash contents
                byte[] contentBytes = File.ReadAllBytes(file);
                if (i == files.Count - 1)
                    md5.TransformFinalBlock(contentBytes, 0, contentBytes.Length);
                else
                    md5.TransformBlock(contentBytes, 0, contentBytes.Length, contentBytes, 0);
            }

            return BitConverter.ToString(md5.Hash).Replace("-", "").ToLower();
        }
        public static async Task<int> CountJson(string FolderLocation)
        {
            int count = 0;
            if (Directory.Exists(FolderLocation))
            {
                count = await Task.Run(() => Directory.GetFiles(FolderLocation, "*.json", SearchOption.AllDirectories).Count());
            }
            else
            {
                count = -1;
            }
            return count;
        }

    }
    [Flags]
    public enum MD5Type
    {
        Folder,
        File
    }
}
