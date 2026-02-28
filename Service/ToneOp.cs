using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using cszmcaux;
using HalconDotNet;
using RobotLocation.Model;
using RobotLocation.UI;
using Sunny.UI;
using VisionCore.Ext;
using VisionCore.Log;
namespace RobotLocation.Service
{
    public class ToneOp
    {
        #region 定义后台线程变量
        public static int CameraI = 0;
        public static int CameraO = 0;
        public static float Mspeed = 0;       
        public static List<ToneProcess> ToneProcesses = new List<ToneProcess>();
        //public static HTuple[][] BatchResults = new HTuple[6][]; // 每个相机有4个批次结果
        //public static HTuple[][] Ra = new HTuple[2][]; // 长轴
        //public static HTuple[][] Rb = new HTuple[2][]; // 短轴

        public static bool[] CameraSignalFlags = new bool[6]; // 相机标志信号
        //public static ConcurrentDictionary<int, int>[] IOSignalFlags = new ConcurrentDictionary<int, int>[6]; // 6个相机，每个相机的信号值字典

        // 在 ToneOp 类中添加静态退出标志
        public static bool IsShuttingDownioi = false;
        public static bool IsShuttingDownioo = false;
        public static bool IsShuttingDowncame = false;
        public static double ration = 0.038847;
        public static double ratiow = 0.038822;
        public static long MposI1 { get; set; }
        public static long MposO1 { get; set; }
        public static long OldMposI = 0;
        public static long OldMposO = 0;
        public static int  passI = 0;//内排空槽剔除
        public static int passO = 0;//外排空槽剔除
        public static bool passII = false;//内排空槽剔除标记
        public static bool passOO = false;//外排空槽剔除标记        
        //崔
        public static ConcurrentDictionary<long, res> resMap1 = new ConcurrentDictionary<long, res>();
        public static ConcurrentDictionary<long, res> resMap2 = new ConcurrentDictionary<long, res>();
        public static ConcurrentDictionary<long, res> resMap3 = new ConcurrentDictionary<long, res>();
        public static ConcurrentDictionary<long, res> resMap4 = new ConcurrentDictionary<long, res>();
        public static ConcurrentDictionary<long, res> resMap5 = new ConcurrentDictionary<long, res>();
        public static ConcurrentDictionary<long, res> resMap6 = new ConcurrentDictionary<long, res>();
        public static ring neibuffer = new ring(200,18,2,4,4);   // 内排数据对象
        public static ring waibuffer = new ring(225, 16, 2, 4, 4);   // 外排数据对象

        public static ConcurrentDictionary<long, ppenc> encMapi = new ConcurrentDictionary<long, ppenc>();
        public static ConcurrentDictionary<long, ppenc> encMapo = new ConcurrentDictionary<long, ppenc>();
        public static uint lasttime1 = 0;//上一次的拍照间隔，用于每次判断比较丢帧
        public static uint lasttime2 = 0;
        public static uint lasttime3 = 0;
        public static uint lasttime4 = 0;
        public static uint lasttime5 = 0;
        public static uint lasttime6 = 0;
        public static int Dtime1 = 0;//拍照间隔的差值，判断丢帧
        public static int Dtime2 = 0;
        public static int Dtime3 = 0;
        public static int Dtime4 = 0;
        public static int Dtime5 = 0;
        public static int Dtime6 = 0;
        public static uint passid1 = 0;//记录帧号跳过值
        public static uint passid2 = 0;//记录帧号跳过值
        public static uint passid3 = 0;//记录帧号跳过值
        public static uint passid4 = 0;//记录帧号跳过值
        public static uint passid5 = 0;//记录帧号跳过值
        public static uint passid6 = 0;//记录帧号跳过值
        public static bool isnull1=false;
        public static bool isnull2=false;
        public static bool isnull3=false;
        public static bool isnull4=false;
        public static bool isnull5=false;
        public static bool isnull6=false;
        public static int nullcount1 = 0;
        public static int nullcount2 = 0;
        public static int nullcount3 = 0;
        public static int nullcount4 = 0;
        public static int nullcount5 = 0;
        public static int nullcount6 = 0;
        #endregion

        #region 清理数据
        public static void Reset()
        {
            ToneOp.CameraI = 0;
            ToneOp.CameraO = 0;
            ToneOp.ToneProcesses[0].curOrder = 0;
            ToneOp.ToneProcesses[1].curOrder = 0;
            ToneOp.ToneProcesses[2].curOrder = 0;
            ToneOp.ToneProcesses[3].curOrder = 0;
            ToneOp.ToneProcesses[4].curOrder = 0;
            ToneOp.ToneProcesses[5].curOrder = 0;
            CurOrderI = 0;
            CurOrderO = 0;
            //foreach (var p in ToneProcesses.ToList())
            //{
            //    try
            //    {
            //        p.ProductMap.Clear();
            //        p.ProcessQueuqe.Clear();
            //        p.ResQueuqe.Clear();
            //    }
            //    catch (Exception ex)
            //    {
            //        LogNet.Error($"清除缓存 {p.Name} 时发生异常: {ex.Message}");
            //    }
            //}
            //incount = 0;
            //outcount = 0;
            //OldMposI = 0;
            //OldMposO = 0;
            //resMap1.Clear();
            //resMap2.Clear();
            //resMap3.Clear();
            //resMap4.Clear();
            //resMap5.Clear();
            //resMap6.Clear();
            if (ExtHandler.GetGlobalVar<string>("启动强清") == "true")
            {
                neibuffer.WriteOutRow(incount, incount - mPlcData.N1 - 10);
                waibuffer.WriteOutRow(outcount, outcount - mPlcData.W1 - 10);
                //BlowStart();
            }
            //neibuffer.Clear();
            //waibuffer.Clear();
            //encMapi.Clear();
            //encMapo.Clear();
            //zmcaux.ZAux_Direct_SetMpos(g_handle, 1, 0);
            //zmcaux.ZAux_Direct_SetDpos(g_handle, 1, 0);
        }
        #endregion

