using System.Windows.Input;

using InspectionNet.Core.Models;

namespace InspectionNet.Wpf.Common.MainFrame.ViewModels
{
    public interface IConfigViewModel : IBaseViewModel
    {
        IConfigParameters ConfigParameters { get; }
        bool SaveImage { get; set; }
        string SaveImagePath { get; set; }
        ICommand SearchFolderCommand { get; }
    }
}
