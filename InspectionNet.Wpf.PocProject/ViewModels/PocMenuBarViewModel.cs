using System;

using CommunityToolkit.Mvvm.ComponentModel;

using InspectionNet.Wpf.Common.MainFrame.Services;
using InspectionNet.Wpf.Common.MainFrame.ViewModels;

using MaterialDesignThemes.Wpf;

namespace InspectionNet.Wpf.PocProject.ViewModels
{
    public class PocMenuBarViewModel(IViewNavigationService viewNavigationService) : ObservableObject, IMenuBarViewModel
    {
        #region Variables
        private readonly IViewNavigationService _viewNavigationService = viewNavigationService;
        private int _selectedMenuIndex = -1;
        private bool isDarkTheme = GetBaseTheme() == BaseTheme.Dark;
        #endregion

        #region Properties
        public int SelectedMenuIndex
        {
            get => _selectedMenuIndex;
            set
            {
                if (SetProperty(ref _selectedMenuIndex, value))
                {
                    switch (_selectedMenuIndex)
                    {
                        case 0: _viewNavigationService.ChangeHomeView(); break;
                        case 1: _viewNavigationService.ChangeCameraView(); break;
                        case 2: _viewNavigationService.ChangeLightView(); break;
                        case 3: _viewNavigationService.ChangeConfigView(); break;
                        case 4: _viewNavigationService.ChangeToolView(); break;
                        case 5: _viewNavigationService.ChangeMotionView(); break;
                        case 6: _viewNavigationService.ChangeCommunicationView(); break;
                    }
                }
            }
        }

        public bool IsDarkTheme
        {
            get => isDarkTheme;
            set
            {
                if (SetProperty(ref isDarkTheme, value))
                {
                    ModifyTheme(t => t.SetBaseTheme(value ? BaseTheme.Dark : BaseTheme.Light));
                }
            }
        }

        #endregion

        #region Constructor
        #endregion

        #region Finalizer

        #endregion

        #region Methods
        private static void ModifyTheme(Action<Theme> modificationAction)
        {
            var paletteHelper = new PaletteHelper();
            Theme theme = paletteHelper.GetTheme();
            modificationAction?.Invoke(theme);
            paletteHelper.SetTheme(theme);
        }

        private static BaseTheme GetBaseTheme()
        {
            var paletteHelper = new PaletteHelper();
            return paletteHelper.GetTheme().GetBaseTheme();
        }
        #endregion
    }
}