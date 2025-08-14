using System.IO.Ports;

using InspectionNet.Core.Models;

namespace InspectionNet.Core.Implementations
{
    public class LaonLightControllerConfigParameters : ILightControllerConfigParameters
    {
        public string PortName { get; set; } = string.Empty;
        public int BaudRate { get; set; }
        public int DataBits { get; set; }
        public Parity Parity { get; set; }
        public StopBits StopBits { get; set; }

        public LaonLightControllerConfigParameters()
        {
            
        }

        public LaonLightControllerConfigParameters(ILightController lightController)
        {
            PortName = lightController.PortName;
            BaudRate = lightController.BaudRate;
            DataBits = lightController.DataBits;
            Parity = lightController.Parity;
            StopBits = lightController.StopBits;
        }
    }
}
