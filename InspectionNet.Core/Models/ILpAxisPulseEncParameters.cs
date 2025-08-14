namespace InspectionNet.Core.Models
{
    public interface ILpAxisPulseEncParameters
    {
        uint PulseOutMethod { get; set; }
        uint EncInputMethod { get; set; }
        double MinVelocity { get; set; }
        double MaxVelocity { get; set; }
        int MovePulse { get; set; }
        double MoveUnit { get; set; }
        uint InitAbsRelMode { get; set; }
        uint InitProfileMode { get; set; }
    }
}