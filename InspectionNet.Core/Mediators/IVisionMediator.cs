using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InspectionNet.Core.Enums;
using InspectionNet.Core.Models;
using InspectionNet.Core.VisionTools;

namespace InspectionNet.Core.Mediators
{
    public interface IVisionMediator
    {
        IVision SelectedVision { get; set; }
        bool IsToolRun { get; set; }
        bool IsInitMotion { get; set; }
        bool UseGrabImage { get; set; }

        event EventHandler<IVision> SelectedVisionChanged;
        event EventHandler AlignCompleted;
        event EventHandler MotionInitializeCompleted;
        event EventHandler<ILpGrabResult> VisionCameraImageGrabbed;
        event EventHandler<ILpToolResult> VisionRuleToolGroupCompleted;
        event EventHandler<ILpToolResult> VisionAiToolGroupCompleted;
        event EventHandler<ILpGrabResult> SelectedVisionCameraImageGrabbed;
        event EventHandler<ILpToolResult> SelectedVisionRuleToolGroupCompleted;
        event EventHandler<ILpToolResult> SelectedVisionAiToolGroupCompleted;

        void InitStep();
        void Align();
        void Run();
        void Stop();
        //void LiveStartCamera();
        //void LiveStopCamera();
    }
}
