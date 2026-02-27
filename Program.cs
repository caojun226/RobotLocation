using LittleCommon.Tool;
using RobotLocation.UI;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionCore.Ext;

namespace RobotLocation
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (ProcessUtils.IsExitProcess())
            {
                UIPage page = new UIPage();
                page.ShowWarningDialog("已经打开一个程序");                
            }
            else
            {
                UIPage page = new UIPage();
                page.ShowWaitForm("正在启动，请稍后……");
                ExtHandler.Init();
                //Application.Run(new MFrm());
                Application.Run(new xop1080());
                page.Close();                
            }
        }
    }
}
