using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InspectionNet.Core.Configs;

namespace InspectionNet.Wpf.PocProject.Configs
{
    public class MotionConfig : IMotionConfig
    {
        public string MotFilePath { get; set; } = $@"{AppDomain.CurrentDomain.BaseDirectory}Settings\Default.mot";
        public string MotionConfigFilePath { get; set; } = $@"{AppDomain.CurrentDomain.BaseDirectory}Settings\MotionConfig.json";
    }
}
