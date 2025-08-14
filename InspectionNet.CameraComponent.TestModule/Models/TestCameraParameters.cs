using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using InspectionNet.Core.Models;

namespace InspectionNet.CameraComponent.TestModule.Models
{
    internal class TestCameraParameters : ILpCameraParameters
    {
        public IEnumerable<string> PixelFormatAllValues { get; }
        public IEnumerable<string> TriggerModeAllValues { get; }
        public IEnumerable<string> TriggerSourceAllValues { get; }
        public IEnumerable<string> UserSetSelectorAllValues { get; }
        public IEnumerable<string> UserSetDefaultAllValues { get; }
        public string PixelFormat { get; set; }
        public string TriggerMode { get; set; }
        public string TriggerSource { get; set; }
        public double Gain { get; set; }
        public double ExposureTime { get; set; }
        public string UserSetSelector { get; set; }
        public string UserSetDefault { get; set; }
        public string DeviceUserID { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public event EventHandler UserSetLoaded;
        public event EventHandler UserSetSaved;
        public event PropertyChangedEventHandler? PropertyChanged;

        public IList<IGenApiParameter> ToList()
        {
            throw new NotImplementedException();
        }

        public void TriggerSoftware()
        {
            MessageBox.Show("Trigger Software");
        }

        public void UserSetLoad()
        {
            MessageBox.Show("UserSetLoad");
        }

        public void UserSetSave()
        {
            MessageBox.Show("UserSetSave");
        }
    }
}
