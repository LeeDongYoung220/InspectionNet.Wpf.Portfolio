using System;

using CommunityToolkit.Mvvm.DependencyInjection;

using InspectionNet.Core.Managers;
using InspectionNet.Core.Mediators;
using InspectionNet.Core.Services;

using InspectionNet.Wpf.Common.MainFrame.Services;
using InspectionNet.Wpf.Common.MainFrame.ViewModels;
using InspectionNet.Wpf.Common.Models;
using InspectionNet.Wpf.Common.Views;
using InspectionNet.Wpf.MainFrame.Services;
using InspectionNet.Wpf.MainFrame.ViewModels;
using InspectionNet.Wpf.PocProject.Mediators;
using InspectionNet.Wpf.PocProject.Models;
using InspectionNet.Wpf.PocProject.Services;
using InspectionNet.Wpf.PocProject.ViewModels;
using InspectionNet.Wpf.PocProject.Views;

using Microsoft.Extensions.DependencyInjection;
using InspectionNet.CameraComponent.TestModule.Services;
using InspectionNet.LightComponent.TestModule.Services;
using InspectionNet.EnvironmentTools.Logger.Services;
using InspectionNet.Winform.Common.Services;
using System.Runtime.Versioning;
using InspectionNet.Wpf.VisionTool.CognexModule.Services;
using InspectionNet.VisionTool.TestAiModule.Services;
using InspectionNet.MotionComponent.TestModule.Services;

namespace InspectionNet.Wpf.MainFrame
{
    internal static class Bootstrapper
    {
        private static ServiceProvider _serviceProvider;

        [SupportedOSPlatform("windows")]
        public static void ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IViewNavigationService, ViewNavigationService>();
            serviceCollection.AddTransient<IMainViewModel, MainViewModel>();
            serviceCollection.AddSingleton<IMenuBarControl, PocSideMenuBarControl>();
            serviceCollection.AddSingleton<IMenuBarViewModel, PocMenuBarViewModel>();
            serviceCollection.AddSingleton<ICameraViewModel, PocCameraViewModel>();
            serviceCollection.AddTransient<ICameraView, PocCameraView>();

            RegisterPoc(serviceCollection);
            //RegisterMock(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
            Ioc.Default.ConfigureServices(_serviceProvider);

            _ = _serviceProvider.GetRequiredService<IHomeView>();
            _ = _serviceProvider.GetRequiredService<ICameraView>();
            _ = _serviceProvider.GetRequiredService<IMotionView>();
            _ = _serviceProvider.GetRequiredService<IToolView>();
        }

        public static void CleanupServices()
        {
            _serviceProvider.Dispose();
        }

        private static void RegisterPoc(ServiceCollection serviceCollection)
        {
            // managers
            serviceCollection.AddSingleton<ILogManager, LogManager>();

            // services
            serviceCollection.AddSingleton<IViewNavigationService, ViewNavigationService>();
            serviceCollection.AddSingleton<IConfigService, PocConfigService>();
            serviceCollection.AddSingleton<ICameraService, TestCameraService>();
            serviceCollection.AddSingleton<ILightService, TestLightService>();
            serviceCollection.AddSingleton<IDisplayService, CogDisplayService>();
            serviceCollection.AddSingleton<IRuleToolService, CogToolService>();
            serviceCollection.AddSingleton<IAiToolService, TestAiToolService>();
            serviceCollection.AddSingleton<IMotionService, TestMotionService>();
            serviceCollection.AddSingleton<IFileDialogService, FileDialogService>();

            // mediators
            serviceCollection.AddSingleton<IVisionMediator, VisionMediator>();
            serviceCollection.AddSingleton<ISequence, MockSequence>();

            // viewmodels
            serviceCollection.AddTransient<IMainViewModel, MainViewModel>();
            serviceCollection.AddTransient<ILogoViewModel, PocLogoViewModel>();
            serviceCollection.AddTransient<IMenuBarViewModel, PocMenuBarViewModel>();
            serviceCollection.AddSingleton<IHomeViewModel, PocHomeViewModel>();
            serviceCollection.AddSingleton<ICameraViewModel, PocCameraViewModel>();
            serviceCollection.AddSingleton<ILightViewModel, PocLightViewModel>();
            serviceCollection.AddSingleton<IConfigViewModel, PocConfigViewModel>();
            serviceCollection.AddSingleton<IToolViewModel, PocToolViewModel>();
            serviceCollection.AddSingleton<ICommunicationViewModel, PocCommunicationViewModel>();
            serviceCollection.AddSingleton<IMotionViewModel, PocMotionViewModel>();
            serviceCollection.AddSingleton<IRuleToolViewModel, PocRuleToolViewModel>();
            serviceCollection.AddSingleton<IAiToolViewModel, PocAiToolViewModel>();

            // views
            serviceCollection.AddTransient<ILogoControl, PocLogoControl>();
            serviceCollection.AddTransient<IMenuBarControl, PocSideMenuBarControl>();
            serviceCollection.AddTransient<IHomeView, PocHomeView>();
            serviceCollection.AddTransient<ICameraView, PocCameraView>();
            serviceCollection.AddTransient<ILightView, PocLightView>();
            serviceCollection.AddTransient<IConfigView, PocConfigView>();
            serviceCollection.AddTransient<IToolView, PocToolView>();
            serviceCollection.AddTransient<ICommunicationView, PocCommunicationView>();
            serviceCollection.AddTransient<IMotionView, PocMotionView>();
        }
    }
}
