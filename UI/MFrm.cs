using HalconDotNet;
using LittleCommon.Domain;
using RobotLocation.Model;
using Sunny.UI;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Vision.Service;
using VisionCore.Communication;
using VisionCore.Core;
using VisionCore.Ext;
using VisionCore.Frm;
using VisionCore.Log;
using VisionCore.Tools;
using System.Data;
using System.Data.SQLite;
using RobotLocation.Service;
using System.Runtime.InteropServices;
using System.Web.UI.WebControls.WebParts;
using LevelDB;
using RobotLocation.Properties;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Reflection;
using System.Linq;
using System.Collections.Concurrent;
using System.Web.UI.WebControls;

namespace RobotLocation.UI
{
    public partial class MFrm : UIForm
    {
        //链接数据库
        private string connectionString = "Data Source=data.db;Version=3;";
        private SQLiteConnection connection;
        public string 当前算法 = ExtHandler.GetAutoLoadSolName();
        public int img1 = 0;// ToneOp.ToneProcesses[0].curOrder;//内一照片数
        public int img2 = 0; //ToneOp.ToneProcesses[1].curOrder;//内一照片数
        public int img3 = 0; //ToneOp.ToneProcesses[2].curOrder;//内一照片数
        public int img4 = 0; //ToneOp.ToneProcesses[3].curOrder;//内一照片数
        public int img5 = 0; //ToneOp.ToneProcesses[4].curOrder;//内一照片数
        public int img6 = 0; //ToneOp.ToneProcesses[5].curOrder;//内一照片数
        public int 校准步数 = 0;

        //定义托尼类型
        public MFrm()
        {
            InitializeComponent();
            InitializeDatabase();
            //this.WindowState = FormWindowState.Maximized;
            //this.ShowFullScreen = true; // 设置全屏显示
            this.ControlBox = false; // 去掉控制框（包括关闭按钮）
            this.Text = "XOP1080 视觉选丸仪-当前牌号:" + 当前算法; // 去掉标题栏文字
            //开机强制关闭下位连锁，防止程序崩溃后残留标记
            var r = EComManageer.GetECommunication("ModbusTcpNet0");
            if (r.status)
            {
                EComManageer.Write<bool>("ModbusTcpNet0", "529", false);
            }
            else
            {
                LogNet.Info("下位连锁异常，无法运行");
            }
            uiDatePicker1.Value = DateTime.Now;
            LoadSettings();
            //全局主题设置
            uiStyleManager1.Style =UIStyle.Blue;
        }
        //数据库初始化函数
        private void InitializeDatabase()
        {
            connection = new SQLiteConnection(connectionString);
            connection.Open();

            // 创建结果表（如果不存在）
            string createTableQuery1 = @"
                CREATE TABLE IF NOT EXISTS 结果 (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                日期 TEXT, 
                OK TEXT,
                NG TEXT, 
                剔除 TEXT, 
                通过 TEXT, 
                合格率 TEXT,
                粒径 TEXT,
                圆度值 TEXT,
                标准差 TEXT,
                皮帽 TEXT,
                脏污 TEXT,
                偏心 TEXT,
                凹陷 TEXT,
                凸点 TEXT,
                气泡 TEXT,
                异型 TEXT,
                空壳 TEXT,
                尺寸 TEXT,
                圆度 TEXT
                );";
            using (SQLiteCommand command = new SQLiteCommand(createTableQuery1, connection))
            {
                command.ExecuteNonQuery();
            }

        }
        
