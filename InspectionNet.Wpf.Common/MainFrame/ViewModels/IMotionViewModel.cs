using System.Collections.ObjectModel;
using System.Windows.Input;

using InspectionNet.Core.Models;
using InspectionNet.Core.Views;

using InspectionNet.Wpf.Common.Models;

namespace InspectionNet.Wpf.Common.MainFrame.ViewModels
{
    public interface IMotionViewModel
    {
        IEnumerable<ILpAxis> AxisList { get; }
        ObservableCollection<ILpAxisPosition> InitPositions { get; }
        ObservableCollection<ILpAxisPosition> RunPositions { get; }
        ILpAxisPosition SelectedInitPosition { get; set; }
        ILpAxisPosition SelectedRunPosition { get; set; }
        string SelectedInitPositionName { get; set; }
        string SelectedRunPositionName { get; set; }
        string SelectedPositionName { get; set; }
        IPositionGenerator PositionGenerator { get; set; }
        ILpDisplay LaonDisplay { get; set; }

        ICommand JogMoveLeftCommand { get; }
        ICommand JogMoveRightCommand { get; }
        ICommand JogMoveUpCommand { get; }
        ICommand JogMoveDownCommand { get; }
        ICommand JogMoveXStopCommand { get; }
        ICommand JogMoveYStopCommand { get; }
        ICommand LoadConfigFileCommand { get; }
        ICommand AddInitPositionCommand { get; }
        ICommand RemoveInitPositionCommand { get; }
        ICommand AddRunPositionCommand { get; }
        ICommand RemoveRunPositionCommand { get; }
        bool IsToolRun { get; set; }
        ICommand RemoveAllRunPositionCommand { get; }
    }
}
