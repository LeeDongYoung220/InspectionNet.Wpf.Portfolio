using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using InspectionNet.Core.Models;

namespace InspectionNet.LightComponent.TestModule.Models
{
    internal class LightController : ILightController
    {
        private readonly SerialPort _port;

        public List<ILightProtocol>? LightProtocols { get; }
        public ILightProtocol? SelectedLightProtocol { get; set; }
        public bool IsOpen { get; }
        public string PortName { get; set; } = string.Empty;
        public int BaudRate { get; set; }
        public int DataBits { get; set; }
        public Parity Parity { get; set; }
        public StopBits StopBits { get; set; }

        public event EventHandler<SerialPort>? SerialPortOpened;
        public event EventHandler? SerialPortClosed;
        public event EventHandler<string>? SerialPortDataSent;
        public event EventHandler<string>? SerialPortDataReceived;
        public event PropertyChangedEventHandler? PropertyChanged;

        public LightController()
        {
            _port = new SerialPort();
        }

        public void Close()
        {
            SerialPortClosed?.Invoke(this, EventArgs.Empty);
            MessageBox.Show("Close Port");
        }

        public void LoadConfigFile(ILightControllerConfigParameters parameters)
        {
            PortName = parameters.PortName;
            BaudRate = parameters.BaudRate;
            DataBits = parameters.DataBits;
            Parity = parameters.Parity;
            StopBits = parameters.StopBits;
        }

        public void Open()
        {
            SerialPortOpened?.Invoke(this, _port);
            MessageBox.Show("Open Port");
        }

        public void SetLight(int ch, int value)
        {
            MessageBox.Show("Set Light");
        }
    }
}
