using HalconDotNet;
using LittleCommon.Domain;
using LittleCommon.Tool;
using RobotLocation.Model;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using VisionCore.Ext;
using VisionCore.Log;
using WENYUPCIE;

namespace RobotLocation.Service
{
    internal class ToneOp
    {
        //相机
        public static List<ToneProcess> ToneProcesses = new List<ToneProcess>();
        //结果数组
        public static HTuple[] BatchResults { get; set; } = new HTuple[4];

        public static Thread RunTH;
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
                Exe();
            });
            RunTH.IsBackground = true;
            RunTH.Priority = ThreadPriority.Highest;
            RunTH.Start();
            return r;
        }
        public static void Exe()
        {
            //内排轮询取结果
            while (true)
            {
                Thread.Sleep(2);
                //内1
                foreach (var order in ToneProcesses[0].ProductMap.Keys.ToList())
                {
                    
                        if (ToneProcesses[0].ProductMap.TryGetValue(order, out var p))
                        {
                            if (p.complete == true)
                            {
                                DetermineRejection(p, 0, 0);
                                ToneProcesses[0].ProductMap.TryRemove(order, out _);
                            }
                        }
                    
                }

                //内2
                foreach (var order in ToneProcesses[1].ProductMap.Keys.ToList())
                {

                    if (ToneProcesses[1].ProductMap.TryGetValue(order, out var p))
                    {
                        if (p.complete == true)
                        {
                            DetermineRejection(p, 1, 0);
                            ToneProcesses[1].ProductMap.TryRemove(order, out _);
                        }
                    }

                }

                //内3
                foreach (var order in ToneProcesses[2].ProductMap.Keys.ToList())
                {

                    if (ToneProcesses[2].ProductMap.TryGetValue(order, out var p))
                    {
                        if (p.complete == true)
                        {
                            DetermineRejection(p, 2, 0);
                            ToneProcesses[2].ProductMap.TryRemove(order, out _);
                        }
                    }

                }

                //外1
                foreach (var order in ToneProcesses[3].ProductMap.Keys.ToList())
                {

                    if (ToneProcesses[3].ProductMap.TryGetValue(order, out var p))
                    {
                        if (p.complete == true)
                        {
                            DetermineRejection(p, 3, 0);
                            ToneProcesses[3].ProductMap.TryRemove(order, out _);
                        }
                    }

                }

                //外2
                foreach (var order in ToneProcesses[4].ProductMap.Keys.ToList())
                {

                    if (ToneProcesses[4].ProductMap.TryGetValue(order, out var p))
                    {
                        if (p.complete == true)
                        {
                            DetermineRejection(p, 4, 0);
                            ToneProcesses[4].ProductMap.TryRemove(order, out _);
                        }
                    }

                }

                //外3
                foreach (var order in ToneProcesses[5].ProductMap.Keys.ToList())
                {

                    if (ToneProcesses[5].ProductMap.TryGetValue(order, out var p))
                    {
                        if (p.complete == true)
                        {
                            DetermineRejection(p, 5, 0);
                            ToneProcesses[5].ProductMap.TryRemove(order, out _);
                        }
                    }

                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p：结果字典"></param>
        /// <param name="camera：相机（0-5）"></param>
        /// <param name="cardid：IO卡 （内 = 0  ，外 = 1）"></param>
        public static void DetermineRejection(Product p,int camera,int cardid)
        {
            try
            {
                bool Signal = false; // 相机标志位
                int batchIndex = p.Order % 4;
                if (p.Res != null)
                {
                    BatchResults[batchIndex] = p.Res;
                }
                else
                {
                    HTuple hv_Newtuple;
                    HOperatorSet.TupleGenConst(32, -1, out hv_Newtuple);
                    BatchResults[batchIndex] = hv_Newtuple;
                    LogNet.Info("结果数组 ：空" + "-次数：" + p.Order.ToString());
                }


                if (p.Order >= 4)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        WENYUPCIE.WenYu.WriteIO(cardid, i, 1);
                        bool shouldReject = false; // 当前孔位的剔除标记

                        int row1 = 24 + i;
                        int row2 = 16 + i;
                        int row3 = 8 + i;
                 
                        switch (batchIndex)
                        {
                            case 0:
                                if (BatchResults[2][row1] == 1 || BatchResults[3][row2] == 1 || BatchResults[0][row3] == 1 || BatchResults[1][i] == 1)
                                {
                                    shouldReject = true;  // 孔位剔除信号
                                    Signal = true; //相机标志信号       
                                }
                                break;
                            case 1:
                                if (BatchResults[3][row1] == 1 || BatchResults[0][row2] == 1 || BatchResults[1][row3] == 1 || BatchResults[2][i] == 1)
                                {
                                    shouldReject = true;  // 孔位剔除信号
                                    Signal = true; //相机标志信号       
                                }
                                break;
                            case 2:
                                if (BatchResults[0][row1] == 1 || BatchResults[1][row2] == 1 || BatchResults[2][row3] == 1 || BatchResults[3][i] == 1)
                                {
                                    shouldReject = true;  // 孔位剔除信号
                                    Signal = true; //相机标志信号       
                                }
                                break;
                            case 3:
                                if (BatchResults[1][row1] == 1 || BatchResults[2][row2] == 1 || BatchResults[3][row3] == 1 || BatchResults[0][i] == 1)
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
                            WENYUPCIE.WenYu.WriteIO(0, 8* cardid + i, 0);                 
                        }
                    }
                    if (Signal)
                    {
                        //打开相机标志信号
                        WENYUPCIE.WenYu.WriteOutputIO(cardid, camera, 1);
                    }
                }


            }
            catch (Exception ex)
            {
                LogNet.Error($"剔除判断异常:   "+ ex.ToString());
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
