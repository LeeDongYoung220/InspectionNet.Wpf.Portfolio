using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InspectionNet.Core.Mediators;
using InspectionNet.Core.Models;
using InspectionNet.Core.Services;
using InspectionNet.Core.VisionTools;

namespace InspectionNet.Wpf.PocProject.Mediators
{
    public class MockVisionMediator : IVisionMediator
    {
        private readonly ICameraService _cameraService;
        private readonly IDisplayService _displayService;
        private readonly IRuleToolService _ruleToolService;
        private bool _isAlignReady = false;

        public IVision SelectedVision { get; set; }
        public bool IsToolRun { get; set; }
        public bool IsInitMotion { get; set; }
        public bool UseGrabImage { get; set; }

        public event EventHandler<IVision> SelectedVisionChanged;
        public event EventHandler AlignCompleted;
        public event EventHandler MotionInitializeCompleted;
        public event EventHandler<ILpGrabResult> VisionCameraImageGrabbed;
        public event EventHandler<ILpToolResult> VisionRuleToolGroupCompleted;
        public event EventHandler<ILpToolResult> VisionAiToolGroupCompleted;
        public event EventHandler<ILpGrabResult> SelectedVisionCameraImageGrabbed;
        public event EventHandler<ILpToolResult> SelectedVisionRuleToolGroupCompleted;
        public event EventHandler<ILpToolResult> SelectedVisionAiToolGroupCompleted;

        public MockVisionMediator(ICameraService cameraService,
                                  IDisplayService displayService,
                                  IRuleToolService ruleToolService)
        {
            _cameraService = cameraService;
            _cameraService.CameraImageGrabbed += CameraService_CameraImageGrabbed;
            _displayService = displayService;
            _ruleToolService = ruleToolService;
        }

        private void CameraService_CameraImageGrabbed(object sender, ILpGrabResult e)
        {
            if (_isAlignReady)
            {
                var image = _displayService.CreateImage(e);
                _ruleToolService.RunSequence(image);
            }
        }

        public void Align()
        {
            if (_isAlignReady)
            {
                Run();
            }
        }

        public void InitStep()
        {
            throw new NotImplementedException();
        }

        public void LiveStartCamera()
        {
            throw new NotImplementedException();
        }

        public void LiveStopCamera()
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            _cameraService.SelectedCamera.Parameters.TriggerSoftware();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
