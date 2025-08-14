using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

using InspectionNet.Core.Views;
using InspectionNet.Wpf.Common.MainFrame.ViewModels;

namespace InspectionNet.Wpf.PocProject.ViewModels
{
    public class MockHomeViewModel : IHomeViewModel
    {
        public bool IsInitializedCamera { get; }
        public bool IsInitializedTool { get; }
        public bool IsInitializedMotion { get; }
        public BitmapImage GrabImage { get; set; }
        public bool IsToolRun { get; set; }
        public bool IsInitMotion { get; set; }
        public ILpDisplay LaonDisplay { get; }
        public ICommand InitComponentCommand { get; }
        public ICommand AlignSequenceCommand { get; }
        public ICommand RunSequenceCommand { get; }
        public ICommand SequenceStopCommand { get; }
        public ICommand LiveStartCameraCommand { get; }
        public ICommand LiveStopCameraCommand { get; }
    }
}
