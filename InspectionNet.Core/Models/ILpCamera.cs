using System;

namespace InspectionNet.Core.Models
{
    public interface ILpCamera : IDisposable
    {
        //Properties
        string Id { get; }
        bool IsCreated { get; }
        bool IsOpen { get; }
        bool IsGrabStarted { get; }
        bool IsLive { get; }
        ILpCameraParameters? Parameters { get; }
        bool IsLiveReady { get; }

        //Event
        event EventHandler<ILpCameraParameters>? Opened;
        event EventHandler? Closed;
        event EventHandler? GrabStarted;
        event EventHandler? GrabStopped;
        event EventHandler? ConnectionLost;
        event EventHandler<ILpGrabResult>? ImageGrabbed;
        event EventHandler<ILpCameraParameters>? ParameterChanged;

        //Method
        void Open();
        void Close();
        void GrabStart();
        void GrabStop();
        void LoadConfigFile(ILpCameraConfigParameters config);
    }
}
