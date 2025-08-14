using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InspectionNet.Core.Models
{
    public interface ILpAxis : ILpAxisMonitoringParameters, ILpAxisControlParameters
    {
        int Index { get; }
        string Name { get; }
        ILpAxisParameters Parameters { get; }
    }
}
