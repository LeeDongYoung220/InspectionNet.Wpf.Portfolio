namespace InspectionNet.Core.Models
{
    public interface ILpAxisInOutParameters
    {
        uint InPosition { get; set; }
        uint Alarm { get; set; }
        uint NegEndLimit { get; set; }
        uint PosEndLimit { get; set; }
        uint ZPhaseLevel { get; set; }
        uint StopSignalMode { get; set; }
        uint StopSignalLevel { get; set; }
        uint SvOnLevel { get; set; }
        uint AlarmResetLevel { get; set; }
        uint EncoderType { get; set; }
    }
}