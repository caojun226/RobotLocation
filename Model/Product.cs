using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotLocation.Model
{
    public class Product
    {
        public int Order { get; set; }

        public bool Complete { get; set; }
        public HObject Img { get; set; }
        public  HTuple Res { get; set; }
        public uint useTime {  get; set; }
        public long mposi { get; set; }
        public long mposo { get; set; }

    }
}
