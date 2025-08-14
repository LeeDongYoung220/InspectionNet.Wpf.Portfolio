using System;

using Cognex.VisionPro.CalibFix;
using Cognex.VisionPro.ToolBlock;

using InspectionNet.Core.Implementations;
using InspectionNet.Core.Models;
using InspectionNet.Core.VisionTools;

namespace InspectionNet.VisionTool.CognexModule.Common.Models
{
    public interface ILpCogToolGroup : ILpToolGroup, IDisposable
    {
        CogToolBlock ToolBlock { get; }
        CogCalibCheckerboardTool CalibCheckerboardTool { get; }
    }
}
