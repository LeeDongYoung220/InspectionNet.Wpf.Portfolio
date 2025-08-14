using System;

namespace InspectionNet.Core.Models
{
    public interface IClientHandler
    {
        event EventHandler Connected;
        event EventHandler Disconnected;
        event EventHandler<string> DataReceived;

        void Send(string msg);
    }
}
