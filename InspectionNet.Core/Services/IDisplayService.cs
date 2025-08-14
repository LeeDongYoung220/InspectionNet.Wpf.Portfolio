using System.Drawing;

using InspectionNet.Core.Models;
using InspectionNet.Core.Views;

namespace InspectionNet.Core.Services
{
    public interface IDisplayService
    {
        ILpDisplay ToolViewDisplay { get; }
        ILpImage CreateImage(IList<IGenApiParameter> grabResult);
        ILpImage CreateImage(string filePath);
        ILpImage CreateImage(Image image);
        ILpImage CreateImage(ILpGrabResult grabResult);
        ILpDisplay GetDisplay();
    }
}
