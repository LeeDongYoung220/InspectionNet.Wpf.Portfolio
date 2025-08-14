using System.ComponentModel;

namespace InspectionNet.Core.Models
{
    public interface IConfigParameters : INotifyPropertyChanged
    {
        string SaveImagePath { get; set; }
        bool SaveOriginImage { get; set; }
        bool SaveOverlayImage { get; set; }
        string LoadCameraConfigFilePath { get; set; }
        string SaveCameraConfigFilePath { get; set; }
        string LoadLightControllerConfigFilePath { get; set; }
        string SaveLightControllerConfigFilePath { get; set; }
        string LoadCalibrationFilePath { get; set; }

        void LoadConfigFile(IConfigParameters parameters);
    }
}
