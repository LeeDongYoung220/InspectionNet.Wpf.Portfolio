using System.ComponentModel;
using System.Runtime.CompilerServices;

using InspectionNet.Core.Models;

namespace InspectionNet.Wpf.PocProject.Models
{
    public class PocConfigParameters : IConfigParameters
    {
        private string _savePath = string.Empty;
        public string SaveImagePath
        {
            get => _savePath;
            set
            {
                if (_savePath != value)
                {
                    _savePath = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _saveOriginImage;
        public bool SaveOriginImage
        {
            get => _saveOriginImage;
            set
            {
                if (_saveOriginImage != value)
                {
                    _saveOriginImage = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _saveOverlayImage;
        public bool SaveOverlayImage
        {
            get => _saveOverlayImage;
            set
            {
                if (_saveOverlayImage != value)
                {
                    _saveOverlayImage = value;
                }
                OnPropertyChanged();
            }
        }

        private string _loadCameraConfigFilePath = string.Empty;
        public string LoadCameraConfigFilePath
        {
            get => _loadCameraConfigFilePath;
            set
            {
                if (_loadCameraConfigFilePath != value)
                {
                    _loadCameraConfigFilePath = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _saveCameraConfigFilePath = string.Empty;
        public string SaveCameraConfigFilePath
        {
            get => _saveCameraConfigFilePath;
            set
            {
                if (_saveCameraConfigFilePath != value)
                {
                    _saveCameraConfigFilePath = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _loadLightControllerConfigFilePath = string.Empty;
        public string LoadLightControllerConfigFilePath
        {
            get => _loadLightControllerConfigFilePath;
            set
            {
                if (_loadLightControllerConfigFilePath != value)
                {
                    _loadLightControllerConfigFilePath = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _saveLightControllerConfigFilePath = string.Empty;
        public string SaveLightControllerConfigFilePath
        {
            get => _saveLightControllerConfigFilePath;
            set
            {
                if (_saveLightControllerConfigFilePath != value)
                {
                    _saveLightControllerConfigFilePath = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _loadCalibrationFilePath = "Calibration\\CheckerboardCalibration.vpp";
        public string LoadCalibrationFilePath
        {
            get => _loadCalibrationFilePath;
            set
            {
                if (_loadCalibrationFilePath != value)
                {
                    _loadCalibrationFilePath = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void LoadConfigFile(IConfigParameters parameters)
        {
            SaveImagePath = parameters.SaveImagePath;
            SaveOriginImage = parameters.SaveOriginImage;
            SaveOverlayImage = parameters.SaveOverlayImage;
            LoadCameraConfigFilePath = parameters.LoadCameraConfigFilePath;
            SaveCameraConfigFilePath = parameters.SaveCameraConfigFilePath;
            LoadLightControllerConfigFilePath = parameters.LoadLightControllerConfigFilePath;
            SaveLightControllerConfigFilePath = parameters.SaveLightControllerConfigFilePath;
            LoadCalibrationFilePath = parameters.LoadCalibrationFilePath;
        }
    }
}
