using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

using InspectionNet.Core.Enums;
using InspectionNet.Core.Managers;
using InspectionNet.Core.Mediators;
using InspectionNet.Core.Models;
using InspectionNet.Core.Services;
using InspectionNet.Core.StaticClasses;
using InspectionNet.Core.VisionTools;

using InspectionNet.Wpf.PocProject.Models;

using Windows.Networking.Vpn;

namespace InspectionNet.Wpf.PocProject.Mediators
{
    public class VisionMediator : IVisionMediator
    {
        #region Variables

        private const string OUTPUTX = "OutputX";
        private const string OUTPUTY = "OutputY";
        private readonly ICameraService _cameraService;
        private readonly IRuleToolService _ruleToolService;
        private readonly IConfigService _configService;
        private readonly IAiToolService _aiToolService;
        private readonly IDisplayService _displayService;
        private readonly IMotionService _motionService;
        private readonly ILogManager _logManager;
        private readonly IList<Vision> _visionList = [];
        private OperationStatus _operationStatus = OperationStatus.None;
        private int _alignStep = -1;
        private int _runStep = -1;
        private bool _isLive = false;
        private ILpImage _currentImage;

        #endregion

        #region Properties

        public bool IsToolRun { get; set; }
        public bool IsInitMotion { get; set; }
        public IVision SelectedVision { get; set; }
        public bool UseGrabImage { get; set; }

        #endregion

        #region Events

        public event EventHandler<IVision> SelectedVisionChanged;
        public event EventHandler InitializeCompleted;
        public event EventHandler MotionInitializeCompleted;
        public event EventHandler AlignCompleted;
        public event EventHandler<ILpGrabResult> VisionCameraImageGrabbed;
        public event EventHandler<ILpToolResult> VisionRuleToolGroupCompleted;
        public event EventHandler<ILpToolResult> VisionAiToolGroupCompleted;
        public event EventHandler<ILpGrabResult> SelectedVisionCameraImageGrabbed;
        public event EventHandler<ILpToolResult> SelectedVisionRuleToolGroupCompleted;
        public event EventHandler<ILpToolResult> SelectedVisionAiToolGroupCompleted;

        #endregion

        #region Constructor

        public VisionMediator(ICameraService cameraService,
                              IRuleToolService ruleToolService,
                              IConfigService configService,
                              IAiToolService aiToolService,
                              IDisplayService displayService,
                              IMotionService motionService,
                              ILogManager logManager)
        {
            _cameraService = cameraService;
            _cameraService.CameraOpened += CameraService_CameraOpened;
            _cameraService.CameraListChanged += CameraService_CameraListChanged;
            _cameraService.CameraImageGrabbed += CameraService_CameraImageGrabbed;
            _cameraService.SelectedCameraChanged += CameraService_SelectedCameraChanged;
            _cameraService.InitializeCompleted += CameraService_InitializeCompleted;

            _ruleToolService = ruleToolService;
            _ruleToolService.CurrentToolRan += RuleToolService_CurrentToolRan;
            _configService = configService;
            _aiToolService = aiToolService;
            _displayService = displayService;
            _motionService = motionService;
            _motionService.MoveCompleted += MotionService_MoveCompleted;
            _logManager = logManager;

            //AddVision(null);
            //SelectedVision = _visionList.FirstOrDefault();
        }

        private void RuleToolService_CurrentToolRan(object sender, ILpToolResult e)
        {
            VisionRuleToolGroupCompleted?.Invoke(sender, e);
        }

        private void CameraService_CameraOpened(object sender, ILpCameraParameters e)
        {
            var SelectedCameraParameters = e.ToList();
            _currentImage = _displayService.CreateImage(SelectedCameraParameters);
        }

        #endregion

        #region Finalizer

        // Dispose 관련 코드가 있다면 여기에 위치시킵니다.

        #endregion

        #region Methods


        private void CameraService_CameraImageGrabbed(object sender, ILpGrabResult e)
        {
            if (IsSelectedCamera(sender) && UseGrabImage)
            {
                _currentImage.SetPixelData(e.PixelData);
                _ruleToolService.RunSequence(_currentImage);
            }
        }

