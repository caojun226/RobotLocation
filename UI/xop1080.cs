using RobotLocation.Model;
using RobotLocation.Service;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision.Service;
using VisionCore.Ext;
using VisionCore.Log;

namespace RobotLocation.UI
{
    public partial class xop1080 : UIForm
    {
        //public settings _form1;
        private xop1080u xop1080ui = new xop1080u();
        private xop1080m xop1080model = new xop1080m();
        public xop1080sv xop1080Sv = new xop1080sv();
        public bool lastXT = true;//心跳初始值
        private static System.Timers.Timer mUITimer;
        //public static bool CurrentStatus = false;
        public static xop1080 Instance; // 静态字段，用于其他位置调用，by崔
        public static string ERR = "0";
        public static bool bkstop = false;//按钮停机，定时器扫描
        public static bool bkstart= false;//按钮启动，定时器扫描
        public xop1080()
        {
            InitializeComponent();
            Instance = this; // 保存当前实例，by崔
            this.uiTableLayoutPanel2.Controls.Add(xop1080ui.logview, 0, 2);
            tplChar0.Controls.Add(xop1080ui.uiBarChart, 0, 0);
            tplChar0.Controls.Add(xop1080ui.uiDoughnutChart, 0, 1);
            uPImg.Controls.Add(xop1080ui.imageControl);

            //uiTableLayoutPanel5.Controls.Add(xop1080ui.uiLabel,2,0);
            uiStyleManager1.Style = UIStyle.Blue;
            //this.WindowState = FormWindowState.Maximized;

            this.ShowFullScreen = true; // 设置全屏显示
            this.ControlBox = false; // 去掉控制框（包括关闭按钮）
            //用户
            AppMangerTool.mSysUses = (new SVSysUser()).getlst();
            //盘列表
            AppMangerTool.mPlcData = (new SVPlcData()).getlst();
            uiMenuPanle.Visible = false;
            mUITimer = new System.Timers.Timer(5000); // 2秒间隔
            mUITimer.Elapsed += MUITimer_Elapsed;
            mUITimer.AutoReset = true;
            mUITimer.Enabled = true;
            CenterForm();
            var brand = AppMangerTool.mBrandLst.Where(p => p.IsCurent == 1).FirstOrDefault();
            if (brand != null)
            {
                btnBrand.Text = brand.BrandName;
            }
            else
            {
                btnBrand.Text = "无";
            }
            timer1.Start();
        }

        private void CenterForm()
        {
            Screen screen = Screen.FromControl(this);
            Rectangle workingArea = screen.WorkingArea;
            this.Location = new Point(
                (workingArea.Width - this.Width) / 2,
                (workingArea.Height - this.Height) / 2
            );
        }

