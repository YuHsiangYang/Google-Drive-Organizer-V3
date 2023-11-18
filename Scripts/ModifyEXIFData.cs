using Google_Drive_Organizer_V3.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

namespace Google_Drive_Organizer_V3.Scripts
{
    internal static class ModifyEXIFData
    {
        public static async Task Modify(ImageExif image_data, string output_dir)
        {
            //Copy the file to the new location
            File.Copy(image_data.EXIFData.ImagePath, Path.Combine(output_dir, Path.GetFileName(image_data.EXIFData.ImagePath)), true);
            string command = string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\"",
                System.IO.Path.Combine(output_dir, Path.GetFileName(image_data.EXIFData.ImagePath)),
                image_data.JsonData.PhotoTakenTime_DateTime.ToString("yyyy:MM:dd HH:mm:ss"),
                ImageInfo_Functions.DMS_To_Double(image_data.JsonData.GPS_Latitude),
                ImageInfo_Functions.DMS_To_Double(image_data.JsonData.GPS_Longitude),
                image_data.JsonData.GPS_Altitude
                );

            ProcessStartInfo modify = new ProcessStartInfo()
            {
                FileName = @".\Scripts\Modify.exe",
                Arguments = command,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
            };
            await Task.Run(async () =>
            {
               Process process = await Task.Run(() => Process.Start(modify));
            });
            Process.Start(modify);
            Console.WriteLine(command);
        }
    }
}
