using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using System.Windows.Input;
using System.Windows.Threading;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using InspectionNet.Core.Enums;
using InspectionNet.Core.Managers;
using InspectionNet.Core.Mediators;
using InspectionNet.Core.Models;
using InspectionNet.Core.Services;
using InspectionNet.Core.Views;
using InspectionNet.Core.VisionTools;

using InspectionNet.Wpf.Common.MainFrame.ViewModels;
using InspectionNet.Wpf.Common.Models;
using InspectionNet.Wpf.PocProject.Models;

namespace InspectionNet.Wpf.PocProject.ViewModels
{
    public class PocMotionViewModel : ObservableObject, IMotionViewModel
    {
        #region Variables
        private readonly IVisionMediator _visionMediator;
        private readonly ICameraService _cameraService;
        private readonly ILogManager _logManager;
        private readonly IConfigService _configService;
        private readonly IMotionService _motionService;
        private readonly IDisplayService _displayService;
        private readonly Timer _tmrRefresh = new(100);
        private ILpAxisPosition _selectedAxisPosition;
        private ILpAxisPosition _selectedInitPosition;
        private ILpAxisPosition _selectedRunPosition;
        private string _selectedPositionName;
        private string _selectedInitPositionName;
        private string _selectedRunPositionName;
        private IPositionGenerator _positionGenerator;
        private double _fixturePositionX;
        private double _fixturePositionY;
        private ILpImage _currentImage;
        private const string DEFAULT_POSITION_NAME = "DefaultPos";
        #endregion

        #region Properties
        public IEnumerable<ILpAxis> AxisList { get; }
        public ObservableCollection<ILpAxisPosition> InitPositions { get; } = [];
        public ObservableCollection<ILpAxisPosition> RunPositions { get; } = [];
        public ILpAxisPosition SelectedAxisPosition
        {
            get => _selectedAxisPosition;
            set
            {
                if (SetProperty(ref _selectedAxisPosition, value))
                {
                    SelectedPositionName = _selectedAxisPosition?.Name;
                }
            }
        }
        public ILpAxisPosition SelectedInitPosition
        {
            get => _selectedInitPosition;
            set
            {
                if (SetProperty(ref _selectedInitPosition, value))
                {
                    SelectedInitPositionName = _selectedInitPosition?.Name;
                }
            }
        }
        public ILpAxisPosition SelectedRunPosition
        {
            get => _selectedRunPosition;
            set
            {
                if (SetProperty(ref _selectedRunPosition, value))
                {
                    SelectedRunPositionName = _selectedRunPosition?.Name;
                }
            }
        }
        public string SelectedPositionName
        {
            get => _selectedPositionName;
            set => SetProperty(ref _selectedPositionName, value);
        }
        public string SelectedInitPositionName
        {
            get => _selectedInitPositionName;
            set => SetProperty(ref _selectedInitPositionName, value);
        }
        public string SelectedRunPositionName
        {
            get => _selectedRunPositionName;
            set => SetProperty(ref _selectedRunPositionName, value);
        }
        public IPositionGenerator PositionGenerator
        {
            get => _positionGenerator;
            set => SetProperty(ref _positionGenerator, value);
        }
        public double FixturePositionX
        {
            get => _fixturePositionX;
            set => SetProperty(ref _fixturePositionX, value);
        }
        public double FixturePositionY
        {
            get => _fixturePositionY;
            set => SetProperty(ref _fixturePositionY, value);
        }
        public ICommand JogMoveLeftCommand { get; }
        public ICommand JogMoveRightCommand { get; }
        public ICommand JogMoveUpCommand { get; }
        public ICommand JogMoveDownCommand { get; }
        public ICommand JogMoveXStopCommand { get; }
        public ICommand JogMoveYStopCommand { get; }
        public ICommand LoadConfigFileCommand { get; }
        public ICommand SaveConfigFileCommand { get; }
        public ICommand AddInitPositionCommand { get; }
        public ICommand RemoveInitPositionCommand { get; }
        public ICommand AddRunPositionCommand { get; }
        public ICommand RemoveRunPositionCommand { get; }
        public ICommand RemoveAllRunPositionCommand { get; }
        public ICommand MoveFixturePositionCommand { get; }
        public ILpDisplay LaonDisplay { get; set; }
        public bool IsToolRun { get; set; }
        public bool IsRepeatRun { get => _motionService.IsRepeat; set => _motionService.IsRepeat = value; }
        #endregion

        #region Events

        #endregion

