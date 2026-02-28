using System;
using HalconDotNet;

namespace RobotLocation.Model
{
    public class Product : IDisposable
    {
        public int Order;
        public HImage Img;
        public HTuple Res;
        public uint useTime;
        public long mposi;
        public long mposo;
        public bool Complete = false;

        private bool _disposed = false;

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // 释放托管资源
                }

                // 释放非托管资源
                if (Img != null)
                {
                    Img.Dispose();
                    Img = null;
                }
                if (Res != null)
                {
                    Res.Dispose();
                    Res = null;
                }

                _disposed = true;
            }
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~Product()
        {
            Dispose(false);
        }
    }
}