        #region 初始化
        public static bool Init()
        {
            //step1:尝试打开IO口，失败后即刻返回
            bool r1 = OpenZm();
            if (!r1)
            {
                LogNet.Error("控制卡打开失败,");
                //return false;
            }
            else { LogNet.Info("控制卡加载成功"); }
            //爆珠项目专用数据随想和字典
            //for (int i = 0; i < 6; i++)
            //{
            //    //BatchResults[i] = new HTuple[4];
            //    IOSignalFlags[i] = new ConcurrentDictionary<int, int>(); // 初始化每个相机的信号值字典
            //}
            //for (int i = 0; i < 2; i++)
            //{
            //    Ra[i] = new HTuple[4];
            //    Rb[i] = new HTuple[4];
            //}
            //step3,根据流程建立对应的流程列表和线程，要求流程名对应相应的相机名
            ToneProcesses.Add(new ToneProcess() { Name = "内1" });
            ToneProcesses.Add(new ToneProcess() { Name = "内2" });
            ToneProcesses.Add(new ToneProcess() { Name = "内3" });
            ToneProcesses.Add(new ToneProcess() { Name = "外1" });
            ToneProcesses.Add(new ToneProcess() { Name = "外2" });
            ToneProcesses.Add(new ToneProcess() { Name = "外3" });
            //step3:尝试拉起相机线程
            //初始化流程对应的线程
            foreach (var p in ToneProcesses.ToList())
            {
                try
                {
                    var pr = p.Init();
                    if (!pr)
                    {
                        LogNet.Error(p.Name + "相机线程启动失败");
                    }
                }
                catch (Exception ex)
                {
                    LogNet.Error($"初始化线程 {p.Name} 时发生异常: {ex.Message}");
                }
            }
            //sstep4:开启相结果缓存
            for (int i = 0; i < ToneProcesses.Count; i++)
            {
                int cameraIndex = i;
                Task.Run(() => ProcessCamera(cameraIndex), CancellationToken.None);
            }
            return true;
        }
        #endregion

        #region 控制卡，正运动

        #region 定义运动控制卡变量
        public static GCHandle gch;
        public static string AutoUpStr = "";
        public static Int32 AutoUpLen = 0;
        public static int controlReturn = 0;
        //IO数量
        public static int IOCount = 8;
        public static IntPtr g_handleAutoUp;
        public static ConnectContext cc;
        //
        public static IntPtr g_handle;         //链接返回的句柄，可以作为卡号
        public static IntPtr handle_null = (IntPtr)0; //空句柄，表示无控制卡连接      
        public static int[] Reg_Flag;//静态数组，用于存储锁存标志，可能用于标记锁存操作是否成功或是否需要执行。
        public static float[] Reg_Pos;//静态数组，用于存储锁存位置，可能用于记录每次锁存操作的位置。
        public static int mode_REG = 4;//标识锁存操作的模式
        public int Reg_COUNT = 0;//计数锁存操作的次数
        public static float[] POS_OF_HW;//存储硬件位置比较输出的位置值。
        public static int[] TIME_OF_HW;//存储硬件位置比较输出的时间值。
        public static int[] STATUS_OF_HW;//存储硬件位置比较输出的状态值。
        public static int[] OP_OF_HW;//存储硬件位置比较输出的操作数。
        public static int[] HW_SKI_COUNT;//计数硬件位置比较输出的跳过次数。
        public static int[] HW_RUN_COUNT; //硬件位置比较输出的执行次数。
        public static int Hw_Reg_Space = 30022;//硬件寄存器的总空间大小。
        public static int Hw_Reg_tab_space = 128; //硬件寄存器表的空间大小。
        public static int Hw_Reg_Tab_Star = 100;//硬件寄存器表的起始位置。
        public static int G_run_flag = 0;//控制循环的运行状态。值为 0 表示停止，值为 1 表示运行。
        //当前内盘序号
        public static int CurOrderI = 0;
        public static int CurOrderO = 0;
        public static int[] count = new int[16];
        public static int[] tab_count = new int[16];
        //定义参数
        public static int buffer = 0;
        public static float[] tablei = new float[1];
        public static float[] tableo = new float[1];
        public static float reback_pos = 0;
        public static int outcount = 0;
        public static int incount = 0;
        public static bool  isblow = false;
        public static int rincount = 0;

        #endregion

        #region 打开连接
        public static bool OpenZm()
        {
            bool r1 = OpenAutoUp();
            bool r2 = OpenHW();
            return r1 && r2;
        }
        #endregion

        #region 打开local1连接
        public static bool OpenHW()
        {
            string Buffer;
            Buffer = "LOCAL1";
            if (g_handle != handle_null)
            {
                zmcaux.ZAux_Close(g_handle);
                g_handle = handle_null;
            }
            int iresult = zmcaux.ZAux_FastOpen(5, Buffer, 1000, out g_handle);//打开控制卡

            if (iresult != 0)
            {
                g_handle = (IntPtr)0;
                LogNet.Error("控制卡打开失败！");
                return false;
            }
            else
            {
                LogNet.Info("控制卡连接成功！");
                //Set_HW_Param();
                //  RUN_HW();
                return true;
            }
        }
        #endregion

