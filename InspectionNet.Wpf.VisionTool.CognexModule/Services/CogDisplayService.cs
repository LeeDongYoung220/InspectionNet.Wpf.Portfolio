using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using InspectionNet.Core.Implementations;
using InspectionNet.Core.Models;
using InspectionNet.Core.Services;
using InspectionNet.Core.Views;
using InspectionNet.VisionTool.CognexModule.Common.Implementations;
using InspectionNet.Wpf.VisionTool.CognexModule.Controls;

namespace InspectionNet.Wpf.VisionTool.CognexModule.Services
{
    public class CogDisplayService : IDisplayService
    {
        #region Variables

        #endregion

        #region Properties
        public ILpDisplay ToolViewDisplay { get; }

        #endregion

        #region Events

        #endregion

        #region Constructor
        public CogDisplayService()
        {
            ToolViewDisplay = new LpCogDisplay();
        }

        #endregion

        #region Finalizer

        #endregion

        #region Methods
        public ILpImage CreateImage(ILpGrabResult grabResult) => new LpCogImage(grabResult, true);

        public ILpImage CreateImage(string filePath) => new LpCogImage(filePath);

        public ILpImage CreateImage(Image image) => new LpCogImage(image);

        public ILpImage CreateImage(IList<IGenApiParameter> e)
        {
            var width = e.FirstOrDefault(x => x.Name == "Width");
            var height = e.FirstOrDefault(x => x.Name == "Height");
            if (width == null || height == null) return null;
            if (width is IntegerParameter intWidth && height is IntegerParameter intHeight)
            {
                return new LpCogImage(intWidth.Value, intHeight.Value);
            }
            else
                return null;
        }

        public ILpDisplay GetDisplay() => new LpCogDisplay();

        #endregion
    }
}