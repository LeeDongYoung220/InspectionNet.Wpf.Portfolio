using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;

using InspectionNet.Core.VisionTools;
using InspectionNet.Wpf.Common.Models;

namespace InspectionNet.Wpf.PocProject.Models
{
    public class ToolGroupParameter : ObservableObject, IToolGroupParameter
    {
        private readonly ILpRuleToolGroup _currentToolGroup;

        public bool IsSaveCsv
        {
            get => _currentToolGroup.IsSaveCsv;
            set => _currentToolGroup.IsSaveCsv = value;
        }
        public bool IsSaveOriginImage
        {
            get => _currentToolGroup.IsSaveOriginImage;
            set => _currentToolGroup.IsSaveOriginImage = value;
        }
        public bool IsCalibrationMode
        {
            get => _currentToolGroup.IsCalibrationMode;
            set => _currentToolGroup.IsCalibrationMode = value;
        }

        public ToolGroupParameter(ILpRuleToolGroup currentToolGroup)
        {
            _currentToolGroup = currentToolGroup;
            IsSaveCsv = _currentToolGroup.IsSaveCsv;
            IsSaveOriginImage = _currentToolGroup.IsSaveOriginImage;
            IsCalibrationMode = _currentToolGroup.IsCalibrationMode;
        }
    }
}
