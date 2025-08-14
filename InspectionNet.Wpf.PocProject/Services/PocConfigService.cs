using System;
using System.IO;

using InspectionNet.Core.Configs;
using InspectionNet.Core.Implementations;
using InspectionNet.Core.Models;
using InspectionNet.Core.Services;
using InspectionNet.Core.StaticClasses;
using InspectionNet.Wpf.PocProject.Configs;
using InspectionNet.Wpf.PocProject.Models;

namespace InspectionNet.Wpf.PocProject.Services
{
    public class PocConfigService : IConfigService
    {
        private const string CONFIG_FILE_PATH = "Config\\Config.json";
        private const string VISION_TOOL_FILE_PATH = "Config\\VisionToolConfig.json";
        private readonly PocConfigParameters _configParameters = new();
        private IVisionToolConfig _visionToolConfig;
        private MotionConfig _motionConfig;

        public event EventHandler<ILpCameraConfigParameters> CameraConfigLoaded;
        public event EventHandler CameraConfigSaved;
        public event EventHandler<ILightControllerConfigParameters> LightControllerConfigLoaded;
        public event EventHandler LightControllerConfigSaved;

        public PocConfigService()
        {
            InitVisionToolConfig();
            InitMotionConfig();
        }

        private void InitMotionConfig()
        {
            _motionConfig = new MotionConfig();
        }

        private void InitVisionToolConfig()
        {
            if (File.Exists(VISION_TOOL_FILE_PATH))
            {
                _visionToolConfig = JsonHelper.LoadObject<VisionToolConfig>(VISION_TOOL_FILE_PATH);
            }
            else
            {
                _visionToolConfig = new VisionToolConfig();
            }   
        }

        public IConfigParameters GetConfigParameters() => _configParameters;
        public IVisionToolConfig GetVisionToolConfig() => _visionToolConfig;

        public void LoadCameraConfigFile()
        {
            string cameraConfigFilePath = _configParameters.LoadCameraConfigFilePath;
            if (string.IsNullOrEmpty(cameraConfigFilePath)) 
                throw new FileNotFoundException("Path is null", nameof(cameraConfigFilePath));
            var jsonObj = JsonHelper.LoadObject<LaonCameraConfigParameters>(_configParameters.LoadCameraConfigFilePath);
            CameraConfigLoaded?.Invoke(this, jsonObj);
        }

        public void SaveCameraConfigFile(ILpCameraConfigParameters parameters)
        {
            if (string.IsNullOrEmpty(_configParameters.SaveCameraConfigFilePath)) throw new FileNotFoundException("Path is null");
            JsonHelper.SaveObject(_configParameters.SaveCameraConfigFilePath, parameters);
            CameraConfigSaved?.Invoke(this, EventArgs.Empty);
        }

        public void LoadLightControllerConfigFile()
        {
            if (string.IsNullOrEmpty(_configParameters.LoadLightControllerConfigFilePath)) throw new FileNotFoundException("Path is null");
            var jsonObj = JsonHelper.LoadObject<LaonLightControllerConfigParameters>(_configParameters.LoadLightControllerConfigFilePath);
            LightControllerConfigLoaded?.Invoke(this, jsonObj);
        }

        public void SaveLightControllerConfigFile(ILightControllerConfigParameters parameters)
        {
            if (string.IsNullOrEmpty(_configParameters.SaveLightControllerConfigFilePath)) throw new FileNotFoundException("Path is null");
            JsonHelper.SaveObject(_configParameters.SaveLightControllerConfigFilePath, parameters);
            LightControllerConfigSaved?.Invoke(this, EventArgs.Empty);
        }

        public void LoadConfigFile()
        {
            if (!File.Exists(CONFIG_FILE_PATH)) throw new FileNotFoundException(CONFIG_FILE_PATH);
            var jsonObj = JsonHelper.LoadObject<PocConfigParameters>(CONFIG_FILE_PATH);
            _configParameters.LoadConfigFile(jsonObj);
        }

        public void SaveConfigFile()
        {
            if (!Directory.Exists(Path.GetDirectoryName(CONFIG_FILE_PATH)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(CONFIG_FILE_PATH));
            }
            JsonHelper.SaveObject(CONFIG_FILE_PATH, _configParameters);
        }

        public IMotionConfig GetMotionConfig()
        {
            return _motionConfig;
        }
    }
}
