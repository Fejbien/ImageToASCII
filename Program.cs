using System;
using System.Drawing;
using System.IO;

namespace ToASCII
{
    internal class Program
    {
        //static readonly string DENSITY = @" .'`^,:; Il!i><~+_-?][}{1)(|\/tfjrxnuvczXYUJCLQ0OZmwqpdbkhao*#MW&8%B@$";
        static readonly string DENSITY = @" .°*oO#@";

        private static string GetFileName()
        {
            Console.WriteLine("File name in pictures folder (with extension): ");
            return Console.ReadLine();
        }

        private static int ScaleDown()
        {
            Console.WriteLine("Scale down the image by (one nothing change, two its a half of the image etc): ");
            var prasing = int.TryParse(Console.ReadLine(), out var size);
            return prasing ? size : 1;
        }

        private static float Map(float n, float start1, float stop1, float start2, float stop2)
        {
            return ((n - start1) / (stop1 - start1)) * (stop2 - start2) + start2;
        }

        static void Main(string[] args)
        {
            var fileName = GetFileName();
            var sizing = ScaleDown();

            var picturesPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            var path = picturesPath + @"\" + fileName;
            var savingPath = picturesPath + @"\" + Path.GetFileNameWithoutExtension(fileName) + "-ASCII.txt";

            if (!File.Exists(path))
            {
                Console.WriteLine($"Your file doesnt exist!\nFile path: {path}");
                Console.ReadKey();
                Environment.Exit(0);
            }

            Console.Clear();

            Bitmap preSized = new Bitmap(path);

            Size size = new Size(preSized.Width / sizing, preSized.Height / sizing);
            Bitmap img = new Bitmap(preSized, size);

            string line = string.Empty;
            for (int height = 0; height < size.Height; height++)
            {
                for (int width = 0; width < size.Width; width++)
                {
                    Color pixel = img.GetPixel(width, height);
                    float brightness = pixel.GetBrightness();

                    var index = (int)Math.Floor(Map(brightness, 0, 1, 0, DENSITY.Length - 1));
                    line += " " + DENSITY[index];
                }
                line += "\n";
            }

            Console.WriteLine(line);

            using (StreamWriter sw = File.CreateText(savingPath))
                sw.WriteLine(line);

            Console.ReadKey();
        }
    }
}