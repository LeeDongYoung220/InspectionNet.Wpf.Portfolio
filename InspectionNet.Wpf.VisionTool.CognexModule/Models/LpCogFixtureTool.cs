using System;

using Cognex.VisionPro;
using Cognex.VisionPro.CalibFix;
using Cognex.VisionPro.Dimensioning;
using Cognex.VisionPro.ToolBlock;

using InspectionNet.Core.Implementations;
using InspectionNet.Core.StaticClasses;
using InspectionNet.Core.VisionTools;
using InspectionNet.VisionTool.CognexModule.Common.Implementations;
using InspectionNet.Wpf.Common.Models;

namespace InspectionNet.Wpf.VisionTool.CognexModule.Models
{
    internal class LpCogFixtureTool : ILpFixtureTool
    {
        #region Variables
        private readonly CogFixtureTool _fixtureTool = new();
        private readonly CogCreateCircleTool _createCircleTool = new();
        private readonly string _spaceName;
        #endregion

        #region Properties

        #endregion

        #region Events

        #endregion

        #region Constructor
        public LpCogFixtureTool(string spaceName, double p1X, double p1Y, double p2X, double p2Y)
        {
            _spaceName = spaceName;
            CreateCoordinateSystem(spaceName, p1X, p1Y, p2X, p2Y);
            InitCreateCircleTool();
        }

        public LpCogFixtureTool(string spaceName, PointD p1, PointD p2) : this(spaceName, p1.X, p1.Y, p2.X, p2.Y) { }
        #endregion

        #region Finalizer

        private bool disposed;
        ~LpCogFixtureTool()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //dispose managed resources
                    DisposeTool();
                }

                //dispose of unmanaged resoureces

                disposed = true;
            }
        }

        public void DisposeTool()
        {
            _createCircleTool.Dispose();
            _fixtureTool.Dispose();
        }
        #endregion

        #region Methods
        private void InitCreateCircleTool()
        {
            _createCircleTool.InputImage = _fixtureTool.InputImage;
        }

        private void CreateCoordinateSystem(string spaceName, double p1X, double p1Y, double p2X, double p2Y)
        {
            InitFixtureTool(spaceName, p1X, p1Y, p2X, p2Y);
            _fixtureTool.Run();
        }

        private void InitFixtureTool(string spaceName, double p1X, double p1Y, double p2X, double p2Y)
        {
            var inputImage = new CogImage8Grey(1, 1);
            _fixtureTool.InputImage = inputImage;
            _fixtureTool.RunParams.Action = CogFixtureActionConstants.EstablishNewFixture;
            _fixtureTool.RunParams.SpaceToOutput = CogFixtureSpaceToOutputConstants.UnfixturedSpace;
            _fixtureTool.RunParams.FixturedSpaceNameDuplicateHandling = CogFixturedSpaceNameDuplicateHandlingConstants.Enhanced;
            _fixtureTool.RunParams.FixturedSpaceName = spaceName;
            CogTransform2DLinear cogTransform2DLinear = CreateCogTransform2DLinear(p1X, p1Y, p2X, p2Y);
            _fixtureTool.RunParams.UnfixturedFromFixturedTransform = cogTransform2DLinear;
        }

        private static CogTransform2DLinear CreateCogTransform2DLinear(double p1X, double p1Y, double p2X, double p2Y)
        {
            double deg = MathHelper.GetAngle(p1X, p1Y, p2X, p2Y);
            var cogTransform2DLinear = new CogTransform2DLinear
            {
                TranslationX = p1X,
                TranslationY = p1Y,
                Aspect = 1,
                Scaling = 1,
                Rotation = CogMisc.DegToRad(deg)
            };
            return cogTransform2DLinear;
        }

        private ILpToolResult CreateCogToolResult()
        {
            var cogRecord = _createCircleTool.CreateLastRunRecord();
            var resultX = _createCircleTool.GetOutputCircle().CenterX;
            var resultY = _createCircleTool.GetOutputCircle().CenterY;
            CogToolBlockTerminalCollection outputs =
            [
                new CogToolBlockTerminal("OutputX", resultX, typeof(double)),
                new CogToolBlockTerminal("OutputY", resultY, typeof(double))
            ];
            ILpToolResult toolResult = new LpCogToolResult(cogRecord, outputs);
            return toolResult;
        }

        private CogCircle CreateCogCircle(double px, double py)
        {
            return new CogCircle
            {
                SelectedSpaceName = $@"@\{_spaceName}",
                CenterX = px,
                CenterY = py,
                Radius = 1
            };
        }

        public ILpToolResult? Run(double px, double py)
        {
            _createCircleTool.InputCircle = CreateCogCircle(px, py);
            _createCircleTool.Run();
            ILpToolResult? toolResult = default;
            if (_createCircleTool.RunStatus.Result == CogToolResultConstants.Accept)
            {
                toolResult = CreateCogToolResult();
            }
            return toolResult;
        }

        public ILpToolResult? Run(PointD p)
        {
            return Run(p.X, p.Y);
        }
        #endregion
    }
}