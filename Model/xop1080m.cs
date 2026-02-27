using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RobotLocation.Service;
using RobotLocation.UI;
using Sunny.UI;
using VisionCore.Communication;
using VisionCore.Ext;
using VisionCore.Log;
using static System.Net.Mime.MediaTypeNames;

namespace RobotLocation.Model
{
    internal class xop1080m
    {
        private string connectionString = "Data Source=data.db;Version=3;";
        private SQLiteConnection connection;
        // 默认参数配置（参数名 + 默认值）
        private Dictionary<string, string> defaultSettings = new Dictionary<string, string>
        {
            { "粒径", "3.00" },
            { "公差", "0.10" },
            { "圆度", "0.97" },
            { "色差", "3.5" },
            { "L", "128" },
            { "A", "128" },
            { "B", "128" },
            { "轴差", "0.2" },
            { "标准差", "0.75" },
            { "主转速", "40" },
            { "皮带转速", "20" },
            { "拨珠1转速", "20" },
            { "拨珠2转速", "20" },
            { "搅动转速", "400" },
            //{ "内1剔除", "66" },
            //{ "内2剔除", "52" },
            //{ "内3剔除", "32" },
            //{ "内喷气", "3" },
            //{ "内关气", "7" },
            //{ "内偏移", "12" },
            //{ "外1剔除", "74" },
            //{ "外2剔除", "60" },
            //{ "外3剔除", "36" },
            //{ "外喷气", "11" },
            //{ "外关气", "15" },
            //{ "外偏移", "9" },
            //{ "缓存", "7" },
            //{ "自动打印", "0" }
        };

        public Dictionary<string, string> vars = new Dictionary<string, string>
        {
            /* 1 日期 */          { "日期",        DateTime.Now.ToString("yyyy-MM-dd") },
            /* 2 时间 */          { "时间",        DateTime.Now.ToString("HH:mm:ss") },
            /* 3 检测牌号 */      { "检测牌号",    "XB001" },
            /* 4 检测直径 */      { "检测直径",    "3.00" },
            /* 5 检测偏差 */      { "检测偏差",    "0.00" },
            /* 6 检测圆度 */      { "检测圆度",    "0.97" },
            /* 7 色差阈值 */      { "色差阈值",    "3.5" },

            /* 8 合格 */          { "合格",        "0" },
            /* 9 剔除 */          { "剔除",        "0" },
            /*11 合格率 */        { "合格率",      "0" },

            /*12 粒径 */          { "粒径",        "0" },
            /*14 粒径MA */        { "粒径MA",      "0" },
            /*15 粒径MIN */       { "粒径MIN",     "0" },

            /*16 圆度值 */        { "圆度值",      "0" },
            /*18 圆度MA */        { "圆度MA",      "0" },
            /*19 圆度MIN */       { "圆度MIN",     "0" },

            /*20 标准差 */        { "标准差",      "0" },
            /*22 标准差MA */      { "标准差MA",    "0" },
            /*23 标准差MIN */     { "标准差MIN",   "0" },

            /*24 长轴 */          { "长轴",        "0" },
            /*26 长轴MA */        { "长轴MA",      "0" },
            /*27 长轴MIN */       { "长轴MIN",     "0" },

            /*28 短轴 */          { "短轴",        "0" },
            /*30 短轴MA */        { "短轴MA",      "0" },
            /*31 短轴MIN */       { "短轴MIN",     "0" },

            /*32 皮帽 */          { "皮帽",        "0" },
            /*33 脏污 */          { "脏污",        "0" },
            /*34 异色 */          { "异色",        "0" },
            /*35 凹陷 */          { "凹陷",        "0" },
            /*36 凸点 */          { "凸点",        "0" },
            /*37 气泡 */          { "气泡",        "0" },
            /*38 实心 */          { "实心",        "0" },
            /*39 异形 */          { "异形",        "0" },
            /*40 尺寸 */          { "尺寸",        "0" },
            /*41 圆度 */          { "圆度",        "0" },
            /*42 色差 */          { "色差",        "0" },

            /* 其余控制参数保持不变 */
            { "粒径标准",   "3.00" },
            { "公差",       "0.10" },
            { "圆度标准",   "0.97" },
            { "色差标准",   "3.5" },
            { "L",          "128" },
            { "A",          "128" },
            { "B",          "128" },
            { "主转速",     "40" },
            { "皮带转速",   "6" },
            { "拨珠1转速",  "40" },
            { "拨珠2转速",  "40" },
            { "搅动转速",   "200" },
            { "内1剔除",    "66" },
            { "内2剔除",    "52" },
            { "内3剔除",    "32" },
            { "内喷气",     "3" },
            { "内关气",     "7" },
            { "内偏移",     "12" },
            { "外1剔除",    "74" },
            { "外2剔除",    "60" },
            { "外3剔除",    "36" },
            { "外喷气",     "11" },
            { "外关气",     "15" },
            { "外偏移",     "9" },
            { "缓存",       "7" },
            { "自动打印",   "0" },
            { "剔除计数",   "0" }
        };

