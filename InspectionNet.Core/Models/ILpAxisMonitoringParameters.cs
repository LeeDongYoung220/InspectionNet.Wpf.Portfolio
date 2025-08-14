using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InspectionNet.Core.Models
{
    public interface ILpAxisMonitoringParameters
    {
        bool IsServoOn { get; set; }
        bool IsAlarm { get; }
        bool IsInMotion { get; }
        bool IsInPosition { get; }
        bool IsInHome { get; }
        string HomeResult { get; }
        uint HomeRate { get; }
        bool IsPositiveLimit { get; }
        bool IsNegativeLimit { get; }
        bool EmergencyStop { get; }
        double CommandPosition { get; }
        double ActualPosition { get; }
        double CommandVelocity { get; }
        double PositionError { get; }
        string AlarmCode { get; }

        event EventHandler<int> HomeCompleted;
        event EventHandler<int> MotionCompleted;

        void UpdateParameters();
    }
}
