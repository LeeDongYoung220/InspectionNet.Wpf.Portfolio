using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using InspectionNet.CameraComponent.TestModule.Models;
using InspectionNet.Core.Models;
using InspectionNet.Core.Services;

namespace InspectionNet.CameraComponent.TestModule.Services
{
    public class TestCameraService : ICameraService
    {
        private bool disposedValue;

        public IList<ILpCamera> Cameras { get; private set; }
        public ILpCamera? SelectedCamera { get; }
        public string SelectedCameraId { get; set; }

        public event EventHandler<IEnumerable<ILpCamera>?>? CameraListChanged;
        public event EventHandler<ILpCamera?>? SelectedCameraChanged;
        public event EventHandler InitializeCompleted;
        public event EventHandler<ILpCameraParameters>? CameraOpened;
        public event EventHandler? CameraClosed;
        public event EventHandler? CameraGrabStarted;
        public event EventHandler? CameraGrabStopped;
        public event EventHandler<ILpGrabResult>? CameraImageGrabbed;
        public event EventHandler<ILpCameraParameters>? CameraParameterChanged;

        public TestCameraService()
        {

        }

        private void ConnectEvents()
        {
            if (Cameras == null) return;
            foreach (var camera in Cameras)
            {
                camera.Opened += Camera_Opened;
                camera.Closed += Camera_Closed;
                camera.GrabStarted += Camera_GrabStarted;
                camera.GrabStopped += Camera_GrabStopped;
                camera.ImageGrabbed += Camera_ImageGrabbed;
                camera.ParameterChanged += Camera_ParameterChanged;
            }
        }
        private void DisconnectEvents()
        {
            if (Cameras == null) return;
            foreach (var camera in Cameras)
            {
                camera.Opened -= Camera_Opened;
                camera.Closed -= Camera_Closed;
                camera.GrabStarted -= Camera_GrabStarted;
                camera.GrabStopped -= Camera_GrabStopped;
                camera.ImageGrabbed -= Camera_ImageGrabbed;
                camera.ParameterChanged -= Camera_ParameterChanged;
            }
        }

        private void Camera_Opened(object? sender, ILpCameraParameters e) => CameraOpened?.Invoke(this, e);

        private void Camera_Closed(object? sender, EventArgs e) => CameraClosed?.Invoke(this, e);

        private void Camera_GrabStarted(object? sender, EventArgs e) => CameraGrabStarted?.Invoke(this, e);

        private void Camera_GrabStopped(object? sender, EventArgs e) => CameraGrabStopped?.Invoke(this, e);

        private void Camera_ImageGrabbed(object? sender, ILpGrabResult e) => CameraImageGrabbed?.Invoke(this, e);

        private void Camera_ParameterChanged(object? sender, ILpCameraParameters e) => CameraParameterChanged?.Invoke(sender, e);

        public void Discover()
        {
            DisconnectEvents();
            var cameras = new List<ILpCamera>();
            var camera = new TestCamera();
            cameras.Add(camera);
            Cameras = cameras;
            ConnectEvents();
            CameraListChanged?.Invoke(this, Cameras);
            MessageBox.Show("Discover Camera");
        }

        public ILpCamera? GetCamera(string id)
        {
            MessageBox.Show("Get Camera");
            return Cameras.FirstOrDefault(c => c.Id == id);
        }

        public void GrabStartCamera(string id)
        {
            MessageBox.Show("Grab Start Camera");
            GetCamera(id)?.GrabStart();
        }

        public void GrabStopCamera(string id)
        {
            MessageBox.Show("Grab Stop Camera");
            GetCamera(id)?.GrabStop();
        }

        public void Initialize()
        {
            MessageBox.Show("Initialize Camera");
        }

        public void OpenCamera(string id)
        {
            MessageBox.Show("Open Camera");
            GetCamera(id)?.Open();
        }

        public void CloseCamera(string id)
        {
            MessageBox.Show("Close Camera");
            GetCamera(id)?.Close();
        }

        public void StartLive(ILpCamera selectedCamera)
        {
            MessageBox.Show("Start Live Camera");
        }

        public void StopLive(ILpCamera selectedCamera)
        {
            MessageBox.Show("Stop Live Camera");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리형 상태(관리형 개체)를 삭제합니다.
                }

                // TODO: 비관리형 리소스(비관리형 개체)를 해제하고 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                disposedValue = true;
            }
        }

        // // TODO: 비관리형 리소스를 해제하는 코드가 'Dispose(bool disposing)'에 포함된 경우에만 종료자를 재정의합니다.
        // ~TestCameraService()
        // {
        //     // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
