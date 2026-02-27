using HalconDotNet;
using ImageControl;
using LittleCommon.Domain;
using LittleCommon.Tool;
using RobotLocation.Model;
using Sunny.UI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using VisionCore.BasePlugin.Else;
using VisionCore.Ext;
using VisionCore.Log;
using WENYU_EIO32P;

namespace RobotLocation.Service
{
    internal class ToneOp
    {
        //private static int[] LastCameraOrdersI = new int[3]; // 上一次的拍照次数
        //private static int[] LastCameraOrdersO = new int[3]; // 上一次的拍照次数

        public static int CameraI = 0;
        public static int CameraO = 0;
        public static ConcurrentQueue<int> RQI = new ConcurrentQueue<int>(); // 新开一个不影响数据的消息队列，内排用
        public static ConcurrentQueue<int> RQO = new ConcurrentQueue<int>(); // 新开一个不影响数据的消息，外排用
        public static List<ToneProcess> ToneProcesses = new List<ToneProcess>();
        public static int WaitTime1 = 70; // 内排取结果时间，76.92ms
        public static int WaitTime2 = 60;  // 外排取结果时间，理论66.66ms
        public static HTuple[][] BatchResults = new HTuple[6][]; // 每个相机有4个批次结果
        public static bool[] CameraSignalFlags = new bool[6]; // 相机标志信号
        public static ConcurrentDictionary<int, int>[] IOSignalFlags = new ConcurrentDictionary<int, int>[6]; // 6个相机，每个相机的信号值字典
        public static bool[] 计算完成 = new bool[6]; // 结果标记，确保每次拿到结果
        public static Thread SignalManagerThread; // 信号管理线程
        public static Thread IOManagerII;
        public static Thread IOManagerIO;

        public static bool OpenTWO()
        {
            int Result;
            long VersionNumber = 0;

            Result = WENYU_EIO32P.Program.WY_Open();

            if (Result == 0)
            {
                MessageBox.Show("板卡没有找到！", "提示", MessageBoxButtons.OK);
                // System.Environment.Exit(0);
            }

            Result = WENYU_EIO32P.Program.WY_GetCardVersion(0, ref VersionNumber);
            if (Result == 0) { }
            else MessageBox.Show("板卡1---通讯异常！", "提示", MessageBoxButtons.OK);

            Result = WENYU_EIO32P.Program.WY_GetCardVersion(1, ref VersionNumber);
            if (Result == 0) { }
            else MessageBox.Show("板卡2---通讯异常！", "提示", MessageBoxButtons.OK);
            return true;
        }

        public static bool CloseTWO()
        {
            int Result;
            Result = WENYU_EIO32P.Program.WY_Close();
            return true;
        }

        public static Result Init()
        {
            Result r = new Result();
            bool r1 = OpenTWO();
            bool r2 = WY_SetInterruptFun();

            r.msg = "";
            if (!r1)
            {
                r.msg += "IO卡打开失败,";
            }
            if (!r2)
            {
                r.msg += "IO卡中断绑定失败,";
            }

            ToneProcesses.Add(new ToneProcess() { Name = "内1" });
            ToneProcesses.Add(new ToneProcess() { Name = "内2" });
            ToneProcesses.Add(new ToneProcess() { Name = "内3" });
            ToneProcesses.Add(new ToneProcess() { Name = "外1" });
            ToneProcesses.Add(new ToneProcess() { Name = "外2" });
            ToneProcesses.Add(new ToneProcess() { Name = "外3" });

            for (int i = 0; i < 6; i++)
            {
                BatchResults[i] = new HTuple[4];
                IOSignalFlags[i] = new ConcurrentDictionary<int, int>(); // 初始化每个相机的信号值字典
            }

            bool r4 = true;
            foreach (var p in ToneProcesses)
            {
                var pr = p.Init();
                if (!pr.status)
                {
                    r4 = false;
                    r.msg += pr.msg;
                }
            }

            r.status = r1 && r4;

            for (int i = 0; i < ToneProcesses.Count; i++)
            {
                int cameraIndex = i;
                int waitTime = i < 3 ? WaitTime1 : WaitTime2;
                new Thread(() => ProcessCamera(cameraIndex, waitTime))
                {
                    IsBackground = true
                }.Start();
            }

            // IO线程
            IOManagerII = new Thread(ProcessIOI);
            IOManagerII.IsBackground = true;
            IOManagerII.Start();

            IOManagerIO = new Thread(ProcessIOO);
            IOManagerIO.IsBackground = true;
            IOManagerIO.Start();

            return r;
        }

