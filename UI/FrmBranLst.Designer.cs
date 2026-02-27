namespace RobotLocation.UI
{
    partial class FrmBranLst
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBranLst));
            this.rBtnLst = new Sunny.UI.UIRadioButtonGroup();
            this.btnApp = new Sunny.UI.UISymbolButton();
            this.SuspendLayout();
            // 
            // rBtnLst
            // 
            this.rBtnLst.ColumnCount = 6;
            this.rBtnLst.ColumnInterval = 20;
            this.rBtnLst.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rBtnLst.Location = new System.Drawing.Point(16, 40);
            this.rBtnLst.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rBtnLst.MinimumSize = new System.Drawing.Size(1, 1);
            this.rBtnLst.Name = "rBtnLst";
            this.rBtnLst.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.rBtnLst.Size = new System.Drawing.Size(1180, 690);
            this.rBtnLst.TabIndex = 0;
            this.rBtnLst.Text = "品牌列表";
            this.rBtnLst.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnApp
            // 
            this.btnApp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnApp.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnApp.Location = new System.Drawing.Point(879, 746);
            this.btnApp.Margin = new System.Windows.Forms.Padding(15, 3, 3, 3);
            this.btnApp.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnApp.Name = "btnApp";
            this.btnApp.Size = new System.Drawing.Size(118, 51);
            this.btnApp.Symbol = 557697;
            this.btnApp.TabIndex = 10;
            this.btnApp.Text = "确定";
            this.btnApp.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnApp.Click += new System.EventHandler(this.BtnApp_Click);
            // 
            // FrmBranLst
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Controls.Add(this.btnApp);
            this.Controls.Add(this.rBtnLst);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmBranLst";
            this.Text = "品牌列表";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 800, 450);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UIRadioButtonGroup rBtnLst;
        private Sunny.UI.UISymbolButton btnApp;
    }
}