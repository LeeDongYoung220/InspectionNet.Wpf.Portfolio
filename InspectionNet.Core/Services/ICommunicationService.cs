using System;

using InspectionNet.Core.Models;

namespace InspectionNet.Core.Services
{
    public interface ICommunicationService
    {
        ISocket? Socket { get; }

        event EventHandler Connected;
        event EventHandler Disconnected;
        event EventHandler<string> DataReceived;

        void Open(IConnectionInfo connectionInfo);
        void Close();
        void Send(IClientHandler clientHandler, string msg);
    }
}