        private static void ProcessCamera(int cameraIndex, int waitTime)
        {
            int step = 0;
            int curOrder = 1;
            double resTime = 0;

            while (true)
            {
                Thread.Sleep(1);
                switch (step)
                {
                    case 0:
                        if (ToneProcesses[cameraIndex].ResQueuqe.TryDequeue(out curOrder))
                        {
                            //resTime = HighTime.GetMSec() + waitTime;
                            step = 1;
                        }
                        break;

                    case 1:
                        double cur = HighTime.GetMSec();

                        if (ToneProcesses[cameraIndex].ProductMap.TryGetValue(curOrder, out var pp) &&
                            pp != null && pp.Complete)
                        {
                            if (ToneProcesses[cameraIndex].ProductMap.TryRemove(curOrder, out var p))
                            {
                                DetermineRejection(p, cameraIndex, cameraIndex < 3 ? 0 : 1, BatchResults[cameraIndex]);
                                计算完成[cameraIndex] = true;
                                step = 0;
                            }
                        }

                        break;
                }
            }
        }
        //原来的
        private static void ProcessIOO()
        {
            var stopwatch = new Stopwatch();
            int RQnum = 0;
            while (true)
            {
                Thread.Sleep(1);
                if (RQO.TryDequeue(out RQnum))
                {
                    for (int i = 3; i < 6; i++)
                    {
                        if (IOSignalFlags[i].TryRemove(RQnum - 6, out int ResInt))
                        {
                            // 函数调用1
                            WENYU_EIO32P.Program.WY_SetHighOutPutData(0, ResInt);
                            stopwatch.Restart();
                            while (stopwatch.ElapsedMilliseconds < 2) // 保持1毫秒
                            {
                                System.Threading.Thread.SpinWait(1);
                            }
                            stopwatch.Stop();

                            // 函数调用2
                            WENYU_EIO32P.Program.WY_WriteOutPutBit(1, i, 1);//KAI!!!
                            stopwatch.Restart();
                            while (stopwatch.ElapsedMilliseconds < 2) // 保持1毫秒
                            {
                                System.Threading.Thread.SpinWait(1);
                            }
                            stopwatch.Stop();
                            // 函数调用4
                            WENYU_EIO32P.Program.WY_WriteOutPutBit(1, i, 0);
                            stopwatch.Restart();
                            while (stopwatch.ElapsedMilliseconds < 2) // 保持1毫秒
                            {
                                System.Threading.Thread.SpinWait(1);
                            }
                            stopwatch.Stop();
                            // 函数调用3
                            WENYU_EIO32P.Program.WY_SetHighOutPutData(0, 255);
                            stopwatch.Restart();
                            while (stopwatch.ElapsedMilliseconds < 2) // 保持1毫秒
                            {
                                System.Threading.Thread.SpinWait(1);
                            }
                            stopwatch.Stop();
                        }
                    }
                }
            }
        }