        private bool IsSelectedCamera(object sender)
        {
            return sender is ILpCamera camera && camera.Id == _cameraService.SelectedCameraId;
        }

        private void CameraService_CameraListChanged(object sender, IEnumerable<ILpCamera> e)
        {
            /*RemoveVision();
            foreach (ILpCamera camera in e)
            {
                AddVision(camera);
            }*/
        }

        private void AddVision(ILpCamera camera)
        {
            string calibToolFilePath = _configService.GetVisionToolConfig().CalibToolFilePath;
            var vision = new Vision(camera, _ruleToolService.CreateToolGroup(calibToolFilePath), _aiToolService.CreateToolGroup(), _displayService);
            ConnectEvents(vision);
            _visionList.Add(vision);
        }

        private void RemoveVision(Vision vision)
        {
            DisconnectEvents(vision);
            _visionList.Remove(vision);
        }

        private void RemoveVision()
        {
            if (_visionList?.Count > 0)
            {
                foreach (var vision in _visionList)
                {
                    RemoveVision(vision);
                }
                _visionList.Clear();
            }
        }

        private void ConnectEvents(Vision vision)
        {
            if (vision == null) return;
            vision.CameraImageGrabbed += Vision_CameraImageGrabbed;
            vision.RuleToolGroupCompleted += Vision_RuleToolGroupCompleted;
            vision.AiToolGroupCompleted += Vision_AiToolGroupCompleted;
        }

        private void DisconnectEvents(Vision vision)
        {
            if (vision == null) return;
            vision.CameraImageGrabbed -= Vision_CameraImageGrabbed;
            vision.RuleToolGroupCompleted -= Vision_RuleToolGroupCompleted;
            vision.AiToolGroupCompleted -= Vision_AiToolGroupCompleted;
        }

        private void Vision_CameraImageGrabbed(object sender, ILpGrabResult e)
        {
            if (sender is IVision vision)
            {
                if (vision.Equals(SelectedVision))
                {
                    SelectedVisionCameraImageGrabbed?.Invoke(vision, e);
                    switch (_operationStatus)
                    {
                        //case OperationStatus.Align:
                        //case OperationStatus.Run:
                        default:
                            if (IsToolRun)
                            {
                                SelectedVision.InputImage = _displayService.CreateImage(e);
                                Task.Run(() => SelectedVision.RuleRunCommand.Execute(null));
                            }
                            break;
                    }
                    if (_isLive)
                    {
                        Task.Run(async () =>
                        {
                            await Task.Delay(1);
                            CameraTriggerSoftware();
                        });
                    }
                }
                VisionCameraImageGrabbed?.Invoke(vision, e);
            }
        }
        private void Vision_RuleToolGroupCompleted(object sender, ILpToolResult e)
        {
            Debug.WriteLine("Vision_RuleToolGroupCompleted");
            if (sender is IVision vision)
            {
                if (vision.Equals(SelectedVision))
                {
                    SelectedVisionRuleToolGroupCompleted?.Invoke(vision, e);
                    switch (_operationStatus)
                    {
                        case OperationStatus.Align: GoPosition(e); break;

                        case OperationStatus.Run:
                            if (_motionService.RunPositions.Count() > _runStep)
                            {
                                _motionService.Run(_runStep);
                                _runStep++;
                            }
                            else
                            {
                                _logManager.LogInfo(this, "ToolService_ToolGroupCompleted", "All sequence steps completed. No more steps to run.");
                                return;
                            }

                            break;
                    }
                }
                VisionRuleToolGroupCompleted?.Invoke(vision, e);
            }
        }
        private void Vision_AiToolGroupCompleted(object sender, ILpToolResult e)
        {
            if (sender is IVision vision)
            {
                if (vision.Equals(SelectedVision))
                {
                    SelectedVisionAiToolGroupCompleted?.Invoke(vision, e);
                }
                VisionAiToolGroupCompleted?.Invoke(vision, e);
            }
        }

