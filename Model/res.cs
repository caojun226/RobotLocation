using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotLocation.Model
{
    public class res
    {
        public int resid { get; set; }
        public HTuple Res { get; set; }
        public uint userTime { get; set; }
        public long mposi { get; set; }
        public long mposo { get; set; }

    }
}