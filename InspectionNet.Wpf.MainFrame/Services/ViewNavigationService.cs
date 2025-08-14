using System;

using CommunityToolkit.Mvvm.DependencyInjection;

using InspectionNet.Wpf.Common.MainFrame.Services;
using InspectionNet.Wpf.Common.Views;

using Microsoft.Extensions.DependencyInjection;

namespace InspectionNet.Wpf.MainFrame.Services
{
    internal class ViewNavigationService : IViewNavigationService
    {
        public event EventHandler<ISubView> SubViewChanged;

        private static IBaseControl GetUserControl<T>() where T : IBaseControl => Ioc.Default.GetRequiredService<T>();

        private void ChangeSubView<T>() where T : ISubView
        {
            var subView = Ioc.Default.GetRequiredService<T>();
            SubViewChanged?.Invoke(this, subView);
        }

        public IBaseControl LogoControl() => GetUserControl<ILogoControl>();

        public IBaseControl MenuBarControl() => GetUserControl<IMenuBarControl>();

        public void ChangeHomeView() => ChangeSubView<IHomeView>();

        public void ChangeCameraView() => ChangeSubView<ICameraView>();

        public void ChangeLightView() => ChangeSubView<ILightView>();

        public void ChangeConfigView() => ChangeSubView<IConfigView>();

        public void ChangeToolView() => ChangeSubView<IToolView>();

        public void ChangeMotionView() => ChangeSubView<IMotionView>();

        public void ChangeCommunicationView() => ChangeSubView<ICommunicationView>();
    }
}
