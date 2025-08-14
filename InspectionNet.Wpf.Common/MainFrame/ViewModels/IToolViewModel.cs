using System.Windows.Input;

using InspectionNet.Core.Views;

namespace InspectionNet.Wpf.Common.MainFrame.ViewModels
{
    public interface IToolViewModel : IBaseViewModel
    {
        ILpDisplay LaonDisplay { get; }
        bool UseGrabImage { get; set; }
        bool IsSaveOriginImage { get; set; }
        bool IsSaveOverlayImage { get; set; }
        bool IsSaveCsv { get; set; }

        ICommand ExecuteAllCommand { get; }
        ICommand SelectDirectoryCommand { get; }
        ICommand LoadImageFileCommand { get; }
    }
}
