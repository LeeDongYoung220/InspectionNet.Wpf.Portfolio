using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InspectionNet.Core.Models;
using InspectionNet.Core.Services;
using InspectionNet.LightComponent.TestModule.Models;

namespace InspectionNet.LightComponent.TestModule.Services
{
    public class TestLightService : ILightService
    {
        private readonly ILightController _lightController;

        public List<string> PortNames { get; private set; } = [];
        public IList<int> BaudRates { get; } = [9600, 14400, 19200, 38400, 57600, 115200];
        public IList<int> DataBits { get; } = [4, 5, 6, 7, 8];
        public IList<Parity> Parities { get; } = (IList<Parity>)Enum.GetValues(typeof(Parity));
        public IList<StopBits> StopBits { get; } = (IList<StopBits>)Enum.GetValues(typeof(StopBits));

        public event EventHandler<IEnumerable<string>>? PortListChanged;
        public event EventHandler<SerialPort>? PortOpened;
        public event EventHandler? PortClosed;

        public TestLightService()
        {
            SearchPort();
            _lightController = new LightController();
            ConnectEvents();
        }

        private void ConnectEvents()
        {
            _lightController.SerialPortOpened += LightController_SerialPortOpened;
            _lightController.SerialPortClosed += LightController_SerialPortClosed;
        }

        private void LightController_SerialPortOpened(object? sender, SerialPort e) => PortOpened?.Invoke(sender, e);
        private void LightController_SerialPortClosed(object? sender, EventArgs e) => PortClosed?.Invoke(sender, e);

        public ILightController GetLightController() => _lightController;

        public void SearchPort()
        {
            PortNames = [.. SerialPort.GetPortNames()];
            PortListChanged?.Invoke(this, PortNames);
        }
    }
}