        public xop1080m()
        {
            AppMangerTool.mBrandLst = (new SVBrandData()).getlst();

            this.dataint();
            //this.plcint();
            var dataItem = AppMangerTool.mBrandLst.Where(p => p.IsCurent == 1).FirstOrDefault();
            if (dataItem != null)
            {
                AppMangerTool.mCunBrandID = dataItem.ID;

            }
            this.SettingsInit(AppMangerTool.mCunBrandID);

        }
        #region 初始化数据库
        public void dataint()
        {
            connection = new SQLiteConnection(connectionString);
            connection.Open();
            // 创建结果表（如果不存在）
            string createTableQuery1 = @"
                CREATE TABLE IF NOT EXISTS 结果 (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                日期 TEXT,
                时间 TEXT,
                检测牌号 TEXT,
                检测直径 TEXT,
                检测偏差 TEXT,
                检测圆度 TEXT,
                色差阈值 TEXT,
                合格 TEXT,                
                剔除 TEXT,  
                合格率 TEXT,
                粒径 TEXT,
                粒径MA TEXT,
                粒径MIN TEXT,
                圆度值 TEXT,
                圆度MA TEXT,
                圆度MIN TEXT,
                标准差 TEXT,
                标准差MA TEXT,
                标准差MIN TEXT,
                长轴 TEXT,
                长轴MA TEXT,
                长轴MIN TEXT,
                短轴 TEXT,
                短轴MA TEXT,
                短轴MIN TEXT,
                皮帽 TEXT,
                脏污 TEXT,
                异色 TEXT,
                凹陷 TEXT,
                凸点 TEXT,
                气泡 TEXT,
                黑点 TEXT,
                黑边 TEXT,
                异形 TEXT,
                尺寸 TEXT,
                圆度 TEXT,
                色差 TEXT
                );";
            // 新增参数表（参数名 + 参数值）
            string createTableQuery2 = @"
            CREATE TABLE IF NOT EXISTS 程序参数 (
                参数名 TEXT PRIMARY KEY,  -- 参数名作为主键（确保唯一性）
                参数值 TEXT
            );";
            using (SQLiteCommand command = new SQLiteCommand(createTableQuery1, connection))
            {
                command.ExecuteNonQuery();
            }
            using (SQLiteCommand command2 = new SQLiteCommand(createTableQuery2, connection))
            {
                command2.ExecuteNonQuery();
            }
        }

        #endregion

