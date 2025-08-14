using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

using Cognex.VisionPro;
using Cognex.VisionPro.CalibFix;
using Cognex.VisionPro.ToolBlock;

using InspectionNet.Core.Enums;
using InspectionNet.Core.Implementations;
using InspectionNet.Core.Managers;
using InspectionNet.Core.Models;
using InspectionNet.Core.StaticClasses;
using InspectionNet.Core.VisionTools;
using InspectionNet.VisionTool.CognexModule.Common.Implementations;
using InspectionNet.VisionTool.CognexModule.Common.Models;
using InspectionNet.Wpf.Common.Models;
using InspectionNet.Wpf.VisionTool.CognexModule.Controls;

namespace InspectionNet.Wpf.VisionTool.CognexModule.Models
{
    public class LpCogToolGroup : IWpfToolGroup
    {
        #region Variables
        private const string DEFAULT_SPACENAME = "DefaultFixture";
        private readonly ILogManager _logManager;
        private readonly LpCogSaveOutputImage _saveImageTool = new();
        private readonly LpCogToolBlockEdit _toolBlockEdit = new();
        private readonly LpCogCalibCheckerboardEdit _calibCheckerboardEdit = new();
        private readonly Dictionary<string, ILpFixtureTool> _fixtureTools = [];
        private readonly object _lockObj = new();
        private readonly LpCogToolBlock _toolBlock;
        private CogCalibCheckerboardTool _calibCheckerboardTool;
        #endregion

        #region Properties
        public bool IsCalibrationMode { get; set; }

        public ILpImage CalibrationInputImage
        {
            get => new LpCogImage(_calibCheckerboardTool.InputImage);
            set
            {
                if (value is LpCogImage laonCogImage)
                {
                    _calibCheckerboardTool.InputImage = laonCogImage.CogImage;
                }
            }
        }
        public bool IsSaveOriginImage { get; set; }
        public bool IsSaveCsv { get; set; }
        public Control ToolBlockEditControl
        {
            get
            {
                if (_toolBlock?.ToolBlock != null && !_toolBlock.ToolBlock.Equals(_toolBlockEdit.Subject))
                {
                    _toolBlockEdit.Subject = _toolBlock.ToolBlock;
                }
                return _toolBlockEdit;
                return null;
            }
        }
        public Control CalibCheckerboardEditControl => _calibCheckerboardEdit;

        public ILpImage InputImage
        {
            get => _inputImage;
            set
            {
                _inputImage = value;
                if (value is LpCogImage laonCogImage)
                {
                    _toolBlock.InputImage = laonCogImage.CogImage;
                }
                else
                {
                    _logManager.LogError(this, ViErrorCode.InputImageTypeError, "InputImage가 LaonCogImage 타입이 아닙니다.", null);
                }
            }
        }
        #endregion

        #region Events
        public event EventHandler? InitializeCompleted;
        public event EventHandler<ILpToolResult>? ToolBlockCompleted;
        public event EventHandler<ILpToolResult>? Calibrated;
        #endregion

        #region Constructor
        public LpCogToolGroup(ILogManager logManager, string calibrationToolFilePath = "")
        {
            _logManager = logManager;
            _calibCheckerboardTool = new CogCalibCheckerboardTool(); // Initialize here to ensure it's never null
            InitCalibrationTool(calibrationToolFilePath);
            CreateDefaultFixture();

            _toolBlock = new LpCogToolBlock(_logManager);
            _toolBlock.ToolBlockCompleted += ToolBlock_ToolBlockCompleted;
            _toolBlock.DataFileLoaded += ToolBlock_DataFileLoaded;
            _toolBlockEdit.SubjectChanged += ToolBlockEdit_SubjectChanged;
        }
        #endregion

        #region Finalizer

        private bool disposed;
        private ILpImage _inputImage;

        ~LpCogToolGroup()
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
                    // dispose managed resources
                    DisposeToolGroup();
                }

                // dispose of unmanaged resoureces

