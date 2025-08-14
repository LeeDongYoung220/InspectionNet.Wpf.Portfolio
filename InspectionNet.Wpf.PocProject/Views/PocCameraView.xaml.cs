using System.Windows.Controls;

using InspectionNet.Wpf.Common.Views;

namespace InspectionNet.Wpf.PocProject.Views
{
    /// <summary>
    /// PocCameraView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PocCameraView : UserControl, ICameraView
    {
        public PocCameraView()
        {
            InitializeComponent();
        }
    }
}
