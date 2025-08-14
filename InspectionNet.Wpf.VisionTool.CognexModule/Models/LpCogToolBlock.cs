using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

using Cognex.VisionPro;
using Cognex.VisionPro.ToolBlock;

using InspectionNet.Core.Enums;
using InspectionNet.Core.Managers;
using InspectionNet.Core.Models;
using InspectionNet.Core.VisionTools;
using InspectionNet.VisionTool.CognexModule.Common.Implementations;

namespace InspectionNet.Wpf.VisionTool.CognexModule.Models
{
    public class LpCogToolBlock : ILpToolBlock
    {
        #region Variables
        private readonly ILogManager _logManager;
        private readonly object _lockObj = new();
        private CogToolBlock? _toolBlock;
        private bool disposedValue;
        private ICogImage _inputImage;
        #endregion

        #region Properties
        public CogToolBlock? ToolBlock
        {
            get => _toolBlock;
            set
            {
                if (_toolBlock != null) _toolBlock.Ran -= ToolBlock_Ran;
                _toolBlock = value;
                if (_toolBlock != null) _toolBlock.Ran += ToolBlock_Ran;
            }
        }

        public ICogImage InputImage
        {
            get => _inputImage;
            internal set
            {
                _inputImage = value;
                if (_toolBlock?.Inputs.FirstOrDefault(x=> x.Name == "InputImage") is CogToolBlockTerminal inputTerminal)
                {
                    inputTerminal.Value = _inputImage;
                }
            }
        }
        #endregion

        #region Events
        public event EventHandler<ILpToolResult>? ToolBlockCompleted;
        public event EventHandler? DataFileLoaded;
        #endregion

        #region Constructor
        public LpCogToolBlock(ILogManager logManager)
        {
            _logManager = logManager;

            ToolBlock = InitToolBlock();
        }

        #endregion

        #region Finalizer
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    DisposeToolBlock();
                }

                // 비관리형 리소스(비관리형 개체)를 해제하고 종료자를 재정의합니다.
                // 큰 필드를 null로 설정합니다.
                disposedValue = true;
            }
        }

        private void DisposeToolBlock()
        {
            if (_toolBlock != null)
            {
                _toolBlock.Ran -= ToolBlock_Ran;
                _toolBlock.Dispose();
                _toolBlock = null;
            }
        }

        // // 비관리형 리소스를 해제하는 코드가 'Dispose(bool disposing)'에 포함된 경우에만 종료자를 재정의합니다.
        // ~LpCogToolBlock()
        // {
        //     // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Methods
        private static CogToolBlock InitToolBlock()
        {
            var toolBlock = new CogToolBlock();
            toolBlock.Inputs.Add(new CogToolBlockTerminal("InputImage", typeof(ICogImage)));
            toolBlock.Outputs.Add(new CogToolBlockTerminal("OutputImage", typeof(ICogImage)));
            return toolBlock;
        }
        private void ToolBlock_Ran(object? sender, EventArgs e)
        {
            if (_toolBlock == null)
            {
                _logManager.LogError(this, ViErrorCode.ToolBlockNullError, "ToolBlock이 초기화되지 않았습니다.", null);
                return;
            }
            if (_toolBlock.CreateLastRunRecord().SubRecords.Count > 0)
            {
                ICogRecord cogRecord = _toolBlock.CreateLastRunRecord().SubRecords[0];
                ILpToolResult toolResult = new LpCogToolResult(cogRecord, _toolBlock.Outputs);
                ToolBlockCompleted?.Invoke(this, toolResult);
            }
            else
            {
                _logManager.LogError(this, ViErrorCode.RecordCreationError, "ToolBlock 실행에서 레코드가 생성되지 않았습니다.", null);
            }
        }

        public void Run(ILpImage inputImage)
        {
            lock (_lockObj)
            {
                if (_toolBlock == null) return;
                if (inputImage is LpCogImage laonCogImage)
                {
                    _toolBlock.Inputs["InputImage"].Value = laonCogImage.CogImage;
                    _toolBlock.Run();
                }
                else
                {
                    _logManager.LogError(this, ViErrorCode.InputImageTypeError, "Run 호출 시 inputImage가 LaonCogImage 타입이 아닙니다.", null);
                }
            }
        }

        public void LoadDataFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                ToolBlock = null;
                _logManager.LogError(this, ViErrorCode.RecipeFilePathError, "SetRecipeFile 호출 시 레시피 파일 경로가 유효하지 않습니다.", null);
                return;
            }
            var cogRecipeFile = CogSerializer.LoadObjectFromFile(filePath);
            if (cogRecipeFile is CogToolBlock cogToolBlock)
            {
                ToolBlock = cogToolBlock;
                DataFileLoaded?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                _logManager.LogError(this, ViErrorCode.LoadToolBlockTypeError, "SetRecipeFile 호출 시 레시피 파일이 CogToolBlock 타입이 아닙니다.", null);
                ToolBlock = null;
            }
        }
        #endregion
    }
}