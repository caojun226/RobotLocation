using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotLocation.Model
{
    public class DTO
    {
        public string ReportTime { get; set; } 
        public int QualifiedCount { get; set; }   // 合格
        public int RejectCount { get; set; }   // 剔除
        public double PassCount { get; set; }   // 合格率

        public double AvgDiameter { get; set; }   // 粒径均值
        public double MaxDiameter { get; set; }
        public double MinDiameter { get; set; }

        public double AvgRoundness { get; set; }//圆度
        public double MaxRoundness { get; set; }
        public double MinRoundness { get; set; }

        public double AvgStdDev { get; set; }//标准差
        public double MaxStdDev { get; set; }
        public double MinStdDev { get; set; }

        public double AvgLongAxis { get; set; }//长轴
        public double MaxLongAxis { get; set; }
        public double MinLongAxis { get; set; }

        public double AvgShortAxis { get; set; }//短轴
        public double MaxShortAxis { get; set; }
        public double MinShortAxis { get; set; }
        public string type { get; set; }//检测牌号
        public double DetectDiameter { get; set; }//检测直径
        public double DiameterBias { get; set; }//检测偏差
        public double DetectRoundness { get; set; }//检测圆度
        public double ColorThreshold { get; set; }//色差阈值      
        // 缺陷计数
        public int DefectSkin { get; set; }  // 皮帽
        public int DefectDirty { get; set; }//脏污
        public int DefectColor { get; set; }//异色
        public int DefectConcave { get; set; }//凹陷
        public int DefectConvex { get; set; }//凸点
        public int DefectBubble { get; set; }//气泡
        public int DefectSolid { get; set; }//黑点    
        public int DefectShape { get; set; }//黑边
        public int DefectAlien { get; set; }//异形
        public int DefectSize { get; set; }//尺寸
        public int DefectRound { get; set; }//圆度
        public int DefectChromatic { get; set; }//色差    

        public int TotalDefects =>
            DefectSkin + DefectDirty + DefectColor + DefectConcave +
            DefectConvex + DefectBubble + DefectSolid + DefectShape + DefectAlien+
            DefectSize + DefectRound + DefectChromatic;
    }
}
