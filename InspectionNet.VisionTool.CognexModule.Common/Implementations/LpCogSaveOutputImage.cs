using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Cognex.VisionPro;
using Cognex.VisionPro.ImageFile;

using InspectionNet.Core.VisionTools;
using InspectionNet.VisionTool.CognexModule.Common.Implementations;

namespace InspectionNet.VisionTool.CognexModule.Common.Models
{
    public class LpCogSaveOutputImage
    {
        #region Variables

        //private readonly CogImageFile _imageFile = new CogImageFile();
        //private readonly object _lockObject = new object();
        private int _imageCount;
        private readonly string _folderPath;
        private bool _disposedValue;

        #endregion

        #region Properties

        #endregion

        #region Events

        #endregion

        #region Constructor

        public LpCogSaveOutputImage()
        {
            //_folderPath = Path.Combine(Application.StartupPath, "ResultImage");
        }

        /*public LpCogSaveOutputImage(string savePath = "")
        {
            if (string.IsNullOrEmpty(savePath)) _folderPath = Path.Combine(Application.StartupPath, "ResultImage");
            else _folderPath = savePath;
        }*/

        #endregion

        #region Finalizer

        /*protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // Dispose managed state (managed objects)
                    //_imageFile.Dispose();
                }

                // Free unmanaged resources (unmanaged objects) and override finalizer
                // Set large fields to null
                _disposedValue = true;
            }
        }*/

        //~LpCogSaveOutputImage()
        //{
        //    // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //    Dispose(disposing: false);
        //}

        /*public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            //Dispose(disposing: true);
            //GC.SuppressFinalize(this);
        }*/

        #endregion

        #region Methods

        public void Run(ILpToolResult e)
        {
            /*if (e is LpCogToolResult result)
            {
                var outputImage = result.OutputTerminals.FirstOrDefault(x => x.Name == "OutputImage");
                if (outputImage?.Value is ICogImage img)
                {
                    Task.Run(() =>
                    {
                        lock (_lockObject)
                        {
                            var tmpImage = img.CopyBase(CogImageCopyModeConstants.CopyPixels);
                            if (!Directory.Exists(_folderPath)) Directory.CreateDirectory(_folderPath);
                            var filePath = Path.Combine(_folderPath, $"{DateTime.Now:yyyyMMdd HHmmss.fff}.bmp");
                            _imageFile.Open(filePath, CogImageFileModeConstants.Write);
                            _imageFile.Append(tmpImage);
                            _imageFile.Close();
                        }
                    });
                }
                _imageCount++;
            }*/
        }

        #endregion
    }
}