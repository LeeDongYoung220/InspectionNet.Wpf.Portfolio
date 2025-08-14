using InspectionNet.Core.Implementations;
using InspectionNet.Core.VisionTools;

namespace InspectionNet.Wpf.Common.Models
{
    public interface ILpFixtureTool : IDisposable
    {
        ILpToolResult Run(double px, double py);
        ILpToolResult Run(PointD p);
    }
}
