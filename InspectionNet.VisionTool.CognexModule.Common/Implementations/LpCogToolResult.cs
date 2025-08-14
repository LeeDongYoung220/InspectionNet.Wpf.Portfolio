using System.Collections.Generic;

using Cognex.VisionPro;
using Cognex.VisionPro.ToolBlock;

using InspectionNet.Core.Models;
using InspectionNet.Core.VisionTools;
using InspectionNet.VisionTool.CognexModule.Common.Models;
using System;

namespace InspectionNet.VisionTool.CognexModule.Common.Implementations
{
    public class LpCogToolResult : ILpCogToolResult, IDisposable
    {
        #region Variables

        private bool _disposedValue;

        #endregion

        #region Properties

        public ICogRecord CogRecord { get; }
        public ILpImage ResultImage { get; }
        public CogToolBlockTerminalCollection OutputTerminals { get; }
        public Dictionary<string, object> Outputs { get; } = [];

        #endregion

        #region Events

        #endregion

        #region Constructor

        public LpCogToolResult()
        {
            CogRecord = null;
            ResultImage = null;
            OutputTerminals = null;
        }

        public LpCogToolResult(ILpImage resultImage)
        {
            ResultImage = resultImage;
            CogRecord = null;
            OutputTerminals = null;
        }

        public LpCogToolResult(ICogRecord cogRecord, CogToolBlockTerminalCollection outputTerminals = null)
        {
            CogRecord = cogRecord;
            OutputTerminals = outputTerminals;
            if (OutputTerminals != null)
            {
                foreach (CogToolBlockTerminal outputTerminal in OutputTerminals)
                {
                    Outputs.Add(outputTerminal.Name, outputTerminal.Value);
                }
                bool IsExistOutputImage = Outputs.TryGetValue("OutputImage", out object value);
                if (IsExistOutputImage && value is ICogImage outputImage)
                {
                    ResultImage = new LpCogImage(outputImage);
                }
                else
                {
                    ResultImage = null;
                }
            }
            else
            {
                ResultImage = null;
            }
        }

        #endregion

        #region Finalizer

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // Dispose managed state (managed objects)
                    ResultImage?.Dispose();
                }

                // Free unmanaged resources (unmanaged objects) and override finalizer
                // Set large fields to null
                _disposedValue = true;
            }
        }

        ~LpCogToolResult()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Methods

        #endregion
    }
}