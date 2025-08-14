using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using InspectionNet.Core.Managers;
using InspectionNet.Core.Mediators;
using InspectionNet.Core.Models;
using InspectionNet.Core.Services;
using InspectionNet.Core.Views;
using InspectionNet.Core.VisionTools;

using InspectionNet.Wpf.Common.MainFrame.ViewModels;

namespace InspectionNet.Wpf.PocProject.ViewModels
{
    public class PocHomeViewModel : ObservableObject, IHomeViewModel, IDisposable
    {
        #region Variables

        private readonly ICameraService _cameraService;
        private readonly IRuleToolService _toolService;
        private readonly IMotionService _motionService;
        private readonly IDisplayService _displayService;

        private readonly ISequence _sequence;
        private bool _isInitializedCamera;
        private bool _isInitializedTool;
        private bool _isInitializedMotion;
        private BitmapImage _grabImage;
        private ILpImage _currentImage;
        #endregion

        #region Properties
        public bool IsInitializedCamera { get => _isInitializedCamera; private set => SetProperty(ref _isInitializedCamera, value); }
        public bool IsInitializedTool { get => _isInitializedTool; private set => SetProperty(ref _isInitializedTool, value); }
        public bool IsInitializedMotion { get => _isInitializedMotion; private set => SetProperty(ref _isInitializedMotion, value); }
        public BitmapImage GrabImage { get => _grabImage; set => SetProperty(ref _grabImage, value); }
        public bool IsToolRun { get; set; }
        public bool IsInitMotion { get => isInitMotion; set => SetProperty(ref isInitMotion, value); }

        public ILpDisplay LaonDisplay { get; }
        public ICommand InitComponentCommand { get; }
        public ICommand AlignSequenceCommand { get; }
        public ICommand RunSequenceCommand { get; }
        public ICommand SequenceStopCommand { get; }
        public ICommand LiveStartCameraCommand { get; }
        public ICommand LiveStopCameraCommand { get; }
        #endregion

        #region Events

        #endregion

        #region Constructor
        public PocHomeViewModel(ICameraService cameraService,
                                IRuleToolService toolService,
                                IMotionService motionService,
                                ISequence sequence,
                                IDisplayService displayService)
        {
            _cameraService = cameraService;
            _cameraService.InitializeCompleted += CameraService_InitializeCompleted;
            _cameraService.CameraImageGrabbed += CameraService_CameraImageGrabbed;
            _cameraService.CameraOpened += CameraService_CameraOpened;
            _cameraService.CameraParameterChanged += CameraService_CameraParameterChanged;

            _motionService = motionService;
            _motionService.InitializeCompleted += MotionService_InitializeCompleted;

            _toolService = toolService;
            _toolService.InitializeCompleted += ToolService_InitializeCompleted;
            _toolService.CurrentToolRan += Sequence_VisionToolGroupCompleted;

            _sequence = sequence;
            _sequence.VisionToolGroupCompleted += Sequence_VisionToolGroupCompleted;
            //_sequence.SelectedVisionRuleToolGroupCompleted += VisionMediator_SelectedToolGroupCompleted;

            _displayService = displayService;
            LaonDisplay = _displayService.GetDisplay();

            InitComponentCommand = new RelayCommand(InitComponent);
            AlignSequenceCommand = new RelayCommand(AlignSequence);
            RunSequenceCommand = new RelayCommand(RunSequence);
            SequenceStopCommand = new RelayCommand(SequenceStop);
            LiveStartCameraCommand = new RelayCommand(StartLiveCamera);
            LiveStopCameraCommand = new RelayCommand(StopLiveCamera);

            IsInitMotion = true;
        }

        #endregion

        #region Finalizer

        private bool disposed;
        private bool isInitMotion;

        ~PocHomeViewModel()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //dispose managed resources
                }

                //dispose of unmanaged resoureces

                disposed = true;
            }
        }
        #endregion

        #region Methods

        private void CameraService_CameraParameterChanged(object sender, ILpCameraParameters e)
        {
            _currentImage?.Dispose();
            _currentImage = _displayService.CreateImage(e.ToList());
        }

        private void CameraService_CameraOpened(object sender, ILpCameraParameters e)
        {
            var SelectedCameraParameters = e.ToList();
            _currentImage = _displayService.CreateImage(SelectedCameraParameters);
        }

        private void CameraService_CameraImageGrabbed(object sender, ILpGrabResult e)
        {
            if (sender is not ILpCamera camera || IsToolRun) return;
            if (_cameraService.SelectedCameraId == camera.Id)
            {
                _currentImage.SetPixelData(e.PixelData);
                Dispatcher.CurrentDispatcher.Invoke(() =>
                {
                    LaonDisplay.OverlayClear();
                    LaonDisplay.LaonImage = _currentImage;
                });
            }
        }

        private void Sequence_VisionToolGroupCompleted(object sender, ILpToolResult e)
        {
            if (LaonDisplay == null || !IsToolRun) return;
            LaonDisplay.LaonResult = e;
        }

        private void ToolService_InitializeCompleted(object sender, EventArgs e)
        {
            IsInitializedTool = true;
        }

        private void MotionService_InitializeCompleted(object sender, EventArgs e)
        {
            IsInitializedMotion = true;
        }

        private void CameraService_InitializeCompleted(object sender, EventArgs e)
        {
            IsInitializedCamera = true;
        }

        private void InitComponent()
        {
            IsInitializedMotion = false;
            IsInitializedTool = false;
            IsInitializedCamera = false;
            _cameraService.Initialize();
            _toolService.Initialize();
            if (IsInitMotion) _motionService.Initialize();
        }
        private void AlignSequence()
        {
            _sequence.StartAlign();
        }
        private void RunSequence()
        {
            _sequence.Run();
        }
        private void SequenceStop()
        {
            _sequence.Stop();
        }
        private void StartLiveCamera()
        {
            _cameraService.StartLive(_cameraService.SelectedCamera);
        }
        private void StopLiveCamera()
        {
            _cameraService.StopLive(_cameraService.SelectedCamera);
        }
        #endregion
    }
}
