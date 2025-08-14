using System.Windows.Input;

namespace InspectionNet.Wpf.Common.Models
{
    public interface IPositionGenerator
    {
        double StartX { get; set; }
        double StartY { get; set; }
        int Rows { get; set; }
        int Columns { get; set; }
        double PitchX { get; set; }
        double PitchY { get; set; }
        bool ReverseDirMode { get; set; }
        ICommand RunCommand { get; }
    }
}
