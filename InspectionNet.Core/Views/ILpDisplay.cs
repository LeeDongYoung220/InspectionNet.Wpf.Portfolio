using System.Drawing;

using InspectionNet.Core.Models;
using InspectionNet.Core.VisionTools;

namespace InspectionNet.Core.Views
{
    public interface ILpDisplay
    {
        ILpImage LaonImage { get; set; }
        ILpToolResult LaonResult { get; set; }

        void DisplayFit(bool status);
        Image GetOverlayImage();
        Bitmap CaptureDisplay();
        void SaveImage();
        void OverlayClear();
    }
}