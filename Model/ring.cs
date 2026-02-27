using LevelDB;
using Sunny.UI;
using System;
using System.Linq;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;
using VisionCore.Log;
using static RobotLocation.Model.ring;

namespace RobotLocation.Model
{
    /// <summary>
    /// 基础数据结构：顺时针 1→2→3→出口，每中断右移1排，
    /// 1/2/3相机都能改状态，出口只读当前排8点。
    /// </summary>
    public sealed class ring
    {
        #region ============= 可注入参数 =============
        public int RowsPerTurn { get; }   // 一圈排数
        public int PulsePerRow { get; }   // 每排脉冲数
        public int Tolerance { get; }   // ±脉冲容忍
        public int CameraSpan { get; }   // 每相机写几连排（4）
        #endregion

        #region ============= 内部状态 =============
        private readonly RowData[] _ring;          // 200排数据
        private readonly object[] _locks;          // 每排一把锁         
        #endregion

        #region ============= 数据结构（基础版） ============
        public sealed class RowData
        {
            public long Enc = 0;
            public long Number = 0;
            public byte[] States = new byte[8];   // 已实例化
            public int Version = 0;
            public long wid = 0;
            public bool isnull = true;
        }
        #endregion
        #region ============= 构造（仅字段初始化） ============
        public ring(int rows = 200, int pulsePerRow = 16, int tolerance = 2, int cameraSpan = 4, int queuePower = 4)
        {
            RowsPerTurn = rows;
            PulsePerRow = pulsePerRow;
            Tolerance = tolerance;
            CameraSpan = cameraSpan;
            _ring = new RowData[rows];
            _locks = new object[rows];
            for (int i = 0; i < rows; i++)
            {
                _locks[i] = new object();
                _ring[i] = new RowData();   // 实例化每一排
            }
        }
        #endregion        
        /// <summary>读任意一排状态</summary>
        public (int row, long Number, byte[] states, long enc, int version,long wid,bool isnull) ReadRow(int row)
        {
            byte[] nulldata = new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 };            
            //row = (row+RowsPerTurn)% RowsPerTurn;//负数处理
            lock (_locks[row])
            {
                var d = _ring[row];
                byte[] res = (byte[])d.States.Clone();
                d.States = nulldata;
                return (row, d.Number, res, d.Enc, d.Version, d.wid, d.isnull); // 返回副本
            }
        }
        /// <summary>读编码器值</summary>
        public (int row, long enc) ReadEnc(int row)
        {           
            row = (row + RowsPerTurn) % RowsPerTurn;//负数处理
            lock (_locks[row])
            {
                var d = _ring[row];                
                return (row, d.Enc); 
            }
        }
        public int Readrow(long enc)
        {
            int temprow=0;
            for (int i = 0;i< RowsPerTurn;i++)
            {
                lock (_locks[i])
                {
                    var d = _ring[i];
                    if (d.Enc == enc)
                    {
                        temprow = i;
                        break;
                    }
                    else
                    {
                        temprow = -1;
                    }
                }                
            }
            return temprow;
        }
        //读取输出排，并刷入默认数据0
        public (int row, long Number, byte[] states, long enc, int version,long wid,bool isnull) ReadRowOut(int row)
        {
            byte[] nulldata = new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
            //if (row < 0)
            //{
            //    return (0, 0, nulldata, 0, 0,0);
            //}
            row = (row+RowsPerTurn)% RowsPerTurn;//负数处理
            lock (_locks[row])
            {
                var d = _ring[row];
                d.Version = 0;
                byte[] res = (byte[])d.States.Clone();
                d.States = nulldata;
                return (row, d.Number, res, d.Enc, d.Version,d.wid,d.isnull); // 返回副本
            }
        }
        public void Clear()
        {
            byte[] nulldata = new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 0; i < RowsPerTurn; i++)
            {
                lock (_locks[i])
                {
                    var d = _ring[i];
                    d.Version = 0;                    
                    d.States = nulldata;
                    d.Enc = 0;
                    d.Number = 0;        
                    d.wid = 0;
                    d.isnull = true;
                }
            }            
        }
        /// <summary>行号写一排,用于新建</summary>
        public void WriteByRow(int row, int number, long enc, byte[] states)
        {
            if (row < 0)
            {
                return;
            }
            row %= RowsPerTurn;
            lock (_locks[row])
            {
                var d = _ring[row];
                d.Number = number;
                d.Enc = enc;
                d.States = states != null ? (byte[])states.Clone() : new byte[8]; // 克隆传入的数组
                //d.Version = 0;//取没取标记               
            }
        }
        /// <summary>行号写4排</summary>
        public void WriteBy4Row(int row, byte[] states)
        {
            if (states == null || states.Length < 32)
            {
                LogNet.Info("值不满足条件退出");
                return;
            }           
            byte[] parts = new byte[8];            
            for (int i = 0; i < 4; i++)
            {
                row = (row + RowsPerTurn) % RowsPerTurn;//加一圈的排数防止负数
                for (int j = 0; j < 8; j++)
                {
                    parts[j] = states[i * 8 + j];
                }
                lock (_locks[row])
                {
                    var d = _ring[row];
                    //d.States = (byte[])parts.Clone(); // 克隆数组避免引用共享                    
                    for (int j = 0; j < 8; j++)
                    {
                        d.States[j] |= parts[j]; // 位OR操作
                    }                                      
                    d.Version++; // 升级版本
                    //d.wid=number;
                }
                row++;
            }
        }        
        //输出口前全标记剔除
        public void WriteOutRow(int number,int num)//起始排号，终点排号
        {            
            int row = 0;
            byte[] parts = new byte[8] {1,1,1,1,1,1,1,1};
            while(number>num)
            {
                int rowstart = (number + RowsPerTurn) % RowsPerTurn;
                int rowstop = (num + RowsPerTurn) % RowsPerTurn;
                lock (_locks[(number + RowsPerTurn) % RowsPerTurn])
                {
                    var d = _ring[(number + RowsPerTurn) % RowsPerTurn];
                    //d.States = (byte[])parts.Clone(); // 克隆数组避免引用共享                    
                    for (int j = 0; j < 8; j++)
                    {
                        d.States[j] |= parts[j]; // 位OR操作
                    }
                }
                number--;
            }            
        }
        //单排标记是否为空，区分空的1和剔除的1
        public void WriteAllNull(int number, bool isnull)
        {
            int row = 0;
            row= (number + RowsPerTurn) % RowsPerTurn;
            lock (_locks[row])
            {
                var d = _ring[row];
                d.isnull=isnull; 
            }
        }
    }
}