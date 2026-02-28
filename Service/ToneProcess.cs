using HalconDotNet;
using ImageControl;
using LittleCommon.Domain;
using LittleCommon.Tool;
using MvCamCtrl.NET;
using MvCameraControl;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionCore.Core;
using VisionCore.Ext;
using VisionCore.Log;
using WENYU_EIO32P;
using RobotLocation.Model;

namespace RobotLocation.Service
{
    public class ToneProcess
    {
        public Thread RunTH;
        //1处理队列
        public ConcurrentQueue<int> ProcessQueuqe = new ConcurrentQueue<int>();
        public ConcurrentDictionary<int, Product> ProductMap = new ConcurrentDictionary<int, Product>();
        public ConcurrentQueue<int> ResQueuqe = new ConcurrentQueue<int>();
        public ConcurrentQueue<float> RQ = new ConcurrentQueue<float>(); // 编码器队列
        public string Name;
        public int curOrder = 0;
        public int ImgNGCount;
        public string NGDir;     
        #region  相机
        private MyCamera m_pMyCamera;
        private MyCamera.cbOutputExdelegate ImageCallback;
        public MyCamera.MV_CC_DEVICE_INFO CurDevice;
        public MyCamera.cbExceptiondelegate pCallBackFunc;//异常掉线回调
        public bool Connected = false;
        public uint lastTime = 0;//上一次的时间戳
        public uint useTime = 0;//两次拍照耗时

        // 添加原始数据队列
        private ConcurrentQueue<RawImageData> _rawImageQueue = new ConcurrentQueue<RawImageData>();
        // ch:用于保存图像的缓存 | en:Buffer for saving image
        //private UInt32 m_nBufSizeForSaveImage = 5120 * 5120 * 3 + 2048;
        //private byte[] m_pBufForSaveImage = new byte[5120 * 5120 * 3 + 2048];
        //查找相机

        private static readonly double[] _buf = new double[100];
        public int ppid = 0;
        public bool FindDev()
        {

            MyCamera.MV_CC_DEVICE_INFO_LIST m_pDeviceList = new MyCamera.MV_CC_DEVICE_INFO_LIST();
            int nRet;
            // ch:创建设备列表 en:Create Device List
            System.GC.Collect();
            nRet = MyCamera.MV_CC_EnumDevices_NET(MyCamera.MV_GIGE_DEVICE | MyCamera.MV_USB_DEVICE, ref m_pDeviceList);
            if (0 != nRet)
            {
                LogNet.Error("该设备没有找到任何海康相机,请确认相机是否连接好");
            }

            // ch:在窗体列表中显示设备名 | en:Display device name in the form list
            for (int i = 0; i < m_pDeviceList.nDeviceNum; i++)
            {
                MyCamera.MV_CC_DEVICE_INFO device = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_pDeviceList.pDeviceInfo[i], typeof(MyCamera.MV_CC_DEVICE_INFO));
                if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
                {
                    IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(device.SpecialInfo.stGigEInfo, 0);
                    MyCamera.MV_GIGE_DEVICE_INFO gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_GIGE_DEVICE_INFO));
                    if (Name == gigeInfo.chUserDefinedName)
                    {
                        CurDevice = device;
                        return true;
                    }
                }
                else if (device.nTLayerType == MyCamera.MV_USB_DEVICE)
                {
                    IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(device.SpecialInfo.stUsb3VInfo, 0);
                    MyCamera.MV_USB3_DEVICE_INFO usbInfo = (MyCamera.MV_USB3_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_USB3_DEVICE_INFO));

