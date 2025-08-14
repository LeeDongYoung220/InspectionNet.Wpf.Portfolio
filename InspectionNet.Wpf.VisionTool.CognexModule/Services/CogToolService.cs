using System;
using System.Collections.Generic;

using InspectionNet.Core.Managers;
using InspectionNet.Core.Models;
using InspectionNet.Core.Services;
using InspectionNet.Core.VisionTools;
using InspectionNet.VisionTool.CognexModule.Common.Models;
using InspectionNet.Wpf.Common.Models;
using InspectionNet.Wpf.VisionTool.CognexModule.Models;

namespace InspectionNet.Wpf.VisionTool.CognexModule.Services
{
    public class CogToolService : IRuleToolService
    {
        #region Variables
        private readonly ILogManager _logManager;
        private ILpRuleToolGroup _currentToolGroup = null!;


        #endregion

        #region Properties

        public ILpToolGroup CurrentToolGroup
        {
            get => _currentToolGroup;
            private set
            {
                if (_currentToolGroup != null && _currentToolGroup.Equals(value)) return;
                if (value is ILpRuleToolGroup tg)
                {
                    _currentToolGroup = tg;
                    CurrentToolGroupChanged?.Invoke(this, _currentToolGroup);
                }
            }
        }
        #endregion

        #region Events
        public event EventHandler? InitializeCompleted;
        public event EventHandler<ILpToolResult>? CurrentToolRan;
        public event EventHandler<ILpToolGroup>? CurrentToolGroupChanged;

        #endregion
        #region Constructor

        public CogToolService(ILogManager logManager)
        {
            _logManager = logManager;
            CurrentToolGroup = new LpCogToolGroup(_logManager);
            CurrentToolGroup.ToolBlockCompleted += (sender, e) => CurrentToolRan?.Invoke(sender, e);
        }

        public void Initialize()
        {
            // Initialization logic can be added here if needed
            InitializeCompleted?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Finalizer

        #endregion

        #region Methods
        public ILpRuleToolGroup CreateToolGroup(string calibToolFilePath = "")
        {
            var toolGroup = new LpCogToolGroup(_logManager, calibToolFilePath);
            return toolGroup;
        }

        public void RunSequence(ILpImage e)
        {
            CurrentToolGroup.Run(e);
        }

        public void ConvertPositionInFixture(int v1, int v2, out double outputX, out double outputY)
        {
            throw new NotImplementedException();
        }

        public void CreateFixture(double finalX1, double finalY1, double finalX2, double finalY2)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}