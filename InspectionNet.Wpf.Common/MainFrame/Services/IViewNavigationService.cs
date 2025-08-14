using InspectionNet.Wpf.Common.Views;

namespace InspectionNet.Wpf.Common.MainFrame.Services
{
    public interface IViewNavigationService
    {
        event EventHandler<ISubView> SubViewChanged;

        IBaseControl LogoControl();
        IBaseControl MenuBarControl();
        void ChangeHomeView();
        void ChangeCameraView();
        void ChangeLightView();
        void ChangeConfigView();
        void ChangeToolView();
        void ChangeCommunicationView();
        void ChangeMotionView();
    }
}
