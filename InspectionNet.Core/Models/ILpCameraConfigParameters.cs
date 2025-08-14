namespace InspectionNet.Core.Models
{
    public interface ILpCameraConfigParameters
    {
        string PixelFormat { get; set; }
        string TriggerMode { get; set; }
        string TriggerSource { get; set; }
        double Gain { get; set; }
        double ExposureTime { get; set; }
        string UserSetSelector { get; set; }
        string UserSetDefault { get; set; }
        string DeviceUserID { get; set; }
        int Width { get; set; }
        int Height { get; set; }
    }
}
