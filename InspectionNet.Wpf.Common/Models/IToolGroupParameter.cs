using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionNet.Wpf.Common.Models
{
    public interface IToolGroupParameter
    {
        bool IsCalibrationMode { get; set; }
        bool IsSaveCsv { get; set; }
        bool IsSaveOriginImage { get; set; }
    }
}
