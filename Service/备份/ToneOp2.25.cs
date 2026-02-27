using HalconDotNet;
using LittleCommon.Domain;
using LittleCommon.Tool;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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
        public static Thread RunTH;
        public static Thread RunTH1;

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
            RunTH = new Thread(() =>
            {

                ExeI();

            });
            RunTH.IsBackground = true;
            RunTH.Priority = ThreadPriority.Highest;
            RunTH.Start();

            RunTH1 = new Thread(() =>
            {

                ExeO();

            });
            RunTH1.IsBackground = true;
            RunTH1.Priority = ThreadPriority.Highest;
            RunTH1.Start();
            return r;
        }
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


            HTuple I1temp1 = new HTuple();
            HOperatorSet.TupleGenConst(32, 0, out I1temp1);
            HTuple I1temp2 = new HTuple();
            HOperatorSet.TupleGenConst(32, 0, out I1temp2);
            HTuple I1temp3 = new HTuple();
            HOperatorSet.TupleGenConst(32, 0, out I1temp3);
            HTuple I1temp4 = new HTuple();
            HOperatorSet.TupleGenConst(32, 0, out I1temp4);

            HTuple I2temp1 = new HTuple();
            HOperatorSet.TupleGenConst(32, 0, out I2temp1);
            HTuple I2temp2 = new HTuple();
            HOperatorSet.TupleGenConst(32, 0, out I2temp2);
            HTuple I2temp3 = new HTuple();
            HOperatorSet.TupleGenConst(32, 0, out I2temp3);
            HTuple I2temp4 = new HTuple();
            HOperatorSet.TupleGenConst(32, 0, out I2temp4);

            HTuple I3temp1 = new HTuple();
            HOperatorSet.TupleGenConst(32, 0, out I3temp1);
            HTuple I3temp2 = new HTuple();
            HOperatorSet.TupleGenConst(32, 0, out I3temp2);
            HTuple I3temp3 = new HTuple();
            HOperatorSet.TupleGenConst(32, 0, out I3temp3);
            HTuple I3temp4 = new HTuple();
            HOperatorSet.TupleGenConst(32, 0, out I3temp4);

            //内排轮询取结果
            while (true)
            {
                Thread.Sleep(1);
                //内1
                switch (Step1)
                {
                    case 0:
                        {
                            if (ToneProcesses[0].ResQueuqe.TryDequeue(out ccd1CurOrder) && ccd1CurOrder > 8)
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
                                if (pp != null && pp.complete == true)
                                {
                                    //销毁结果，取得数据
                                    if (ToneProcesses[0].ProductMap.TryRemove(ccd1CurOrder, out var p)) 
                                    {
                                        bool Signal = false; // 相机标志位
                                        if (p.Res != null)
                                        {
                                            LogNet.Info("内1结果：" + p.Res.ToString() + "-次数：" + ccd1CurOrder);
                                            // 判断拍照次数
                                        }

                                        else
                                        {
                                            LogNet.Error("内1结果：空" + "-次数：" + ccd1CurOrder);
                                        }
                                        // 判断拍照次数

                                        if (ccd1CurOrder % 4 == 0)
                                        {
                                            I1temp1 = p.Res;
                                        }
                                        if (ccd1CurOrder % 4 == 1)
                                        {
                                            I1temp2 = p.Res;
                                        }
                                        if (ccd1CurOrder % 4 == 2)
                                        {
                                            I1temp3 = p.Res;
                                        }
                                        if (ccd1CurOrder % 4 == 3)
                                        {
                                            I1temp4 = p.Res;
                                        }
                                        if (ccd1CurOrder > 8)
                                        {
                                            for (int i = 0; i < 8; i++)
                                            {
                                                WENYUPCIE.WenYu.WriteIO(0, i, 1);
                                                bool shouldReject = false; // 当前孔位的剔除标记
                                                switch (ccd1CurOrder % 4)
                                                {
                                                    case 0:
                                                        if (I1temp2[24 + i] == 1 || I1temp3[16 + i] == 1 || I1temp4[8 + i] == 1 || I1temp1[i] == 1)
                                                        {
                                                            shouldReject = true;  // 孔位剔除信号
                                                            Signal = true; //相机标志信号       
                                                        }
                                                        break;
                                                    case 1:
                                                        if (I1temp3[24 + i] == 1 || I1temp4[16 + i] == 1 || I1temp1[8 + i] == 1 || I1temp2[i] == 1)
                                                        {
                                                            shouldReject = true;  // 孔位剔除信号
                                                            Signal = true; //相机标志信号       
                                                        }
                                                        break;
                                                    case 2:
                                                        if (I1temp4[24 + i] == 1 || I1temp1[16 + i] == 1 || I1temp2[8 + i] == 1 || I1temp3[i] == 1)
                                                        {
                                                            shouldReject = true;  // 孔位剔除信号
                                                            Signal = true; //相机标志信号       
                                                        }
                                                        break;
                                                    case 3:
                                                        if (I1temp1[24 + i] == 1 || I1temp2[16 + i] == 1 || I1temp3[8 + i] == 1 || I1temp4[i] == 1)
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
                                                    WENYUPCIE.WenYu.WriteIO(0, i, 0);
                                                    //LogNet.Info("内1孔位剔除 :   " + i);                    
                                                }
                                            }
                                            if (Signal)
                                            {
                                                //打开相机标志信号
                                                WENYUPCIE.WenYu.WriteOutputIO(1, 0, 1);
                                                //LogNet.Info("内1相机剔除。。。  ");
                                            }
                                        }
                                        Step1 = 0;
                                    }
                                    else
                                    {
                                        LogNet.Warn(" 内1算法异常" + ccd1CurOrder);
                                    }
                                    Step1 = 0;
                                }
                            }
                            else
                            {
                                LogNet.Warn(" 内1算法超时" + ccd1CurOrder);
                                Step1 = 0;
                            }
                            break;
                        }

                }
                //内2
                switch (Step2)
                {
                    case 0:
                        {
                            if (ToneProcesses[1].ResQueuqe.TryDequeue(out ccd2CurOrder) && ccd2CurOrder > 8)
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
                                if (pp != null && pp.complete == true)
                                {
                                    //判断是否超时
                                    if (ToneProcesses[1].ProductMap.TryRemove(ccd2CurOrder, out var p))
                                    {
                                        bool Signal = false; // 相机标志位                                    
                                        if (p.Res != null)
                                        {
                                            LogNet.Info("内2结果：" + p.Res.ToString() + "-次数：" + ccd2CurOrder);
                                        }
                                        else
                                        {
                                            LogNet.Info("内2结果：空" + "-次数：" + ccd2CurOrder);
                                        }
                                        //判断拍照次数
                                        if (ccd2CurOrder % 4 == 0)
                                        {
                                            I2temp1 = p.Res;
                                        }
                                        if (ccd1CurOrder % 4 == 1)
                                        {
                                            I2temp2 = p.Res;
                                        }
                                        if (ccd1CurOrder % 4 == 2)
                                        {
                                            I2temp3 = p.Res;
                                        }
                                        if (ccd1CurOrder % 4 == 3)
                                        {
                                            I2temp4 = p.Res;
                                        }

                                        if (ccd2CurOrder > 8)
                                        {
                                            for (int i = 0; i < 8; i++)
                                            {
                                                WENYUPCIE.WenYu.WriteIO(0, i, 1);
                                                bool shouldReject = false; // 当前孔位的剔除标记
                                                switch (ccd2CurOrder % 4)
                                                {
                                                    case 0:
                                                        if (I2temp2[24 + i] == 1 || I2temp3[16 + i] == 1 || I2temp4[8 + i] == 1 || I2temp1[i] == 1)
                                                        {
                                                            shouldReject = true;  // 孔位剔除信号
                                                            Signal = true; //相机标志信号       
                                                        }
                                                        break;
                                                    case 1:
                                                        if (I2temp3[24 + i] == 1 || I2temp4[16 + i] == 1 || I2temp1[8 + i] == 1 || I2temp2[i] == 1)
                                                        {
                                                            shouldReject = true;  // 孔位剔除信号
                                                            Signal = true; //相机标志信号       
                                                        }
                                                        break;
                                                    case 2:
                                                        if (I2temp4[24 + i] == 1 || I2temp1[16 + i] == 1 || I2temp2[8 + i] == 1 || I2temp3[i] == 1)
                                                        {
                                                            shouldReject = true;  // 孔位剔除信号
                                                            Signal = true; //相机标志信号       
                                                        }
                                                        break;
                                                    case 3:
                                                        if (I2temp1[24 + i] == 1 || I2temp2[16 + i] == 1 || I2temp3[8 + i] == 1 || I2temp4[i] == 1)
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
                                                    WENYUPCIE.WenYu.WriteIO(0, i, 0);
                                                    //LogNet.Info("内2孔位剔除 :   " + i);                    
                                                }
                                            }
                                            if (Signal)
                                            {
                                                //打开相机标志信号
                                                WENYUPCIE.WenYu.WriteOutputIO(1, 1, 1);
                                                //LogNet.Info("内2相机剔除。。。  ");
                                            }
                                        }
                                        Step2 = 0;
                                    }
                                    else
                                    {
                                        LogNet.Warn(" 内2算法超时" + ccd2CurOrder);
                                    }
                                    Step2 = 0;
                                }
                            }
                            else
                            {
                                LogNet.Warn(" 内2算法超时" + ccd2CurOrder);
                                Step2 = 0;
                            }
                            break;
                        }
                }
                //内3
                switch (Step3)
                {
                    case 0:
                        {
                            if (ToneProcesses[2].ResQueuqe.TryDequeue(out ccd3CurOrder) && ccd3CurOrder > 8)
                            {

                                ccdResTime3 = HighTime.GetMSec() + WaitTime1;
                                Step3 = 1;
                            }
                        }
                        break;
                    case 1:
                        {
                            double cur = HighTime.GetMSec();
                            if (cur < ccdResTime3 )
                            {
                                //尝试取结果
                                ToneProcesses[2].ProductMap.TryGetValue(ccd3CurOrder, out var pp);
                                //有结果处理
                                if (pp != null && pp.complete == true)
                                {
                                    //判断是否超时                               
                                    if (ToneProcesses[2].ProductMap.TryRemove(ccd3CurOrder, out var p)) 
                                    {
                                        bool Signal = false; // 相机标志位
                                        if (p.Res != null)
                                        {
                                            LogNet.Info("内3结果：" + p.Res.ToString() + "-次数：" + ccd3CurOrder);
                                        }
                                        else
                                        {
                                            LogNet.Info("内3结果：空" + "-次数：" + ccd3CurOrder);
                                        }
                                        //判断拍照次数
                                        if (ccd3CurOrder % 4 == 0)
                                        {
                                            I3temp1 = p.Res;
                                        }
                                        if (ccd3CurOrder % 4 == 1)
                                        {
                                            I3temp2 = p.Res;
                                        }
                                        if (ccd3CurOrder % 4 == 2)
                                        {
                                            I3temp3 = p.Res;
                                        }
                                        if (ccd3CurOrder % 4 == 3)
                                        {
                                            I3temp4 = p.Res;
                                        }

                                        if (ccd3CurOrder > 8)
                                        {
                                            for (int i = 0; i < 8; i++)
                                            {
                                                WENYUPCIE.WenYu.WriteIO(0, i, 1);
                                                bool shouldReject = false; // 当前孔位的剔除标记
                                                switch (ccd3CurOrder % 4)
                                                {
                                                    case 0:
                                                        if (I3temp2[24 + i] == 1 || I3temp3[16 + i] == 1 || I3temp4[8 + i] == 1 || I3temp1[i] == 1)
                                                        {
                                                            shouldReject = true;  // 孔位剔除信号
                                                            Signal = true; //相机标志信号       
                                                        }
                                                        break;
                                                    case 1:
                                                        if (I3temp3[24 + i] == 1 || I3temp4[16 + i] == 1 || I3temp1[8 + i] == 1 || I3temp2[i] == 1)
                                                        {
                                                            shouldReject = true;  // 孔位剔除信号
                                                            Signal = true; //相机标志信号       
                                                        }
                                                        break;
                                                    case 2:
                                                        if (I3temp4[24 + i] == 1 || I3temp1[16 + i] == 1 || I3temp2[8 + i] == 1 || I3temp3[i] == 1)
                                                        {
                                                            shouldReject = true;  // 孔位剔除信号
                                                            Signal = true; //相机标志信号       
                                                        }
                                                        break;
                                                    case 3:
                                                        if (I3temp1[24 + i] == 1 || I3temp2[16 + i] == 1 || I3temp3[8 + i] == 1 || I3temp4[i] == 1)
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
                                                    WENYUPCIE.WenYu.WriteIO(0, i, 0);
                                                    //LogNet.Info("内3孔位剔除 :   " + i);                    
                                                }
                                            }
                                            if (Signal)
                                            {
                                                //打开相机标志信号
                                                WENYUPCIE.WenYu.WriteOutputIO(1, 2, 1);
                                                //LogNet.Info("内3相机剔除。。。  ");
                                            }
                                            Step3 = 0;
                                        }                                       
                                    }
                                    else
                                    {
                                        LogNet.Warn(" 内3算法异常" + ccd3CurOrder);
                                    }
                                    Step3 = 0;
                                }                                
                            }
                            else
                            {
                                LogNet.Warn(" 内3算法超时"+ ccd3CurOrder);
                                Step3 = 0;
                            }
                            break;
                        }

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

            HTuple O1temp1 = new HTuple();
            HOperatorSet.TupleGenConst(32, 0, out O1temp1);
            HTuple O1temp2 = new HTuple();
            HOperatorSet.TupleGenConst(32, 0, out O1temp2);
            HTuple O1temp3 = new HTuple();
            HOperatorSet.TupleGenConst(32, 0, out O1temp3);
            HTuple O1temp4 = new HTuple();
            HOperatorSet.TupleGenConst(32, 0, out O1temp4);

            HTuple O2temp1 = new HTuple();
            HOperatorSet.TupleGenConst(32, 0, out O2temp1);
            HTuple O2temp2 = new HTuple();
            HOperatorSet.TupleGenConst(32, 0, out O2temp2);
            HTuple O2temp3 = new HTuple();
            HOperatorSet.TupleGenConst(32, 0, out O2temp3);
            HTuple O2temp4 = new HTuple();
            HOperatorSet.TupleGenConst(32, 0, out O2temp4);

            HTuple O3temp1 = new HTuple();
            HOperatorSet.TupleGenConst(32, 0, out O3temp1);
            HTuple O3temp2 = new HTuple();
            HOperatorSet.TupleGenConst(32, 0, out O3temp2);
            HTuple O3temp3 = new HTuple();
            HOperatorSet.TupleGenConst(32, 0, out O3temp3);
            HTuple O3temp4 = new HTuple();
            HOperatorSet.TupleGenConst(32, 0, out O3temp4);
            while (true)
            {
                Thread.Sleep(1);

                //相机4
                switch (Step4)
                {
                    case 0:
                        {
                            if (ToneProcesses[3].ResQueuqe.TryDequeue(out ccd4CurOrder) && ccd4CurOrder > 8)
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
                                if (pp != null && pp.complete == true)
                                {
                                    if (ToneProcesses[3].ProductMap.TryRemove(ccd4CurOrder, out var p))
                                    {
                                        bool Signal = false; // 相机标志位
                                        if (p.Res != null)
                                        {
                                            LogNet.Info("外1结果：" + p.Res.ToString() + "-次数：" + ccd4CurOrder);
                                        }
                                        else
                                        {
                                            LogNet.Info("外1结果：空" + "-次数：" + ccd4CurOrder);
                                        }
                                        //判断拍照次数
                                        if (ccd4CurOrder % 4 == 0)
                                        {
                                            O1temp1 = p.Res;
                                        }
                                        if (ccd4CurOrder % 4 == 1)
                                        {
                                            O1temp2 = p.Res;
                                        }
                                        if (ccd4CurOrder % 4 == 2)
                                        {
                                            O1temp3 = p.Res;
                                        }
                                        if (ccd4CurOrder % 4 == 3)
                                        {
                                            O1temp4 = p.Res;
                                        }

                                        if (ccd4CurOrder > 8)
                                        {
                                            for (int i = 0; i < 8; i++)
                                            {
                                                WENYUPCIE.WenYu.WriteIO(0, i, 1);
                                                bool shouldReject = false; // 当前孔位的剔除标记
                                                switch (ccd4CurOrder % 4)
                                                {
                                                    case 0:
                                                        if (O1temp2[24 + i] == 1 || O1temp3[16 + i] == 1 || O1temp4[8 + i] == 1 || O1temp1[i] == 1)
                                                        {
                                                            shouldReject = true;  // 孔位剔除信号
                                                            Signal = true; //相机标志信号       
                                                        }
                                                        break;
                                                    case 1:
                                                        if (O1temp3[24 + i] == 1 || O1temp4[16 + i] == 1 || O1temp1[8 + i] == 1 || O1temp2[i] == 1)
                                                        {
                                                            shouldReject = true;  // 孔位剔除信号
                                                            Signal = true; //相机标志信号       
                                                        }
                                                        break;
                                                    case 2:
                                                        if (O1temp4[24 + i] == 1 || O1temp1[16 + i] == 1 || O1temp2[8 + i] == 1 || O1temp3[i] == 1)
                                                        {
                                                            shouldReject = true;  // 孔位剔除信号
                                                            Signal = true; //相机标志信号       
                                                        }
                                                        break;
                                                    case 3:
                                                        if (O1temp1[24 + i] == 1 || O1temp2[16 + i] == 1 || O1temp3[8 + i] == 1 || O1temp4[i] == 1)
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
                                                    WENYUPCIE.WenYu.WriteIO(0, i + 8, 0);
                                                    //LogNet.Info("外1孔位剔除 :   " + i);                    
                                                }
                                            }
                                            if (Signal)
                                            {
                                                //打开相机标志信号
                                                WENYUPCIE.WenYu.WriteOutputIO(1, 3, 1);
                                                //LogNet.Info("外1相机剔除。。。  ");
                                            }
                                        }
                                        Step4 = 0;
                                    }
                                    else
                                    {
                                        LogNet.Warn(" 外1算法异常" + ccd4CurOrder);
                                    }
                                    Step4 = 0;
                                }
                            }
                            else
                            {
                                LogNet.Warn(" 外1算法超时" + ccd4CurOrder);
                                Step4 = 0;
                            }
                            break;
                        }
                }
                //相机5
                switch (Step5)
                {
                    case 0:
                        {
                            if (ToneProcesses[4].ResQueuqe.TryDequeue(out ccd5CurOrder) && ccd5CurOrder > 8)
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
                                if (pp != null && pp.complete == true)
                                {
                                    if (ToneProcesses[4].ProductMap.TryRemove(ccd5CurOrder, out var p))
                                    {
                                        bool Signal = false; // 相机标志位
                                        if (p.Res != null)
                                        {
                                            LogNet.Info("外2结果：" + p.Res.ToString() + "-次数：" + ccd5CurOrder);
                                        }
                                        else
                                        {
                                            LogNet.Info("外2结果：空" + "-次数：" + ccd5CurOrder);
                                        }
                                        //判断拍照次数
                                        if (ccd5CurOrder % 4 == 0)
                                        {
                                            O2temp1 = p.Res;
                                        }
                                        if (ccd5CurOrder % 4 == 1)
                                        {
                                            O2temp2 = p.Res;
                                        }
                                        if (ccd5CurOrder % 4 == 2)
                                        {
                                            O2temp3 = p.Res;
                                        }
                                        if (ccd5CurOrder % 4 == 3)
                                        {
                                            O2temp4 = p.Res;
                                        }

                                        if (ccd5CurOrder > 8)
                                        {
                                            for (int i = 0; i < 8; i++)
                                            {
                                                WENYUPCIE.WenYu.WriteIO(0, i, 1);
                                                bool shouldReject = false; // 当前孔位的剔除标记
                                                switch (ccd5CurOrder % 4)
                                                {
                                                    case 0:
                                                        if (O2temp2[24 + i] == 1 || O2temp3[16 + i] == 1 || O2temp4[8 + i] == 1 || O2temp1[i] == 1)
                                                        {
                                                            shouldReject = true;  // 孔位剔除信号
                                                            Signal = true; //相机标志信号       
                                                        }
                                                        break;
                                                    case 1:
                                                        if (O2temp3[24 + i] == 1 || O2temp4[16 + i] == 1 || O2temp1[8 + i] == 1 || O2temp2[i] == 1)
                                                        {
                                                            shouldReject = true;  // 孔位剔除信号
                                                            Signal = true; //相机标志信号       
                                                        }
                                                        break;
                                                    case 2:
                                                        if (O2temp4[24 + i] == 1 || O2temp1[16 + i] == 1 || O2temp2[8 + i] == 1 || O2temp3[i] == 1)
                                                        {
                                                            shouldReject = true;  // 孔位剔除信号
                                                            Signal = true; //相机标志信号       
                                                        }
                                                        break;
                                                    case 3:
                                                        if (O2temp1[24 + i] == 1 || O2temp2[16 + i] == 1 || O2temp3[8 + i] == 1 || O2temp4[i] == 1)
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
                                                    WENYUPCIE.WenYu.WriteIO(0, i + 8, 0);
                                                    //LogNet.Info("外2孔位剔除 :   " + i);                    
                                                }
                                            }
                                            if (Signal)
                                            {
                                                //打开相机标志信号
                                                WENYUPCIE.WenYu.WriteOutputIO(1, 4, 1);
                                                //LogNet.Info("外2相机剔除。。。  ");
                                            }
                                        }
                                        Step5 = 0;
                                    }
                                    else
                                    {
                                        LogNet.Warn(" 外2算法异常" + ccd5CurOrder);
                                        Step5 = 0;
                                    }
                                }
                            }

                            else
                            {
                                LogNet.Warn(" 外2算法超时" + ccd5CurOrder);
                                Step5 = 0;
                            }

                            break;
                        }

                }
                //相机6
                switch (Step6)
                {
                    case 0:
                        {
                            if (ToneProcesses[5].ResQueuqe.TryDequeue(out ccd6CurOrder) && ccd6CurOrder > 8)
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
                                if (pp != null && pp.complete == true)
                                {
                                    if (ToneProcesses[5].ProductMap.TryRemove(ccd6CurOrder, out var p))
                                    {
                                        bool Signal = false; // 相机标志位
                                        if (p.Res != null)
                                        {
                                            LogNet.Info("外3结果：" + p.Res.ToString() + "-次数：" + ccd6CurOrder);
                                        }
                                        else
                                        {
                                            LogNet.Info("外3结果：空" + "-次数：" + ccd6CurOrder);
                                        }
                                        //判断拍照次数
                                        if (ccd6CurOrder % 4 == 0)
                                        {
                                            O3temp1 = p.Res;
                                        }
                                        if (ccd6CurOrder % 4 == 1)
                                        {
                                            O3temp2 = p.Res;
                                        }
                                        if (ccd6CurOrder % 4 == 2)
                                        {
                                            O3temp3 = p.Res;
                                        }
                                        if (ccd6CurOrder % 4 == 3)
                                        {
                                            O3temp4 = p.Res;
                                        }

                                        if (ccd6CurOrder >= 8)
                                        {
                                            for (int i = 0; i < 8; i++)
                                            {
                                                WENYUPCIE.WenYu.WriteIO(0, i, 1);
                                                bool shouldReject = false; // 当前孔位的剔除标记
                                                switch (ccd6CurOrder % 4)
                                                {
                                                    case 0:
                                                        if (O3temp2[24 + i] == 1 || O3temp3[16 + i] == 1 || O3temp4[8 + i] == 1 || O3temp1[i] == 1)
                                                        {
                                                            shouldReject = true;  // 孔位剔除信号
                                                            Signal = true; //相机标志信号       
                                                        }
                                                        break;
                                                    case 1:
                                                        if (O3temp3[24 + i] == 1 || O3temp4[16 + i] == 1 || O3temp1[8 + i] == 1 || O3temp2[i] == 1)
                                                        {
                                                            shouldReject = true;  // 孔位剔除信号
                                                            Signal = true; //相机标志信号       
                                                        }
                                                        break;
                                                    case 2:
                                                        if (O3temp4[24 + i] == 1 || O3temp1[16 + i] == 1 || O3temp2[8 + i] == 1 || O3temp3[i] == 1)
                                                        {
                                                            shouldReject = true;  // 孔位剔除信号
                                                            Signal = true; //相机标志信号       
                                                        }
                                                        break;
                                                    case 3:
                                                        if (O3temp1[24 + i] == 1 || O3temp2[16 + i] == 1 || O3temp3[8 + i] == 1 || O3temp4[i] == 1)
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
                                                    WENYUPCIE.WenYu.WriteIO(0, i + 8, 0);
                                                    //LogNet.Info("外3孔位剔除 :   " + i);                    
                                                }
                                            }
                                            if (Signal)
                                            {
                                                //打开相机标志信号
                                                WENYUPCIE.WenYu.WriteOutputIO(1, 5, 1);
                                                //LogNet.Info("外3相机剔除。。。  ");
                                            }
                                        }
                                        Step6 = 0;
                                    }
                                    else
                                    {
                                        LogNet.Warn(" 外2算法异常" + ccd6CurOrder);
                                        Step6 = 0;
                                    }
                                }
                            }
                            else
                            {
                                LogNet.Warn(" 外3算法超时" + ccd6CurOrder);
                                Step6 = 0;
                            }

                            break;
                        }
                }
            }
        }
        public static void Destroy()
        {
            foreach (var p in ToneProcesses)
            {
                p.Destroy();
            }
            WENYUPCIE.WenYu.CloseTWO();
            ExtHandler.Destroy();
        }

    }
}
