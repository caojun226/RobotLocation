using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using LevelDB.NativePointer;
using RobotLocation.Model;
using RobotLocation.Service;
using Sunny.UI;
using Sunny.UI.Win32;
using VisionCore.Communication;
using VisionCore.Ext;
using VisionCore.Frm;
using VisionCore.Log;
using VisionCore.Tools;
using VisionCore.Deep;

namespace RobotLocation.UI
{
    public partial class settings : UIForm
    {
        //链接数据库
        private string connectionString = "Data Source=data.db;Version=3;";
        private SQLiteConnection connection;
        private xop1080m xop1080model = new xop1080m();
        private xop1080u xop1080ui = new xop1080u();
        //private xop1080sv xop1080Sv = new xop1080sv();
        private List<TabPage> uiMenu = new List<TabPage>();
        List<CheckItem> itemsLst = new List<CheckItem>();
        List<UserFunction> mFunLst = new List<UserFunction>();
        int mUIndex = 0;
        private DataGridViewRow _currentPrintRow = null;//存储选中行的字段

        public static settings removeparas { get; set; }
        public settings()
        {
            InitializeComponent();
            uiDatePicker1.Value = DateTime.Now; 
        }
             

        //窗口加载
        private void settings_Load(object sender, EventArgs e)
        {           
            mFunLst = (new SVUserFunction()).getlst();
            mBrandLst = (new SVBrandData()).getlst();
            foreach (var item in AppMangerTool.mSysUses)
            {
                uiUsersLst.Items.Add(item.userName);
            }
            for (int i = 0; i < uiTabControlMenu.TabPages.Count; i++)
            {
                TabPage menuItem = uiTabControlMenu.TabPages[i];
                CheckItem chkItem = new CheckItem();
                chkItem.Text = menuItem.Text;
                itemsLst.Add(chkItem);
            }
            if (AppMangerTool.curIndex != 0)
            {
                string userName = AppMangerTool.mSysUses[AppMangerTool.curIndex].userName;
                List<TabPage> lstUI = new List<TabPage>();
                for (int i = 0; i < uiTabControlMenu.TabPages.Count; i++)
                {
                    TabPage menuItem = uiTabControlMenu.TabPages[i];
                    if (mFunLst.Where(p => p.uName == userName && p.fName == menuItem.Text).Count() == 0)
                    {
                        lstUI.Add(menuItem);
                    }
                }
                for (int i = 0; i < lstUI.Count(); i++)
                {
                    lstUI[i].Parent = null;
                }
            }
            if (AppMangerTool.curIndex != 0)
            {
                string userName = AppMangerTool.mSysUses[AppMangerTool.curIndex].userName;

                for (int i = 0; i < uiMenu.Count; i++)
                {
                    if (mFunLst.Where(p => p.fName == uiMenu[i].Text && p.uName == userName).Count() > 0)
                    {
                        uiTabControlMenu.TabPages.Add(uiMenu[i]);
                    }
                }
            }

            List<string> Sols = ExtHandler.GetSols();
            uiComboBox.Items.Clear();
            foreach (string sol in Sols)
            {
                uiComboBox.Items.Add(sol);
            }

            uiRadioButton1.Checked = true;
            uiRadioButton2.Checked = true;
            uiRadioButton3.Checked = true;
            uiRadioButton4.Checked = true;
            uiRadioButton5.Checked = true;
            loaddata();
            uiUsersLst.SelectedIndex = 0;
            
            DebugSwitch();
            xop1080m.LoadSettings();//加载电机转速参数，功能未实现！！
            //自动打印数据
            if (ExtHandler.IsLoad)
            {
                loadPlcPro();
                if (ExtHandler.GetGlobalVar<string>("停机打印") == "true")
                {
                    uiSwitch1.Active = true;
                }
                else
                {
                    uiSwitch1.Active = false;
                }
                if (ExtHandler.GetGlobalVar<string>("空盘清理") == "true")
                {
                    uiSwitch9.Active = true;
                }
                else
                {
                    uiSwitch9.Active = false;
                }
                if (ExtHandler.GetGlobalVar<string>("启动强清") == "true")
                {
                    uiSwitch10.Active = true;
                }
                else
                {
                    uiSwitch10.Active = false;
                }
                if (ExtHandler.GetGlobalVar<string>("空盘停机") == "true")
                {
                    uiSwitch11.Active = true;
                }
                else
                {
                    uiSwitch11.Active = false;
                }
                txtBlowrow.Text = ExtHandler.GetGlobalVar<int>("吹气排数").ToString();
                txtBlowlap.Text = ExtHandler.GetGlobalVar<int>("吹气圈数").ToString();
                ReadDeepText();
            }
        }
        //关闭
        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            this.Close();
            xop1080sv.MCSetM(10, 30, 0);//清除手动运行状态
        }
        //通用设置
        private void uiSymbolButton4_Click(object sender, EventArgs e)
        {
            FrmTool.ShowDialog<CommonSetFrm>();
        }