        #region 初始化硬件比较参数，执行一次
        public static void Set_HW_Param()
        {
            //int axis = 1;
            POS_OF_HW = new float[IOCount + 8];
            int TIME_OF_HW = 0;
            STATUS_OF_HW = new int[IOCount + 8];//输出口开关
            OP_OF_HW = new int[IOCount + 8];
            Reg_Flag = new int[IOCount * Hw_Reg_Space];
            Reg_Pos = new float[IOCount + 8 * Hw_Reg_Space];
            HW_SKI_COUNT = new int[IOCount + 8];
            HW_RUN_COUNT = new int[IOCount];
            count = new int[IOCount + 8];
            tab_count = new int[IOCount + 8];
            //统一设置输出参数，基础偏移、开关模式、开关时长和0-15输出口
            for (int i = 0; i < IOCount + 8; i++)
            {
                STATUS_OF_HW[i] = 1;
                OP_OF_HW[i] = i;
            }
            //设置HW比较输出定时器,为0-15号输出口（内外排）同时设置
            for (int i = 0; i < 16; i++)
            {
                int ret = 0;
                if (mPlcData != null)
                {
                    if (i < 8)
                    {
                        TIME_OF_HW = mPlcData.NPQ * 1000;
                        //LogNet.Info("内TIME_OF_HW" + TIME_OF_HW);
                    }
                    else
                    {
                        TIME_OF_HW = mPlcData.WPQ * 1000;
                        //LogNet.Info("外TIME_OF_HW" + TIME_OF_HW);
                    }
                }
                else
                {
                    if (i < 8)
                    {
                        TIME_OF_HW = 8000;
                    }
                    else
                    {
                        TIME_OF_HW = 8000;
                    }
                }
                ret = zmcaux.ZAux_Direct_HwTimer(g_handle, 2, TIME_OF_HW + 10, TIME_OF_HW, 1, ~STATUS_OF_HW[i], OP_OF_HW[i]);
                if (ret != 0)
                {
                    ret = zmcaux.ZAux_Direct_HwTimer(g_handle, 2, TIME_OF_HW + 10, TIME_OF_HW, 1, ~STATUS_OF_HW[i], OP_OF_HW[i]);
                    LogNet.Error("设置输出定时器错误！");
                }
            }
            //配置成关闭为到位响应状态的IO口，需要提前关闭，0-15号口通用
            for (int i = 0; i < 16; i++)
            {
                int ret = 0;
                //if (STATUS_OF_HW[i] == 0)
                //{
                ret = zmcaux.ZAux_Direct_SetOp(g_handle, OP_OF_HW[i], 0);
                if (ret != 0)
                {
                    ret = zmcaux.ZAux_Direct_SetOp(g_handle, OP_OF_HW[i], 0);
                    LogNet.Error("关闭输出口错误！");
                }
                //}
                //LogNet.Info("设置IO口关闭:" + i);
            }
            int[] op_num = OP_OF_HW;
            int[] op_status = STATUS_OF_HW;
            //开16个轴来完成，0-15号
            for (int i = 0; i < 16; i++)
            {
                //停止并删除没有完成的点，模式2
                int ret = zmcaux.ZAux_Direct_HwPswitch2(g_handle, i, 2, op_num[i], op_status[i], 0, 0, 1, 0);
                if (ret != 0)
                {
                    ret = zmcaux.ZAux_Direct_HwPswitch2(g_handle, i, 2, op_num[i], op_status[i], 0, 0, 1, 0);
                    LogNet.Error("硬件位置比较清零错误！");
                }
            }
            //设置轴类型，0,1编码器轴            
            zmcaux.ZAux_Direct_SetAtype(g_handle, 1, 6);
        }
        #endregion

        #region 打开中断网口连接        
        public static bool OpenAutoUp()
        {
            try
            {
                cc = new ConnectContext("网口");
                //IP地址
                string Buffer = "127.0.0.1";
                controlReturn = cc.OpenConnect(ref g_handleAutoUp, Buffer);

                //ControllerInit();
                RegisterZAuxCallBack();
                return true;
            }
            catch (NullReferenceException ne)
            {
                LogNet.Warn(string.Format("{0}: {1}", ne.GetType().Name, ne.Message));
                return false;
            }
        }
        #endregion

        #region 注册中断回调函数
        public static void RegisterZAuxCallBack()
        {
            int iresult = 0;
            string cmdbuff;
            StringBuilder cmdbuffAck = new StringBuilder(1024);
            cmdbuff = "AutoCmdString =" + "";                     //生成主动上报命令
            iresult = zmcaux.ZAux_Execute(g_handleAutoUp, cmdbuff, cmdbuffAck, 1024);

            zmcaux.ZAuxCallBack mycall = new zmcaux.ZAuxCallBack(ZmcAutoCallBaceTest);
            gch = GCHandle.Alloc(mycall);
            zmcaux.ZAux_SetAutoUpCallBack(g_handleAutoUp, (zmcaux.ZAuxCallBack)gch.Target);
        }
        #endregion

