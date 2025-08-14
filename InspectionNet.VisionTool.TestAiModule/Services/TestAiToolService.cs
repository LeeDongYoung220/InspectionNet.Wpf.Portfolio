using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using InspectionNet.Core.Services;
using InspectionNet.Core.VisionTools;

namespace InspectionNet.VisionTool.TestAiModule.Services
{
    public class TestAiToolService : IAiToolService
    {
        public ILpToolGroup? CurrentToolGroup { get; }

        public event EventHandler? InitializeCompleted;
        public event EventHandler<ILpToolGroup>? CurrentToolGroupChanged;
        public event EventHandler<ILpToolResult>? CurrentToolRan;

        public ILpToolGroup CreateToolGroup()
        {
            MessageBox.Show("CreateToolGroup");
            return null;
        }

        public void Initialize()
        {
            MessageBox.Show("Initialize");
            InitializeCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}
