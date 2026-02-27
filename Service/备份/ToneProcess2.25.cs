using HalconDotNet;
using LittleCommon.Domain;
using LittleCommon.Tool;
using MvCamCtrl.NET;
using RobotLocation.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VisionCore.Core;
using VisionCore.Ext;
using VisionCore.Log;
using WENYUPCIE;

namespace RobotLocation.Service
{
    public class ToneProcess
    {
        public Thread RunTH;
        //1处理队列
        public ConcurrentQueue<int> ProcessQueuqe = new ConcurrentQueue<int>();
        /// <summary>
        /// 
        /// </summary>
        public ConcurrentDictionary<int,Product> ProductMap=new ConcurrentDictionary<int,Product>();

        /// <summary>
        /// 结果队列
        /// </summary>
        /// 
        public ConcurrentQueue<int> ResQueuqe = new ConcurrentQueue<int>();

        public int curOrder = 0;
        #region  相机

        private MyCamera m_pMyCamera;
        private MyCamera.cbOutputExdelegate ImageCallback;
        public MyCamera.MV_CC_DEVICE_INFO CurDevice;
        MyCamera.cbExceptiondelegate pCallBackFunc;//异常掉线回调
       
        public bool Connected = false;
        public bool FindDev()
        {

            MyCamera.MV_CC_DEVICE_INFO_LIST m_pDeviceList = new MyCamera.MV_CC_DEVICE_INFO_LIST();
            int nRet;
            // ch:创建设备列表 en:Create Device List
            System.GC.Collect();
            nRet = MyCamera.MV_CC_EnumDevices_NET(MyCamera.MV_GIGE_DEVICE | MyCamera.MV_USB_DEVICE, ref m_pDeviceList);
            if (0 != nRet)
            {
                LogNet.Warn("该设备没有找到任何海康相机,请确认相机是否连接好");
                return false;
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
                        return false;
                    }

                }
            }
            LogNet.Warn(string.Format("该设备没有找到当前为 {0} 的海康相机,请确认相机是否连接好", Name));
            return false;


        }
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
        private void ImageCallbackFunc(IntPtr pData, ref MyCamera.MV_FRAME_OUT_INFO_EX pFrameInfo, IntPtr pUser)
        {
            try
            {
                HImage Image = new HImage();
                if (pFrameInfo.enPixelType == MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8)
                {
                    Image.GenImage1("byte", pFrameInfo.nWidth, pFrameInfo.nHeight, pData);
                }
                else if (pFrameInfo.enPixelType == MyCamera.MvGvspPixelType.PixelType_Gvsp_BGR8_Packed)
                {
                    Image.GenImageInterleaved(pData,
                      "bgr",
                      pFrameInfo.nWidth, pFrameInfo.nHeight,
                      -1, "byte",
                      pFrameInfo.nWidth, pFrameInfo.nHeight,
                      0, 0, -1, 0);
                }
                else if (pFrameInfo.enPixelType == MyCamera.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed)
                {
                    Image.GenImageInterleaved(pData,
                     "rgb",
                     pFrameInfo.nWidth, pFrameInfo.nHeight,
                     -1, "byte",
                     pFrameInfo.nWidth, pFrameInfo.nHeight,
                     0, 0, -1, 0);
                }
                else if (pFrameInfo.enPixelType == MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG8)
                {
                    HImage tempImage = new HImage();
                    tempImage.GenImage1("byte", pFrameInfo.nWidth, pFrameInfo.nHeight, pData);
                    Image = tempImage.CfaToRgb("bayer_bg", "bilinear");
                }
                else if (pFrameInfo.enPixelType == MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB8)
                {
                    HImage tempImage = new HImage();
                    tempImage.GenImage1("byte", pFrameInfo.nWidth, pFrameInfo.nHeight, pData);
                    Image = tempImage.CfaToRgb("bayer_gb", "bilinear");
                }
                else if (pFrameInfo.enPixelType == MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR8)
                {
                    HImage tempImage = new HImage();
                    tempImage.GenImage1("byte", pFrameInfo.nWidth, pFrameInfo.nHeight, pData);
                    Image = tempImage.CfaToRgb("bayer_gr", "bilinear");
                }
                else if (pFrameInfo.enPixelType == MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG8)
                {
                    HImage tempImage = new HImage();
                    tempImage.GenImage1("byte", pFrameInfo.nWidth, pFrameInfo.nHeight, pData);
                    Image = tempImage.CfaToRgb("bayer_rg", "bilinear");
                }
                else
                {
                    LogNet.Error("没有找到合适图像格式");
                }
                //1.
                    curOrder++;
                    ProductMap.TryAdd(curOrder,new Product() { 
                    Order= curOrder,
                    Img= Image
                });
                //处理
                ProcessQueuqe.Enqueue(curOrder);
                //LogNet.Info(Name + "采集到图像，次数：" + curOrder);
                //3.信号
                //if (curOrder >= 4) { 
                ResQueuqe.Enqueue(curOrder);
                //}

            }
            catch (Exception ex)
            {
                LogNet.Error("获取图像失败:" + ex.Message);

            }



        }
        private void SaveImg(HObject Img, bool r)
        {
            if (!r)
            {
                if (ImgNGCount > 60)
                {
                    Directory.Delete(NGDir, true);

                }
                if (!Directory.Exists(NGDir))
                {
                    Directory.CreateDirectory(NGDir);
                }
                ImgNGCount++;
                HOperatorSet.WriteImage(Img, "bmp", 0, NGDir + "\\" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_ff"));
            }


        }
        #endregion
        public void Exe(HObject Image)
        {
                ExtHandler.AddGlobalVar<HObject>(Name + "图像", Image);
                ExtHandler.ExeProj(Name);
        }
        public void Exe(ref Product p)
        {
            double s = HighTime.GetMSec();
            //LogNet.Info(Name + "图像开始处理，次数：" + p.Order);
            //
            p.complete = false;
            Exe(p.Img);
            //取结果。
            p.Res= ExtHandler.GetGlobalVar<HTuple>(Name+"结果");
            p.complete = true;
            double e = HighTime.GetMSec();
            double t = e - s;
            if (t > 85)
            {
                LogNet.Error(Name + "图像处理完成，次数：" + p.Order + "耗时:" + t);
            }
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
        public string Name;
        public int ImgNGCount;
        public string NGDir;
        public Result Init()
        {
            Result result = new Result();
            bool r0 = FindDev();
            if (!r0)
            {
                result.msg += "相机:" + Name + "找不到,";
            }

            bool r = ConnectDev();
            result.msg = "";
            if (r)
            {
                result.msg += "相机:" + Name + "连接成功,";
            }
            else
            {
                result.msg += "相机:" + Name + "连接失败,";
            }
       
            result.status = r0 && r;
            

            RunTH = new Thread(() => {
                while (true)
                {
                    Thread.Sleep(1);
                    if (ProcessQueuqe.TryDequeue(out int  order))
                    {
                        if (ProductMap.TryGetValue(order, out Product p))
                        {
                            Exe(ref p);
                        }
                        
                    }
                }
            });
            RunTH.IsBackground = true;
            RunTH.Priority = ThreadPriority.Highest;
            RunTH.Start();
            //NGDir = "E:\\" + Name + "NG";
            //DirectoryInfo directoryInfo1 = new DirectoryInfo(NGDir);
            //if (directoryInfo1.Exists)
            //{
            //    ImgNGCount = directoryInfo1.GetFiles().Length;
            //    if (ImgNGCount > 60)
            //    {
            //        Directory.Delete(NGDir, true);

            //    }
            //}
            return result;
        }
        public void Destroy()
        {
            DisConnectDev();
        }
    }
    
}
