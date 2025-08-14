using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using InspectionNet.Core.Models;

namespace InspectionNet.CameraComponent.TestModule.Models
{
    internal class TestCamera : ILpCamera
    {
        private bool disposedValue;

        public string Id { get; }
        public bool IsCreated { get; }
        public bool IsOpen { get; }
        public bool IsGrabStarted { get; }
        public bool IsLive { get; }
        public ILpCameraParameters? Parameters { get; }
        public bool IsLiveReady { get; }

        public event EventHandler<ILpCameraParameters>? Opened;
        public event EventHandler? Closed;
        public event EventHandler? GrabStarted;
        public event EventHandler? GrabStopped;
        public event EventHandler? ConnectionLost;
        public event EventHandler<ILpGrabResult>? ImageGrabbed;
        public event EventHandler<ILpCameraParameters>? ParameterChanged;

        public TestCamera()
        {
            Parameters = new TestCameraParameters();
        }

        public void Close()
        {
            MessageBox.Show("Close");
            Closed?.Invoke(this, EventArgs.Empty);
        }

        public void GrabStart()
        {
            MessageBox.Show("Grab Start");
            GrabStarted?.Invoke(this, EventArgs.Empty);
        }

        public void GrabStop()
        {
            MessageBox.Show("Grab Stop");
            GrabStopped?.Invoke(this, EventArgs.Empty);
        }

        public void LoadConfigFile(ILpCameraConfigParameters config)
        {
            MessageBox.Show("LoadConfigFile");
        }

        public void Open()
        {
            MessageBox.Show("Open");
            Opened?.Invoke(this, Parameters);
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
        // ~TestCamera()
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
