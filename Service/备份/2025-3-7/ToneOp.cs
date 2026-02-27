using HalconDotNet;
using LittleCommon.Domain;
using LittleCommon.Tool;
using RobotLocation.Model;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using VisionCore.BasePlugin.Else;
using VisionCore.Ext;
using VisionCore.Log;

namespace RobotLocation.Service
{
    internal class ToneOp
    {
        //相机
        public static List<ToneProcess> ToneProcesses = new List<ToneProcess>();
        public static int WaitTime1 = 100;//内排取结果时间，100ms
        public static int WaitTime2 = 88;//外排取结果时间，理论88ms
        //结果数组
        public static HTuple[] BatchResults1 { get; set; } = new HTuple[4];
        public static HTuple[] BatchResults2 { get; set; } = new HTuple[4];
        public static HTuple[] BatchResults3 { get; set; } = new HTuple[4];
        public static HTuple[] BatchResults4 { get; set; } = new HTuple[4];
        public static HTuple[] BatchResults5 { get; set; } = new HTuple[4];
        public static HTuple[] BatchResults6 { get; set; } = new HTuple[4];

        public static Thread RunTH;
        public static Thread RunTH1;
        public static Thread RunTH2;
        public static Thread RunTH3;
        public static Thread RunTH4;
        public static Thread RunTH5;
        public static Thread RunTH6;
        public static Result Init()
        {
            Result r = new Result();
            bool r1 = WENYUPCIE.WenYu.OpenTWO();
            r.msg = "";
            if (!r1)
            {
                r.msg += "IO卡打开失败,";
            }

            ToneProcesses.Add(new ToneProcess()
            {
                Name = "内1"

            });
            ToneProcesses.Add(new ToneProcess()
            {
                Name = "内2"
            });
            ToneProcesses.Add(new ToneProcess()
            {
                Name = "内3"
            });
            ToneProcesses.Add(new ToneProcess()
            {
                Name = "外1"
            });
            ToneProcesses.Add(new ToneProcess()
            {
                Name = "外2"
            });
            ToneProcesses.Add(new ToneProcess()
            {
                Name = "外3"
            });

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
            //双线程处理结果
            RunTH = new Thread(() =>
            {
                ExeI();
            });
            RunTH.IsBackground = true;
            //RunTH.Priority = ThreadPriority.Highest;
            RunTH.Start();

            RunTH1 = new Thread(() =>
            {
                ExeO();
            });
            RunTH1.IsBackground = true;
            //RunTH1.Priority = ThreadPriority.Highest;
            RunTH1.Start();
            //

            /*6线程处理结果
            RunTH1 = new Thread(() =>
            {
                Exe1();
            });
            RunTH1.IsBackground = true;
            //RunTH.Priority = ThreadPriority.Highest;
            RunTH1.Start();

            RunTH2 = new Thread(() =>
            {
                Exe2();
            });
           
            RunTH2.IsBackground = true;
            //RunTH.Priority = ThreadPriority.Highest;
            RunTH2.Start();

            RunTH3 = new Thread(() =>
            {
                Exe3();
            });

            RunTH3.IsBackground = true;
            //RunTH.Priority = ThreadPriority.Highest;
            RunTH3.Start();

            RunTH4 = new Thread(() =>
            {
                Exe4();
            });

            RunTH4.IsBackground = true;
            //RunTH.Priority = ThreadPriority.Highest;
            RunTH4.Start();

            RunTH5 = new Thread(() =>
            {
                Exe5();
            });

            RunTH5.IsBackground = true;
            //RunTH.Priority = ThreadPriority.Highest;
            RunTH5.Start();

            RunTH6 = new Thread(() =>
            {
                Exe6();
            });

            RunTH6.IsBackground = true;
            //RunTH.Priority = ThreadPriority.Highest;
            RunTH6.Start();
            */

            return r;
        }
        /*
        //6线程处理结果，带1次超时处理超时周期内刷2次IO口，丢弃已过去的8个结果
        //内1
        public static void Exe1()
        {
            int Step1 = 0;
            int ccd1CurOrder = 1;
            double ccdResTime1 = 0;
            double ccdResTime1_1 = 0;
            while (true)
            {
                Thread.Sleep(1);
                //内1
                switch (Step1)
                {
                    case 0:
                        {
                            if (ToneProcesses[0].ResQueuqe.TryDequeue(out ccd1CurOrder))
                            {
                                ccdResTime1 = HighTime.GetMSec() + WaitTime1;
                                ccdResTime1_1 = HighTime.GetMSec() + 2 * WaitTime1;
                                Step1 = 1;
                            }
                        }
                        break;
                    case 1:
                        {
                            double cur = HighTime.GetMSec();
                            if (cur < ccdResTime1)
                            {
                                //尝试取结果标记
                                ToneProcesses[0].ProductMap.TryGetValue(ccd1CurOrder, out var pp);
                                //有结果处理数据
                                if (pp != null && pp.Complete == true)
                                {
                                    //销毁结果，取得数据
                                    if (ToneProcesses[0].ProductMap.TryRemove(ccd1CurOrder, out var p))
                                    {
                                        //汇总剔除
                                        DetermineRejection(p, 0, 0, BatchResults1);
                                        Step1 = 0;
                                    }
                                }
                            }
                            else if (cur >= ccdResTime1 && cur < ccdResTime1_1)
                            {
                                //尝试取结果
                                ToneProcesses[0].ProductMap.TryGetValue(ccd1CurOrder, out var pp);
                                //有结果处理
                                if (pp != null && pp.Complete == true)
                                {
                                    //取回结果并处理                               
                                    if (ToneProcesses[0].ProductMap.TryRemove(ccd1CurOrder, out var p))
                                    {
                                        //移动拍照次数
                                        p.Order = p.Order + 1;
                                        //处理数据，丢弃前8个结果
                                        // 创建一个新的HTuple
                                        HTuple result = new HTuple();
                                        //LogNet.Info("内1处理前:" + p.Res.ToString() + "p.order:" + p.Order + "相机次数:" + ccd1CurOrder);
                                        // 将原始HTuple的内容前移
                                        int rt1 = 0;
                                        for (int i = 0; i < 24; i++)
                                        {
                                            result[i] = p.Res[i + 8];
                                            if (result[i]==1)
                                            {
                                                rt1=rt1 + 1;
                                            }
                                        }

                                        // 在后面补零
                                        for (int i = 0; i < 8; i++)
                                        {
                                            result[24 + i] = 0;
                                        }
                                        p.Res = result;
                                        //判断拍照次数
                                        DetermineRejection(p, 0, 0, BatchResults1);
                                        //LogNet.Info("内1处理后:" + p.Res.ToString() + ccd1CurOrder);
                                        LogNet.Info("内1超时追加数据:" + rt1 + "个");
                                        Step1 = 0;
                                    }
                                }
                            }
                            else
                            {
                                //销毁结果，取得数据
                                if (ToneProcesses[0].ProductMap.TryRemove(ccd1CurOrder, out var p))
                                {
                                    HTuple hv_Newtuple;
                                    HOperatorSet.TupleGenConst(32, 0, out hv_Newtuple);
                                    p.Res = hv_Newtuple;
                                    //汇总剔除
                                    DetermineRejection(p, 0, 0, BatchResults1);
                                }
                                LogNet.Warn(" 内1算法超时，丢失1/4次检测数据" + ccd1CurOrder);
                                Step1 = 0;
                            }
                        }
                        break;
                }
            }
         }
        public static void Exe2()
        {
            int Step2 = 0;
            int ccd2CurOrder = 1;
            double ccdResTime2 = 0;
            double ccdResTime2_1 = 0;
            while (true)
            {
                Thread.Sleep(1);
                //内2
                switch (Step2)
                {
                    case 0:
                        {
                            if (ToneProcesses[1].ResQueuqe.TryDequeue(out ccd2CurOrder))
                            {

                                ccdResTime2 = HighTime.GetMSec() + WaitTime1;
                                ccdResTime2_1 = HighTime.GetMSec() + 2 * WaitTime1;
                                Step2 = 1;
                            }
                        }
                        break;
                    case 1:
                        {
                            double cur = HighTime.GetMSec();
                            if (cur < ccdResTime2)
                            {
                                ToneProcesses[1].ProductMap.TryGetValue(ccd2CurOrder, out var pp);
                                if (pp != null && pp.Complete == true)
                                {
                                    //判断是否超时
                                    if (ToneProcesses[1].ProductMap.TryRemove(ccd2CurOrder, out var p))
                                    {
                                        //判断拍照次数
                                        DetermineRejection(p, 1, 0, BatchResults2);
                                        Step2 = 0;
                                    }
                                }
                            }
                            else if (cur >= ccdResTime2 && cur < ccdResTime2_1)
                            {
                                //尝试取结果
                                ToneProcesses[1].ProductMap.TryGetValue(ccd2CurOrder, out var pp);
                                //有结果处理
                                if (pp != null && pp.Complete == true)
                                {
                                    //取回结果并处理                               
                                    if (ToneProcesses[1].ProductMap.TryRemove(ccd2CurOrder, out var p))
                                    {
                                        //移动拍照次数
                                        p.Order = p.Order + 1;
                                        //处理数据，丢弃前8个结果
                                        // 创建一个新的HTuple
                                        HTuple result = new HTuple();
                                        //LogNet.Info("内2处理前:" + p.Res.ToString() + "p.order:" + p.Order + "相机次数:" + ccd2CurOrder);
                                        int rt2 = 0;
                                        // 将原始HTuple的内容前移
                                        for (int i = 0; i < 24; i++)
                                        {
                                            result[i] = p.Res[i + 8];
                                            if (result[i] == 1)
                                            {
                                                rt2 = rt2 + 1;
                                            }
                                        }

                                        // 在后面补零
                                        for (int i = 0; i < 8; i++)
                                        {
                                            result[24 + i] = 0;
                                        }
                                        p.Res = result;
                                        //判断拍照次数
                                        DetermineRejection(p, 1, 0, BatchResults2);
                                        //LogNet.Info("内2处理后:" + p.Res.ToString() + ccd2CurOrder);
                                        LogNet.Info("内2超时追加数据:" + rt2+ "个");
                                        Step2 = 0;
                                    }
                                }
                            }
                            else
                            {
                                //销毁结果，取得数据
                                if (ToneProcesses[1].ProductMap.TryRemove(ccd2CurOrder, out var p))
                                {
                                    HTuple hv_Newtuple;
                                    HOperatorSet.TupleGenConst(32, 0, out hv_Newtuple);
                                    p.Res = hv_Newtuple;
                                    //汇总剔除
                                    DetermineRejection(p, 1, 0, BatchResults2);
                                }
                                LogNet.Warn(" 内2算法超时，丢失1、4次检测数据" + ccd2CurOrder);
                                Step2 = 0;
                            }
                        }
                        break;
                }
            }

         }
        public static void Exe3()
        {
            int Step3 = 0;
            int ccd3CurOrder = 1;
            double ccdResTime3 = 0;
            double ccdResTime3_1 = 0; //超时1周期           
            while (true)
            {
                Thread.Sleep(1);
                //内3
                switch (Step3)
                {
                    case 0:
                        {
                            if (ToneProcesses[2].ResQueuqe.TryDequeue(out ccd3CurOrder))
                            {

                                ccdResTime3 = HighTime.GetMSec() + WaitTime1;
                                ccdResTime3_1 = HighTime.GetMSec() + 2* WaitTime1;
                                Step3 = 1;
                            }
                        }
                        break;
                    case 1:
                        {
                            double cur = HighTime.GetMSec();
                            if (cur < ccdResTime3)
                            {
                                //尝试取结果
                                ToneProcesses[2].ProductMap.TryGetValue(ccd3CurOrder, out var pp);
                                //有结果处理
                                if (pp != null && pp.Complete == true)
                                {
                                    //判断是否超时                               
                                    if (ToneProcesses[2].ProductMap.TryRemove(ccd3CurOrder, out var p))
                                    {
                                        //判断拍照次数
                                        DetermineRejection(p, 2, 0, BatchResults3);
                                        Step3 = 0;
                                    }
                                }
                            }
                            //超时1次
                            else if(cur >= ccdResTime3 && cur < ccdResTime3_1)
                            {
                                //尝试取结果
                                ToneProcesses[2].ProductMap.TryGetValue(ccd3CurOrder, out var pp);
                                //有结果处理
                                if (pp != null && pp.Complete == true)
                                {
                                    //取回结果并处理                               
                                    if (ToneProcesses[2].ProductMap.TryRemove(ccd3CurOrder, out var p))
                                    {
                                        //移动拍照次数
                                        p.Order = p.Order +1;
                                        //处理数据，丢弃前8个结果
                                        // 创建一个新的HTuple
                                        HTuple result = new HTuple();
                                        //LogNet.Info("内3处理前:" + p.Res.ToString() +"p.order:" + p.Order+"相机次数:" +ccd3CurOrder);
                                        int rt3 = 0;
                                        // 将原始HTuple的内容前移
                                        for (int i = 0; i < 24; i++)
                                        {
                                            result[i ] = p.Res[i+8];
                                            if (result[i] == 1)
                                            {
                                                rt3 = rt3 + 1;
                                            }
                                        }

                                        // 在后面补零
                                        for (int i = 0; i < 8; i++)
                                        {
                                            result[24+ i] = 0;
                                        }
                                        p.Res = result;
                                        //判断拍照次数
                                        DetermineRejection(p, 2, 0, BatchResults3);
                                        //LogNet.Info("内3处理后:" +p.Res.ToString()+ ccd3CurOrder);
                                        LogNet.Info("内3超时追加数据:" + rt3 + "个");
                                        Step3 = 0;
                                    }
                                }
                            }
                            else
                            {
                                //销毁结果，取得数据
                                if (ToneProcesses[2].ProductMap.TryRemove(ccd3CurOrder, out var p))
                                {
                                    HTuple hv_Newtuple;
                                    HOperatorSet.TupleGenConst(32, 0, out hv_Newtuple);
                                    p.Res = hv_Newtuple;
                                    //汇总剔除
                                    DetermineRejection(p, 2, 0, BatchResults3);
                                }
                                LogNet.Warn(" 内3算法超时，丢失1/4次检测数据" + ccd3CurOrder);
                                Step3 = 0;
                            }
                        }
                        break;
                }
            }
         }
        public static void Exe4()
        {
            int Step4 = 0;
            int ccd4CurOrder = 1;
            double ccdResTime4 = 0;
            double ccdResTime4_1 = 0;
            while (true)
            {
                Thread.Sleep(1);
                //外1
                switch (Step4)
                {
                    case 0:
                        {
                            if (ToneProcesses[3].ResQueuqe.TryDequeue(out ccd4CurOrder))
                            {

                                ccdResTime4 = HighTime.GetMSec() + WaitTime2;
                                ccdResTime4_1 = HighTime.GetMSec() + 2 * WaitTime2;
                                Step4 = 1;
                            }
                        }
                        break;
                    case 1:
                        {
                            double cur = HighTime.GetMSec();
                            if (cur < ccdResTime4)
                            {
                                //尝试取对象
                                ToneProcesses[3].ProductMap.TryGetValue(ccd4CurOrder, out var pp);
                                //判断对象
                                if (pp != null && pp.Complete == true)
                                {
                                    if (ToneProcesses[3].ProductMap.TryRemove(ccd4CurOrder, out var p))
                                    {
                                        //判断拍照次数
                                        DetermineRejection(p, 3, 1, BatchResults4);
                                        Step4 = 0;
                                    }
                                }
                            }
                            //超时1次
                            else if (cur >= ccdResTime4 && cur < ccdResTime4_1)
                            {
                                //尝试取结果
                                ToneProcesses[3].ProductMap.TryGetValue(ccd4CurOrder, out var pp);
                                //有结果处理
                                if (pp != null && pp.Complete == true)
                                {
                                    //取回结果并处理                               
                                    if (ToneProcesses[3].ProductMap.TryRemove(ccd4CurOrder, out var p))
                                    {
                                        //移动拍照次数
                                        p.Order = p.Order + 1;
                                        //处理数据，丢弃前8个结果
                                        // 创建一个新的HTuple
                                        HTuple result = new HTuple();
                                        //LogNet.Info("外1处理前:" + p.Res.ToString() + "p.order:" + p.Order + "相机次数:" + ccd4CurOrder);
                                        int rt4 = 0;
                                        // 将原始HTuple的内容前移
                                        for (int i = 0; i < 24; i++)
                                        {
                                            result[i] = p.Res[i + 8];
                                            if (result[i] == 1)
                                            {
                                                rt4 = rt4 + 1;
                                            }
                                        }

                                        // 在后面补零
                                        for (int i = 0; i < 8; i++)
                                        {
                                            result[24 + i] = 0;
                                        }
                                        p.Res = result;
                                        //判断拍照次数
                                        DetermineRejection(p, 3, 1, BatchResults4);
                                        //LogNet.Info("外1处理后:" + p.Res.ToString() + ccd4CurOrder);
                                        LogNet.Info("外1超时追加数据:" +rt4 + "个");
                                        Step4 = 0;
                                    }
                                }
                            }
                            else
                            {
                                //销毁结果，取得数据
                                if (ToneProcesses[3].ProductMap.TryRemove(ccd4CurOrder, out var p))
                                {
                                    HTuple hv_Newtuple;
                                    HOperatorSet.TupleGenConst(32, 0, out hv_Newtuple);
                                    p.Res = hv_Newtuple;
                                    //汇总剔除
                                    DetermineRejection(p, 3, 1, BatchResults4);
                                }
                                LogNet.Warn(" 外1算法超时，丢失1/4 次检测数据" + ccd4CurOrder);
                                Step4 = 0;
                            }
                        }
                        break;
                }
            }
        }
        public static void Exe5()
        {
            
            int Step5 = 0;
            int ccd5CurOrder = 1;
            double ccdResTime5 = 0;
            double ccdResTime5_1 = 0;
            while (true)
            {
                Thread.Sleep(1);
                //外2
                switch (Step5)
                {
                    case 0:
                        {
                            if (ToneProcesses[4].ResQueuqe.TryDequeue(out ccd5CurOrder))
                            {

                                ccdResTime5 = HighTime.GetMSec() + WaitTime2;
                                ccdResTime5_1 = HighTime.GetMSec() + 2 * WaitTime2;
                                Step5 = 1;
                            }
                        }
                        break;
                    case 1:
                        {
                            double cur = HighTime.GetMSec();
                            if (cur < ccdResTime5)
                            {
                                //判断是否有结果
                                ToneProcesses[4].ProductMap.TryGetValue(ccd5CurOrder, out var pp);
                                if (pp != null && pp.Complete == true)
                                {
                                    if (ToneProcesses[4].ProductMap.TryRemove(ccd5CurOrder, out var p))
                                    {
                                        //判断拍照次数
                                        DetermineRejection(p, 4, 1, BatchResults5);
                                        Step5 = 0;
                                    }
                                }
                            }
                            //超时1次
                            else if (cur >= ccdResTime5 && cur < ccdResTime5_1)
                            {
                                //尝试取结果
                                ToneProcesses[4].ProductMap.TryGetValue(ccd5CurOrder, out var pp);
                                //有结果处理
                                if (pp != null && pp.Complete == true)
                                {
                                    //取回结果并处理                               
                                    if (ToneProcesses[4].ProductMap.TryRemove(ccd5CurOrder, out var p))
                                    {
                                        //移动拍照次数
                                        p.Order = p.Order + 1;
                                        //处理数据，丢弃前8个结果
                                        // 创建一个新的HTuple
                                        HTuple result = new HTuple();
                                        //LogNet.Info("外2处理前:" + p.Res.ToString() + "p.order:" + p.Order + "相机次数:" + ccd5CurOrder);
                                        int rt5 = 0;
                                        // 将原始HTuple的内容前移
                                        for (int i = 0; i < 24; i++)
                                        {
                                            result[i] = p.Res[i + 8];
                                            if (result[i] == 1)
                                            {
                                                rt5 = rt5 + 1;
                                            }
                                        }

                                        // 在后面补零
                                        for (int i = 0; i < 8; i++)
                                        {
                                            result[24 + i] = 0;
                                        }
                                        p.Res = result;
                                        //判断拍照次数
                                        DetermineRejection(p, 4, 1, BatchResults5);
                                        //LogNet.Info("外2处理后:" + p.Res.ToString() + ccd5CurOrder);
                                        LogNet.Info("外2超时追加数据:" + rt5 + "个");
                                        Step5 = 0;
                                    }
                                }
                            }
                            else
                            {
                                //销毁结果，取得数据
                                if (ToneProcesses[4].ProductMap.TryRemove(ccd5CurOrder, out var p))
                                {
                                    HTuple hv_Newtuple;
                                    HOperatorSet.TupleGenConst(32, 0, out hv_Newtuple);
                                    p.Res = hv_Newtuple;
                                    //汇总剔除
                                    DetermineRejection(p, 4, 1, BatchResults5);
                                }
                                LogNet.Warn(" 外2算法超时，丢失1/4 次检测数据" + ccd5CurOrder);
                                Step5 = 0;
                            }
                        }
                        break;
                }
            }
        }
        public static void Exe6()
        {   
            int Step6 = 0;
            int ccd6CurOrder = 1;
            double ccdResTime6 = 0;
            double ccdResTime6_1 = 0;
            while (true)
            {
                Thread.Sleep(1);
                //相机6
                switch (Step6)
                {
                    case 0:
                        {
                            if (ToneProcesses[5].ResQueuqe.TryDequeue(out ccd6CurOrder))
                            {

                                ccdResTime6 = HighTime.GetMSec() + WaitTime2;
                                ccdResTime6_1 = HighTime.GetMSec() + 2 * WaitTime2;
                                Step6 = 1;
                            }
                        }
                        break;
                    case 1:
                        {
                            double cur = HighTime.GetMSec();
                            if (cur < ccdResTime6)
                            {
                                //判断是否超时
                                ToneProcesses[5].ProductMap.TryGetValue(ccd6CurOrder, out var pp);
                                if (pp != null && pp.Complete == true)
                                {
                                    if (ToneProcesses[5].ProductMap.TryRemove(ccd6CurOrder, out var p))
                                    {
                                        //判断拍照次数
                                        DetermineRejection(p, 5, 1, BatchResults6);
                                        Step6 = 0;
                                    }
                                }
                            }
                            //超时1次
                            else if (cur >= ccdResTime6 && cur < ccdResTime6_1)
                            {
                                //尝试取结果
                                ToneProcesses[5].ProductMap.TryGetValue(ccd6CurOrder, out var pp);
                                //有结果处理
                                if (pp != null && pp.Complete == true)
                                {
                                    //取回结果并处理                               
                                    if (ToneProcesses[5].ProductMap.TryRemove(ccd6CurOrder, out var p))
                                    {
                                        //移动拍照次数
                                        p.Order = p.Order + 1;
                                        //处理数据，丢弃前8个结果
                                        // 创建一个新的HTuple
                                        HTuple result = new HTuple();
                                        //LogNet.Info("外3处理前:" + p.Res.ToString() + "p.order:" + p.Order + "相机次数:" + ccd6CurOrder);
                                        int rt6 = 0;
                                        // 将原始HTuple的内容前移
                                        for (int i = 0; i < 24; i++)
                                        {
                                            result[i] = p.Res[i + 8];
                                            if (result[i] == 1)
                                            {
                                                rt6 = rt6 + 1;
                                            }
                                        }

                                        // 在后面补零
                                        for (int i = 0; i < 8; i++)
                                        {
                                            result[24 + i] = 0;
                                        }
                                        p.Res = result;
                                        //判断拍照次数
                                        DetermineRejection(p, 5, 1, BatchResults6);
                                        //LogNet.Info("外3处理后:" + p.Res.ToString() + ccd6CurOrder);
                                        LogNet.Info("外3超时追加数据:" + rt6 + "个");
                                        Step6 = 0;
                                    }
                                }
                            }
                            else
                            {
                                //销毁结果，取得数据
                                if (ToneProcesses[5].ProductMap.TryRemove(ccd6CurOrder, out var p))
                                {
                                    HTuple hv_Newtuple;
                                    HOperatorSet.TupleGenConst(32, 0, out hv_Newtuple);
                                    p.Res = hv_Newtuple;
                                    //汇总剔除
                                    DetermineRejection(p, 5, 1, BatchResults6);
                                }
                                LogNet.Warn(" 外3算法超时，丢失1/4 次检测数据" + ccd6CurOrder);
                                Step6 = 0;
                            }
                        }
                        break;
                }
            }
        }
        */

