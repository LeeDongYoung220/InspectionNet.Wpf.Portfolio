using CommunityToolkit.Mvvm.DependencyInjection;

namespace InspectionNet.Wpf.Common.MainFrame.ViewModels.Implementations
{
    public class ViewModelLocator
    {
        public static IMainViewModel Main => Ioc.Default.GetRequiredService<IMainViewModel>();
        public static ILogoViewModel Logo => Ioc.Default.GetRequiredService<ILogoViewModel>();
        public static IMenuBarViewModel MenuBar => Ioc.Default.GetRequiredService<IMenuBarViewModel>();
        public static IHomeViewModel Home => Ioc.Default.GetRequiredService<IHomeViewModel>();
        public static ICameraViewModel Camera => Ioc.Default.GetRequiredService<ICameraViewModel>();
        public static ILightViewModel Light => Ioc.Default.GetRequiredService<ILightViewModel>();
        public static IConfigViewModel Config => Ioc.Default.GetRequiredService<IConfigViewModel>();
        public static IToolViewModel Tool => Ioc.Default.GetRequiredService<IToolViewModel>();
        public static ICommunicationViewModel Communication => Ioc.Default.GetRequiredService<ICommunicationViewModel>();
        public static IMotionViewModel Motion => Ioc.Default.GetRequiredService<IMotionViewModel>();
        public static IRuleToolViewModel RuleTool => Ioc.Default.GetRequiredService<IRuleToolViewModel>();
        public static IAiToolViewModel AiTool => Ioc.Default.GetRequiredService<IAiToolViewModel>();

    }
}