        private void uiComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string SolName = (string)uiComboBox.SelectedItem;
            if (!string.IsNullOrEmpty(SolName))
            {
                ExtHandler.Load(SolName, new Action<bool>(r =>
                {
                    Invoke(new Action(() =>
                    {
                        if (!r)
                        {
                            uiComboBox.SelectedItem = null;
                            this.ShowErrorDialog("型号加载失败");
                        }
                        else
                        {
                            //this.Text = "XOP1080 视觉选丸仪-当前牌号:" + 当前算法; // 去掉标题栏文字
                        }
                    }));
                }));
            }
        }

        private void uiSymbolButton6_Click(object sender, EventArgs e)
        {
            ExtHandler.ShowExFrm();
        }
        //克隆方案
        private void uiSymbolButton5_Click(object sender, EventArgs e)
        {
            if (uiComboBox.SelectedIndex != -1)
            {
                ExtHandler.AddSol(new Action<bool>(r =>
                {
                    if (r)
                    {
                        List<string> Sols = ExtHandler.GetSols();
                        //
                        Invoke(new Action(() =>
                        {
                            uiComboBox.Items.Clear();
                            foreach (string sol in Sols)
                            {
                                if (sol == ExtHandler.GetAutoLoadSolName())
                                {
                                    r = true;
                                }
                                uiComboBox.Items.Add(sol);
                            }
                        }));
                    }
                }));
            }

        }

        private void uiSymbolButton7_Click(object sender, EventArgs e)
        {
            ExtHandler.SaveSol();
        }
        //默认参数设置
        private void setuisettings(int bID)
        {
            try
            {
               
                Dictionary<string, string> settings = xop1080model.getBrandData(bID);
                if (settings.Count() > 0)
                {

                    this.uiNumPadTextBox19.Text = settings["粒径"];
                    this.uiNumPadTextBox20.Text = settings["公差"];
                    this.uiNumPadTextBox21.Text = settings["圆度"];
                    this.uiNumPadTextBox22.Text = settings["色差"];
                    this.uiNumPadTextBox23.Text = settings["L"];
                    this.uiNumPadTextBox24.Text = settings["A"];
                    this.uiNumPadTextBox25.Text = settings["B"];
                    this.uiNumPadTextBox31.Text = settings["轴差"];
                    this.uiNumPadTextBox32.Text = settings["标准差"];
                    this.uiNumPadTextBox26.Text = settings["主转速"];
                    this.uiNumPadTextBox27.Text = settings["皮带转速"];
                    this.uiNumPadTextBox28.Text = settings["拨珠1转速"];
                    this.uiNumPadTextBox29.Text = settings["拨珠2转速"];
                    this.uiNumPadTextBox30.Text = settings["搅动转速"];

                    //if (ExtHandler.IsLoad)
                    //{
                    //    ExtHandler.AddGlobalVar("检测粒径", settings["粒径"].ToDouble());
                    //    ExtHandler.AddGlobalVar("粒径偏差", settings["公差"].ToDouble());
                    //    ExtHandler.AddGlobalVar("圆度", settings["圆度"].ToDouble());
                    //    ExtHandler.AddGlobalVar("色差阈值", settings["色差"].ToDouble());
                    //    ExtHandler.AddGlobalVar("UI_L", settings["L"].ToDouble());
                    //    ExtHandler.AddGlobalVar("UI_A", settings["A"].ToDouble());
                    //    ExtHandler.AddGlobalVar("UI_B", settings["B"].ToDouble());
                    //    ExtHandler.AddGlobalVar("轴差", settings["轴差"].ToDouble());
                    //    ExtHandler.AddGlobalVar("标准差", settings["标准差"].ToDouble());
                    //    ExtHandler.SaveSol();
                    //}
                    
                }
                else
                {
                    this.uiNumPadTextBox19.Text = "3.0";
                    this.uiNumPadTextBox20.Text = "0.1";
                    this.uiNumPadTextBox21.Text = "0.9";
                    this.uiNumPadTextBox22.Text = "10";
                    this.uiNumPadTextBox23.Text = "128";
                    this.uiNumPadTextBox24.Text = "128";
                    this.uiNumPadTextBox25.Text = "128";
                    this.uiNumPadTextBox31.Text = "0";
                    this.uiNumPadTextBox32.Text = "0";
                    this.uiNumPadTextBox26.Text = "40";
                    this.uiNumPadTextBox27.Text = "20";
                    this.uiNumPadTextBox28.Text = "20";
                    this.uiNumPadTextBox29.Text = "20";
                    this.uiNumPadTextBox30.Text = "400";
                }

            }
            catch (Exception ex)
            {

                LogNet.Error("换牌失败 ！"+ ex.ToString());
            }
            

        }
        private void uiSymbolButton14_Click(object sender, EventArgs e)
        {
            try
            {
                int bId = mBrandLst[uBrandLst.SelectedIndex].ID;
                Dictionary<string, string> settings = xop1080model.getBrandData(bId);
                if (settings.Count() > 0)
                {

                    ExtHandler.AddGlobalVar("检测粒径", settings["粒径"].ToDouble());
                    ExtHandler.AddGlobalVar("粒径偏差", settings["公差"].ToDouble());
                    ExtHandler.AddGlobalVar("圆度", settings["圆度"].ToDouble());
                    ExtHandler.AddGlobalVar("色差阈值", settings["色差"].ToDouble());
                    ExtHandler.AddGlobalVar("UI_L", settings["L"].ToDouble());
                    ExtHandler.AddGlobalVar("UI_A", settings["A"].ToDouble());
                    ExtHandler.AddGlobalVar("UI_B", settings["B"].ToDouble());
                    ExtHandler.AddGlobalVar("轴差", settings["轴差"].ToDouble());
                    ExtHandler.AddGlobalVar("标准差", settings["标准差"].ToDouble());
                    LogNet.Info("设置检测参数成功");
                }
                ExtHandler.SaveSol();
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
                    double L = uiNumPadTextBox23.Text.ToDouble() - 128.0;
                    double A = uiNumPadTextBox24.Text.ToDouble() - 128.0;
                    double B = uiNumPadTextBox25.Text.ToDouble() - 128.0;
                    Color labColor = RobotLocation.Model.xop1080m.LabToRgb(L, A, B);

                    // 步骤4: 绘制圆形
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.FillEllipse(new SolidBrush(labColor), center.X, center.Y, diameter, diameter);
                }

                // 步骤5: 显示结果
                pictureBox1.Image = bmp;

            }
            catch (Exception ex)
            {

                LogNet.Error("保存参数错误！"+ ex.ToString());
            }
        }
        //读取颜色均值
        private void uiSymbolButton15_Click(object sender, EventArgs e)
        {
            if (ExtHandler.GetGlobalVar<double>("r_L") != 0)
            {
                uiNumPadTextBox23.Text = ExtHandler.GetGlobalVar<double>("r_L").ToString();
            }
            if (ExtHandler.GetGlobalVar<double>("r_A") != 0)
            {
                uiNumPadTextBox24.Text = ExtHandler.GetGlobalVar<double>("r_A").ToString();
            }
            if (ExtHandler.GetGlobalVar<double>("r_B") != 0)
            {
                uiNumPadTextBox25.Text = ExtHandler.GetGlobalVar<double>("r_B").ToString();
            }
        }
        //选择日期筛选
        private void uiDatePicker1_ValueChanged(object sender, DateTime value)
        {
            // 获取 uiDatePicker 中选择的日期
            DateTime selectedDate = uiDatePicker1.Value;
            string dateString = selectedDate.ToString("yyyy-MM-dd");
            // 查询数据库
            //string query = $"SELECT ID, 日期, 通过,剔除,合格率, 粒径, 圆度值 FROM 结果 WHERE 日期 = '{dateString}'";
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
        //设置检测参数到PLC
        private void uiSymbolButton10_Click(object sender, EventArgs e)
        {
            plcData itemData = GetPlcData();
            (new SVPlcData()).updData(itemData);
            //if (AppMangerTool.plcIndex != ulstPLC.SelectedIndex)
            //{
            //    return;
            //}
            ToneOp.getPlcData(itemData.proName);
            ToneOp.Set_HW_Param();

        }
        //速度下发plc，使用了默认值
        private void uiSymbolButton21_Click(object sender, EventArgs e)
        {
            xop1080sv.setdata("SPEED1", uiNumPadTextBox27.Text.ToFloat()*60);
            xop1080sv.setdata("SPEED2", uiNumPadTextBox30.Text.ToFloat()*60);
            xop1080sv.setdata("SPEED3", uiNumPadTextBox28.Text.ToFloat() *60);
            xop1080sv.setdata("SPEED4", uiNumPadTextBox29.Text.ToFloat()*60);
            xop1080sv.chspeedM0(uiNumPadTextBox26.Text.ToUShort());
            //存入数据库
            mBrandID = mBrandLst[uBrandLst.SelectedIndex].ID;
            var settings = new List<(string Key, string Value)>
            {
                ("粒径", uiNumPadTextBox19.Text),
                ("公差", uiNumPadTextBox20.Text),
                ("圆度", uiNumPadTextBox21.Text),
                ("色差", uiNumPadTextBox22.Text),
                ("L", uiNumPadTextBox23.Text),
                ("A", uiNumPadTextBox24.Text),
                ("B", uiNumPadTextBox25.Text),
                ("长短抽差", uiNumPadTextBox31.Text),
                ("标准差", uiNumPadTextBox32.Text),
                 ("主转速", uiNumPadTextBox26.Text),
                 ("皮带转速", uiNumPadTextBox27.Text),
                 ("拨珠1转速", uiNumPadTextBox28.Text),
                 ("拨珠2转速", uiNumPadTextBox29.Text),
                 ("搅动转速", uiNumPadTextBox30.Text),
            };
            (new SVBrandData()).DeletAppPrgm(mBrandID);
            xop1080model.SVsettings(settings, mBrandID);
            LogNet.Info("参数设置保存成功，主转盘速度重启后生效");
        }
        //单独启停电机
        private void uiSymbolButton16_Click(object sender, EventArgs e)
        {
            var button = sender as Sunny.UI.UISymbolButton;
            if (button == null) return; // 确保按钮对象不为空

            bool isStart = button.Tag?.ToString() == "false"; // 从 Tag 中读取当前状态            
            // 更新按钮状态
            if (isStart)
            {
                button.Text = "停止";
                button.Symbol = 61517;
                button.Style = UIStyle.Red;
                button.BackColor = Color.Green;
                uiRadioButton1.Enabled = false;
                uiRadioButton6.Enabled = false;
                if(uiRadioButton6.Checked)
                {
                    xop1080sv.chdirM0(1);
                    xop1080sv.chspeedM0(uiNumPadTextBox26.Text.ToUShort());
                }
                else
                {
                    xop1080sv.chdirM0(0);
                    xop1080sv.chspeedM0(uiNumPadTextBox26.Text.ToUShort());
                }

               
            }
            else
            {
                button.Text = "启动";
                button.Symbol = 361515;
                button.Style = UIStyle.Blue;
                button.BackColor = Color.Blue;
                uiRadioButton1.Enabled = true;
                uiRadioButton6.Enabled = true;
                xop1080sv.stopM0();
            }

            // 调用 runmt 方法，传入 mtid 和当前状态
            //xop1080ui.runmt(28, isStart, uiRadioButton6.Checked);

            // 更新按钮的状态
            button.Tag = isStart ? "true" : "false"; // 更新为字符串       
        }

        private void uiSymbolButton17_Click(object sender, EventArgs e)
        {
            var button = sender as Sunny.UI.UISymbolButton;
            if (button == null) return; // 确保按钮对象不为空

            bool isStart = button.Tag?.ToString() == "false"; // 从 Tag 中读取当前状态

            // 更新按钮状态
            if (isStart)
            {
                button.Text = "停止";
                button.Symbol = 61517;
                button.Style = UIStyle.Red;
                button.BackColor = Color.Green;
                uiRadioButton2.Enabled = false;
                uiRadioButton7.Enabled = false;
            }
            else
            {
                button.Text = "启动";
                button.Symbol = 361515;
                button.Style = UIStyle.Blue;
                button.BackColor = Color.Blue;
                uiRadioButton2.Enabled = true;
                uiRadioButton7.Enabled = true;
            }

            // 调用 runmt 方法，传入 mtid 和当前状态
            xop1080ui.runmt(20, isStart, uiRadioButton7.Checked);

            // 更新按钮的状态
            button.Tag = isStart ? "true" : "false"; // 更新为字符串
        }

        private void uiSymbolButton18_Click(object sender, EventArgs e)
        {
            var button = sender as Sunny.UI.UISymbolButton;
            if (button == null) return; // 确保按钮对象不为空

            bool isStart = button.Tag?.ToString() == "false"; // 从 Tag 中读取当前状态

            // 更新按钮状态
            if (isStart)
            {
                button.Text = "停止";
                button.Symbol = 61517;
                button.Style = UIStyle.Red;
                button.BackColor = Color.Green;
                uiRadioButton3.Enabled = false;
                uiRadioButton8.Enabled = false;
            }
            else
            {
                button.Text = "启动";
                button.Symbol = 361515;
                button.Style = UIStyle.Blue;
                button.BackColor = Color.Blue;
                uiRadioButton3.Enabled = true;
                uiRadioButton8.Enabled = true;
            }

            // 调用 runmt 方法，传入 mtid 和当前状态
            xop1080ui.runmt(24, isStart, uiRadioButton8.Checked);

            // 更新按钮的状态
            button.Tag = isStart ? "true" : "false"; // 更新为字符串
        }

        private void uiSymbolButton19_Click(object sender, EventArgs e)
        {
            var button = sender as Sunny.UI.UISymbolButton;
            if (button == null) return; // 确保按钮对象不为空

            bool isStart = button.Tag?.ToString() == "false"; // 从 Tag 中读取当前状态

            // 更新按钮状态
            if (isStart)
            {
                button.Text = "停止";
                button.Symbol = 61517;
                button.Style = UIStyle.Red;
                button.BackColor = Color.Green;
                uiRadioButton4.Enabled = false;
                uiRadioButton9.Enabled = false;
            }
            else
            {
                button.Text = "启动";
                button.Symbol = 361515;
                button.Style = UIStyle.Blue;
                button.BackColor = Color.Blue;
                uiRadioButton4.Enabled = true;
                uiRadioButton9.Enabled = true;
            }

            // 调用 runmt 方法，传入 mtid 和当前状态
            xop1080ui.runmt(26, isStart, uiRadioButton9.Checked);

            // 更新按钮的状态
            button.Tag = isStart ? "true" : "false"; // 更新为字符串
        }

        private void uiSymbolButton20_Click(object sender, EventArgs e)
        {
            var button = sender as Sunny.UI.UISymbolButton;
            if (button == null) return; // 确保按钮对象不为空

            bool isStart = button.Tag?.ToString() == "false"; // 从 Tag 中读取当前状态

            // 更新按钮状态
            if (isStart)
            {
                button.Text = "停止";
                button.Symbol = 61517;
                button.Style = UIStyle.Red;
                button.BackColor = Color.Green;
                uiRadioButton5.Enabled = false;
                uiRadioButton10.Enabled = false;
            }
            else
            {
                button.Text = "启动";
                button.Symbol = 361515;
                button.Style = UIStyle.Blue;
                button.BackColor = Color.Blue;
                uiRadioButton5.Enabled = true;
                uiRadioButton10.Enabled = true;
            }

            // 调用 runmt 方法，传入 mtid 和当前状态
            xop1080ui.runmt(22, isStart, uiRadioButton10.Checked);

            // 更新按钮的状态
            button.Tag = isStart ? "true" : "false"; // 更新为字符串
        }
        //单独调用打印按钮，需要传入打印的数据
        private void uiSymbolButton9_Click(object sender, EventArgs e)
        {
            //try
            //{
                if (_currentPrintRow == null)
                {
                    UIMessageBox.ShowWarning("请先选中一行数据！");
                    return;
                }
                // 把选中行同步到全局变量
                SaveRowToGlobals(_currentPrintRow);
                // 打印
               xop1080m.PT_res();
            //}
            //catch
            //{
            //    LogNet.Error("打印失败");
            //}

        }
        int mBrandID = 0;
        //选中数据存入全局变量
        private void SaveRowToGlobals(DataGridViewRow r)
        {

            //日期
            string Time = Convert.ToString(r.Cells["日期"].Value ?? "N/A") + "    " + Convert.ToString(r.Cells["时间"].Value ?? "N/A");
            ExtHandler.AddGlobalVar("日期", Time);
            //计数
            ExtHandler.AddGlobalVar("合格", Convert.ToInt32(r.Cells["合格"].Value ?? 0));
            ExtHandler.AddGlobalVar("剔除", Convert.ToInt32(r.Cells["剔除"].Value ?? 0));
            ExtHandler.AddGlobalVar("合格率", Convert.ToDouble(r.Cells["合格率"].Value ?? 0));

            // 粒径
            ExtHandler.AddGlobalVar("粒径均值", Convert.ToDouble(r.Cells["粒径"].Value ?? 0));
            ExtHandler.AddGlobalVar("粒径最大值", Convert.ToDouble(r.Cells["粒径MA"].Value ?? 0));
            ExtHandler.AddGlobalVar("粒径最小值", Convert.ToDouble(r.Cells["粒径MIN"].Value ?? 0));

            // 圆度
            ExtHandler.AddGlobalVar("圆度均值", Convert.ToDouble(r.Cells["圆度值"].Value ?? 0));
            ExtHandler.AddGlobalVar("圆度最大值", Convert.ToDouble(r.Cells["圆度MA"].Value ?? 0));
            ExtHandler.AddGlobalVar("圆度最小值", Convert.ToDouble(r.Cells["圆度MIN"].Value ?? 0));

            // 方差
            ExtHandler.AddGlobalVar("方差均值", Convert.ToDouble(r.Cells["标准差"].Value ?? 0));
            ExtHandler.AddGlobalVar("方差最大值", Convert.ToDouble(r.Cells["标准差MA"].Value ?? 0));
            ExtHandler.AddGlobalVar("方差最小值", Convert.ToDouble(r.Cells["标准差MIN"].Value ?? 0));

            // 长/短轴
            ExtHandler.AddGlobalVar("长轴均值", Convert.ToDouble(r.Cells["长轴"].Value ?? 0));
            ExtHandler.AddGlobalVar("长轴最大值", Convert.ToDouble(r.Cells["长轴MA"].Value ?? 0));
            ExtHandler.AddGlobalVar("长轴最小值", Convert.ToDouble(r.Cells["长轴MIN"].Value ?? 0));

            ExtHandler.AddGlobalVar("短轴均值", Convert.ToDouble(r.Cells["短轴"].Value ?? 0));
            ExtHandler.AddGlobalVar("短轴最大值", Convert.ToDouble(r.Cells["短轴MA"].Value ?? 0));
            ExtHandler.AddGlobalVar("短轴最小值", Convert.ToDouble(r.Cells["短轴MIN"].Value ?? 0));

            // 检测设置
            ExtHandler.AddGlobalVar("检测粒径", Convert.ToDouble(r.Cells["检测直径"].Value ?? 0));
            ExtHandler.AddGlobalVar("粒径偏差", Convert.ToDouble(r.Cells["检测偏差"].Value ?? 0));
            ExtHandler.AddGlobalVar("圆度", Convert.ToDouble(r.Cells["检测圆度"].Value ?? 0));
            ExtHandler.AddGlobalVar("色差阈值", Convert.ToDouble(r.Cells["色差阈值"].Value ?? 0));

            ExtHandler.AddGlobalVar("r_001", Convert.ToInt32(r.Cells["皮帽"].Value ?? 0));
            ExtHandler.AddGlobalVar("r_002", Convert.ToInt32(r.Cells["脏污"].Value ?? 0));
            ExtHandler.AddGlobalVar("r_003", Convert.ToInt32(r.Cells["异色"].Value ?? 0));
            ExtHandler.AddGlobalVar("r_004", Convert.ToInt32(r.Cells["凹陷"].Value ?? 0));
            ExtHandler.AddGlobalVar("r_005", Convert.ToInt32(r.Cells["凸点"].Value ?? 0));
            ExtHandler.AddGlobalVar("r_006", Convert.ToInt32(r.Cells["气泡"].Value ?? 0));
            ExtHandler.AddGlobalVar("r_007", Convert.ToInt32(r.Cells["黑点"].Value ?? 0));
            ExtHandler.AddGlobalVar("r_008", Convert.ToInt32(r.Cells["黑边"].Value ?? 0));
            ExtHandler.AddGlobalVar("r_012", Convert.ToInt32(r.Cells["异形"].Value ?? 0));

            ExtHandler.AddGlobalVar("r_009", Convert.ToInt32(r.Cells["尺寸"].Value ?? 0));
            ExtHandler.AddGlobalVar("r_010", Convert.ToInt32(r.Cells["圆度"].Value ?? 0));
            ExtHandler.AddGlobalVar("r_011", Convert.ToInt32(r.Cells["色差"].Value ?? 0));

        }
        //数据保存
        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {

        }
        //参数设置保存
        private void uiSymbolButton3_Click(object sender, EventArgs e)
        {
            ExtHandler.AddGlobalVar("停机打印", uiSwitch1.Active ? "true" : "false");            
            ExtHandler.AddGlobalVar("空盘清理", uiSwitch9.Active ? "true" : "false");            
            ExtHandler.AddGlobalVar("启动强清", uiSwitch10.Active ? "true" : "false");            
            ExtHandler.AddGlobalVar("空盘停机", uiSwitch11.Active ? "true" : "false");
            ExtHandler.AddGlobalVar("吹气排数", int.Parse(txtBlowrow.Text));
            ExtHandler.AddGlobalVar("吹气圈数", int.Parse(txtBlowlap.Text));
            ExtHandler.SaveSol();
        }
        //数据刷新
        private void uiSymbolButton8_Click(object sender, EventArgs e)
        {
            loaddata();
        }

        private void loaddata()
        {
            connection = new SQLiteConnection(connectionString);
            connection.Open();
            try
            {
                // 只查询需要的字段
                string query1 = "SELECT * FROM 结果";
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
                        //uiDataGridView1.Columns["ID"].HeaderText = "ID";
                        //uiDataGridView1.Columns["日期"].HeaderText = "日期";
                        //uiDataGridView1.Columns["合格率"].HeaderText = "合格率";
                        //uiDataGridView1.Columns["通过"].HeaderText = "剔除";
                        //uiDataGridView1.Columns["合格率"].HeaderText = "合格率";
                        //uiDataGridView1.Columns["粒径"].HeaderText = "粒径";
                        //uiDataGridView1.Columns["圆度值"].HeaderText = "圆度";
                        //uiDataGridView1.Columns["皮帽"].HeaderText = "皮帽";
                    }
                    // 添加点击事件
                    uiDataGridView1.CellClick += UiDataGridView1_CellClick;
                }
            }
            catch (Exception ex)
            {
                LogNet.Error("查询数据时出错:" + ex.Message);
            }
        }

        //单击数据筛选显示
        private void UiDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            // 缓存当前行
            _currentPrintRow = (sender as UIDataGridView).Rows[e.RowIndex];            
        }                           

        private void UiUsersLst_SelectedIndexChanged(object sender, EventArgs e)
        {
            flowPanel.Controls.Clear();
            mUIndex = uiUsersLst.SelectedIndex;

            if (mUIndex == 0)
            {
                foreach (var item in itemsLst)
                {
                    var checkBox = new UICheckBox
                    {
                        Text = item.Text,
                        Checked = true,
                        Size = new Size(300, 25)
                    };
                    checkBox.Enabled = false;
                    flowPanel.Controls.Add(checkBox);
                }
            }
            else
            {
                foreach (var item in itemsLst)
                {
                    var checkBox = new UICheckBox
                    {
                        Text = item.Text,
                        Checked = false,
                        Size = new Size(300, 25)
                    };
                    int isExist = mFunLst.Where(p => p.uName == AppMangerTool.mSysUses[mUIndex].userName && p.fName == checkBox.Text).Count();
                    if (isExist > 0)
                    {
                        checkBox.Checked = true;
                    }
                    if (AppMangerTool.curIndex != 0)
                    {
                        checkBox.Enabled = false;
                    }
                    checkBox.Click += CheckBox_Click;
                    flowPanel.Controls.Add(checkBox);
                }
            }
        }

        private void CheckBox_Click(object sender, EventArgs e)
        {
            UICheckBox chk = sender as UICheckBox;
            SVUserFunction sVUserFunction = new SVUserFunction();
            if (chk.Checked == true)
            {
                UserFunction userFunction = new UserFunction();
                userFunction.fName = chk.Text;
                userFunction.uName = AppMangerTool.mSysUses[mUIndex].userName;
                sVUserFunction.insertData(userFunction);
            }
            else
            {
                UserFunction userFunction = new UserFunction();
                userFunction.fName = chk.Text;
                userFunction.uName = AppMangerTool.mSysUses[mUIndex].userName;
                sVUserFunction.del(userFunction);
            }
            mFunLst = sVUserFunction.getlst();
        }

        //内1
        private void uiSwitch2_ValueChanged(object sender, bool value)
        {
            if (uiSwitch2.Active == true)
            {
                ExtHandler.AddGlobalVar<bool>("isdebugi1", true);
            }
            else
            {
                ExtHandler.AddGlobalVar<bool>("isdebugi1", false);
            }
        }
        //外1
        private void uiSwitch3_ValueChanged(object sender, bool value)
        {
            if (uiSwitch3.Active == true)
            {
                ExtHandler.AddGlobalVar<bool>("isdebugo1", true);
            }
            else
            {
                ExtHandler.AddGlobalVar<bool>("isdebugo1", false);
            }
        }
        //内2
        private void uiSwitch4_ValueChanged(object sender, bool value)
        {
            if (uiSwitch4.Active == true)
            {
                ExtHandler.AddGlobalVar<bool>("isdebugi2", true);
            }
            else
            {
                ExtHandler.AddGlobalVar<bool>("isdebugi2", false);
            }
        }
        //外2
        private void uiSwitch5_ValueChanged(object sender, bool value)
        {
            if (uiSwitch5.Active == true)
            {
                ExtHandler.AddGlobalVar<bool>("isdebugo2", true);
            }
            else
            {
                ExtHandler.AddGlobalVar<bool>("isdebugo2", false);
            }
        }
        //内3
        private void uiSwitch6_ValueChanged(object sender, bool value)
        {
            if (uiSwitch6.Active == true)
            {
                ExtHandler.AddGlobalVar<bool>("isdebugi3", true);
            }
            else
            {
                ExtHandler.AddGlobalVar<bool>("isdebugi3", false);
            }
        }
        //外3
        private void uiSwitch7_ValueChanged(object sender, bool value)
        {
            if (uiSwitch7.Active == true)
            {
                ExtHandler.AddGlobalVar<bool>("isdebugo3", true);
            }
            else
            {
                ExtHandler.AddGlobalVar<bool>("isdebugo3", false);
            }
        }

        /// <summary>
        /// 调试开关加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DebugSwitch()
        {
            try
            {
                if (ExtHandler.IsLoad)
                {
                    uiSwitch2.Active = ExtHandler.GetGlobalVar<bool>("isdebugi1");
                    uiSwitch3.Active = ExtHandler.GetGlobalVar<bool>("isdebugo1");
                    uiSwitch4.Active = ExtHandler.GetGlobalVar<bool>("isdebugi2");
                    uiSwitch5.Active = ExtHandler.GetGlobalVar<bool>("isdebugo2");
                    uiSwitch6.Active = ExtHandler.GetGlobalVar<bool>("isdebugi3");
                    uiSwitch7.Active = ExtHandler.GetGlobalVar<bool>("isdebugo3");
                }
                
            }
            catch (Exception)
            {

                LogNet.Warn("没有加载方案！");
            }
            
        }
        //调试复位
        private void uiSymbolButton22_Click(object sender, EventArgs e)
        {
            uiSwitch2.Active = false;
            uiSwitch3.Active = false;
            uiSwitch4.Active = false;
            uiSwitch5.Active = false;
            uiSwitch6.Active = false;
            uiSwitch7.Active = false;

            ExtHandler.AddGlobalVar<bool>("isdebugi1", true);
            ExtHandler.AddGlobalVar<bool>("isdebugi2", true);
            ExtHandler.AddGlobalVar<bool>("isdebugi3", true);
            ExtHandler.AddGlobalVar<bool>("isdebugo1", true);
            ExtHandler.AddGlobalVar<bool>("isdebugo2", true);
            ExtHandler.AddGlobalVar<bool>("isdebugo3", true);
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        public void loadPlcPro()
        {
            //盘号加载
            //int rowIndex = 0;
            //ulstPLC.Items.Clear();
            //for (int i = 0; i < AppMangerTool.mPlcData.Count; i++)
            //{
            //    ulstPLC.Items.Add(AppMangerTool.mPlcData[i].proName);
            //    if (AppMangerTool.mPlcData[i].isCurent == 1)
            //    {
            //        rowIndex = i;
            //    }
            //}
            //ulstPLC.SelectedIndex = rowIndex;
            ulstPLC.Items.Clear();
            var SolName = ExtHandler.GetAutoLoadSolName();
            ulstPLC.Items.Add(SolName);
            ulstPLC.SelectedFirst();

            int selectedIndex = 0;
            switch (SolName)
            {
                case "小盘":
                    selectedIndex = 1;
                    break;
                case "中盘":
                    selectedIndex = 2;
                    break;
                case "大盘":
                    selectedIndex = 3;
                    break;
                case "特大盘":
                    selectedIndex = 4;
                    break;
            }
            plcData itemData = AppMangerTool.mPlcData[selectedIndex-1];
            uiNumPadTextBox6.Text = itemData.N1.ToString();
            uiNumPadTextBox7.Text = itemData.W1.ToString();
            uiNumPadTextBox8.Text = itemData.N2.ToString();
            uiNumPadTextBox10.Text = itemData.W2.ToString();
            uiNumPadTextBox9.Text = itemData.N3.ToString();
            uiNumPadTextBox11.Text = itemData.W3.ToString();
            uiNumPadTextBox12.Text = itemData.NPQ.ToString();
            uiNumPadTextBox15.Text = itemData.WPQ.ToString();
            uiNumPadTextBox13.Text = itemData.NGQ.ToString();
            uiNumPadTextBox16.Text = itemData.WGQ.ToString();
            uiNumPadTextBox14.Text = itemData.NPY.ToString();
            uiNumPadTextBox17.Text = itemData.WPY.ToString();


            //牌号加载
            int rowIndex = 0;
            uBrandLst.Items.Clear();
            for (int i = 0; i < mBrandLst.Count; i++)
            {
                uBrandLst.Items.Add(mBrandLst[i].BrandName);
                if (mBrandLst[i].IsCurent == 1)
                {
                    rowIndex = i;
                }
            }
            uBrandLst.SelectedIndex = rowIndex;
        }

        private plcData GetPlcData()
        {
            var SolName = ExtHandler.GetAutoLoadSolName();
            int selectedIndex = 0;
            switch (SolName)
            {
                case "小盘":
                    selectedIndex = 1;
                    break;
                case "中盘":
                    selectedIndex = 2;
                    break;
                case "大盘":
                    selectedIndex = 3;
                    break;
                case "特大盘":
                    selectedIndex = 4;
                    break;
            }

            plcData itemData = AppMangerTool.mPlcData[selectedIndex-1];
            
            itemData.N1 = uiNumPadTextBox6.Text.ToInt();
            itemData.N2 = uiNumPadTextBox8.Text.ToInt();
            itemData.N3 = uiNumPadTextBox9.Text.ToInt();
            itemData.W1 = uiNumPadTextBox7.Text.ToInt();
            itemData.W2 = uiNumPadTextBox10.Text.ToInt();
            itemData.W3 = uiNumPadTextBox11.Text.ToInt();

            itemData.NPQ = uiNumPadTextBox12.Text.ToInt();
            itemData.WPQ = uiNumPadTextBox15.Text.ToInt();

            itemData.NGQ = uiNumPadTextBox13.Text.ToInt();
            itemData.WGQ = uiNumPadTextBox16.Text.ToInt();

            itemData.NPY = uiNumPadTextBox14.Text.ToInt();
            itemData.WPY = uiNumPadTextBox17.Text.ToInt();

            return itemData;
        }

        /// <summary>
        /// 数据库读取剔除参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ulstPLC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ulstPLC.SelectedIndex >= 0)
            {
                plcData itemData = AppMangerTool.mPlcData[ulstPLC.SelectedIndex];
                uiNumPadTextBox6.Text = itemData.N1.ToString();
                uiNumPadTextBox7.Text = itemData.W1.ToString();
                uiNumPadTextBox8.Text = itemData.N2.ToString();
                uiNumPadTextBox10.Text = itemData.W2.ToString();
                uiNumPadTextBox9.Text = itemData.N3.ToString();
                uiNumPadTextBox11.Text = itemData.W3.ToString();
                uiNumPadTextBox12.Text = itemData.NPQ.ToString();
                uiNumPadTextBox15.Text = itemData.WPQ.ToString();
                uiNumPadTextBox13.Text = itemData.NGQ.ToString();
                uiNumPadTextBox16.Text = itemData.WGQ.ToString();
                uiNumPadTextBox14.Text = itemData.NPY.ToString();
                uiNumPadTextBox17.Text = itemData.WPY.ToString();
            }
        }

        /// <summary>
        /// 品牌列表
        /// </summary>
        List<BrandData> mBrandLst = new List<BrandData>();
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            BrandData brandData = new BrandData();
            FrmBrandSet _form = new FrmBrandSet(mBrandLst, brandData);
            if (_form.ShowDialog() == DialogResult.OK)
            {
                mBrandLst.Add(_form.mBrandData);
                uBrandLst.Items.Add(_form.mBrandData.BrandName);
            }
        }

        private void BtnUpd_Click(object sender, EventArgs e)
        {
            int index = uBrandLst.SelectedIndex;
            if (index >= 0)
            {
                FrmBrandSet _form = new FrmBrandSet(mBrandLst, mBrandLst[index]);
                if (_form.ShowDialog() == DialogResult.OK)
                {
                    mBrandLst = (new SVBrandData()).getlst();
                    uBrandLst.Items.Clear();
                    for (int i = 0; i < mBrandLst.Count; i++)
                    {
                        uBrandLst.Items.Add(mBrandLst[i].BrandName);

                        uBrandLst.SelectedIndex = index;
                    }
                }
            }

        }

        private void BtnDel_Click(object sender, EventArgs e)
        {
            if (uBrandLst.SelectedIndex < 0)
            {
                this.ShowWarningDialog("请选择要删除的品牌");
                return;
            }
            if (mBrandLst.Count() == 1)
            {
                this.ShowWarningDialog("系统至少保持有一个品牌");
                return;
            }
            int rowIndex = uBrandLst.SelectedIndex;
            BrandData brand = mBrandLst[rowIndex];
            if (brand.IsCurent == 1)
            {
                this.ShowWarningDialog("该品牌为当前品牌不能删除");
            }
            else
            {
                (new SVBrandData()).delete(brand);
                (new SVBrandData()).DeletAppPrgm(brand.ID);
                mBrandLst.RemoveAt(rowIndex);
                uBrandLst.Items.RemoveAt(rowIndex);
                this.ShowSuccessDialog("删除成功");
            }
        }

        private void UBrandLst_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (uBrandLst.SelectedIndex >= 0)
            {
                int bId = mBrandLst[uBrandLst.SelectedIndex].ID;
                setuisettings(bId);
            }

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            mBrandID = mBrandLst[uBrandLst.SelectedIndex].ID;
            var settings = new List<(string Key, string Value)>
            {
                ("粒径", uiNumPadTextBox19.Text),
                ("公差", uiNumPadTextBox20.Text),
                ("圆度", uiNumPadTextBox21.Text),
                ("色差", uiNumPadTextBox22.Text),
                ("L", uiNumPadTextBox23.Text),
                ("A", uiNumPadTextBox24.Text),
                ("B", uiNumPadTextBox25.Text),
                ("长短抽差", uiNumPadTextBox31.Text),
                ("标准差", uiNumPadTextBox32.Text),
                ("主转速", uiNumPadTextBox26.Text),
                 ("皮带转速", uiNumPadTextBox27.Text),
                 ("拨珠1转速", uiNumPadTextBox28.Text),
                 ("拨珠2转速", uiNumPadTextBox29.Text),
                 ("搅动转速", uiNumPadTextBox30.Text),
            };
            (new SVBrandData()).DeletAppPrgm(mBrandID);
            xop1080model.SVsettings(settings, mBrandID);
            LogNet.Info("参数设置保存成功");
        }

        private void btnDetset_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("osk.exe");
                Thread.Sleep(20);
                ExtHandler.OpenModFrm("内1", "目标检测0");
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 标定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCal_Click(object sender, EventArgs e)
        {
            try
            {
                // 提取标准值并校验输入
                if (!double.TryParse(uiNumPadTextBox33.Text, out double standardValue))
                {
                    MessageBox.Show("请输入有效的标准数值！", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (ExtHandler.GetGlobalVar<Int32>("Counti")>0)
                {
                    var tempi = ExtHandler.GetGlobalVar<double>("meanSumi")/ExtHandler.GetGlobalVar<Int32>("Counti");
                    ExtHandler.AddGlobalVar("Scalei", double.Parse(uiNumPadTextBox33.Text)/ tempi);
                    LogNet.Info("内排标定系数 ： " + double.Parse(uiNumPadTextBox33.Text) / tempi);
                    ExtHandler.SaveSol();
                }
                //单排标定，写入临时标定值，by崔
                else
                {
                    ExtHandler.AddGlobalVar("Scalei", tempscalei);
                    LogNet.Info("内排标定系数保持 ： " + tempscalei);
                }
                if (ExtHandler.GetGlobalVar<Int32>("Counto") > 0)
                {
                    var tempo = ExtHandler.GetGlobalVar<double>("meanSumo")/ExtHandler.GetGlobalVar<Int32>("Counto");
                    ExtHandler.AddGlobalVar("Scaleo", double.Parse(uiNumPadTextBox33.Text)/ tempo);
                    LogNet.Info("外排标定系数 ： " + double.Parse(uiNumPadTextBox33.Text) / tempo);
                    ExtHandler.SaveSol();
                }
                //单排标定，写入临时标定值，by崔
                else
                {
                    ExtHandler.AddGlobalVar("Scaleo", tempscaleo);
                    LogNet.Info("外排标定系数保持 ： " + tempscaleo);
                }

            }
            catch (Exception ex)
            {
                LogNet.Error("标定失败"+ ex.ToString());
            }
                
            
        }

        //标定使能
        public static double tempscalei;
        public static double tempscaleo;
        private void uiSwitch8_ValueChanged(object sender, bool value)
        {
            if (uiSwitch8.Active == true)
            {
                tempscalei=ExtHandler.GetGlobalVar<Double>("Scalei");
                tempscaleo=ExtHandler.GetGlobalVar<Double>("Scaleo");
                ExtHandler.AddGlobalVar<Double>("Scalei", 1);
                ExtHandler.AddGlobalVar<Double>("Scaleo", 1);
            }
        }

        private void uiNumPadTextBox13_ValueChanged(object sender, string value)
        {

        }

        private void uiButton1_Click(object sender, EventArgs e)
        {

            ResJudge rjv = new ResJudge();
            var resJudges = rjv.Params;

         }

        /// <summary>
        /// 读目标检测参数
        /// </summary>
        private void ReadDeepText()
        {
            // 1. 获取数据并检查是否为null或空
            var data = DeepManager.GetResJudges("目标检测0");
            if (data == null || data.Count == 0)
            {
                ClearUiAndDisableSwitches();
                return;
            }

            // 2. 安全地获取第一个元素
            var firstItem = data[0];
            if (firstItem == null || firstItem.Params == null || firstItem.Params.Count == 0)
            {
                ClearUiAndDisableSwitches();
                return;
            }

            // 3. 将需要操作的控件放入数组，方便循环处理
            var deepTexts = new[] { deepText1, deepText2, deepText3, deepText4, deepText5, deepText6, deepText7, deepText8, deepText9 };
            var switches = new[] { swDeep1, swDeep2, swDeep3, swDeep4, swDeep5, swDeep6, swDeep7, swDeep8, swDeep9 };

            // 4. 循环处理，避免重复代码
            for (int i = 0; i < deepTexts.Length; i++)
            {
                // 检查Params列表是否包含当前索引的元素
                if (i < firstItem.Params.Count && firstItem.Params[i] != null)
                {
                    // 安全地获取MaxValue
                    var maxValue = firstItem.Params[i].MaxValue;
                    deepTexts[i].Text = maxValue.ToString();

                    // 根据MaxValue设置开关状态
                    switches[i].Active = (maxValue != 1);
                }
                else
                {
                    // 如果数据不存在，则清空文本并设置一个默认的开关状态
                    deepTexts[i].Text = "N/A"; // 使用"N/A"比空字符串更明确表示无数据
                    switches[i].Active = false; // 根据业务逻辑决定默认状态
                }
            }
        }

        // 提取一个辅助方法来处理无数据时的UI状态
        private void ClearUiAndDisableSwitches()
        {
            var deepTexts = new[] { deepText1, deepText2, deepText3, deepText4, deepText5, deepText6, deepText7, deepText8, deepText9 };
            var switches = new[] { swDeep1, swDeep2, swDeep3, swDeep4, swDeep5, swDeep6, swDeep7, swDeep8, swDeep9 };

            foreach (var textBox in deepTexts)
            {
                textBox.Text = "N/A"; // 使用"N/A"比空字符串更明确表示无数据
            }

            foreach (var sw in switches)
            {
                sw.Active = false; // 根据业务逻辑决定默认状态
            }
        }
        /// <summary>
        /// 写目标检测参数
        /// </summary>
        private void WriteDeepText()
        {
            var data = DeepManager.GetResJudges("目标检测0");
            if (data == null)
            {
                return;
            }

            // 2. 将需要操作的控件放入数组，方便循环处理
            var deepTexts = new[] { deepText1, deepText2, deepText3, deepText4, deepText5, deepText6, deepText7, deepText8, deepText9 };
            var switches = new[] { swDeep1, swDeep2, swDeep3, swDeep4, swDeep5, swDeep6, swDeep7, swDeep8, swDeep9 };
            for (int dataIndex = 0; dataIndex < 2; dataIndex++)
            {
                if (dataIndex >= data.Count)
                {
                    break;
                }

                var currentDataItem = data[dataIndex];
                if (currentDataItem == null || currentDataItem.Params == null)
                {
                    continue;
                }
                for (int i = 0; i < deepTexts.Length; i++)
                {
                    if (i >= currentDataItem.Params.Count)
                    {
                        break;
                    }

                    var param = currentDataItem.Params[i];
                    if (param == null)
                    {
                        continue;
                    }
                    if (double.TryParse(deepTexts[i].Text, out double parsedValue))
                    {
                        param.MaxValue = switches[i].Active ? parsedValue : 1.0;
                    }
                    else
                    {
                        param.MaxValue = 1.0;
                    }
                }
            }
        }

        private void btnWriteDeep_Click(object sender, EventArgs e)
        {
            WriteDeepText();
            ExtHandler.SaveSol();
            ReadDeepText();
        }
    }


    
    public class CheckItem
    {
        public string Text { get; set; }
        public bool Checked { get; set; }
    }
}
