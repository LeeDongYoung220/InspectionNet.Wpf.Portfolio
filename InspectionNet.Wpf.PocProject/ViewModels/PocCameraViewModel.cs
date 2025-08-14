using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using InspectionNet.Core.Enums;
using InspectionNet.Core.Models;
using InspectionNet.Core.Services;
using InspectionNet.Core.Views;

using InspectionNet.Wpf.Common.MainFrame.ViewModels;
using InspectionNet.Wpf.Common.StaticClasses;
using InspectionNet.Wpf.PocProject.Services;

namespace InspectionNet.Wpf.PocProject.ViewModels
{
    public class PocCameraViewModel : ObservableObject, ICameraViewModel, IDisposable
    {
        #region Variables
        private readonly ICameraService _cameraService;
        private readonly IConfigService _configService;
        private readonly IDisplayService _displayService;
        private readonly IFileDialogService _fileDialogService;
        private ILpGrabResult _lastGrabResult;
        private string _saveImagePath;
        private GrabberMaker _selectedGrabberMaker;
        private IEnumerable<string> _cameraList;
        private string _selectedCameraId;
        private IList<IGenApiParameter> _selectedCameraParameters;
        private ILpImage _currentImage;

        #endregion

        #region Properties
        public IList<GrabberMaker> GrabberMakers { get; private set; }
        public GrabberMaker SelectedGrabberMaker
        {
            get => _selectedGrabberMaker;
            set => SetProperty(ref _selectedGrabberMaker, value);
        }
        public IEnumerable<string> CameraIdList
        {
            get => _cameraList;
            set => SetProperty(ref _cameraList, value);
        }
        public string SelectedCameraId
        {
            get => _selectedCameraId;
            set
            {
                if (SetProperty(ref _selectedCameraId, value))
                {
                    _cameraService.SelectedCameraId = value;
                }
            }
        }
        public ILpDisplay LaonDisplay { get; private set; }
        public IList<IGenApiParameter> SelectedCameraParameters
        {
            get => _selectedCameraParameters;
            set => SetProperty(ref _selectedCameraParameters, value);
        }


        public ICommand DiscoverCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ICommand CloseCommand { get; private set; }
        public ICommand GrabStartCommand { get; private set; }
        public ICommand GrabStopCommand { get; private set; }
        public ICommand SelectDirectoryCommand { get; private set; }
        public ICommand SaveImageCommand { get; private set; }
        public int CameraFrame { get => cameraFrame; private set => SetProperty(ref cameraFrame, value); }
        #endregion

        #region Events

        #endregion

        #region Constructor
        public PocCameraViewModel(ICameraService cameraService,
                                  IConfigService configService,
                                  IDisplayService displayService,
                                  IFileDialogService fileDialogService)
        {
            _cameraService = cameraService;
            _configService = configService;
            _displayService = displayService;
            _fileDialogService = fileDialogService;
            InitCameraService();
            InitGrabberMaker();
            InitDisplay();
            InitCammand();
            InitFramerTimer();
        }

        private void InitFramerTimer()
        {
            _timer = new Timer
            {
                Interval = 1000 // 1 second
            };
            _timer.Elapsed += Timer_Elapsed;
        }

        private void InitDisplay()
        {
            LaonDisplay = _displayService.GetDisplay();
        }

        private void InitGrabberMaker()
        {
            GrabberMakers = (IList<GrabberMaker>)Enum.GetValues(typeof(GrabberMaker));
        }

        private void InitCammand()
        {
            DiscoverCommand = new RelayCommand(Discover);
            OpenCommand = new RelayCommand(Open, CanExecuteOpen);
            CloseCommand = new RelayCommand(Close, CanExecuteClose);
            GrabStartCommand = new RelayCommand(GrabStart, CanExecuteGrabStart);
            GrabStopCommand = new RelayCommand(GrabStop, CanExecuteGrabStop);
            SelectDirectoryCommand = new RelayCommand(SelectDirectory);
            SaveImageCommand = new RelayCommand(SaveImage);
        }

        private void InitCameraService()
        {
            _cameraService.CameraListChanged += CameraService_CameraListChanged;
            _cameraService.SelectedCameraChanged += CameraService_SelectedCameraChanged;
            _cameraService.CameraOpened += CameraService_CameraOpened;
            _cameraService.CameraClosed += CameraService_CameraClosed;
            _cameraService.CameraGrabStarted += CameraService_CameraGrabStarted;
            _cameraService.CameraGrabStopped += CameraService_CameraGrabStopped;
            _cameraService.CameraImageGrabbed += CameraService_CameraImageGrabbed;
            _cameraService.CameraParameterChanged += CameraService_CameraParameterChanged;
        }
        #endregion

