using System.IO.Ports;

namespace InspectionNet.Core.Models
{
    public interface ILightControllerConfigParameters
    {
        string PortName { get; set; }
        int BaudRate { get; set; }
        int DataBits { get; set; }
        Parity Parity { get; set; }
        StopBits StopBits { get; set; }
    }
}
