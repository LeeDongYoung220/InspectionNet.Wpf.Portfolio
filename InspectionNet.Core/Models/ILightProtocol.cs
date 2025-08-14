using System;

namespace InspectionNet.Core.Models
{
    public interface ILightProtocol
    {
        event EventHandler<string> SerialPortDataSent;

        void SetLight(int ch, int value);
    }
}
