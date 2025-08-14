using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;

using InspectionNet.Wpf.Common.MainFrame.Services;
using InspectionNet.Wpf.Common.MainFrame.ViewModels;
using InspectionNet.Wpf.Common.Views;

namespace InspectionNet.Wpf.MainFrame.ViewModels
{
    public class MainViewModel : ObservableObject, IMainViewModel
    {
        private readonly IViewNavigationService _viewNavigationService;

        public IBaseControl LogoControl { get; }
        public IBaseControl MenuBarControl { get; }

        private ISubView _currentSubView;
        public ISubView CurrentSubView
        {
            get => _currentSubView;
            //set => _currentSubView = value;
            set => SetProperty(ref _currentSubView, value);
        }

        public MainViewModel(IViewNavigationService viewNavigationService)
        {
            _viewNavigationService = viewNavigationService;
            _viewNavigationService.SubViewChanged += ViewNavigationService_SubViewChanged;

            LogoControl = _viewNavigationService.LogoControl();
            MenuBarControl = _viewNavigationService.MenuBarControl();
        }

        private async void ViewNavigationService_SubViewChanged(object sender, ISubView e)
        {
            if (CurrentSubView != null) (CurrentSubView as Control).Visibility = Visibility.Collapsed;
            await Task.Delay(10);
            CurrentSubView = e;
        }
    }
}