        //数据加载方法
        private void LoadData()
        {
            try
            {
                // 只查询需要的字段
                string query1 = "SELECT ID, 日期, 合格率, 粒径, 圆度值 FROM 结果";
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query1, connection))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count == 0)
                    {
                        LogNet.Error("无历史记录数据");
                    }
                    else
                    {
                        // 将查询结果绑定到 DataGridView
                        uiDataGridView1.DataSource = dataTable;
                        // 设置列标题（如果需要）
                        uiDataGridView1.Columns["ID"].HeaderText = "ID";
                        uiDataGridView1.Columns["日期"].HeaderText = "日期";
                        uiDataGridView1.Columns["合格率"].HeaderText = "合格率";
                        uiDataGridView1.Columns["粒径"].HeaderText = "粒径";
                        uiDataGridView1.Columns["圆度值"].HeaderText = "圆度";
                    }
                    // 添加点击事件
                    uiDataGridView1.CellClick += UiDataGridView1_CellClick;
                }
            }
            catch (Exception ex)
            {
                LogNet.Error("查询数据时出错:"+ ex.Message);
            }
        }
        //单击数据筛选显示
        private void UiDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                UIDataGridView uiDataGridView = sender as UIDataGridView;
                DataGridViewRow row = uiDataGridView.Rows[e.RowIndex];

                // 获取选中行的 ID
                if (row.Cells["ID"].Value != null && int.TryParse(row.Cells["ID"].Value.ToString(), out int id))
                {
                    // 根据 ID 查询数据库并显示全部数据
                    DisplayFullData(id);
                }
                else
                {
                    LogNet.Error("无法获取有效的 ID");
                }
            }
        }
        private void DisplayFullData(int id)
        {
            try
            {
                string query = "SELECT * FROM 结果 WHERE ID = @Id";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.Add(new SQLiteParameter("@Id", DbType.Int32) { Value = id });
                    //connection.Open();
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // 清空面板
                            flowLayoutPanel1.Controls.Clear();

                            // 创建 Label 来显示每个字段的数据
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string fieldName = reader.GetName(i);
                                object value = reader.GetValue(i);

                                // 创建 Label
                                System.Windows.Forms.Label label = new System.Windows.Forms.Label();
                                label.Text = $"{fieldName}: {value}";
                                label.AutoSize = true;
                                label.Margin = new Padding(10, 5, 10, 5);
                                label.Font = new System.Drawing.Font("Microsoft YaHei", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                                label.ForeColor = System.Drawing.Color.FromArgb(40, 40, 40);

                                // 添加到面板
                                flowLayoutPanel1.Controls.Add(label);
                            }
                        }
                        else
                        {
                            flowLayoutPanel1.Controls.Clear();
                            System.Windows.Forms.Label label = new  System.Windows.Forms.Label();
                            label.Text = "未找到数据";
                            label.AutoSize = true;
                            label.Margin = new Padding(10, 5, 10, 5);
                            flowLayoutPanel1.Controls.Add(label);
                        }
                    }
                    //connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"查询数据时出错: {ex.Message}");
            }
        }
        //  跟新UI显示
        private void UpdateUI(string status, string id)
        {
            // 根据状态改变UI显示
            //Sunny.UI.UISwitch controlName = new UISwitch;
            string controlName = $"uiSwitch{id}";
            UISwitch sw = tabPage3.Controls[controlName] as UISwitch;
            if (status == "是")
            {
                sw.Active = true;
            }
            else if (status == "否")
            {
                sw.Active = false;
            }
            else
            {
                //空数据处理
            }
        }

        //数据查询按钮
        private void uiButton11_Click(object sender, EventArgs e)
        {
            LoadData(); // 刷新数据
        }

        #region 型号管理
        private void SolComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string SolName = (string)SolComboBox.SelectedItem;
            if (!string.IsNullOrEmpty(SolName))
            {
                ExtHandler.Load(SolName, new Action<bool>(r =>
                {
                    Invoke(new Action(() =>
                    {

                        if (!r)
                        {
                            SolComboBox.SelectedItem = null;
                            this.ShowErrorDialog("型号加载失败");
                        }
                        else
                        {
                            this.Text = "XOP1080 视觉选丸仪-当前牌号:" + 当前算法; // 去掉标题栏文字
                        }
                    }));
                }));
            }
        }
        private void AddSolButton_Click(object sender, System.EventArgs e)
        {
            ExtHandler.AddSol(new Action<bool>(r =>
            {
                if (r)
                {
                    List<string> Sols = ExtHandler.GetSols();
                    //
                    Invoke(new Action(() =>
                    {
                        SolComboBox.Items.Clear();
                        foreach (string sol in Sols)
                        {
                            if (sol == ExtHandler.GetAutoLoadSolName())
                            {
                                r = true;
                            }
                            SolComboBox.Items.Add(sol);
                        }
                    }));
                }
            }));
        }
        #endregion

        #region 调试
        private void SolButton_Click(object sender, System.EventArgs e)
        {
            ExtHandler.ShowExFrm();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveButton.Enabled = false;
            ExtHandler.SaveSol();
            SaveButton.Enabled = true;
        }
        private void ValButton_Click(object sender, System.EventArgs e)
        {
            FrmTool.ShowDialog<CommonSetFrm>();
        }
        #endregion

        #region 初始化
        private void InitVibrating()
        {
            Invoke(new Action(() =>
            {
                Config.LoadConfig();
                Config.SetCanvas(MPanel);
                logView1.Start();
            }));
        }
        private void AutoLoad()
        {
            Invoke(new Action(() =>
            {
                List<string> Sols = ExtHandler.GetSols();
                SolComboBox.Items.Clear();
                bool r = false;
                foreach (string sol in Sols)
                {
                    if (sol == ExtHandler.GetAutoLoadSolName())
                    {
                        r = true;
                    }
                    SolComboBox.Items.Add(sol);
                }
                if (r)
                {
                    SolComboBox.SelectedItem = ExtHandler.GetAutoLoadSolName();
                }
                else
                {
                    SolComboBox.SelectedItem = null;
                }
            }));

        }
        private void MFrm_Load(object sender, System.EventArgs e)
        {
            this.ShowWaitForm("正在初始化");
            Task.Run(() =>
            {
                InitVibrating();
                AutoLoad();
                var r = ToneOp.Init();
                Invoke(new Action(() =>
                {
                    this.HideWaitForm();
                    if (!r.status)
                    {
                        this.ShowErrorDialog(r.msg);
                    }


                }));

               
                //LoadData();
            });
        }
        private void MFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.ShowWaitForm("正在关闭程序");
            ToneOp.Destroy();
            this.HideWaitForm();
            //connection.Close();//释放数据连接
        }
        #endregion

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpButton_Click(object sender, EventArgs e)
        {
            //.自动模式
            LogNet.Info("自动运行");
            //EComManageer.Write<short>("ModbusTcpNet0", "1208", 16);
            //ExtHandler.CycleRunSol();
            OpButton.Style = UIStyle.Green;
            timer1.Interval = 100;
            timer1.Start();
            //plc标记启停状态
            var r = EComManageer.GetECommunication("ModbusTcpNet0");
            if (r.status)
            {
                EComManageer.Write<bool>("ModbusTcpNet0", "529", true);
            }
            else
            {
                LogNet.Info("下位连锁异常，无法运行");
            }
            TestImage();
            LogNet.Info("检测就绪，使用按钮启动运行");
            //读取尺寸参数显示
            uiLedLabel1.Text = ExtHandler.GetGlobalVar<double>("检测粒径").ToString()
                + "-"
                + ExtHandler.GetGlobalVar<double>("粒径偏差").ToString()
                + "-"
                + ExtHandler.GetGlobalVar<double>("圆度").ToString()
                ;
            CleanData();//检测数据清零
            //初始化服务
        }

        //检测数据清零
        private void CleanData()
        {
            ExtHandler.OnceRunProj("初始化数据");
            //初始计数清零，防止销毁不到队列
            ToneOp.CameraI = 0;
            ToneOp.CameraO = 0;
            ToneOp.ToneProcesses[0].curOrder = 0;
            ToneOp.ToneProcesses[1].curOrder = 0;
            ToneOp.ToneProcesses[2].curOrder = 0;
            ToneOp.ToneProcesses[3].curOrder = 0;
            ToneOp.ToneProcesses[4].curOrder = 0;
            ToneOp.ToneProcesses[5].curOrder = 0;
            ToneOp.缓存 = uiIntegerUpDown1.Value;
            LogNet.Info("排照次数及触发次数清零");
        }
        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OPStopButton_Click(object sender, EventArgs e)
        {
            var r = EComManageer.GetECommunication("ModbusTcpNet0");
            if (r.status)
            {
                EComManageer.Write<bool>("ModbusTcpNet0", "529", false);
                LogNet.Info("检测停止，无法自动运行");
                //清空手动按钮启动状态和央视
                EComManageer.Write<bool>("ModbusTcpNet0", "500", false);
                MainStart.Style = UIStyle.Blue;
                MainStart.Text = "启动";
                EComManageer.Write<bool>("ModbusTcpNet0", "505", false);
                CutStart.Style = UIStyle.Blue;
                CutStart.Text = "启动";
                EComManageer.Write<bool>("ModbusTcpNet0", "510", false);
                Turn1Start.Style = UIStyle.Blue;
                Turn1Start.Text = "启动";
                EComManageer.Write<bool>("ModbusTcpNet0", "515", false);
                Turn2Start.Style = UIStyle.Blue;
                Turn2Start.Text = "启动";
                EComManageer.Write<bool>("ModbusTcpNet0", "520", false);
                StirStart.Style = UIStyle.Blue;
                StirStart.Text = "启动";
            }
            else
            {
                LogNet.Info("下位连锁异常，无法自动运行！");
            }
            Thread.Sleep(100);
            LogNet.Info("停止检测");
            SVdata();
            ExtHandler.StopRunSol();
            timer1.Stop();
            //EComManageer.Write<bool>("ModbusTcpNet0", "501", true);
            //EComManageer.Write<short>("ModbusTcpNet0", "1208", 64);
            //ToneOp.Destroy();
            //ToneProcess toneProcess = new ToneProcess();
           // toneProcess.Destroy();
            OpButton.Style = UIStyle.Gray;
            //plc标记启停状态

            //清理照片和中断计数
            //停止自动打印
            if (uiSwitch1.Active)
            {
                PT_res();
            }
        }

        private void SVdata()
        {            
            string r_ok = uiLabel49.Text.ToString();
            string r_ng = uiLabel50.Text.ToString();
            string r_剔除 = uiLabel28.Text.ToString();
            string r_通过 = uiLabel30.Text.ToString();
            //string r_合格率 = uiLabel30.Text.ToString();//合格率，需要从打印地方同意
            string r_粒径 = uiLabel70.Text.ToString();
            string r_圆度值 = uiLabel71.Text.ToString();
            string r_标准差 = uiLabel66.Text.ToString();
            string r_001 = uiLabel38.Text.ToString();
            string r_002 = uiLabel39.Text.ToString();
            string r_003 = uiLabel40.Text.ToString();
            string r_004 = uiLabel41.Text.ToString();
            string r_005 = uiLabel42.Text.ToString();
            string r_006 = uiLabel43.Text.ToString();
            string r_007 = uiLabel44.Text.ToString();
            string r_008 = uiLabel45.Text.ToString();
            string r_009 = uiLabel46.Text.ToString();
            string r_010 = uiLabel52.Text.ToString();
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            int 通过 = ExtHandler.GetGlobalVar<Int32>("通过");
            int 剔除 = ExtHandler.GetGlobalVar<Int32>("r_tc");
            float 合格率 = ((float)通过 / (通过 + 剔除)) * 100;
            double r_合格率 = Math.Round((float)合格率, 3);
            string insertQuery = "INSERT INTO 结果 (日期,OK,NG,剔除,通过,合格率,粒径,圆度值,标准差,皮帽,脏污,偏心,凹陷,凸点,气泡,异型,空壳,尺寸,圆度) VALUES (@日期,@OK, @NG,@剔除,@通过,@合格率,@粒径,@圆度值,@标准差,@皮帽,@脏污,@偏心,@凹陷,@凸点,@气泡,@异型,@空壳,@尺寸,@圆度)";
            using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@日期", date);
                command.Parameters.AddWithValue("@OK", r_ok);
                command.Parameters.AddWithValue("@NG", r_ng);
                command.Parameters.AddWithValue("@剔除", r_剔除);
                command.Parameters.AddWithValue("@通过", r_通过);
                command.Parameters.AddWithValue("@合格率", r_合格率);
                command.Parameters.AddWithValue("@粒径", r_粒径);
                command.Parameters.AddWithValue("@圆度值", r_圆度值);
                command.Parameters.AddWithValue("@标准差", r_标准差);
                command.Parameters.AddWithValue("@皮帽", r_001);
                command.Parameters.AddWithValue("@脏污", r_002);
                command.Parameters.AddWithValue("@偏心", r_003);
                command.Parameters.AddWithValue("@凹陷", r_004);
                command.Parameters.AddWithValue("@凸点", r_005);
                command.Parameters.AddWithValue("@气泡", r_006);
                command.Parameters.AddWithValue("@异型", r_007);
                command.Parameters.AddWithValue("@空壳", r_008);
                command.Parameters.AddWithValue("@尺寸", r_009);
                command.Parameters.AddWithValue("@圆度", r_010);                
                command.ExecuteNonQuery();
            }
            LoadData(); // 刷新数据
        }
        /// <summary>
        /// 设置速度
        /// </summary>
        private void SaveVar()
        {
            //VarParam.BoxH = MainUpDown.Value;
            VarParam.Main = Convert.ToUInt16(MainUpDown.Value);
            VarParam.Cut = Convert.ToUInt16(CutUpDown.Value);
            VarParam.Turn1 = Convert.ToUInt16(TurnUpDown1.Value);
            VarParam.Turn2 = Convert.ToUInt16(TurnUpDown2.Value);
            VarParam.Stir = Convert.ToUInt16(StirUpDown.Value);
            SendVar();
            ExtHandler.SaveConf();
            this.ShowSuccessTip("设置成功");
        }
        private void SendVar()
        {
            var r = EComManageer.GetECommunication("ModbusTcpNet0");
            if (r.status)
            {
                var e = (ECommunication)r.data;
                if (e.IsConnected)
                {
                    //速度设置
                    EComManageer.Write<short>("ModbusTcpNet0", "1000", short.Parse(VarParam.Main.ToString()));
                    EComManageer.Write<short>("ModbusTcpNet0", "1001", short.Parse(VarParam.Cut.ToString()));
                    EComManageer.Write<short>("ModbusTcpNet0", "1002", short.Parse(VarParam.Turn1.ToString()));
                    EComManageer.Write<short>("ModbusTcpNet0", "1003", short.Parse(VarParam.Turn2.ToString()));
                    EComManageer.Write<short>("ModbusTcpNet0", "1004", short.Parse(VarParam.Stir.ToString()));
                }
            }
        }

        private void SaveABButton_Click(object sender, EventArgs e)
        {
            SaveVar();
        }

        public void TestImage()//算法预热
        {
            try
            {
                // 2. 读取测试图像
                HObject testImage;
                HOperatorSet.ReadImage(out testImage, "E:/Image/test/test.bmp");

                // 3. 循环执行处理
                for (int i = 0; i < 10; i++)
                {
                    ExtHandler.ExeProj("内1");
                    ExtHandler.ExeProj("内2");
                    ExtHandler.ExeProj("内3");
                    ExtHandler.ExeProj("外1");
                    ExtHandler.ExeProj("外2");
                    ExtHandler.ExeProj("外3");
                }

                // 4. 释放资源
                testImage.Dispose();

            }
            catch (Exception ex)
            {
                LogNet.Error("测试失败:   " + ex.ToString());
            }

        }

        #region   电气参数

        private bool MainRun = false;

        //主电机启停
        private void MainStart_Click(object sender, EventArgs e)
        {
            if (!MainRun)
            {
                MainRun = true;
                MainStart.Text = "停止";
                MainStart.Style = UIStyle.Green;
            }
            else
            {
                MainRun = false;
                MainStart.Text = "启动";
                MainStart.Style = UIStyle.Blue;
            }
            var r = EComManageer.GetECommunication("ModbusTcpNet0");
            if (r.status & MainRun)
            {
                EComManageer.Write<bool>("ModbusTcpNet0", "500", true);
            }
            else if (r.status & !MainRun)
            {
                EComManageer.Write<bool>("ModbusTcpNet0", "500", false);
            }
        }

        private void RemoveSet_Click(object sender, EventArgs e)
        {
            var r = EComManageer.GetECommunication("ModbusTcpNet0");
            if (r.status)
            {
                var el = (ECommunication)r.data;
                if (el.IsConnected)
                {
                    //速度设置
                    EComManageer.Write<short>("ModbusTcpNet0", "1500", short.Parse(InsideStep1.Value.ToString()));
                    EComManageer.Write<short>("ModbusTcpNet0", "1501", short.Parse(InsideStep2.Value.ToString()));
                    EComManageer.Write<short>("ModbusTcpNet0", "1502", short.Parse(InsideStep3.Value.ToString()));
                    EComManageer.Write<short>("ModbusTcpNet0", "1503", short.Parse(OutsideStep1.Value.ToString()));
                    EComManageer.Write<short>("ModbusTcpNet0", "1504", short.Parse(OutsideStep2.Value.ToString()));
                    EComManageer.Write<short>("ModbusTcpNet0", "1505", short.Parse(OutsideStep3.Value.ToString()));
                    EComManageer.Write<short>("ModbusTcpNet0", "1506", short.Parse(InBlowTime.Value.ToString()));
                    EComManageer.Write<short>("ModbusTcpNet0", "1507", short.Parse(InOffBlowTime.Value.ToString()));
                    EComManageer.Write<short>("ModbusTcpNet0", "1508", short.Parse(OutBlowTime.Value.ToString()));
                    EComManageer.Write<short>("ModbusTcpNet0", "1509", short.Parse(OutOffBlowTime.Value.ToString()));
                    EComManageer.Write<short>("ModbusTcpNet0", "1510", short.Parse(IDeviation.Value.ToString()));
                    EComManageer.Write<short>("ModbusTcpNet0", "1511", short.Parse(ODeviation.Value.ToString()));
                }
            }
        }
        //选项卡电机读取plc参数，后续切换至选项卡切换中运行
        private void readplcdate(object sender, EventArgs e)
        {
            var r = EComManageer.GetECommunication("ModbusTcpNet0");
            if (r.status)
            {
                var Main = EComManageer.Read<UInt16>("ModbusTcpNet0", "1000");
                var Cut = EComManageer.Read<UInt16>("ModbusTcpNet0", "1001");
                var Turn1 = EComManageer.Read<UInt16>("ModbusTcpNet0", "1002");
                var Turn2 = EComManageer.Read<UInt16>("ModbusTcpNet0", "1003");
                var Stir = EComManageer.Read<UInt16>("ModbusTcpNet0", "1004");
                //读取剔除步数，来源单片机返回数据
                var step1 = EComManageer.Read<UInt16>("ModbusTcpNet0", "1400");
                var step2 = EComManageer.Read<UInt16>("ModbusTcpNet0", "1401");
                var step3 = EComManageer.Read<UInt16>("ModbusTcpNet0", "1402");
                var step4 = EComManageer.Read<UInt16>("ModbusTcpNet0", "1403");
                var step5 = EComManageer.Read<UInt16>("ModbusTcpNet0", "1404");
                var step6 = EComManageer.Read<UInt16>("ModbusTcpNet0", "1405");
                var time1 = EComManageer.Read<UInt16>("ModbusTcpNet0", "1406");
                var time2 = EComManageer.Read<UInt16>("ModbusTcpNet0", "1407");
                var time3 = EComManageer.Read<UInt16>("ModbusTcpNet0", "1408");
                var time4 = EComManageer.Read<UInt16>("ModbusTcpNet0", "1409");
                var step7 = EComManageer.Read<UInt16>("ModbusTcpNet0", "1410");//内偏移
                var step8 = EComManageer.Read<UInt16>("ModbusTcpNet0", "1411");//外偏移*/                
                //显示数据
                MainUpDown.Value = Convert.ToUInt16(Main.data);//主电机速度
                CutUpDown.Value = Convert.ToUInt16(Cut.data);//皮带电机速度
                TurnUpDown1.Value = Convert.ToUInt16(Turn1.data);//拨珠盘1速度
                TurnUpDown2.Value = Convert.ToUInt16(Turn2.data);//拨珠盘2速度
                StirUpDown.Value = Convert.ToUInt16(Stir.data);//搅动棍速度
                InsideStep1.Value = Convert.ToUInt16(step1.data);
                InsideStep2.Value = Convert.ToUInt16(step2.data);
                InsideStep3.Value = Convert.ToUInt16(step3.data);
                OutsideStep1.Value = Convert.ToUInt16(step4.data);
                OutsideStep2.Value = Convert.ToUInt16(step5.data);
                OutsideStep3.Value = Convert.ToUInt16(step6.data);
                InBlowTime.Value = Convert.ToUInt16(time1.data);
                InOffBlowTime.Value = Convert.ToUInt16(time2.data);
                OutBlowTime.Value = Convert.ToUInt16(time3.data);
                OutOffBlowTime.Value = Convert.ToUInt16(time4.data);
                IDeviation.Value = Convert.ToUInt16(step7.data);
                ODeviation.Value = Convert.ToUInt16(step8.data);
            }
            //读取数据库，用于结果展示             

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            uiLabel49.Text = ExtHandler.GetGlobalVar<Int32>("r_ok").ToString();
            uiLabel50.Text = ExtHandler.GetGlobalVar<Int32>("r_ng").ToString();
            uiLabel70.Text = ExtHandler.GetGlobalVar<double>("粒径均值").ToString();       
            uiLabel66.Text = ExtHandler.GetGlobalVar<double>("方差均值").ToString();
            uiLabel71.Text = ExtHandler.GetGlobalVar<double>("圆度均值").ToString();
            uiLabel30.Text = ExtHandler.GetGlobalVar<Int32>("通过").ToString();
            uiLabel53.Text = ExtHandler.GetGlobalVar<double>("色差E").ToString();
            uiLabel53.Text = uiLabel53.Text+"/"+Math.Round((float)ExtHandler.GetGlobalVar<Int32>("r_011") / (float)ExtHandler.GetGlobalVar<Int32>("r_ng") * 100, 3).ToString() + "%";
            var r = EComManageer.GetECommunication("ModbusTcpNet0");
            if (r.status)
            {
                var cull = EComManageer.Read<UInt16>("ModbusTcpNet0", "1412");//剔除*/
                uiLabel28.Text = cull.data.ToString();
                ExtHandler.AddGlobalVar("r_tc", Convert.ToInt32(cull.data));
                //int 通过数 = ExtHandler.GetGlobalVar<Int32>("通过") - ExtHandler.GetGlobalVar<Int32>("空槽");
                //ExtHandler.AddGlobalVar("通过", 通过数);
            }
            if ( ExtHandler.GetGlobalVar<Int32>("内排数") > 0 && ExtHandler.GetGlobalVar<Int32>("外排数") > 0)
            {
                    ExtHandler.AddGlobalVar("通过",
                    ExtHandler.GetGlobalVar<Int32>("内通过")
                    + ExtHandler.GetGlobalVar<Int32>("外通过")                  
                    - ExtHandler.GetGlobalVar<Int32>("r_tc"));
            }
            else
            {
                ExtHandler.AddGlobalVar("内空槽", 0);
                ExtHandler.AddGlobalVar("外空槽", 0);
            }
            if(ExtHandler.GetGlobalVar<Int32>("r_ng")>0)
              {
                uiLabel38.Text = Math.Round((float)ExtHandler.GetGlobalVar<Int32>("r_001") / (float)ExtHandler.GetGlobalVar<Int32>("r_ng")*100,3).ToString() + "%";
                uiLabel39.Text = Math.Round((float)ExtHandler.GetGlobalVar<Int32>("r_002") / (float)ExtHandler.GetGlobalVar<Int32>("r_ng") * 100, 3).ToString() + "%";
                uiLabel40.Text = Math.Round((float)ExtHandler.GetGlobalVar<Int32>("r_003") / (float)ExtHandler.GetGlobalVar<Int32>("r_ng") * 100, 3).ToString() + "%";
                uiLabel41.Text = Math.Round((float)ExtHandler.GetGlobalVar<Int32>("r_004") / (float)ExtHandler.GetGlobalVar<Int32>("r_ng") * 100,3).ToString() + "%";
                uiLabel42.Text = Math.Round((float)ExtHandler.GetGlobalVar<Int32>("r_005") / (float)ExtHandler.GetGlobalVar<Int32>("r_ng") * 100, 3).ToString() + "%";
                uiLabel43.Text = Math.Round((float)ExtHandler.GetGlobalVar<Int32>("r_006") / (float)ExtHandler.GetGlobalVar<Int32>("r_ng") * 100, 3).ToString() + "%";
                uiLabel44.Text = Math.Round((float)ExtHandler.GetGlobalVar<Int32>("r_007") / (float)ExtHandler.GetGlobalVar<Int32>("r_ng") * 100, 3).ToString() + "%";
                uiLabel45.Text = Math.Round((float)ExtHandler.GetGlobalVar<Int32>("r_008") / (float)ExtHandler.GetGlobalVar<Int32>("r_ng") * 100, 3).ToString() + "%";
                uiLabel46.Text = Math.Round((float)ExtHandler.GetGlobalVar<Int32>("r_009") / (float)ExtHandler.GetGlobalVar<Int32>("r_ng") * 100, 3).ToString() + "%";
                uiLabel52.Text = Math.Round((float)ExtHandler.GetGlobalVar<Int32>("r_010") / (float)ExtHandler.GetGlobalVar<Int32>("r_ng") * 100, 3).ToString() + "%";
            }
        }
        //主电机方向
        private bool MainRotation_d = false;
        private void MainRotation_Click(object sender, EventArgs e)
        {
            if (!MainRotation_d)
            {
                MainRotation_d = true;
                MainRotation.Text = "反转";
                MainRotation.Style = UIStyle.Green;
            }
            else
            {
                MainRotation_d = false;
                MainRotation.Text = "正转";
                MainRotation.Style = UIStyle.Blue;
            }
            var r = EComManageer.GetECommunication("ModbusTcpNet0");
            if (r.status & MainRotation_d)
            {
                EComManageer.Write<bool>("ModbusTcpNet0", "501", true);
            }
            else if (r.status & !MainRotation_d)
            {
                EComManageer.Write<bool>("ModbusTcpNet0", "501", false);
            }
        }
        //皮带电机启停
        private bool CutRun = false;
        private void CutStart_Click(object sender, EventArgs e)
        {
            if (!CutRun)
            {
                CutRun = true;
                CutStart.Text = "停止";
                CutStart.Style = UIStyle.Green;
            }
            else
            {
                CutRun = false;
                CutStart.Text = "启动";
                CutStart.Style = UIStyle.Blue;
            }
            var r = EComManageer.GetECommunication("ModbusTcpNet0");
            if (r.status & CutRun)
            {
                EComManageer.Write<bool>("ModbusTcpNet0", "505", true);
            }
            else if (r.status & !CutRun)
            {
                EComManageer.Write<bool>("ModbusTcpNet0", "505", false);
            }
        }
        // 皮带电机正反转
        private bool CutRotation_d = false;
        private void CutRotation_Click(object sender, EventArgs e)
        {
            if (!CutRotation_d)
            {
                CutRotation_d = true;
                CutRotation.Text = "反转";
                CutRotation.Style = UIStyle.Green;
            }
            else
            {
                CutRotation_d = false;
                CutRotation.Text = "正转";
                CutRotation.Style = UIStyle.Blue;
            }
            var r = EComManageer.GetECommunication("ModbusTcpNet0");
            if (r.status & CutRotation_d)
            {
                EComManageer.Write<bool>("ModbusTcpNet0", "506", true);
            }
            else if (r.status & !CutRotation_d)
            {
                EComManageer.Write<bool>("ModbusTcpNet0", "506", false);
            }
        }
        // 拨珠1电机启停
        private bool Turn1 = false;
        private void Turn1Start_Click(object sender, EventArgs e)
        {
            if (!Turn1)
            {
                Turn1 = true;
                Turn1Start.Text = "停止";
                Turn1Start.Style = UIStyle.Green;
            }
            else
            {
                Turn1 = false;
                Turn1Start.Text = "启动";
                Turn1Start.Style = UIStyle.Blue;
            }
            var r = EComManageer.GetECommunication("ModbusTcpNet0");
            if (r.status & Turn1)
            {
                EComManageer.Write<bool>("ModbusTcpNet0", "515", true);
            }
            else if (r.status & !Turn1)
            {
                EComManageer.Write<bool>("ModbusTcpNet0", "515", false);
            }
        }
        //拨珠1电机正反转
        private bool Turn1_d = false;
        private void Turn1Rotation_Click(object sender, EventArgs e)
        {
            if (!Turn1_d)
            {
                Turn1_d = true;
                Turn1Rotation.Text = "反转";
                Turn1Rotation.Style = UIStyle.Green;
            }
            else
            {
                Turn1_d = false;
                Turn1Rotation.Text = "正转";
                Turn1Rotation.Style = UIStyle.Blue;
            }
            var r = EComManageer.GetECommunication("ModbusTcpNet0");
            if (r.status & Turn1_d)
            {
                EComManageer.Write<bool>("ModbusTcpNet0", "516", true);
            }
            else if (r.status & !Turn1_d)
            {
                EComManageer.Write<bool>("ModbusTcpNet0", "516", false);
            }
        }
        //拨珠2电机启停
        private bool Turn2 = false;
        private void Turn2Start_Click(object sender, EventArgs e)
        {
            if (!Turn2)
            {
                Turn2 = true;
                Turn2Start.Text = "停止";
                Turn2Start.Style = UIStyle.Green;
            }
            else
            {
                Turn2 = false;
                Turn2Start.Text = "启动";
                Turn2Start.Style = UIStyle.Blue;
            }
            var r = EComManageer.GetECommunication("ModbusTcpNet0");
            if (r.status & Turn2)
            {
                EComManageer.Write<bool>("ModbusTcpNet0", "520", true);
            }
            else if (r.status & !Turn2)
            {
                EComManageer.Write<bool>("ModbusTcpNet0", "520", false);
            }
        }
        //拨珠2电机方向
        private bool Turn2_d = false;
        private void Turn2Rotation_Click(object sender, EventArgs e)
        {
            if (!Turn2_d)
            {
                Turn2_d = true;
                Turn2Rotation.Text = "反转";
                Turn2Rotation.Style = UIStyle.Green;
            }
            else
            {
                Turn2_d = false;
                Turn2Rotation.Text = "正转";
                Turn2Rotation.Style = UIStyle.Blue;
            }
            var r = EComManageer.GetECommunication("ModbusTcpNet0");
            if (r.status & Turn2_d)
            {
                EComManageer.Write<bool>("ModbusTcpNet0", "521", true);
            }
            else if (r.status & !Turn2_d)
            {
                EComManageer.Write<bool>("ModbusTcpNet0", "521", false);
            }
        }
        //搅动电机启停
        private bool Stir = false;
        private void StirStart_Click(object sender, EventArgs e)
        {
            if (!Stir)
            {
                Stir = true;
                StirStart.Text = "停止";
                StirStart.Style = UIStyle.Green;
            }
            else
            {
                Stir = false;
                StirStart.Text = "启动";
                StirStart.Style = UIStyle.Blue;
            }
            var r = EComManageer.GetECommunication("ModbusTcpNet0");
            if (r.status & Stir)
            {
                EComManageer.Write<bool>("ModbusTcpNet0", "510", true);
            }
            else if (r.status & !Stir)
            {
                EComManageer.Write<bool>("ModbusTcpNet0", "510", false);
            }
        }
        //搅动电机正反转
        private bool Stir_d = false;
        private void StirRotation_Click(object sender, EventArgs e)
        {
            if (!Stir_d)
            {
                Stir_d = true;
                StirRotation.Text = "反转";
                StirRotation.Style = UIStyle.Green;
            }
            else
            {
                Stir_d = false;
                StirRotation.Text = "正转";
                StirRotation.Style = UIStyle.Blue;
            }
            var r = EComManageer.GetECommunication("ModbusTcpNet0");
            if (r.status & Stir_d)
            {
                EComManageer.Write<bool>("ModbusTcpNet0", "511", true);
            }
            else if (r.status & !Stir_d)
            {
                EComManageer.Write<bool>("ModbusTcpNet0", "511", false);
            }
        }
        //全开吹气清洁，未作启停状态判断
        private bool CleanFlow_d = false;
        private void CleanFlow_Click(object sender, EventArgs e)
        {
            if (!CleanFlow_d)
            {
                CleanFlow_d = true;
                CleanFlow.Text = "关闭吹气";
                CleanFlow.Style = UIStyle.Green;
            }
            else
            {
                CleanFlow_d = false;
                CleanFlow.Text = "清洁吹气";
                CleanFlow.Style = UIStyle.Blue;
            }

        }

        private void uiDatePicker1_ValueChanged(object sender, DateTime value)
        {
            // 获取 uiDatePicker 中选择的日期
            DateTime selectedDate = uiDatePicker1.Value;
            string dateString = selectedDate.ToString("yyyy-MM-dd");

            // 查询数据库
            string query = $"SELECT * FROM 结果 WHERE 日期 = '{dateString}'";
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=data.db;Version=3;"))
            {
                try
                {
                    conn.Open();
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // 将查询结果显示在 DataGridView 中
                    uiDataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"查询失败: {ex.Message}");
                }
            }
        }

        private void uiLabel19_Click(object sender, EventArgs e)
        {

        }

        //读取当前计算的平均值作为输出值参考

        private void uiButton13_Click(object sender, EventArgs e)
        {
            if (uiLabel70.Text.ToDouble() != 0)
            {
                uiNumPadTextBox2.Text = uiLabel70.Text;            }
           
            if (uiLabel71.Text.ToDouble() != 0)
            {
                uiNumPadTextBox4.Text = uiLabel71.Text;
            }
            if (ExtHandler.GetGlobalVar<double>("r_L") != 0)
            {
                uiNumPadTextBox5.Text = ExtHandler.GetGlobalVar<double>("r_L").ToString();
            }
            if (ExtHandler.GetGlobalVar<double>("r_A") != 0)
            {
                uiNumPadTextBox6.Text = ExtHandler.GetGlobalVar<double>("r_A").ToString();
            }
            if (ExtHandler.GetGlobalVar<double>("r_B") != 0)
            {
                uiNumPadTextBox7.Text = ExtHandler.GetGlobalVar<double>("r_B").ToString();
            }
        }
        //设置检测参数
        private void uiButton12_Click(object sender, EventArgs e)
        {
            ExtHandler.AddGlobalVar("检测粒径", uiNumPadTextBox2.Text.ToDouble());//uiDoubleUpDown1.Value);
            ExtHandler.AddGlobalVar("粒径偏差", uiNumPadTextBox3.Text.ToDouble());// uiDoubleUpDown2.Value);
            ExtHandler.AddGlobalVar("圆度", uiNumPadTextBox4.Text.ToDouble());// uiDoubleUpDown3.Value);            
            ExtHandler.AddGlobalVar("色差阈值", uiNumPadTextBox1.Text.ToDouble());
            ExtHandler.AddGlobalVar("UI_L", uiNumPadTextBox5.Text.ToDouble());
            ExtHandler.AddGlobalVar("UI_A", uiNumPadTextBox6.Text.ToDouble());
            ExtHandler.AddGlobalVar("UI_B", uiNumPadTextBox7.Text.ToDouble());
            ToneOp.缓存= uiIntegerUpDown1.Value;
            LogNet.Info("设置检测参数：" 
                + uiNumPadTextBox2.Text 
                + "±" + uiNumPadTextBox3.Text 
                + "圆度" + uiNumPadTextBox4.Text 
                +"缓存"+ uiIntegerUpDown1.Value
                + "色差阈值:" + uiNumPadTextBox1.Text
                + "L:" + uiNumPadTextBox5.Text
                + "A:" + uiNumPadTextBox6.Text
                + "B:" + uiNumPadTextBox7.Text);
            uiLedLabel1.Text = ExtHandler.GetGlobalVar<double>("检测粒径").ToString()
               + "-"
               + ExtHandler.GetGlobalVar<double>("粒径偏差").ToString()
               + "-"
               + ExtHandler.GetGlobalVar<double>("圆度").ToString()
               ;
            // 步骤1: 创建绘图位图
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                // 步骤2: 设置绘制参数
                int diameter = 46;          // 直径
                Point center = new Point(    // 圆心坐标
                    (pictureBox1.Width - diameter) / 2,
                    (pictureBox1.Height - diameter) / 2
                );

                // 步骤3: 转换Lab颜色,修正halcon转换无负数，确保正确显示
                double L = uiNumPadTextBox5.Text.ToDouble()-128.0;
                double A = uiNumPadTextBox6.Text.ToDouble()-128.0;
                double B = uiNumPadTextBox7.Text.ToDouble() - 128.0;
                Color labColor = LabToRgb(L, A, B);

                // 步骤4: 绘制圆形
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.FillEllipse(new SolidBrush(labColor), center.X, center.Y, diameter, diameter);
            }

            // 步骤5: 显示结果
            pictureBox1.Image = bmp;
        }
        //lab转换rgb方法
        public static Color LabToRgb(double l, double a, double b)
        {
            const double epsilon = 0.008856;
            const double kappa = 903.3;
            double Xn = 0.95047;
            double Yn = 1.0;
            double Zn = 1.08883;

            double fy = (l + 16) / 116;
            double fx = a / 500 + fy;
            double fz = fy - b / 200;

            // 计算X
            double X = (Math.Pow(fx, 3) > epsilon) ? Xn * Math.Pow(fx, 3) : Xn * (116 * fx - 16) / kappa;

            // 计算Y
            double Y = (l > kappa * epsilon) ? Yn * Math.Pow((l + 16) / 116, 3) : Yn * l / kappa;

            // 计算Z
            double Z = (Math.Pow(fz, 3) > epsilon) ? Zn * Math.Pow(fz, 3) : Zn * (116 * fz - 16) / kappa;

            // XYZ转线性RGB
            double R_linear = 3.2406 * X - 1.5372 * Y - 0.4986 * Z;
            double G_linear = -0.9689 * X + 1.8758 * Y + 0.0415 * Z;
            double B_linear = 0.0557 * X - 0.2040 * Y + 1.0570 * Z;

            // Gamma校正
            R_linear = (R_linear <= 0.0031308) ? 12.92 * R_linear : 1.055 * Math.Pow(R_linear, 1 / 2.4) - 0.055;
            G_linear = (G_linear <= 0.0031308) ? 12.92 * G_linear : 1.055 * Math.Pow(G_linear, 1 / 2.4) - 0.055;
            B_linear = (B_linear <= 0.0031308) ? 12.92 * B_linear : 1.055 * Math.Pow(B_linear, 1 / 2.4) - 0.055;

            // 限制范围并转换为Color
            int red = (int)(Clamp(R_linear, 0, 1) * 255 + 0.5);
            int green = (int)(Clamp(G_linear, 0, 1) * 255 + 0.5);
            int blue = (int)(Clamp(B_linear, 0, 1) * 255 + 0.5);

            return Color.FromArgb(red, green, blue);
        }
        private static double Clamp(double value, double min, double max)
        {
            return Math.Max(min, Math.Min(max, value));
        }

        private void uiButton14_Click(object sender, EventArgs e)
        {
            // 显示确认提示框
            if (UIMessageBox.ShowAsk("确定退出程序？整机将停止运行！", true, UIMessageDialogButtons.Ok))
            {
                SaveSettings();
                this.Close();
            }
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        // 写入INI文件
        // 写入INI文件
        public static void Write(string iniPath, string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, iniPath);
        }

        // 读取INI文件
        public static string Read(string iniPath, string section, string key, string defaultValue)
        {
            StringBuilder buffer = new StringBuilder(255);
            int result = GetPrivateProfileString(section, key, defaultValue, buffer, 255, iniPath);
            return buffer.ToString();
        }
        // 保存界面参数到INI文件
        public void SaveSettings()
        {
            string iniPath = Application.StartupPath + "\\data.ini";
            IniFile ini = new IniFile(iniPath);

            // 保存 uiNumPadTextBox2 到 5 的值
            ini.Write("检测设置", "粒径", uiNumPadTextBox2.Text);
            ini.Write("检测设置", "偏差", uiNumPadTextBox3.Text);
            ini.Write("检测设置", "圆度", uiNumPadTextBox4.Text);          
            ini.Write("检测设置", "缓存", uiIntegerUpDown1.Value);
            ini.Write("检测设置", "色差阈值", uiNumPadTextBox1.Text);
            ini.Write("检测设置", "UI_L", uiNumPadTextBox5.Text);
            ini.Write("检测设置", "UI_A", uiNumPadTextBox6.Text);
            ini.Write("检测设置", "UI_B", uiNumPadTextBox7.Text);

            // 保存 uiSwitch1 和 uiSwitch11 的值            
            ini.Write("打印设置", "打印开关", uiSwitch1.Active.ToString());

        }

        // 读取界面参数从INI文件
        public void LoadSettings()
        {
            string iniPath = Application.StartupPath + "\\data.ini";
            IniFile ini = new IniFile(iniPath);

            // 读取 uiNumPadTextBox2 到 5 的值
            uiNumPadTextBox2.Text = ini.Read("检测设置", "粒径", "");
            uiNumPadTextBox3.Text = ini.Read("检测设置", "偏差", "");
            uiNumPadTextBox4.Text = ini.Read("检测设置", "圆度", "");
            uiNumPadTextBox1.Text = ini.Read("检测设置", "色差阈值", "");
            uiNumPadTextBox5.Text = ini.Read("检测设置", "UI_L", "");
            uiNumPadTextBox6.Text = ini.Read("检测设置", "UI_A", "");
            uiNumPadTextBox7.Text = ini.Read("检测设置", "UI_B", "");
            uiIntegerUpDown1.Value = ini.Read("检测设置", "缓存", "").ToInt();
            // 读取 uiSwitch1 和 uiSwitch11 的值
            bool switch1Value = bool.Parse(ini.Read("打印设置", "打印开关", "False"));
            uiSwitch1.Active = switch1Value;            
        }


        private void uiLabel24_Click(object sender, EventArgs e)
        {

        }

        //打印程序开始

        [DllImport("POS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "POS_Port_OpenA")]
        static extern Int32 POS_Port_OpenA(String lpName, Int32 iPort, bool bFile, String path);

        [DllImport("POS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "POS_Port_Close")]
        static extern Int32 POS_Port_Close(long iPort);

        [DllImport("POS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "POS_Output_PrintData")]
        static extern Int32 POS_Output_PrintData(long printID, byte[] strBuff, Int32 ilen);
        [DllImport("POS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "POS_Control_AlignType")]
        static extern Int32 POS_Control_AlignType(long printID, Int32 iAlignType);
        [DllImport("POS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "POS_Output_PrintFontStringA")]
        static extern Int32 POS_Output_PrintFontStringA(long printID, Int32 iFont, Int32 iThick, Int32 iWidth, Int32 iHeight, Int32 iUnderLine, String lpString);
        [DllImport("POS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "POS_Output_PrintTwoDimensionalBarcodeA")]
        static extern Int32 POS_Output_PrintTwoDimensionalBarcodeA(long printID, Int32 iType, Int32 parameter1, Int32 parameter2, Int32 parameter3, String lpString);
        [DllImport("POS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "POS_Control_CutPaper")]
        static extern Int32 POS_Control_CutPaper(long printID, Int32 type, Int32 len);
        [DllImport("POS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "POS_Control_ReSet")]
        static extern Int32 POS_Control_ReSet(long printID);
        private void PT_res()
        {
            int ipt;
            long a = POS_Port_OpenA("SP-USB1", 1002, false, "");
            if (a < 0)
            {
                //MessageBox.Show("打开端口失败" + a);
            }
            else
            {
                //MessageBox.Show("打开端口成功");
                byte[] cmd = new byte[] { 0x1c, 0x26 };
                POS_Output_PrintData(a, cmd, 2);
                POS_Control_AlignType(a, 1);
                POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, "XOP1080 视觉选丸仪\r\n");
                POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, "检测数据\r\n");
                POS_Control_AlignType(a, 0);
                DateTime now = DateTime.Now;
                POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, now + "                 " + ExtHandler.GetAutoLoadSolName() + "\r\n");
                POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, "-------------------------------------------\r\n");
                POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, "合格:         " + ExtHandler.GetGlobalVar<Int32>("r_ok").ToString() + "次\r\n");
                POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, "NG:           " + ExtHandler.GetGlobalVar<Int32>("r_ng").ToString() + "次\r\n");
                POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, "剔除:         " + ExtHandler.GetGlobalVar<Int32>("r_tc").ToString() + "粒\r\n");
                POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, "通过:         " + ExtHandler.GetGlobalVar<Int32>("通过").ToString() + "粒\r\n");
                int 通过 = ExtHandler.GetGlobalVar<Int32>("通过");
                int 剔除 = ExtHandler.GetGlobalVar<Int32>("r_tc");
                float 合格率 = ((float)通过 /(通过+剔除))*100;
                POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, "合格率:       " + 合格率.ToString() + "%\r\n");
                POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, "-------------------------------------------\r\n");
                POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, "            Agv       Max         Min        \r\n");
                POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, " 粒径:     " + ExtHandler.GetGlobalVar<double>("粒径均值").ToString() + "       " + ExtHandler.GetGlobalVar<double>("粒径最大值").ToString() + "       " + ExtHandler.GetGlobalVar<double>("粒径最小值").ToString() + "\r\n");
                POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, " 圆度:     " + ExtHandler.GetGlobalVar<double>("圆度均值").ToString() + "       " + ExtHandler.GetGlobalVar<double>("圆度最大值").ToString() + "       " + ExtHandler.GetGlobalVar<double>("圆度最小值").ToString() + "\r\n");
                //POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, " 均值:     " + ExtHandler.GetGlobalVar<double>("拟合均值").ToString() + "       " + ExtHandler.GetGlobalVar<double>("拟合最大值").ToString() + "       " + ExtHandler.GetGlobalVar<double>("拟合最小值").ToString() + "\r\n");
                POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, " 标准差:   " + ExtHandler.GetGlobalVar<double>("方差均值").ToString() + "       " + ExtHandler.GetGlobalVar<double>("方差最大值").ToString() + "       " + ExtHandler.GetGlobalVar<double>("方差最小值").ToString() + "\r\n");
                POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, " 长轴:     " + ExtHandler.GetGlobalVar<double>("长轴均值").ToString() + "       " + ExtHandler.GetGlobalVar<double>("长轴最大值").ToString() + "       " + ExtHandler.GetGlobalVar<double>("长轴最小值").ToString() + "\r\n");
                POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, " 短轴:     " + ExtHandler.GetGlobalVar<double>("短轴均值").ToString() + "       " + ExtHandler.GetGlobalVar<double>("短轴最大值").ToString() + "       " + ExtHandler.GetGlobalVar<double>("短轴最小值").ToString() + "\r\n");
                POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, "-------------------------------------------\r\n");
                POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, "皮帽：    " + Math.Round((float)ExtHandler.GetGlobalVar<Int32>("r_001") / (float)ExtHandler.GetGlobalVar<Int32>("r_ng") * 100, 3).ToString() + "%\r\n");
                POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, "脏污：    " + Math.Round((float)ExtHandler.GetGlobalVar<Int32>("r_002") / (float)ExtHandler.GetGlobalVar<Int32>("r_ng") * 100, 3).ToString() + "%\r\n");
                POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, "偏心：    " + Math.Round((float)ExtHandler.GetGlobalVar<Int32>("r_003") / (float)ExtHandler.GetGlobalVar<Int32>("r_ng") * 100, 3).ToString() + "%\r\n");
                POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, "凹陷：    " + Math.Round((float)ExtHandler.GetGlobalVar<Int32>("r_004") / (float)ExtHandler.GetGlobalVar<Int32>("r_ng") * 100, 3).ToString() + "%\r\n");
                POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, "凸点：    " + Math.Round((float)ExtHandler.GetGlobalVar<Int32>("r_005") / (float)ExtHandler.GetGlobalVar<Int32>("r_ng") * 100, 3).ToString() + "%\r\n");
                POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, "气泡：    " + Math.Round((float)ExtHandler.GetGlobalVar<Int32>("r_006") / (float)ExtHandler.GetGlobalVar<Int32>("r_ng") * 100, 3).ToString() + "%\r\n");
                POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, "异型：    " + Math.Round((float)ExtHandler.GetGlobalVar<Int32>("r_007") / (float)ExtHandler.GetGlobalVar<Int32>("r_ng") * 100, 3).ToString() + "%\r\n");
                POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, "空壳：    " + Math.Round((float)ExtHandler.GetGlobalVar<Int32>("r_008") / (float)ExtHandler.GetGlobalVar<Int32>("r_ng") * 100, 3).ToString() + "%\r\n");
                POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, "尺寸：    " + Math.Round((float)ExtHandler.GetGlobalVar<Int32>("r_009") / (float)ExtHandler.GetGlobalVar<Int32>("r_ng") * 100, 3).ToString() + "%\r\n");
                POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, "圆度：    " + Math.Round((float)ExtHandler.GetGlobalVar<Int32>("r_010") / (float)ExtHandler.GetGlobalVar<Int32>("r_ng") * 100, 3).ToString() + "%\r\n");
                POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, "色差：    " + uiLabel53.Text + "\r\n");
                POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, "-------------------------------------------\r\n");
                POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, "检测直径：    " + ExtHandler.GetGlobalVar<double>("检测粒径").ToString() + "\r\n");
                POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, "检测偏差：    " + ExtHandler.GetGlobalVar<double>("粒径偏差").ToString() + "\r\n");
                POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, "检测圆度：    " + ExtHandler.GetGlobalVar<double>("圆度").ToString() + "\r\n");
                POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, "色差阈值：    " + ExtHandler.GetGlobalVar<double>("色差阈值").ToString() + "\r\n");
                //POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, "算法模型：\r\n");
                //POS_Output_PrintFontStringA(a, 0, 0, 0, 0, 0, "模型版本：\r\n");
                POS_Control_CutPaper(a, 1, 3);
                POS_Control_ReSet(a);

            }
            long b = POS_Port_Close(a);
            if (b == 0)
            {
                //MessageBox.Show("关闭端口成功");
            }
        }
        //打印程序结束
        private void uiButton1_Click(object sender, EventArgs e)
        {
            PT_res();
        }
        static int cal_step = 0;
        static bool 校准 = false;
        static int 缓存 = 0;
        private void uiButton2_Click(object sender, EventArgs e)
        {
            string SolName = (string)SolComboBox.SelectedItem;
            ToneOp.缓存 = uiIntegerUpDown1.Value;
            //ExtHandler.StopRunSol();
            if (SolName != "校准算法")
            {
                SolName = "校准算法";
                ExtHandler.Load(SolName, new Action<bool>(rt =>
                {
                    Invoke(new Action(() =>
                    {
                        if (!rt)
                        {
                            SolComboBox.SelectedItem = null;
                            this.ShowErrorDialog("未找到校准算法");
                        }
                        当前算法 = "校准算法";
                        this.Text = "XOP1080 视觉选丸仪-当前牌号:" + 当前算法; // 去掉标题栏文字
                    }));
                }));
                //强制下位运行
            }
            else
            {
                LogNet.Info("当前方案已为校准算法，继续操作");
            }
            var rr = EComManageer.GetECommunication("ModbusTcpNet0");
            if (rr.status)
            {
                EComManageer.Write<bool>("ModbusTcpNet0", "529", true);
                LogNet.Info("下位连锁强制成功");
                校准 = true;
            }
            else
            {
                LogNet.Info("下位连锁异常，无法运行");
                校准 = false;
            }
            uiButton2.Style = UIStyle.Green;
            //初始计数清零，防止销毁不到队列
            ToneOp.CameraI = 0;
            ToneOp.CameraO = 0;            
            ToneOp.ToneProcesses[0].curOrder = 0;
            ToneOp.ToneProcesses[1].curOrder = 0;
            ToneOp.ToneProcesses[2].curOrder = 0;
            ToneOp.ToneProcesses[3].curOrder = 0;
            ToneOp.ToneProcesses[4].curOrder = 0;
            ToneOp.ToneProcesses[5].curOrder = 0;
            if (ToneOp.CameraI == 0 && ToneOp.CameraO == 0)
            {
                LogNet.Warn("初始计数清零，开始校准，使用按钮启动转盘");
            }
        }

        private void uiButton3_Click(object sender, EventArgs e)
        {
            //切换回选择的方案
            //ExtHandler.StopRunSol();
            校准 = false;
            缓存 = 0;
            cal_step1.ItemIndex = -1;
            uiButton2.Style = UIStyle.Blue;
            //强制下位运行
            var r = EComManageer.GetECommunication("ModbusTcpNet0");
            if (r.status)
            {
                EComManageer.Write<bool>("ModbusTcpNet0", "529", false);
            }
            else
            {
                LogNet.Info("下位连锁异常，无法运行");
            }
            string rws = (string)SolComboBox.SelectedItem;
            ExtHandler.Load(rws, new Action<bool>(rw =>
            {
                Invoke(new Action(() =>
                {
                    if (!rw)
                    {
                        SolComboBox.SelectedItem = null;
                        this.ShowErrorDialog("未找到算法");
                    }
                    当前算法 = rws;
                    this.Text = "XOP1080 视觉选丸仪-当前牌号:" + 当前算法; // 去掉标题栏文字
                }));
            }));

        }
        private List<string> steps1 = new List<string>
        {
            "强制下位",
            "拍照中断",
            "次数统计",
            "缓存预估",
            "强制剔除"
        };
        private List<string> steps2 = new List<string>
        {
            "移位缓存",
            "多相机",
            "IO链路",
            "电气数据",
            "通讯数据",
            "参数保存"
        };
        private List<string> steps3 = new List<string>
        {
            "运行速度",
            "相机位置",
            "剔除步数",
            "剔除窗口",
            "参数保存"
        };
        private List<string> steps4 = new List<string>
        {
            "尺寸标定",
            "尺寸校准",
            "颜色标定",
            "颜色校准",
            "参数保存"
        };

        private void CheckBox_checkedChanged(object sender, EventArgs e, List<string> steps)
        {
            UICheckBox checkBox = sender as UICheckBox;
            if (checkBox != null)
            {
                int index = steps.IndexOf(checkBox.Text);
                if (checkBox.Checked)
                {
                    for (int i = 0; i <= index; i++)
                    {
                        ((UICheckBox)this.uiPanel4.Controls[i]).Checked = true;
                    }
                }
                else
                {
                    for (int i = index; i < steps1.Count; i++)
                    {
                        ((UICheckBox)this.uiPanel4.Controls[i]).Checked = false;
                    }
                }
            }
        }
        private void NewCheckBox(List<string> steps)
        {
            int y = 20;
            this.uiPanel4.Controls.Clear();
            foreach (var step in steps)
            {
                UICheckBox checkBox = new UICheckBox
                {
                    Text = step,
                    Location = new System.Drawing.Point(20, y),
                    Size = new System.Drawing.Size(300, 20),
                    Checked = false,
                    //Enabled = false
                };
                checkBox.CheckedChanged += (s, e) => CheckBox_checkedChanged(s, e, steps);
                //checkBox.CheckedChanged += CheckBox_checkedChanged(,,steps);
                this.uiPanel4.Controls.Add(checkBox);
                y += 30;
            };
        }
        private void cal_step1_ItemIndexChanged(object sender, int value)
        {
            switch (cal_step1.ItemIndex)
            {
                case 0:
                    NewCheckBox(steps1);
                    break;
                case 1:
                    NewCheckBox(steps2);
                    break;
                case 2:
                    NewCheckBox(steps3);
                    break;
                case 3:
                    NewCheckBox(steps4);
                    break;
            }
        }

        private async void uiButton4_Click(object sender, EventArgs e)
        {

            while (校准) // 外层循环，确保整个流程可以重复
            {
                img1 = ToneOp.ToneProcesses[0].curOrder;
                img2 = ToneOp.ToneProcesses[1].curOrder;
                img3 = ToneOp.ToneProcesses[2].curOrder;
                img4 = ToneOp.ToneProcesses[3].curOrder;
                img5 = ToneOp.ToneProcesses[4].curOrder;
                img6 = ToneOp.ToneProcesses[5].curOrder;
                ConcurrentDictionary<int, int> 内1接受结果 = ToneOp.IOSignalFlags[0];
                ConcurrentDictionary<int, int> 内2接受结果 = ToneOp.IOSignalFlags[1];
                ConcurrentDictionary<int, int> 内3接受结果 = ToneOp.IOSignalFlags[2];
                ConcurrentDictionary<int, int> 外1接受结果 = ToneOp.IOSignalFlags[3];
                ConcurrentDictionary<int, int> 外2接受结果 = ToneOp.IOSignalFlags[4];
                ConcurrentDictionary<int, int> 外3接受结果 = ToneOp.IOSignalFlags[5];

                // 获取 uiPanel4 中所有被选中的 UICheckBox
                var checkedCheckBoxes = this.uiPanel4.Controls.OfType<UICheckBox>()
                    .Where(cb => cb.Checked)
                    .ToList();

                // 获取选中的复选框数量            
                // 获取最后一个被选中的复选框
                string text = string.Empty; // 在方法外部声明 text 变量
                if (checkedCheckBoxes.Count > 0)
                {
                    UICheckBox lastCheckedCheckBox = checkedCheckBoxes.Last();
                    text = lastCheckedCheckBox.Text;
                }
                else
                {
                    LogNet.Warn("请选择至少一个校准流程");
                    return;
                }
                if (text != null)
                {
                    switch (text)
                    {
                        case "强制下位":
                            LogNet.Warn($"执行以下校准流程: {text}");
                            if (ToneOp.CameraI == 0 && ToneOp.CameraO == 0)
                            {
                                LogNet.Warn("下位已强制，数据已清零，按钮启动校准");
                            }
                            await Task.Delay(1000); // 延迟1秒，避免过高频率
                            break;
                        case "拍照中断":
                            LogNet.Warn($"执行以下校准流程: {text}");
                            if (ToneOp.CameraI == 0)
                            {
                                LogNet.Warn("未收到拍照中断");
                            }
                            else
                            {
                                LogNet.Info("已收到拍照中断");
                                LogNet.Info("内排触发次数:" + ToneOp.CameraI.ToString());
                                LogNet.Info("外排触发次数:" + ToneOp.CameraO.ToString());
                            }
                            await Task.Delay(1000); // 延迟1秒，避免过高频率
                            break;
                        case "次数统计":
                            LogNet.Warn($"执行以下校准流程: {text}");
                            if ((ToneOp.CameraI - img1) < 5 && (ToneOp.CameraI - img2) < 5 && (ToneOp.CameraI - img3) < 5)
                            {
                                LogNet.Info("内1触发-拍照[" + ToneOp.CameraI + "-" + img1 + "=" + (ToneOp.CameraI - img1).ToString() + "]");
                                LogNet.Info("内2触发-拍照[" + ToneOp.CameraI + "-" + img2 + "=" + (ToneOp.CameraI - img2).ToString() + "]");
                                LogNet.Info("内3触发-拍照[" + ToneOp.CameraI + "-" + img3 + "=" + (ToneOp.CameraI - img3).ToString() + "]");
                            }
                            else
                            {
                                LogNet.Warn("缓存差溢出5张！！");
                            }
                            if ((ToneOp.CameraO - img4) < 5 && (ToneOp.CameraO - img5) < 5 && (ToneOp.CameraO - img6) < 5)
                            {
                                LogNet.Info("外1触发-拍照[" + ToneOp.CameraO + "-" + img4 + "=" + (ToneOp.CameraO - img4).ToString() + "]");
                                LogNet.Info("外2触发-拍照[" + ToneOp.CameraO + "-" + img5 + "=" + (ToneOp.CameraO - img5).ToString() + "]");
                                LogNet.Info("外3触发-拍照[" + ToneOp.CameraO + "-" + img6 + "=" + (ToneOp.CameraO - img6).ToString() + "]");
                            }
                            await Task.Delay(1000); // 延迟1秒，避免过高频率
                            break;
                        case "缓存预估":
                            LogNet.Warn($"执行以下校准流程: {text}");                            
                            int 内排最大值 = Math.Max(ToneOp.CameraI - img1, ToneOp.CameraI - img2);
                            内排最大值 = Math.Max(内排最大值, ToneOp.CameraI-img3);
                            if (内排最大值>=缓存)
                            {
                                缓存 = 内排最大值;
                                LogNet.Info("内排缓存预估不小于："+缓存);
                            }
                            else
                            {
                                LogNet.Info("内排缓存预估不小于：" + 缓存);
                            }
                            if(缓存 > 5)
                            {
                                LogNet.Warn("内排缓存预估超5张，无法缓存！！");
                            }
                            int 外排最大值 = Math.Max(ToneOp.CameraO - img4, ToneOp.CameraO - img5);
                            外排最大值 = Math.Max(外排最大值, ToneOp.CameraO - img6);
                            if (外排最大值 >= 缓存)
                            {
                                缓存 = 外排最大值;
                                LogNet.Info("外排缓存预估不小于：" + 缓存);
                            }
                            else
                            {
                                LogNet.Info("外排缓存预估不小于：" + 缓存);
                            }
                            if(缓存 >5)
                            {
                                LogNet.Warn("外排缓存预估超5张，无法缓存！！");
                            }
                            await Task.Delay(1000); // 延迟1秒，避免过高频率
                            break;
                        case "强制剔除":
                            LogNet.Warn($"执行以下校准流程: {text}");
                            HTuple temp_1 = new HTuple();
                            HOperatorSet.TupleGenConst(32, 1, out temp_1);//设置默认值
                            ExtHandler.AddGlobalVar("内1结果", temp_1);
                            ExtHandler.AddGlobalVar("内2结果", temp_1);
                            ExtHandler.AddGlobalVar("内3结果", temp_1);
                            ExtHandler.AddGlobalVar("外1结果", temp_1);
                            ExtHandler.AddGlobalVar("外2结果", temp_1);
                            ExtHandler.AddGlobalVar("外3结果", temp_1);
                            LogNet.Info("强制剔除内排:全部");
                            LogNet.Info("强制剔除外排:全部");
                            await Task.Delay(1000); // 延迟1秒，避免过高频率
                            break;
                        case "移位缓存":
                            await Task.Delay(1000); // 延迟1秒，避免过高频率
                            LogNet.Warn($"执行以下校准流程: {text}");
                            LogNet.Info("缓存步数："+ToneOp.缓存.ToString());
                            // 遍历字典中的所有键值对
                            foreach (var kvp in 内1接受结果)
                            {
                                int key = kvp.Key;
                                int value = kvp.Value;
                                LogNet.Info("内一接收结果：" + key + "次，数据" + value);
                            }
                            //foreach (var kvp in 内2接受结果)
                            //{
                            //    int key = kvp.Key;
                            //    int value = kvp.Value;
                            //    LogNet.Info("内二接收结果：" + key + "次，数据" + value);
                            //}
                            //foreach (var kvp in 内3接受结果)
                            //{
                            //    int key = kvp.Key;
                            //    int value = kvp.Value;
                            //    LogNet.Info("内三接收结果：" + key + "次，数据" + value);
                            //}
                            LogNet.Info("内一刷IO数据：" + ToneOp.CameraI +"次刷"+ (ToneOp.CameraI-uiIntegerUpDown1.Value) + "照片，数据:"+ ToneOp.刷新内1数据);
                            //LogNet.Info("内二刷IO数据：" + ToneOp.CameraI + "次，数据" + ToneOp.刷新内2数据);
                            //LogNet.Info("内三刷IO数据：" + ToneOp.CameraI + "次，数据" + ToneOp.刷新内3数据);
                            //foreach (var kvp in 外1接受结果)
                            //{
                            //    int key = kvp.Key;
                            //    int value = kvp.Value;
                            //    LogNet.Info("外一接收结果：" + key + "次，数据" + value);
                            //}
                            //foreach (var kvp in 外2接受结果)
                            //{
                            //    int key = kvp.Key;
                            //    int value = kvp.Value;
                            //    LogNet.Info("外二接收结果：" + key + "次，数据" + value);
                            //}
                            //foreach (var kvp in 外3接受结果)
                            //{
                            //    int key = kvp.Key;
                            //    int value = kvp.Value;
                            //    LogNet.Info("外三接收结果：" + key + "次，数据" + value);
                            //}
                            //LogNet.Info("外一刷IO数据：" + ToneOp.CameraI + "次，数据" + ToneOp.刷新外1数据);
                            //LogNet.Info("外二刷IO数据：" + ToneOp.CameraI + "次，数据" + ToneOp.刷新外2数据);
                            //LogNet.Info("外三刷IO数据：" + ToneOp.CameraI + "次，数据" + ToneOp.刷新外3数据);
                            break;
                    }
                }
            }
        }
       
        private void uiButton5_Click(object sender, EventArgs e)
        {
            //恢复强制剔除数据
            HTuple temp_1 = new HTuple();
            HOperatorSet.TupleGenConst(32, 0, out temp_1);//设置默认值
            ExtHandler.AddGlobalVar("内1结果", temp_1);
            ExtHandler.AddGlobalVar("内2结果", temp_1);
            ExtHandler.AddGlobalVar("内3结果", temp_1);
            ExtHandler.AddGlobalVar("外1结果", temp_1);
            ExtHandler.AddGlobalVar("外2结果", temp_1);
            ExtHandler.AddGlobalVar("外3结果", temp_1);
            //初始计数清零，防止销毁不到队列
            ToneOp.CameraI = 0;
            ToneOp.CameraO = 0;
            ToneOp.ToneProcesses[0].curOrder = 0;
            ToneOp.ToneProcesses[1].curOrder = 0;
            ToneOp.ToneProcesses[2].curOrder = 0;
            ToneOp.ToneProcesses[3].curOrder = 0;
            ToneOp.ToneProcesses[4].curOrder = 0;
            ToneOp.ToneProcesses[5].curOrder = 0;
            缓存 = 0;
        }

        private void uiButton6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("检测程序即将关闭程序及计算机", "提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
            //调用异步线程关机，确保程序先退出
            Task.Run(() =>
            {
                System.Threading.Thread.Sleep(5000);
                Process.Start("shutdown.exe", "/s /t 0");
            });
            SaveSettings();
            //Application.Exit();
            this.Close();
        }

        private void logView1_Load(object sender, EventArgs e)
        {

        }
    }
}
#endregion