using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotLocation.Model
{
    public class RemoveParameters
    {


        private static readonly Lazy<RemoveParameters> _instance =
        new Lazy<RemoveParameters>(() => new RemoveParameters());
        public static RemoveParameters Instance => _instance.Value;
        /// <summary>
        /// 剔除步数
        /// </summary>
        public int removeStep1 { get; set; }
        public int removeStep2 { get; set; }
        public int removeStep3 { get; set; }
        public int removeStep4 { get; set; }
        public int removeStep5 { get; set; }
        public int removeStep6 { get; set; }

        /// <summary>
        /// 吹气时间
        /// </summary>
        public int jetTime1 { get; set; }
        public int jetTime2 { get; set; }

        /// <summary>
        /// 吹气偏移量
        /// </summary>
        public int jetOffset1 { get; set; }
        public int jetOffset2 { get; set; }

    }
}
