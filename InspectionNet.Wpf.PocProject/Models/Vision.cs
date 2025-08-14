using System;
using System.Windows;
using System.Windows.Input;

using CommunityToolkit.Mvvm.Input;

using InspectionNet.Core.Models;
using InspectionNet.Core.Services;
using InspectionNet.Core.StaticClasses;
using InspectionNet.Core.VisionTools;

using InspectionNet.Wpf.Common.Models;

namespace InspectionNet.Wpf.PocProject.Models
{
    public class Vision : IVision
    {
        private const string SPACENAME = "Fixture";
        private const string OUTPUTX = "OutputX";
        private const string OUTPUTY = "OutputY";
        private readonly System.Windows.Forms.OpenFileDialog _openFileDialog = new();
        private readonly ILpCamera _camera;
        private readonly ILpRuleToolGroup _ruleToolGroup;
        private readonly ILpToolGroup _aiToolGroup;
        private readonly IDisplayService _displayService;
        private ILpGrabResult _lastGrabResult;

        public ILpImage InputImage { get; set; }
        public string CameraId { get => _camera?.Id ?? string.Empty; }
        public ILpRuleToolGroup ToolGroup { get => _ruleToolGroup; }
        public bool IsCalibrationMode
        {
            get => _ruleToolGroup.IsCalibrationMode;
            set => _ruleToolGroup.IsCalibrationMode = value;
        }
        public bool IsSaveOriginImage
        {
            get => _ruleToolGroup.IsSaveOriginImage;
            set => _ruleToolGroup.IsSaveOriginImage = value;
        }
        public bool IsSaveCsv
        {
            get => _ruleToolGroup.IsSaveCsv;
            set => _ruleToolGroup.IsSaveCsv = value;
        }
        public ICommand OpenCalibToolCommand { get; }
        public ICommand OpenToolBlockCommand { get; }
        public ICommand LoadTrainFileCommand { get; }
        public ICommand RuleRunCommand { get; }
        public ICommand AiRunCommand { get; }
        public ICommand RunCommand { get; }

        public event EventHandler<ILpGrabResult> CameraImageGrabbed;
        public event EventHandler<ILpToolResult> RuleToolGroupCompleted;
        public event EventHandler<ILpToolResult> AiToolGroupCompleted;

        public Vision(ILpCamera camera,
                      ILpRuleToolGroup ruleToolGroup,
                      ILpToolGroup aiToolGroup,
                      IDisplayService displayService)
        {
            if (camera != null)
            {
                _camera = camera;
                _camera.ImageGrabbed += Camera_ImageGrabbed;
            }

            _ruleToolGroup = ruleToolGroup;
            _ruleToolGroup.ToolBlockCompleted += RuleToolGroup_ToolBlockCompleted;

            _aiToolGroup = aiToolGroup;
            _aiToolGroup.ToolBlockCompleted += AiToolGroup_ToolBlockCompleted;

            _displayService = displayService;

            OpenCalibToolCommand = new RelayCommand(OpenCalibTool);
            OpenToolBlockCommand = new RelayCommand(OpenToolBlock);
            LoadTrainFileCommand = new RelayCommand(LoadTrainFile);
            RuleRunCommand = new RelayCommand(RuleRun);
            AiRunCommand = new RelayCommand(AiRun);
            RunCommand = new RelayCommand(Run);
        }



        private void Camera_ImageGrabbed(object sender, ILpGrabResult e)
        {
            _lastGrabResult = e;
            CameraImageGrabbed?.Invoke(this, e);
        }

        private void RuleToolGroup_ToolBlockCompleted(object sender, ILpToolResult e) => RuleToolGroupCompleted?.Invoke(this, e);
        private void AiToolGroup_ToolBlockCompleted(object sender, ILpToolResult e) => AiToolGroupCompleted?.Invoke(this, e);
        private void OpenCalibTool()
        {
            try
            {
                if (_lastGrabResult != null)
                {
                    _ruleToolGroup.CalibrationInputImage = _displayService.CreateImage(_lastGrabResult);
                }
                Window window = new()
                {
                    Title = "Checkerboard Calibration Tool Edit",
                    Content = (_ruleToolGroup as IWpfToolGroup).CalibCheckerboardEditControl,
                };
                window.Show();
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
            }

        }
        private void OpenToolBlock()
        {
            Window window = new()
            {
                Title = "Tool Block Edit",
                Content = (_ruleToolGroup as IWpfToolGroup).ToolBlockEditControl,
            };
            window.Show();
        }
        private void LoadTrainFile()
        {
            if (_openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string trainFilePath = _openFileDialog.FileName;
                _aiToolGroup.Initialize(trainFilePath);
            }
        }
        private void RuleRun()
        {
            if (InputImage == null)
            {
                Console.WriteLine("Input image is null. Please set an input image before running the AI tool group.");
                return;
            }
            _ruleToolGroup.Run(InputImage);
        }
        private void AiRun()
        {
            if (InputImage == null)
            {
                Console.WriteLine("Input image is null. Please set an input image before running the AI tool group.");
                return;
            }
            _aiToolGroup.Run(InputImage);
        }
        private void Run()
        {
            RuleRun();
            AiRun();
        }

        public void CreateFixture(double p1X, double p1Y, double p2X, double p2Y) => _ruleToolGroup.CreateFixture(SPACENAME, p1X, p1Y, p2X, p2Y);
        public void ConvertPositionInFixture(double positionX, double positionY, out double outputX, out double outputY)
        {
            ILpToolResult fixtureResult = _ruleToolGroup.RunFixture(SPACENAME, positionX, positionY);
            outputX = ToolHelper.GetTerminalValueDouble(fixtureResult, OUTPUTX);
            outputY = ToolHelper.GetTerminalValueDouble(fixtureResult, OUTPUTY);
        }
    }
}