                    if (Name == usbInfo.chUserDefinedName)
                    {
                        CurDevice = device;
                        return true;
                    }

                }
                //LogNet.Info($"FindDev 找到设备，LayerType = {(int)CurDevice.nTLayerType}");//by崔
               
            }
            //LogNet.Error(string.Format("该设备没有找到当前为 {0} 的海康相机,请确认相机是否连接好", Name));
            return false;
        }
        //设置触发模式
        public bool SetTriggerMode(string mode)
        {
            int nRet = 0;
            nRet = m_pMyCamera.MV_CC_SetEnumValue_NET("TriggerMode", 1);
            switch (mode)
            {
                case "软触发":
                    nRet += m_pMyCamera.MV_CC_SetEnumValue_NET("TriggerSource", 7);

                    break;
                case "硬触发":
                    nRet += m_pMyCamera.MV_CC_SetEnumValue_NET("TriggerSource", 0);
                    //   nRet += m_pMyCamera.MV_CC_SetEnumValue_NET("TriggerActivation", 0);

                    break;
            }
            if (nRet != MyCamera.MV_OK)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        public bool SendTriggerSoftware()
        {
            try
            {
                int nRet = 0;
                // ch:触发命令 | en:Trigger command
                nRet = m_pMyCamera.MV_CC_SetCommandValue_NET("TriggerSoftware");
                if (MyCamera.MV_OK != nRet)
                {
                    LogNet.Error("发送软触发失败:");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                LogNet.Error("发送软触发失败:" + ex.Message);
                return false;
            }
        }
        //设置曝光和增益
        public bool SetVal(float ExposeTime, int Gain)
        {
            try
            {
                m_pMyCamera.MV_CC_SetEnumValue_NET("ExposureAuto", 0);

                int nRet = m_pMyCamera.MV_CC_SetFloatValue_NET("ExposureTime", ExposeTime);
                if (nRet != MyCamera.MV_OK)
                    return false;

                m_pMyCamera.MV_CC_SetEnumValue_NET("GainAuto", 0);

                nRet = m_pMyCamera.MV_CC_SetFloatValue_NET("Gain", Gain);
                if (nRet != MyCamera.MV_OK)
                    return false;
            }
            catch (Exception ex)
            {
                LogNet.Error("设置曝光和增益失败:" + ex.Message);
            }
            return true;
        }
        //连接相机
        public bool ConnectDev()
        {
            try
            {
                // 如果设备已经连接先断开
                DisConnectDev();
                int nRet = -1;
                // ch:打开设备 | en:Open device
                if (null == m_pMyCamera)
                {
                    m_pMyCamera = new MyCamera();
                    if (null == m_pMyCamera)
                    {
                        return false;
                    }
                }
                nRet = m_pMyCamera.MV_CC_CreateDevice_NET(ref CurDevice);
                if (MyCamera.MV_OK != nRet)
                {
                    return false;
                }
                nRet = m_pMyCamera.MV_CC_OpenDevice_NET();
                if (MyCamera.MV_OK != nRet)
                {
                    m_pMyCamera.MV_CC_DestroyDevice_NET();
                    return false;
                }
                // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
                if (CurDevice.nTLayerType == MyCamera.MV_GIGE_DEVICE)
                {
                    int nPacketSize = m_pMyCamera.MV_CC_GetOptimalPacketSize_NET();
                    if (nPacketSize > 0)
                    {
                        nRet = m_pMyCamera.MV_CC_SetIntValue_NET("GevSCPSPacketSize", (uint)nPacketSize);
                        if (nRet != MyCamera.MV_OK)
                        {
                            LogNet.Error("设置数据包大小失败！");
                        }
                    }
                    else
                    {
                        LogNet.Error("获取数据包大小失败！");
                    }
                }
                // ch:设置采集连续模式 | en:Set Continues Aquisition Mode
                //m_pMyCamera.MV_CC_SetEnumValue_NET("AcquisitionMode", 2);// ch:工作在连续模式 | en:Acquisition On Continuous Mode
                // ch:注册回调函数 | en:Register image callback
                ImageCallback = new MyCamera.cbOutputExdelegate(ImageCallbackFunc);
                nRet = m_pMyCamera.MV_CC_RegisterImageCallBackEx_NET(ImageCallback, IntPtr.Zero);
                if (MyCamera.MV_OK != nRet)
                {
                    LogNet.Error("注册相机回调函数失败");
                }
                // ch:开启抓图 || en: start grab image
                nRet = m_pMyCamera.MV_CC_StartGrabbing_NET();
                if (MyCamera.MV_OK != nRet)
                {
                    Connected = false;
                }
                else
                {
                    Connected = true;
                }
            }
            catch (Exception)
            {
                Connected = false;
            }
            return Connected;
        }
        //断开相机
        public void DisConnectDev()
        {
            try
            {
                if (Connected)
                {
                    int nRet;

                    nRet = m_pMyCamera.MV_CC_CloseDevice_NET();
                    if (MyCamera.MV_OK != nRet)
                    {
                        return;                        
                    }

                    nRet = m_pMyCamera.MV_CC_DestroyDevice_NET();
                    if (MyCamera.MV_OK != nRet)
                    {
                        return;
                    }
                    Connected = false;
                }
            }
            catch (Exception ex)
            {
                LogNet.Error("相机断开失败:" + ex.Message);
            }

        }
        /// <summary>
        /// 相机回调函数 - 优化版（只做数据拷贝，快速返回）
        /// </summary>
        private void ImageCallbackFunc(IntPtr pData, ref MyCamera.MV_FRAME_OUT_INFO_EX pFrameInfo, IntPtr pUser)
        {
            try
            {
                // 获取编码器位置（快速操作）
                var tempmposi = ToneOp.MposI1;
                var tempmposo = ToneOp.MposO1;

                // 计算图像数据大小
                int dataSize = CalculateDataSize(pFrameInfo);
                if (dataSize <= 0) return;

                // 快速拷贝原始数据
                byte[] rawData = new byte[dataSize];
                Marshal.Copy(pData, rawData, 0, dataSize);

                // 计算时间间隔
                uint useTime = 0;
                ulong time = (ulong)pFrameInfo.nDevTimeStampLow + ((ulong)pFrameInfo.nDevTimeStampHigh << 32);
                ulong ppTimeStamp = time / 100000;
                if (lastTime != 0)
                {
                    useTime = (uint)ppTimeStamp - lastTime;
                }
                lastTime = (uint)ppTimeStamp;

                // 快速入队原始数据
                if (pFrameInfo.nFrameNum > 0)
                {
                    _rawImageQueue.Enqueue(new RawImageData
                    {
                        Data = rawData,
                        Width = (int)pFrameInfo.nWidth,
                        Height = (int)pFrameInfo.nHeight,
                        PixelType = pFrameInfo.enPixelType,
                        FrameNum = pFrameInfo.nFrameNum,
                        TimeStamp = (uint)ppTimeStamp,
                        MposI = tempmposi,
                        MposO = tempmposo,
                        UseTime = useTime
                    });
                }
            }
            catch (Exception ex)
            {
                LogNet.Error($"获取图像失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 计算图像数据大小
        /// </summary>
        private int CalculateDataSize(MyCamera.MV_FRAME_OUT_INFO_EX pFrameInfo)
        {
            int pixelSize = 1;
            switch (pFrameInfo.enPixelType)
            {
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG8:
                    pixelSize = 1;
                    break;
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BGR8_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed:
                    pixelSize = 3;
                    break;
                default:
                    return -1;
            }
            return (int)pFrameInfo.nWidth * (int)pFrameInfo.nHeight * pixelSize;
        }

        #endregion
        public void Exe(HObject Image)
        {
            // 写全集变量，执行流程
            ExtHandler.AddGlobalVar<HObject>(Name + "图像", Image);
            //运行流程
            ExtHandler.ExeProj(Name);            
        }
        public void Exe(ref Product p)
        {
            Exe(p.Img);
            //取结果。
            p.Res = ExtHandler.GetGlobalVar<HTuple>(Name + "结果");
            p.Complete = true;
        }
        public List<string> LoadNg()
        {

            List<string> ngs = new List<string>();
            if (Directory.Exists(NGDir))
            {
                return Directory.GetFiles(NGDir).ToList();
            }
            return ngs;
        }
        public void DelNg()
        {
            if (Directory.Exists(NGDir))
                Directory.Delete(NGDir, true);
        }
        public bool Init()
        {
            
            //防止重复拉起
            if (IsThreadRunning())
            {   
                return true;
            }
            else
            {
                Result result = new Result();
                //step1:查询相机
                bool r0 = FindDev();
                if (!r0)
                {
                    LogNet.Error("相机:" + Name + "找不到");
                    return false;
                }
                //step2：连接相机
                bool r = ConnectDev();
                if (r)
                {
                    LogNet.Info("相机:" + Name + "连接成功");
                }
                else
                {
                    LogNet.Error("相机:" + Name + "连接失败");
                    return false;
                }
                runpj();
            }            
            return true;            
        }

        //不在初始化直接运行解决方案，由启动按钮运行
        /// <summary>
        /// 启动处理线程 - 优化版
        /// </summary>
        public void runpj()
        {
            RunTH = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        Thread.Sleep(1);

                        // 从原始数据队列获取数据
                        if (_rawImageQueue.TryDequeue(out RawImageData rawData))
                        {
                            // 在后台线程进行格式转换（不影响相机回调）
                            HImage image = ConvertToHImage(rawData);
                            if (image == null)
                            {
                                rawData.Dispose();
                                continue;
                            }

                            // 创建Product并处理
                            curOrder++;
                            var product = new Product
                            {
                                Order = curOrder,
                                Img = image,
                                useTime = rawData.UseTime,
                                mposi = rawData.MposI,
                                mposo = rawData.MposO
                            };

                            try
                            {
                                // 执行视觉处理
                                Exe(ref product);
                            }
                            finally
                            {
                                // 释放资源
                                product.Dispose();
                                rawData.Dispose();
                            }
                        }
                    }
                    catch (ThreadInterruptedException)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                        LogNet.Error($"处理图像异常: {ex.Message}");
                    }
                }
            });
            RunTH.IsBackground = true;
            //RunTH.Priority = ThreadPriority.Highest;
            RunTH.Start();
        }

        /// <summary>
        /// 将原始数据转换为HImage（在后台线程执行）
        /// </summary>
        private HImage ConvertToHImage(RawImageData rawData)
        {
            HImage image = new HImage();

            // 使用GCHandle固定内存
            GCHandle handle = GCHandle.Alloc(rawData.Data, GCHandleType.Pinned);
            try
            {
                IntPtr pData = handle.AddrOfPinnedObject();

                switch (rawData.PixelType)
                {
                    case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8:
                        image.GenImage1("byte", rawData.Width, rawData.Height, pData);
                        break;

                    case MyCamera.MvGvspPixelType.PixelType_Gvsp_BGR8_Packed:
                        image.GenImageInterleaved(pData, "bgr", rawData.Width, rawData.Height,
                            -1, "byte", rawData.Width, rawData.Height, 0, 0, -1, 0);
                        break;

                    case MyCamera.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed:
                        image.GenImageInterleaved(pData, "rgb", rawData.Width, rawData.Height,
                            -1, "byte", rawData.Width, rawData.Height, 0, 0, -1, 0);
                        break;

                    case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG8:
                        using (HImage tempImage = new HImage())
                        {
                            tempImage.GenImage1("byte", rawData.Width, rawData.Height, pData);
                            image = tempImage.CfaToRgb("bayer_bg", "bilinear");
                        }
                        break;

                    case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB8:
                        using (HImage tempImage = new HImage())
                        {
                            tempImage.GenImage1("byte", rawData.Width, rawData.Height, pData);
                            image = tempImage.CfaToRgb("bayer_gb", "bilinear");
                        }
                        break;

                    case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR8:
                        using (HImage tempImage = new HImage())
                        {
                            tempImage.GenImage1("byte", rawData.Width, rawData.Height, pData);
                            image = tempImage.CfaToRgb("bayer_gr", "bilinear");
                        }
                        break;

                    case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG8:
                        using (HImage tempImage = new HImage())
                        {
                            tempImage.GenImage1("byte", rawData.Width, rawData.Height, pData);
                            image = tempImage.CfaToRgb("bayer_rg", "bilinear");
                        }
                        break;

                    default:
                        LogNet.Error($"不支持的像素格式: {rawData.PixelType}");
                        return null;
                }

                return image;
            }
            finally
            {
                handle.Free();
            }
        }


        // 判断线程是否正在运行
        public bool IsThreadRunning()
        {
            return RunTH != null && RunTH.IsAlive;
        }

        /// <summary>
        /// 销毁资源
        /// </summary>
        public void Destroy()
        {
            // 停止处理线程
            if (RunTH != null && RunTH.IsAlive)
            {
                RunTH.Interrupt();
                RunTH.Join(1000);  // 等待最多1秒
                RunTH = null;
            }

            // 清空队列并释放资源
            while (ProcessQueuqe.TryDequeue(out int order))
            {
                if (ProductMap.TryRemove(order, out Product p))
                {
                    p?.Dispose();
                }
            }
            ProductMap.Clear();

            // 清空原始数据队列
            while (_rawImageQueue.TryDequeue(out RawImageData rawData))
            {
                rawData?.Dispose();
            }

            // 断开相机
            DisConnectDev();
        }

        MyCamera.MV_CC_DEVICE_INFO m_pDeviceInfo;
        /// <summary>
        /// 官方丢帧检测demo
        /// </summary>
        private string GetLostFrame()
        {
            LogNet.Info($"LayerType raw value = {(int)m_pDeviceInfo.nTLayerType}");
            if (m_pDeviceInfo.nTLayerType == MyCamera.MV_GIGE_DEVICE)
            {
                // 1. 先分配一块 **空内存** 给 SDK 填写
                int size = Marshal.SizeOf(typeof(MyCamera.MV_MATCH_INFO_NET_DETECT));
                IntPtr pInfo = Marshal.AllocHGlobal(size);

                // 2. 填描述头
                MyCamera.MV_ALL_MATCH_INFO pstInfo = new MyCamera.MV_ALL_MATCH_INFO
                {
                    nType = MyCamera.MV_MATCH_TYPE_NET_DETECT,
                    nInfoSize = (uint)size,
                    pInfo = pInfo
                };

                // 3. **让 SDK 把数据写进 pInfo**
                int ret = m_pMyCamera.MV_CC_GetAllMatchInfo_NET(ref pstInfo);
                if (ret != MyCamera.MV_OK)
                {
                    Marshal.FreeHGlobal(pInfo);
                    return "001";
                }

                // 4. **把填好的内存读成结构体**
                var netInfo = (MyCamera.MV_MATCH_INFO_NET_DETECT)Marshal.PtrToStructure(pInfo, typeof(MyCamera.MV_MATCH_INFO_NET_DETECT));

                Marshal.FreeHGlobal(pInfo);
                return netInfo.nLostFrameCount.ToString();
            }
            else if (m_pDeviceInfo.nTLayerType == MyCamera.MV_USB_DEVICE)
            {
                /* 同上面流程，结构体换成 MV_MATCH_INFO_USB_DETECT，取 nErrorFrameCount 即可 */
                int size = Marshal.SizeOf(typeof(MyCamera.MV_MATCH_INFO_USB_DETECT));
                IntPtr pInfo = Marshal.AllocHGlobal(size);

                MyCamera.MV_ALL_MATCH_INFO pstInfo = new MyCamera.MV_ALL_MATCH_INFO
                {
                    nType = MyCamera.MV_MATCH_TYPE_USB_DETECT,
                    nInfoSize = (uint)size,
                    pInfo = pInfo
                };

                int ret = m_pMyCamera.MV_CC_GetAllMatchInfo_NET(ref pstInfo);
                if (ret != MyCamera.MV_OK)
                {
                    Marshal.FreeHGlobal(pInfo);
                    return "002";
                }

                var usbInfo = (MyCamera.MV_MATCH_INFO_USB_DETECT)Marshal.PtrToStructure(pInfo, typeof(MyCamera.MV_MATCH_INFO_USB_DETECT));
                Marshal.FreeHGlobal(pInfo);
                return usbInfo.nErrorFrameCount.ToString();
            }
            return "003";
        }
        private uint _lastGigeLost = 0;   // GigE 上一次累计
        private uint _lastUsbLost = 0;   // USB 上一次累计
        /// <summary>
        /// 返回：本次回调相比上一次丢了多少帧（GigE/USB 自动分支）
        /// </summary>
        private uint GetLostIncr()
        {
            uint now = 0;
            if (CurDevice.nTLayerType == MyCamera.MV_GIGE_DEVICE)
                now = uint.Parse(GetLostFrame());   // 你的原函数返回字符串
            else if (CurDevice.nTLayerType == MyCamera.MV_USB_DEVICE)
                now = uint.Parse(GetLostFrame());   // USB 返回 nErrorFrameCount
            else
                return 0;
            ref uint last = ref (CurDevice.nTLayerType == MyCamera.MV_GIGE_DEVICE ? ref _lastGigeLost : ref _lastUsbLost);
            uint incr = (now > last) ? now - last : 0;
            last = now;
            return incr;
        }

        /// <summary>
        /// 原始图像数据结构
        /// </summary>
        public class RawImageData : IDisposable
        {
            public byte[] Data;
            public int Width;
            public int Height;
            public MyCamera.MvGvspPixelType PixelType;
            public uint FrameNum;
            public uint TimeStamp;
            public long MposI;
            public long MposO;
            public uint UseTime;

            public void Dispose()
            {
                Data = null;
            }
        }
    }

}