        #region  剔除参数
        public static plcData mPlcData;
        public static void getPlcData(string panName)
        {
            List<plcData> lst = (new SVPlcData()).getlst();
            switch (panName)
            {
                case "小盘":
                    mPlcData = lst[0];
                    break;
                case "中盘":
                    mPlcData = lst[1];
                    break;
                case "大盘":
                    mPlcData = lst[2];
                    break;
                case "特大盘":
                    mPlcData = lst[3];
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region 中断处理函数
        public static void ZmcAutoCallBaceTest(IntPtr handle, Int32 itypecode, Int32 idatalength, StringBuilder pdata)
        {
            int ret = 0;
            float curLocation = 0;
            float curLocationB = 0;
            float pfValue = 0;
            string pdatastring = pdata.ToString();
            if (pdatastring == "START")
            {
                xop1080.bkstart=true;
            }
            if (pdatastring == "STOP")
            {                
                xop1080.bkstop = true;
            }
            if (pdatastring == "ERR")
            {               
                xop1080.bkstop = true;
            }

            //内外排判断
            if (pdatastring == "IN")
            {
                incount++;
                //zmcaux.ZAux_Direct_Regist(g_handle, 1, 4);
                //内排锁存值
                ret = zmcaux.ZAux_Direct_GetRegPos(g_handle, 1, ref curLocation);
                //curLocation = (float)Math.Round(curLocation / 4);
                int gap = (int)(curLocation - OldMposI);
                if (gap < 14 || gap > 22 && curLocation > 300) // 死区外才处理
                {
                    //BlowStart();
                    int lost = (int)Math.Round((gap - 18) / 18.0);
                    LogNet.Info($"内排漏/误触发 差值={gap}, 次数={lost},清理吹气开");
                    //仅补差1排的，误触发不管，多余的不管
                    if (gap > 32 || gap < 40)
                    {
                    //强制补1排
                    LogNet.Info("内自动纠偏:" + (curLocation - 18));
                    neibuffer.WriteByRow(incount, incount, (long)(curLocation - 18), new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });
                    encMapi.TryAdd((long)(curLocation - 18), new ppenc //编码器字典，后续查字典得位置
                    {
                        enc = (long)(curLocation - 18),
                        row = incount % 200,
                        number = incount,
                    });
                    ZmdataIn((long)(curLocation - 18), incount);
                    incount++;
                }
                    if (lost >= 2 && OldMposI > 300)
                    {
                        xop1080.bkstop = true;
                        LogNet.Error("内排连续丢触发停机");
                    }
                 }
                //写入空排对象
                neibuffer.WriteByRow(incount, incount, (long)curLocation, new byte[] {0,0,0,0,0,0,0,0} );                
                OldMposI = (long)curLocation;
                MposI1 = (long)curLocation;
                encMapi.TryAdd(MposI1, new ppenc //编码器字典，后续查字典得位置
                {
                   enc=MposI1,
                   row=incount%200,
                   number= incount,
                });
                ZmdataIn((long)curLocation,incount);
                ///取车速
                zmcaux.ZAux_Direct_GetMspeed(g_handle, 1, ref pfValue);
                Mspeed = pfValue;
                //if (isblow&&incount > rincount)
                //{
                //    BlowStop();
                //}
            }
            if (pdatastring == "OUT")
            {
                outcount++;
                //设置模式
                //zmcaux.ZAux_Direct_Regist(g_handle, 1, 15);
                //外排锁存值
                ret = zmcaux.ZAux_Direct_GetRegPosB(g_handle, 1, ref curLocationB);
                //curLocationB = (float)Math.Round(curLocationB / 4);
                int gap = (int)(curLocationB - OldMposO);
                if (gap < 12 || gap > 19 && curLocationB > 300) // 死区外才处理
                {
                    //BlowStart();
                    int lost = (int)Math.Round((gap - 16) / 16.0);
                    //outcount += lost;//强制移盘，不写数据
                    LogNet.Info($"外排漏/误触发 差值={gap}, 丢失次数={lost},清理吹气开");
                    if (gap > 28 || gap < 36)
                    {
                        //强制补1排
                        LogNet.Info("外自动纠偏:" + (curLocationB - 16));
                        waibuffer.WriteByRow(outcount, outcount, (long)(curLocationB - 16), new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });
                        encMapo.TryAdd((long)(curLocationB - 16), new ppenc //编码器字典，后续查字典得位置
                        {
                            enc = (long)(curLocationB - 16),
                            row = outcount % 225,
                            number = outcount,
                        });
                        ZmdataOut((long)(curLocationB - 16), outcount);
                        outcount++;
                    }
                    if (lost >= 2 && OldMposO > 300)
                    {
                        xop1080.bkstop = true;
                        LogNet.Error("外排连续丢触发停机");
                    }
                }
                waibuffer.WriteByRow(outcount, outcount, (long)curLocationB, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });                
                OldMposO = (long)curLocationB;
                MposO1 = (long)curLocationB;
                encMapo.TryAdd(MposO1, new ppenc //编码器字典，后续查字典得位置
                {
                    enc = MposO1,
                    row = outcount % 225,
                    number = outcount,
                });
                ZmdataOut((long)curLocationB,outcount);
                //if (outcount%(ExtHandler.GetGlobalVar<int>("吹气圈数") *225)==1)
                //{
                //    //BlowStart();
                //}
            }
            return;
        }
        #endregion

