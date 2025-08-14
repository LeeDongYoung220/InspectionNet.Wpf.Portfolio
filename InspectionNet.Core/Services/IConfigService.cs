using System;

using InspectionNet.Core.Configs;
using InspectionNet.Core.Models;

namespace InspectionNet.Core.Services
{
    public interface IConfigService
    {
        event EventHandler<ILpCameraConfigParameters> CameraConfigLoaded;
        event EventHandler CameraConfigSaved;
        event EventHandler<ILightControllerConfigParameters> LightControllerConfigLoaded;
        event EventHandler LightControllerConfigSaved;

        IConfigParameters GetConfigParameters();
        void LoadCameraConfigFile();
        void SaveCameraConfigFile(ILpCameraConfigParameters parameters);
        void LoadLightControllerConfigFile();
        void SaveLightControllerConfigFile(ILightControllerConfigParameters parameters);
        void LoadConfigFile();
        void SaveConfigFile();
        IVisionToolConfig GetVisionToolConfig();
        IMotionConfig GetMotionConfig();
    }
}
