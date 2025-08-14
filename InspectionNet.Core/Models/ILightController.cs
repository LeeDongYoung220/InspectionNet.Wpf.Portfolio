using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;

namespace InspectionNet.Core.Models
{
    public interface ILightController : ILightControllerConfigParameters, INotifyPropertyChanged
    {
        List<ILightProtocol>? LightProtocols { get; }
        ILightProtocol? SelectedLightProtocol { get; set; }
        bool IsOpen { get; }

        event EventHandler<SerialPort>? SerialPortOpened;
        event EventHandler? SerialPortClosed;
        event EventHandler<string>? SerialPortDataSent;
        event EventHandler<string>? SerialPortDataReceived;

        void Open();
        void Close();
        void SetLight(int ch, int value);
        void LoadConfigFile(ILightControllerConfigParameters parameters);
    }
}
