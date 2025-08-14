namespace InspectionNet.Core.Models
{
    public interface ILpAxisSoftwareLimitParameters
    {
        double NegSoftLimit { get; set; }
        double PosSoftLimit { get; set; }
        uint SoftLimitSel { get; set; }
        uint SoftLimitStopMode { get; set; }
        uint SoftLimitEnable { get; set; }
    }
}