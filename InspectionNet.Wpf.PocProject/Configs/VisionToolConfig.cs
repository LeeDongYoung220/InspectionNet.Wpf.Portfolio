using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InspectionNet.Core.Configs;

namespace InspectionNet.Wpf.PocProject.Configs
{
    public class VisionToolConfig : IVisionToolConfig
    {
        public string AlignToolFilePath { get; set; } = "Settings\\Fiducial.vpp";
        public string RunToolFilePath { get; set; } = "Settings\\Run.vpp";
        public string CalibToolFilePath { get; set; } = "Settings\\CheckerboardCalib.vpp";

    }
}
