using System.Windows.Input;
using System.Windows.Media.Imaging;

using InspectionNet.Core.Views;

namespace InspectionNet.Wpf.Common.MainFrame.ViewModels
{
    public interface IHomeViewModel : IBaseViewModel
    {
        bool IsInitializedCamera { get; }
        bool IsInitializedTool { get; }
        bool IsInitializedMotion { get; }
        BitmapImage GrabImage { get; set; }
        bool IsToolRun { get; set; }
        bool IsInitMotion { get; set; }
        ILpDisplay LaonDisplay { get; }

        ICommand InitComponentCommand { get; }
        ICommand AlignSequenceCommand { get; }
        ICommand RunSequenceCommand { get; }
        ICommand SequenceStopCommand { get; }
        ICommand LiveStartCameraCommand { get; }
        ICommand LiveStopCameraCommand { get; }
    }
}