        #region Constructor
        public PocMotionViewModel(IMotionService motionService,
                                  IVisionMediator visionMediator,
                                  ICameraService cameraService,
                                  ILogManager logManager,
                                  IConfigService configService,
                                  IDisplayService displayService)
        {
            _logManager = logManager;
            _cameraService = cameraService;
            _cameraService.CameraOpened += CameraService_CameraOpened;
            _cameraService.CameraImageGrabbed += CameraService_CameraImageGrabbed;
            _cameraService.CameraParameterChanged += CameraService_CameraParameterChanged;

            _visionMediator = visionMediator;
            _visionMediator.SelectedVisionRuleToolGroupCompleted += VisionMediator_SelectedToolGroupCompleted;

            _configService = configService;

            _motionService = motionService;

            _displayService = displayService;
            LaonDisplay = _displayService.GetDisplay();

            AxisList = _motionService.AxisList;
            var axisX = AxisList.ElementAtOrDefault(0);
            var axisY = AxisList.ElementAtOrDefault(1);

            InitPositions.Clear();
            foreach (var initPosition in _motionService.AlignPositions)
            {
                InitPositions.Add(initPosition);
            }
            RunPositions.Clear();
            foreach (var runPosition in _motionService.RunPositions)
            {
                RunPositions.Add(runPosition);
            }

            SelectedRunPositionName = GetDefaultName(_motionService.RunPositions);
            _motionService.InitPositionAdded += MotionService_InitPositionAdded;
            _motionService.InitPositionRemoved += MotionService_InitPositionRemoved;
            _motionService.RunPositionAdded += MotionService_RunPositionAdded;
            _motionService.RunPositionRemoved += MotionService_RunPositionRemoved;
            _motionService.RunPositionCleared += MotionService_RunPositionCleared;

            _positionGenerator = new PositionGenerator(_motionService);

            _tmrRefresh.Elapsed += TmrRefresh_Elapsed;
            _tmrRefresh.Start();

            JogMoveLeftCommand = axisX?.MoveNegativeCommand;
            JogMoveRightCommand = axisX?.MovePositiveCommand;
            JogMoveUpCommand = axisY?.MovePositiveCommand;
            JogMoveDownCommand = axisY?.MoveNegativeCommand;
            JogMoveXStopCommand = axisX?.MoveStopCommand;
            JogMoveYStopCommand = axisY?.MoveStopCommand;

            LoadConfigFileCommand = new RelayCommand(LoadConfigFile);
            SaveConfigFileCommand = new RelayCommand(SaveConfigFile);

            AddInitPositionCommand = new RelayCommand(AddInitPosition);
            RemoveInitPositionCommand = new RelayCommand(RemoveInitPosition);
            AddRunPositionCommand = new RelayCommand(AddRunPosition);
            RemoveRunPositionCommand = new RelayCommand(RemoveRunPosition);
            RemoveAllRunPositionCommand = new RelayCommand(RemoveAllRunPosition);
            MoveFixturePositionCommand = new RelayCommand(MoveFixturePosition);
        }


        private static string GetDefaultName(IEnumerable<ILpAxisPosition> runPositions)
        {
            var count = runPositions != null ? runPositions.Count().ToString() : "0";
            return $"{DEFAULT_POSITION_NAME}_{count}";
        }

        #endregion

        #region Finalizer
        ~PocMotionViewModel()
        {
            _motionService.InitPositionAdded -= MotionService_InitPositionAdded;
            _motionService.InitPositionRemoved -= MotionService_InitPositionRemoved;
            _motionService.RunPositionAdded -= MotionService_RunPositionAdded;
            _motionService.RunPositionRemoved -= MotionService_RunPositionRemoved;

            _tmrRefresh.Elapsed -= TmrRefresh_Elapsed;
            _tmrRefresh.Stop();
            _motionService.Close();
        }
        #endregion

        #region Methods

        private void CameraService_CameraParameterChanged(object sender, ILpCameraParameters e)
        {
            _currentImage?.Dispose();
            if (sender is ILpCamera camera && camera.Id == _cameraService.SelectedCameraId)
            {
                _currentImage = _displayService.CreateImage(e.ToList());
            }
        }
        private void CameraService_CameraOpened(object sender, ILpCameraParameters e)
        {
            if (sender is ILpCamera camera && camera.Id == _cameraService.SelectedCameraId)
            {
                _currentImage = _displayService.CreateImage(e.ToList());
            }
        }
        private void CameraService_CameraImageGrabbed(object sender, ILpGrabResult e)
        {
            if (sender is not ILpCamera camera || IsToolRun) return;
            if (_cameraService.SelectedCameraId == camera.Id)
            {
                _currentImage.SetPixelData(e.PixelData);
                Dispatcher.CurrentDispatcher.Invoke(() =>
                {
                    LaonDisplay.LaonImage = _currentImage;
                });
            }
        }
        private void MotionService_InitPositionAdded(object sender, ILpAxisPosition e) => InitPositions.Add(e);
        private void MotionService_InitPositionRemoved(object sender, ILpAxisPosition e) => InitPositions.Remove(e);
        private void MotionService_RunPositionAdded(object sender, ILpAxisPosition e) => RunPositions.Add(e);
        private void MotionService_RunPositionRemoved(object sender, ILpAxisPosition e) => RunPositions.Remove(e);
        private void MotionService_RunPositionCleared(object sender, EventArgs e) => RunPositions.Clear();
        private void VisionMediator_SelectedToolGroupCompleted(object sender, ILpToolResult e)
        {
            if (LaonDisplay == null || !IsToolRun) return;
            LaonDisplay.LaonResult = e;
        }
        private void TmrRefresh_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                foreach (var axis in AxisList)
                {
                    axis.UpdateParameters();
                }
            }
            catch (InvalidOperationException ex)
            {
                _logManager.LogError(sender, ViErrorCode.AxisParameterUpdateError, "Error occurred while updating axis parameters.", ex);
            }

        }

        private void LoadConfigFile() => _motionService.LoadConfigFile(_configService.GetMotionConfig().MotionConfigFilePath);
        private void SaveConfigFile() => _motionService.SaveConfigFile();

        private void AddInitPosition() => _motionService.AddInitPosition(SelectedInitPositionName);
        private void RemoveInitPosition() => _motionService.RemoveInitPosition(SelectedInitPositionName);
        private void AddRunPosition() => _motionService.AddRunPosition(SelectedRunPositionName);
        private void RemoveRunPosition() => _motionService.RemoveRunPosition(SelectedRunPositionName);
        private void RemoveAllRunPosition() => _motionService.ClearRunPosition();
        private void MoveFixturePosition()
        {
            _visionMediator.SelectedVision.ConvertPositionInFixture(FixturePositionX, FixturePositionY, out double ConvertX, out double ConvertY);
            _motionService.MoveTarget(ConvertX, ConvertY);
        }
        #endregion
    }
}