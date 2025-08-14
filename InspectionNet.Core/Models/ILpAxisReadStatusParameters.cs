using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionNet.Core.Models
{
    public interface ILpAxisReadStatusParameters
    {
        uint IsServoOn { get; }
        uint IsInPosition { get; }
        uint IsInMotion { get; }
        uint IsInHome { get; }
        uint IsAlarm { get; }
        ushort AlarmCode { get; }
        uint IsPositiveLimit { get; }
        uint IsNegativeLimit { get; }
        uint IsStopSignal { get; }
        double CommandPosition { get; }
        double ActualPosition { get; }
        double PositionError { get; }
        double CommandVelocity { get; }
    }
}
