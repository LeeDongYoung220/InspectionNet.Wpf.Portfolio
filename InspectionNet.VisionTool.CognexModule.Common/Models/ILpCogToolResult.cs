using Cognex.VisionPro;

using InspectionNet.Core.VisionTools;

namespace InspectionNet.VisionTool.CognexModule.Common.Models
{
    public interface ILpCogToolResult : ILpToolResult
    {
        ICogRecord CogRecord { get; }
    }
}
