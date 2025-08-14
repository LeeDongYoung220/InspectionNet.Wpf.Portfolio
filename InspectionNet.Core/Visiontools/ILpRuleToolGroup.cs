using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InspectionNet.Core.Implementations;
using InspectionNet.Core.Models;

namespace InspectionNet.Core.VisionTools
{
    public interface ILpRuleToolGroup : ILpToolGroup
    {
        bool IsCalibrationMode { get; set; }
        ILpImage CalibrationInputImage { get; set; }

        event EventHandler<ILpToolResult> Calibrated;

        ILpToolResult? Calibrate(ILpImage inputImage);
        void CreateFixture(string spaceName, PointD point, PointD point2);
        void CreateFixture(string spaceName, double p1X, double p1Y, double p2X, double p2Y);
        ILpToolResult? RunFixture(string spaceName, PointD point);
        ILpToolResult? RunFixture(string spaceName, double p1X, double p1Y);
    }
}
