using InspectionNet.Wpf.Common.Models;

namespace InspectionNet.Wpf.PocProject.Models
{
    public class FilePathManager : IFilePathManager
    {
        public string AlignToolBlockFilePath => "Settings\\Fiducial.vpp";
        public string CalibToolFilePath => "Settings\\CheckerboardCalib.vpp";

    }
}
