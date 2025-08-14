using System.Windows.Controls;

using InspectionNet.Core.Models;
using InspectionNet.Core.VisionTools;

namespace InspectionNet.Wpf.Common.Models
{
    public interface IWpfToolGroup : ILpRuleToolGroup
    {
        Control ToolBlockEditControl { get; }
        Control CalibCheckerboardEditControl { get; }
    }
}
