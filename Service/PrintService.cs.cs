using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using VisionCore.Ext;
using VisionCore.Log;

namespace RobotLocation.Service
{
    /// <summary>
    /// 打印服务 - 全异步实现
    /// </summary>
    public class PrintService : IDisposable
    {
        #region DLL导入

        [DllImport("POS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "POS_Port_OpenA")]
        private static extern Int32 POS_Port_OpenA(String lpName, Int32 iPort, bool bFile, String path);

        [DllImport("POS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "POS_Port_Close")]
        private static extern Int32 POS_Port_Close(long iPort);

        [DllImport("POS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "POS_Output_PrintData")]
        private static extern Int32 POS_Output_PrintData(long printID, byte[] strBuff, Int32 ilen);

        [DllImport("POS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "POS_Control_AlignType")]
        private static extern Int32 POS_Control_AlignType(long printID, Int32 iAlignType);

        [DllImport("POS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "POS_Output_PrintFontStringA")]
        private static extern Int32 POS_Output_PrintFontStringA(long printID, Int32 iFont, Int32 iThick, Int32 iWidth, Int32 iHeight, Int32 iUnderLine, String lpString);

        [DllImport("POS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "POS_Control_CutPaper")]
        private static extern Int32 POS_Control_CutPaper(long printID, Int32 type, Int32 len);

        [DllImport("POS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "POS_Control_ReSet")]
        private static extern Int32 POS_Control_ReSet(long printID);

        #endregion

        #region 配置

        private const string PRINTER_PORT = "SP-USB1";
        private const int PRINTER_PORT_NUM = 1002;
        private const int MAX_RETRY_COUNT = 3;
        private const int RETRY_INTERVAL_MS = 1000;

        #endregion

        #region 单例

        private static readonly Lazy<PrintService> _instance =
            new Lazy<PrintService>(() => new PrintService(), true);

        public static PrintService Instance => _instance.Value;

        private PrintService() { }

        #endregion

        #region 状态

        public bool IsConnected { get; private set; }
        public bool IsPrinting { get; private set; }
        public string LastError { get; private set; }

        /// <summary>
        /// 打印完成事件
        /// </summary>
        public event EventHandler<PrintResult> PrintCompleted;

        #endregion

        #region 公共异步方法

        /// <summary>
        /// 异步打印报告（主方法）
        /// </summary>
        public async Task<PrintResult> PrintAsync(PrintReportData report)
        {
            // 防止并发打印
            if (IsPrinting)
            {
                return new PrintResult
                {
                    Success = false,
                    ErrorMessage = "正在打印中，请稍候..."
                };
            }

            IsPrinting = true;

            try
            {
                // 数据验证
                if (report == null)
                {
                    return CreateErrorResult("报告数据为空");
                }

                // 在后台线程执行打印
                var result = await Task.Run(() => DoPrintWithRetry(report));

                // 触发完成事件
                PrintCompleted?.Invoke(this, result);

                return result;
            }
            catch (Exception ex)
            {
                LogNet.Error($"打印异常: {ex.Message}");
                return CreateErrorResult($"打印异常: {ex.Message}");
            }
            finally
            {
                IsPrinting = false;
            }
        }

        /// <summary>
        /// 异步从全局变量打印
        /// </summary>
        public async Task<PrintResult> PrintFromGlobalVarsAsync()
        {
            try
            {
                var report = await Task.Run(() => BuildReportFromGlobalVars());
                return await PrintAsync(report);
            }
            catch (Exception ex)
            {
                LogNet.Error($"构建打印数据失败: {ex.Message}");
                return CreateErrorResult($"构建打印数据失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 异步检查打印机连接
        /// </summary>
        public async Task<bool> CheckConnectionAsync()
        {
            return await Task.Run(() =>
            {
                try
                {
                    long hPort = POS_Port_OpenA(PRINTER_PORT, PRINTER_PORT_NUM, false, "");
                    if (hPort >= 0)
                    {
                        POS_Port_Close(hPort);
                        IsConnected = true;
                        return true;
                    }
                    IsConnected = false;
                    LastError = "无法打开打印机端口";
                    return false;
                }
                catch (Exception ex)
                {
                    LastError = $"检查打印机异常: {ex.Message}";
                    LogNet.Error(LastError);
                    IsConnected = false;
                    return false;
                }
            });
        }

        /// <summary>
        /// 异步打印（带UI回调）
        /// </summary>
        /// <param name="report">报告数据</param>
        /// <param name="uiCallback">UI线程回调</param>
        public async void PrintAsync(PrintReportData report, Action<PrintResult> uiCallback)
        {
            var result = await PrintAsync(report);
            uiCallback?.Invoke(result);
        }

        /// <summary>
        /// 异步从全局变量打印（带UI回调）
        /// </summary>
        public async void PrintFromGlobalVarsAsync(Action<PrintResult> uiCallback)
        {
            var result = await PrintFromGlobalVarsAsync();
            uiCallback?.Invoke(result);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 带重试的打印
        /// </summary>
        private PrintResult DoPrintWithRetry(PrintReportData rpt)
        {
            for (int retry = 0; retry < MAX_RETRY_COUNT; retry++)
            {
                try
                {
                    if (retry > 0)
                    {
                        LogNet.Info($"打印重试 ({retry + 1}/{MAX_RETRY_COUNT})...");
                        Thread.Sleep(RETRY_INTERVAL_MS);
                    }

                    bool success = DoPrint(rpt);
                    if (success)
                    {
                        LogNet.Info("打印成功");
                        return new PrintResult { Success = true };
                    }
                }
                catch (DllNotFoundException ex)
                {
                    LogNet.Error($"打印驱动未找到: {ex.Message}");
                    return CreateErrorResult("打印驱动未安装或损坏");
                }
                catch (SEHException ex)
                {
                    LogNet.Error($"打印机通信异常: {ex.Message}");
                    // 通信异常可以重试
                }
                catch (Exception ex)
                {
                    LogNet.Error($"打印异常: {ex.Message}");
                }
            }

            return CreateErrorResult("打印失败，已重试3次");
        }

        /// <summary>
        /// 执行打印
        /// </summary>
        private bool DoPrint(PrintReportData rpt)
        {
            long hPort = -1;

            try
            {
                hPort = POS_Port_OpenA(PRINTER_PORT, PRINTER_PORT_NUM, false, "");
                if (hPort < 0)
                {
                    LastError = "无法打开打印机端口";
                    return false;
                }

                // 初始化
                byte[] cmd = { 0x1c, 0x26 };
                POS_Output_PrintData(hPort, cmd, 2);
                POS_Control_AlignType(hPort, 1);

                // 标题
                PrintString(hPort, "XOP1080 视觉选丸仪\r\n");
                PrintString(hPort, "检测数据\r\n");
                POS_Control_AlignType(hPort, 0);

                // 时间型号
                PrintString(hPort, $"{rpt.ReportTime:yyyy-MM-dd HH:mm:ss}    {rpt.ModelName}\r\n");
                PrintString(hPort, "-------------------------------------------\r\n");

                // 检测参数
                PrintString(hPort, $"检测牌号：    {rpt.BrandName}\r\n");
                PrintString(hPort, $"检测直径：    {rpt.DetectDiameter:F3}\r\n");
                PrintString(hPort, $"检测偏差：    {rpt.DiameterBias:F3}\r\n");
                PrintString(hPort, $"检测圆度：    {rpt.DetectRoundness:F3}\r\n");
                PrintString(hPort, $"色差阈值：    {rpt.ColorThreshold:F3}\r\n");
                PrintString(hPort, "-------------------------------------------\r\n");

                // 计数
                PrintString(hPort, $"合格:         {rpt.QualifiedCount}次\r\n");
                PrintString(hPort, $"剔除:         {rpt.RejectCount}粒\r\n");
                PrintString(hPort, $"合格率:       {rpt.PassRate:F2}%\r\n");
                PrintString(hPort, "-------------------------------------------\r\n");

                // 数值区间
                PrintString(hPort, "              Agv       Max         Min\r\n");
                PrintRange(hPort, "粒径  ", rpt.AvgDiameter, rpt.MaxDiameter, rpt.MinDiameter);
                PrintRange(hPort, "圆度值", rpt.AvgRoundness, rpt.MaxRoundness, rpt.MinRoundness);
                PrintRange(hPort, "长轴  ", rpt.AvgLongAxis, rpt.MaxLongAxis, rpt.MinLongAxis);
                PrintRange(hPort, "短轴  ", rpt.AvgShortAxis, rpt.MaxShortAxis, rpt.MinShortAxis);
                PrintString(hPort, "-------------------------------------------\r\n");

                // 缺陷
                PrintDefect(hPort, "皮帽", rpt.DefectSkin, rpt.TotalDefects);
                PrintDefect(hPort, "脏污", rpt.DefectDirty, rpt.TotalDefects);
                PrintDefect(hPort, "偏心", rpt.DefectColor, rpt.TotalDefects);
                PrintDefect(hPort, "凹陷", rpt.DefectConcave, rpt.TotalDefects);
                PrintDefect(hPort, "凸点", rpt.DefectConvex, rpt.TotalDefects);
                PrintDefect(hPort, "气泡", rpt.DefectBubble, rpt.TotalDefects);
                PrintDefect(hPort, "黑点", rpt.DefectSolid, rpt.TotalDefects);
                PrintDefect(hPort, "黑边", rpt.DefectShape, rpt.TotalDefects);
                PrintDefect(hPort, "异形", rpt.DefectAlien, rpt.TotalDefects);
                PrintDefect(hPort, "尺寸", rpt.DefectSize, rpt.TotalDefects);
                PrintDefect(hPort, "圆度", rpt.DefectRound, rpt.TotalDefects);
                PrintDefect(hPort, "色差", rpt.DefectChromatic, rpt.TotalDefects);

                PrintString(hPort, "-------------------------------------------\r\n");

                // 切纸
                POS_Control_CutPaper(hPort, 1, 3);
                POS_Control_ReSet(hPort);

                return true;
            }
            finally
            {
                if (hPort >= 0)
                {
                    try { POS_Port_Close(hPort); } catch { }
                }
            }
        }

        private void PrintString(long hPort, string text)
        {
            try
            {
                POS_Output_PrintFontStringA(hPort, 0, 0, 0, 0, 0, text ?? "");
            }
            catch (Exception ex)
            {
                LogNet.Warn($"打印字符串异常: {ex.Message}");
            }
        }

        private void PrintRange(long hPort, string title, double avg, double max, double min)
        {
            PrintString(hPort, $" {title}:     {avg:F3}    {max:F3}    {min:F3}\r\n");
        }

        private void PrintDefect(long hPort, string name, int count, int total)
        {
            double percent = total == 0 ? 0 : (double)count / total * 100;
            PrintString(hPort, $"{name}：    {percent:F2}%\r\n");
        }

        private PrintReportData BuildReportFromGlobalVars()
        {
            return new PrintReportData
            {
                ReportTime = DateTime.Now,
                ModelName = SafeGetGlobalVar("AutoLoadSolName", "未知"),
                BrandName = SafeGetGlobalVar("type", "XB001"),
                DetectDiameter = SafeGetGlobalVar("检测粒径", 0.0),
                DiameterBias = SafeGetGlobalVar("粒径偏差", 0.0),
                DetectRoundness = SafeGetGlobalVar("圆度", 0.0),
                ColorThreshold = SafeGetGlobalVar("色差阈值", 0.0),
                QualifiedCount = SafeGetGlobalVar("合格", 0),
                RejectCount = SafeGetGlobalVar("剔除", 0),
                PassRate = SafeGetGlobalVar("合格率", 0.0),
                AvgDiameter = SafeGetGlobalVar("粒径均值", 0.0),
                MaxDiameter = SafeGetGlobalVar("粒径最大值", 0.0),
                MinDiameter = SafeGetGlobalVar("粒径最小值", 0.0),
                AvgRoundness = SafeGetGlobalVar("圆度均值", 0.0),
                MaxRoundness = SafeGetGlobalVar("圆度最大值", 0.0),
                MinRoundness = SafeGetGlobalVar("圆度最小值", 0.0),
                AvgLongAxis = SafeGetGlobalVar("长轴均值", 0.0),
                MaxLongAxis = SafeGetGlobalVar("长轴最大值", 0.0),
                MinLongAxis = SafeGetGlobalVar("长轴最小值", 0.0),
                AvgShortAxis = SafeGetGlobalVar("短轴均值", 0.0),
                MaxShortAxis = SafeGetGlobalVar("短轴最大值", 0.0),
                MinShortAxis = SafeGetGlobalVar("短轴最小值", 0.0),
                DefectSkin = SafeGetGlobalVar("r_001", 0),
                DefectDirty = SafeGetGlobalVar("r_002", 0),
                DefectColor = SafeGetGlobalVar("r_003", 0),
                DefectConcave = SafeGetGlobalVar("r_004", 0),
                DefectConvex = SafeGetGlobalVar("r_005", 0),
                DefectBubble = SafeGetGlobalVar("r_006", 0),
                DefectSolid = SafeGetGlobalVar("r_007", 0),
                DefectShape = SafeGetGlobalVar("r_008", 0),
                DefectAlien = SafeGetGlobalVar("r_012", 0),
                DefectSize = SafeGetGlobalVar("r_009", 0),
                DefectRound = SafeGetGlobalVar("r_010", 0),
                DefectChromatic = SafeGetGlobalVar("r_011", 0)
            };
        }

        private T SafeGetGlobalVar<T>(string key, T defaultValue)
        {
            try { return ExtHandler.GetGlobalVar<T>(key); }
            catch { return defaultValue; }
        }

        private PrintResult CreateErrorResult(string message)
        {
            LastError = message;
            return new PrintResult { Success = false, ErrorMessage = message };
        }

        #endregion

        public void Dispose() { }
    }

    #region 数据模型

    public class PrintReportData
    {
        public DateTime ReportTime { get; set; }
        public string ModelName { get; set; }
        public string BrandName { get; set; }
        public double DetectDiameter { get; set; }
        public double DiameterBias { get; set; }
        public double DetectRoundness { get; set; }
        public double ColorThreshold { get; set; }
        public int QualifiedCount { get; set; }
        public int RejectCount { get; set; }
        public double PassRate { get; set; }
        public double AvgDiameter { get; set; }
        public double MaxDiameter { get; set; }
        public double MinDiameter { get; set; }
        public double AvgRoundness { get; set; }
        public double MaxRoundness { get; set; }
        public double MinRoundness { get; set; }
        public double AvgLongAxis { get; set; }
        public double MaxLongAxis { get; set; }
        public double MinLongAxis { get; set; }
        public double AvgShortAxis { get; set; }
        public double MaxShortAxis { get; set; }
        public double MinShortAxis { get; set; }
        public int DefectSkin { get; set; }
        public int DefectDirty { get; set; }
        public int DefectColor { get; set; }
        public int DefectConcave { get; set; }
        public int DefectConvex { get; set; }
        public int DefectBubble { get; set; }
        public int DefectSolid { get; set; }
        public int DefectShape { get; set; }
        public int DefectAlien { get; set; }
        public int DefectSize { get; set; }
        public int DefectRound { get; set; }
        public int DefectChromatic { get; set; }

        public int TotalDefects => DefectSkin + DefectDirty + DefectColor + DefectConcave +
                                   DefectConvex + DefectBubble + DefectSolid + DefectShape +
                                   DefectAlien + DefectSize + DefectRound + DefectChromatic;
    }

    public class PrintResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }

    #endregion
}
