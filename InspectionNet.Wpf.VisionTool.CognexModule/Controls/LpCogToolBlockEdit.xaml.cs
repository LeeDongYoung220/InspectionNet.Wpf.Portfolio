using System;
using System.Windows.Controls;

using Cognex.VisionPro.ToolBlock;

using InspectionNet.Wpf.Common.StaticClasses;

namespace InspectionNet.Wpf.VisionTool.CognexModule.Controls
{
    /// <summary>
    /// LpCogToolBlockEdit.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LpCogToolBlockEdit : UserControl
    {
        private readonly CogToolBlockEditV2 _cogToolBlockEdit = new() { LocalDisplayVisible = false };

        public CogToolBlock Subject
        {
            get => _cogToolBlockEdit.Subject;
            set
            {
                ThreadInvoker.DispatcherInvoke(() => _cogToolBlockEdit.Subject = value);
            }
        }

        public event EventHandler? SubjectChanged;

        public LpCogToolBlockEdit()
        {
            InitializeComponent();

            wfh.Child = _cogToolBlockEdit;
            _cogToolBlockEdit.SubjectChanged += CogToolBlockEdit_SubjectChanged;
        }

        private void CogToolBlockEdit_SubjectChanged(object? sender, EventArgs e) => SubjectChanged?.Invoke(this, e);
    }
}