        private static void ProcessIOI()
        {

            var stopwatch = new Stopwatch();
            int RQnum = 0;
            while (true)
            {
                Thread.Sleep(1);
                if (RQI.TryDequeue(out RQnum))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (IOSignalFlags[i].TryRemove(RQnum - 6, out int ResInt))
                        {
                            // 函数调用1
                            WENYU_EIO32P.Program.WY_SetLowOutPutData(0, ResInt);
                            stopwatch.Restart();
                            while (stopwatch.ElapsedMilliseconds < 2) // 保持1毫秒
                            {
                                System.Threading.Thread.SpinWait(1);
                            }
                            stopwatch.Stop();

                            // 函数调用2
                            WENYU_EIO32P.Program.WY_WriteOutPutBit(1, i, 1);//KAI!!!
                            stopwatch.Restart();
                            while (stopwatch.ElapsedMilliseconds < 2) // 保持1毫秒
                            {
                                System.Threading.Thread.SpinWait(1);
                            }
                            stopwatch.Stop();
                            // 函数调用4
                            WENYU_EIO32P.Program.WY_WriteOutPutBit(1, i, 0);
                            stopwatch.Restart();
                            while (stopwatch.ElapsedMilliseconds < 2) // 保持1毫秒
                            {
                                System.Threading.Thread.SpinWait(1);
                            }
                            stopwatch.Stop();
                            // 函数调用3
                            WENYU_EIO32P.Program.WY_SetLowOutPutData(0, 255);
                            stopwatch.Restart();
                            while (stopwatch.ElapsedMilliseconds < 2) // 保持1毫秒
                            {
                                System.Threading.Thread.SpinWait(1);
                            }
                            stopwatch.Stop();
                        }
                    }
                }
            }

        }

        public static void DetermineRejection(Product p, int camera, int cardid, HTuple[] BatchResults)
        {
            try
            {
                int Signal = 0; // 使用整数表示信号标志，初始值为0
                int batchIndex = p.Order % 4;

                if (p.Res == null)
                {
                    HOperatorSet.TupleGenConst(32, 0, out HTuple p1); // 如果结果为空，生成一个长度为32的全零HTuple
                    BatchResults[batchIndex] = p1;
                }
                else
                {
                    BatchResults[batchIndex] = p.Res; // 将当前结果存储到对应批次
                }

                if (p.Order >= 4)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        bool shouldReject = false; // 当前孔位的剔除标记
                        int row1 = 24 + i;
                        int row2 = 16 + i;
                        int row3 = 8 + i;

                        switch (batchIndex)
                        {
                            case 0:
                                shouldReject = BatchResults[1][row1] == 1 || BatchResults[2][row2] == 1 || BatchResults[3][row3] == 1 || BatchResults[0][i] == 1;
                                break;
                            case 1:
                                shouldReject = BatchResults[2][row1] == 1 || BatchResults[3][row2] == 1 || BatchResults[0][row3] == 1 || BatchResults[1][i] == 1;
                                break;
                            case 2:
                                shouldReject = BatchResults[3][row1] == 1 || BatchResults[0][row2] == 1 || BatchResults[1][row3] == 1 || BatchResults[2][i] == 1;
                                break;
                            case 3:
                                shouldReject = BatchResults[0][row1] == 1 || BatchResults[1][row2] == 1 || BatchResults[2][row3] == 1 || BatchResults[3][i] == 1;
                                break;
                        }

                        if (shouldReject)
                        {
                            // 将第 i 位设置为 1
                            Signal |= (1 << i);
                        }
                        else
                        {
                            // 将第 i 位设置为 0
                            Signal &= ~(1 << i);
                        }
                    }
                }
                // 按位取反
                Signal = ~Signal;
                // 存储信号值到IOSignalFlags
                IOSignalFlags[camera].TryAdd(p.Order, Signal & 0xFF);

                // 输出到日志
                //LogNet.Info($"相机{camera}, 拍照次数{p.Order}, 信号值: {Signal & 0xFF} (二进制: {Convert.ToString(Signal & 0xFF, 2).PadLeft(8, '0')})");
            }
            catch (Exception ex)
            {
                LogNet.Error($"剔除判断异常: 相机编号={camera}, 批次={p.Order}, 异常信息={ex.ToString()}");
            }
        }
        public static bool WY_SetInterruptFun()
        {
            int Result = WENYU_EIO32P.Program.WY_SetInterruptFun(1, eventInterrupt); // 设置中断处理函数
            if (Result != 0)
            {
                LogNet.Error("设置中断处理函数失败，错误码：" + Result);
                return false;
            }

            Result = WENYU_EIO32P.Program.WY_SetInterruptEnable(1, 1); // 启用中断
            if (Result != 0)
            {
                LogNet.Error("启用中断失败，错误码：" + Result);
                return false;
            }

            byte RiseEnable = 0xff; // 初始化输入端口上升沿中断
            byte FallEnable = 0xff; // 初始化输入端口下降沿中断
            Result = WENYU_EIO32P.Program.WY_SetInterruptPortEnable(1, RiseEnable, FallEnable); // 设置中断端口
            if (Result != 0)
            {
                LogNet.Error("设置中断端口失败，错误码：" + Result);
                return false;
            }

            return true; // 返回成功标志
        }

        private static WENYU_EIO32P.EventInterrupt eventInterrupt = new EventInterrupt(InterruptFun);

        private static void InterruptFun()
        {
            UInt16 RiseData = 0;
            UInt16 FallData = 0;
            long InPutData = 0;

            int Result = WENYU_EIO32P.Program.WY_GetInterruptData(1, ref RiseData, ref FallData); // 获取中断数据
            if (Result != 0)
            {
                LogNet.Error("获取中断数据失败，错误码：" + Result);
                return;
            }

            Result = WENYU_EIO32P.Program.WY_GetInPutData(1, ref InPutData); // 获取输入数据
            if (Result != 0)
            {
                LogNet.Error("获取输入数据失败，错误码：" + Result);
                return;
            }

            if (RiseData != 0xffff)
            {
                if ((RiseData & 0x0001) == 0x0000) // 检测DI00上升沿
                {
                    if ((InPutData & 0x0001) == 0x0001)
                    {
                        // 处理内排中断
                        CameraI++;
                        RQI.Enqueue(CameraI);
                    }
                    Result = WENYU_EIO32P.Program.WY_ClrInterruptRisePort0(1); // 清除中断标志
                    if (Result != 0)
                    {
                        LogNet.Error("清除DI00上升沿中断标志失败，错误码：" + Result);
                    }
                }

                if ((RiseData & 0x0002) == 0x0000) // 检测DI01上升沿
                {
                    if ((InPutData & 0x0002) == 0x0002)
                    {
                        // 处理外排中断
                        CameraO++;
                        RQO.Enqueue(CameraO);
                    }
                    Result = WENYU_EIO32P.Program.WY_ClrInterruptRisePort1(1); // 清除中断标志
                    if (Result != 0)
                    {
                        LogNet.Error("清除DI01上升沿中断标志失败，错误码：" + Result);
                    }
                }
            }

            WENYU_EIO32P.Program.WY_ResetInterrupt(1); // 重置中断
        }

        public static void Destroy()
        {
            try
            {
                foreach (var p in ToneProcesses)
                {
                    p.Destroy();
                }

                CloseTWO();

                if (SignalManagerThread != null && SignalManagerThread.IsAlive)
                {
                    SignalManagerThread.Interrupt();
                    SignalManagerThread.Join();
                }
                //if (IOManagerII != null && IOManagerII.IsAlive)
                //{
                //    IOManagerII.Interrupt();
                //    IOManagerII.Join();
                //}

                //if (IOManagerIO != null && IOManagerIO.IsAlive)
                //{
                //    IOManagerIO.Interrupt();
                //    IOManagerIO.Join();
                //}
                RQI = new ConcurrentQueue<int>();
                RQO = new ConcurrentQueue<int>();

                LogNet.Info("资源清理完成，程序已安全退出。");
            }
            catch (Exception ex)
            {
                LogNet.Error($"资源清理失败: {ex.ToString()}");
            }
        }
    }
}