namespace InspectionNet.Core.Models
{
    public interface ILpAxisUserMoveParameters
    {
        double InitPosition { get; set; }
        double InitVelocity { get; set; }
        double InitAccel { get; set; }
        double InitDecel { get; set; }
    }
}