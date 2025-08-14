using InspectionNet.Wpf.Common.Views;

namespace InspectionNet.Wpf.Common.MainFrame.ViewModels
{
    public interface IMainViewModel : IBaseViewModel
    {
        IBaseControl LogoControl { get; }
        IBaseControl MenuBarControl { get; }
        ISubView CurrentSubView { get; set; }
    }
}
