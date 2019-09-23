using System;
using CUtility.Math;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace CRender.IO
{
    public static class PPMWriter
    {
        public static void SaveAsPPM<T>(this T image, string path, bool overwrite = false) where T : IPPMImage
        {
            if (!overwrite && File.Exists(path))
                throw new Exception($"{path} already exists");

            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.Write($"P3\n{image.Width} {image.Height}\n255\n");
                for (int i = 0; i < image.Height; i++)
                {
                    for (int j = 0; j < image.Width; j++)
                    {
                        GenericVector<float> color = image.GetColorAt(j, i);
                        writer.Write($"{RGB01To255(color.R)} {RGB01To255(color.G)} {RGB01To255(color.B)} ");
                    }
                    writer.WriteLine();
                }
            }
        }

        private static int RGB01To255(float f) => JMath.RoundToInt(f * 255f);
    }
}
