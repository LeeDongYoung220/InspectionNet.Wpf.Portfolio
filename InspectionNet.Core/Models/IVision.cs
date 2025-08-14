using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using InspectionNet.Core.VisionTools;

namespace InspectionNet.Core.Models
{
    public interface IVision
    {
        ILpImage InputImage { get; set; }
        string CameraId { get; }
        ILpRuleToolGroup ToolGroup { get; }
        bool IsCalibrationMode { get; set; }
        bool IsSaveOriginImage { get; set; }
        bool IsSaveCsv { get; set; }
        ICommand OpenCalibToolCommand { get; }
        ICommand OpenToolBlockCommand { get; }
        ICommand LoadTrainFileCommand { get; }
        ICommand RuleRunCommand { get; }
        ICommand AiRunCommand { get; }
        ICommand RunCommand { get; }

        event EventHandler<ILpGrabResult> CameraImageGrabbed;
        event EventHandler<ILpToolResult> RuleToolGroupCompleted;
        event EventHandler<ILpToolResult> AiToolGroupCompleted;

        void CreateFixture(double p1X, double p1Y, double p2X, double p2Y);
        void ConvertPositionInFixture(double positionX, double positionY, out double outputX, out double outputY);
    }
}
