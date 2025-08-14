using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms.Integration;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using InspectionNet.Core.Managers;
using InspectionNet.Core.Mediators;
using InspectionNet.Core.Models;
using InspectionNet.Core.Services;
using InspectionNet.Core.StaticClasses;
using InspectionNet.Core.Views;
using InspectionNet.Core.VisionTools;

using InspectionNet.Wpf.Common.MainFrame.ViewModels;

namespace InspectionNet.Wpf.PocProject.ViewModels
{
    public class PocToolViewModel : ObservableObject, IToolViewModel, IDisposable
    {
        #region Variables
        private readonly IVisionMediator _visionMediator;
        private readonly IDisplayService _displayService;
        private readonly ICameraService _cameraService;
        private readonly IRuleToolService _ruleToolService;
        private readonly ILogManager _logService;
        private readonly System.Windows.Forms.FolderBrowserDialog _folderBrowserDialog = new();
        private readonly System.Windows.Forms.OpenFileDialog _openFileDialog = new();
        private readonly List<string> _imageList = [];
        private IVision _selectedVision;
        #endregion

        #region Properties
        public ILpDisplay LaonDisplay { get; }
        public IVision SelectedVision
        {
            get => _selectedVision;
            set => SetProperty(ref _selectedVision, value);
        }
        public bool UseGrabImage { get => _visionMediator.UseGrabImage; set => _visionMediator.UseGrabImage = value; }
        public bool IsSaveOriginImage { get => _selectedVision?.ToolGroup?.IsSaveOriginImage ?? false; set => _selectedVision.ToolGroup.IsSaveOriginImage = value; }
        public bool IsSaveOverlayImage { get; set; }
        public bool IsSaveCsv { get => _selectedVision?.ToolGroup?.IsSaveCsv ?? false; set => _selectedVision.ToolGroup.IsSaveCsv = value; }
        public ICommand ExecuteAllCommand { get; }
        public ICommand SelectDirectoryCommand { get; }
        public ICommand LoadImageFileCommand { get; }
        #endregion

        #region Events

        #endregion

        #region Constructor
        public PocToolViewModel(IVisionMediator visionMediator,
                                IDisplayService displayService,
                                IRuleToolService ruleToolService,
                                ILogManager logService)
        {
            _visionMediator = visionMediator;
            SelectedVision = _visionMediator.SelectedVision;
            _visionMediator.SelectedVisionChanged += VisionMediator_SelectedVisionChanged;
            _visionMediator.SelectedVisionRuleToolGroupCompleted += VisionMediator_SelectedRuleToolGroupCompleted;
            _visionMediator.SelectedVisionAiToolGroupCompleted += VisionMediator_SelectedAiToolGroupCompleted;
            _visionMediator.VisionRuleToolGroupCompleted += VisionMediator_VisionRuleToolGroupCompleted;

            _displayService = displayService;
            _ruleToolService = ruleToolService;
            LaonDisplay = _displayService.GetDisplay();
            _logService = logService;

            SelectDirectoryCommand = new RelayCommand(SelectDirectory);
            ExecuteAllCommand = new RelayCommand(ExecuteAll);
            LoadImageFileCommand = new RelayCommand(LoadImageFile);
        }

        private void VisionMediator_VisionRuleToolGroupCompleted(object sender, ILpToolResult e)
        {
            LaonDisplay.LaonResult = e;
        }
        #endregion

        #region Finalizer

        private bool disposed;
        ~PocToolViewModel()
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
                    DisposeVisionMediator();
                }

                //dispose of unmanaged resoureces

                disposed = true;
            }
        }

        public void DisposeVisionMediator()
        {
            _visionMediator.SelectedVisionChanged -= VisionMediator_SelectedVisionChanged;
            _visionMediator.SelectedVisionRuleToolGroupCompleted -= VisionMediator_SelectedRuleToolGroupCompleted;
            _visionMediator.SelectedVisionAiToolGroupCompleted -= VisionMediator_SelectedAiToolGroupCompleted;
        }
        #endregion

        #region Methods
        private void VisionMediator_SelectedVisionChanged(object sender, IVision e) => SelectedVision = e;
        private void VisionMediator_SelectedRuleToolGroupCompleted(object sender, ILpToolResult e)
        {
            if (LaonDisplay == null) return;
            LaonDisplay.LaonResult = e;

            if (IsSaveOverlayImage)
            {
                var img = LaonDisplay.GetOverlayImage();
                var imgPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OverlayImage");
                if (!Directory.Exists(imgPath)) Directory.CreateDirectory(imgPath);
                var filePath = Path.Combine(imgPath, $"{DateTime.Now:yyyyMMdd HHmmss.fff}.bmp");
                img.Save(filePath, ImageFormat.Bmp);
            }
        }
        private void VisionMediator_SelectedAiToolGroupCompleted(object sender, ILpToolResult e)
        {
            LaonDisplay.LaonImage = e.ResultImage;
        }
        private void SelectDirectory()
        {
            if (_folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                InitImageList(_folderBrowserDialog.SelectedPath);
            }
        }
        private void InitImageList(string folderPath)
        {
            if (!Directory.Exists(folderPath)) return;
            var list = Directory.GetFiles(folderPath);
            _imageList.Clear();
            for (int i = 0; i < list.Length; i++)
            {
                if (IsImageFile(list[i]))
                {
                    _imageList.Add(list[i]);
                }
            }
        }
        private void LoadImageFile()
        {
            if (UseGrabImage) return;
            if (_openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var currentImageFileName = _openFileDialog.FileName;
                if (IsImageFile(currentImageFileName))
                {
                    var img = _displayService.CreateImage(currentImageFileName);
                    _ruleToolService.CurrentToolGroup.InputImage = img;
                }
                else
                {
                    _logService.LogInfo(this, "The file is not an image");
                }
            }
        }

        // 이미지파일 확장자 검사
        private static bool IsImageFile(string imageFileName)
        {
            var extension = Path.GetExtension(imageFileName).ToLowerInvariant();
            return BitmapHelper.AvailableFormat.Contains(extension);
        }

        private void ExecuteAll()
        {
            for (int i = 0; i < _imageList.Count; i++)
            {
                var currentImageFileName = _imageList[i];
                if (IsImageFile(currentImageFileName))
                {
                    var img = _displayService.CreateImage(currentImageFileName);
                    _ruleToolService.CurrentToolGroup.Run(img);
                }
            }
        }
        #endregion
    }
}