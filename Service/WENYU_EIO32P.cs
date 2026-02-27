using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;                   //**必须要的**

namespace WENYU_EIO32P
{

    public delegate void EventInterrupt();//定义委托事件
    public static class Program
    {
        /// <summary>
        /// 
        /****************************************************************************
                  函数名称: WY_Open
                  功能描述: 打开当前板卡，在关闭系统前，必须用WY_Close函数关闭。
                  参数列表:
                    返回值: 表示电脑中WENYU_EIO16P板卡数量。
          技术支持联系电话：13510401592
          *****************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_Open", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_Open();

        /****************************************************************************
                函数名称: WY_Close
                功能描述: 关闭注销当前板卡。关闭后，不能对板卡相关操作。它与WY_Open函数对应。
                参数列表:
                  返回值: 表示电脑中WENYU_EIO16P板卡数量。  
        技术支持联系电话：13510401592
        *****************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_Close", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_Close();

        /****************************************************************************
                函数名称: WY_GetCardVersion
                功能描述: 获取本开关量控制卡版本号
                参数列表:
	              CardNo: 操作当前板卡;
               VerNumber: 返回版本号，30代表版本3.0;
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败  
        技术支持联系电话：13510401592
        *****************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_GetCardVersion", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_GetCardVersion(int CardNo, ref long VerNumber);

        /*****************************************************************************
                函数名称: WY_GetLowInPutData
                功能描述: 获取开关量控制卡输入端口低8位数据
                参数列表:
	            DeviceID: 当前板卡DeviceID参数值。（从WY_Open函数获取）。
                 LowData: 输入端口低8位数据，对应关系如下：
                                LowData数据: bit7,bit6,bit5,bit4,bit3,bit2,bit1,bit0
                               对应输入端口: Input7,Input6,Input5,Input4,Input3,Input2,Input1,Input0
                  返回值: 表示函数返状态  0:正确    1:板卡连接失败
        技术支持联系电话：13510401592
        ******************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_GetLowInPutData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_GetLowInPutData(int CardNo, ref long LowData);

        /******************************************************************************
                函数名称: WY_GetHighInPutData
                功能描述: 获取开关量控制卡输入端口高8位数据
                参数列表:
                DeviceID: 当前板卡DeviceID参数值。（从WY_Open函数获取）。
                HighData: 输入端口高8位数据，对应关系如下：
                               HighData数据:bit7,bit6,bit5,bit4,bit3,bit2,bit1,bit0
                               对应输入端口:Input15,Input14,Input13,Input12,Input11,Input10,Input9,Input8
                  返回值: 表示函数返状态  0:正确    1:板卡连接失败
        技术支持联系电话：13510401592
        *******************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_GetHighInPutData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_GetHighInPutData(int CardNo, ref long HighData);

        /*****************************************************************************
                函数名称: WY_GetLowOutPutData
                功能描述: 获取开关量控制卡输出端口低8位输出数据
                参数列表:
		        DeviceID: 当前板卡DeviceID参数值。（从WY_Open函数获取）。
                 LowData: 输出端口低8位数据，对应关系如下：
                                LowData数据:bit7,bit6,bit5,bit4,bit3,bit2,bit1,bit0
                               对应输入端口:Output7,Onput6,Onput5,Onput4,Onput3,Onput2,Onput1,Onput0
                  返回值: 表示函数返状态  0:正确    1:板卡连接失败
        技术支持联系电话：13510401592
        *****************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_GetLowOutPutData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_GetLowOutPutData(int CardNo, ref long LowData);

        /*******************************************************************************
                函数名称: WY_GetHighOutPutData
                功能描述: 获取开关量控制卡输出端口高8位输出数据
                参数列表:
		        DeviceID: 当前板卡DeviceID参数值。（从WY_Open函数获取）。
                HighData: 输出端口高8位数据，对应关系如下：
                               HighData数据:bit7,bit6,bit5,bit4,bit3,bit2,bit1,bit0
                               对应输入端口:Output15,Onput14,Onput13,Onput12,Onput11,Onput10,Onput9,Onput8
                  返回值: 表示函数返状态  0:正确    1:板卡连接失败
        技术支持联系电话：13510401592
        ******************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_GetHighOutPutData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_GetHighOutPutData(int CardNo, ref long HighData);

        /****************************************************************************
                函数名称: WY_GetOutPutData
                功能描述: 获取开关量控制卡输出端口输出数据
                参数列表:
		        DeviceID: 当前板卡DeviceID参数值。（从WY_Open函数获取）。
              OutPutData: 输出端口数据
                  返回值: 表示函数返状态  0:正确    1:板卡连接失败
        技术支持联系电话：13510401592
        ****************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_GetOutPutData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_GetOutPutData(int CardNo, ref long OutPutData);
        
        /***************************************************************************
                函数名称: WY_GetInPutData
                功能描述: 获取开关量控制卡输入端数据
                参数列表:
                  CardNo: 操作当前板卡。
               InPutData: 返回输入端口数据（0位对应DIOO；1位对应DI01;2位对应DI02;3位对应DI03...)。
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败
        技术支持联系电话：13510401592
        ****************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_GetInPutData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_GetInPutData(int CardNo, ref long InPutData);

        /***************************************************************************
                函数名称: WY_ReadInPutbit0
                功能描述: 获取开关量控制卡输入端口0状态
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 返回输入端口电平值（0：表示有信号，1：表示无信号）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ReadInPutbit0", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ReadInPutbit0(int CardNo, ref int value);

        /************************************************************************
                函数名称: WY_ReadInPutbit1
                功能描述: 获取开关量控制卡输入端口1状态
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 返回输入端口电平值（0：表示有信号，1：表示无信号）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败 
        技术支持联系电话：13510401592
        *************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ReadInPutbit1", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ReadInPutbit1(int CardNo, ref int value);

        /************************************************************************
                函数名称: WY_ReadInPutbit2
                功能描述: 获取开关量控制卡输入端口2状态
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 返回输入端口电平值（0：表示有信号，1：表示无信号）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败
        技术支持联系电话：13510401592
        *************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ReadInPutbit2", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ReadInPutbit2(int CardNo, ref int value);

        /************************************************************************
                函数名称: WY_ReadInPutbit3
                功能描述: 获取开关量控制卡输入端口3状态
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 返回输入端口电平值（0：表示有信号，1：表示无信号）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败
        技术支持联系电话：13510401592
        *************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ReadInPutbit3", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ReadInPutbit3(int CardNo, ref int value);

        /************************************************************************
                函数名称: WY_ReadInPutbit4
                功能描述: 获取开关量控制卡输入端口4状态
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 返回输入端口电平值（0：表示有信号，1：表示无信号）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败
        技术支持联系电话：13510401592
        *************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ReadInPutbit4", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ReadInPutbit4(int CardNo, ref int value);

        /************************************************************************
                函数名称: WY_ReadInPutbit5
                功能描述: 获取开关量控制卡输入端口5状态
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 返回输入端口电平值（0：表示有信号，1：表示无信号）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败
        技术支持联系电话：13510401592
        *************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ReadInPutbit5", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ReadInPutbit5(int CardNo, ref int value);

        /************************************************************************
                函数名称: WY_ReadInPutbit6
                功能描述: 获取开关量控制卡输入端口6状态
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 返回输入端口电平值（0：表示有信号，1：表示无信号）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败 
        技术支持联系电话：13510401592
        *************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ReadInPutbit6", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ReadInPutbit6(int CardNo, ref int value);

        /************************************************************************
                函数名称: WY_ReadInPutbit7
                功能描述: 获取开关量控制卡输入端口7状态
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 返回输入端口电平值（0：表示有信号，1：表示无信号）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ReadInPutbit7", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ReadInPutbit7(int CardNo, ref int value);

        /************************************************************************
                函数名称: WY_ReadInPutbit8
                功能描述: 获取开关量控制卡输入端口8状态
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 返回输入端口电平值（0：表示有信号，1：表示无信号）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ReadInPutbit8", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ReadInPutbit8(int CardNo, ref int value);

        /************************************************************************
                函数名称: WY_ReadInPutbit9
                功能描述: 获取开关量控制卡输入端口9状态
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 返回输入端口电平值（0：表示有信号，1：表示无信号）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ReadInPutbit9", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ReadInPutbit9(int CardNo, ref int value);

        /************************************************************************
                函数名称: WY_ReadInPutbit10
                功能描述: 获取开关量控制卡输入端口10状态
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 返回输入端口电平值（0：表示有信号，1：表示无信号）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ReadInPutbit10", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ReadInPutbit10(int CardNo, ref int value);

        /************************************************************************
                函数名称: WY_ReadInPutbit11
                功能描述: 获取开关量控制卡输入端口11状态
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 返回输入端口电平值（0：表示有信号，1：表示无信号）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败  5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ReadInPutbit11", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ReadInPutbit11(int CardNo, ref int value);

        /************************************************************************
                函数名称: WY_ReadInPutbit12
                功能描述: 获取开关量控制卡输入端口12状态
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 返回输入端口电平值（0：表示有信号，1：表示无信号）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败  5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ReadInPutbit12", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ReadInPutbit12(int CardNo, ref int value);

        /************************************************************************
                函数名称: WY_ReadInPutbit13
                功能描述: 获取开关量控制卡输入端口13状态
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 返回输入端口电平值（0：表示有信号，1：表示无信号）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败  5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ReadInPutbit13", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ReadInPutbit13(int CardNo, ref int value);

        /************************************************************************
                函数名称: WY_ReadInPutbit14
                功能描述: 获取开关量控制卡输入端口14状态
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 返回输入端口电平值（0：表示有信号，1：表示无信号）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败  5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ReadInPutbit14", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ReadInPutbit14(int CardNo, ref int value);

        /************************************************************************
                函数名称: WY_ReadInPutbit15
                功能描述: 获取开关量控制卡输入端口15状态
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 返回输入端口电平值（0：表示有信号，1：表示无信号）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败  5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ReadInPutbit15", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ReadInPutbit15(int CardNo, ref int value);

        /************************************************************************
            函数名称: WY_ReadInPutbit
            功能描述: 获取开关量控制卡输入端口状态
            参数列表:
              CardNo: 操作当前板卡。
             portbit: DI端口号,0，1，2，3，4，5，6，7；
               value: 返回输入端口电平值（0：表示有信号，1：表示无信号）
              返回值: 表示函数返状态  0:操作成功    1:板卡连接失败   5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ReadInPutbit", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ReadInPutbit(int CardNo, int portbit, ref int value);

        /************************************************************************
                函数名称: WY_SetOutPutData
                功能描述: 设置开关量控制卡输出端口16位数据
                参数列表:
                  CardNo: 操作当前板卡。
              OutPutData: 输出端口数据。（0位对应DOOO；1位对应DO01;2位对应DO02;3位对应DO03...)。。
                  返回值: 表示函数返状态   0: 操作成功    
                                           1:板卡连接失败   
                                           3：输入参数错误,OutPutData输入数值超出范围0x0000~0xffff
                                           5: 板卡连接中断
        技术支持联系电话：13510401592
        **************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetOutPutData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetOutPutData(int CardNo, long OutPutData);

        /************************************************************************
                函数名称: WY_SetLowOutPutData
                功能描述: 设置开关量控制卡输出端口低8位数据
                参数列表:
		        DeviceID: 当前板卡DeviceID参数值。（从WY_Open函数获取）。
                 LowData: 输出端口低8位数据，对应关系如下：
                                 LowData数据:bit7,bit6,bit5,bit4,bit3,bit2,bit1,bit0
                                对应输入端口:Output7,Onput6,Onput5,Onput4,Onput3,Onput2,Onput1,Onput0
                  返回值: 表示函数返状态   0:正确    1:板卡连接失败  3：输入参数错误,输入数值超出范围0x00~0xff
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetLowOutPutData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetLowOutPutData(int CardNo, long LowData);

        /************************************************************************
                函数名称: WY_SetHighOutPutData
                功能描述: 设置开关量控制卡输出端口高8位数据
                参数列表:
                DeviceID: 当前板卡DeviceID参数值。（从WY_Open函数获取）。
                HighData: 输出端口高8位数据，对应关系如下：
                                HighData数据:bit 15,bit14,bit13,bit12,bit11,bit10,bit9,bit8,
                                对应输入端口:Output7,Onput6,Onput5,Onput4,Onput3,Onput2,Onput1,Onput0
                  返回值: 表示函数返状态   0:正确    1:板卡连接失败  
                                           3：输入参数错误,输入数值超出范围0x00~0xff
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetHighOutPutData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetHighOutPutData(int CardNo, long HighData);

        /************************************************************************
                函数名称: WY_WriteOutPutBit0
                功能描述: 向开关量控制卡输出端口0写入数据
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 表示端口DO00输出电平值（0：表示低电平，1：表示高电平）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败  3:输入参数错误  5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_WriteOutPutBit0", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_WriteOutPutBit0(int CardNo, int value);


        /***********************************************************************
                函数名称: WY_WriteOutPutBit1
                功能描述: 向开关量控制卡输出端口1写入数据
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 表示端口DO01输出电平值（0：表示低电平，1：表示高电平）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败  3:输入参数错误  5:板卡连接中断
        技术支持联系电话：13510401592
        ***********************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_WriteOutPutBit1", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_WriteOutPutBit1(int CardNo, int value);

        /**********************************************************************
             '* 函数名称: WY_WriteOutPutBit2
             '* 功能描述: 向开关量控制卡输出端口2写入数据
             '* 参数列表:
                  CardNo: 操作当前板卡。
                   value: 表示端口DO02输出电平值（0：表示低电平，1：表示高电平）
               '* 返回值: 表示函数返状态  0:操作成功    1:板卡连接失败  3:输入参数错误  5:板卡连接中断
        技术支持联系电话：13510401592
        **********************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_WriteOutPutBit2", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_WriteOutPutBit2(int CardNo, int value);

        /*********************************************************************
                函数名称: WY_WriteOutPutBit3
                功能描述: 向开关量控制卡输出端口3写入数据
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 表示端口DO03输出电平值（0：表示低电平，1：表示高电平）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败  3:输入参数错误  5:板卡连接中断
        技术支持联系电话：13510401592
        ***********************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_WriteOutPutBit3", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_WriteOutPutBit3(int CardNo, int value);

        /*********************************************************************
                函数名称: WY_WriteOutPutBit4
                功能描述: 向开关量控制卡输出端口4写入数据
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 表示端口DO04输出电平值（0：表示低电平，1：表示高电平）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败  3:输入参数错误 5:板卡连接中断
        技术支持联系电话：13510401592
        **********************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_WriteOutPutBit4", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_WriteOutPutBit4(int CardNo, int value);

        /*********************************************************************
                函数名称: WY_WriteOutPutBit5
                功能描述: 向开关量控制卡输出端口5写入数据
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 表示端口DO05输出电平值（0：表示低电平，1：表示高电平）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败  3:输入参数错误 5:板卡连接中断
        技术支持联系电话：13510401592
        *********************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_WriteOutPutBit5", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_WriteOutPutBit5(int CardNo, int value);

        /********************************************************************
                函数名称: WY_WriteOutPutBit6
                功能描述: 向开关量控制卡输出端口6写入数据
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 表示端口电平值（0：表示低电平，1：表示高电平）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败  3:输入参数错误 5:板卡连接中断
        技术支持联系电话：13510401592
        *********************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_WriteOutPutBit6", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_WriteOutPutBit6(int CardNo, int value);

        /************************************************************************
                函数名称: WY_WriteOutPutBit7
                功能描述: 向开关量控制卡输出端口7写入数据
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 表示端口电平值（0：表示低电平，1：表示高电平）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败  3:输入参数错误 5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_WriteOutPutBit7", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_WriteOutPutBit7(int CardNo, int value);

        /************************************************************************
                函数名称: WY_WriteOutPutBit8
                功能描述: 向开关量控制卡输出端口8写入数据
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 表示端口电平值（0：表示低电平，1：表示高电平）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败  3:输入参数错误 5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_WriteOutPutBit8", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_WriteOutPutBit8(int CardNo, int value);

        /************************************************************************
                函数名称: WY_WriteOutPutBit9
                功能描述: 向开关量控制卡输出端口9写入数据
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 表示端口电平值（0：表示低电平，1：表示高电平）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败  3:输入参数错误 5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_WriteOutPutBit9", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_WriteOutPutBit9(int CardNo, int value);

        /************************************************************************
                函数名称: WY_WriteOutPutBit10
                功能描述: 向开关量控制卡输出端口10写入数据
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 表示端口电平值（0：表示低电平，1：表示高电平）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败  3:输入参数错误 5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_WriteOutPutBit10", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_WriteOutPutBit10(int CardNo, int value);

        /************************************************************************
                函数名称: WY_WriteOutPutBit11
                功能描述: 向开关量控制卡输出端口11写入数据
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 表示端口电平值（0：表示低电平，1：表示高电平）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败  3:输入参数错误 5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_WriteOutPutBit11", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_WriteOutPutBit11(int CardNo, int value);

        /************************************************************************
                函数名称: WY_WriteOutPutBit12
                功能描述: 向开关量控制卡输出端口12写入数据
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 表示端口电平值（0：表示低电平，1：表示高电平）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败  3:输入参数错误 5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_WriteOutPutBit12", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_WriteOutPutBit12(int CardNo, int value);

        /************************************************************************
                函数名称: WY_WriteOutPutBit13
                功能描述: 向开关量控制卡输出端口13写入数据
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 表示端口电平值（0：表示低电平，1：表示高电平）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败  3:输入参数错误 5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_WriteOutPutBit13", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_WriteOutPutBit13(int CardNo, int value);

        /************************************************************************
                函数名称: WY_WriteOutPutBit14
                功能描述: 向开关量控制卡输出端口14写入数据
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 表示端口电平值（0：表示低电平，1：表示高电平）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败  3:输入参数错误 5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_WriteOutPutBit14", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_WriteOutPutBit14(int CardNo, int value);

        /************************************************************************
                函数名称: WY_WriteOutPutBit15
                功能描述: 向开关量控制卡输出端口15写入数据
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 表示端口电平值（0：表示低电平，1：表示高电平）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败  3:输入参数错误 5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_WriteOutPutBit15", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_WriteOutPutBit15(int CardNo, int value);

        /*************************************************************************
                函数名称: WY_WriteOutPutBit
                功能描述: 向开关量控制卡输出端口信号
                参数列表:
                  CardNo: 操作当前板卡。
                 portbit: DI端口号,0，1，2，3，4，5，6，7；
                   value: 表示端口电平值（0：表示低电平，1：表示高电平）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败  3:输入参数错误   5:板卡连接中断
        技术支持联系电话：13510401592
        ***************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_WriteOutPutBit", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_WriteOutPutBit(int CardNo, int portbit, int value);

        /************************************************************************
                函数名称: WY_ReadOutPutBit0
                功能描述: 向开关量控制卡输出端口0回读数据
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 回读输出端口DO00电平值（0：表示低电平，1：表示高电平）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ReadOutPutBit0", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ReadOutPutBit0(int CardNo, ref int value);

        /************************************************************************
                函数名称: WY_ReadOutPutBit1
                功能描述: 向开关量控制卡输出端口1回读数据
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 回读输出端口DO01电平值（0：表示低电平，1：表示高电平）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败   5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ReadOutPutBit1", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ReadOutPutBit1(int CardNo, ref int value);

        /************************************************************************
                函数名称: WY_ReadOutPutBit2
                功能描述: 向开关量控制卡输出端口2回读数据
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 回读输出端口DO02电平值（0：表示低电平，1：表示高电平）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败  5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ReadOutPutBit2", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ReadOutPutBit2(int CardNo, ref int value);

        /************************************************************************
                函数名称: WY_ReadOutPutBit3
                功能描述: 向开关量控制卡输出端口3回读数据
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 回读输出端口DO03电平值（0：表示低电平，1：表示高电平）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败  5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ReadOutPutBit3", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ReadOutPutBit3(int CardNo, ref int value);

        /************************************************************************
                函数名称: WY_ReadOutPutBit4
                功能描述: 向开关量控制卡输出端口4回读数据
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 回读输出端口DO04电平值（0：表示低电平，1：表示高电平）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败  5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ReadOutPutBit4", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ReadOutPutBit4(int CardNo, ref int value);

        /************************************************************************
                函数名称: WY_ReadOutPutBit5
                功能描述: 向开关量控制卡输出端口5回读数据
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 回读输出端口DO05电平值（0：表示低电平，1：表示高电平）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败   5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ReadOutPutBit5", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ReadOutPutBit5(int CardNo, ref int value);

        /************************************************************************
                函数名称: WY_ReadOutPutBit6
                功能描述: 向开关量控制卡输出端口6回读数据
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 回读输出端口DO06电平值（0：表示低电平，1：表示高电平）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ReadOutPutBit6", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ReadOutPutBit6(int CardNo, ref int value);

        /************************************************************************
                函数名称: WY_ReadOutPutBit7
                功能描述: 向开关量控制卡输出端口7回读数据
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 回读输出端口DO07电平值（0：表示低电平，1：表示高电平）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ReadOutPutBit7", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ReadOutPutBit7(int CardNo, ref int value);

        /************************************************************************
                函数名称: WY_ReadOutPutBit8
                功能描述: 向开关量控制卡输出端口8回读数据
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 回读输出端口DO08电平值（0：表示低电平，1：表示高电平）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ReadOutPutBit8", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ReadOutPutBit8(int CardNo, ref int value);

        /************************************************************************
                函数名称: WY_ReadOutPutBit9
                功能描述: 向开关量控制卡输出端口9回读数据
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 回读输出端口DO09电平值（0：表示低电平，1：表示高电平）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ReadOutPutBit9", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ReadOutPutBit9(int CardNo, ref int value);

        /************************************************************************
                函数名称: WY_ReadOutPutBit10
                功能描述: 向开关量控制卡输出端口10回读数据
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 回读输出端口DO10电平值（0：表示低电平，1：表示高电平）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ReadOutPutBit10", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ReadOutPutBit10(int CardNo, ref int value);

        /************************************************************************
                函数名称: WY_ReadOutPutBit11
                功能描述: 向开关量控制卡输出端口11回读数据
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 回读输出端口DO11电平值（0：表示低电平，1：表示高电平）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ReadOutPutBit11", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ReadOutPutBit11(int CardNo, ref int value);

        /************************************************************************
                函数名称: WY_ReadOutPutBit12
                功能描述: 向开关量控制卡输出端口12回读数据
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 回读输出端口DO12电平值（0：表示低电平，1：表示高电平）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ReadOutPutBit12", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ReadOutPutBit12(int CardNo, ref int value);

        /************************************************************************
                函数名称: WY_ReadOutPutBit13
                功能描述: 向开关量控制卡输出端口13回读数据
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 回读输出端口DO13电平值（0：表示低电平，1：表示高电平）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ReadOutPutBit13", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ReadOutPutBit13(int CardNo, ref int value);

        /************************************************************************
                函数名称: WY_ReadOutPutBit14
                功能描述: 向开关量控制卡输出端口14回读数据
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 回读输出端口DO14电平值（0：表示低电平，1：表示高电平）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ReadOutPutBit14", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ReadOutPutBit14(int CardNo, ref int value);

        /************************************************************************
                函数名称: WY_ReadOutPutBit15
                功能描述: 向开关量控制卡输出端口15回读数据
                参数列表:
                  CardNo: 操作当前板卡。
                   value: 回读输出端口DO15电平值（0：表示低电平，1：表示高电平）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ReadOutPutBit15", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ReadOutPutBit15(int CardNo, ref int value);

        /************************************************************************
                函数名称: WY_ReadOutPutBit
                功能描述: 向开关量控制卡输出端口回读数据
                参数列表:
                  CardNo: 操作当前板卡。
                 portbit: DI端口号,0，1，2，3，4，5，6，7；
                   value: 回读输出端口电平值（0：表示低电平，1：表示高电平）
                  返回值: 表示函数返状态  0:操作成功    1:板卡连接失败  5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ReadOutPutBit", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ReadOutPutBit(int CardNo, int portbit, ref int value);

        /************************************************************************
                        函数名称: WY_ResetInterrupt
                        功能描述: 中断复位。中断事件发生后，必须用WY_ResetInterrupt函数复位，才能进行下一次中断。
                        参数列表:
                          CardNo: 操作当前板卡。
                          返回值: 表示函数返状态  0:操作成功    1:板卡操作失败  5:板卡连接中断
         技术支持联系电话：13510401592
         ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ResetInterrupt", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ResetInterrupt(int CardNo);

        /************************************************************************
                        函数名称: WY_GetInterruptData
                        功能描述: 发生中断事件后，茯取中断数据。
                        参数列表:
                          CardNo: 操作当前板卡。
                        RiseData：DI端口上升沿中断发生数据。
                                  位0：如果为0：表示DI00端口产生上升沿中断；1：表示DI00端口没有产生上升沿中断；
                                  位1：如果为0：表示DI01端口产生上升沿中断；1：表示DI01端口没有产生上升沿中断；
                                  位2：如果为0：表示DI02端口产生上升沿中断；1：表示DI02端口没有产生上升沿中断；
                                  位3：如果为0：表示DI03端口产生上升沿中断；1：表示DI03端口没有产生上升沿中断；
                                  位4：如果为0：表示DI04端口产生上升沿中断；1：表示DI04端口没有产生上升沿中断；
                                  位5：如果为0：表示DI05端口产生上升沿中断；1：表示DI05端口没有产生上升沿中断；
                                  位6：如果为0：表示DI06端口产生上升沿中断；1：表示DI06端口没有产生上升沿中断；
                                  位7：如果为0：表示DI07端口产生上升沿中断；1：表示DI07端口没有产生上升沿中断；
                                  位8：如果为0：表示DI08端口产生上升沿中断；1：表示DI08端口没有产生上升沿中断；
                                  位9：如果为0：表示DI09端口产生上升沿中断；1：表示DI09端口没有产生上升沿中断；
                                  位10：如果为0：表示DI10端口产生上升沿中断；1：表示DI10端口没有产生上升沿中断；
                                  位11：如果为0：表示DI11端口产生上升沿中断；1：表示DI11端口没有产生上升沿中断；
                                  位12：如果为0：表示DI12端口产生上升沿中断；1：表示DI12端口没有产生上升沿中断；
                                  位13：如果为0：表示DI13端口产生上升沿中断；1：表示DI13端口没有产生上升沿中断；
                                  位14：如果为0：表示DI14端口产生上升沿中断；1：表示DI14端口没有产生上升沿中断；
                                  位15：如果为0：表示DI15端口产生上升沿中断；1：表示DI15端口没有产生上升沿中断；
                        FallData：DI端口下降沿中断发生数据。
                                  位0：如果为0：表示DI00端口产生下降沿中断；1：表示DI00端口没有产生下降沿中断；
                                  位1：如果为0：表示DI01端口产生下降沿中断；1：表示DI01端口没有产生下降沿中断；
                                  位2：如果为0：表示DI02端口产生下降沿中断；1：表示DI02端口没有产生下降沿中断；
                                  位3：如果为0：表示DI03端口产生下降沿中断；1：表示DI03端口没有产生下降沿中断；
                                  位4：如果为0：表示DI04端口产生下降沿中断；1：表示DI04端口没有产生下降沿中断；
                                  位5：如果为0：表示DI05端口产生下降沿中断；1：表示DI05端口没有产生下降沿中断；
                                  位6：如果为0：表示DI06端口产生下降沿中断；1：表示DI06端口没有产生下降沿中断；
                                  位7：如果为0：表示DI07端口产生下降沿中断；1：表示DI07端口没有产生下降沿中断；
                                  位8：如果为0：表示DI08端口产生下降沿中断；1：表示DI08端口没有产生下降沿中断；
                                  位9：如果为0：表示DI09端口产生下降沿中断；1：表示DI09端口没有产生下降沿中断；
                                  位10：如果为0：表示DI10端口产生下降沿中断；1：表示DI10端口没有产生下降沿中断；
                                  位11：如果为0：表示DI11端口产生下降沿中断；1：表示DI11端口没有产生下降沿中断；
                                  位12：如果为0：表示DI12端口产生下降沿中断；1：表示DI12端口没有产生下降沿中断；
                                  位13：如果为0：表示DI13端口产生下降沿中断；1：表示DI13端口没有产生下降沿中断；
                                  位14：如果为0：表示DI14端口产生下降沿中断；1：表示DI14端口没有产生下降沿中断；
                                  位15：如果为0：表示DI15端口产生下降沿中断；1：表示DI15端口没有产生下降沿中断；
                          返回值: 表示函数返状态  0:正确    1:板卡操作失败   5:板卡连接中断
         技术支持联系电话：13510401592
         ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_GetInterruptData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_GetInterruptData(int CardNo, ref UInt16 RiseData, ref UInt16 FallData);

        /************************************************************************
                        函数名称: WY_SetInterruptFun
                        功能描述: 设置中断事件函数名称；
                        参数列表:
                          CardNo: 操作当前板卡。
                  EventInterrupt：用户自定义中断函数名称。
                          返回值: 表示函数返状态  0:操作成功    1:板卡操作失败    5:板卡连接中断
         技术支持联系电话：13510401592
         ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetInterruptFun", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetInterruptFun(int CardNo, EventInterrupt InterruptFun);

        /************************************************************************
                        函数名称: WY_SetInterruptEnable
                        功能描述: 设置中断功能使能打开或关闭；
                        参数列表:
                          CardNo: 操作当前板卡。
                          Enable：中断功能打开或关闭；0：中断功能关闭； 1：中断功能打开
                          返回值: 表示函数返状态  0:操作成功    1:板卡操作失败  5:板卡连接中断
         技术支持联系电话：13510401592
         ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetInterruptEnable", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetInterruptEnable(int CardNo, int Enable);

        /************************************************************************
                        函数名称: WY_SetInterruptPortEnable
                        功能描述: 设置DI端口中断功能使能；
                        参数列表:
                          CardNo: 操作当前板卡。
                      RiseEnable：DI端口上升沿中断功能打开或关闭，各位对应关系如下：
                                  位0：如果为0：表示DI00端口产生上升沿中断使能关闭；1：表示DI00端口产生上升沿中断使能打开；
                                  位1：如果为0：表示DI01端口产生上升沿中断使能关闭；1：表示DI01端口产生上升沿中断使能打开；
                                  位2：如果为0：表示DI02端口产生上升沿中断使能关闭；1：表示DI02端口产生上升沿中断使能打开；
                                  位3：如果为0：表示DI03端口产生上升沿中断使能关闭；1：表示DI03端口产生上升沿中断使能打开；
                                  位4：如果为0：表示DI04端口产生上升沿中断使能关闭；1：表示DI04端口产生上升沿中断使能打开；
                                  位5：如果为0：表示DI05端口产生上升沿中断使能关闭；1：表示DI05端口产生上升沿中断使能打开；
                                  位6：如果为0：表示DI06端口产生上升沿中断使能关闭；1：表示DI06端口产生上升沿中断使能打开；
                                  位7：如果为0：表示DI07端口产生上升沿中断使能关闭；1：表示DI07端口产生上升沿中断使能打开；
                                  位8：如果为0：表示DI08端口产生上升沿中断使能关闭；1：表示DI08端口产生上升沿中断使能打开；
                                  位9：如果为0：表示DI09端口产生上升沿中断使能关闭；1：表示DI09端口产生上升沿中断使能打开；
                                  位10：如果为0：表示DI10端口产生上升沿中断使能关闭；1：表示DI10端口产生上升沿中断使能打开；
                                  位11：如果为0：表示DI11端口产生上升沿中断使能关闭；1：表示DI11端口产生上升沿中断使能打开；
                                  位12：如果为0：表示DI12端口产生上升沿中断使能关闭；1：表示DI12端口产生上升沿中断使能打开；
                                  位13：如果为0：表示DI13端口产生上升沿中断使能关闭；1：表示DI13端口产生上升沿中断使能打开；
                                  位14：如果为0：表示DI14端口产生上升沿中断使能关闭；1：表示DI14端口产生上升沿中断使能打开；
                                  位15：如果为0：表示DI15端口产生上升沿中断使能关闭；1：表示DI15端口产生上升沿中断使能打开；
                      FallEnable：DI端口上升沿中断功能打开或关闭，各位对应关系如下：
                                  位0：如果为0：表示DI00端口产生下降沿中断使能关闭；1：表示DI00端口产生下降沿中断使能打开；
                                  位1：如果为0：表示DI01端口产生下降沿中断使能关闭；1：表示DI01端口产生下降沿中断使能打开；
                                  位2：如果为0：表示DI02端口产生下降沿中断使能关闭；1：表示DI02端口产生下降沿中断使能打开；
                                  位3：如果为0：表示DI03端口产生下降沿中断使能关闭；1：表示DI03端口产生下降沿中断使能打开；
                                  位4：如果为0：表示DI04端口产生下降沿中断使能关闭；1：表示DI04端口产生下降沿中断使能打开；
                                  位5：如果为0：表示DI05端口产生下降沿中断使能关闭；1：表示DI05端口产生下降沿中断使能打开；
                                  位6：如果为0：表示DI06端口产生下降沿中断使能关闭；1：表示DI06端口产生下降沿中断使能打开；
                                  位7：如果为0：表示DI07端口产生下降沿中断使能关闭；1：表示DI07端口产生下降沿中断使能打开；   
                                  位8：如果为0：表示DI08端口产生下降沿中断使能关闭；1：表示DI08端口产生下降沿中断使能打开；
                                  位9：如果为0：表示DI09端口产生下降沿中断使能关闭；1：表示DI09端口产生下降沿中断使能打开；
                                  位10：如果为0：表示DI10端口产生下降沿中断使能关闭；1：表示DI10端口产生下降沿中断使能打开；
                                  位11：如果为0：表示DI11端口产生下降沿中断使能关闭；1：表示DI11端口产生下降沿中断使能打开；
                                  位12：如果为0：表示DI12端口产生下降沿中断使能关闭；1：表示DI12端口产生下降沿中断使能打开；
                                  位13：如果为0：表示DI13端口产生下降沿中断使能关闭；1：表示DI13端口产生下降沿中断使能打开；
                                  位14：如果为0：表示DI14端口产生下降沿中断使能关闭；1：表示DI14端口产生下降沿中断使能打开；
                                  位15：如果为0：表示DI15端口产生下降沿中断使能关闭；1：表示DI15端口产生下降沿中断使能打开；   
                          返回值: 表示函数返状态  0:操作成功    1:板卡操作失败    5:板卡连接中断
         技术支持联系电话：13510401592
         ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetInterruptPortEnable", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetInterruptPortEnable(int CardNo, UInt16 RiseEnable, UInt16 FallEnable);

        /************************************************************************
                       函数名称: WY_ClrInterruptRisePort
                       功能描述: 清除指定DI端口发生的上升沿中断事件。中断事件发生后，
                                 必须用此函数清除后，才能进行下一次DI上升沿中断。
                       参数列表:
                         CardNo: 操作的板卡的编号，对应PCIe槽0,1,2,3....
                        portbit: DI端口号,0，1，2，3，4，5，6，7；
                         返回值: 表示函数返状态  0:操作成功    1:板卡操作失败  5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ClrInterruptRisePort", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ClrInterruptRisePort(int CardNo, int portbit);

        /************************************************************************
                       函数名称: WY_ClrInterruptRisePort0
                       功能描述: 清除当前DI00端口发生的上升沿中断事件。中断事件发生后，
                                 必须用此函数清除后，才能进行下一次DIOO上升沿中断。
                       参数列表:
                         CardNo: 操作当前板卡。
                         返回值: 表示函数返状态  0:操作成功    1:板卡操作失败  5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ClrInterruptRisePort0", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ClrInterruptRisePort0(int CardNo);

        /************************************************************************
                       函数名称: WY_ClrInterruptRisePort1
                       功能描述: 清除当前DI01端口发生的上升沿中断事件。中断事件发生后，
                                 必须用此函数清除后，才能进行下一次DIO1上升沿中断。
                       参数列表:
                         CardNo: 操作当前板卡。
                         返回值: 表示函数返状态  0:操作成功    1:板卡操作失败  5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ClrInterruptRisePort1", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ClrInterruptRisePort1(int CardNo);

        /************************************************************************
                       函数名称: WY_ClrInterruptRisePort2
                       功能描述: 清除当前DI02端口发生的上升沿中断事件。中断事件发生后，
                                 必须用此函数清除后，才能进行下一次DIO2上升沿中断。
                       参数列表:
                         CardNo: 操作当前板卡。
                         返回值: 表示函数返状态  0:操作成功    1:板卡操作失败   5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ClrInterruptRisePort2", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ClrInterruptRisePort2(int CardNo);

        /************************************************************************
                       函数名称: WY_ClrInterruptRisePort3
                       功能描述: 清除当前DI03端口发生的上升沿中断事件。中断事件发生后，
                                 必须用此函数清除后，才能进行下一次DIO3上升沿中断。
                       参数列表:
                         CardNo: 操作当前板卡。
                         返回值: 表示函数返状态  0:操作成功    1:板卡操作失败   5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ClrInterruptRisePort3", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ClrInterruptRisePort3(int CardNo);

        /************************************************************************
                       函数名称: WY_ClrInterruptRisePort4
                       功能描述: 清除当前DI04端口发生的上升沿中断事件。中断事件发生后，
                                 必须用此函数清除后，才能进行下一次DIO4上升沿中断。
                       参数列表:
                         CardNo: 操作当前板卡。
                         返回值: 表示函数返状态  0:操作成功    1:板卡操作失败   5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ClrInterruptRisePort4", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ClrInterruptRisePort4(int CardNo);

        /************************************************************************
                       函数名称: WY_ClrInterruptRisePort5
                       功能描述: 清除当前DI05端口发生的上升沿中断事件。中断事件发生后，
                                 必须用此函数清除后，才能进行下一次DIO5上升沿中断。
                       参数列表:
                         CardNo: 操作当前板卡。
                         返回值: 表示函数返状态  0:操作成功    1:板卡操作失败   5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ClrInterruptRisePort5", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ClrInterruptRisePort5(int CardNo);

        /************************************************************************
                       函数名称: WY_ClrInterruptRisePort6
                       功能描述: 清除当前DI06端口发生的上升沿中断事件。中断事件发生后，
                                 必须用此函数清除后，才能进行下一次DIO6上升沿中断。
                       参数列表:
                         CardNo: 操作当前板卡。
                         返回值: 表示函数返状态  0:操作成功    1:板卡操作失败   5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ClrInterruptRisePort6", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ClrInterruptRisePort6(int CardNo);

        /************************************************************************
                       函数名称: WY_ClrInterruptRisePort7
                       功能描述: 清除当前DI07端口发生的上升沿中断事件。中断事件发生后，
                                 必须用此函数清除后，才能进行下一次DIO7上升沿中断。
                       参数列表:
                         CardNo: 操作当前板卡。
                         返回值: 表示函数返状态  0:操作成功    1:板卡操作失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ClrInterruptRisePort7", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ClrInterruptRisePort7(int CardNo);

        /************************************************************************
                       函数名称: WY_ClrInterruptRisePort8
                       功能描述: 清除当前DI08端口发生的上升沿中断事件。中断事件发生后，
                                 必须用此函数清除后，才能进行下一次DIO8上升沿中断。
                       参数列表:
                         CardNo: 操作当前板卡。
                         返回值: 表示函数返状态  0:操作成功    1:板卡操作失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ClrInterruptRisePort8", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ClrInterruptRisePort8(int CardNo);

        /************************************************************************
                       函数名称: WY_ClrInterruptRisePort9
                       功能描述: 清除当前DI09端口发生的上升沿中断事件。中断事件发生后，
                                 必须用此函数清除后，才能进行下一次DIO9上升沿中断。
                       参数列表:
                         CardNo: 操作当前板卡。
                         返回值: 表示函数返状态  0:操作成功    1:板卡操作失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ClrInterruptRisePort9", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ClrInterruptRisePort9(int CardNo);

        /************************************************************************
                       函数名称: WY_ClrInterruptRisePort10
                       功能描述: 清除当前DI10端口发生的上升沿中断事件。中断事件发生后，
                                 必须用此函数清除后，才能进行下一次DI10上升沿中断。
                       参数列表:
                         CardNo: 操作当前板卡。
                         返回值: 表示函数返状态  0:操作成功    1:板卡操作失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ClrInterruptRisePort10", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ClrInterruptRisePort10(int CardNo);

        /************************************************************************
                       函数名称: WY_ClrInterruptRisePort11
                       功能描述: 清除当前DI11端口发生的上升沿中断事件。中断事件发生后，
                                 必须用此函数清除后，才能进行下一次DI11上升沿中断。
                       参数列表:
                         CardNo: 操作当前板卡。
                         返回值: 表示函数返状态  0:操作成功    1:板卡操作失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ClrInterruptRisePort11", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ClrInterruptRisePort11(int CardNo);

        /************************************************************************
                       函数名称: WY_ClrInterruptRisePort12
                       功能描述: 清除当前DI12端口发生的上升沿中断事件。中断事件发生后，
                                 必须用此函数清除后，才能进行下一次DI12上升沿中断。
                       参数列表:
                         CardNo: 操作当前板卡。
                         返回值: 表示函数返状态  0:操作成功    1:板卡操作失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ClrInterruptRisePort12", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ClrInterruptRisePort12(int CardNo);

        /************************************************************************
                       函数名称: WY_ClrInterruptRisePort13
                       功能描述: 清除当前DI13端口发生的上升沿中断事件。中断事件发生后，
                                 必须用此函数清除后，才能进行下一次DI13上升沿中断。
                       参数列表:
                         CardNo: 操作当前板卡。
                         返回值: 表示函数返状态  0:操作成功    1:板卡操作失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ClrInterruptRisePort13", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ClrInterruptRisePort13(int CardNo);

        /************************************************************************
                       函数名称: WY_ClrInterruptRisePort14
                       功能描述: 清除当前DI14端口发生的上升沿中断事件。中断事件发生后，
                                 必须用此函数清除后，才能进行下一次DI14上升沿中断。
                       参数列表:
                         CardNo: 操作当前板卡。
                         返回值: 表示函数返状态  0:操作成功    1:板卡操作失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ClrInterruptRisePort14", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ClrInterruptRisePort14(int CardNo);

        /************************************************************************
                       函数名称: WY_ClrInterruptRisePort15
                       功能描述: 清除当前DI15端口发生的上升沿中断事件。中断事件发生后，
                                 必须用此函数清除后，才能进行下一次DI15上升沿中断。
                       参数列表:
                         CardNo: 操作当前板卡。
                         返回值: 表示函数返状态  0:操作成功    1:板卡操作失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ClrInterruptRisePort15", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ClrInterruptRisePort15(int CardNo);



        //extern "C" int _declspec(dllexport) WY_ClrInterruptFallPort(int CardNo,int portbit );
        /************************************************************************
                       函数名称: WY_ClrInterruptFallPort0
                       功能描述: 清除指定DI端口发生的下降沿中断事件。中断事件发生后，
                                 必须用此函数清除后，才能进行下一次DI下降沿中断。
                       参数列表:
                         CardNo: 操作当前板卡。
                        portbit: DI端口号,0，1，2，3，4，5，6，7；
                         返回值: 表示函数返状态  0:操作成功    1:板卡操作失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ClrInterruptFallPort", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ClrInterruptFallPort(int CardNo, int portbit);

        /************************************************************************
                       函数名称: WY_ClrInterruptFallPort0
                       功能描述: 清除当前DI00端口发生的下降沿中断事件。中断事件发生后，
                                 必须用此函数清除后，才能进行下一次DIOO下降沿中断。
                       参数列表:
                         CardNo: 操作当前板卡。
                         返回值: 表示函数返状态  0:操作成功    1:板卡操作失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ClrInterruptFallPort0", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ClrInterruptFallPort0(int CardNo);

        /************************************************************************
                       函数名称: WY_ClrInterruptFallPort1
                       功能描述: 清除当前DI01端口发生的下降沿中断事件。中断事件发生后，
                                 必须用此函数清除后，才能进行下一次DIO1下降沿中断。
                       参数列表:
                         CardNo: 操作当前板卡。
                         返回值: 表示函数返状态  0:操作成功    1:板卡操作失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ClrInterruptFallPort1", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ClrInterruptFallPort1(int CardNo);

        /************************************************************************
                       函数名称: WY_ClrInterruptFallPort2
                       功能描述: 清除当前DI02端口发生的下降沿中断事件。中断事件发生后，
                                 必须用此函数清除后，才能进行下一次DIO2下降沿中断。
                       参数列表:
                         CardNo: 操作当前板卡。
                         返回值: 表示函数返状态  0:操作成功    1:板卡操作失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ClrInterruptFallPort2", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ClrInterruptFallPort2(int CardNo);

        /************************************************************************
                       函数名称: WY_ClrInterruptFallPort3
                       功能描述: 清除当前DI03端口发生的下降沿中断事件。中断事件发生后，
                                 必须用此函数清除后，才能进行下一次DIO3下降沿中断。
                       参数列表:
                         CardNo: 操作当前板卡。
                         返回值: 表示函数返状态  0:操作成功    1:板卡操作失败     5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ClrInterruptFallPort3", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ClrInterruptFallPort3(int CardNo);

        /************************************************************************
                       函数名称: WY_ClrInterruptFallPort4
                       功能描述: 清除当前DI04端口发生的下降沿中断事件。中断事件发生后，
                                 必须用此函数清除后，才能进行下一次DIO4下降沿中断。
                       参数列表:
                         CardNo: 操作当前板卡。
                         返回值: 表示函数返状态  0:操作成功    1:板卡操作失败   5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ClrInterruptFallPort4", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ClrInterruptFallPort4(int CardNo);

        /************************************************************************
                       函数名称: WY_ClrInterruptFallPort5
                       功能描述: 清除当前DI05端口发生的下降沿中断事件。中断事件发生后，
                                 必须用此函数清除后，才能进行下一次DIO5下降沿中断。
                       参数列表:
                         CardNo: 操作当前板卡。
                         返回值: 表示函数返状态  0:操作成功    1:板卡操作失败   5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ClrInterruptFallPort5", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ClrInterruptFallPort5(int CardNo);

        /************************************************************************
                       函数名称: WY_ClrInterruptFallPort6
                       功能描述: 清除当前DI06端口发生的下降沿中断事件。中断事件发生后，
                                 必须用此函数清除后，才能进行下一次DIO6下降沿中断。
                       参数列表:
                         CardNo: 操作当前板卡。
                         返回值: 表示函数返状态  0:操作成功    1:板卡操作失败   5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ClrInterruptFallPort6", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ClrInterruptFallPort6(int CardNo);

        /************************************************************************
                       函数名称: WY_ClrInterruptFallPort7
                       功能描述: 清除当前DI07端口发生的下降沿中断事件。中断事件发生后，
                                 必须用此函数清除后，才能进行下一次DIO7下降沿中断。
                       参数列表:
                         CardNo: 操作当前板卡。
                         返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ClrInterruptFallPort7", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ClrInterruptFallPort7(int CardNo);

        /************************************************************************
                      函数名称: WY_ClrInterruptFallPort8
                      功能描述: 清除当前DI08端口发生的下降沿中断事件。中断事件发生后，
                                必须用此函数清除后，才能进行下一次DIO8下降沿中断。
                      参数列表:
                        CardNo: 操作当前板卡。
                        返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
       技术支持联系电话：13510401592
       ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ClrInterruptFallPort8", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ClrInterruptFallPort8(int CardNo);

        /************************************************************************
                      函数名称: WY_ClrInterruptFallPort9
                      功能描述: 清除当前DI09端口发生的下降沿中断事件。中断事件发生后，
                                必须用此函数清除后，才能进行下一次DIO9下降沿中断。
                      参数列表:
                        CardNo: 操作当前板卡。
                        返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
       技术支持联系电话：13510401592
       ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ClrInterruptFallPort9", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ClrInterruptFallPort9(int CardNo);

        /************************************************************************
                      函数名称: WY_ClrInterruptFallPort10
                      功能描述: 清除当前DI10端口发生的下降沿中断事件。中断事件发生后，
                                必须用此函数清除后，才能进行下一次DI10下降沿中断。
                      参数列表:
                        CardNo: 操作当前板卡。
                        返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
       技术支持联系电话：13510401592
       ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ClrInterruptFallPort10", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ClrInterruptFallPort10(int CardNo);

        /************************************************************************
                      函数名称: WY_ClrInterruptFallPort11
                      功能描述: 清除当前DI11端口发生的下降沿中断事件。中断事件发生后，
                                必须用此函数清除后，才能进行下一次DI11下降沿中断。
                      参数列表:
                        CardNo: 操作当前板卡。
                        返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
       技术支持联系电话：13510401592
       ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ClrInterruptFallPort11", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ClrInterruptFallPort11(int CardNo);

        /************************************************************************
                      函数名称: WY_ClrInterruptFallPort12
                      功能描述: 清除当前DI12端口发生的下降沿中断事件。中断事件发生后，
                                必须用此函数清除后，才能进行下一次DI12下降沿中断。
                      参数列表:
                        CardNo: 操作当前板卡。
                        返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
       技术支持联系电话：13510401592
       ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ClrInterruptFallPort12", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ClrInterruptFallPort12(int CardNo);

        /************************************************************************
                      函数名称: WY_ClrInterruptFallPort13
                      功能描述: 清除当前DI13端口发生的下降沿中断事件。中断事件发生后，
                                必须用此函数清除后，才能进行下一次DI13下降沿中断。
                      参数列表:
                        CardNo: 操作当前板卡。
                        返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
       技术支持联系电话：13510401592
       ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ClrInterruptFallPort13", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ClrInterruptFallPort13(int CardNo);

        /************************************************************************
                      函数名称: WY_ClrInterruptFallPort14
                      功能描述: 清除当前DI14端口发生的下降沿中断事件。中断事件发生后，
                                必须用此函数清除后，才能进行下一次DI14下降沿中断。
                      参数列表:
                        CardNo: 操作当前板卡。
                        返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
       技术支持联系电话：13510401592
       ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ClrInterruptFallPort14", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ClrInterruptFallPort14(int CardNo);

        /************************************************************************
                      函数名称: WY_ClrInterruptFallPort15
                      功能描述: 清除当前DI15端口发生的下降沿中断事件。中断事件发生后，
                                必须用此函数清除后，才能进行下一次DI15下降沿中断。
                      参数列表:
                        CardNo: 操作当前板卡。
                        返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
       技术支持联系电话：13510401592
       ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_ClrInterruptFallPort15", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_ClrInterruptFallPort15(int CardNo);

        /************************************************************************
                       函数名称: WY_SetRiseInterruptPortEnableBit
                       功能描述: 设置DI端口其中某一个端口上升沿中断功能使能。
                       参数列表:
                         CardNo: 操作当前板卡。
                        portbit: DI端口号,0，1，2，3，4，5，6，7；
                          value: 1:中断使能打开   0：中断关闭
                         返回值: 表示函数返状态  0:操作成功    1:操作失败  5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetRiseInterruptPortEnableBit", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetRiseInterruptPortEnableBit(int CardNo, int portbit, int value);

        /************************************************************************
                       函数名称: WY_SetRiseInterruptPortEnableBit0
                       功能描述: 设置DIOO端口上升沿中断功能使能。
                       参数列表:
                         CardNo: 操作当前板卡。
                          value: 1:中断使能打开   0：中断关闭
                         返回值: 表示函数返状态  0:操作成功    1:操作失败      5:板卡连接中断   
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetRiseInterruptPortEnableBit0", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetRiseInterruptPortEnableBit0(int CardNo, int value);

        /************************************************************************
                       函数名称: WY_SetRiseInterruptPortEnableBit1
                       功能描述: 设置DIO1端口上升沿中断功能使能。
                       参数列表:
                         CardNo: 操作当前板卡。
                          value: 1:中断使能打开   0：中断关闭
                         返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetRiseInterruptPortEnableBit1", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetRiseInterruptPortEnableBit1(int CardNo, int value);

        /************************************************************************
                       函数名称: WY_SetRiseInterruptPortEnableBit2
                       功能描述: 设置DIO2端口上升沿中断功能使能。
                       参数列表:
                         CardNo: 操作当前板卡。
                          value: 1:中断使能打开   0：中断关闭
                         返回值: 表示函数返状态  0:操作成功    1:操作失败     5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetRiseInterruptPortEnableBit2", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetRiseInterruptPortEnableBit2(int CardNo, int value);

        /************************************************************************
                       函数名称: WY_SetRiseInterruptPortEnableBit3
                       功能描述: 设置DIO3端口上升沿中断功能使能。
                       参数列表:
                         CardNo: 操作当前板卡。
                          value: 1:中断使能打开   0：中断关闭
                         返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetRiseInterruptPortEnableBit3", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetRiseInterruptPortEnableBit3(int CardNo, int value);

        /************************************************************************
                       函数名称: WY_SetRiseInterruptPortEnableBit4
                       功能描述: 设置DIO4端口上升沿中断功能使能。
                       参数列表:
                         CardNo: 操作当前板卡。
                          value: 1:中断使能打开   0：中断关闭
                         返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetRiseInterruptPortEnableBit4", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetRiseInterruptPortEnableBit4(int CardNo, int value);

        /************************************************************************
                       函数名称: WY_SetRiseInterruptPortEnableBit5
                       功能描述: 设置DIO5端口上升沿中断功能使能。
                       参数列表:
                         CardNo: 操作当前板卡。
                          value: 1:中断使能打开   0：中断关闭
                         返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetRiseInterruptPortEnableBit5", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetRiseInterruptPortEnableBit5(int CardNo, int value);

        /************************************************************************
                       函数名称: WY_SetRiseInterruptPortEnableBit6
                       功能描述: 设置DIO6端口上升沿中断功能使能。
                       参数列表:
                         CardNo: 操作当前板卡。
                          value: 1:中断使能打开   0：中断关闭
                         返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetRiseInterruptPortEnableBit6", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetRiseInterruptPortEnableBit6(int CardNo, int value);

        /************************************************************************
                       函数名称: WY_SetRiseInterruptPortEnableBit7
                       功能描述: 设置DIO7端口上升沿中断功能使能。
                       参数列表:
                         CardNo: 操作当前板卡。
                          value: 1:中断使能打开   0：中断关闭
                         返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetRiseInterruptPortEnableBit7", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetRiseInterruptPortEnableBit7(int CardNo, int value);

        /************************************************************************
                      函数名称: WY_SetRiseInterruptPortEnableBit8
                      功能描述: 设置DIO8端口上升沿中断功能使能。
                      参数列表:
                        CardNo: 操作当前板卡。
                         value: 1:中断使能打开   0：中断关闭
                        返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
       技术支持联系电话：13510401592
       ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetRiseInterruptPortEnableBit8", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetRiseInterruptPortEnableBit8(int CardNo, int value);

        /************************************************************************
                      函数名称: WY_SetRiseInterruptPortEnableBit9
                      功能描述: 设置DIO9端口上升沿中断功能使能。
                      参数列表:
                        CardNo: 操作当前板卡。
                         value: 1:中断使能打开   0：中断关闭
                        返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
       技术支持联系电话：13510401592
       ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetRiseInterruptPortEnableBit9", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetRiseInterruptPortEnableBit9(int CardNo, int value);

        /************************************************************************
                      函数名称: WY_SetRiseInterruptPortEnableBit10
                      功能描述: 设置DI10端口上升沿中断功能使能。
                      参数列表:
                        CardNo: 操作当前板卡。
                         value: 1:中断使能打开   0：中断关闭
                        返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
       技术支持联系电话：13510401592
       ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetRiseInterruptPortEnableBit10", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetRiseInterruptPortEnableBit10(int CardNo, int value);

        /************************************************************************
                      函数名称: WY_SetRiseInterruptPortEnableBit11
                      功能描述: 设置DI11端口上升沿中断功能使能。
                      参数列表:
                        CardNo: 操作当前板卡。
                         value: 1:中断使能打开   0：中断关闭
                        返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
       技术支持联系电话：13510401592
       ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetRiseInterruptPortEnableBit11", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetRiseInterruptPortEnableBit11(int CardNo, int value);

        /************************************************************************
                      函数名称: WY_SetRiseInterruptPortEnableBit12
                      功能描述: 设置DI12端口上升沿中断功能使能。
                      参数列表:
                        CardNo: 操作当前板卡。
                         value: 1:中断使能打开   0：中断关闭
                        返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
       技术支持联系电话：13510401592
       ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetRiseInterruptPortEnableBit12", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetRiseInterruptPortEnableBit12(int CardNo, int value);

        /************************************************************************
                      函数名称: WY_SetRiseInterruptPortEnableBit13
                      功能描述: 设置DI13端口上升沿中断功能使能。
                      参数列表:
                        CardNo: 操作当前板卡。
                         value: 1:中断使能打开   0：中断关闭
                        返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
       技术支持联系电话：13510401592
       ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetRiseInterruptPortEnableBit13", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetRiseInterruptPortEnableBit13(int CardNo, int value);

        /************************************************************************
                      函数名称: WY_SetRiseInterruptPortEnableBit14
                      功能描述: 设置DI14端口上升沿中断功能使能。
                      参数列表:
                        CardNo: 操作当前板卡。
                         value: 1:中断使能打开   0：中断关闭
                        返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
       技术支持联系电话：13510401592
       ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetRiseInterruptPortEnableBit14", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetRiseInterruptPortEnableBit14(int CardNo, int value);

        /************************************************************************
                      函数名称: WY_SetRiseInterruptPortEnableBit15
                      功能描述: 设置DI15端口上升沿中断功能使能。
                      参数列表:
                        CardNo: 操作当前板卡。
                         value: 1:中断使能打开   0：中断关闭
                        返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
       技术支持联系电话：13510401592
       ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetRiseInterruptPortEnableBit15", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetRiseInterruptPortEnableBit15(int CardNo, int value);

        /************************************************************************
                       函数名称: WY_SetFallInterruptPortEnableBit
                       功能描述: 设置DI端口其中某一个端口的下降沿中断功能使能。
                       参数列表:
                         CardNo: 操作当前板卡。
                        portbit: DI端口号,0，1，2，3，4，5，6，7；
                          value: 1:中断使能打开   0：中断关闭
                         返回值: 表示函数返状态  0:操作成功    1:操作失败   5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetFallInterruptPortEnableBit", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetFallInterruptPortEnableBit(int CardNo, int portbit, int value);

        /************************************************************************
                       函数名称: WY_SetFallInterruptPortEnableBit0
                       功能描述: 设置DIOO端口下降沿中断功能使能。
                       参数列表:
                         CardNo: 操作当前板卡。
                          value: 1:中断使能打开   0：中断关闭
                         返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetFallInterruptPortEnableBit0", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetFallInterruptPortEnableBit0(int CardNo, int value);

        /************************************************************************
                       函数名称: WY_SetFallInterruptPortEnableBit1
                       功能描述: 设置DIO1端口下降沿中断功能使能。
                       参数列表:
                         CardNo: 操作当前板卡。
                          value: 1:中断使能打开   0：中断关闭
                         返回值: 表示函数返状态  0:操作成功    1:操作失败   5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetFallInterruptPortEnableBit1", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetFallInterruptPortEnableBit1(int CardNo, int value);

        /************************************************************************
                       函数名称: WY_SetFallInterruptPortEnableBit2
                       功能描述: 设置DIO2端口下降沿中断功能使能。
                       参数列表:
                         CardNo: 操作当前板卡。
                          value: 1:中断使能打开   0：中断关闭
                         返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetFallInterruptPortEnableBit2", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetFallInterruptPortEnableBit2(int CardNo, int value);

        /************************************************************************
                       函数名称: WY_SetFallInterruptPortEnableBit3
                       功能描述: 设置DIO3端口下降沿中断功能使能。
                       参数列表:
                         CardNo: 操作当前板卡。
                          value: 1:中断使能打开   0：中断关闭
                         返回值: 表示函数返状态  0:操作成功    1:操作失败     5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetFallInterruptPortEnableBit3", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetFallInterruptPortEnableBit3(int CardNo, int value);

        /************************************************************************
                       函数名称: WY_SetFallInterruptPortEnableBit4
                       功能描述: 设置DIO4端口下降沿中断功能使能。
                       参数列表:
                         CardNo: 操作当前板卡。
                          value: 1:中断使能打开   0：中断关闭
                         返回值: 表示函数返状态  0:操作成功    1:操作失败     5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetFallInterruptPortEnableBit4", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetFallInterruptPortEnableBit4(int CardNo, int value);

        /************************************************************************
                       函数名称: WY_SetFallInterruptPortEnableBit5
                       功能描述: 设置DIO5端口下降沿中断功能使能。
                       参数列表:
                         CardNo: 操作当前板卡。
                          value: 1:中断使能打开   0：中断关闭
                         返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetFallInterruptPortEnableBit5", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetFallInterruptPortEnableBit5(int CardNo, int value);

        /************************************************************************
                       函数名称: WY_SetFallInterruptPortEnableBit6
                       功能描述: 设置DIO6端口下降沿中断功能使能。
                       参数列表:
                         CardNo: 操作当前板卡。
                          value: 1:中断使能打开   0：中断关闭
                         返回值: 表示函数返状态  0:操作成功    1:操作失败     5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetFallInterruptPortEnableBit6", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetFallInterruptPortEnableBit6(int CardNo, int value);

        /************************************************************************
                       函数名称: WY_SetFallInterruptPortEnableBit7
                       功能描述: 设置DIO7端口下降沿中断功能使能。
                       参数列表:
                         CardNo: 操作当前板卡。
                          value: 1:中断使能打开   0：中断关闭
                         返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetFallInterruptPortEnableBit7", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetFallInterruptPortEnableBit7(int CardNo, int value);

        /************************************************************************
                       函数名称: WY_SetFallInterruptPortEnableBit8
                       功能描述: 设置DIO8端口下降沿中断功能使能。
                       参数列表:
                         CardNo: 操作当前板卡。
                          value: 1:中断使能打开   0：中断关闭
                         返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetFallInterruptPortEnableBit8", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetFallInterruptPortEnableBit8(int CardNo, int value);

        /************************************************************************
                       函数名称: WY_SetFallInterruptPortEnableBit9
                       功能描述: 设置DIO9端口下降沿中断功能使能。
                       参数列表:
                         CardNo: 操作当前板卡。
                          value: 1:中断使能打开   0：中断关闭
                         返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetFallInterruptPortEnableBit9", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetFallInterruptPortEnableBit9(int CardNo, int value);

        /************************************************************************
                       函数名称: WY_SetFallInterruptPortEnableBit10
                       功能描述: 设置DI10端口下降沿中断功能使能。
                       参数列表:
                         CardNo: 操作当前板卡。
                          value: 1:中断使能打开   0：中断关闭
                         返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetFallInterruptPortEnableBit10", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetFallInterruptPortEnableBit10(int CardNo, int value);

        /************************************************************************
                       函数名称: WY_SetFallInterruptPortEnableBit11
                       功能描述: 设置DI11端口下降沿中断功能使能。
                       参数列表:
                         CardNo: 操作当前板卡。
                          value: 1:中断使能打开   0：中断关闭
                         返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetFallInterruptPortEnableBit11", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetFallInterruptPortEnableBit11(int CardNo, int value);

        /************************************************************************
                       函数名称: WY_SetFallInterruptPortEnableBit12
                       功能描述: 设置DI12端口下降沿中断功能使能。
                       参数列表:
                         CardNo: 操作当前板卡。
                          value: 1:中断使能打开   0：中断关闭
                         返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetFallInterruptPortEnableBit12", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetFallInterruptPortEnableBit12(int CardNo, int value);

        /************************************************************************
                       函数名称: WY_SetFallInterruptPortEnableBit13
                       功能描述: 设置DI13端口下降沿中断功能使能。
                       参数列表:
                         CardNo: 操作当前板卡。
                          value: 1:中断使能打开   0：中断关闭
                         返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetFallInterruptPortEnableBit13", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetFallInterruptPortEnableBit13(int CardNo, int value);

        /************************************************************************
                       函数名称: WY_SetFallInterruptPortEnableBit14
                       功能描述: 设置DI14端口下降沿中断功能使能。
                       参数列表:
                         CardNo: 操作当前板卡。
                          value: 1:中断使能打开   0：中断关闭
                         返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetFallInterruptPortEnableBit14", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetFallInterruptPortEnableBit14(int CardNo, int value);

        /************************************************************************
                       函数名称: WY_SetFallInterruptPortEnableBit15
                       功能描述: 设置DI15端口下降沿中断功能使能。
                       参数列表:
                         CardNo: 操作当前板卡。
                          value: 1:中断使能打开   0：中断关闭
                         返回值: 表示函数返状态  0:操作成功    1:操作失败    5:板卡连接中断
        技术支持联系电话：13510401592
        ************************************************************************/
        [DllImport("WENYU_EIO32P.dll", EntryPoint = "WY_SetFallInterruptPortEnableBit15", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int WY_SetFallInterruptPortEnableBit15(int CardNo, int value);
      }
 }
