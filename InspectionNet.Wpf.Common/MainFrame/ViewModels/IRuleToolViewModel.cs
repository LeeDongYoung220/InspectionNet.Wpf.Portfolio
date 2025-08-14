using System;
using System.Windows.Input;

using InspectionNet.Core.Models;
using InspectionNet.Core.VisionTools;

namespace InspectionNet.Wpf.Common.MainFrame.ViewModels
{
    public interface IRuleToolViewModel
    {
        string CameraId { get; }
        ICommand OpenCalibToolCommand { get; }
        ICommand OpenToolBlockCommand { get; }
        ICommand RuleRunCommand { get; }
    }
}