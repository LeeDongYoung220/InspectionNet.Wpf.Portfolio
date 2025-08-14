using System.Drawing;
using System.Drawing.Imaging;

namespace InspectionNet.Core.Models
{
    public interface ILpGrabResult
    {
        byte[]? PixelData { get; }
        int Width { get; }
        int Height { get; }
        int Stride { get; }
        PixelFormat PixelFormat { get; }

        Bitmap ToBitmap();
    }
}
