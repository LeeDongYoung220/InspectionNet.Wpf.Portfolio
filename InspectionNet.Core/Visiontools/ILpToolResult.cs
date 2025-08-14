using System.Collections.Generic;

using InspectionNet.Core.Models;

namespace InspectionNet.Core.VisionTools
{
    public interface ILpToolResult
    {
        ILpImage? ResultImage { get; }
        Dictionary<string, object> Outputs { get; }
    }
}