        private void MUITimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            uiButton1.FillColor = Color.MediumSeaGreen;
            uiButton2.FillColor = Color.Maroon;
            mUITimer.Stop();
        }

        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            // 显示确认提示框
            if (UIMessageBox.ShowAsk("确定退出程序？整机将停止运行！", true, UIMessageDialogButtons.Ok))
            {
                //xop1080model.SaveSettings();
                uiMenuPanle.Visible = false;
                this.Close();
            }
        }

        private void uiSymbolButton3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("检测程序即将关闭程序及计算机", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //调用异步线程关机，确保程序先退出
            uiMenuPanle.Visible = false;
            Task.Run(() =>
            {
                System.Threading.Thread.Sleep(5000);
                Process.Start("shutdown.exe", "/s /t 0");
            });
            //SaveSettings();
            Application.Exit();
            this.Close();
        }

        private void xop1080_Load(object sender, EventArgs e)
        {
            //加载方案
            InitVibrating();
            ToneOp.Init();
            this.Activate();
            AutoLoad();
            xop1080sv.MCSetM(2, 1, 1);
            xop1080sv.chspeedM0(40);//强制改速度，防止电机未保存上次速度，启动时候无速度
                                    // 订阅打印完成事件
            PrintService.Instance.PrintCompleted += OnPrintCompleted;

            // 异步检查打印机连接
            _ = CheckPrinterAsync();
        }

        private void AutoLoad()
        {
            Invoke(new Action(() =>
            {
                List<string> Sols = ExtHandler.GetSols();
                uiComboBox1.Items.Clear();
                bool r = false;
                foreach (string sol in Sols)
                {
                    if (sol == ExtHandler.GetAutoLoadSolName())
                    {
                        r = true;
                    }
                    uiComboBox1.Items.Add(sol);
                }
                if (r)
                {
                    uiComboBox1.SelectedItem = ExtHandler.GetAutoLoadSolName();
                    ToneOp.getPlcData(uiComboBox1.SelectedItem.ToString());
                    ToneOp.Set_HW_Param();
                }
                else
                {
                    uiComboBox1.SelectedItem = null;
                }
            }));
        }
        private void InitVibrating()
        {
            Invoke(new Action(() =>
            {
                Config.LoadConfig();
                Config.SetCanvas(xop1080ui.imageControl);
                xop1080ui.logview.Start();
            }));
        }

        //启动按钮
        private void uiButton1_Click(object sender, EventArgs e)
        {
            try
            {
                bkstop=false;
                startexe();//分离启动逻辑，方便下位机调用 
            }
            catch (Exception ex)
            {
                LogNet.Error("启动失败！" + ex.ToString());
            }
        }
        public void startexe()
        { 
            try
                {
                    ERR = xop1080sv.getdata("ERR");
                    if (ERR == "0")
                    {
                        LogNet.Info("自动运行");                        
                        xop1080model.CleanData();//检测数据清零!!!
                        xop1080model.SetData(btnBrand.Text.ToString());//传入牌号
                        xop1080sv.MCSetM(6, 1, 1);//写入启动M0
                        xop1080sv.chdirM0(0);//强制正向                        
                        xop1080sv.startM0();//启动主电机
                        xop1080sv.MCSetM(0, 1, 1);//启动下位自动程序
                        List<BrandData> mBrandLst = new List<BrandData>();
                        uiComboBox1.Enabled = false;
                        btnSwitchBrand.Enabled = false;
                        var brand = AppMangerTool.mBrandLst.Where(p => p.IsCurent == 1).FirstOrDefault();
                        if (brand != null)
                        {
                            Dictionary<string, string> parameters = xop1080model.getBrandData(brand.ID);
                            ExtHandler.AddGlobalVar("检测粒径", parameters["粒径"].ToDouble());
                            ExtHandler.AddGlobalVar("粒径偏差", parameters["公差"].ToDouble());
                            ExtHandler.AddGlobalVar("圆度", parameters["圆度"].ToDouble());
                            ExtHandler.AddGlobalVar("色差阈值", parameters["色差"].ToDouble());
                            ExtHandler.AddGlobalVar("UI_L", parameters["L"].ToDouble());
                            ExtHandler.AddGlobalVar("UI_A", parameters["A"].ToDouble());
                            ExtHandler.AddGlobalVar("UI_B", parameters["B"].ToDouble());
                            ExtHandler.AddGlobalVar("轴差", parameters["轴差"].ToDouble());
                            ExtHandler.AddGlobalVar("标准差", parameters["标准差"].ToDouble());
                            LogNet.Info("设置检测参数成功");
                        }
                        //根据采集图片切换开关，启用和禁用流程，名字采集图保存
                        if (uiSwitch1.Active && uiComboBox1.Text != null)
                        {
                            ExtHandler.SetModEnable("内1", "采集图保存", true);
                            ExtHandler.SetModEnable("内2", "采集图保存", true);
                            ExtHandler.SetModEnable("内3", "采集图保存", true);
                            ExtHandler.SetModEnable("外1", "采集图保存", true);
                            ExtHandler.SetModEnable("外2", "采集图保存", true);
                            ExtHandler.SetModEnable("外3", "采集图保存", true);
                        }
                        else
                        {
                            ExtHandler.SetModEnable("内1", "采集图保存", false);
                            ExtHandler.SetModEnable("内2", "采集图保存", false);
                            ExtHandler.SetModEnable("内3", "采集图保存", false);
                            ExtHandler.SetModEnable("外1", "采集图保存", false);
                            ExtHandler.SetModEnable("外2", "采集图保存", false);
                            ExtHandler.SetModEnable("外3", "采集图保存", false);
                        }
                        //根据缺陷图切换开关，启用和禁用流程，名字？？？
                        if (uiSwitchSave.Active && uiComboBox1.Text != null)
                        {
                            ExtHandler.SetModEnable("内1", "缺陷图保存", true);
                            ExtHandler.SetModEnable("内2", "缺陷图保存", true);
                            ExtHandler.SetModEnable("内3", "缺陷图保存", true);
                            ExtHandler.SetModEnable("外1", "缺陷图保存", true);
                            ExtHandler.SetModEnable("外2", "缺陷图保存", true);
                            ExtHandler.SetModEnable("外3", "缺陷图保存", true);
                        }
                        else
                        {
                            ExtHandler.SetModEnable("内1", "缺陷图保存", false);
                            ExtHandler.SetModEnable("内2", "缺陷图保存", false);
                            ExtHandler.SetModEnable("内3", "缺陷图保存", false);
                            ExtHandler.SetModEnable("外1", "缺陷图保存", false);
                            ExtHandler.SetModEnable("外2", "缺陷图保存", false);
                            ExtHandler.SetModEnable("外3", "缺陷图保存", false);
                        }   
                    }                    
                    else
                    {
                        LogNet.Error("启动失败,排除故障后重启检测端");
                    }
                }
                catch
                {
                }       
        }
        //停止按钮
        private void uiButton2_Click(object sender, EventArgs e)
        {
            bkstart=false;
            stopexe();//独立停止方法，供中断用
            //LogNet.Info("内排盘数据");
            //for (int i = 199; i >= 0; i--)
            //{
            //    var (rowOut, number, states, enc, version, wid, isnull) = ToneOp.neibuffer.ReadRow(i);
            //    LogNet.Info(rowOut.ToString() + "-" + number.ToString() + "-" + $"[{string.Join(",", states)}]" + "-" + enc.ToString() + "-" + version + "-" + isnull);
            //}
            //LogNet.Info("外排盘数据");
            //for (int i = 224; i >= 0; i--)
            //{
            //    var (rowOut, number, states, enc, version, wid, isnull) = ToneOp.waibuffer.ReadRow(i);
            //    LogNet.Info(rowOut.ToString() + "-" + number.ToString() + "-" + $"[{string.Join(",", states)}]" + "-" + enc.ToString() + "-" + version + "-" + wid + isnull);
            //}
        }
        public void stopexe()
        {
            //RunOnUiThread(() =>
            //{
                //// 确保在UI线程执行（统一线程环境）
                //    if (this.InvokeRequired)
                //{
                //    this.Invoke(new Action(stopexe));
                //    return;
                //}
                // 增加状态校验：若已停止，直接返回               
                //xop1080model.plcclear();
                xop1080sv.MCSetM(6, 1, 0);//停止复位启动M0
                xop1080sv.stopM0();//停止主电机
                Thread.Sleep(100);
                LogNet.Info("停止检测");
                uiComboBox1.Enabled = true;
                btnSwitchBrand.Enabled = true;
                xop1080model.SVdata();//存储检测数据到数据库
                                      //ExtHandler.StopRunSol();               
                //停止自动打印
                //ToneOp.Destroy();
                ExtHandler.StopRunSol();
                string 自动打印 = ExtHandler.GetGlobalVar<String>("停机打印");
                bool 急停 = false;
                if (xop1080sv.MCGetM(1) == "1")
                {
                    急停 = true;
                }
                ERR = xop1080sv.getdata("ERR");
                if (ERR == "1")
                {
                    LogNet.Error("急停停机，请排除故障后重启软件");
                }
                if (自动打印 == "true" && !急停 && ERR != "1")
                {
                    PrintService.Instance.PrintFromGlobalVarsAsync();
                }
                ToneOp.passI = 0;
                ToneOp.passO = 0;
                ToneOp.passII = false;
                ToneOp.passOO = false;
                //ToneOp.BlowStop();
        //});
        }

        private void xop1080_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.ShowWaitForm("正在关闭程序");
            xop1080sv.MCSetM(0, 100, 0);
            xop1080sv.stopM0();
            //xop1080model.plcclear();
            xop1080sv.Close();//关闭串口
            ToneOp.Destroy();
            timer1.Stop();
            this.HideWaitForm();
        }
        //定时器1
        int temp = 0;
        double totalSpeed = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ExtHandler.IsLoad)
            {
                xop1080model.getvars();
                xop1080ui.redatashow();
                string stop = xop1080sv.MCGetM(5);//读取下位机故障，用于停机
                if (stop == "1")
                {
                    this.stopexe();
                    //uiButton2.PerformClick();
                    // 在工作线程里
                    //this.Invoke(new Action(() => uiButton2.PerformClick()));
                    //this.BeginInvoke(new Action(() => uiButton2.PerformClick()));
                    LogNet.Error("下位停机");
                }
                if (bkstart == true)
                {
                    this.startexe();
                    //uiButton1.PerformClick();
                    // 在工作线程里
                    //this.Invoke(new Action(() => uiButton1.PerformClick()));
                    //this.BeginInvoke(new Action(() => uiButton1.PerformClick()));
                    bkstart = false;
                }
                if (bkstop == true)
                {
                    this.stopexe();
                    //uiButton2.PerformClick();
                    // 在工作线程里
                    //this.Invoke(new Action(() => uiButton2.PerformClick()));
                    //this.BeginInvoke(new Action(() => uiButton2.PerformClick()));
                    bkstop = false;
                }
                try
                {
                    int passed = ExtHandler.GetGlobalVar<Int32>("通过");
                    int rejected = ExtHandler.GetGlobalVar<Int32>("NgCount");
                    passed = Math.Abs(passed);
                    rejected = Math.Abs(rejected);

                    // 3. 良率计算核心逻辑
                    int validPassed = Math.Max(passed - rejected, 0);
                    double percentage = 0;

                    // 计算百分比，避免除以零
                    if (passed > 0)
                    {
                        percentage = (validPassed / (double)passed) * 100; // 避免整数除法
                        percentage = Math.Round(percentage, 2); // 保留两位小数

                        uiDigitalLabel1.Value = validPassed;
                        uiDigitalLabel2.Value = rejected;
                        uiDigitalLabel3.Value = percentage;

                    }
                    else
                    {
                        uiDigitalLabel3.Value = 0;
                    }
                    //车速
                    totalSpeed = totalSpeed + ToneOp.Mspeed * 0.34;

                    if (temp == 19)
                    {
                        // 计算平均值并格式化为两位小数
                        double averageSpeed = totalSpeed / 20;
                        string speedText = $"{averageSpeed:F2} 万/小时";
                        uLabelSeep.Text = speedText;
                        temp = -1;
                        totalSpeed = 0;
                    }
                    temp++;
                    uiLabel13.Text = ExtHandler.GetGlobalVar<double>("粒径均值").ToString();
                    uiLabel17.Text = ExtHandler.GetGlobalVar<double>("粒径最大值").ToString();
                    uiLabel22.Text = ExtHandler.GetGlobalVar<double>("粒径最小值").ToString();
                    uiLabel12.Text = ExtHandler.GetGlobalVar<double>("长轴均值").ToString();
                    uiLabel16.Text = ExtHandler.GetGlobalVar<double>("长轴最大值").ToString();
                    uiLabel21.Text = ExtHandler.GetGlobalVar<double>("长轴最小值").ToString();
                    uiLabel11.Text = ExtHandler.GetGlobalVar<double>("短轴均值").ToString();
                    uiLabel15.Text = ExtHandler.GetGlobalVar<double>("短轴最大值").ToString();
                    uiLabel20.Text = ExtHandler.GetGlobalVar<double>("短轴最小值").ToString();
                    uiLabel10.Text = ExtHandler.GetGlobalVar<double>("圆度均值").ToString();
                    uiLabel14.Text = ExtHandler.GetGlobalVar<double>("圆度最大值").ToString();
                    uiLabel19.Text = ExtHandler.GetGlobalVar<double>("圆度最小值").ToString();

                    uiLabel25.Text = ExtHandler.GetGlobalVar<Int32>("r_001").ToString();
                    uiLabel30.Text = ExtHandler.GetGlobalVar<Int32>("r_002").ToString();
                    uiLabel33.Text = ExtHandler.GetGlobalVar<Int32>("r_003").ToString();
                    uiLabel36.Text = ExtHandler.GetGlobalVar<Int32>("r_004").ToString();
                    uiLabel50.Text = ExtHandler.GetGlobalVar<Int32>("r_005").ToString();
                    uiLabel45.Text = ExtHandler.GetGlobalVar<Int32>("r_006").ToString();
                    uiLabel39.Text = ExtHandler.GetGlobalVar<Int32>("r_007").ToString();
                    uiLabel42.Text = ExtHandler.GetGlobalVar<Int32>("r_008").ToString();
                    uiLabel73.Text = ExtHandler.GetGlobalVar<Int32>("r_012").ToString();


                    uiLabel64.Text = ExtHandler.GetGlobalVar<Int32>("r_009").ToString();
                    uiLabel59.Text = ExtHandler.GetGlobalVar<Int32>("r_010").ToString();
                    uiLabel56.Text = ExtHandler.GetGlobalVar<Int32>("r_011").ToString();

                    double 缺陷总数 = ExtHandler.GetGlobalVar<Int32>("r_001")
                        + ExtHandler.GetGlobalVar<Int32>("r_002")
                        + ExtHandler.GetGlobalVar<Int32>("r_003")
                        + ExtHandler.GetGlobalVar<Int32>("r_004")
                        + ExtHandler.GetGlobalVar<Int32>("r_005")
                        + ExtHandler.GetGlobalVar<Int32>("r_006")
                        + ExtHandler.GetGlobalVar<Int32>("r_007")
                        + ExtHandler.GetGlobalVar<Int32>("r_008")
                        + ExtHandler.GetGlobalVar<Int32>("r_009")
                        + ExtHandler.GetGlobalVar<Int32>("r_010")
                        + ExtHandler.GetGlobalVar<Int32>("r_011")
                        + ExtHandler.GetGlobalVar<Int32>("r_012");

                    uiLabel28.Text = Math.Round((float)ExtHandler.GetGlobalVar<Int32>("r_001") / (float)缺陷总数 * 100, 2).ToString();
                    uiLabel29.Text = Math.Round((float)ExtHandler.GetGlobalVar<Int32>("r_002") / (float)缺陷总数 * 100, 2).ToString();
                    uiLabel32.Text = Math.Round((float)ExtHandler.GetGlobalVar<Int32>("r_003") / (float)缺陷总数 * 100, 2).ToString();
                    uiLabel35.Text = Math.Round((float)ExtHandler.GetGlobalVar<Int32>("r_004") / (float)缺陷总数 * 100, 2).ToString();
                    uiLabel47.Text = Math.Round((float)ExtHandler.GetGlobalVar<Int32>("r_005") / (float)缺陷总数 * 100, 2).ToString();
                    uiLabel44.Text = Math.Round((float)ExtHandler.GetGlobalVar<Int32>("r_006") / (float)缺陷总数 * 100, 2).ToString();
                    uiLabel38.Text = Math.Round((float)ExtHandler.GetGlobalVar<Int32>("r_007") / (float)缺陷总数 * 100, 2).ToString();
                    uiLabel41.Text = Math.Round((float)ExtHandler.GetGlobalVar<Int32>("r_008") / (float)缺陷总数 * 100, 2).ToString();
                    uiLabel61.Text = Math.Round((float)ExtHandler.GetGlobalVar<Int32>("r_009") / (float)缺陷总数 * 100, 2).ToString();
                    uiLabel58.Text = Math.Round((float)ExtHandler.GetGlobalVar<Int32>("r_010") / (float)缺陷总数 * 100, 2).ToString();
                    uiLabel55.Text = Math.Round((float)ExtHandler.GetGlobalVar<Int32>("r_011") / (float)缺陷总数 * 100, 2).ToString();
                    uiLabel72.Text = Math.Round((float)ExtHandler.GetGlobalVar<Int32>("r_012") / (float)缺陷总数 * 100, 2).ToString();

                    if (ToneOp.passI >= 200 && ToneOp.passO >= 225)
                    {
                        if (ExtHandler.GetGlobalVar<string>("空盘停机") == "true")
                        {
                            stopexe();//强制剔除一圈停止                    
                            LogNet.Info("空盘停机");
                        }
                    }
                }

                catch (Exception ex)
                {
                    LogNet.Error("剔除计数时发生错误: " + ex.ToString());
                }
            }                             
        }

        private int firstPage = -1;
        //写入切换后的标定系数，by崔
        public static double temScalei=0 ;
        public static double temScaleo=0;
        private void uiComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //写入切换后的标定系数，by崔，启动后防止程序运行标定，切换时再次读取最新的
            if (temScalei != 0 && temScaleo != 0)
            {
                temScalei = ExtHandler.GetGlobalVar<double>("Scalei");
                temScaleo = ExtHandler.GetGlobalVar<double>("Scaleo");
            }
            string SolName = (string)uiComboBox1.SelectedItem;
            if (!string.IsNullOrEmpty(SolName))
            {
                //方案之间传递标定系数，by崔
                ExtHandler.Load(SolName, new Action<bool>(r =>
                {
                    Invoke(new Action(() =>
                    {
                        if (!r)
                        {
                            uiComboBox1.SelectedItem = null;
                            this.ShowErrorDialog("型号加载失败");
                        }
                        else
                        {
                            //写入切换后的标定系数，by崔,判断!=0排除启动时候没有方案
                            if(temScalei!=0 && temScaleo != 0)
                            {
                                ExtHandler.AddGlobalVar("Scalei", temScalei);
                                ExtHandler.AddGlobalVar("Scaleo", temScaleo);
                                LogNet.Info("盘间标定数据同步成功，内：" + temScalei + "外：" + temScaleo);
                                ExtHandler.SaveSol();
                            }                            
                            //方案之间传递标定系数，by崔,方案加载成功后，读取标定值，用于换盘同步
                            temScalei = ExtHandler.GetGlobalVar<double>("Scalei");
                            temScaleo = ExtHandler.GetGlobalVar<double>("Scaleo");
                        }
                    }));
                }));
                ToneOp.getPlcData(SolName);
                ToneOp.Set_HW_Param();
            }

            AppMangerTool.plcIndex = uiComboBox1.SelectedIndex;
            if (firstPage >= 0)
            {
                int rowIndex = uiComboBox1.SelectedIndex;

                //AppMangerTool.writePlc(rowIndex);
            }
            else
            {
                firstPage = 0;
            }

        }

        private void btLogin_Click(object sender, EventArgs e)
        {
            if (btLogin.Text.Contains("退出"))
            {
                AppMangerTool.curIndex = -1;
                btLogin.Text = "未登陆";

            }
        }

        private void BtnNvg_Click(object sender, EventArgs e)
        {
            uiMenuPanle.Visible = true;
        }

        private void SbtnClose_Click(object sender, EventArgs e)
        {
            uiMenuPanle.Visible = false;
        }

        private void BtnSysManger_Click(object sender, EventArgs e)
        {
            uiMenuPanle.Visible = false;

            if (AppMangerTool.curIndex < 0)
            {
                btLogin.Text = "未登陆";
                UcLogin ucLogin = new UcLogin();
                if (ucLogin.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                btLogin.Text = AppMangerTool.mSysUses[AppMangerTool.curIndex].userName + "[退出]";
                settings _form1 = new settings();
                //_form1.Parent = this;
                _form1.ShowDialog();
            }
            else
            {
                btLogin.Text = AppMangerTool.mSysUses[AppMangerTool.curIndex].userName + "[退出]";
                settings _form1 = new settings();
                // _form1.Parent = this;
                _form1.ShowDialog();
            }

        }

        private void btnSwitchBrand_Click(object sender, EventArgs e)
        {
            try
            {
                FrmBranLst frm = new FrmBranLst();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    btnBrand.Text = frm.mBrandData.BrandName;
                    xop1080m brandParameters = new xop1080m();
                    Dictionary<string, string> parameters = brandParameters.getBrandData(frm.mBrandData.ID);
                    ExtHandler.AddGlobalVar("检测粒径", parameters["粒径"].ToDouble());
                    ExtHandler.AddGlobalVar("粒径偏差", parameters["公差"].ToDouble());
                    ExtHandler.AddGlobalVar("圆度", parameters["圆度"].ToDouble());
                    ExtHandler.AddGlobalVar("色差阈值", parameters["色差"].ToDouble());
                    ExtHandler.AddGlobalVar("UI_L", parameters["L"].ToDouble());
                    ExtHandler.AddGlobalVar("UI_A", parameters["A"].ToDouble());
                    ExtHandler.AddGlobalVar("UI_B", parameters["B"].ToDouble());
                    ExtHandler.AddGlobalVar("轴差", parameters["轴差"].ToDouble());
                    ExtHandler.AddGlobalVar("标准差", parameters["标准差"].ToDouble());
                    LogNet.Info("设置检测参数成功");
                }
                
            }
            catch (Exception ex)
            {
                LogNet.Error("设置品牌参数失败！" + ex.ToString());
            }

        }

        /// <summary>
        /// 缺陷图开关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="value"></param>
        private void uiSwitchSave_ValueChanged(object sender, bool value)
        {
            //获取配置对象
            //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (uiSwitchSave.Active)
            {
                ExtHandler.SetModEnable("内1", "缺陷图保存", true);
                ExtHandler.SetModEnable("内2", "缺陷图保存", true);
                ExtHandler.SetModEnable("内3", "缺陷图保存", true);
                ExtHandler.SetModEnable("外1", "缺陷图保存", true);
                ExtHandler.SetModEnable("外2", "缺陷图保存", true);
                ExtHandler.SetModEnable("外3", "缺陷图保存", true);
            }
            else
            {
                ExtHandler.SetModEnable("内1", "缺陷图保存", false);
                ExtHandler.SetModEnable("内2", "缺陷图保存", false);
                ExtHandler.SetModEnable("内3", "缺陷图保存", false);
                ExtHandler.SetModEnable("外1", "缺陷图保存", false);
                ExtHandler.SetModEnable("外2", "缺陷图保存", false);
                ExtHandler.SetModEnable("外3", "缺陷图保存", false);
            }
            //修改键值对
            //config.AppSettings.Settings["isSaveDef"].Value = VisionCore.Tools.ToolManger.mSaveDefImg.ToString();

            //保存并刷新
            //config.Save(ConfigurationSaveMode.Modified);
            //ConfigurationManager.RefreshSection("appSettings");

        }

        /// <summary>
        /// 获取当前盘号
        /// </summary>
        /// <returns></returns>
        //public static string GetDialNumber()
        //{
        //    string dialNumber = uiComboBox1.Text;
        //    return dialNumber;
        //}

        private void uiSwitch1_ValueChanged(object sender, bool value)
        {
            if (uiSwitch1.Active)
            {
                ExtHandler.SetModEnable("内1", "采集图保存", true);
                ExtHandler.SetModEnable("内2", "采集图保存", true);
                ExtHandler.SetModEnable("内3", "采集图保存", true);
                ExtHandler.SetModEnable("外1", "采集图保存", true);
                ExtHandler.SetModEnable("外2", "采集图保存", true);
                ExtHandler.SetModEnable("外3", "采集图保存", true);
            }
            else
            {
                ExtHandler.SetModEnable("内1", "采集图保存", false);
                ExtHandler.SetModEnable("内2", "采集图保存", false);
                ExtHandler.SetModEnable("内3", "采集图保存", false);
                ExtHandler.SetModEnable("外1", "采集图保存", false);
                ExtHandler.SetModEnable("外2", "采集图保存", false);
                ExtHandler.SetModEnable("外3", "采集图保存", false);
            }
        }


        //**********************打印相关**********************
        /// <summary>
        /// 异步检查打印机
        /// </summary>
        private async Task CheckPrinterAsync()
        {
            bool connected = await PrintService.Instance.CheckConnectionAsync();
            if (!connected)
            {
                LogNet.Warn("打印机未连接，打印功能不可用");
            }
        }

        /// <summary>
        /// 打印完成事件处理
        /// </summary>
        private void OnPrintCompleted(object sender, PrintResult result)
        {
            if (result.Success)
            {
                LogNet.Info("打印任务完成");
            }
            else
            {
                LogNet.Error($"打印任务失败: {result.ErrorMessage}");
            }
        }
    }
}
