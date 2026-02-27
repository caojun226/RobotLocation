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
        //缓存拍照次数，用于刷新IO
        private static int[] LastCameraOrdersI = new int[3]; // 上一次的拍照次数
        private static int[] LastCameraOrdersO = new int[3]; // 上一次的拍照次数

        public static int CameraI = 0;
        public static int CameraO = 0;
        public static ConcurrentQueue<int> RQI = new ConcurrentQueue<int>();//新开一个不影响数据的消息队列，内排用
        public static ConcurrentQueue<int> RQO = new ConcurrentQueue<int>();//新开一个不影响数据的消息，外排用
        //来图了

        // 相机
        public static List<ToneProcess> ToneProcesses = new List<ToneProcess>();
        public static int WaitTime1 = 70; // 内排取结果时间，76.92ms
        public static int WaitTime2 = 60;  // 外排取结果时间，理论66.66ms

        // 结果数组
        public static HTuple[][] BatchResults = new HTuple[6][]; // 每个相机有4个批次结果

        // 信号标志
        public static bool[] CameraSignalFlags = new bool[6]; // 相机标志信号
        public static bool[][] IOSignalFlags = new bool[6][]; // 孔位信号标志
        public static bool[] 计算完成 = new bool[6]; // 结果标记，确保每次拿到结果

        // 线程
        public static Thread SignalManagerThread; // 信号管理线程

        //IO
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

            //WENYUPCIE

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
            //long VersionNumber = 0;
            //int ResultIo = 0;
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

            // 初始化相机处理逻辑
            ToneProcesses.Add(new ToneProcess() { Name = "内1" });
            ToneProcesses.Add(new ToneProcess() { Name = "内2" });
            ToneProcesses.Add(new ToneProcess() { Name = "内3" });
            ToneProcesses.Add(new ToneProcess() { Name = "外1" });
            ToneProcesses.Add(new ToneProcess() { Name = "外2" });
            ToneProcesses.Add(new ToneProcess() { Name = "外3" });

            // 初始化结果数组和信号标志
            for (int i = 0; i < 6; i++)
            {
                BatchResults[i] = new HTuple[4];
                IOSignalFlags[i] = new bool[8];
            }

            // 初始化相机
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

            // 启动内排信号管理线程
            //SignalManagerThread = new Thread(SignalManagerI);
            //SignalManagerThread.IsBackground = true;
            //SignalManagerThread.Start();

            //// 启动外排信号管理线程
            //SignalManagerThread = new Thread(SignalManagerO);
            //SignalManagerThread.IsBackground = true;
            //SignalManagerThread.Start();

            // 启动每个相机的处理线程
            for (int i = 0; i < ToneProcesses.Count; i++)
            {
                int cameraIndex = i;
                int waitTime = i < 3 ? WaitTime1 : WaitTime2;
                new Thread(() => ProcessCamera(cameraIndex, waitTime))
                {
                    IsBackground = true
                }.Start();
            }

            return r;
        }

        // 相机处理逻辑

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
                            resTime = HighTime.GetMSec() + waitTime;
                            step = 1;
                            //LogNet.Info($"相机{cameraIndex}获取到新的订单号：{curOrder}");                            
                        }
                        break;

                    case 1:
                        double cur = HighTime.GetMSec();
                        if (cur < resTime)
                        {
                            if (ToneProcesses[cameraIndex].ProductMap.TryGetValue(curOrder, out var pp) &&
                                pp != null && pp.Complete)
                            {
                                if (ToneProcesses[cameraIndex].ProductMap.TryRemove(curOrder, out var p))
                                {
                                    DetermineRejection(p, cameraIndex, cameraIndex < 3 ? 0 : 1, BatchResults[cameraIndex]);
                                    计算完成[cameraIndex] = true;
                                    step = 0;
                                    //LogNet.Info($"相机{cameraIndex}处理完成订单号：{curOrder}");
                                }
                            }
                        }
                        else
                        {
                            if (ToneProcesses[cameraIndex].ProductMap.TryRemove(curOrder, out var p))
                            {
                                HTuple hv_Newtuple;
                                HOperatorSet.TupleGenConst(32, 0, out hv_Newtuple);
                                p.Res = hv_Newtuple;
                                DetermineRejection(p, cameraIndex, cameraIndex < 3 ? 0 : 1, BatchResults[cameraIndex]);
                            }
                            LogNet.Warn($"相机{cameraIndex}算法超时，丢失检测数据，订单号：{curOrder}");
                            计算完成[cameraIndex] = false;
                            step = 0;
                        }
                        break;
                }
            }
        }

        //内排数据处理，不带缓存版

        //private static async void SignalManagerI()
        //{
        //    try
        //    {
        //        int RqId = 0;
        //        int stepI = 0;
        //        while (true)
        //        {
        //            for (int camera = 0; camera < 3; camera++)
        //            {
        //                switch (stepI)
        //                {

        //                    case 0:
        //                        if (ToneProcesses[camera].RQI.TryDequeue(out RqId))
        //                        {
        //                            stepI = 1;
        //                        }
        //                        break;
        //                    case 1:
        //                        if (计算完成[camera])
        //                        {
        //                            for (int i = 0; i < 8; i++)
        //                            {
        //                                if (IOSignalFlags[camera][i])
        //                                {
        //                                    WENYUPCIE.WenYu.WriteIO(0, i, 0); // 打开孔位信号,外排
        //                                }
        //                            }
        //                            if (CameraSignalFlags[camera])
        //                            {
        //                                WENYUPCIE.WenYu.WriteOutputIO(1, camera, 1); // 打开相机标志信号
        //                                                                             //3.输出日志
        //                                LogNet.Info($"相机 {camera} 信号: {string.Join(", ", IOSignalFlags[camera])}次数{RqId}计算ID:{ToneProcesses[camera].curOrder}");

        //                                CameraSignalFlags[camera] = false; // 清除相机标志信号标志
        //                            }
        //                            //4.清除孔位信号标志
        //                            for (int i = 0; i < 8; i++)
        //                            {
        //                                IOSignalFlags[camera][i] = false; // 清除孔位信号标志
        //                                WENYUPCIE.WenYu.WriteIO(0, i, 1); // 关闭孔位信号,内排
        //                            }
        //                            计算完成[camera] = false;
        //                            stepI = 0;

        //                        }
        //                        break;
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogNet.Error($"信号管理线程异常: {ex.ToString()}");
        //    }

        //    //如果不需要刷新信号，则短暂休眠以避免过度占用CPU


        //        Thread.Sleep(1); // 短暂休眠，避免过度占用CPU

        //}

        //外排处理逻辑，不带缓存版
        //private static void SignalManagerO()
        //{
        //    try
        //    {

        //        int RqId = 0;
        //        //int stepO = 0;
        //        while (true)
        //        {

        //            for (int camera = 3; camera < 6; camera++)
        //            {

        //                if (计算完成[camera])
        //                {
        //                    //int signalFlagsInt = BoolArrayToInt(IOSignalFlags[camera]);
        //                    //LogNet.Info("xiangji   " + camera + "数据是   " + signalFlagsInt);
        //                    for (int i = 0; i < 8; i++)
        //                    {
        //                        if (IOSignalFlags[camera][i])
        //                        {
        //                            WENYU_EIO32P.Program.WY_WriteOutPutBit(0, i + 8, 0);
        //                            //WENYUPCIE.WenYu.WriteIO(0, i + 8, 0); // 打开孔位信号,外排
        //                            //Thread.Sleep(1);
        //                        }
        //                    }
   
        //                    //WENYUPCIE.WenYu.WriteIO(0, i + 8, 0);
        //                    if (CameraSignalFlags[camera])
        //                    {
        //                        //Thread.Sleep(1);
        //                        //WENYUPCIE.WenYu.WriteOutputIO(1, camera, 1); 

        //                        //switch (camera)
        //                        //{
        //                        //    case 3:
        //                        //        WENYU_EIO32P.Program.WY_SetLowOutPutData(1, 247);
        //                        //        break;
        //                        //    case 4:
        //                        //        WENYU_EIO32P.Program.WY_SetLowOutPutData(1, 239);
        //                        //        break;
        //                        //    case 5:
        //                        //        WENYU_EIO32P.Program.WY_SetLowOutPutData(1, 223);
        //                        //        break;
        //                        //}
        //                        WENYU_EIO32P.Program.WY_WriteOutPutBit(1, camera, 0);
        //                        Thread.Sleep(2);
        //                        WENYU_EIO32P.Program.WY_WriteOutPutBit(1, camera, 1);

        //                        // 3. 输出日志
        //                        LogNet.Info($"相机 {camera} 信号: {string.Join(", ", IOSignalFlags[camera])}次数{RqId}计算ID:{ToneProcesses[camera].curOrder}");
        //                        CameraSignalFlags[camera] = false; // 清除相机标志信号标志
        //                    }
        //                    //4.清除孔位信号标志
        //                    for (int i = 0; i < 8; i++)
        //                    {
        //                        IOSignalFlags[camera][i] = false; // 清除孔位信号标志
        //                        WENYU_EIO32P.Program.WY_WriteOutPutBit(0, i + 8, 1);
        //                    }
        //                    //Thread.Sleep(2);
        //                    //WENYUPCIE.WenYu.WriteIO(1, camera, 1);
        //                    计算完成[camera] = false;
        //                    //stepO = 0;

        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogNet.Error($"信号管理线程异常: {ex.ToString()}");
        //    }

        //    // 如果不需要刷新信号，则短暂休眠以避免过度占用CPU

        //    Thread.Sleep(1); // 短暂休眠，避免过度占用CPU

        //}


        //数组转整数
        public static int BoolArrayToInt(bool[] boolArray)
        {
            if (boolArray == null || boolArray.Length != 8)
            {
                throw new ArgumentException("数组必须是8位长。");
            }

            int result = 0;
            for (int i = 0; i < boolArray.Length; i++)
            {
                if (boolArray[i])
                {
                    result |= 1 << (7 - i); // 将1左移(7-i)位，然后与result进行或操作
                }
            }
            return result;
        }
        //外派信号逻辑，带缓存版本
        //private static void SignalManagerO()
        //{
        //    try
        //    {
        //        // 缓存列表，存储相机信号数组和状态标记
        //        List<(int Camera, bool[] SignalFlags, bool IsCameraSignal, int Order)> cache = new List<(int, bool[], bool, int)>();
        //        int stepO = 0;
        //        cache.Clear();
        //        while (true)
        //        {
        //            // 遍历相机编号 3 到 5
        //            for (int camera = 3; camera < 6; camera++)
        //            {
        //                switch (stepO)
        //                {

        //                    case 0:
        //                        // 检查计算完成标志，存入缓存
        //                        if (计算完成[camera])
        //                        {
        //                            bool[] signalFlags = new bool[8];
        //                            Array.Copy(IOSignalFlags[camera], signalFlags, 8);
        //                            bool isCameraSignal = CameraSignalFlags[camera];
        //                            // 将数据加入缓存
        //                            cache.Add((camera, signalFlags, isCameraSignal, ToneProcesses[camera].curOrder));
        //                            //LogNet.Info("---------------------------------------------");
        //                            //foreach (var item in cache)
        //                            // {
        //                            //     if (item.Camera == 3)
        //                            //        {
        //                            //            LogNet.Info($"缓存大小={cache.Count}, 相机={item.Camera}, 数据=[{string.Join(", ", item.SignalFlags)}], 是否剔除={item.IsCameraSignal}, 图片序列={item.Order}");
        //                            //      }
        //                            // }    
        //                            // 如果缓存超过5个，移除最早的数据
        //                            if (cache.Count >= 5)
        //                            {
        //                                cache.RemoveAt(0);
        //                            }

        //                            // 清除计算完成标志
        //                            计算完成[camera] = false;
        //                            //ToneProcesses[camera].RQO.TryDequeue(out int RqIdI);
        //                        }
        //                        stepO = 1;
        //                        break;

        //                    case 1:
        //                        //检查队列是否有数据
        //                        if (ToneProcesses[camera].RQO.TryDequeue(out int RqId)) ;
        //                        {
        //                            // 如果缓存中有数据，取出最早的数据并处理
        //                            if (cache.Count > 3)
        //                            {

        //                                var (cachedCamera, cachedSignalFlags, cachedIsCameraSignal, cachedOrder) = cache[0];
        //                                cache.RemoveAt(0); // 移除最早的数据
        //                                //LogNet.Info(cache.Count.ToString());
        //                                //ToneProcesses[camera].RQO.TryDequeue(out int RqId1);
        //                                // 打开孔位信号
        //                                for (int i = 0; i < 8; i++)
        //                                {
        //                                    if (cachedSignalFlags[i])
        //                                    {
        //                                        WENYU_EIO32P.Program.WY_WriteOutPutBit(0, i+8,0);
        //                                        //Thread.Sleep(1);
        //                                        //WENYUPCIE.WenYu.WriteIO(0, i + 8, 0); // 打开孔位信号, 外排
        //                                    }
        //                                }

        //                                // 打开相机标志信号
        //                                if (cachedIsCameraSignal)
        //                                {
        //                                    WENYU_EIO32P.Program.WY_WriteOutPutBit(1, cachedCamera, 0);
        //                                    Thread.Sleep(2);
        //                                    WENYU_EIO32P.Program.WY_WriteOutPutBit(1, cachedCamera, 1);
        //                                    //WENYUPCIE.WenYu.WriteOutputIO(1, cachedCamera, 1); // 打开相机标志信号
        //                                    LogNet.Info($"相机 {cachedCamera} 信号: {string.Join(", ", cachedSignalFlags)} 缓存次数 {cachedOrder},刷新:{RqId},当前拍照次数:{ToneProcesses[camera].curOrder}");
        //                                }

        //                                // 清除信号标志
        //                                for (int i = 0; i < 8; i++)
        //                                {
        //                                    cachedSignalFlags[i] = false;
        //                                    WENYU_EIO32P.Program.WY_WriteOutPutBit(0, i + 8, 1);

        //                                    //WENYUPCIE.WenYu.WriteIO(0, i + 8, 1); // 关闭孔位信号, 内排
        //                                }

        //                            }
        //                            stepO = 0;
        //                        }
        //                        //stepO = 0;
        //                        break;

        //                }
        //            }

        //            Thread.Sleep(1); // 短暂休眠，避免过度占用CPU

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogNet.Error($"信号管理线程异常: {ex.ToString()}");
        //    }
        //}

        //剔除逻辑判断

        //内排数据处理，不带缓存版

        //private static async void SignalManagerO()
        //{
        //    try
        //    {
        //        int RqId = 0;
        //        int stepI = 0;
        //        while (true)
        //        {
        //            for (int camera = 3; camera < 6; camera++)
        //            {
        //                switch (stepI)
        //                {

        //                    case 0:
        //                        if (RQO.TryDequeue(out RqId))
        //                        {
        //                            stepI = 1;
        //                        }
        //                        break;
        //                    case 1:
        //                        if (计算完成[camera])
        //                        {
        //                            for (int i = 0; i < 8; i++)
        //                            {
        //                                if (IOSignalFlags[camera][i])
        //                                {
        //                                    WENYU_EIO32P.Program.WY_WriteOutPutBit(0, i + 8, 0);
                                     
        //                                }
        //                            }
        //                            if (CameraSignalFlags[camera])
        //                            {
        //                                WENYU_EIO32P.Program.WY_WriteOutPutBit(1, camera, 0);

        //                                //3.输出日志
        //                                LogNet.Info($"相机 {camera} 信号: {string.Join(", ", IOSignalFlags[camera])}次数{RqId}计算ID:{ToneProcesses[camera].curOrder}");

        //                                CameraSignalFlags[camera] = false; // 清除相机标志信号标志
        //                            }
        //                            //4.清除孔位信号标志
        //                            for (int i = 0; i < 8; i++)
        //                            {
        //                                IOSignalFlags[camera][i] = false; // 清除孔位信号标志
        //                                WENYU_EIO32P.Program.WY_WriteOutPutBit(1, camera, 0);
        //                                Thread.Sleep(2);
        //                                WENYU_EIO32P.Program.WY_WriteOutPutBit(1, camera, 1);
        //                            }
        //                            计算完成[camera] = false;
        //                            stepI = 0;

        //                        }
        //                        break;
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogNet.Error($"信号管理线程异常: {ex.ToString()}");
        //    }

        //    //如果不需要刷新信号，则短暂休眠以避免过度占用CPU


        //    Thread.Sleep(1); // 短暂休眠，避免过度占用CPU

        //}
        public static void DetermineRejection(Product p, int camera, int cardid, HTuple[] BatchResults)
        {
            try
            {
                bool Signal = false; // 相机标志位
                int batchIndex = p.Order % 4;

                // 如果结果为空，生成一个全零的HTuple
                if (p.Res == null)
                {
                    //HOperatorSet.TupleGenConst(32, 0, out p.Res);
                }

                // 将当前结果存储到对应批次
                BatchResults[batchIndex] = p.Res;

                // 如果拍照次数 >= 4，开始进行剔除判断
                if (p.Order >= 4)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        bool shouldReject = false; // 当前孔位的剔除标记
                        int row1 = 24 + i;
                        int row2 = 16 + i;
                        int row3 = 8 + i;

                        // 根据批次索引判断是否需要剔除
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

                        // 如果需要剔除，设置对应的IO信号标志
                        if (shouldReject)
                        {
                            IOSignalFlags[camera][i] = true;
                            Signal = true; // 设置相机标志信号
                        }
                    }

                    // 如果有剔除信号，设置相机标志信号
                    if (Signal)
                    {
                        CameraSignalFlags[camera] = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogNet.Error($"剔除判断异常: 相机编号={camera}, 批次={p.Order}, 异常信息={ex.ToString()}");
            }
        }

        public static Action<bool, int> InterruptHandler;

        private static WENYU_EIO32P.EventInterrupt eventInterrupt = new EventInterrupt(InterruptFun);

        public static bool WY_SetInterruptFun()
        {
            int Result = WENYU_EIO32P.Program.WY_SetInterruptFun(1, eventInterrupt);
            Result = WENYU_EIO32P.Program.WY_SetInterruptEnable(1, 1);
            //   if (Result != 0) MessageBox.Show("函数返回值错误!", "提示", MessageBoxButtons.OK);

            byte RiseEnable = 0xff;//初始化输入端口上升沿中断，方使后面操作使用
            byte FallEnable = 0xff;//初始化输入端口下降沿中断，方使后面操作使用
            Result = WENYU_EIO32P.Program.WY_SetInterruptPortEnable(1, RiseEnable, FallEnable);
            return Result == 0;
        }

        private static void InterruptFun()//此函数为中断事件函数
        {
            UInt16 RiseData = 0;
            UInt16 FallData = 0;
            long InPutData = 0;

            int Result = WENYU_EIO32P.Program.WY_GetInterruptData(1, ref RiseData, ref FallData);
            //   if (Result != 0) MessageBox.Show("函数返回值错误!", "提示", MessageBoxButtons.OK);
            Result = WENYU_EIO32P.Program.WY_GetInPutData(1, ref InPutData);
            //   if (Result != 0) MessageBox.Show("函数返回值错误!", "提示", MessageBoxButtons.OK);


            if (RiseData != 0xffff)
            {
                if ((RiseData & 0x0001) == 0x0000)
                {
                    if ((InPutData & 0x0001) == 0x0001)
                    {
                        //CameraI++;
                        //RQI.Enqueue(CameraI);
                        //LogNet.Info("内上升沿   中断");
                        //this.DisplayMessage.Text = "DI00口上升沿产生中断\r\n" + this.DisplayMessage.Text;
                    }
                    Result = WENYU_EIO32P.Program.WY_ClrInterruptRisePort0(1);
                    if (Result != 0) MessageBox.Show("函数返回值错误!", "提示", MessageBoxButtons.OK);
                }
                if ((RiseData & 0x0002) == 0x0000)
                {
                    if ((InPutData & 0x0002) == 0x0002)
                    {
                        //for (int camera = 3; camera < 6; camera++)
                        //{
                        //    //CameraO++;
                        //    //RQO.Enqueue(CameraO);
                        //    //LogNet.Info("外上升沿   中断");
                        //    //this.DisplayMessage.Text = "DI01口上升沿产生中断\r\n" + this.DisplayMessage.Text;
                        //}
                        var toneOp = new ToneOp();
                        toneOp.SetOutputAsync().Wait();
                        Result = WENYU_EIO32P.Program.WY_ClrInterruptRisePort1(1);
                        if (Result != 0) MessageBox.Show("函数返回值错误!", "提示", MessageBoxButtons.OK);
                    }
                }
            }
            WENYU_EIO32P.Program.WY_ResetInterrupt(1);
        }

        public async Task SetOutputAsync()
        {
            // 第一次写操作（后台线程执行）
            await Task.Run(() => WENYU_EIO32P.Program.WY_WriteOutPutBit(1, 4, 0));

            // 异步等待2ms（不阻塞线程）
            await Task.Delay(1);

            // 第二次写操作（后台线程执行）
            await Task.Run(() => WENYU_EIO32P.Program.WY_WriteOutPutBit(1, 4, 1));
        }
        //销毁对象
        public static void Destroy()
        {
            foreach (var p in ToneProcesses)
            {
                p.Destroy();
            }
            CloseTWO();
            ExtHandler.Destroy();
        }
    }
}