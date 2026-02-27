using System.Linq;
using System.Windows.Forms;

namespace RobotLocation.UI
{
    partial class MFrm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MFrm));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.uiStyleManager1 = new Sunny.UI.UIStyleManager(this.components);
            this.uiTabControl1 = new Sunny.UI.UITabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.uiButton6 = new Sunny.UI.UIButton();
            this.uiLabel30 = new Sunny.UI.UILabel();
            this.uiLabel29 = new Sunny.UI.UILabel();
            this.uiLabel28 = new Sunny.UI.UILabel();
            this.uiLabel27 = new Sunny.UI.UILabel();
            this.uiButton14 = new Sunny.UI.UIButton();
            this.uiLedLabel1 = new Sunny.UI.UILedLabel();
            this.uiGroupBox4 = new Sunny.UI.UIGroupBox();
            this.uiLabel53 = new Sunny.UI.UILabel();
            this.uiLabel51 = new Sunny.UI.UILabel();
            this.uiLabel52 = new Sunny.UI.UILabel();
            this.uiLabel46 = new Sunny.UI.UILabel();
            this.uiLabel45 = new Sunny.UI.UILabel();
            this.uiLabel44 = new Sunny.UI.UILabel();
            this.uiLabel43 = new Sunny.UI.UILabel();
            this.uiLabel42 = new Sunny.UI.UILabel();
            this.uiLabel37 = new Sunny.UI.UILabel();
            this.uiLabel36 = new Sunny.UI.UILabel();
            this.uiLabel35 = new Sunny.UI.UILabel();
            this.uiLabel34 = new Sunny.UI.UILabel();
            this.uiLabel33 = new Sunny.UI.UILabel();
            this.uiLabel32 = new Sunny.UI.UILabel();
            this.uiLabel41 = new Sunny.UI.UILabel();
            this.uiLabel31 = new Sunny.UI.UILabel();
            this.uiLabel40 = new Sunny.UI.UILabel();
            this.uiLabel26 = new Sunny.UI.UILabel();
            this.uiLabel39 = new Sunny.UI.UILabel();
            this.uiLabel23 = new Sunny.UI.UILabel();
            this.uiLabel38 = new Sunny.UI.UILabel();
            this.uiLabel22 = new Sunny.UI.UILabel();
            this.uiLabel66 = new Sunny.UI.UILabel();
            this.uiLabel67 = new Sunny.UI.UILabel();
            this.uiLabel70 = new Sunny.UI.UILabel();
            this.uiLabel71 = new Sunny.UI.UILabel();
            this.uiLabel74 = new Sunny.UI.UILabel();
            this.uiLabel75 = new Sunny.UI.UILabel();
            this.uiLabel50 = new Sunny.UI.UILabel();
            this.uiLabel49 = new Sunny.UI.UILabel();
            this.uiLabel48 = new Sunny.UI.UILabel();
            this.uiLabel47 = new Sunny.UI.UILabel();
            this.SaveButton = new Sunny.UI.UIButton();
            this.OPStopButton = new Sunny.UI.UIButton();
            this.ValButton = new Sunny.UI.UIButton();
            this.OpButton = new Sunny.UI.UIButton();
            this.AddSolButton = new Sunny.UI.UIButton();
            this.SolButton = new Sunny.UI.UIButton();
            this.SolComboBox = new Sunny.UI.UIComboBox();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.uiGroupBox2 = new Sunny.UI.UIGroupBox();
            this.ODeviation = new Sunny.UI.UIIntegerUpDown();
            this.uiLabel18 = new Sunny.UI.UILabel();
            this.IDeviation = new Sunny.UI.UIIntegerUpDown();
            this.uiLabel17 = new Sunny.UI.UILabel();
            this.OutOffBlowTime = new Sunny.UI.UIIntegerUpDown();
            this.RemoveSet = new Sunny.UI.UIButton();
            this.uiLabel13 = new Sunny.UI.UILabel();
            this.OutsideStep3 = new Sunny.UI.UIIntegerUpDown();
            this.OutBlowTime = new Sunny.UI.UIIntegerUpDown();
            this.uiLabel12 = new Sunny.UI.UILabel();
            this.uiLabel14 = new Sunny.UI.UILabel();
            this.OutsideStep2 = new Sunny.UI.UIIntegerUpDown();
            this.InOffBlowTime = new Sunny.UI.UIIntegerUpDown();
            this.uiLabel11 = new Sunny.UI.UILabel();
            this.uiLabel15 = new Sunny.UI.UILabel();
            this.OutsideStep1 = new Sunny.UI.UIIntegerUpDown();
            this.InBlowTime = new Sunny.UI.UIIntegerUpDown();
            this.uiLabel16 = new Sunny.UI.UILabel();
            this.uiLabel10 = new Sunny.UI.UILabel();
            this.InsideStep3 = new Sunny.UI.UIIntegerUpDown();
            this.uiLabel9 = new Sunny.UI.UILabel();
            this.InsideStep2 = new Sunny.UI.UIIntegerUpDown();
            this.uiLabel8 = new Sunny.UI.UILabel();
            this.InsideStep1 = new Sunny.UI.UIIntegerUpDown();
            this.uiLabel7 = new Sunny.UI.UILabel();
            this.WorkGroupBox = new Sunny.UI.UIGroupBox();
            this.StirRotation = new Sunny.UI.UIButton();
            this.Turn2Rotation = new Sunny.UI.UIButton();
            this.Turn1Rotation = new Sunny.UI.UIButton();
            this.CutRotation = new Sunny.UI.UIButton();
            this.MainRotation = new Sunny.UI.UIButton();
            this.StirStart = new Sunny.UI.UIButton();
            this.StirUpDown = new Sunny.UI.UIIntegerUpDown();
            this.uiLabel6 = new Sunny.UI.UILabel();
            this.Turn2Start = new Sunny.UI.UIButton();
            this.Turn1Start = new Sunny.UI.UIButton();
            this.CutStart = new Sunny.UI.UIButton();
            this.MainStart = new Sunny.UI.UIButton();
            this.TurnUpDown2 = new Sunny.UI.UIIntegerUpDown();
            this.uiLabel5 = new Sunny.UI.UILabel();
            this.MainUpDown = new Sunny.UI.UIIntegerUpDown();
            this.TurnUpDown1 = new Sunny.UI.UIIntegerUpDown();
            this.CutUpDown = new Sunny.UI.UIIntegerUpDown();
            this.uiLabel4 = new Sunny.UI.UILabel();
            this.uiLabel3 = new Sunny.UI.UILabel();
            this.uiLabel2 = new Sunny.UI.UILabel();
            this.SaveABButton = new Sunny.UI.UIButton();
            this.CleanFlow = new Sunny.UI.UIButton();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.uiGroupBox5 = new Sunny.UI.UIGroupBox();
            this.uiPanel4 = new Sunny.UI.UIPanel();
            this.uiProcessBar1 = new Sunny.UI.UIProcessBar();
            this.uiButton5 = new Sunny.UI.UIButton();
            this.uiButton4 = new Sunny.UI.UIButton();
            this.cal_step1 = new Sunny.UI.UIBreadcrumb();
            this.uiButton3 = new Sunny.UI.UIButton();
            this.uiButton2 = new Sunny.UI.UIButton();
            this.uiGroupBox3 = new Sunny.UI.UIGroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.uiNumPadTextBox7 = new Sunny.UI.UINumPadTextBox();
            this.uiLabel57 = new Sunny.UI.UILabel();
            this.uiNumPadTextBox6 = new Sunny.UI.UINumPadTextBox();
            this.uiLabel56 = new Sunny.UI.UILabel();
            this.uiNumPadTextBox5 = new Sunny.UI.UINumPadTextBox();
            this.uiLabel55 = new Sunny.UI.UILabel();
            this.uiNumPadTextBox1 = new Sunny.UI.UINumPadTextBox();
            this.uiLabel54 = new Sunny.UI.UILabel();
            this.uiLabel25 = new Sunny.UI.UILabel();
            this.uiIntegerUpDown1 = new Sunny.UI.UIIntegerUpDown();
            this.uiButton1 = new Sunny.UI.UIButton();
            this.uiLabel24 = new Sunny.UI.UILabel();
            this.uiSwitch1 = new Sunny.UI.UISwitch();
            this.uiNumPadTextBox4 = new Sunny.UI.UINumPadTextBox();
            this.uiNumPadTextBox3 = new Sunny.UI.UINumPadTextBox();
            this.uiNumPadTextBox2 = new Sunny.UI.UINumPadTextBox();
            this.uiButton13 = new Sunny.UI.UIButton();
            this.uiButton12 = new Sunny.UI.UIButton();
            this.uiLabel21 = new Sunny.UI.UILabel();
            this.uiLabel19 = new Sunny.UI.UILabel();
            this.uiLabel20 = new Sunny.UI.UILabel();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.uiDataGridView1 = new Sunny.UI.UIDataGridView();
            this.uiButton11 = new Sunny.UI.UIButton();
            this.uiDatePicker1 = new Sunny.UI.UIDatePicker();
            this.uiPanel1 = new Sunny.UI.UIPanel();
            this.uiPanel2 = new Sunny.UI.UIPanel();
            this.uiPanel3 = new Sunny.UI.UIPanel();
            this.MPanel = new ImageControl.ViewPanel();
            this.uiGroupBox1 = new Sunny.UI.UIGroupBox();
            this.logView1 = new VisionCore.Component.LogView();
            this.uiTabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.uiGroupBox4.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.uiGroupBox2.SuspendLayout();
            this.WorkGroupBox.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.uiGroupBox5.SuspendLayout();
            this.uiGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiDataGridView1)).BeginInit();
            this.uiPanel1.SuspendLayout();
            this.uiPanel2.SuspendLayout();
            this.uiPanel3.SuspendLayout();
            this.uiGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // uiStyleManager1
            // 
            this.uiStyleManager1.GlobalFont = true;
            this.uiStyleManager1.GlobalFontName = "微软雅黑";
            // 
            // uiTabControl1
            // 
            this.uiTabControl1.Controls.Add(this.tabPage2);
            this.uiTabControl1.Controls.Add(this.tabPage1);
            this.uiTabControl1.Controls.Add(this.tabPage3);
            this.uiTabControl1.Controls.Add(this.tabPage4);
            this.uiTabControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.uiTabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.uiTabControl1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiTabControl1.ItemSize = new System.Drawing.Size(80, 50);
            this.uiTabControl1.Location = new System.Drawing.Point(0, 0);
            this.uiTabControl1.MainPage = "";
            this.uiTabControl1.Name = "uiTabControl1";
            this.uiTabControl1.SelectedIndex = 0;
            this.uiTabControl1.Size = new System.Drawing.Size(473, 827);
            this.uiTabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.uiTabControl1.TabIndex = 1;
            this.uiTabControl1.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiTabControl1.Click += new System.EventHandler(this.readplcdate);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.White;
            this.tabPage2.Controls.Add(this.uiButton6);
            this.tabPage2.Controls.Add(this.uiLabel30);
            this.tabPage2.Controls.Add(this.uiLabel29);
            this.tabPage2.Controls.Add(this.uiLabel28);
            this.tabPage2.Controls.Add(this.uiLabel27);
            this.tabPage2.Controls.Add(this.uiButton14);
            this.tabPage2.Controls.Add(this.uiLedLabel1);
            this.tabPage2.Controls.Add(this.uiGroupBox4);
            this.tabPage2.Controls.Add(this.uiLabel50);
            this.tabPage2.Controls.Add(this.uiLabel49);
            this.tabPage2.Controls.Add(this.uiLabel48);
            this.tabPage2.Controls.Add(this.uiLabel47);
            this.tabPage2.Controls.Add(this.SaveButton);
            this.tabPage2.Controls.Add(this.OPStopButton);
            this.tabPage2.Controls.Add(this.ValButton);
            this.tabPage2.Controls.Add(this.OpButton);
            this.tabPage2.Controls.Add(this.AddSolButton);
            this.tabPage2.Controls.Add(this.SolButton);
            this.tabPage2.Controls.Add(this.SolComboBox);
            this.tabPage2.Controls.Add(this.uiLabel1);
            this.tabPage2.Location = new System.Drawing.Point(0, 50);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(473, 777);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "主界面";
            // 
            // uiButton6
            // 
            this.uiButton6.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton6.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton6.Location = new System.Drawing.Point(403, 555);
            this.uiButton6.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton6.Name = "uiButton6";
            this.uiButton6.Size = new System.Drawing.Size(67, 34);
            this.uiButton6.TabIndex = 50;
            this.uiButton6.Text = "关机";
            this.uiButton6.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton6.Click += new System.EventHandler(this.uiButton6_Click);
            // 
            // uiLabel30
            // 
            this.uiLabel30.Font = new System.Drawing.Font("微软雅黑", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel30.ForeColor = System.Drawing.Color.Red;
            this.uiLabel30.Location = new System.Drawing.Point(104, 126);
            this.uiLabel30.Name = "uiLabel30";
            this.uiLabel30.Size = new System.Drawing.Size(249, 46);
            this.uiLabel30.TabIndex = 49;
            this.uiLabel30.Text = "0";
            this.uiLabel30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel29
            // 
            this.uiLabel29.Font = new System.Drawing.Font("微软雅黑", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel29.ForeColor = System.Drawing.Color.Lime;
            this.uiLabel29.Location = new System.Drawing.Point(12, 122);
            this.uiLabel29.Name = "uiLabel29";
            this.uiLabel29.Size = new System.Drawing.Size(86, 55);
            this.uiLabel29.TabIndex = 48;
            this.uiLabel29.Text = "通过";
            this.uiLabel29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel28
            // 
            this.uiLabel28.Font = new System.Drawing.Font("微软雅黑", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel28.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel28.Location = new System.Drawing.Point(109, 61);
            this.uiLabel28.Name = "uiLabel28";
            this.uiLabel28.Size = new System.Drawing.Size(244, 46);
            this.uiLabel28.TabIndex = 47;
            this.uiLabel28.Text = "0";
            this.uiLabel28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel27
            // 
            this.uiLabel27.Font = new System.Drawing.Font("微软雅黑", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel27.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel27.Location = new System.Drawing.Point(10, 54);
            this.uiLabel27.Name = "uiLabel27";
            this.uiLabel27.Size = new System.Drawing.Size(86, 55);
            this.uiLabel27.TabIndex = 46;
            this.uiLabel27.Text = "剔除";
            this.uiLabel27.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiButton14
            // 
            this.uiButton14.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton14.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton14.Location = new System.Drawing.Point(324, 555);
            this.uiButton14.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton14.Name = "uiButton14";
            this.uiButton14.Size = new System.Drawing.Size(67, 34);
            this.uiButton14.TabIndex = 45;
            this.uiButton14.Text = "退出";
            this.uiButton14.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton14.Click += new System.EventHandler(this.uiButton14_Click);
            // 
            // uiLedLabel1
            // 
            this.uiLedLabel1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLedLabel1.ForeColor = System.Drawing.Color.Red;
            this.uiLedLabel1.Location = new System.Drawing.Point(19, 189);
            this.uiLedLabel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiLedLabel1.Name = "uiLedLabel1";
            this.uiLedLabel1.Size = new System.Drawing.Size(424, 35);
            this.uiLedLabel1.TabIndex = 44;
            this.uiLedLabel1.Text = "0";
            // 
            // uiGroupBox4
            // 
            this.uiGroupBox4.Controls.Add(this.uiLabel53);
            this.uiGroupBox4.Controls.Add(this.uiLabel51);
            this.uiGroupBox4.Controls.Add(this.uiLabel52);
            this.uiGroupBox4.Controls.Add(this.uiLabel46);
            this.uiGroupBox4.Controls.Add(this.uiLabel45);
            this.uiGroupBox4.Controls.Add(this.uiLabel44);
            this.uiGroupBox4.Controls.Add(this.uiLabel43);
            this.uiGroupBox4.Controls.Add(this.uiLabel42);
            this.uiGroupBox4.Controls.Add(this.uiLabel37);
            this.uiGroupBox4.Controls.Add(this.uiLabel36);
            this.uiGroupBox4.Controls.Add(this.uiLabel35);
            this.uiGroupBox4.Controls.Add(this.uiLabel34);
            this.uiGroupBox4.Controls.Add(this.uiLabel33);
            this.uiGroupBox4.Controls.Add(this.uiLabel32);
            this.uiGroupBox4.Controls.Add(this.uiLabel41);
            this.uiGroupBox4.Controls.Add(this.uiLabel31);
            this.uiGroupBox4.Controls.Add(this.uiLabel40);
            this.uiGroupBox4.Controls.Add(this.uiLabel26);
            this.uiGroupBox4.Controls.Add(this.uiLabel39);
            this.uiGroupBox4.Controls.Add(this.uiLabel23);
            this.uiGroupBox4.Controls.Add(this.uiLabel38);
            this.uiGroupBox4.Controls.Add(this.uiLabel22);
            this.uiGroupBox4.Controls.Add(this.uiLabel66);
            this.uiGroupBox4.Controls.Add(this.uiLabel67);
            this.uiGroupBox4.Controls.Add(this.uiLabel70);
            this.uiGroupBox4.Controls.Add(this.uiLabel71);
            this.uiGroupBox4.Controls.Add(this.uiLabel74);
            this.uiGroupBox4.Controls.Add(this.uiLabel75);
            this.uiGroupBox4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox4.Location = new System.Drawing.Point(4, 236);
            this.uiGroupBox4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox4.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox4.Name = "uiGroupBox4";
            this.uiGroupBox4.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox4.Size = new System.Drawing.Size(465, 294);
            this.uiGroupBox4.TabIndex = 42;
            this.uiGroupBox4.Text = "数据概览";
            this.uiGroupBox4.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel53
            // 
            this.uiLabel53.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel53.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel53.Location = new System.Drawing.Point(328, 256);
            this.uiLabel53.Name = "uiLabel53";
            this.uiLabel53.Size = new System.Drawing.Size(131, 28);
            this.uiLabel53.TabIndex = 73;
            this.uiLabel53.Text = "0";
            this.uiLabel53.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel51
            // 
            this.uiLabel51.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel51.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel51.Location = new System.Drawing.Point(242, 261);
            this.uiLabel51.Name = "uiLabel51";
            this.uiLabel51.Size = new System.Drawing.Size(63, 18);
            this.uiLabel51.TabIndex = 72;
            this.uiLabel51.Text = "色差：";
            this.uiLabel51.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel52
            // 
            this.uiLabel52.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel52.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel52.Location = new System.Drawing.Point(328, 218);
            this.uiLabel52.Name = "uiLabel52";
            this.uiLabel52.Size = new System.Drawing.Size(76, 28);
            this.uiLabel52.TabIndex = 71;
            this.uiLabel52.Text = "0";
            this.uiLabel52.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel46
            // 
            this.uiLabel46.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel46.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel46.Location = new System.Drawing.Point(328, 181);
            this.uiLabel46.Name = "uiLabel46";
            this.uiLabel46.Size = new System.Drawing.Size(76, 28);
            this.uiLabel46.TabIndex = 70;
            this.uiLabel46.Text = "0";
            this.uiLabel46.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel45
            // 
            this.uiLabel45.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel45.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel45.Location = new System.Drawing.Point(328, 145);
            this.uiLabel45.Name = "uiLabel45";
            this.uiLabel45.Size = new System.Drawing.Size(76, 28);
            this.uiLabel45.TabIndex = 69;
            this.uiLabel45.Text = "0";
            this.uiLabel45.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel44
            // 
            this.uiLabel44.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel44.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel44.Location = new System.Drawing.Point(328, 107);
            this.uiLabel44.Name = "uiLabel44";
            this.uiLabel44.Size = new System.Drawing.Size(76, 28);
            this.uiLabel44.TabIndex = 68;
            this.uiLabel44.Text = "0";
            this.uiLabel44.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel43
            // 
            this.uiLabel43.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel43.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel43.Location = new System.Drawing.Point(328, 72);
            this.uiLabel43.Name = "uiLabel43";
            this.uiLabel43.Size = new System.Drawing.Size(76, 28);
            this.uiLabel43.TabIndex = 67;
            this.uiLabel43.Text = "0";
            this.uiLabel43.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel42
            // 
            this.uiLabel42.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel42.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel42.Location = new System.Drawing.Point(328, 32);
            this.uiLabel42.Name = "uiLabel42";
            this.uiLabel42.Size = new System.Drawing.Size(76, 28);
            this.uiLabel42.TabIndex = 66;
            this.uiLabel42.Text = "0";
            this.uiLabel42.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel37
            // 
            this.uiLabel37.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel37.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel37.Location = new System.Drawing.Point(241, 221);
            this.uiLabel37.Name = "uiLabel37";
            this.uiLabel37.Size = new System.Drawing.Size(63, 18);
            this.uiLabel37.TabIndex = 65;
            this.uiLabel37.Text = "圆度：";
            this.uiLabel37.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel36
            // 
            this.uiLabel36.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel36.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel36.Location = new System.Drawing.Point(242, 186);
            this.uiLabel36.Name = "uiLabel36";
            this.uiLabel36.Size = new System.Drawing.Size(63, 18);
            this.uiLabel36.TabIndex = 64;
            this.uiLabel36.Text = "尺寸：";
            this.uiLabel36.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel35
            // 
            this.uiLabel35.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel35.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel35.Location = new System.Drawing.Point(242, 147);
            this.uiLabel35.Name = "uiLabel35";
            this.uiLabel35.Size = new System.Drawing.Size(63, 18);
            this.uiLabel35.TabIndex = 63;
            this.uiLabel35.Text = "空壳：";
            this.uiLabel35.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel34
            // 
            this.uiLabel34.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel34.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel34.Location = new System.Drawing.Point(242, 111);
            this.uiLabel34.Name = "uiLabel34";
            this.uiLabel34.Size = new System.Drawing.Size(63, 18);
            this.uiLabel34.TabIndex = 62;
            this.uiLabel34.Text = "异型：";
            this.uiLabel34.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel33
            // 
            this.uiLabel33.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel33.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel33.Location = new System.Drawing.Point(242, 77);
            this.uiLabel33.Name = "uiLabel33";
            this.uiLabel33.Size = new System.Drawing.Size(63, 18);
            this.uiLabel33.TabIndex = 61;
            this.uiLabel33.Text = "气泡：";
            this.uiLabel33.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel32
            // 
            this.uiLabel32.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel32.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel32.Location = new System.Drawing.Point(242, 37);
            this.uiLabel32.Name = "uiLabel32";
            this.uiLabel32.Size = new System.Drawing.Size(63, 18);
            this.uiLabel32.TabIndex = 60;
            this.uiLabel32.Text = "凸点：";
            this.uiLabel32.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel41
            // 
            this.uiLabel41.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel41.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel41.Location = new System.Drawing.Point(117, 256);
            this.uiLabel41.Name = "uiLabel41";
            this.uiLabel41.Size = new System.Drawing.Size(76, 28);
            this.uiLabel41.TabIndex = 59;
            this.uiLabel41.Text = "0";
            this.uiLabel41.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel31
            // 
            this.uiLabel31.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel31.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel31.Location = new System.Drawing.Point(24, 261);
            this.uiLabel31.Name = "uiLabel31";
            this.uiLabel31.Size = new System.Drawing.Size(63, 18);
            this.uiLabel31.TabIndex = 58;
            this.uiLabel31.Text = "凹陷：";
            this.uiLabel31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel40
            // 
            this.uiLabel40.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel40.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel40.Location = new System.Drawing.Point(116, 216);
            this.uiLabel40.Name = "uiLabel40";
            this.uiLabel40.Size = new System.Drawing.Size(76, 28);
            this.uiLabel40.TabIndex = 57;
            this.uiLabel40.Text = "0";
            this.uiLabel40.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel26
            // 
            this.uiLabel26.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel26.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel26.Location = new System.Drawing.Point(22, 221);
            this.uiLabel26.Name = "uiLabel26";
            this.uiLabel26.Size = new System.Drawing.Size(63, 18);
            this.uiLabel26.TabIndex = 56;
            this.uiLabel26.Text = "偏心：";
            this.uiLabel26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel39
            // 
            this.uiLabel39.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel39.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel39.Location = new System.Drawing.Point(117, 179);
            this.uiLabel39.Name = "uiLabel39";
            this.uiLabel39.Size = new System.Drawing.Size(76, 28);
            this.uiLabel39.TabIndex = 55;
            this.uiLabel39.Text = "0";
            this.uiLabel39.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel23
            // 
            this.uiLabel23.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel23.Location = new System.Drawing.Point(20, 184);
            this.uiLabel23.Name = "uiLabel23";
            this.uiLabel23.Size = new System.Drawing.Size(63, 18);
            this.uiLabel23.TabIndex = 54;
            this.uiLabel23.Text = "脏污：";
            this.uiLabel23.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel38
            // 
            this.uiLabel38.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel38.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel38.Location = new System.Drawing.Point(116, 140);
            this.uiLabel38.Name = "uiLabel38";
            this.uiLabel38.Size = new System.Drawing.Size(76, 28);
            this.uiLabel38.TabIndex = 53;
            this.uiLabel38.Text = "0";
            this.uiLabel38.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel22
            // 
            this.uiLabel22.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel22.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel22.Location = new System.Drawing.Point(20, 145);
            this.uiLabel22.Name = "uiLabel22";
            this.uiLabel22.Size = new System.Drawing.Size(63, 18);
            this.uiLabel22.TabIndex = 52;
            this.uiLabel22.Text = "皮帽：";
            this.uiLabel22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel66
            // 
            this.uiLabel66.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel66.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel66.Location = new System.Drawing.Point(116, 102);
            this.uiLabel66.Name = "uiLabel66";
            this.uiLabel66.Size = new System.Drawing.Size(76, 28);
            this.uiLabel66.TabIndex = 51;
            this.uiLabel66.Text = "0";
            this.uiLabel66.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel67
            // 
            this.uiLabel67.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel67.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel67.Location = new System.Drawing.Point(19, 107);
            this.uiLabel67.Name = "uiLabel67";
            this.uiLabel67.Size = new System.Drawing.Size(75, 23);
            this.uiLabel67.TabIndex = 50;
            this.uiLabel67.Text = "标准差：";
            this.uiLabel67.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel70
            // 
            this.uiLabel70.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel70.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel70.Location = new System.Drawing.Point(117, 33);
            this.uiLabel70.Name = "uiLabel70";
            this.uiLabel70.Size = new System.Drawing.Size(76, 28);
            this.uiLabel70.TabIndex = 46;
            this.uiLabel70.Text = "0";
            this.uiLabel70.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel71
            // 
            this.uiLabel71.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel71.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel71.Location = new System.Drawing.Point(117, 68);
            this.uiLabel71.Name = "uiLabel71";
            this.uiLabel71.Size = new System.Drawing.Size(76, 28);
            this.uiLabel71.TabIndex = 45;
            this.uiLabel71.Text = "0";
            this.uiLabel71.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel74
            // 
            this.uiLabel74.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel74.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel74.Location = new System.Drawing.Point(20, 38);
            this.uiLabel74.Name = "uiLabel74";
            this.uiLabel74.Size = new System.Drawing.Size(91, 18);
            this.uiLabel74.TabIndex = 36;
            this.uiLabel74.Text = "标圆粒径：";
            this.uiLabel74.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel75
            // 
            this.uiLabel75.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel75.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel75.Location = new System.Drawing.Point(20, 73);
            this.uiLabel75.Name = "uiLabel75";
            this.uiLabel75.Size = new System.Drawing.Size(74, 18);
            this.uiLabel75.TabIndex = 33;
            this.uiLabel75.Text = "圆度值：";
            this.uiLabel75.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel50
            // 
            this.uiLabel50.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel50.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel50.Location = new System.Drawing.Point(300, 13);
            this.uiLabel50.Name = "uiLabel50";
            this.uiLabel50.Size = new System.Drawing.Size(163, 39);
            this.uiLabel50.TabIndex = 41;
            this.uiLabel50.Text = "0";
            this.uiLabel50.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel49
            // 
            this.uiLabel49.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel49.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel49.Location = new System.Drawing.Point(59, 10);
            this.uiLabel49.Name = "uiLabel49";
            this.uiLabel49.Size = new System.Drawing.Size(165, 42);
            this.uiLabel49.TabIndex = 40;
            this.uiLabel49.Text = "0";
            this.uiLabel49.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel48
            // 
            this.uiLabel48.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel48.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel48.Location = new System.Drawing.Point(241, 10);
            this.uiLabel48.Name = "uiLabel48";
            this.uiLabel48.Size = new System.Drawing.Size(48, 45);
            this.uiLabel48.TabIndex = 39;
            this.uiLabel48.Text = "NG";
            this.uiLabel48.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel47
            // 
            this.uiLabel47.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel47.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel47.Location = new System.Drawing.Point(10, 13);
            this.uiLabel47.Name = "uiLabel47";
            this.uiLabel47.Size = new System.Drawing.Size(49, 41);
            this.uiLabel47.TabIndex = 38;
            this.uiLabel47.Text = "OK";
            this.uiLabel47.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SaveButton
            // 
            this.SaveButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SaveButton.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SaveButton.Location = new System.Drawing.Point(235, 555);
            this.SaveButton.MinimumSize = new System.Drawing.Size(1, 1);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(63, 34);
            this.SaveButton.TabIndex = 37;
            this.SaveButton.Text = "保存";
            this.SaveButton.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // OPStopButton
            // 
            this.OPStopButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.OPStopButton.FillColor = System.Drawing.Color.Gray;
            this.OPStopButton.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OPStopButton.Location = new System.Drawing.Point(273, 713);
            this.OPStopButton.MinimumSize = new System.Drawing.Size(1, 1);
            this.OPStopButton.Name = "OPStopButton";
            this.OPStopButton.Size = new System.Drawing.Size(157, 56);
            this.OPStopButton.TabIndex = 34;
            this.OPStopButton.Text = "停止";
            this.OPStopButton.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OPStopButton.Click += new System.EventHandler(this.OPStopButton_Click);
            // 
            // ValButton
            // 
            this.ValButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ValButton.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ValButton.Location = new System.Drawing.Point(122, 555);
            this.ValButton.MinimumSize = new System.Drawing.Size(1, 1);
            this.ValButton.Name = "ValButton";
            this.ValButton.Size = new System.Drawing.Size(89, 34);
            this.ValButton.TabIndex = 36;
            this.ValButton.Text = "系统设置";
            this.ValButton.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ValButton.Click += new System.EventHandler(this.ValButton_Click);
            // 
            // OpButton
            // 
            this.OpButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.OpButton.FillColor = System.Drawing.Color.Gray;
            this.OpButton.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OpButton.Location = new System.Drawing.Point(27, 713);
            this.OpButton.MinimumSize = new System.Drawing.Size(1, 1);
            this.OpButton.Name = "OpButton";
            this.OpButton.Size = new System.Drawing.Size(161, 56);
            this.OpButton.TabIndex = 19;
            this.OpButton.Text = "启动";
            this.OpButton.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OpButton.Click += new System.EventHandler(this.OpButton_Click);
            // 
            // AddSolButton
            // 
            this.AddSolButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AddSolButton.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.AddSolButton.Location = new System.Drawing.Point(335, 628);
            this.AddSolButton.MinimumSize = new System.Drawing.Size(1, 1);
            this.AddSolButton.Name = "AddSolButton";
            this.AddSolButton.Size = new System.Drawing.Size(100, 29);
            this.AddSolButton.TabIndex = 18;
            this.AddSolButton.Text = "克隆方案";
            this.AddSolButton.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.AddSolButton.Click += new System.EventHandler(this.AddSolButton_Click);
            // 
            // SolButton
            // 
            this.SolButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SolButton.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SolButton.Location = new System.Drawing.Point(13, 555);
            this.SolButton.MinimumSize = new System.Drawing.Size(1, 1);
            this.SolButton.Name = "SolButton";
            this.SolButton.Size = new System.Drawing.Size(88, 34);
            this.SolButton.TabIndex = 35;
            this.SolButton.Text = "方案设置";
            this.SolButton.TipsFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SolButton.Click += new System.EventHandler(this.SolButton_Click);
            // 
            // SolComboBox
            // 
            this.SolComboBox.DataSource = null;
            this.SolComboBox.FillColor = System.Drawing.Color.White;
            this.SolComboBox.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SolComboBox.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.SolComboBox.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.SolComboBox.Location = new System.Drawing.Point(120, 628);
            this.SolComboBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SolComboBox.MinimumSize = new System.Drawing.Size(63, 0);
            this.SolComboBox.Name = "SolComboBox";
            this.SolComboBox.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.SolComboBox.Size = new System.Drawing.Size(204, 29);
            this.SolComboBox.SymbolSize = 24;
            this.SolComboBox.TabIndex = 17;
            this.SolComboBox.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.SolComboBox.Watermark = "";
            this.SolComboBox.SelectedIndexChanged += new System.EventHandler(this.SolComboBox_SelectedIndexChanged);
            // 
            // uiLabel1
            // 
            this.uiLabel1.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel1.Location = new System.Drawing.Point(15, 628);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(83, 29);
            this.uiLabel1.TabIndex = 16;
            this.uiLabel1.Text = "方案选择";
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.uiGroupBox2);
            this.tabPage1.Controls.Add(this.WorkGroupBox);
            this.tabPage1.Location = new System.Drawing.Point(0, 40);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(200, 60);
            this.tabPage1.TabIndex = 4;
            this.tabPage1.Text = "电气调试";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // uiGroupBox2
            // 
            this.uiGroupBox2.Controls.Add(this.ODeviation);
            this.uiGroupBox2.Controls.Add(this.uiLabel18);
            this.uiGroupBox2.Controls.Add(this.IDeviation);
            this.uiGroupBox2.Controls.Add(this.uiLabel17);
            this.uiGroupBox2.Controls.Add(this.OutOffBlowTime);
            this.uiGroupBox2.Controls.Add(this.RemoveSet);
            this.uiGroupBox2.Controls.Add(this.uiLabel13);
            this.uiGroupBox2.Controls.Add(this.OutsideStep3);
            this.uiGroupBox2.Controls.Add(this.OutBlowTime);
            this.uiGroupBox2.Controls.Add(this.uiLabel12);
            this.uiGroupBox2.Controls.Add(this.uiLabel14);
            this.uiGroupBox2.Controls.Add(this.OutsideStep2);
            this.uiGroupBox2.Controls.Add(this.InOffBlowTime);
            this.uiGroupBox2.Controls.Add(this.uiLabel11);
            this.uiGroupBox2.Controls.Add(this.uiLabel15);
            this.uiGroupBox2.Controls.Add(this.OutsideStep1);
            this.uiGroupBox2.Controls.Add(this.InBlowTime);
            this.uiGroupBox2.Controls.Add(this.uiLabel16);
            this.uiGroupBox2.Controls.Add(this.uiLabel10);
            this.uiGroupBox2.Controls.Add(this.InsideStep3);
            this.uiGroupBox2.Controls.Add(this.uiLabel9);
            this.uiGroupBox2.Controls.Add(this.InsideStep2);
            this.uiGroupBox2.Controls.Add(this.uiLabel8);
            this.uiGroupBox2.Controls.Add(this.InsideStep1);
            this.uiGroupBox2.Controls.Add(this.uiLabel7);
            this.uiGroupBox2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox2.Location = new System.Drawing.Point(4, 348);
            this.uiGroupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox2.Name = "uiGroupBox2";
            this.uiGroupBox2.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox2.Size = new System.Drawing.Size(468, 439);
            this.uiGroupBox2.TabIndex = 38;
            this.uiGroupBox2.Text = "剔除参数";
            this.uiGroupBox2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ODeviation
            // 
            this.ODeviation.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ODeviation.Location = new System.Drawing.Point(343, 353);
            this.ODeviation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ODeviation.Maximum = 15;
            this.ODeviation.Minimum = 0;
            this.ODeviation.MinimumSize = new System.Drawing.Size(100, 0);
            this.ODeviation.Name = "ODeviation";
            this.ODeviation.ShowText = false;
            this.ODeviation.Size = new System.Drawing.Size(109, 29);
            this.ODeviation.TabIndex = 59;
            this.ODeviation.Text = "uiIntegerUpDown2";
            this.ODeviation.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.ODeviation.Value = 1;
            // 
            // uiLabel18
            // 
            this.uiLabel18.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel18.Location = new System.Drawing.Point(248, 353);
            this.uiLabel18.Name = "uiLabel18";
            this.uiLabel18.Size = new System.Drawing.Size(92, 29);
            this.uiLabel18.TabIndex = 60;
            this.uiLabel18.Text = "外偏移";
            this.uiLabel18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // IDeviation
            // 
            this.IDeviation.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.IDeviation.Location = new System.Drawing.Point(343, 314);
            this.IDeviation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.IDeviation.Maximum = 18;
            this.IDeviation.Minimum = 0;
            this.IDeviation.MinimumSize = new System.Drawing.Size(100, 0);
            this.IDeviation.Name = "IDeviation";
            this.IDeviation.ShowText = false;
            this.IDeviation.Size = new System.Drawing.Size(109, 29);
            this.IDeviation.TabIndex = 59;
            this.IDeviation.Text = "uiIntegerUpDown2";
            this.IDeviation.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.IDeviation.Value = 1;
            // 
            // uiLabel17
            // 
            this.uiLabel17.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel17.Location = new System.Drawing.Point(248, 314);
            this.uiLabel17.Name = "uiLabel17";
            this.uiLabel17.Size = new System.Drawing.Size(92, 29);
            this.uiLabel17.TabIndex = 60;
            this.uiLabel17.Text = "内偏移";
            this.uiLabel17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // OutOffBlowTime
            // 
            this.OutOffBlowTime.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OutOffBlowTime.Location = new System.Drawing.Point(343, 268);
            this.OutOffBlowTime.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.OutOffBlowTime.Maximum = 15;
            this.OutOffBlowTime.Minimum = 1;
            this.OutOffBlowTime.MinimumSize = new System.Drawing.Size(100, 0);
            this.OutOffBlowTime.Name = "OutOffBlowTime";
            this.OutOffBlowTime.ShowText = false;
            this.OutOffBlowTime.Size = new System.Drawing.Size(109, 29);
            this.OutOffBlowTime.TabIndex = 55;
            this.OutOffBlowTime.Text = "uiIntegerUpDown2";
            this.OutOffBlowTime.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.OutOffBlowTime.Value = 11;
            // 
            // RemoveSet
            // 
            this.RemoveSet.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RemoveSet.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.RemoveSet.Location = new System.Drawing.Point(311, 388);
            this.RemoveSet.MinimumSize = new System.Drawing.Size(1, 1);
            this.RemoveSet.Name = "RemoveSet";
            this.RemoveSet.Size = new System.Drawing.Size(109, 34);
            this.RemoveSet.TabIndex = 53;
            this.RemoveSet.Text = "设置";
            this.RemoveSet.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.RemoveSet.Click += new System.EventHandler(this.RemoveSet_Click);
            // 
            // uiLabel13
            // 
            this.uiLabel13.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel13.Location = new System.Drawing.Point(248, 268);
            this.uiLabel13.Name = "uiLabel13";
            this.uiLabel13.Size = new System.Drawing.Size(92, 29);
            this.uiLabel13.TabIndex = 58;
            this.uiLabel13.Text = "外关气时间";
            this.uiLabel13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // OutsideStep3
            // 
            this.OutsideStep3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OutsideStep3.Location = new System.Drawing.Point(118, 393);
            this.OutsideStep3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.OutsideStep3.Maximum = 100;
            this.OutsideStep3.Minimum = 0;
            this.OutsideStep3.MinimumSize = new System.Drawing.Size(100, 0);
            this.OutsideStep3.Name = "OutsideStep3";
            this.OutsideStep3.ShowText = false;
            this.OutsideStep3.Size = new System.Drawing.Size(109, 29);
            this.OutsideStep3.TabIndex = 0;
            this.OutsideStep3.Text = "uiIntegerUpDown2";
            this.OutsideStep3.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.OutsideStep3.Value = 30;
            // 
            // OutBlowTime
            // 
            this.OutBlowTime.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OutBlowTime.Location = new System.Drawing.Point(343, 199);
            this.OutBlowTime.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.OutBlowTime.Maximum = 15;
            this.OutBlowTime.Minimum = 1;
            this.OutBlowTime.MinimumSize = new System.Drawing.Size(100, 0);
            this.OutBlowTime.Name = "OutBlowTime";
            this.OutBlowTime.RadiusSides = Sunny.UI.UICornerRadiusSides.None;
            this.OutBlowTime.ShowText = false;
            this.OutBlowTime.Size = new System.Drawing.Size(109, 29);
            this.OutBlowTime.TabIndex = 56;
            this.OutBlowTime.Text = "uiIntegerUpDown2";
            this.OutBlowTime.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.OutBlowTime.Value = 9;
            // 
            // uiLabel12
            // 
            this.uiLabel12.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel12.Location = new System.Drawing.Point(9, 393);
            this.uiLabel12.Name = "uiLabel12";
            this.uiLabel12.Size = new System.Drawing.Size(92, 29);
            this.uiLabel12.TabIndex = 52;
            this.uiLabel12.Text = "外3剔除步数";
            this.uiLabel12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel14
            // 
            this.uiLabel14.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel14.Location = new System.Drawing.Point(248, 199);
            this.uiLabel14.Name = "uiLabel14";
            this.uiLabel14.Size = new System.Drawing.Size(92, 29);
            this.uiLabel14.TabIndex = 59;
            this.uiLabel14.Text = "外喷气时间";
            this.uiLabel14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // OutsideStep2
            // 
            this.OutsideStep2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OutsideStep2.Location = new System.Drawing.Point(118, 330);
            this.OutsideStep2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.OutsideStep2.Maximum = 100;
            this.OutsideStep2.Minimum = 0;
            this.OutsideStep2.MinimumSize = new System.Drawing.Size(100, 0);
            this.OutsideStep2.Name = "OutsideStep2";
            this.OutsideStep2.ShowText = false;
            this.OutsideStep2.Size = new System.Drawing.Size(109, 29);
            this.OutsideStep2.TabIndex = 0;
            this.OutsideStep2.Text = "uiIntegerUpDown2";
            this.OutsideStep2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.OutsideStep2.Value = 50;
            // 
            // InOffBlowTime
            // 
            this.InOffBlowTime.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.InOffBlowTime.Location = new System.Drawing.Point(343, 124);
            this.InOffBlowTime.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.InOffBlowTime.Maximum = 17;
            this.InOffBlowTime.Minimum = 1;
            this.InOffBlowTime.MinimumSize = new System.Drawing.Size(100, 0);
            this.InOffBlowTime.Name = "InOffBlowTime";
            this.InOffBlowTime.ShowText = false;
            this.InOffBlowTime.Size = new System.Drawing.Size(109, 29);
            this.InOffBlowTime.TabIndex = 57;
            this.InOffBlowTime.Text = "uiIntegerUpDown9";
            this.InOffBlowTime.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.InOffBlowTime.Value = 2;
            // 
            // uiLabel11
            // 
            this.uiLabel11.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel11.Location = new System.Drawing.Point(9, 330);
            this.uiLabel11.Name = "uiLabel11";
            this.uiLabel11.Size = new System.Drawing.Size(92, 29);
            this.uiLabel11.TabIndex = 52;
            this.uiLabel11.Text = "外2剔除步数";
            this.uiLabel11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel15
            // 
            this.uiLabel15.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel15.Location = new System.Drawing.Point(248, 124);
            this.uiLabel15.Name = "uiLabel15";
            this.uiLabel15.Size = new System.Drawing.Size(92, 29);
            this.uiLabel15.TabIndex = 60;
            this.uiLabel15.Text = "内关气时间";
            this.uiLabel15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // OutsideStep1
            // 
            this.OutsideStep1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OutsideStep1.Location = new System.Drawing.Point(118, 268);
            this.OutsideStep1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.OutsideStep1.Maximum = 100;
            this.OutsideStep1.Minimum = 0;
            this.OutsideStep1.MinimumSize = new System.Drawing.Size(100, 0);
            this.OutsideStep1.Name = "OutsideStep1";
            this.OutsideStep1.ShowText = false;
            this.OutsideStep1.Size = new System.Drawing.Size(109, 29);
            this.OutsideStep1.TabIndex = 0;
            this.OutsideStep1.Text = "uiIntegerUpDown2";
            this.OutsideStep1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.OutsideStep1.Value = 72;
            // 
            // InBlowTime
            // 
            this.InBlowTime.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.InBlowTime.Location = new System.Drawing.Point(343, 60);
            this.InBlowTime.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.InBlowTime.Maximum = 17;
            this.InBlowTime.Minimum = 1;
            this.InBlowTime.MinimumSize = new System.Drawing.Size(100, 0);
            this.InBlowTime.Name = "InBlowTime";
            this.InBlowTime.ShowText = false;
            this.InBlowTime.Size = new System.Drawing.Size(109, 29);
            this.InBlowTime.TabIndex = 53;
            this.InBlowTime.Text = "uiIntegerUpDown2";
            this.InBlowTime.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.InBlowTime.Value = 1;
            // 
            // uiLabel16
            // 
            this.uiLabel16.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel16.Location = new System.Drawing.Point(248, 60);
            this.uiLabel16.Name = "uiLabel16";
            this.uiLabel16.Size = new System.Drawing.Size(92, 29);
            this.uiLabel16.TabIndex = 54;
            this.uiLabel16.Text = "内喷气时间";
            this.uiLabel16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel10
            // 
            this.uiLabel10.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel10.Location = new System.Drawing.Point(9, 268);
            this.uiLabel10.Name = "uiLabel10";
            this.uiLabel10.Size = new System.Drawing.Size(92, 29);
            this.uiLabel10.TabIndex = 52;
            this.uiLabel10.Text = "外1剔除步数";
            this.uiLabel10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // InsideStep3
            // 
            this.InsideStep3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.InsideStep3.Location = new System.Drawing.Point(118, 199);
            this.InsideStep3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.InsideStep3.Maximum = 100;
            this.InsideStep3.Minimum = 0;
            this.InsideStep3.MinimumSize = new System.Drawing.Size(100, 0);
            this.InsideStep3.Name = "InsideStep3";
            this.InsideStep3.ShowText = false;
            this.InsideStep3.Size = new System.Drawing.Size(109, 29);
            this.InsideStep3.TabIndex = 0;
            this.InsideStep3.Text = "uiIntegerUpDown2";
            this.InsideStep3.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.InsideStep3.Value = 20;
            // 
            // uiLabel9
            // 
            this.uiLabel9.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel9.Location = new System.Drawing.Point(9, 199);
            this.uiLabel9.Name = "uiLabel9";
            this.uiLabel9.Size = new System.Drawing.Size(92, 29);
            this.uiLabel9.TabIndex = 52;
            this.uiLabel9.Text = "内3剔除步数";
            this.uiLabel9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // InsideStep2
            // 
            this.InsideStep2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.InsideStep2.Location = new System.Drawing.Point(118, 124);
            this.InsideStep2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.InsideStep2.Maximum = 100;
            this.InsideStep2.Minimum = 0;
            this.InsideStep2.MinimumSize = new System.Drawing.Size(100, 0);
            this.InsideStep2.Name = "InsideStep2";
            this.InsideStep2.ShowText = false;
            this.InsideStep2.Size = new System.Drawing.Size(109, 29);
            this.InsideStep2.TabIndex = 0;
            this.InsideStep2.Text = "uiIntegerUpDown2";
            this.InsideStep2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.InsideStep2.Value = 55;
            // 
            // uiLabel8
            // 
            this.uiLabel8.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel8.Location = new System.Drawing.Point(9, 124);
            this.uiLabel8.Name = "uiLabel8";
            this.uiLabel8.Size = new System.Drawing.Size(92, 29);
            this.uiLabel8.TabIndex = 52;
            this.uiLabel8.Text = "内2剔除步数";
            this.uiLabel8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // InsideStep1
            // 
            this.InsideStep1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.InsideStep1.Location = new System.Drawing.Point(118, 60);
            this.InsideStep1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.InsideStep1.Maximum = 100;
            this.InsideStep1.Minimum = 0;
            this.InsideStep1.MinimumSize = new System.Drawing.Size(100, 0);
            this.InsideStep1.Name = "InsideStep1";
            this.InsideStep1.ShowText = false;
            this.InsideStep1.Size = new System.Drawing.Size(109, 29);
            this.InsideStep1.TabIndex = 44;
            this.InsideStep1.Text = "uiIntegerUpDown2";
            this.InsideStep1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.InsideStep1.Value = 65;
            // 
            // uiLabel7
            // 
            this.uiLabel7.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel7.Location = new System.Drawing.Point(9, 60);
            this.uiLabel7.Name = "uiLabel7";
            this.uiLabel7.Size = new System.Drawing.Size(92, 29);
            this.uiLabel7.TabIndex = 50;
            this.uiLabel7.Text = "内1剔除步数";
            this.uiLabel7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // WorkGroupBox
            // 
            this.WorkGroupBox.Controls.Add(this.StirRotation);
            this.WorkGroupBox.Controls.Add(this.Turn2Rotation);
            this.WorkGroupBox.Controls.Add(this.Turn1Rotation);
            this.WorkGroupBox.Controls.Add(this.CutRotation);
            this.WorkGroupBox.Controls.Add(this.MainRotation);
            this.WorkGroupBox.Controls.Add(this.StirStart);
            this.WorkGroupBox.Controls.Add(this.StirUpDown);
            this.WorkGroupBox.Controls.Add(this.uiLabel6);
            this.WorkGroupBox.Controls.Add(this.Turn2Start);
            this.WorkGroupBox.Controls.Add(this.Turn1Start);
            this.WorkGroupBox.Controls.Add(this.CutStart);
            this.WorkGroupBox.Controls.Add(this.MainStart);
            this.WorkGroupBox.Controls.Add(this.TurnUpDown2);
            this.WorkGroupBox.Controls.Add(this.uiLabel5);
            this.WorkGroupBox.Controls.Add(this.MainUpDown);
            this.WorkGroupBox.Controls.Add(this.TurnUpDown1);
            this.WorkGroupBox.Controls.Add(this.CutUpDown);
            this.WorkGroupBox.Controls.Add(this.uiLabel4);
            this.WorkGroupBox.Controls.Add(this.uiLabel3);
            this.WorkGroupBox.Controls.Add(this.uiLabel2);
            this.WorkGroupBox.Controls.Add(this.SaveABButton);
            this.WorkGroupBox.Controls.Add(this.CleanFlow);
            this.WorkGroupBox.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.WorkGroupBox.Location = new System.Drawing.Point(4, 0);
            this.WorkGroupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.WorkGroupBox.MinimumSize = new System.Drawing.Size(1, 1);
            this.WorkGroupBox.Name = "WorkGroupBox";
            this.WorkGroupBox.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.WorkGroupBox.Size = new System.Drawing.Size(472, 345);
            this.WorkGroupBox.TabIndex = 23;
            this.WorkGroupBox.Text = "速度设置";
            this.WorkGroupBox.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // StirRotation
            // 
            this.StirRotation.Cursor = System.Windows.Forms.Cursors.Hand;
            this.StirRotation.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.StirRotation.Location = new System.Drawing.Point(225, 248);
            this.StirRotation.MinimumSize = new System.Drawing.Size(1, 1);
            this.StirRotation.Name = "StirRotation";
            this.StirRotation.Size = new System.Drawing.Size(100, 29);
            this.StirRotation.TabIndex = 49;
            this.StirRotation.Text = "正转";
            this.StirRotation.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.StirRotation.Click += new System.EventHandler(this.StirRotation_Click);
            // 
            // Turn2Rotation
            // 
            this.Turn2Rotation.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Turn2Rotation.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Turn2Rotation.Location = new System.Drawing.Point(226, 193);
            this.Turn2Rotation.MinimumSize = new System.Drawing.Size(1, 1);
            this.Turn2Rotation.Name = "Turn2Rotation";
            this.Turn2Rotation.Size = new System.Drawing.Size(100, 29);
            this.Turn2Rotation.TabIndex = 48;
            this.Turn2Rotation.Text = "正转";
            this.Turn2Rotation.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Turn2Rotation.Click += new System.EventHandler(this.Turn2Rotation_Click);
            // 
            // Turn1Rotation
            // 
            this.Turn1Rotation.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Turn1Rotation.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Turn1Rotation.Location = new System.Drawing.Point(226, 142);
            this.Turn1Rotation.MinimumSize = new System.Drawing.Size(1, 1);
            this.Turn1Rotation.Name = "Turn1Rotation";
            this.Turn1Rotation.Size = new System.Drawing.Size(100, 29);
            this.Turn1Rotation.TabIndex = 47;
            this.Turn1Rotation.Text = "正转";
            this.Turn1Rotation.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Turn1Rotation.Click += new System.EventHandler(this.Turn1Rotation_Click);
            // 
            // CutRotation
            // 
            this.CutRotation.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CutRotation.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CutRotation.Location = new System.Drawing.Point(226, 92);
            this.CutRotation.MinimumSize = new System.Drawing.Size(1, 1);
            this.CutRotation.Name = "CutRotation";
            this.CutRotation.Size = new System.Drawing.Size(100, 29);
            this.CutRotation.TabIndex = 46;
            this.CutRotation.Text = "正转";
            this.CutRotation.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CutRotation.Click += new System.EventHandler(this.CutRotation_Click);
            // 
            // MainRotation
            // 
            this.MainRotation.Cursor = System.Windows.Forms.Cursors.Hand;
            this.MainRotation.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MainRotation.Location = new System.Drawing.Point(226, 45);
            this.MainRotation.MinimumSize = new System.Drawing.Size(1, 1);
            this.MainRotation.Name = "MainRotation";
            this.MainRotation.Size = new System.Drawing.Size(100, 29);
            this.MainRotation.TabIndex = 45;
            this.MainRotation.Text = "正转";
            this.MainRotation.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MainRotation.Click += new System.EventHandler(this.MainRotation_Click);
            // 
            // StirStart
            // 
            this.StirStart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.StirStart.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.StirStart.Location = new System.Drawing.Point(346, 248);
            this.StirStart.MinimumSize = new System.Drawing.Size(1, 1);
            this.StirStart.Name = "StirStart";
            this.StirStart.Size = new System.Drawing.Size(100, 29);
            this.StirStart.TabIndex = 44;
            this.StirStart.Text = "启动";
            this.StirStart.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.StirStart.Click += new System.EventHandler(this.StirStart_Click);
            // 
            // StirUpDown
            // 
            this.StirUpDown.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.StirUpDown.Location = new System.Drawing.Point(91, 248);
            this.StirUpDown.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.StirUpDown.Maximum = 200;
            this.StirUpDown.Minimum = 0;
            this.StirUpDown.MinimumSize = new System.Drawing.Size(100, 0);
            this.StirUpDown.Name = "StirUpDown";
            this.StirUpDown.ShowText = false;
            this.StirUpDown.Size = new System.Drawing.Size(109, 29);
            this.StirUpDown.TabIndex = 43;
            this.StirUpDown.Text = "uiIntegerUpDown2";
            this.StirUpDown.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiLabel6
            // 
            this.uiLabel6.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel6.Location = new System.Drawing.Point(9, 258);
            this.uiLabel6.Name = "uiLabel6";
            this.uiLabel6.Size = new System.Drawing.Size(92, 18);
            this.uiLabel6.TabIndex = 42;
            this.uiLabel6.Text = "搅动电机";
            this.uiLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Turn2Start
            // 
            this.Turn2Start.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Turn2Start.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Turn2Start.Location = new System.Drawing.Point(347, 193);
            this.Turn2Start.MinimumSize = new System.Drawing.Size(1, 1);
            this.Turn2Start.Name = "Turn2Start";
            this.Turn2Start.Size = new System.Drawing.Size(100, 29);
            this.Turn2Start.TabIndex = 41;
            this.Turn2Start.Text = "启动";
            this.Turn2Start.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Turn2Start.Click += new System.EventHandler(this.Turn2Start_Click);
            // 
            // Turn1Start
            // 
            this.Turn1Start.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Turn1Start.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Turn1Start.Location = new System.Drawing.Point(347, 142);
            this.Turn1Start.MinimumSize = new System.Drawing.Size(1, 1);
            this.Turn1Start.Name = "Turn1Start";
            this.Turn1Start.Size = new System.Drawing.Size(100, 29);
            this.Turn1Start.TabIndex = 40;
            this.Turn1Start.Text = "启动";
            this.Turn1Start.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Turn1Start.Click += new System.EventHandler(this.Turn1Start_Click);
            // 
            // CutStart
            // 
            this.CutStart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CutStart.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CutStart.Location = new System.Drawing.Point(347, 92);
            this.CutStart.MinimumSize = new System.Drawing.Size(1, 1);
            this.CutStart.Name = "CutStart";
            this.CutStart.Size = new System.Drawing.Size(100, 29);
            this.CutStart.TabIndex = 39;
            this.CutStart.Text = "启动";
            this.CutStart.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CutStart.Click += new System.EventHandler(this.CutStart_Click);
            // 
            // MainStart
            // 
            this.MainStart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.MainStart.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MainStart.Location = new System.Drawing.Point(347, 45);
            this.MainStart.MinimumSize = new System.Drawing.Size(1, 1);
            this.MainStart.Name = "MainStart";
            this.MainStart.Size = new System.Drawing.Size(100, 29);
            this.MainStart.TabIndex = 38;
            this.MainStart.Text = "启动";
            this.MainStart.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MainStart.Click += new System.EventHandler(this.MainStart_Click);
            // 
            // TurnUpDown2
            // 
            this.TurnUpDown2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TurnUpDown2.Location = new System.Drawing.Point(91, 193);
            this.TurnUpDown2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TurnUpDown2.Maximum = 100;
            this.TurnUpDown2.Minimum = 0;
            this.TurnUpDown2.MinimumSize = new System.Drawing.Size(100, 0);
            this.TurnUpDown2.Name = "TurnUpDown2";
            this.TurnUpDown2.ShowText = false;
            this.TurnUpDown2.Size = new System.Drawing.Size(109, 29);
            this.TurnUpDown2.TabIndex = 37;
            this.TurnUpDown2.Text = "uiIntegerUpDown2";
            this.TurnUpDown2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiLabel5
            // 
            this.uiLabel5.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel5.Location = new System.Drawing.Point(9, 203);
            this.uiLabel5.Name = "uiLabel5";
            this.uiLabel5.Size = new System.Drawing.Size(92, 18);
            this.uiLabel5.TabIndex = 36;
            this.uiLabel5.Text = "拨珠电机2  ";
            this.uiLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MainUpDown
            // 
            this.MainUpDown.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MainUpDown.Location = new System.Drawing.Point(91, 45);
            this.MainUpDown.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MainUpDown.Maximum = 200;
            this.MainUpDown.Minimum = 0;
            this.MainUpDown.MinimumSize = new System.Drawing.Size(100, 0);
            this.MainUpDown.Name = "MainUpDown";
            this.MainUpDown.ShowText = false;
            this.MainUpDown.Size = new System.Drawing.Size(109, 29);
            this.MainUpDown.TabIndex = 35;
            this.MainUpDown.Text = "uiIntegerUpDown1";
            this.MainUpDown.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TurnUpDown1
            // 
            this.TurnUpDown1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TurnUpDown1.Location = new System.Drawing.Point(91, 142);
            this.TurnUpDown1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TurnUpDown1.Maximum = 100;
            this.TurnUpDown1.Minimum = 0;
            this.TurnUpDown1.MinimumSize = new System.Drawing.Size(100, 0);
            this.TurnUpDown1.Name = "TurnUpDown1";
            this.TurnUpDown1.ShowText = false;
            this.TurnUpDown1.Size = new System.Drawing.Size(109, 29);
            this.TurnUpDown1.TabIndex = 35;
            this.TurnUpDown1.Text = "uiIntegerUpDown2";
            this.TurnUpDown1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CutUpDown
            // 
            this.CutUpDown.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CutUpDown.Location = new System.Drawing.Point(91, 91);
            this.CutUpDown.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CutUpDown.Maximum = 100;
            this.CutUpDown.Minimum = 0;
            this.CutUpDown.MinimumSize = new System.Drawing.Size(100, 0);
            this.CutUpDown.Name = "CutUpDown";
            this.CutUpDown.ShowText = false;
            this.CutUpDown.Size = new System.Drawing.Size(109, 29);
            this.CutUpDown.TabIndex = 34;
            this.CutUpDown.Text = "uiIntegerUpDown1";
            this.CutUpDown.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiLabel4
            // 
            this.uiLabel4.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel4.Location = new System.Drawing.Point(8, 152);
            this.uiLabel4.Name = "uiLabel4";
            this.uiLabel4.Size = new System.Drawing.Size(92, 18);
            this.uiLabel4.TabIndex = 33;
            this.uiLabel4.Text = "拨珠电机1  ";
            this.uiLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel3
            // 
            this.uiLabel3.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel3.Location = new System.Drawing.Point(8, 90);
            this.uiLabel3.Name = "uiLabel3";
            this.uiLabel3.Size = new System.Drawing.Size(105, 28);
            this.uiLabel3.TabIndex = 32;
            this.uiLabel3.Text = "皮带电机";
            this.uiLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel2
            // 
            this.uiLabel2.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel2.Location = new System.Drawing.Point(8, 45);
            this.uiLabel2.Name = "uiLabel2";
            this.uiLabel2.Size = new System.Drawing.Size(76, 28);
            this.uiLabel2.TabIndex = 30;
            this.uiLabel2.Text = "主电机";
            this.uiLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SaveABButton
            // 
            this.SaveABButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SaveABButton.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SaveABButton.Location = new System.Drawing.Point(91, 295);
            this.SaveABButton.MinimumSize = new System.Drawing.Size(1, 1);
            this.SaveABButton.Name = "SaveABButton";
            this.SaveABButton.Size = new System.Drawing.Size(109, 34);
            this.SaveABButton.TabIndex = 22;
            this.SaveABButton.Text = "设置";
            this.SaveABButton.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SaveABButton.Click += new System.EventHandler(this.SaveABButton_Click);
            // 
            // CleanFlow
            // 
            this.CleanFlow.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CleanFlow.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CleanFlow.Location = new System.Drawing.Point(302, 290);
            this.CleanFlow.MinimumSize = new System.Drawing.Size(1, 1);
            this.CleanFlow.Name = "CleanFlow";
            this.CleanFlow.Size = new System.Drawing.Size(108, 39);
            this.CleanFlow.TabIndex = 22;
            this.CleanFlow.Text = "清洁吹气";
            this.CleanFlow.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CleanFlow.Click += new System.EventHandler(this.CleanFlow_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.uiGroupBox5);
            this.tabPage3.Controls.Add(this.uiGroupBox3);
            this.tabPage3.Location = new System.Drawing.Point(0, 40);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(200, 60);
            this.tabPage3.TabIndex = 5;
            this.tabPage3.Text = "检测设置";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // uiGroupBox5
            // 
            this.uiGroupBox5.Controls.Add(this.uiPanel4);
            this.uiGroupBox5.Controls.Add(this.uiProcessBar1);
            this.uiGroupBox5.Controls.Add(this.uiButton5);
            this.uiGroupBox5.Controls.Add(this.uiButton4);
            this.uiGroupBox5.Controls.Add(this.cal_step1);
            this.uiGroupBox5.Controls.Add(this.uiButton3);
            this.uiGroupBox5.Controls.Add(this.uiButton2);
            this.uiGroupBox5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox5.Location = new System.Drawing.Point(1, 371);
            this.uiGroupBox5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox5.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox5.Name = "uiGroupBox5";
            this.uiGroupBox5.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox5.Size = new System.Drawing.Size(465, 401);
            this.uiGroupBox5.TabIndex = 25;
            this.uiGroupBox5.Text = "校准及数据";
            this.uiGroupBox5.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiPanel4
            // 
            this.uiPanel4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiPanel4.Location = new System.Drawing.Point(26, 152);
            this.uiPanel4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel4.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel4.Name = "uiPanel4";
            this.uiPanel4.Size = new System.Drawing.Size(249, 202);
            this.uiPanel4.TabIndex = 59;
            this.uiPanel4.Text = null;
            this.uiPanel4.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiProcessBar1
            // 
            this.uiProcessBar1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.uiProcessBar1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiProcessBar1.Location = new System.Drawing.Point(27, 360);
            this.uiProcessBar1.MinimumSize = new System.Drawing.Size(3, 3);
            this.uiProcessBar1.Name = "uiProcessBar1";
            this.uiProcessBar1.Size = new System.Drawing.Size(408, 29);
            this.uiProcessBar1.TabIndex = 58;
            this.uiProcessBar1.Text = "uiProcessBar1";
            // 
            // uiButton5
            // 
            this.uiButton5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton5.Location = new System.Drawing.Point(317, 297);
            this.uiButton5.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton5.Name = "uiButton5";
            this.uiButton5.Size = new System.Drawing.Size(100, 35);
            this.uiButton5.TabIndex = 57;
            this.uiButton5.Text = "复位";
            this.uiButton5.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton5.Click += new System.EventHandler(this.uiButton5_Click);
            // 
            // uiButton4
            // 
            this.uiButton4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton4.Location = new System.Drawing.Point(317, 182);
            this.uiButton4.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton4.Name = "uiButton4";
            this.uiButton4.Size = new System.Drawing.Size(100, 35);
            this.uiButton4.TabIndex = 56;
            this.uiButton4.Text = "执行";
            this.uiButton4.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton4.Click += new System.EventHandler(this.uiButton4_Click);
            // 
            // cal_step1
            // 
            this.cal_step1.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cal_step1.Items.AddRange(new object[] {
            "信号校准",
            "数据校准",
            "参数校准",
            "尺寸颜色"});
            this.cal_step1.ItemWidth = 124;
            this.cal_step1.Location = new System.Drawing.Point(15, 110);
            this.cal_step1.MinimumSize = new System.Drawing.Size(1, 1);
            this.cal_step1.Name = "cal_step1";
            this.cal_step1.Size = new System.Drawing.Size(435, 34);
            this.cal_step1.TabIndex = 44;
            this.cal_step1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cal_step1.UnSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.cal_step1.ItemIndexChanged += new Sunny.UI.UIBreadcrumb.OnValueChanged(this.cal_step1_ItemIndexChanged);
            // 
            // uiButton3
            // 
            this.uiButton3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton3.Location = new System.Drawing.Point(169, 47);
            this.uiButton3.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton3.Name = "uiButton3";
            this.uiButton3.Size = new System.Drawing.Size(109, 34);
            this.uiButton3.TabIndex = 54;
            this.uiButton3.Text = "校准停止";
            this.uiButton3.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton3.Click += new System.EventHandler(this.uiButton3_Click);
            // 
            // uiButton2
            // 
            this.uiButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton2.Location = new System.Drawing.Point(13, 47);
            this.uiButton2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton2.Name = "uiButton2";
            this.uiButton2.Size = new System.Drawing.Size(109, 34);
            this.uiButton2.TabIndex = 43;
            this.uiButton2.Text = "校准开始";
            this.uiButton2.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton2.Click += new System.EventHandler(this.uiButton2_Click);
            // 
            // uiGroupBox3
            // 
            this.uiGroupBox3.Controls.Add(this.pictureBox1);
            this.uiGroupBox3.Controls.Add(this.uiNumPadTextBox7);
            this.uiGroupBox3.Controls.Add(this.uiLabel57);
            this.uiGroupBox3.Controls.Add(this.uiNumPadTextBox6);
            this.uiGroupBox3.Controls.Add(this.uiLabel56);
            this.uiGroupBox3.Controls.Add(this.uiNumPadTextBox5);
            this.uiGroupBox3.Controls.Add(this.uiLabel55);
            this.uiGroupBox3.Controls.Add(this.uiNumPadTextBox1);
            this.uiGroupBox3.Controls.Add(this.uiLabel54);
            this.uiGroupBox3.Controls.Add(this.uiLabel25);
            this.uiGroupBox3.Controls.Add(this.uiIntegerUpDown1);
            this.uiGroupBox3.Controls.Add(this.uiButton1);
            this.uiGroupBox3.Controls.Add(this.uiLabel24);
            this.uiGroupBox3.Controls.Add(this.uiSwitch1);
            this.uiGroupBox3.Controls.Add(this.uiNumPadTextBox4);
            this.uiGroupBox3.Controls.Add(this.uiNumPadTextBox3);
            this.uiGroupBox3.Controls.Add(this.uiNumPadTextBox2);
            this.uiGroupBox3.Controls.Add(this.uiButton13);
            this.uiGroupBox3.Controls.Add(this.uiButton12);
            this.uiGroupBox3.Controls.Add(this.uiLabel21);
            this.uiGroupBox3.Controls.Add(this.uiLabel19);
            this.uiGroupBox3.Controls.Add(this.uiLabel20);
            this.uiGroupBox3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox3.Location = new System.Drawing.Point(0, 0);
            this.uiGroupBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox3.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox3.Name = "uiGroupBox3";
            this.uiGroupBox3.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox3.Size = new System.Drawing.Size(465, 361);
            this.uiGroupBox3.TabIndex = 24;
            this.uiGroupBox3.Text = "尺寸设置";
            this.uiGroupBox3.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(170, 194);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(61, 50);
            this.pictureBox1.TabIndex = 81;
            this.pictureBox1.TabStop = false;
            // 
            // uiNumPadTextBox7
            // 
            this.uiNumPadTextBox7.DecimalPlaces = 3;
            this.uiNumPadTextBox7.FillColor = System.Drawing.Color.White;
            this.uiNumPadTextBox7.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiNumPadTextBox7.Location = new System.Drawing.Point(309, 238);
            this.uiNumPadTextBox7.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiNumPadTextBox7.MinimumSize = new System.Drawing.Size(63, 0);
            this.uiNumPadTextBox7.Name = "uiNumPadTextBox7";
            this.uiNumPadTextBox7.NumPadType = Sunny.UI.NumPadType.Double;
            this.uiNumPadTextBox7.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.uiNumPadTextBox7.Size = new System.Drawing.Size(116, 31);
            this.uiNumPadTextBox7.SymbolSize = 24;
            this.uiNumPadTextBox7.TabIndex = 80;
            this.uiNumPadTextBox7.Text = "0.0";
            this.uiNumPadTextBox7.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiNumPadTextBox7.Watermark = "";
            // 
            // uiLabel57
            // 
            this.uiLabel57.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel57.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel57.Location = new System.Drawing.Point(269, 243);
            this.uiLabel57.Name = "uiLabel57";
            this.uiLabel57.Size = new System.Drawing.Size(48, 18);
            this.uiLabel57.TabIndex = 79;
            this.uiLabel57.Text = "B:";
            this.uiLabel57.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiNumPadTextBox6
            // 
            this.uiNumPadTextBox6.DecimalPlaces = 3;
            this.uiNumPadTextBox6.FillColor = System.Drawing.Color.White;
            this.uiNumPadTextBox6.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiNumPadTextBox6.Location = new System.Drawing.Point(309, 197);
            this.uiNumPadTextBox6.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiNumPadTextBox6.MinimumSize = new System.Drawing.Size(63, 0);
            this.uiNumPadTextBox6.Name = "uiNumPadTextBox6";
            this.uiNumPadTextBox6.NumPadType = Sunny.UI.NumPadType.Double;
            this.uiNumPadTextBox6.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.uiNumPadTextBox6.Size = new System.Drawing.Size(116, 31);
            this.uiNumPadTextBox6.SymbolSize = 24;
            this.uiNumPadTextBox6.TabIndex = 78;
            this.uiNumPadTextBox6.Text = "0.0";
            this.uiNumPadTextBox6.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiNumPadTextBox6.Watermark = "";
            // 
            // uiLabel56
            // 
            this.uiLabel56.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel56.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel56.Location = new System.Drawing.Point(269, 202);
            this.uiLabel56.Name = "uiLabel56";
            this.uiLabel56.Size = new System.Drawing.Size(48, 18);
            this.uiLabel56.TabIndex = 77;
            this.uiLabel56.Text = "A:";
            this.uiLabel56.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiNumPadTextBox5
            // 
            this.uiNumPadTextBox5.DecimalPlaces = 3;
            this.uiNumPadTextBox5.FillColor = System.Drawing.Color.White;
            this.uiNumPadTextBox5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiNumPadTextBox5.Location = new System.Drawing.Point(309, 156);
            this.uiNumPadTextBox5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiNumPadTextBox5.MinimumSize = new System.Drawing.Size(63, 0);
            this.uiNumPadTextBox5.Name = "uiNumPadTextBox5";
            this.uiNumPadTextBox5.NumPadType = Sunny.UI.NumPadType.Double;
            this.uiNumPadTextBox5.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.uiNumPadTextBox5.Size = new System.Drawing.Size(116, 31);
            this.uiNumPadTextBox5.SymbolSize = 24;
            this.uiNumPadTextBox5.TabIndex = 76;
            this.uiNumPadTextBox5.Text = "100.0";
            this.uiNumPadTextBox5.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiNumPadTextBox5.Watermark = "";
            // 
            // uiLabel55
            // 
            this.uiLabel55.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel55.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel55.Location = new System.Drawing.Point(269, 161);
            this.uiLabel55.Name = "uiLabel55";
            this.uiLabel55.Size = new System.Drawing.Size(48, 18);
            this.uiLabel55.TabIndex = 75;
            this.uiLabel55.Text = "L:";
            this.uiLabel55.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiNumPadTextBox1
            // 
            this.uiNumPadTextBox1.DecimalPlaces = 3;
            this.uiNumPadTextBox1.FillColor = System.Drawing.Color.White;
            this.uiNumPadTextBox1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiNumPadTextBox1.Location = new System.Drawing.Point(117, 155);
            this.uiNumPadTextBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiNumPadTextBox1.MinimumSize = new System.Drawing.Size(63, 0);
            this.uiNumPadTextBox1.Name = "uiNumPadTextBox1";
            this.uiNumPadTextBox1.NumPadType = Sunny.UI.NumPadType.Double;
            this.uiNumPadTextBox1.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.uiNumPadTextBox1.Size = new System.Drawing.Size(116, 31);
            this.uiNumPadTextBox1.SymbolSize = 24;
            this.uiNumPadTextBox1.TabIndex = 74;
            this.uiNumPadTextBox1.Text = "5.0";
            this.uiNumPadTextBox1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiNumPadTextBox1.Watermark = "";
            // 
            // uiLabel54
            // 
            this.uiLabel54.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel54.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel54.Location = new System.Drawing.Point(13, 163);
            this.uiLabel54.Name = "uiLabel54";
            this.uiLabel54.Size = new System.Drawing.Size(92, 18);
            this.uiLabel54.TabIndex = 73;
            this.uiLabel54.Text = "色差阈值:";
            this.uiLabel54.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel25
            // 
            this.uiLabel25.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel25.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel25.Location = new System.Drawing.Point(13, 266);
            this.uiLabel25.Name = "uiLabel25";
            this.uiLabel25.Size = new System.Drawing.Size(62, 18);
            this.uiLabel25.TabIndex = 72;
            this.uiLabel25.Text = "缓存:";
            this.uiLabel25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiIntegerUpDown1
            // 
            this.uiIntegerUpDown1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiIntegerUpDown1.Location = new System.Drawing.Point(82, 255);
            this.uiIntegerUpDown1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiIntegerUpDown1.Maximum = 10;
            this.uiIntegerUpDown1.Minimum = 0;
            this.uiIntegerUpDown1.MinimumSize = new System.Drawing.Size(100, 0);
            this.uiIntegerUpDown1.Name = "uiIntegerUpDown1";
            this.uiIntegerUpDown1.ShowText = false;
            this.uiIntegerUpDown1.Size = new System.Drawing.Size(109, 29);
            this.uiIntegerUpDown1.TabIndex = 71;
            this.uiIntegerUpDown1.Text = "uiIntegerUpDown1";
            this.uiIntegerUpDown1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiIntegerUpDown1.Value = 7;
            // 
            // uiButton1
            // 
            this.uiButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton1.Location = new System.Drawing.Point(211, 313);
            this.uiButton1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton1.Name = "uiButton1";
            this.uiButton1.Size = new System.Drawing.Size(109, 34);
            this.uiButton1.TabIndex = 70;
            this.uiButton1.Text = "打印";
            this.uiButton1.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton1.Click += new System.EventHandler(this.uiButton1_Click);
            // 
            // uiLabel24
            // 
            this.uiLabel24.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel24.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel24.Location = new System.Drawing.Point(11, 322);
            this.uiLabel24.Name = "uiLabel24";
            this.uiLabel24.Size = new System.Drawing.Size(98, 18);
            this.uiLabel24.TabIndex = 69;
            this.uiLabel24.Text = "自动打印:";
            this.uiLabel24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiLabel24.Click += new System.EventHandler(this.uiLabel24_Click);
            // 
            // uiSwitch1
            // 
            this.uiSwitch1.Active = true;
            this.uiSwitch1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiSwitch1.Location = new System.Drawing.Point(127, 322);
            this.uiSwitch1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiSwitch1.Name = "uiSwitch1";
            this.uiSwitch1.Size = new System.Drawing.Size(60, 25);
            this.uiSwitch1.SwitchShape = Sunny.UI.UISwitch.UISwitchShape.Square;
            this.uiSwitch1.TabIndex = 68;
            this.uiSwitch1.Text = "uiSwitch1";
            // 
            // uiNumPadTextBox4
            // 
            this.uiNumPadTextBox4.DecimalPlaces = 3;
            this.uiNumPadTextBox4.FillColor = System.Drawing.Color.White;
            this.uiNumPadTextBox4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiNumPadTextBox4.Location = new System.Drawing.Point(117, 102);
            this.uiNumPadTextBox4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiNumPadTextBox4.MinimumSize = new System.Drawing.Size(63, 0);
            this.uiNumPadTextBox4.Name = "uiNumPadTextBox4";
            this.uiNumPadTextBox4.NumPadType = Sunny.UI.NumPadType.Double;
            this.uiNumPadTextBox4.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.uiNumPadTextBox4.Size = new System.Drawing.Size(116, 31);
            this.uiNumPadTextBox4.SymbolSize = 24;
            this.uiNumPadTextBox4.TabIndex = 66;
            this.uiNumPadTextBox4.Text = "0.020";
            this.uiNumPadTextBox4.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiNumPadTextBox4.Watermark = "";
            // 
            // uiNumPadTextBox3
            // 
            this.uiNumPadTextBox3.DecimalPlaces = 3;
            this.uiNumPadTextBox3.FillColor = System.Drawing.Color.White;
            this.uiNumPadTextBox3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiNumPadTextBox3.Location = new System.Drawing.Point(302, 47);
            this.uiNumPadTextBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiNumPadTextBox3.MinimumSize = new System.Drawing.Size(63, 0);
            this.uiNumPadTextBox3.Name = "uiNumPadTextBox3";
            this.uiNumPadTextBox3.NumPadType = Sunny.UI.NumPadType.Double;
            this.uiNumPadTextBox3.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.uiNumPadTextBox3.Size = new System.Drawing.Size(116, 31);
            this.uiNumPadTextBox3.SymbolSize = 24;
            this.uiNumPadTextBox3.TabIndex = 65;
            this.uiNumPadTextBox3.Text = "0.05";
            this.uiNumPadTextBox3.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiNumPadTextBox3.Watermark = "";
            // 
            // uiNumPadTextBox2
            // 
            this.uiNumPadTextBox2.FillColor = System.Drawing.Color.White;
            this.uiNumPadTextBox2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiNumPadTextBox2.Location = new System.Drawing.Point(117, 47);
            this.uiNumPadTextBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiNumPadTextBox2.MinimumSize = new System.Drawing.Size(63, 0);
            this.uiNumPadTextBox2.Name = "uiNumPadTextBox2";
            this.uiNumPadTextBox2.NumPadType = Sunny.UI.NumPadType.Double;
            this.uiNumPadTextBox2.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.uiNumPadTextBox2.Size = new System.Drawing.Size(116, 31);
            this.uiNumPadTextBox2.SymbolSize = 24;
            this.uiNumPadTextBox2.TabIndex = 64;
            this.uiNumPadTextBox2.Text = "3.00";
            this.uiNumPadTextBox2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiNumPadTextBox2.Watermark = "";
            // 
            // uiButton13
            // 
            this.uiButton13.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton13.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton13.Location = new System.Drawing.Point(309, 102);
            this.uiButton13.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton13.Name = "uiButton13";
            this.uiButton13.Size = new System.Drawing.Size(109, 34);
            this.uiButton13.TabIndex = 43;
            this.uiButton13.Text = "读均值";
            this.uiButton13.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton13.Click += new System.EventHandler(this.uiButton13_Click);
            // 
            // uiButton12
            // 
            this.uiButton12.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton12.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton12.Location = new System.Drawing.Point(353, 313);
            this.uiButton12.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton12.Name = "uiButton12";
            this.uiButton12.Size = new System.Drawing.Size(109, 34);
            this.uiButton12.TabIndex = 42;
            this.uiButton12.Text = "设置";
            this.uiButton12.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton12.Click += new System.EventHandler(this.uiButton12_Click);
            // 
            // uiLabel21
            // 
            this.uiLabel21.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel21.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel21.Location = new System.Drawing.Point(13, 110);
            this.uiLabel21.Name = "uiLabel21";
            this.uiLabel21.Size = new System.Drawing.Size(62, 18);
            this.uiLabel21.TabIndex = 40;
            this.uiLabel21.Text = "圆度:";
            this.uiLabel21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel19
            // 
            this.uiLabel19.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel19.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel19.Location = new System.Drawing.Point(240, 53);
            this.uiLabel19.Name = "uiLabel19";
            this.uiLabel19.Size = new System.Drawing.Size(39, 18);
            this.uiLabel19.TabIndex = 38;
            this.uiLabel19.Text = "±";
            this.uiLabel19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiLabel19.Click += new System.EventHandler(this.uiLabel19_Click);
            // 
            // uiLabel20
            // 
            this.uiLabel20.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel20.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel20.Location = new System.Drawing.Point(13, 53);
            this.uiLabel20.Name = "uiLabel20";
            this.uiLabel20.Size = new System.Drawing.Size(62, 18);
            this.uiLabel20.TabIndex = 36;
            this.uiLabel20.Text = "粒径:";
            this.uiLabel20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.flowLayoutPanel1);
            this.tabPage4.Controls.Add(this.uiDataGridView1);
            this.tabPage4.Controls.Add(this.uiButton11);
            this.tabPage4.Controls.Add(this.uiDatePicker1);
            this.tabPage4.Location = new System.Drawing.Point(0, 40);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(200, 60);
            this.tabPage4.TabIndex = 6;
            this.tabPage4.Text = "数据统计";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Location = new System.Drawing.Point(4, 372);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(462, 402);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // uiDataGridView1
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            this.uiDataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.uiDataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.uiDataGridView1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            this.uiDataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.uiDataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.uiDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(236)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.uiDataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            this.uiDataGridView1.EnableHeadersVisualStyles = false;
            this.uiDataGridView1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiDataGridView1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(173)))), ((int)(((byte)(255)))));
            this.uiDataGridView1.Location = new System.Drawing.Point(4, 49);
            this.uiDataGridView1.Name = "uiDataGridView1";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.uiDataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(236)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiDataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.uiDataGridView1.RowTemplate.Height = 23;
            this.uiDataGridView1.SelectedIndex = -1;
            this.uiDataGridView1.Size = new System.Drawing.Size(462, 317);
            this.uiDataGridView1.TabIndex = 4;
            // 
            // uiButton11
            // 
            this.uiButton11.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton11.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton11.Location = new System.Drawing.Point(186, 8);
            this.uiButton11.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton11.Name = "uiButton11";
            this.uiButton11.Size = new System.Drawing.Size(100, 35);
            this.uiButton11.TabIndex = 1;
            this.uiButton11.Text = "刷新";
            this.uiButton11.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton11.Click += new System.EventHandler(this.uiButton11_Click);
            // 
            // uiDatePicker1
            // 
            this.uiDatePicker1.FillColor = System.Drawing.Color.White;
            this.uiDatePicker1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiDatePicker1.Location = new System.Drawing.Point(4, 10);
            this.uiDatePicker1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiDatePicker1.MaxLength = 10;
            this.uiDatePicker1.MinimumSize = new System.Drawing.Size(63, 0);
            this.uiDatePicker1.Name = "uiDatePicker1";
            this.uiDatePicker1.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.uiDatePicker1.Size = new System.Drawing.Size(150, 29);
            this.uiDatePicker1.SymbolDropDown = 61555;
            this.uiDatePicker1.SymbolNormal = 61555;
            this.uiDatePicker1.SymbolSize = 24;
            this.uiDatePicker1.TabIndex = 3;
            this.uiDatePicker1.Text = "2025-03-10";
            this.uiDatePicker1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.uiDatePicker1.Value = new System.DateTime(2025, 3, 10, 0, 0, 0, 0);
            this.uiDatePicker1.Watermark = "";
            this.uiDatePicker1.ValueChanged += new Sunny.UI.UIDatePicker.OnDateTimeChanged(this.uiDatePicker1_ValueChanged);
            // 
            // uiPanel1
            // 
            this.uiPanel1.Controls.Add(this.uiPanel2);
            this.uiPanel1.Controls.Add(this.uiTabControl1);
            this.uiPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiPanel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiPanel1.Location = new System.Drawing.Point(0, 35);
            this.uiPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(1280, 827);
            this.uiPanel1.TabIndex = 1;
            this.uiPanel1.Text = "uiPanel1";
            this.uiPanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiPanel2
            // 
            this.uiPanel2.Controls.Add(this.uiPanel3);
            this.uiPanel2.Controls.Add(this.uiGroupBox1);
            this.uiPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiPanel2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiPanel2.Location = new System.Drawing.Point(473, 0);
            this.uiPanel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel2.Name = "uiPanel2";
            this.uiPanel2.Size = new System.Drawing.Size(807, 827);
            this.uiPanel2.TabIndex = 2;
            this.uiPanel2.Text = "uiPanel2";
            this.uiPanel2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiPanel3
            // 
            this.uiPanel3.Controls.Add(this.MPanel);
            this.uiPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiPanel3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiPanel3.Location = new System.Drawing.Point(0, 0);
            this.uiPanel3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel3.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel3.Name = "uiPanel3";
            this.uiPanel3.Size = new System.Drawing.Size(807, 665);
            this.uiPanel3.TabIndex = 3;
            this.uiPanel3.Text = "uiPanel3";
            this.uiPanel3.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MPanel
            // 
            this.MPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MPanel.Enabled = false;
            this.MPanel.Location = new System.Drawing.Point(0, 0);
            this.MPanel.Margin = new System.Windows.Forms.Padding(0);
            this.MPanel.Name = "MPanel";
            this.MPanel.Size = new System.Drawing.Size(807, 665);
            this.MPanel.TabIndex = 0;
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.Controls.Add(this.logView1);
            this.uiGroupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.uiGroupBox1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox1.Location = new System.Drawing.Point(0, 665);
            this.uiGroupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox1.Size = new System.Drawing.Size(807, 162);
            this.uiGroupBox1.TabIndex = 2;
            this.uiGroupBox1.Text = "日志";
            this.uiGroupBox1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // logView1
            // 
            this.logView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logView1.Location = new System.Drawing.Point(0, 32);
            this.logView1.Margin = new System.Windows.Forms.Padding(0);
            this.logView1.Name = "logView1";
            this.logView1.Size = new System.Drawing.Size(807, 130);
            this.logView1.TabIndex = 0;
            this.logView1.Load += new System.EventHandler(this.logView1_Load);
            // 
            // MFrm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1280, 862);
            this.Controls.Add(this.uiPanel1);
            this.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.MinimumSize = new System.Drawing.Size(1278, 820);
            this.Name = "MFrm";
            this.Text = "XOP1080 视觉选丸仪";
            this.TitleFont = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ZoomScaleRect = new System.Drawing.Rectangle(19, 19, 800, 450);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MFrm_FormClosing);
            this.Load += new System.EventHandler(this.MFrm_Load);
            this.uiTabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.uiGroupBox4.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.uiGroupBox2.ResumeLayout(false);
            this.WorkGroupBox.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.uiGroupBox5.ResumeLayout(false);
            this.uiGroupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiDataGridView1)).EndInit();
            this.uiPanel1.ResumeLayout(false);
            this.uiPanel2.ResumeLayout(false);
            this.uiPanel3.ResumeLayout(false);
            this.uiGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        public Sunny.UI.UIStyleManager uiStyleManager1;
        private Sunny.UI.UITabControl uiTabControl1;
        private TabPage tabPage2;
        private Sunny.UI.UIButton uiButton14;
        private Sunny.UI.UILedLabel uiLedLabel1;
        private Sunny.UI.UIGroupBox uiGroupBox4;
        private Sunny.UI.UILabel uiLabel66;
        private Sunny.UI.UILabel uiLabel67;
        private Sunny.UI.UILabel uiLabel70;
        private Sunny.UI.UILabel uiLabel71;
        private Sunny.UI.UILabel uiLabel74;
        private Sunny.UI.UILabel uiLabel75;
        private Sunny.UI.UILabel uiLabel50;
        private Sunny.UI.UILabel uiLabel49;
        private Sunny.UI.UILabel uiLabel48;
        private Sunny.UI.UILabel uiLabel47;
        private Sunny.UI.UIButton SaveButton;
        private Sunny.UI.UIButton OPStopButton;
        private Sunny.UI.UIButton ValButton;
        private Sunny.UI.UIButton OpButton;
        private Sunny.UI.UIButton AddSolButton;
        private Sunny.UI.UIButton SolButton;
        private Sunny.UI.UIComboBox SolComboBox;
        private Sunny.UI.UILabel uiLabel1;
        private TabPage tabPage1;
        private Sunny.UI.UIGroupBox uiGroupBox2;
        private Sunny.UI.UIIntegerUpDown ODeviation;
        private Sunny.UI.UILabel uiLabel18;
        private Sunny.UI.UIIntegerUpDown IDeviation;
        private Sunny.UI.UILabel uiLabel17;
        private Sunny.UI.UIIntegerUpDown OutOffBlowTime;
        private Sunny.UI.UIButton RemoveSet;
        private Sunny.UI.UILabel uiLabel13;
        private Sunny.UI.UIIntegerUpDown OutsideStep3;
        private Sunny.UI.UIIntegerUpDown OutBlowTime;
        private Sunny.UI.UILabel uiLabel12;
        private Sunny.UI.UILabel uiLabel14;
        private Sunny.UI.UIIntegerUpDown OutsideStep2;
        private Sunny.UI.UIIntegerUpDown InOffBlowTime;
        private Sunny.UI.UILabel uiLabel11;
        private Sunny.UI.UILabel uiLabel15;
        private Sunny.UI.UIIntegerUpDown OutsideStep1;
        private Sunny.UI.UIIntegerUpDown InBlowTime;
        private Sunny.UI.UILabel uiLabel16;
        private Sunny.UI.UILabel uiLabel10;
        private Sunny.UI.UIIntegerUpDown InsideStep3;
        private Sunny.UI.UILabel uiLabel9;
        private Sunny.UI.UIIntegerUpDown InsideStep2;
        private Sunny.UI.UILabel uiLabel8;
        private Sunny.UI.UIIntegerUpDown InsideStep1;
        private Sunny.UI.UILabel uiLabel7;
        private Sunny.UI.UIGroupBox WorkGroupBox;
        private Sunny.UI.UIButton StirRotation;
        private Sunny.UI.UIButton Turn2Rotation;
        private Sunny.UI.UIButton Turn1Rotation;
        private Sunny.UI.UIButton CutRotation;
        private Sunny.UI.UIButton MainRotation;
        private Sunny.UI.UIButton StirStart;
        private Sunny.UI.UIIntegerUpDown StirUpDown;
        private Sunny.UI.UILabel uiLabel6;
        private Sunny.UI.UIButton Turn2Start;
        private Sunny.UI.UIButton Turn1Start;
        private Sunny.UI.UIButton CutStart;
        private Sunny.UI.UIButton MainStart;
        private Sunny.UI.UIIntegerUpDown TurnUpDown2;
        private Sunny.UI.UILabel uiLabel5;
        private Sunny.UI.UIIntegerUpDown MainUpDown;
        private Sunny.UI.UIIntegerUpDown TurnUpDown1;
        private Sunny.UI.UIIntegerUpDown CutUpDown;
        private Sunny.UI.UILabel uiLabel4;
        private Sunny.UI.UILabel uiLabel3;
        private Sunny.UI.UILabel uiLabel2;
        private Sunny.UI.UIButton SaveABButton;
        private Sunny.UI.UIButton CleanFlow;
        private TabPage tabPage3;
        private Sunny.UI.UIGroupBox uiGroupBox5;
        private Sunny.UI.UIPanel uiPanel4;
        private Sunny.UI.UIProcessBar uiProcessBar1;
        private Sunny.UI.UIButton uiButton5;
        private Sunny.UI.UIButton uiButton4;
        private Sunny.UI.UIBreadcrumb cal_step1;
        private Sunny.UI.UIButton uiButton3;
        private Sunny.UI.UIButton uiButton2;
        private Sunny.UI.UIGroupBox uiGroupBox3;
        private Sunny.UI.UILabel uiLabel25;
        private Sunny.UI.UIIntegerUpDown uiIntegerUpDown1;
        private Sunny.UI.UIButton uiButton1;
        private Sunny.UI.UILabel uiLabel24;
        private Sunny.UI.UISwitch uiSwitch1;
        private Sunny.UI.UINumPadTextBox uiNumPadTextBox4;
        private Sunny.UI.UINumPadTextBox uiNumPadTextBox3;
        private Sunny.UI.UINumPadTextBox uiNumPadTextBox2;
        private Sunny.UI.UIButton uiButton13;
        private Sunny.UI.UIButton uiButton12;
        private Sunny.UI.UILabel uiLabel21;
        private Sunny.UI.UILabel uiLabel19;
        private Sunny.UI.UILabel uiLabel20;
        private Sunny.UI.UIPanel uiPanel1;
        private Sunny.UI.UIPanel uiPanel2;
        private Sunny.UI.UIPanel uiPanel3;
        private ImageControl.ViewPanel MPanel;
        private Sunny.UI.UIGroupBox uiGroupBox1;
        private VisionCore.Component.LogView logView1;
        private Sunny.UI.UILabel uiLabel28;
        private Sunny.UI.UILabel uiLabel27;
        private Sunny.UI.UILabel uiLabel30;
        private Sunny.UI.UILabel uiLabel29;
        private Sunny.UI.UILabel uiLabel39;
        private Sunny.UI.UILabel uiLabel23;
        private Sunny.UI.UILabel uiLabel38;
        private Sunny.UI.UILabel uiLabel22;
        private TabPage tabPage4;
        private Sunny.UI.UIDataGridView uiDataGridView1;
        private Sunny.UI.UIButton uiButton11;
        private Sunny.UI.UIDatePicker uiDatePicker1;
        private Sunny.UI.UILabel uiLabel52;
        private Sunny.UI.UILabel uiLabel46;
        private Sunny.UI.UILabel uiLabel45;
        private Sunny.UI.UILabel uiLabel44;
        private Sunny.UI.UILabel uiLabel43;
        private Sunny.UI.UILabel uiLabel42;
        private Sunny.UI.UILabel uiLabel37;
        private Sunny.UI.UILabel uiLabel36;
        private Sunny.UI.UILabel uiLabel35;
        private Sunny.UI.UILabel uiLabel34;
        private Sunny.UI.UILabel uiLabel33;
        private Sunny.UI.UILabel uiLabel32;
        private Sunny.UI.UILabel uiLabel41;
        private Sunny.UI.UILabel uiLabel31;
        private Sunny.UI.UILabel uiLabel40;
        private Sunny.UI.UILabel uiLabel26;
        private FlowLayoutPanel flowLayoutPanel1;
        private Sunny.UI.UILabel uiLabel53;
        private Sunny.UI.UILabel uiLabel51;
        private Sunny.UI.UINumPadTextBox uiNumPadTextBox1;
        private Sunny.UI.UILabel uiLabel54;
        private Sunny.UI.UINumPadTextBox uiNumPadTextBox7;
        private Sunny.UI.UILabel uiLabel57;
        private Sunny.UI.UINumPadTextBox uiNumPadTextBox6;
        private Sunny.UI.UILabel uiLabel56;
        private Sunny.UI.UINumPadTextBox uiNumPadTextBox5;
        private Sunny.UI.UILabel uiLabel55;
        private Sunny.UI.UIButton uiButton6;
        private PictureBox pictureBox1;
    }
}