        #region Finalizer
        private bool disposedValue;
        private Timer _timer;
        private int _frame;
        private int cameraFrame;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Dispose managed resources.
                }
                DisposeCamera();
                disposedValue = true;
            }
        }

        ~PocCameraViewModel()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void DisposeCamera()
        {
            _cameraService.CameraListChanged -= CameraService_CameraListChanged;
            _cameraService.SelectedCameraChanged -= CameraService_SelectedCameraChanged;
            _cameraService.CameraOpened -= CameraService_CameraOpened;
            _cameraService.CameraClosed -= CameraService_CameraClosed;
            _cameraService.CameraGrabStarted -= CameraService_CameraGrabStarted;
            _cameraService.CameraGrabStopped -= CameraService_CameraGrabStopped;
            _cameraService.CameraImageGrabbed -= CameraService_CameraImageGrabbed;
        }
        #endregion

        #region Methods

        private void CameraService_CameraParameterChanged(object sender, ILpCameraParameters e)
        {
            if (!IsSelectedCamera(sender)) return;
            _currentImage?.Dispose();
            _currentImage = _displayService.CreateImage(e.ToList());
        }

        private void CameraService_CameraImageGrabbed(object sender, ILpGrabResult e)
        {
            if (!IsSelectedCamera(sender)) return;
            var parameters = _configService.GetConfigParameters();
            
            _currentImage.SetPixelData(e.PixelData);
            _lastGrabResult = e;
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                LaonDisplay.LaonImage = _currentImage;
            });
            
            _frame++;
            if (!_cameraService.SelectedCamera.IsLive && parameters.SaveOriginImage && !string.IsNullOrEmpty(parameters.SaveImagePath))
            {
                SaveImage(_currentImage, parameters.SaveImagePath);
            }
        }

        private void CameraService_CameraListChanged(object sender, IEnumerable<ILpCamera> e)
        {
            if (e.Any())
            {
                CameraIdList = e.Select(c => c.Id);
                if (string.IsNullOrEmpty(SelectedCameraId)) SelectedCameraId = CameraIdList.FirstOrDefault();
            }
            else
            {
                MessageBox.Show("카메라가 없습니다.");
            }
        }

        private void CameraService_SelectedCameraChanged(object sender, ILpCamera e)
        {
            CanExecuteRefresh();
        }

        private void CameraService_CameraOpened(object sender, ILpCameraParameters e)
        {
            if (IsSelectedCamera(sender))
            {
                SelectedCameraParameters = e.ToList();
                _currentImage = _displayService.CreateImage(SelectedCameraParameters);
                CanExecuteRefresh();
            }
        }

        private bool IsSelectedCamera(object sender)
        {
            return sender is ILpCamera camera && _cameraService.SelectedCamera.Id == camera.Id;
        }

        private void CameraService_CameraClosed(object sender, EventArgs e)
        {
            if (IsSelectedCamera(sender))
            {
                SelectedCameraParameters = null;
                CanExecuteRefresh();
            }
        }
        private void CameraService_CameraGrabStarted(object sender, EventArgs e)
        {
            if (!IsSelectedCamera(sender)) return;
            CanExecuteRefresh();
        }
        private void CameraService_CameraGrabStopped(object sender, EventArgs e)
        {
            if (!IsSelectedCamera(sender)) return;
            CanExecuteRefresh();
        }
        private void Discover() => _cameraService.Discover();
        private void Open() => _cameraService.OpenCamera(_selectedCameraId);
        private void Close() => _cameraService.CloseCamera(_selectedCameraId);
        private void GrabStart()
        {
            if (!_timer.Enabled) _timer.Start();
            _cameraService.GrabStartCamera(_selectedCameraId);
        }

        private void GrabStop()
        {
            _cameraService.GrabStopCamera(_selectedCameraId);
            if (_timer.Enabled) _timer.Stop();
        }

        private void SelectDirectory()
        {
            var selectedPath = _fileDialogService.ShowFolderBrowserDialog();
            if (!string.IsNullOrEmpty(selectedPath))
            {
                _saveImagePath = selectedPath;
            }
        }
        private void SaveImage()
        {
            if (_lastGrabResult == null || string.IsNullOrEmpty(_saveImagePath)) return;

            ILpImage image = _displayService.CreateImage(_lastGrabResult);
            SaveImage(image, _saveImagePath);
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CameraFrame = _frame;
            _frame = 0;
        }
        private static void SaveImage(ILpImage image, string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            image.OriginalImage.Save(Path.Combine(path, $"{DateTime.Now:yyyyMMdd_HHmmss}.bmp"), System.Drawing.Imaging.ImageFormat.Bmp);
        }
        private bool CanExecuteOpen() => _cameraService.SelectedCamera != null && !_cameraService.SelectedCamera.IsOpen;
        private bool CanExecuteClose() => _cameraService.SelectedCamera != null && _cameraService.SelectedCamera.IsOpen;
        private bool CanExecuteGrabStart() => _cameraService.SelectedCamera != null && _cameraService.SelectedCamera.IsOpen && !_cameraService.SelectedCamera.IsGrabStarted;
        private bool CanExecuteGrabStop() => _cameraService.SelectedCamera != null && _cameraService.SelectedCamera.IsOpen && _cameraService.SelectedCamera.IsGrabStarted;
        private void CanExecuteRefresh()
        {
            ThreadInvoker.DispatcherInvoke(() =>
            {
                (OpenCommand as RelayCommand)?.NotifyCanExecuteChanged();
                (CloseCommand as RelayCommand)?.NotifyCanExecuteChanged();
                (GrabStartCommand as RelayCommand)?.NotifyCanExecuteChanged();
                (GrabStopCommand as RelayCommand)?.NotifyCanExecuteChanged();
            });
        }
        #endregion
    }
}
