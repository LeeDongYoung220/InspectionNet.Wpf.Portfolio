using System;

using InspectionNet.Core.Models;

namespace InspectionNet.Core.Implementations
{
    public class LaonCameraConfigParameters : ILpCameraConfigParameters
    {
        public string PixelFormat { get; set; } = string.Empty;
        public string TriggerMode { get; set; } = string.Empty;
        public string TriggerSource { get; set; } = string.Empty;
        public double Gain { get; set; }
        public double ExposureTime { get; set; }
        public string UserSetSelector { get; set; } = string.Empty;
        public string UserSetDefault { get; set; } = string.Empty;
        public string DeviceUserID { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }

        public LaonCameraConfigParameters()
        {

        }

        public LaonCameraConfigParameters(ILpCameraParameters parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            PixelFormat = parameters.PixelFormat;
            TriggerMode = parameters.TriggerMode;
            TriggerSource = parameters.TriggerSource;
            Gain = parameters.Gain;
            ExposureTime = parameters.ExposureTime;
            UserSetSelector = parameters.UserSetSelector;
            UserSetDefault = parameters.UserSetDefault;
        }
    }
}
