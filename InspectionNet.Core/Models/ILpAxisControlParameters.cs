using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InspectionNet.Core.Models
{
    public interface ILpAxisControlParameters
    {
        double TargetPositionAbs { get; set; }
        double TargetPositionRel { get; set; }
        double UserVelocity { get; set; }
        double UserAcceleration { get; set; }
        double UserDeceleration { get; set; }

        ICommand PositionClearCommand { get; }
        ICommand HomeStartCommand { get; }
        ICommand MoveStopCommand { get; }
        ICommand MovePositiveCommand { get; }
        ICommand MoveNegativeCommand { get; }
        ICommand MoveTargetPositionCommand { get; }

        bool ServoOn();
        bool ServoOff();
    }
}
