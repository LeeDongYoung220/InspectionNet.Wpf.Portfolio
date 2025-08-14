using System;
using System.Collections.Generic;

using InspectionNet.Core.Models;

namespace InspectionNet.Core.Services
{
    public interface ICameraService : IDisposable
    {
        IList<ILpCamera> Cameras { get; }
        ILpCamera? SelectedCamera { get; }
        string SelectedCameraId { get; set; }

        event EventHandler<IEnumerable<ILpCamera>?>? CameraListChanged;
        event EventHandler<ILpCamera?>? SelectedCameraChanged;
        event EventHandler InitializeCompleted;
        event EventHandler<ILpCameraParameters>? CameraOpened;
        event EventHandler? CameraClosed;
        event EventHandler? CameraGrabStarted;
        event EventHandler? CameraGrabStopped;
        event EventHandler<ILpGrabResult>? CameraImageGrabbed;
        event EventHandler<ILpCameraParameters>? CameraParameterChanged;

        //event EventHandler<ILpCameraParameters>? SelectedCameraOpened;
        //event EventHandler SelectedCameraClosed;
        //event EventHandler SelectedCameraGrabStarted;
        //event EventHandler SelectedCameraGrabStopped;
        //event EventHandler<ILpGrabResult>? SelectedCameraImageGrabbed;

        void Initialize();
        void Discover();
        ILpCamera? GetCamera(string id);
        void OpenCamera(string id);
        void CloseCamera(string id);
        void GrabStartCamera(string id);
        void GrabStopCamera(string id);
        void StartLive(ILpCamera selectedCamera);
        void StopLive(ILpCamera selectedCamera);
    }
}
