using System;
using System.Collections.Generic;
using System.IO.Ports;

using InspectionNet.Core.Models;

namespace InspectionNet.Core.Services
{
    public interface ILightService
    {
        List<string>? PortNames { get; }
        IList<int> BaudRates { get; }
        IList<int> DataBits { get; }
        IList<Parity> Parities { get; }
        IList<StopBits> StopBits { get; }

        event EventHandler<IEnumerable<string>>? PortListChanged;
        event EventHandler<SerialPort>? PortOpened;
        event EventHandler? PortClosed;

        void SearchPort();
        ILightController GetLightController();
    }
}
