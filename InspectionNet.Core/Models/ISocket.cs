using System;

namespace InspectionNet.Core.Models
{
    public interface ISocket
    {
        bool IsOpen { get; }

        event EventHandler Connected;
        event EventHandler Disconnected;
        event EventHandler<string> DataReceived;

        void Init(string ip, int port);
        void Start();
        void Stop();
        void Send(IClientHandler clientHandler, string msg);
    }
}
