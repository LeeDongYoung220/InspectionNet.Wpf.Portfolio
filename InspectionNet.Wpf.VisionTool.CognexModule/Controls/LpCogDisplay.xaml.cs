using System;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Media;

using InspectionNet.Core.Models;
using InspectionNet.Core.Views;
using InspectionNet.Core.VisionTools;
using InspectionNet.VisionTool.CognexModule.Common.Controls;

namespace InspectionNet.Wpf.VisionTool.CognexModule.Controls
{
    /// <summary>
    /// LpCogDisplay.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LpCogDisplay : UserControl, ILpDisplay
    {
        private readonly LaonCogDisplay _cogDisplay;

        public ILpImage LaonImage { get => _cogDisplay.LaonImage; set => _cogDisplay.LaonImage = value; }
        public ILpToolResult LaonResult { get => _cogDisplay.LaonResult; set => _cogDisplay.LaonResult = value; }
        public LpCogDisplay()
        {
            InitializeComponent();
            _cogDisplay = new LaonCogDisplay(SystemColors.ControlDarkDark);
            wfh.Child = _cogDisplay;
        }
        public Bitmap CaptureDisplay() => _cogDisplay.CaptureDisplay();

        public void DisplayFit(bool status) => _cogDisplay.DisplayFit(status);

        public System.Drawing.Image GetOverlayImage() => _cogDisplay.GetOverlayImage();

        public void SaveImage() => _cogDisplay.SaveImage();

        public void OverlayClear()
        {
            _cogDisplay.OverlayClear();
        }
    }
}