        private void CameraService_SelectedCameraChanged(object sender, ILpCamera e)
        {
            SelectedVision = _visionList.FirstOrDefault(v => v.CameraId == e.Id);
            if (SelectedVision != null) SelectedVisionChanged.Invoke(this, SelectedVision);
        }

        private void CameraService_InitializeCompleted(object sender, EventArgs e)
        {
            InitializeCompleted?.Invoke(this, e);
        }

        private void MotionService_MoveCompleted(object sender, EventArgs e)
        {
            switch (_operationStatus)
            {
                case OperationStatus.Align:
                    switch (_alignStep)
                    {
                        case 0:
                        case 1:
                        case 2:
                            CameraTriggerSoftware();
                            break;
                    }
                    break;

                case OperationStatus.Run:
                    CameraTriggerSoftware();
                    break;
            }
        }

        private void CameraTriggerSoftware() => _cameraService.SelectedCamera?.Parameters.TriggerSoftware();

        private void GoPosition(ILpToolResult e)
        {
            if (IsAlignOK(e, out double resultX, out double resultY))
            {
                AlignSequence(_alignStep);
            }
            else
            {
                _motionService.MoveTarget(resultX, resultY, true);
            }
            var stepString = _alignStep == 0 ? "First" : "Second";
            _logManager.LogInfo(this, $"{stepString} Position Align : {resultX}, {resultY}");
        }

        private void AlignSequence(int alignStep)
        {
            switch (alignStep)
            {
                case 0:
                    _motionService.SetAlignFinalPosition(0);
                    _motionService.MoveAlignPosition(1);
                    _alignStep = 1;
                    break;

                case 1:
                    _motionService.SetAlignFinalPosition(1);
                    ILpAxisPosition posL = _motionService.AlignPositions.ElementAtOrDefault(0);
                    ILpAxisPosition posR = _motionService.AlignPositions.ElementAtOrDefault(1);
                    SelectedVision.CreateFixture(posL.FinalX, posL.FinalY, posR.FinalX, posR.FinalY);
                    SelectedVision.ConvertPositionInFixture(0, 0, out double outputX, out double outputY);
                    _motionService.MoveTarget(outputX, outputY);
                    _alignStep = 2;
                    break;

                case 2:
                    _operationStatus = OperationStatus.None;
                    AlignCompleted?.Invoke(this, EventArgs.Empty);
                    _alignStep = -1;
                    break;

            }
        }

        private static bool IsAlignOK(ILpToolResult e, out double resultX, out double resultY)
        {
            resultX = ToolHelper.GetTerminalValueDouble(e, OUTPUTX);
            resultY = ToolHelper.GetTerminalValueDouble(e, OUTPUTY);
            return IsSpecOK(resultX, resultY);
        }

        private static bool IsSpecOK(double x, double y)
        {
            double specX = 20;
            double specY = 20;
            if (Math.Abs(x) < specX && Math.Abs(y) < specY) return true;
            return false;
        }

        public void InitStep()
        {
            _alignStep = 0;
            _runStep = 0;
        }

        public void Align()
        {
            _operationStatus = OperationStatus.Align;
            _motionService.MoveAlignPosition(0);
            _alignStep = 0;
        }

        public void Run()
        {
            _runStep = 0;
            _operationStatus = OperationStatus.Run;
            _motionService.Run(0);
        }

        public void Stop()
        {
            _alignStep = 0;
            _runStep = 0;
            _operationStatus = OperationStatus.None;
            _motionService.MoveStop();
        }

        public void LiveStartCamera()
        {
            if (_cameraService.SelectedCamera == null)
            {
                _logManager.LogInfo(this, "SelectedCamera is null");
                return;
            }

            bool liveReady = _cameraService.SelectedCamera.IsLiveReady;
            if (liveReady)
            {
                _isLive = true;
                CameraTriggerSoftware();
            }
            else
            {
                _logManager.LogInfo(this, "Camera is not ready");
            }
        }

        public void LiveStopCamera()
        {
            _isLive = false;
        }

        #endregion
    }
}
