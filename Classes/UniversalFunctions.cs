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

namespace Google_Drive_Organizer_V3
{
    public static class UniversalFunctions
    {
        public static void Disappear_Element(UIElement uIElement, Duration animation_duration)
        {
            DoubleAnimation disappear_scale = new DoubleAnimation(1, 0.5, animation_duration);
            DoubleAnimation disappear_opacity = new DoubleAnimation(1, 0, animation_duration);
            ScaleTransform transform = new ScaleTransform();
            uIElement.RenderTransform = transform;
            uIElement.RenderTransformOrigin = new Point(0.5, 0.5);
            transform.BeginAnimation(ScaleTransform.ScaleXProperty, disappear_scale);
            transform.BeginAnimation(ScaleTransform.ScaleYProperty, disappear_scale);
            uIElement.BeginAnimation(System.Windows.Controls.UserControl.OpacityProperty, disappear_opacity);
        }
        public static void Appear_Element(UIElement uIElement, Duration animation_duration)
        {
            DoubleAnimation appear_scale = new DoubleAnimation(.5, 1, animation_duration);
            DoubleAnimation appear_opacity = new DoubleAnimation(0, 1, animation_duration);
            ScaleTransform transform = new ScaleTransform();
            uIElement.RenderTransform = transform;
            uIElement.RenderTransformOrigin = new Point(0.5, 0.5);
            transform.BeginAnimation(ScaleTransform.ScaleXProperty, appear_scale);
            transform.BeginAnimation(ScaleTransform.ScaleYProperty, appear_scale);
            uIElement.BeginAnimation(System.Windows.Controls.UserControl.OpacityProperty, appear_opacity);
            //transform.BeginAnimation(ScaleTransform.CenterYProperty, appear_scale);
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