        /// <summary>
        /// 开机关闭下位连锁
        /// </summary>
        //public void plcint()
        //{
        //    //开机强制关闭下位连锁，防止程序崩溃后残留标记
        //    var r = EComManageer.GetECommunication("ModbusTcpNet0");
        //    if (r.status)
        //    {
        //        EComManageer.Write<bool>("ModbusTcpNet0", "529", false);
        //    }
        //    else
        //    {
        //        LogNet.Info("下位连锁异常，无法运行");
        //    }
        //}
        public Dictionary<string, string> settings = new Dictionary<string, string>();
        public void SettingsInit(int bID)
        {

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // 查询所有已有参数
                string query = string.Format("SELECT 参数名, 参数值 FROM 程序参数 where BrandID={0}", AppMangerTool.mCunBrandID);
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string key = reader["参数名"].ToString();
                        string value = reader["参数值"].ToString();
                        settings[key] = value;
                    }
                }
                // 遍历默认设置
                foreach (var kvp in defaultSettings)
                {
                    // 如果参数不存在或值等于 "0"，设置默认值并更新数据库
                    if (!settings.ContainsKey(kvp.Key) || settings[kvp.Key] == "0")
                    {
                        settings[kvp.Key] = kvp.Value;

                        // 将默认值写入数据库
                        string insertQuery = "INSERT OR REPLACE INTO 程序参数 (参数名, 参数值,BrandID) VALUES (@Key, @Value,@BrandID);";
                        using (var cmd = new SQLiteCommand(insertQuery, connection))
                        {
                            cmd.Parameters.AddWithValue("@Key", kvp.Key);
                            cmd.Parameters.AddWithValue("@Value", kvp.Value);
                            cmd.Parameters.AddWithValue("@BrandID", AppMangerTool.mCunBrandID);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public Dictionary<string, string> getBrandData(int bId)
        {
            Dictionary<string, string> pramData = new Dictionary<string, string>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // 查询所有已有参数
                string query = string.Format("SELECT 参数名, 参数值 FROM 程序参数 where BrandID={0}", bId);
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string key = reader["参数名"].ToString();
                        string value = reader["参数值"].ToString();
                        pramData[key] = value;
                    }
                }
                // 遍历默认设置
                foreach (var kvp in defaultSettings)
                {
                    // 如果参数不存在或值等于 "0"，设置默认值并更新数据库
                    if (!pramData.ContainsKey(kvp.Key) || pramData[kvp.Key] == "0")
                    {
                        pramData[kvp.Key] = kvp.Value;

                        // 将默认值写入数据库
                        string insertQuery = "INSERT  INTO 程序参数 (参数名, 参数值,BrandID) VALUES (@Key, @Value,@BrandID);";
                        using (var cmd = new SQLiteCommand(insertQuery, connection))
                        {
                            cmd.Parameters.AddWithValue("@Key", kvp.Key);
                            cmd.Parameters.AddWithValue("@Value", kvp.Value);
                            cmd.Parameters.AddWithValue("@BrandID", bId);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                return pramData;
            }

        }
        public static void LoadSettings()
        {
        }

        public static void SaveSettings()
        {
            //逻辑未实现
        }
        //public bool plcclear()
        //{
        //    var r = EComManageer.GetECommunication("ModbusTcpNet0");
        //    if (r.status)
        //    {
        //        EComManageer.Write<bool>("ModbusTcpNet0", "529", false);
        //        //清空手动按钮启动状态和央视
        //        EComManageer.Write<bool>("ModbusTcpNet0", "500", false);
        //        EComManageer.Write<bool>("ModbusTcpNet0", "505", false);
        //        EComManageer.Write<bool>("ModbusTcpNet0", "510", false);
        //        EComManageer.Write<bool>("ModbusTcpNet0", "515", false);
        //        EComManageer.Write<bool>("ModbusTcpNet0", "520", false);
        //        return true;
        //    }
        //    else
        //    {
        //        LogNet.Error("下位连锁异常，无法停止其状态，使用按钮或急停停止下位运行！");
        //        return false;
        //    }
        //}
        //读取PLC数据
        //private void readplcdate(object sender, EventArgs e)
        //{
        //    var r = EComManageer.GetECommunication("ModbusTcpNet0");
        //    if (r.status)
        //    {
        //        var Main = EComManageer.Read<UInt16>("ModbusTcpNet0", "1000");
        //        var Cut = EComManageer.Read<UInt16>("ModbusTcpNet0", "1001");
        //        var Turn1 = EComManageer.Read<UInt16>("ModbusTcpNet0", "1002");
        //        var Turn2 = EComManageer.Read<UInt16>("ModbusTcpNet0", "1003");
        //        var Stir = EComManageer.Read<UInt16>("ModbusTcpNet0", "1004");
        //        //读取剔除步数，来源单片机返回数据
        //        var step1 = EComManageer.Read<UInt16>("ModbusTcpNet0", "1400");
        //        var step2 = EComManageer.Read<UInt16>("ModbusTcpNet0", "1401");
        //        var step3 = EComManageer.Read<UInt16>("ModbusTcpNet0", "1402");
        //        var step4 = EComManageer.Read<UInt16>("ModbusTcpNet0", "1403");
        //        var step5 = EComManageer.Read<UInt16>("ModbusTcpNet0", "1404");
        //        var step6 = EComManageer.Read<UInt16>("ModbusTcpNet0", "1405");
        //        var time1 = EComManageer.Read<UInt16>("ModbusTcpNet0", "1406");
        //        var time2 = EComManageer.Read<UInt16>("ModbusTcpNet0", "1407");
        //        var time3 = EComManageer.Read<UInt16>("ModbusTcpNet0", "1408");
        //        var time4 = EComManageer.Read<UInt16>("ModbusTcpNet0", "1409");
        //        var step7 = EComManageer.Read<UInt16>("ModbusTcpNet0", "1410");//内偏移
        //        var step8 = EComManageer.Read<UInt16>("ModbusTcpNet0", "1411");//外偏移*/                
        //        //显示数据
        //    }
        //}
        //读取历史数据
        private void LoadData()
        {
            try
            {
                // 只查询需要的字段
                //string query1 = "SELECT ID, 日期,通过,剔除,合格率, 粒径, 圆度值 FROM 结果";
                string query1 = "SELECT* FROM 结果";
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
                    }
                }
            }
            catch (Exception ex)
            {
                LogNet.Error("查询数据时出错:" + ex.Message);
            }

        }

        public void CleanData()
        {
            ExtHandler.OnceRunProj("初始化数据");
            //初始计数清零，防止销毁不到队列
            ToneOp.Reset();

            //ToneOp.缓存 = uiIntegerUpDown1.Value;

            LogNet.Info("排照次数及触发次数清零");
        }

        public void SetData(string type)
        {
            ExtHandler.AddGlobalVar("type", type);
        }
        public void getvars()
        {
            List<string> Sols = ExtHandler.GetSols();
            if (Sols != null && Sols.Count > 0)
            {
                /* 1~7 基础信息 */
                vars["日期"] = DateTime.Now.ToString("yyyy-MM-dd");
                vars["时间"] = DateTime.Now.ToString("HH:mm:ss");
                vars["检测牌号"] = ExtHandler.GetGlobalVar<string>("type") ?? "XB001";
                vars["检测直径"] = ExtHandler.GetGlobalVar<double>("检测粒径").ToString("F3");
                vars["检测偏差"] = ExtHandler.GetGlobalVar<double>("粒径偏差").ToString("F3");
                vars["检测圆度"] = ExtHandler.GetGlobalVar<double>("圆度").ToString("F3");
                vars["色差阈值"] = ExtHandler.GetGlobalVar<double>("色差阈值").ToString("F2");

                /* 8~11 计数 & 合格率 */
                vars["合格"] = ExtHandler.GetGlobalVar<int>("r_ok").ToString();
                vars["剔除"] = ExtHandler.GetGlobalVar<int>("r_tc").ToString();
                int total = ExtHandler.GetGlobalVar<int>("通过") + ExtHandler.GetGlobalVar<int>("r_tc");
                vars["合格率"] = (total == 0 ? 0 : (double)ExtHandler.GetGlobalVar<int>("通过") / total * 100).ToString("F2");

                /* 12~15 粒径 */
                vars["粒径"] = ExtHandler.GetGlobalVar<double>("粒径均值").ToString("F3");
                vars["粒径MA"] = ExtHandler.GetGlobalVar<double>("粒径最大值").ToString("F3");
                vars["粒径MIN"] = ExtHandler.GetGlobalVar<double>("粒径最小值").ToString("F3");

                /* 16~19 圆度 */
                vars["圆度值"] = ExtHandler.GetGlobalVar<double>("圆度均值").ToString("F3");
                vars["圆度MA"] = ExtHandler.GetGlobalVar<double>("圆度最大值").ToString("F3");
                vars["圆度MIN"] = ExtHandler.GetGlobalVar<double>("圆度最小值").ToString("F3");

                /* 20~23 标准差 */
                vars["标准差"] = ExtHandler.GetGlobalVar<double>("方差均值").ToString("F3");
                vars["标准差MA"] = ExtHandler.GetGlobalVar<double>("方差最大值").ToString("F3");
                vars["标准差MIN"] = ExtHandler.GetGlobalVar<double>("方差最小值").ToString("F3");

                /* 24~27 长轴 */
                vars["长轴"] = ExtHandler.GetGlobalVar<double>("长轴均值").ToString("F3");
                vars["长轴MA"] = ExtHandler.GetGlobalVar<double>("长轴最大值").ToString("F3");
                vars["长轴MIN"] = ExtHandler.GetGlobalVar<double>("长轴最小值").ToString("F3");

                /* 28~31 短轴 */
                vars["短轴"] = ExtHandler.GetGlobalVar<double>("短轴均值").ToString("F3");
                vars["短轴MA"] = ExtHandler.GetGlobalVar<double>("短轴最大值").ToString("F3");
                vars["短轴MIN"] = ExtHandler.GetGlobalVar<double>("短轴最小值").ToString("F3");

                /* 32~42 缺陷 */
                vars["皮帽"] = ExtHandler.GetGlobalVar<int>("r_001").ToString();
                vars["脏污"] = ExtHandler.GetGlobalVar<int>("r_002").ToString();
                vars["异色"] = ExtHandler.GetGlobalVar<int>("r_003").ToString();
                vars["凹陷"] = ExtHandler.GetGlobalVar<int>("r_004").ToString();
                vars["凸点"] = ExtHandler.GetGlobalVar<int>("r_005").ToString();
                vars["气泡"] = ExtHandler.GetGlobalVar<int>("r_006").ToString();
                vars["黑点"] = ExtHandler.GetGlobalVar<int>("r_007").ToString();
                vars["黑边"] = ExtHandler.GetGlobalVar<int>("r_008").ToString();
                vars["异形"] = ExtHandler.GetGlobalVar<int>("r_012").ToString();

                vars["尺寸"] = ExtHandler.GetGlobalVar<int>("r_009").ToString();
                vars["圆度"] = ExtHandler.GetGlobalVar<int>("r_010").ToString();
                vars["色差"] = ExtHandler.GetGlobalVar<int>("r_011").ToString();
            }
        }

        public void SVdata()
        {
            // 1. 当前日期、时间
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string time = DateTime.Now.ToString("HH-mm-ss");

            // 2. 合格率实时计算
            int 剔除 = ExtHandler.GetGlobalVar<int>("NgCount");
            int 通过 = ExtHandler.GetGlobalVar<int>("通过");

            int 合格 = 通过 - 剔除;

            // 3. 良率计算核心逻辑
            int validPassed = Math.Max(通过 - 剔除, 0);
            double percentage = 0;

            // 计算百分比，避免除以零
            if (通过 > 0)
            {
                percentage = (validPassed / (double)通过) * 100; // 避免整数除法
            }
            double 合格率 = Math.Round(percentage, 2); // 保留两位小数

            // 3. 拼写正确的 SQL
            string sql =
                "INSERT INTO 结果 (日期,时间,检测牌号,检测直径,检测偏差,检测圆度,色差阈值," +
                "合格,剔除,合格率," +
                "粒径,粒径MA,粒径MIN," +
                "圆度值,圆度MA,圆度MIN," +
                "标准差,标准差MA,标准差MIN," +
                "长轴,长轴MA,长轴MIN," +
                "短轴,短轴MA,短轴MIN," +
                "皮帽,脏污,异色,凹陷,凸点,气泡,黑点,黑边,异形,尺寸,圆度,色差) " +
                "VALUES(@日期,@时间,@检测牌号,@检测直径,@检测偏差,@检测圆度,@色差阈值," +
                       "@合格,@剔除,@合格率," +
                       "@粒径,@粒径MA,@粒径MIN," +
                       "@圆度值,@圆度MA,@圆度MIN," +
                       "@标准差,@标准差MA,@标准差MIN," +
                       "@长轴,@长轴MA,@长轴MIN," +
                       "@短轴,@短轴MA,@短轴MIN," +
                       "@皮帽,@脏污,@异色,@凹陷,@凸点,@气泡,@黑点,@黑边,@异形,@尺寸,@圆度,@色差);";

            using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
            {
                // 基础信息
                cmd.Parameters.AddWithValue("@日期", date);
                cmd.Parameters.AddWithValue("@时间", time);
                cmd.Parameters.AddWithValue("@检测牌号", vars["检测牌号"]);
                cmd.Parameters.AddWithValue("@检测直径", vars["检测直径"]);
                cmd.Parameters.AddWithValue("@检测偏差", vars["检测偏差"]);
                cmd.Parameters.AddWithValue("@检测圆度", vars["检测圆度"]);
                cmd.Parameters.AddWithValue("@色差阈值", vars["色差阈值"]);

                // 计数
                cmd.Parameters.AddWithValue("@合格", 合格);
                cmd.Parameters.AddWithValue("@剔除", 剔除);
                cmd.Parameters.AddWithValue("@合格率", 合格率.ToString("F3"));

                // 粒径
                cmd.Parameters.AddWithValue("@粒径", vars["粒径"]);
                cmd.Parameters.AddWithValue("@粒径MA", vars["粒径MA"]);
                cmd.Parameters.AddWithValue("@粒径MIN", vars["粒径MIN"]);

                // 圆度
                cmd.Parameters.AddWithValue("@圆度值", vars["圆度值"]);
                cmd.Parameters.AddWithValue("@圆度MA", vars["圆度MA"]);
                cmd.Parameters.AddWithValue("@圆度MIN", vars["圆度MIN"]);

                // 标准差
                cmd.Parameters.AddWithValue("@标准差", vars["标准差"]);
                cmd.Parameters.AddWithValue("@标准差MA", vars["标准差MA"]);
                cmd.Parameters.AddWithValue("@标准差MIN", vars["标准差MIN"]);

                // 长轴
                cmd.Parameters.AddWithValue("@长轴", vars["长轴"]);
                cmd.Parameters.AddWithValue("@长轴MA", vars["长轴MA"]);
                cmd.Parameters.AddWithValue("@长轴MIN", vars["长轴MIN"]);

                // 短轴
                cmd.Parameters.AddWithValue("@短轴", vars["短轴"]);
                cmd.Parameters.AddWithValue("@短轴MA", vars["短轴MA"]);
                cmd.Parameters.AddWithValue("@短轴MIN", vars["短轴MIN"]);

                // 缺陷
                cmd.Parameters.AddWithValue("@皮帽", vars["皮帽"]);
                cmd.Parameters.AddWithValue("@脏污", vars["脏污"]);
                cmd.Parameters.AddWithValue("@异色", vars["异色"]);
                cmd.Parameters.AddWithValue("@凹陷", vars["凹陷"]);
                cmd.Parameters.AddWithValue("@凸点", vars["凸点"]);
                cmd.Parameters.AddWithValue("@气泡", vars["气泡"]);
                cmd.Parameters.AddWithValue("@黑点", vars["黑点"]);
                cmd.Parameters.AddWithValue("@黑边", vars["黑边"]);
                cmd.Parameters.AddWithValue("@异形", vars["异形"]);
                cmd.Parameters.AddWithValue("@尺寸", vars["尺寸"]);
                cmd.Parameters.AddWithValue("@圆度", vars["圆度"]);
                cmd.Parameters.AddWithValue("@色差", vars["色差"]);

                cmd.ExecuteNonQuery();
            }
            LoadData(); // 刷新数据
        }

        public void SVsettings(List<(string Key, string Value)> settings, int BID)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // 开始事务
                    using (var transaction = connection.BeginTransaction())
                    {
                        string insertQuery = @"
                INSERT  INTO 程序参数 (参数名, 参数值,BrandID) 
                VALUES (@Key, @Value,@BrandID);";

                        using (var command = new SQLiteCommand(insertQuery, connection))
                        {
                            foreach (var (key, value) in settings)
                            {
                                command.Parameters.Clear();
                                command.Parameters.AddWithValue("@Key", key);
                                command.Parameters.AddWithValue("@Value", value);
                                command.Parameters.AddWithValue("@BrandID", BID);
                                command.ExecuteNonQuery();
                            }
                        }

                        // 提交事务
                        transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                LogNet.Error($"保存设置时发生异常: {ex.Message}\n堆栈信息: {ex.StackTrace}");
            }
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

        public static bool PrintReport(DTO rpt)
        {
            const string port = "SP-USB1";
            long hPort = POS_Port_OpenA(port, 1002, false, "");
            if (hPort < 0)
                return false;

            try
            {
                // 初始化打印
                byte[] cmd = { 0x1c, 0x26 };
                POS_Output_PrintData(hPort, cmd, 2);
                POS_Control_AlignType(hPort, 1);

                // 标题
                POS_Output_PrintFontStringA(hPort, 0, 0, 0, 0, 0, "XOP1080 视觉选丸仪\r\n");
                POS_Output_PrintFontStringA(hPort, 0, 0, 0, 0, 0, "检测数据\r\n");
                POS_Control_AlignType(hPort, 0);

                // 时间、溶液名
                POS_Output_PrintFontStringA(hPort, 0, 0, 0, 0, 0,
                    $"{rpt.ReportTime:yyyy-MM-dd HH:mm:ss}                 {ExtHandler.GetAutoLoadSolName()}\r\n");

                // 基本统计
                POS_Output_PrintFontStringA(hPort, 0, 0, 0, 0, 0,
                    "-------------------------------------------\r\n");

                POS_Output_PrintFontStringA(hPort, 0, 0, 0, 0, 0,
                  $"检测牌号：    {rpt.type}\r\n");
                POS_Output_PrintFontStringA(hPort, 0, 0, 0, 0, 0,
                    $"检测直径：    {rpt.DetectDiameter:F3}\r\n");
                POS_Output_PrintFontStringA(hPort, 0, 0, 0, 0, 0,
                    $"检测偏差：    {rpt.DiameterBias:F3}\r\n");
                POS_Output_PrintFontStringA(hPort, 0, 0, 0, 0, 0,
                    $"检测圆度：    {rpt.DetectRoundness:F3}\r\n");
                POS_Output_PrintFontStringA(hPort, 0, 0, 0, 0, 0,
                    $"色差阈值：    {rpt.ColorThreshold:F3}\r\n");
                POS_Output_PrintFontStringA(hPort, 0, 0, 0, 0, 0,
                    "-------------------------------------------\r\n");

                POS_Output_PrintFontStringA(hPort, 0, 0, 0, 0, 0,
                    $"合格:         {rpt.QualifiedCount}次\r\n");
                POS_Output_PrintFontStringA(hPort, 0, 0, 0, 0, 0,
                    $"剔除:         {rpt.RejectCount}粒\r\n");
                POS_Output_PrintFontStringA(hPort, 0, 0, 0, 0, 0,
                    $"合格率:       {rpt.PassCount}%\r\n");

                // 数值区间
                POS_Output_PrintFontStringA(hPort, 0, 0, 0, 0, 0,
                    "-------------------------------------------\r\n");
                POS_Output_PrintFontStringA(hPort, 0, 0, 0, 0, 0,
                    "              Agv       Max         Min        \r\n");

                void PrintRange(string title, double avg, double max, double min) =>
                    POS_Output_PrintFontStringA(hPort, 0, 0, 0, 0, 0,
                        $" {title}:     {avg:F3}       {max:F3}       {min:F3}\r\n");

                PrintRange("粒径  ", rpt.AvgDiameter, rpt.MaxDiameter, rpt.MinDiameter);
                PrintRange("圆度值", rpt.AvgRoundness, rpt.MaxRoundness, rpt.MinRoundness);
                //PrintRange("标准差", rpt.AvgStdDev, rpt.MaxStdDev, rpt.MinStdDev);
                PrintRange("长轴  ", rpt.AvgLongAxis, rpt.MaxLongAxis, rpt.MinLongAxis);
                PrintRange("短轴  ", rpt.AvgShortAxis, rpt.MaxShortAxis, rpt.MinShortAxis);

                // 缺陷占比
                POS_Output_PrintFontStringA(hPort, 0, 0, 0, 0, 0,
                    "-------------------------------------------\r\n");

                void PrintDefect(string name, int cnt)
                {
                    double percent = rpt.TotalDefects == 0 ? 0 : (double)cnt / rpt.TotalDefects * 100;
                    POS_Output_PrintFontStringA(hPort, 0, 0, 0, 0, 0,
                        $"{name}：    {percent:F3}%\r\n");
                }

                PrintDefect("皮帽", rpt.DefectSkin);
                PrintDefect("脏污", rpt.DefectDirty);
                PrintDefect("偏心", rpt.DefectColor);
                PrintDefect("凹陷", rpt.DefectConcave);
                PrintDefect("凸点", rpt.DefectConvex);
                PrintDefect("气泡", rpt.DefectBubble);
                PrintDefect("黑点", rpt.DefectSolid);
                PrintDefect("黑边", rpt.DefectShape);
                PrintDefect("异形", rpt.DefectAlien);
                PrintDefect("尺寸", rpt.DefectSize);
                PrintDefect("圆度", rpt.DefectRound);
                PrintDefect("色差", rpt.DefectChromatic);

                POS_Output_PrintFontStringA(hPort, 0, 0, 0, 0, 0,
                    "-------------------------------------------\r\n");

                // 切纸
                POS_Control_CutPaper(hPort, 1, 3);
                POS_Control_ReSet(hPort);
            }
            finally
            {
                POS_Port_Close(hPort);
            }
            return true;
        }

        public static void PT_res()
        {
            // 假设从数据库或界面拿到一条记录
            var report = new DTO
            {
                ReportTime = ExtHandler.GetGlobalVar<string>("日期"),
                // 计数
                QualifiedCount = ExtHandler.GetGlobalVar<int>("合格"),
                RejectCount = ExtHandler.GetGlobalVar<int>("剔除"),
                PassCount = ExtHandler.GetGlobalVar<double>("合格率"),

                // 粒径
                AvgDiameter = ExtHandler.GetGlobalVar<double>("粒径均值"),
                MaxDiameter = ExtHandler.GetGlobalVar<double>("粒径最大值"),
                MinDiameter = ExtHandler.GetGlobalVar<double>("粒径最小值"),

                // 圆度
                AvgRoundness = ExtHandler.GetGlobalVar<double>("圆度均值"),
                MaxRoundness = ExtHandler.GetGlobalVar<double>("圆度最大值"),
                MinRoundness = ExtHandler.GetGlobalVar<double>("圆度最小值"),

                // 标准差
                /*
                AvgStdDev = ExtHandler.GetGlobalVar<double>("方差均值"),
                MaxStdDev = ExtHandler.GetGlobalVar<double>("方差最大值"),
                MinStdDev = ExtHandler.GetGlobalVar<double>("方差最小值"),
                */

                // 长/短轴
                AvgLongAxis = ExtHandler.GetGlobalVar<double>("长轴均值"),
                MaxLongAxis = ExtHandler.GetGlobalVar<double>("长轴最大值"),
                MinLongAxis = ExtHandler.GetGlobalVar<double>("长轴最小值"),

                AvgShortAxis = ExtHandler.GetGlobalVar<double>("短轴均值"),
                MaxShortAxis = ExtHandler.GetGlobalVar<double>("短轴最大值"),
                MinShortAxis = ExtHandler.GetGlobalVar<double>("短轴最小值"),

                // 检测设置
                DetectDiameter = ExtHandler.GetGlobalVar<double>("检测粒径"),
                DiameterBias = ExtHandler.GetGlobalVar<double>("粒径偏差"),
                DetectRoundness = ExtHandler.GetGlobalVar<double>("圆度"),
                ColorThreshold = ExtHandler.GetGlobalVar<double>("色差阈值"),

                // 缺陷个数
                DefectSkin = ExtHandler.GetGlobalVar<int>("r_001"),
                DefectDirty = ExtHandler.GetGlobalVar<int>("r_002"),
                DefectColor = ExtHandler.GetGlobalVar<int>("r_003"),
                DefectConcave = ExtHandler.GetGlobalVar<int>("r_004"),
                DefectConvex = ExtHandler.GetGlobalVar<int>("r_005"),
                DefectBubble = ExtHandler.GetGlobalVar<int>("r_006"),
                DefectSolid = ExtHandler.GetGlobalVar<int>("r_007"),
                DefectShape = ExtHandler.GetGlobalVar<int>("r_008"),
                DefectAlien = ExtHandler.GetGlobalVar<int>("r_012"),

                DefectSize = ExtHandler.GetGlobalVar<int>("r_009"),
                DefectRound = ExtHandler.GetGlobalVar<int>("r_010"),
                DefectChromatic = ExtHandler.GetGlobalVar<int>("r_011"),
                type = ExtHandler.GetGlobalVar<String>("type")
                // ……其余字段照填
            };
            PrintReport(report);
        }
    }
}
