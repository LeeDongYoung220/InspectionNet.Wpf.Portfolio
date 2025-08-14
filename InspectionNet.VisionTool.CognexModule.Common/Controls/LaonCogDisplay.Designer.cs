namespace InspectionNet.VisionTool.CognexModule.Common.Controls
{
    partial class LaonCogDisplay
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            cogDisplayStatusBarV21 = new Cognex.VisionPro.CogDisplayStatusBarV2();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            cogRecordDisplay1 = new Cognex.VisionPro.CogRecordDisplay();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // cogDisplayStatusBarV21
            // 
            cogDisplayStatusBarV21.CoordinateSpaceName = "*\\#";
            cogDisplayStatusBarV21.CoordinateSpaceName3D = "*\\#";
            cogDisplayStatusBarV21.Dock = System.Windows.Forms.DockStyle.Fill;
            cogDisplayStatusBarV21.Location = new System.Drawing.Point(0, 531);
            cogDisplayStatusBarV21.Margin = new System.Windows.Forms.Padding(0);
            cogDisplayStatusBarV21.Name = "cogDisplayStatusBarV21";
            cogDisplayStatusBarV21.RightToLeft = System.Windows.Forms.RightToLeft.No;
            cogDisplayStatusBarV21.Size = new System.Drawing.Size(800, 31);
            cogDisplayStatusBarV21.TabIndex = 1;
            cogDisplayStatusBarV21.Use3DCoordinateSpaceTree = false;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(cogDisplayStatusBarV21, 0, 1);
            tableLayoutPanel1.Controls.Add(cogRecordDisplay1, 0, 0);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            tableLayoutPanel1.Size = new System.Drawing.Size(800, 562);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // cogRecordDisplay1
            // 
            cogRecordDisplay1.Dock = System.Windows.Forms.DockStyle.Fill;
            cogRecordDisplay1.HorizontalScrollBar = false;
            cogRecordDisplay1.Location = new System.Drawing.Point(3, 3);
            cogRecordDisplay1.Name = "cogRecordDisplay1";
            cogRecordDisplay1.Size = new System.Drawing.Size(794, 525);
            cogRecordDisplay1.TabIndex = 2;
            cogRecordDisplay1.Text = "cogRecordDisplay1";
            cogRecordDisplay1.VerticalScrollBar = false;
            // 
            // LaonCogDisplay
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel1);
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "LaonCogDisplay";
            Size = new System.Drawing.Size(800, 562);
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion
        private Cognex.VisionPro.CogDisplayStatusBarV2 cogDisplayStatusBarV21;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Cognex.VisionPro.CogRecordDisplay cogRecordDisplay1;
    }
}
