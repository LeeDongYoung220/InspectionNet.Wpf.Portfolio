using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Versioning;

namespace InspectionNet.Core.StaticClasses
{
    public static class BitmapHelper
    {
        public static string[] AvailableFormat { get; } = { ".bmp", ".jpg", ".jpeg", ".png", ".gif", ".tiff", ".tif" };

        public static ColorPalette GetColorPalette()
        {
            var bmp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed);
            var palette = bmp.Palette;
            for (int i = 0; i < palette.Entries.Length; i++)
            {
                palette.Entries[i] = Color.FromArgb(i, i, i);
            }
            bmp.Dispose();
            return palette;
        }

        public static ColorPalette GetJetColorPalette(int alpha = 255)
        {
            var bmp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed);
            var palette = bmp.Palette;
            for (int i = 0; i < palette.Entries.Length; i++)
            {
                palette.Entries[i] = Color.FromArgb(alpha, GetJetColor(i));
            }
            bmp.Dispose();
            return palette;
        }

        public static Bitmap Convert32bppTo24bpp(Bitmap src)
        {
            var dst = new Bitmap(src.Width, src.Height, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(dst))
            {
                g.DrawImage(src, new Rectangle(0, 0, src.Width, src.Height));
            }
            return dst;
        }

        private static Color GetJetColor(int index)
        {
            int r, g, b;
            if (index < 64)
            {
                r = 0;
                g = 4 * index;
                b = 255;
            }
            else if (index < 128)
            {
                r = 4 * (index - 64);
                g = 255;
                b = 255 - 4 * (index - 64);
            }
            else if (index < 192)
            {
                r = 255;
                g = 255 - 4 * (index - 128);
                b = 0;
            }
            else
            {
                r = 255;
                g = 0;
                b = 0;
            }

            return Color.FromArgb(r, g, b);
        }
    }
}