        //双线程处理结果
        public static void ExeI()
        {
            int Step1 = 0;
            int ccd1CurOrder = 1;
            double ccdResTime1 = 0;

            int Step2 = 0;
            int ccd2CurOrder = 1;
            double ccdResTime2 = 0;

            int Step3 = 0;
            int ccd3CurOrder = 1;
            double ccdResTime3 = 0;
            while (true)
            {
                Thread.Sleep(1);
                //内1
                switch (Step1)
                {
                    case 0:
                        {
                            if (ToneProcesses[0].ResQueuqe.TryDequeue(out ccd1CurOrder))
                            {
                                ccdResTime1 = HighTime.GetMSec() + WaitTime1;
                                Step1 = 1;
                            }
                        }
                        break;
                    case 1:
                        {
                            double cur = HighTime.GetMSec();
                            if (cur < ccdResTime1)
                            {
                                //尝试取结果标记
                                ToneProcesses[0].ProductMap.TryGetValue(ccd1CurOrder, out var pp);
                                //有结果处理数据
                                if (pp != null && pp.Complete == true)
                                {
                                    //销毁结果，取得数据
                                    if (ToneProcesses[0].ProductMap.TryRemove(ccd1CurOrder, out var p))
                                    {
                                        //汇总剔除
                                        DetermineRejection(p, 0, 0, BatchResults1);
                                        Step1 = 0;
                                    }
                                }
                            }
                            else
                            {
                                //销毁结果，取得数据
                                if (ToneProcesses[0].ProductMap.TryRemove(ccd1CurOrder, out var p))
                                {
                                    HTuple hv_Newtuple;
                                    HOperatorSet.TupleGenConst(32, 0, out hv_Newtuple);
                                    p.Res = hv_Newtuple;
                                    //汇总剔除
                                    DetermineRejection(p, 0, 0, BatchResults1);
                                }
                                LogNet.Warn(" 内1算法超时" + ccd1CurOrder);
                                Step1 = 0;
                            }
                        }
                        break;
                }
                //内2
                switch (Step2)
                {
                    case 0:
                        {
                            if (ToneProcesses[1].ResQueuqe.TryDequeue(out ccd2CurOrder))
                            {

                                ccdResTime2 = HighTime.GetMSec() + WaitTime1;
                                Step2 = 1;
                            }
                        }
                        break;
                    case 1:
                        {
                            double cur = HighTime.GetMSec();
                            if (cur < ccdResTime2)
                            {
                                ToneProcesses[1].ProductMap.TryGetValue(ccd2CurOrder, out var pp);
                                if (pp != null && pp.Complete == true)
                                {
                                    //判断是否超时
                                    if (ToneProcesses[1].ProductMap.TryRemove(ccd2CurOrder, out var p))
                                    {
                                        //判断拍照次数
                                        DetermineRejection(p, 1, 0, BatchResults2);
                                        Step2 = 0;
                                    }
                                }
                            }
                            else
                            {
                                //销毁结果，取得数据
                                if (ToneProcesses[1].ProductMap.TryRemove(ccd2CurOrder, out var p))
                                {
                                    HTuple hv_Newtuple;
                                    HOperatorSet.TupleGenConst(32, 0, out hv_Newtuple);
                                    p.Res = hv_Newtuple;
                                    //汇总剔除
                                    DetermineRejection(p, 1, 0, BatchResults2);
                                }
                                LogNet.Warn(" 内2算法超时" + ccd2CurOrder);
                                Step2 = 0;
                            }
                        }
                        break;
                }
                //内3
                switch (Step3)
                {
                    case 0:
                        {
                            if (ToneProcesses[2].ResQueuqe.TryDequeue(out ccd3CurOrder))
                            {

                                ccdResTime3 = HighTime.GetMSec() + WaitTime1;
                                Step3 = 1;
                            }
                        }
                        break;
                    case 1:
                        {
                            double cur = HighTime.GetMSec();
                            if (cur < ccdResTime3)
                            {
                                //尝试取结果
                                ToneProcesses[2].ProductMap.TryGetValue(ccd3CurOrder, out var pp);
                                //有结果处理
                                if (pp != null && pp.Complete == true)
                                {
                                    //判断是否超时                               
                                    if (ToneProcesses[2].ProductMap.TryRemove(ccd3CurOrder, out var p))
                                    {
                                        //判断拍照次数
                                        DetermineRejection(p, 2, 0, BatchResults3);
                                        Step3 = 0;
                                    }
                                }
                            }
                            else
                            {
                                //销毁结果，取得数据
                                if (ToneProcesses[2].ProductMap.TryRemove(ccd3CurOrder, out var p))
                                {
                                    HTuple hv_Newtuple;
                                    HOperatorSet.TupleGenConst(32, 0, out hv_Newtuple);
                                    p.Res = hv_Newtuple;
                                    //汇总剔除
                                    DetermineRejection(p, 2, 0, BatchResults3);
                                }
                                LogNet.Warn(" 内3算法超时" + ccd3CurOrder);
                                Step3 = 0;
                            }
                        }
                    break;
                }
            }

           
        }
        public static void ExeO()
        {
            int Step4 = 0;
            int ccd4CurOrder = 1;
            double ccdResTime4 = 0;

            int Step5 = 0;
            int ccd5CurOrder = 1;
            double ccdResTime5 = 0;

            int Step6 = 0;
            int ccd6CurOrder = 1;
            double ccdResTime6 = 0;

            while (true)
            {
                Thread.Sleep(1);
                //外1
                switch (Step4)
                {
                    case 0:
                        {
                            if (ToneProcesses[3].ResQueuqe.TryDequeue(out ccd4CurOrder) )
                            {

                                ccdResTime4 = HighTime.GetMSec() + WaitTime2;
                                Step4 = 1;
                            }
                        }
                        break;
                    case 1:
                        {
                            double cur = HighTime.GetMSec();
                            if (cur < ccdResTime4)
                            {
                                //尝试取对象
                                ToneProcesses[3].ProductMap.TryGetValue(ccd4CurOrder, out var pp);
                                //判断对象
                                if (pp != null && pp.Complete == true)
                                {
                                    if (ToneProcesses[3].ProductMap.TryRemove(ccd4CurOrder, out var p))
                                    {
                                        //判断拍照次数
                                        DetermineRejection(p, 3, 1, BatchResults4);
                                        Step4 = 0;
                                    }
                                }
                            }
                            else
                            {
                                //销毁结果，取得数据
                                if (ToneProcesses[3].ProductMap.TryRemove(ccd4CurOrder, out var p))
                                {
                                    HTuple hv_Newtuple;
                                    HOperatorSet.TupleGenConst(32, 0, out hv_Newtuple);
                                    p.Res = hv_Newtuple;
                                    //汇总剔除
                                    DetermineRejection(p, 3, 1, BatchResults4);
                                }
                                LogNet.Warn(" 外1算法超时" + ccd4CurOrder);
                                Step4 = 0;
                            }
                        }
                        break;
                }
                //外2
                switch (Step5)
                {
                    case 0:
                        {
                            if (ToneProcesses[4].ResQueuqe.TryDequeue(out ccd5CurOrder))
                            {

                                ccdResTime5 = HighTime.GetMSec() + WaitTime2;
                                Step5 = 1;
                            }
                        }
                        break;
                    case 1:
                        {
                            double cur = HighTime.GetMSec();
                            if (cur < ccdResTime5)
                            {
                                //判断是否有结果
                                ToneProcesses[4].ProductMap.TryGetValue(ccd5CurOrder, out var pp);
                                if (pp != null && pp.Complete == true)
                                {
                                    if (ToneProcesses[4].ProductMap.TryRemove(ccd5CurOrder, out var p))
                                    {
                                        //判断拍照次数
                                        DetermineRejection(p, 4, 1, BatchResults5);
                                        Step5 = 0;
                                    }
                                }
                            }
                            else
                            {
                                //销毁结果，取得数据
                                if (ToneProcesses[4].ProductMap.TryRemove(ccd5CurOrder, out var p))
                                {
                                    HTuple hv_Newtuple;
                                    HOperatorSet.TupleGenConst(32, 0, out hv_Newtuple);
                                    p.Res = hv_Newtuple;
                                    //汇总剔除
                                    DetermineRejection(p, 4, 1, BatchResults5);
                                }
                                LogNet.Warn(" 外2算法超时" + ccd5CurOrder);
                                Step5 = 0;
                            }
                        }
                        break;
                }
                //相机6
                switch (Step6)
                {
                    case 0:
                        {
                            if (ToneProcesses[5].ResQueuqe.TryDequeue(out ccd6CurOrder))
                            {

                                ccdResTime6 = HighTime.GetMSec() + WaitTime2;
                                Step6 = 1;
                            }
                        }
                        break;
                    case 1:
                        {
                            double cur = HighTime.GetMSec();
                            if (cur < ccdResTime6)
                            {
                                //判断是否超时
                                ToneProcesses[5].ProductMap.TryGetValue(ccd6CurOrder, out var pp);
                                if (pp != null && pp.Complete == true)
                                {
                                    if (ToneProcesses[5].ProductMap.TryRemove(ccd6CurOrder, out var p))
                                    {
                                        //判断拍照次数
                                        DetermineRejection(p, 5, 1, BatchResults6);
                                        Step6 = 0;
                                    }
                                }
                            }
                            else
                            {
                                //销毁结果，取得数据
                                if (ToneProcesses[5].ProductMap.TryRemove(ccd6CurOrder, out var p))
                                {
                                    HTuple hv_Newtuple;
                                    HOperatorSet.TupleGenConst(32, 0, out hv_Newtuple);
                                    p.Res = hv_Newtuple;
                                    //汇总剔除
                                    DetermineRejection(p, 5, 1, BatchResults6);
                                }
                                LogNet.Warn(" 外3算法超时" + ccd6CurOrder);
                                Step6 = 0;
                            }
                        }
                        break;
                }
            }
        }//

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p：结果字典"></param>
        /// <param name="camera：相机（0-5）"></param>
        /// <param name="cardid：IO卡 （内 = 0  ，外 = 1）"></param>
        public static void DetermineRejection(Product p, int camera, int cardid, HTuple[] BatchResults)
        {
            try
            {
                bool Signal = false; // 相机标志位
                int batchIndex = p.Order % 4;
                //LogNet.Info("相机 ：" + camera + "-次数：" + p.Order.ToString()+"结果 ：");
                if (p.Res != null)
                {
                    BatchResults[batchIndex] = p.Res;
                    //LogNet.Info(camera+":"+p.Res.ToString());
                }
                else
                {
                    HTuple hv_Newtuple;
                    HOperatorSet.TupleGenConst(32, 0, out hv_Newtuple);
                    p.Res = hv_Newtuple;
                }
                if (p.Order >= 4)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        WENYUPCIE.WenYu.WriteIO(0, 8 * cardid + i, 1);
                        bool shouldReject = false; // 当前孔位的剔除标记

                        int row1 = 24 + i;
                        int row2 = 16 + i;
                        int row3 = 8 + i;

                        //switch (batchIndex)
                        //{
                        //    case 0:
                        //        if (BatchResults[1][row1] == 1 || BatchResults[2][row2] == 1 || BatchResults[3][row3] == 1 || BatchResults[0][i] == 1)
                        //        {
                        //            shouldReject = true;  // 孔位剔除信号
                        //            Signal = true; //相机标志信号       
                        //        }
                        //        break;
                        //    case 1:
                        //        if (BatchResults[2][row1] == 1 || BatchResults[3][row2] == 1 || BatchResults[0][row3] == 1 || BatchResults[1][i] == 1)
                        //        {
                        //            shouldReject = true;  // 孔位剔除信号
                        //            Signal = true; //相机标志信号       
                        //        }
                        //        break;
                        //    case 2:
                        //        if (BatchResults[3][row1] == 1 || BatchResults[0][row2] == 1 || BatchResults[1][row3] == 1 || BatchResults[2][i] == 1)
                        //        {
                        //            shouldReject = true;  // 孔位剔除信号
                        //            Signal = true; //相机标志信号       
                        //        }
                        //        break;
                        //    case 3:
                        //        if (BatchResults[0][row1] == 1 || BatchResults[1][row2] == 1 || BatchResults[2][row3] == 1 || BatchResults[3][i] == 1)
                        //        {
                        //            shouldReject = true;  // 孔位剔除信号
                        //            Signal = true; //相机标志信号       
                        //        }
                        //        break;
                        //}
                        switch (batchIndex)
                        {
                            case 0:
                                if (BatchResults[1][row1] == 1 || BatchResults[2][row2] == 1 || BatchResults[3][row3] == 1 || BatchResults[0][i] == 1)
                                {
                                    shouldReject = true;  // 孔位剔除信号
                                    Signal = true; //相机标志信号       
                                }
                                break;
                            case 1:
                                if (BatchResults[2][row1] == 1 || BatchResults[3][row2] == 1 || BatchResults[0][row3] == 1 || BatchResults[1][i] == 1)
                                {
                                    shouldReject = true;  // 孔位剔除信号
                                    Signal = true; //相机标志信号       
                                }
                                break;
                            case 2:
                                if (BatchResults[3][row1] == 1 || BatchResults[0][row2] == 1 || BatchResults[1][row3] == 1 || BatchResults[2][i] == 1)
                                {
                                    shouldReject = true;  // 孔位剔除信号
                                    Signal = true; //相机标志信号       
                                }
                                break;
                            case 3:
                                if (BatchResults[0][row1] == 1 || BatchResults[1][row2] == 1 || BatchResults[2][row3] == 1 || BatchResults[3][i] == 1)
                                {
                                    shouldReject = true;  // 孔位剔除信号
                                    Signal = true; //相机标志信号       
                                }
                                break;
                        }
                        // 硬件控制触发
                        if (shouldReject)
                        {
                            // 打开孔位IO                    
                            WENYUPCIE.WenYu.WriteIO(0, 8 * cardid + i, 0);
                        }
                    }
                    if (Signal)
                    {
                        //打开相机标志信号
                        WENYUPCIE.WenYu.WriteOutputIO(1, camera, 1);
                    }
                }
            }
            catch (Exception ex)
            {
                LogNet.Error($"剔除判断异常:   " + ex.ToString());
            }
        }
        public static void Destroy()
        {
            foreach (var p in ToneProcesses)
            {
                p.Destroy();
            }
            //WENYUPCIE.WenYu.CloseTWO();
            ExtHandler.Destroy();
        }
    }
}
