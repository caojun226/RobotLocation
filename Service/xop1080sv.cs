using cszmcaux;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VisionCore.Ext;
using VisionCore.Log;
using System.IO.Ports;
using System.Windows.Forms;
using VisionCore.Communication;
using System.IO;
using System.Configuration;

namespace RobotLocation.Service
{
    public class xop1080sv
    {
        public readonly int _checkInterval = 20000; // 检查间隔（毫秒）
        public CancellationTokenSource _cancellationTokenSource;
        public static IntPtr g_handlea;         //链接返回的句柄，界面服务用       
        public static IntPtr handle_null = (IntPtr)0; //空句柄，表示无控制卡连接
        public static SerialPort serialPort;
        public xop1080sv()
        {
            OpenMC();//带返回值的，可用于故障
            //MCSetM();
            openport();
        }

        public static void openport()
        {
            serialPort = new SerialPort();
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
            else
            {
                try
                {
                    // 从配置文件读取参数，文件位置RobotLocation.exe.config
                    serialPort.PortName = ConfigurationManager.AppSettings["PortName"];
                    serialPort.BaudRate = int.Parse(ConfigurationManager.AppSettings["BaudRate"]);
                    serialPort.DataBits = int.Parse(ConfigurationManager.AppSettings["DataBits"]);
                    serialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), ConfigurationManager.AppSettings["StopBits"]);
                    serialPort.Parity = (Parity)Enum.Parse(typeof(Parity), ConfigurationManager.AppSettings["Parity"]);
                    serialPort.Handshake = (Handshake)Enum.Parse(typeof(Handshake), ConfigurationManager.AppSettings["Handshake"]);

                    serialPort.Open();
                    LogNet.Info("串口通讯正常");
                    //serialPort.PortName = "COM15";
                    //serialPort.BaudRate = 38400;// 115200;
                    //serialPort.DataBits = 8;
                    //serialPort.StopBits = StopBits.Two;
                    //serialPort.Parity = Parity.None;
                    //serialPort.Handshake = Handshake.None;
                    //serialPort.Open();
                    //LogNet.Info("串口通讯正常");
                }
                catch (Exception ex)
                {
                    LogNet.Info("串口通讯异常"+ ex.ToString()); ;
                }
            }
        }
        private const byte SlaveAddress = 0x01; // 设备地址
        private const ushort RegisterAddress = 0x6203; // 寄存器地址
        public static string GenerateSpeedSetCommand(ushort speedRPM)
        {
            // 功能码：06（写单个寄存器）
            byte functionCode = 0x06;

            // 将速度值转换为两个字节（大端序）
            byte highByte = (byte)(speedRPM >> 8);
            byte lowByte = (byte)(speedRPM & 0xFF);

            // 构造数据帧
            byte[] dataFrame = new byte[]
            {
            SlaveAddress,
            functionCode,
            (byte)(RegisterAddress >> 8),
            (byte)(RegisterAddress & 0xFF),
            highByte,
            lowByte
            };

            // 计算CRC校验
            ushort crc = CalculateCRC16(dataFrame);
            byte[] crcBytes = BitConverter.GetBytes(crc);

            // 构造完整的Modbus RTU指令
            StringBuilder commandBuilder = new StringBuilder();
            foreach (byte b in dataFrame)
            {
                commandBuilder.AppendFormat("{0:X2} ", b);
            }
            commandBuilder.AppendFormat("{0:X2} {1:X2}", crcBytes[0], crcBytes[1]);

            return commandBuilder.ToString().Trim();
        }

        private static ushort CalculateCRC16(byte[] data)
        {
            ushort crc = 0xFFFF;
            foreach (byte b in data)
            {
                crc ^= b;
                for (int i = 0; i < 8; i++)
                {
                    if ((crc & 0x0001) != 0)
                    {
                        crc >>= 1;
                        crc ^= 0xA001;
                    }
                    else
                    {
                        crc >>= 1;
                    }
                }
            }
            return crc;
        }
        public static void chdirM0(int dir)
        {
            if (dir == 0)
            {
                string[] commands = new string[]
                {
                    "01 06 00 07 00 00 38 0B"
                };
                try
                {
                    senddata(commands);
                }
                catch (Exception ex)
                {
                    LogNet.Info($"发生错误：{ex.Message}");
                }
            }
            else
            {
                string[] commands = new string[]
                {
                    "01 06 00 07 00 01 F9 CB"
                };
                try
                {
                    senddata(commands);
                }
                catch (Exception ex)
                {
                    LogNet.Info($"发生错误：{ex.Message}");
                }

            }
            
            
        }
        /// <summary>
        /// 启动主电机
        /// </summary>
        public static void startM0()
        {
            string commandspeed = GenerateSpeedSetCommand(40);//默认40
            //LogNet.Info(commandspeed.ToString());
            string[] commands = new string[]
            {
                "01 06 62 00 00 02 17 B3",
                //commandspeed.ToString(),//速度由电机自己保存，停机时候设置
                "01 06 60 02 00 10 37 C6"
            };

            try
            {                
               senddata(commands);
            }
            catch (Exception ex)
            {
                LogNet.Info($"发生错误：{ex.Message}");
            }
        }
        /// <summary>
        /// 改变主电机速度。
        /// </summary>
        /// <param name="speed">速度值。</param>
        /// <param name="data">要设置的值。</param>
        public static void chspeedM0(ushort speed)
        {
            string commandspeed = GenerateSpeedSetCommand(speed);
            //LogNet.Info(commandspeed.ToString());
            string[] commands = new string[]
            {
                commandspeed.ToString()
                //"01 06 60 02 00 10 37 C6"
            };

            try
            {
                senddata(commands);
            }
            catch (Exception ex)
            {
                LogNet.Info($"发生错误：{ex.Message}");
            }
        }
        /// <summary>
        /// 停止主电机
        /// </summary>
        public static void stopM0()
        {
            string[] commands = new string[]
            {
                "01 06 60 02 00 40 37 FA"
            };
            try
            {
                senddata(commands);
            }
            catch (Exception ex)
            {
                LogNet.Info($"发生错误：{ex.Message}");
            }
        }

        public static void senddata(string[] commands, int delay = 100)
        {
            if (!serialPort.IsOpen)
            {
                throw new InvalidOperationException("串口未打开！");
            }
            foreach (string command in commands)
            {
                try
                {
                    byte[] commandBytes = HexStringToByteArray(command);
                    serialPort.Write(commandBytes, 0, commandBytes.Length);
                }
                catch (IOException ex)
                {
                    LogNet.Info($"发生I/O错误：{ex.Message}");
                }
                catch (Exception ex)
                {
                    LogNet.Info($"发生错误：{ex.Message}");
                }
                Thread.Sleep(delay);
            }           
            //LogNet.Info("电机已启动！");
        }

        private static byte[] HexStringToByteArray(string hex)
        {
            hex = hex.Replace(" ", ""); // 去掉空格
            return Enumerable.Range(0, hex.Length)
                              .Where(x => x % 2 == 0)
                              .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                              .ToArray();
        }
        /// <summary>
        /// 停止串口。
        /// </summary>
        public static void Close()
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }
        /// <summary>
        /// 打开界面连接的运动控制卡。
        /// </summary>
        public static bool OpenMC()
        {
            string Buffer;
            Buffer = "LOCAL1";
            if (g_handlea != handle_null)
            {
                zmcaux.ZAux_Close(g_handlea);
                g_handlea = handle_null;
            }
            int iresult = zmcaux.ZAux_FastOpen(5, Buffer, 1000, out g_handlea);//打开控制卡

            if (iresult != 0)
            {
                g_handlea = (IntPtr)0;
                LogNet.Error("界面控制卡打开失败！");
                return false;
            }
            else
            {
                //LogNet.Info("界面控制卡连接成功！");
                return true;
            }
        }

        public static void MCSetM(UInt16 start, UInt16 inum, byte on)
        {
            byte[] data = new byte[1];
            data[0] = on;
            int res = zmcaux.ZAux_Modbus_Set0x(g_handlea, start, inum, data);             
        }

        public static string MCGetM(UInt16 start)
        {
            byte[] data = new byte[1];
            data[0] = 0;
            int res = zmcaux.ZAux_Modbus_Get0x(g_handlea, start, 1, data);
            string dd = data[0].ToString();
            return dd;
        }
        /// <summary>
        /// 设置用户变量的值。
        /// </summary>
        /// <param name="name">变量名称。</param>
        /// <param name="data">要设置的值。</param>
        public static void setdata(string name,float data)
        {
            zmcaux.ZAux_Direct_SetUserVar(g_handlea, name, data);
        }
        /// <summary>
        /// 读取用户变量的值。
        /// </summary>
        /// <param name="name">变量名称。</param>
        /// <param name="data">要读取的值。</param>
        public static string getdata(string name)
        {
            float data=0;
            zmcaux.ZAux_Direct_GetUserVar(g_handlea, name, ref data);
            return data.ToString();
        }
    }
}
