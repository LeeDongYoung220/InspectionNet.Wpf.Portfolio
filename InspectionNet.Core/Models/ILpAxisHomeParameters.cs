namespace InspectionNet.Core.Models
{
    public interface ILpAxisHomeParameters
    {
        uint HomeLevel { get; set; }
        int HomeDir { get; set; }
        uint HomeSignal { get; set; }
        uint ZPhaseUse { get; set; }
        double HomeClearTime { get; set; }
        double HomeOffset { get; set; }
        double HomeFirstVelocity { get; set; }
        double HomeSecondVelocity { get; set; }
        double HomeThirdVelocity { get; set; }
        double HomeLastVelocity { get; set; }
        double HomeFirstAccel { get; set; }
        double HomeSecondAccel { get; set; }
        uint HomeResult { get; }
        uint HomeRate { get; }
    }
}