using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

using Cognex.VisionPro;

using InspectionNet.Core.Implementations;
using InspectionNet.Core.Models;
using InspectionNet.Core.StaticClasses;

namespace InspectionNet.VisionTool.CognexModule.Common.Implementations
{
    public class LpCogImage : ILpImage
    {
        #region Variables

        private bool disposedValue;

        #endregion

        #region Properties

        public Image OriginalImage { get; }
        public ICogImage CogImage { get; }
        public IntPtr PixelDataPointer
        {
            get
            {
                if (CogImage is CogImage8Grey cogImage8Grey)
                {
                    return cogImage8Grey.Get8GreyPixelMemory(CogImageDataModeConstants.ReadWrite, 0, 0, cogImage8Grey.Width, cogImage8Grey.Height).Scan0;
                }
                return IntPtr.Zero;
            }
        }

        #endregion

        #region Events

        #endregion

        #region Constructor

        public LpCogImage()
        {
            OriginalImage = null;
            CogImage = new CogImage8Grey(); // Initialize with a default empty CogImage
        }

        public LpCogImage(string filePath)
        {
            string[] validExtensions = BitmapHelper.AvailableFormat;
            if (!validExtensions.Contains(Path.GetExtension(filePath).ToLower())) throw new ArgumentException("Invalid file extension");
            OriginalImage = new Bitmap(filePath);
            CogImage = ConvertCogImage(OriginalImage as Bitmap);
        }

        public LpCogImage(Image image)
        {
            OriginalImage = new Bitmap(image);
            CogImage = ConvertCogImage(OriginalImage as Bitmap);
        }

        public LpCogImage(ICogImage cogImage)
        {
            OriginalImage = null; // OriginalImage is not available when constructing from ICogImage
            CogImage = cogImage;
        }

        public LpCogImage(ILpGrabResult grabResult, bool createOriginalImage = false)
        {
            if (createOriginalImage)
            {
                OriginalImage = grabResult.ToBitmap();
            }
            CogImage = ConvertCogImage(OriginalImage as Bitmap);
        }

        public LpCogImage(int width, int height)
        {
            CogImage = new CogImage8Grey(width, height);
        }

        #endregion

        #region Finalizer

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Dispose managed state (managed objects).
                }
                OriginalImage?.Dispose();
                if (CogImage is CogImage8Grey monoImage) monoImage.Dispose();
                if (CogImage is CogImage24PlanarColor colorImage) colorImage.Dispose();
                disposedValue = true;
            }
        }

        ~LpCogImage()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Methods

        public static ICogImage ConvertCogImage(Bitmap bmp)
        {
            ICogImage cogImage = bmp.PixelFormat switch
            {
                PixelFormat.Format8bppIndexed => new CogImage8Grey(bmp),
                PixelFormat.Format24bppRgb or PixelFormat.Format32bppArgb => new CogImage24PlanarColor(bmp),
                _ => throw new ArgumentException($"Unsupported pixel format: {bmp.PixelFormat}"),
            };
            return cogImage;
        }

        private static ICogImage ConvertCogImage(ILpGrabResult grabResult)
        {
            return grabResult.PixelFormat switch
            {
                PixelFormat.Format8bppIndexed => AllocateAndCopyCogImage8PixelMemory(grabResult),
                PixelFormat.Format24bppRgb => AllocateAndCopyCogImage24PlanarColorPixelMemory(grabResult),
                _ => throw new ArgumentException($"Unsupported pixel format for grab result: {grabResult.PixelFormat}"),
            };
        }

        private static ICogImage AllocateAndCopyCogImage8PixelMemory(ILpGrabResult grabResult)
        {
            CogImage8Grey cogImage8Grey = new(grabResult.Width, grabResult.Height);
            ICogImage8PixelMemory cogImage8PixelMemory = cogImage8Grey.Get8GreyPixelMemory(CogImageDataModeConstants.ReadWrite, 0, 0, cogImage8Grey.Width, cogImage8Grey.Height);
            Marshal.Copy(grabResult.PixelData, 0, cogImage8PixelMemory.Scan0, grabResult.Stride * grabResult.Height);
            cogImage8PixelMemory.Dispose();
            return cogImage8Grey;
        }

        private static ICogImage AllocateAndCopyCogImage24PlanarColorPixelMemory(ILpGrabResult grabResult)
        {
            CogImage24PlanarColor cogImage24PlanarColor = new(grabResult.Width, grabResult.Height);
            cogImage24PlanarColor.Get24PlanarColorPixelMemory(CogImageDataModeConstants.ReadWrite,
                                                              0,
                                                              0,
                                                              cogImage24PlanarColor.Width,
                                                              cogImage24PlanarColor.Height,
                                                              out ICogImage8PixelMemory cogImage8PixelMemory0,
                                                              out ICogImage8PixelMemory cogImage8PixelMemory1,
                                                              out ICogImage8PixelMemory cogImage8PixelMemory2);
            Marshal.Copy(grabResult.PixelData, grabResult.Width * grabResult.Height * 0, cogImage8PixelMemory0.Scan0, grabResult.Width * grabResult.Height);
            Marshal.Copy(grabResult.PixelData, grabResult.Width * grabResult.Height * 1, cogImage8PixelMemory1.Scan0, grabResult.Width * grabResult.Height);
            Marshal.Copy(grabResult.PixelData, grabResult.Width * grabResult.Height * 2, cogImage8PixelMemory2.Scan0, grabResult.Width * grabResult.Height);
            cogImage8PixelMemory0.Dispose();
            cogImage8PixelMemory1.Dispose();
            cogImage8PixelMemory2.Dispose();
            return cogImage24PlanarColor;
        }

        public void SetPixelData(byte[] pixelData)
        {
            if (pixelData.Length != CogImage.Width * CogImage.Height) return;
            Marshal.Copy(pixelData, 0, PixelDataPointer, pixelData.Length);
        }

        #endregion
    }
}