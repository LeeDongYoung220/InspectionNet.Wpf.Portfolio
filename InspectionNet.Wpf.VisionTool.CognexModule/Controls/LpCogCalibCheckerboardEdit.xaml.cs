using System;
using System.Windows.Controls;

using Cognex.VisionPro.CalibFix;

using InspectionNet.Wpf.Common.StaticClasses;

namespace InspectionNet.Wpf.VisionTool.CognexModule.Controls
{
    /// <summary>
    /// LpCogCalibCheckerboardEdit.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LpCogCalibCheckerboardEdit : UserControl
    {
        private readonly CogCalibCheckerboardEditV2 _cogCalibCheckerboardEditV2 = new();

        public CogCalibCheckerboardTool Subject
        {
            get => _cogCalibCheckerboardEditV2.Subject;
            set
            {
                ThreadInvoker.DispatcherInvoke(() => { _cogCalibCheckerboardEditV2.Subject = value; });
            }
        }

        public event EventHandler? SubjectChanged;

        public LpCogCalibCheckerboardEdit()
        {
            InitializeComponent();
            wfh.Child = _cogCalibCheckerboardEditV2;
            _cogCalibCheckerboardEditV2.SubjectChanged += CogCalibCheckerboardEditV2_SubjectChanged;
        }

        private void CogCalibCheckerboardEditV2_SubjectChanged(object? sender, EventArgs e) => SubjectChanged?.Invoke(this, e);
    }
}
