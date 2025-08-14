namespace InspectionNet.Core.Configs
{
    public interface IVisionToolConfig
    {
        string AlignToolFilePath { get; set; }
        string CalibToolFilePath { get; set; }
        string RunToolFilePath { get; set; }
    }
}