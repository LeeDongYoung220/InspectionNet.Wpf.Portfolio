using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

using Cognex.VisionPro;

using InspectionNet.Core.Implementations;
using InspectionNet.Core.Models;
using InspectionNet.Core.Views;
using InspectionNet.Core.VisionTools;
using InspectionNet.VisionTool.CognexModule.Common.Implementations;
using InspectionNet.VisionTool.CognexModule.Common.Models;

namespace InspectionNet.VisionTool.CognexModule.Common.Controls
{
    public partial class LaonCogDisplay : UserControl, ILpDisplay
    {
        private readonly Color _backColor;

        internal CogRecordDisplay Display => cogRecordDisplay1;

        public ILpImage LaonImage
        {
            get { return new LpCogImage(cogRecordDisplay1.Image); }
            set
            {
                if (value is LpCogImage lpCogImage)
                {
                    if (cogRecordDisplay1.InvokeRequired)
                    {
                        cogRecordDisplay1.Invoke(new Action(() =>
                        {
                            LaonImage = lpCogImage;
                        }));
                    }
                    else
                    {
                        var oldImage = cogRecordDisplay1.Image;
                        cogRecordDisplay1.Image = lpCogImage.CogImage;
                        if (!IsEqualSize(oldImage, lpCogImage.CogImage))
                        {
                            cogRecordDisplay1.Fit(true);
                        }
                    }
                }
                else if (value is LpBmpImage lpBmpImage)
                {
                    if (cogRecordDisplay1.InvokeRequired)
                    {
                        cogRecordDisplay1.Invoke(new Action(() =>
                        {
                            LaonImage = lpBmpImage;
                        }));
                    }
                    else
                    {
                        var oldImage = cogRecordDisplay1.Image;
                        var newImage = LpCogImage.ConvertCogImage(lpBmpImage.OriginalImage as Bitmap);
                        cogRecordDisplay1.Image = newImage;
                        if (!IsEqualSize(oldImage, newImage))
                        {
                            cogRecordDisplay1.Fit(true);
                        }
                    }
                }
            }
        }

        public ILpToolResult LaonResult
        {
            get { return new LpCogToolResult(cogRecordDisplay1.Record); }
            set
            {
                if (value is ILpCogToolResult result)
                {
                    if (InvokeRequired)
                    {
                        this.Invoke(new Action(() =>
                        {
                            LaonResult = result;
                        }));
                    }
                    else
                    {
                        cogRecordDisplay1.DrawingEnabled = false;
                        var oldRecord = cogRecordDisplay1.Record;
                        cogRecordDisplay1.Record = result.CogRecord;
                        cogRecordDisplay1.BackColor = _backColor;
                        if (!IsEqualSize(oldRecord, result.CogRecord))
                        {
                            cogRecordDisplay1.Fit(true);
                        }
                        cogRecordDisplay1.DrawingEnabled = true;
                    }
                }
            }
        }

        private bool IsEqualSize(ICogRecord oldRecord, ICogRecord cogRecord)
        {
            if (oldRecord == null) return false;
            if (oldRecord.Content is ICogImage oldImage && cogRecord.Content is ICogImage newImage)
            {
                return oldImage.Width == newImage.Width && oldImage.Height == newImage.Height;
            }
            else
                return false;
        }

        private bool IsEqualSize(ICogImage oldImage, ICogImage newImage)
        {
            if (oldImage == null) return false;
            try
            {
                var result = oldImage.Width == newImage.Width && oldImage.Height == newImage.Height;
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public LaonCogDisplay(Color backColor)
        {
            InitializeComponent();
            _backColor = backColor;
            InitControl();
        }

        private void InitControl()
        {
            DoubleBuffered = true;
            cogRecordDisplay1.BackColor = _backColor;
            cogDisplayStatusBarV21.Display = cogRecordDisplay1;
        }

        public void DisplayFit(bool status)
        {
            Display.Fit(status);
        }

        public Image GetOverlayImage()
        {
            return Display.CreateContentBitmap(Cognex.VisionPro.Display.CogDisplayContentBitmapConstants.Display);
        }

        public void SaveImage()
        {
            Display.CreateContentBitmap(Cognex.VisionPro.Display.CogDisplayContentBitmapConstants.Custom).Save($"C:\\Users\\laonpeople\\Desktop\\아레이몬드\\{DateTime.Now:MMddHHmmssfff}.bmp", ImageFormat.Bmp);
        }

        public Bitmap CaptureDisplay()
        {
            if (InvokeRequired)
            {
                return Invoke(new Func<Bitmap>(() => CaptureDisplay()));
            }
            else
            {
                var bmp = new Bitmap(cogRecordDisplay1.Width, cogRecordDisplay1.Height);
                Graphics g = Graphics.FromImage(bmp);
                var location = PointToScreen(Point.Empty);
                location = new Point(location.X + Margin.Left, location.Y + Margin.Top);
                g.CopyFromScreen(location, Point.Empty, cogRecordDisplay1.Size);
                return bmp;
            }
        }

        public void OverlayClear()
        {
            Display.InteractiveGraphics.Clear();
        }
    }
}
