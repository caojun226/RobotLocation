using cszmcaux;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotLocation.Model
{
    //连接类

    public abstract class Connect
    {
        public abstract string[] scan();
    }


    class LocalConect : Connect
    {
        public override string[] scan()
        {
            string[] stringArray = new string[] { "LOCAL1" };
            return stringArray;
        }
    }

    class EthConnect : Connect
    {
        public override string[] scan()
        {
            int num;
            string[] sArray;
            StringBuilder buffer = new StringBuilder(10240);
            string buff = "";
            zmcaux.ZAux_SearchEthlist(buffer, 10230, 200);
            buff += buffer;
            sArray = buff.Split(' ');
            num = buff.Split(' ').Length;
            sArray = sArray.Take(num - 1).ToArray();
            return sArray;
        }
    }

    class ComConnect : Connect
    {
        public override string[] scan()
        {
            return SerialPort.GetPortNames();
        }
    }

    class PciConnect : Connect
    {
        public override string[] scan()
        {
            int Card;
            Card = zmcaux.ZAux_GetMaxPciCards();
            string[] tmpstr = new string[Card];

            for (int i = 0; i < Card; i++)
            {
                tmpstr[i] = string.Format("PCI{0:D}", i + 1);
            }
            return tmpstr;
        }
    }

    public class ConnectContext
    {
        private Connect cn = null;
        private int ctype = -1;


        public ConnectContext(string type)
        {
            switch (type)
            {
                case "网口":
                    cn = new EthConnect();
                    ctype = 2;
                    break;
                case "串口":
                    cn = new ComConnect();
                    ctype = 1;
                    break;
                case "PCI":
                    cn = new PciConnect();
                    ctype = 4;
                    break;
                case "LOCAL":
                    cn = new LocalConect();
                    ctype = 5;
                    break;
                default:
                    throw new ArgumentException("Incorrect type", "type");
            }
        }

        public string[] ScanConnect()
        {
            return cn.scan();
        }

        public int OpenConnect(ref IntPtr g_handle, string Buffer)
        {
            if (g_handle != (IntPtr)0)
            {
                CloseConnect(g_handle);
            }
            return zmcaux.ZAux_FastOpen(ctype, Buffer, 1000, out g_handle);
        }

        public int CloseConnect(IntPtr g_handle)
        {
            return zmcaux.ZAux_Close(g_handle);
        }
    }

}
