using System;
using System.Drawing;

namespace InspectionNet.Core.Models
{
    public interface ILpImage : IDisposable
    {
        Image? OriginalImage { get; }

        void SetPixelData(byte[] pixelData);
    }
}