        #region 内排处理逻辑
        public static void ZmdataIn(long Mpos,int incountpp)
        {
            //LogNet.Info("内排计数:" + incount);
            int stepI1 = mPlcData.N1;//内一剔除步数
            int stepI2 = mPlcData.N2;//内二剔除步数
            int stepI3 = mPlcData.N3;//内三剔除步数
            int ret = 0;
            long curLocation = Mpos+ 90; //理论取结果时刻，偏移5排即18*5=90个编码器位置，以防漏数可补，既输出前5排即写好比较数据等转到位置
            int id1 = 0;
            int id2 = 0;
            int id3 = 0;
            bool isnullIout = false;
            bool isnull1=false;
            bool isnull2=false;
            bool isnull3 = false;
            byte[] res1out= { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] res2out = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] res3out = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] statesinout = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };//防止出错;
            #region 结果判断汇总
            //根据位置写数据，未做未读到数据异常处理，包括算法超时无判断
            for (int i = 0;i<8;i++) //±4位置找历史数据
            {
                if(resMap1.TryGetValue(i+Mpos-4-180,out var res1)) //内外-180即找更早的10排以前的位置，即等待算法算10个
                {
                    resMap1.TryRemove(res1.mposi, out res1); //删除结果数据
                    id1 = res1.resid; //得到编号
                    res1out =HalconToByte(res1.Res, out isnull1, out int resincount1); //halcon转普通数组，isnull返回是否全空
                    if(encMapi.TryGetValue(res1.mposi, out var encmapi1)) //根据编码器值查询编码器字典排数
                    {                        
                        neibuffer.WriteBy4Row(encmapi1.row+10, res1out);  //写进查询到排数后10排位置                    
                    }
                    else
                    {
                        LogNet.Error("E001" +"-" + res1.mposi);//编码器表无对应数据
                    }
                }
                if (resMap2.TryGetValue(i + Mpos - 4-180, out var res2))
                {
                    resMap2.TryRemove(res2.mposi, out res2);
                    id2 = res2.resid;
                    res2out =HalconToByte(res2.Res, out isnull2, out int resincount2);
                    if (encMapi.TryGetValue(res2.mposi, out var encmapi2))
                    {                       
                        neibuffer.WriteBy4Row(encmapi2.row - (stepI1 - stepI2) + 10, res2out); // 查询排偏移12相机差                   
                    }
                    else
                    {
                        LogNet.Error("E001" + "-" + res2.mposi);//编码器表无对应数据
                    }
                }               
                if (resMap3.TryGetValue(i + Mpos - 4 - 180, out var res3))
                {
                    resMap3.TryRemove(res3.mposi, out res3);
                    id3 = res3.resid;
                    res3out = HalconToByte(res3.Res, out isnull3, out int resincount3);                    
                    if (encMapi.TryGetValue(res3.mposi, out var encmapi3))
                    {                       
                        neibuffer.WriteBy4Row(encmapi3.row-(stepI1 - stepI3)+10, res3out);    // 查询排偏移13相机差                                      
                    }
                    else
                    {
                        LogNet.Error("E001" + "-" + res3.mposi);//编码器表无对应数据
                    }
                }                          
            }
            if (encMapi.TryGetValue(Mpos, out var encmapi)) //查询当前输出结果的编码器信息
            {
                var (rowin, number, statesin, enc, vison, wid, isnullI) = neibuffer.ReadRowOut(encmapi.row - stepI1); //往后推1相机剔除步数排作为取结果排
                statesinout = statesin;  //取得的结果，数据，长度8
                isnullIout = isnullI; //判断是不是空排
            }
            else
            {
                //LogNet.Info("内排输出错误:" + (encmapi.row - stepI1));
            }
            //有珠子清零
            //if (!isnull1 || !isnull2 || !isnull3)
            if (!isnull2)//13相机暂无空槽检测，判断2号相机先
            {
                passI = 0;
                //isnullIout =true;
            }
            if (AllOne(statesinout) && isnullIout)
            {
                passII = true;
                passI += 1;
            }
            else
            {
                passII = false;
                passI = 0;
            }
            //LogNet.Info("内排空剔累计:" + passI);
            #endregion
            #region 取结果刷输出
            if (Mpos >= (180)) //10排后再处理，防止深度学习算法未拉起来的数据出错
            {  
                byte[] bitArray = new byte[8];
                int[] flag = Reg_Flag;
                float[] pos = Reg_Pos;
                float[] set_pos = POS_OF_HW;
                int[] op_num = OP_OF_HW;
                int[] op_time = TIME_OF_HW;
                int[] op_status = STATUS_OF_HW;
                for (int i = 0; i < 8; i++)
                {                    
                    //设置位置
                    switch (i)
                    {
                        case 0:
                            pos[i] = curLocation + mPlcData.NGQ; //0、对应第一排，
                            break;
                        case 1:
                            pos[i] = curLocation + mPlcData.NGQ - 36; //1,对应第2排，奇偶相差18*2=36
                            break;
                        case 2:
                            pos[i] = curLocation + mPlcData.NGQ;
                            break;
                        case 3:
                            pos[i] = curLocation + mPlcData.NGQ - 36;
                            break;
                        case 4:
                            pos[i] = curLocation + mPlcData.NGQ;
                            break;
                        case 5:
                            pos[i] = curLocation + mPlcData.NGQ - 36;
                            break;
                        case 6:
                            pos[i] = curLocation + mPlcData.NGQ;
                            break;
                        case 7:
                            pos[i] = curLocation + mPlcData.NGQ - 36;
                            break;
                        default:
                            break;

                    }
                    buffer = 0;
                    ///编码器输出位置
                    tablei[0] = 0;
                    reback_pos = 0;
                    //为8个孔单独设置硬件比较输出，清空未完成的比较
                    string temp = "?HW_PSWITCH2(" + i.ToString() + ")"; 
                    StringBuilder builder = new StringBuilder(1024);
                    ret = zmcaux.ZAux_Execute(g_handle, temp, builder, 1024);
                    while (ret != 0)
                    {
                        ret = zmcaux.ZAux_Execute(g_handle, temp, builder, 1024);
                    }
                    //单个孔数据，内外通用
                    tablei[0] = pos[i];
                    ret = zmcaux.ZAux_Direct_SetTable(g_handle, Hw_Reg_Tab_Star + (i * Hw_Reg_tab_space) + tab_count[i], 1, tablei);
                    while (ret != 0)
                    {
                        ret = zmcaux.ZAux_Direct_SetTable(g_handle, Hw_Reg_Tab_Star + (i * Hw_Reg_tab_space) + tab_count[i], 1, tablei);
                    }
                    //对触发位置进行判断，避免位置超过导致后面缓冲阻塞,内外排通用
                    ret = zmcaux.ZAux_Direct_GetMpos(g_handle, 1, ref reback_pos);
                    while (ret != 0)
                    {
                        ret = zmcaux.ZAux_Direct_GetMpos(g_handle, 1, ref reback_pos);
                    }
                    if (reback_pos >= tablei[0])
                    {
                        HW_SKI_COUNT[i]++;
                    LogNet.Error("内排锁存位置超限" + reback_pos + "-" + tablei[0].ToString());
                    }
                    else
                    {

                        //if (bitArray[i] == 1)
                        if(statesinout[i]==1) 
                        {
                            zmcaux.ZAux_Direct_HwPswitch2(g_handle, i, 1, op_num[i], op_status[i], Hw_Reg_Tab_Star + (i * Hw_Reg_tab_space) + tab_count[i], Hw_Reg_Tab_Star + (i * Hw_Reg_tab_space) + tab_count[i], 1, 0);
                            if(!passII)
                            {
                                int tempng = ExtHandler.GetGlobalVar<Int32>("NgCount") + 1;
                                ExtHandler.AddGlobalVar("NgCount", tempng);
                            }                           
                        }
                    }
                    if (count[i] == Hw_Reg_Space - 1)
                    {
                        count[i] = 0;
                    }
                    else
                    {
                        count[i]++;
                    }

                    if (tab_count[i] == Hw_Reg_tab_space - 1)
                    {
                        tab_count[i] = 0;
                    }
                    else
                    {
                        tab_count[i]++;
                    }
                }
        }
            #endregion
        }
        #endregion

        #region 外排处理逻辑
        public static void ZmdataOut(long Mpos, int outcountpp)
        {
            //LogNet.Info("外排计数:" + outcount);
            int stepO1 = mPlcData.W1;//外一剔除步数
            int stepO2 = mPlcData.W2;//外二剔除步数
            int stepO3 = mPlcData.W3;//外三剔除步数
            //LogNet.Info("外1剔除步数：  " + mPlcData.W1);
            int ret = 0;
            float curLocationB = Mpos+ 80;
            int id1=0;
            int id2=0;
            //int id3 = 0;
            bool isnullOout = false;
            bool isnull1 = false;
            bool isnull2 = false;
            bool isnull3 = false;
            byte[] res1out = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] res2out = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] res3out = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] statesoutout = { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};//防止出错
            //MposO1 = Mpos;
            #region 结果汇总处理
            //根据位置写数据，未做未读到数据异常处理，包括算法超时无判断
            for (int i = 0; i < 8; i++)
            {
                if (resMap4.TryGetValue(i + Mpos - 4 - 160, out var res1))
                {
                    resMap4.TryRemove(res1.mposo, out res1);
                    id1 = res1.resid;
                    res1out = HalconToByte(res1.Res, out isnull1, out int resincount1);
                    if (encMapo.TryGetValue(res1.mposo, out var encmapo1))
                    {                        
                        waibuffer.WriteBy4Row(encmapo1.row + 10, res1out);                      
                    }
                    else
                    {
                        LogNet.Error("E001" + res1.mposo);
                    }
                } 
                if (resMap5.TryGetValue(i + Mpos - 4 - 160, out var res2))
                {
                    resMap5.TryRemove(res2.mposo, out res2);
                    id2 = res2.resid;
                    res2out = HalconToByte(res2.Res, out isnull2, out int resincount2);
                    if (encMapo.TryGetValue(res2.mposo, out var encmapo2))
                    {
                        waibuffer.WriteBy4Row(encmapo2.row - (stepO1 - stepO2) + 10, res2out);
                    }
                    else
                    {
                        LogNet.Error("E001" + res2.mposo);
                    }
                }               
                if (resMap6.TryGetValue(i + Mpos - 4 - 160, out var res3))
                {
                    resMap6.TryRemove(res3.mposo, out res3);
                    id1 = res3.resid;
                    res3out = HalconToByte(res3.Res, out isnull3, out int resincount3);
                    if (encMapo.TryGetValue(res3.mposo, out var encmapo3))
                    {
                        waibuffer.WriteBy4Row(encmapo3.row - (stepO1 - stepO3) + 10, res3out);                       
                    }
                    else
                    {
                        LogNet.Error("E001" + res3.mposo);
                    }
                }                
            }
            if (encMapo.TryGetValue(Mpos, out var encmapo))
            {
                var (rowin, number, statesin, enc, vison, wid, isnullI) = waibuffer.ReadRowOut(encmapo.row - stepO1);
                statesoutout = statesin;
                isnullOout = isnullI;              
            }
            else
            {
                LogNet.Info("外排输出错误:" + (encmapo.row - stepO1));
            }
            //有珠子清零
            //if (!isnull1 || !isnull2 || !isnull3)
            if (!isnull2)//13相机暂无空槽检测，判断2号相机先
            {
                passO = 0;
                //isnullIout =true;
            }
            if (AllOne(statesoutout) && isnullOout)
            {
                passOO = true;
                passO += 1;
            }
            else
            {
                passOO = false;
                passO = 0;
            }
            //LogNet.Info("内排空剔累计:" + passI);
            #endregion
            #region 结果刷新
            if (Mpos >= (160))
            {
                //int res = 0;               
                int[] flag = Reg_Flag;
                float[] pos = Reg_Pos;
                float[] set_pos = POS_OF_HW;
                int[] op_num = OP_OF_HW;
                int[] op_time = TIME_OF_HW;
                int[] op_status = STATUS_OF_HW;
                for (int i = 8; i < 16; i++)
                {
                    //设置位置
                    switch (i)
                    {
                        case 8:
                            pos[i] = curLocationB + mPlcData.WGQ;
                            break;
                        case 9:
                            pos[i] = curLocationB + mPlcData.WGQ - 32;
                            break;
                        case 10:
                            pos[i] = curLocationB + mPlcData.WGQ;
                            break;
                        case 11:
                            pos[i] = curLocationB + mPlcData.WGQ - 32;
                            break;
                        case 12:
                            pos[i] = curLocationB + mPlcData.WGQ;
                            break;
                        case 13:
                            pos[i] = curLocationB + mPlcData.WGQ - 32;
                            break;
                        case 14:
                            pos[i] = curLocationB + mPlcData.WGQ;
                            break;
                        case 15:
                            pos[i] = curLocationB + mPlcData.WGQ - 32;
                            break;
                        default:
                            break;
                    }
                    //LogNet.Info("外排偏移量：  " + mPlcData.WGQ);
                    buffer = 0;
                    ///编码器输出位置
                    tableo[0] = 0;
                    reback_pos = 0;
                    //为8个孔单独设置硬件比较输出
                    string temp = "?HW_PSWITCH2(" + i.ToString() + ")";
                    StringBuilder builder = new StringBuilder(1024);
                    ret = zmcaux.ZAux_Execute(g_handle, temp, builder, 1024);
                    while (ret != 0)
                    {
                        ret = zmcaux.ZAux_Execute(g_handle, temp, builder, 1024);
                    }
                    //单个孔数据，内外通用
                    tableo[0] = pos[i];
                    ret = zmcaux.ZAux_Direct_SetTable(g_handle, Hw_Reg_Tab_Star + (i * Hw_Reg_tab_space) + tab_count[i], 1, tableo);
                    while (ret != 0)
                    {
                        ret = zmcaux.ZAux_Direct_SetTable(g_handle, Hw_Reg_Tab_Star + (i * Hw_Reg_tab_space) + tab_count[i], 1, tableo);
                    }
                    //对触发位置进行判断，避免位置超过导致后面缓冲阻塞,内外排通用
                    ret = zmcaux.ZAux_Direct_GetMpos(g_handle, 1, ref reback_pos);
                    while (ret != 0)
                    {
                        ret = zmcaux.ZAux_Direct_GetMpos(g_handle, 1, ref reback_pos);
                    }
                    if (reback_pos >= tableo[0])
                    {
                        HW_SKI_COUNT[i]++;
                        LogNet.Error("外排锁存位置超限" + reback_pos + "-" + tablei[0].ToString());
                    }
                    else
                    {                       
                            if (statesoutout[i-8] == 1)
                            {
                            zmcaux.ZAux_Direct_HwPswitch2(g_handle, i, 1, op_num[i], op_status[i], Hw_Reg_Tab_Star + (i * Hw_Reg_tab_space) + tab_count[i], Hw_Reg_Tab_Star + (i * Hw_Reg_tab_space) + tab_count[i], 1, 0);
                            if(!passOO)
                            {
                                int tempng = ExtHandler.GetGlobalVar<Int32>("NgCount") + 1;
                                ExtHandler.AddGlobalVar("NgCount", tempng);
                            }                           
                        }
                    }
                    if (count[i] == Hw_Reg_Space - 1)
                    {
                        count[i] = 0;
                    }
                    else
                    {
                        count[i]++;
                    }

                    if (tab_count[i] == Hw_Reg_tab_space - 1)
                    {
                        tab_count[i] = 0;
                    }
                    else
                    {
                        tab_count[i]++;
                    }
                }
            }
            #endregion
        }
        #endregion

        #region 关闭控制卡连接
        public static bool CloseZm()
        {
            CloseAutoUp();
            return true;
        }
        public static bool CloseAutoUp()
        {
            if (g_handleAutoUp == (IntPtr)0)
                return false;
            cc?.CloseConnect(g_handleAutoUp);
            g_handleAutoUp = (IntPtr)0;
            return true;
        }
        #endregion
        #endregion
        #region 数据处理
        public static void ProcessCamera(int cameraIndex)
        {
            int step = 0;
            int curOrder = 0;
            while (!IsShuttingDowncame)
            {
                try
                {
                    Thread.Sleep(1);
                    switch (step)
                    {
                        case 0:
                            if (ToneProcesses[cameraIndex].ResQueuqe.TryDequeue(out curOrder))
                            {
                                step = 1;
                            }
                            break;
                        case 1:
                            if (ToneProcesses[cameraIndex].ProductMap.TryGetValue(curOrder, out var pp) &&
                                pp != null && pp.Complete)
                            {
                                if (ToneProcesses[cameraIndex].ProductMap.TryRemove(curOrder, out var p))
                                { 
                                    //写结果表
                                    switch (cameraIndex)
                                    {
                                        //内1
                                        case 0:
                                            {
                                                resMap1.TryAdd(p.mposi, new res//结果入字典
                                                {
                                                    resid= p.Order,
                                                    Res = p.Res,
                                                    userTime=p.useTime,
                                                    mposi= p.mposi,
                                                });
                                                //LogNet.Info("存入:"+p.mposi);
                                            }
                                            break;
                                        //内2    
                                        case 1:
                                            {                                          
                                                resMap2.TryAdd(p.mposi, new res//结果入字典
                                                {
                                                    resid = p.Order,
                                                    Res = p.Res,
                                                    userTime = p.useTime,
                                                    mposi= p.mposi,
                                                });                                                
                                            }
                                            break;
                                        //内3
                                        case 2:
                                            {                                                
                                                resMap3.TryAdd(p.mposi, new res//结果入字典
                                                {
                                                    resid = p.Order,
                                                    Res = p.Res,
                                                    userTime = p.useTime,
                                                    mposi= p.mposi,
                                                });
                                            }
                                            break;
                                        //外1
                                        case 3:
                                            {                                                
                                                resMap4.TryAdd(p.mposo, new res//结果入字典
                                                {
                                                    resid = p.Order,
                                                    Res = p.Res,
                                                    userTime = p.useTime,
                                                    mposo= p.mposo,
                                                });
                                            }
                                            break;
                                        //外2
                                        case 4:
                                            {                                                                                               
                                                resMap5.TryAdd(p.mposo, new res //结果入字典
                                                {
                                                    resid = p.Order,
                                                    Res = p.Res,
                                                    userTime = p.useTime,
                                                    mposo= p.mposo,
                                                });
                                            }
                                            break;
                                        //外3
                                        case 5:
                                            {                                                
                                                resMap6.TryAdd(p.mposo, new res //结果入字典
                                                {
                                                    resid = curOrder,
                                                    Res = p.Res,
                                                    userTime = p.useTime,
                                                    mposo= p.mposo,
                                                });
                                            }
                                            break;
                                    }
                                    step = 0;
                                }
                            }
                            break;
                    }

                }
                catch (ThreadInterruptedException)
                {
                    LogNet.Info("ProcessCamera 线程被中断，准备退出");
                    IsShuttingDowncame = true; // 收到中断后设置退出标志
                }

            }
        }
        public static byte[] HalconToByte(HTuple bytes,out bool Empty, out int Count)
        {
            const int LEN = 32;
            byte[] states = new byte[LEN];
            int emptyCount = 0;            // 连续全空计数器
            bool allEmpty = true;          // 本轮是否全空标志
            if (bytes == null || bytes.Length == 0)
            {
                Count = emptyCount;
                Empty = allEmpty;
                return states;   // 已初始化为全 0
            }
            // 第一轮：看是不是全空
            for (int i = 0; i < LEN; i++)
            {
                int v = (i < bytes.Length) ? bytes[i].I : 0;
                if (v != 2)                       // 只要有一个不是2 → 非全空
                {
                    allEmpty = false;
                    break;
                }
            }
            // 第二轮：按结果填充
            if (allEmpty && ExtHandler.GetGlobalVar<string>("空盘清理") == "true")
            {
                emptyCount++;                     // 连续全空计数+1
                for (int i = 0; i < LEN; i++)
                    states[i] = 1;                // 全1输出
                //BlowStart();
            }
            else
            {
                emptyCount = 0;                   // 一旦不连续全空，计数清零
                for (int i = 0; i < LEN; i++)
                {
                    int v = (i < bytes.Length) ? bytes[i].I : 0;
                    states[i] = (v == 1) ? (byte)1 : (byte)0; // 坏=1，空/好=0
                }
            }
            Count = emptyCount;
            Empty = allEmpty;
            return states;
        }

        static bool AllOne(byte[] a)
        {
            foreach (byte b in a)
                if (b != 1) return false;
            return true;
        }
        #endregion
        #region 销毁对象
        public static void Destroy()
        {
            try
            {
                //清理编码器计数值
                zmcaux.ZAux_Direct_SetMpos(g_handle, 1, 0);
                zmcaux.ZAux_Direct_SetDpos(g_handle, 1, 0);
                zmcaux.ZAux_Direct_SetMpos(g_handle, 0, 0);
                zmcaux.ZAux_Direct_SetDpos(g_handle, 0, 0);
                foreach (var p in ToneProcesses)
                {
                    p.Destroy();
                }
                //关闭控制卡连接
                CloseZm();
                LogNet.Info("硬件服务安全退出。");
            }
            catch (Exception ex)
            {
                LogNet.Error($"硬件资源清理失败: {ex.ToString()}");
            }
        }
        #endregion

        #region 盘面清理-台湾
        /// <summary>
        /// 触发孔吹气
        /// </summary>
        //public static void BlowStart()
        //{
        //    int rettemp = zmcaux.ZAux_Direct_SetOp(g_handle, 29, 1);
        //    rincount = incount + ExtHandler.GetGlobalVar<int>("吹气排数");
        //    isblow = true;
        //}
        ///// <summary>
        ///// 触发孔关气
        ///// </summary>
        //public static void BlowStop()
        //{
        //    int rettemp = zmcaux.ZAux_Direct_SetOp(g_handle, 29, 0);
        //    isblow = false;
        //}
        #endregion

    }
}