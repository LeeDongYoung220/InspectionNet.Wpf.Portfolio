using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Versioning;

using InspectionNet.Core.Models;
using InspectionNet.Core.StaticClasses;

namespace InspectionNet.Core.Implementations
{
    public class LpBmpImage : ILpImage
    {
        private bool disposedValue;


        public Image? OriginalImage { get; }

        public LpBmpImage()
        {

        }

        public LpBmpImage(string filePath)
        {
            string[] validExtensions = BitmapHelper.AvailableFormat;
            if (!validExtensions.Contains(Path.GetExtension(filePath).ToLower())) throw new ArgumentException("Invalid file extension");
            var bmp = new Bitmap(filePath);
            if (bmp.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                bmp.Palette = BitmapHelper.GetColorPalette();
            }
            OriginalImage = bmp;
        }

        public LpBmpImage(Image image)
        {
            OriginalImage = new Bitmap(image);
        }

        public LpBmpImage(ILpGrabResult grabResult)
        {
            OriginalImage = grabResult.ToBitmap();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Dispose managed state (managed objects).
                }
                OriginalImage?.Dispose();
                disposedValue = true;
            }
        }

        ~LpBmpImage()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void SetPixelData(byte[] pixelData)
        {
            throw new NotImplementedException();
        }
    }
}
