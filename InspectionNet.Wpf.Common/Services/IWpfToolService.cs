using System.Windows.Controls;

using InspectionNet.Core.Services;

namespace InspectionNet.Wpf.Common.Services
{
    public interface IWpfToolService : IRuleToolService
    {
        Control ToolSequenceControl { get; }
        Control CalibCheckerboardEditControl { get; }
    }
}