                disposed = true;
            }
        }

        public void DisposeToolGroup()
        {
            _toolBlock.ToolBlockCompleted -= ToolBlock_ToolBlockCompleted;
            _toolBlock?.Dispose();
            _calibCheckerboardTool?.Dispose();
        }
        #endregion

        #region Methods
        private void ToolBlock_DataFileLoaded(object? sender, EventArgs e) => InitializeCompleted?.Invoke(this, EventArgs.Empty);
        private void InitCalibrationTool(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                var cogObject = CogSerializer.LoadObjectFromFile(filePath);
                if (cogObject is CogCalibCheckerboardTool calibCheckerboardTool)
                {
                    _calibCheckerboardTool = calibCheckerboardTool;
                }
            }
            _calibCheckerboardEdit.Subject = _calibCheckerboardTool;
        }
        private void ToolBlock_ToolBlockCompleted(object? sender, ILpToolResult e)
        {
            if (IsSaveOriginImage) _saveImageTool.Run(e);
            if (IsSaveCsv) CsvHelper.AppendToCsv(e.Outputs, "Result.csv");
            ToolBlockCompleted?.Invoke(this, e);
        }

        private void ToolBlockEdit_SubjectChanged(object? sender, EventArgs e)
        {
            if (sender is LpCogToolBlockEdit toolBlockEdit)
            {
                _toolBlock.ToolBlock = toolBlockEdit.Subject;
            }
        }

        private void CreateDefaultFixture() => AddFixture(DEFAULT_SPACENAME, 0, 0, 0, 0);

        private void AddFixture(string spaceName, double p1X, double p1Y, double p2X, double p2Y)
        {
            var fixtureTool = new LpCogFixtureTool(spaceName, p1X, p1Y, p2X, p2Y);
            _fixtureTools.Add(spaceName, fixtureTool);
        }

        public void Initialize(string dataFilePath = "")
        {
            if (!string.IsNullOrEmpty(dataFilePath))
            {
                _toolBlock?.LoadDataFile(dataFilePath);
            }
            else
            {
                if (_toolBlock != null)
                {
                    InitializeCompleted?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    _logManager.LogError(this, ViErrorCode.ToolBlockNullError, "_toolBlock 이 null입니다.", null);
                }
            }
        }

        public ILpToolResult? Calibrate(ILpImage inputImage)
        {
            if (inputImage == null)
            {
                _logManager.LogError(this, ViErrorCode.InputImageNullError, "Calibrate 호출 시 inputImage가 null입니다.", null);
                return default;
            }
            if (inputImage is LpCogImage laonCogImage)
            {
                _calibCheckerboardTool.Calibration.CalibrationImage = laonCogImage.CogImage;
                _calibCheckerboardTool.Calibration.Calibrate();
                if (_calibCheckerboardTool.RunStatus.Result == CogToolResultConstants.Accept)
                {
                    var cogRecord = _calibCheckerboardTool.CreateCurrentRecord().SubRecords[0]; //Input Image
                    //var cogRecord = _calibCheckerboardTool.CreateCurrentRecord().SubRecords[1]; //Calibration Image
                    //var cogRecord = _calibCheckerboardTool.CreateLastRunRecord().SubRecords[0]; //Output Image
                    var transform = _calibCheckerboardTool.Calibration.GetComputedUncalibratedFromCalibratedTransform();
                    if (transform is CogTransform2DLinear resultData)
                    {
                        var outputTerminals = new CogToolBlockTerminalCollection{
                        new CogToolBlockTerminal("OutputImage", _calibCheckerboardTool.OutputImage, typeof(ICogImage)),
                        new CogToolBlockTerminal("TranslationX", resultData.TranslationX, typeof(double)),
                        new CogToolBlockTerminal("TranslationY", resultData.TranslationX, typeof(double)),
                        new CogToolBlockTerminal("Aspect", resultData.Aspect, typeof(double)),
                        new CogToolBlockTerminal("Skew", resultData.Skew, typeof(double)),
                        new CogToolBlockTerminal("Rotation", resultData.Rotation, typeof(double)),
                        new CogToolBlockTerminal("RotationX", resultData.RotationX, typeof(double)),
                        new CogToolBlockTerminal("RotationY", resultData.RotationY, typeof(double)),
                        new CogToolBlockTerminal("Scaling", resultData.Scaling, typeof(double)), //unit per pixel
                        new CogToolBlockTerminal("ScalingX", resultData.ScalingX, typeof(double)),
                        new CogToolBlockTerminal("ScalingY", resultData.ScalingY, typeof(double))
                        };
                        var toolResult = new LpCogToolResult(cogRecord, outputTerminals);
                        return toolResult;
                    }
                }
            }
            else _logManager.LogError(this, ViErrorCode.InputImageTypeError, "Calibrate 호출 시 inputImage가 LaonCogImage 형식이 아닙니다.", null);
            return default;
        }

        public void Run(ILpImage inputImage)
        {
            lock (_lockObj)
            {
                if (_toolBlock == null) return;
                if (inputImage is LpCogImage laonCogImage)
                {
                    ICogImage toolBlockInputImage = laonCogImage.CogImage;
                    if (IsCalibrationMode)
                    {
                        _calibCheckerboardTool.InputImage = laonCogImage.CogImage;
                        _calibCheckerboardTool.Run();
                        if (_calibCheckerboardTool.RunStatus.Result == CogToolResultConstants.Accept)
                        {
                            toolBlockInputImage = _calibCheckerboardTool.OutputImage;
                        }
                        else
                        {
                            _logManager.LogError(this, ViErrorCode.CalibToolRunError, "CalibrationTool Run 이 완료되지 않았습니다.", null);
                            return;
                        }
                    }
                    _toolBlock.Run(laonCogImage);
                }
                else
                {
                    _logManager.LogError(this, ViErrorCode.InputImageTypeError, "Run 호출 시 inputImage가 LaonCogImage 타입이 아닙니다.", null);
                }
            }
        }

        public void CreateFixture(string spaceName, PointD point, PointD point2)
        {
            CreateFixture(spaceName, point.X, point.Y, point2.X, point2.Y);
        }

        public void CreateFixture(string spaceName, double p1X, double p1Y, double p2X, double p2Y)
        {
            if (_fixtureTools.ContainsKey(spaceName)) return;
            AddFixture(spaceName, p1X, p1Y, p2X, p2Y);
        }

        public ILpToolResult? RunFixture(string spaceName, PointD point)
        {
            return RunFixture(spaceName, point.X, point.Y);
        }

        public ILpToolResult? RunFixture(string spaceName, double p1X, double p1Y)
        {
            /*ILpToolResult result;
            string name = spaceName;
            if (!_fixtureTools.ContainsKey(spaceName)) name = DEFAULT_SPACENAME;
            result = _fixtureTools[name].Run(p1X, p1Y);
            if (result == null)
            {
                _logManager.LogError(this, ViErrorCode.FixtureToolError, "RunFixture 호출 시 FixtureTool이 null이거나 실행에 실패했습니다.", null);
                return null;
            }
            return result;*/
            return null;
        }
        #endregion
    }
}