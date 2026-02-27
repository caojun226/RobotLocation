using RobotLocation.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionCore.Communication;
using VisionCore.Log;

namespace RobotLocation.UI
{
    public static class AppMangerTool
    {
       public static List<sys_user> mSysUses = new List<sys_user>();
       public static List<plcData> mPlcData = new List<plcData>();
       public static List<BrandData> mBrandLst = new List<BrandData>();
       public static int curIndex = -1;
       public static int plcIndex = -1;
       public static int mCunBrandID = 0;

    }
}
