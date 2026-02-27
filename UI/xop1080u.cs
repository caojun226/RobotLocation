using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sunny.UI;
using VisionCore.Component;
using VisionCore.Ext;
using VisionCore.Communication;
using VisionCore.Log;
using RobotLocation.Model;
using RobotLocation.Service;

namespace RobotLocation.UI
{
    internal class xop1080u
    {
        public VisionCore.Component.LogView logview;
        public ImageControl.ViewPanel imageControl;
        public Sunny.UI.UIBarChart uiBarChart;
        public Sunny.UI.UIPieChart uiPieChart;
        public Sunny.UI.UIDoughnutChart uiDoughnutChart;
        public Sunny.UI.UILabel uiLabel;
        private xop1080m xop1080model = new xop1080m();
        public xop1080u()
        {           
            this.logview = new VisionCore.Component.LogView();
            this.logint();            
            this.imageControl = new ImageControl.ViewPanel();
            this.imgint();
            this.uiBarChart = new Sunny.UI.UIBarChart();
            this.BarChartShow();
            this.uiPieChart = new Sunny.UI.UIPieChart();
            this.PieChartShow();
            this.uiDoughnutChart = new Sunny.UI.UIDoughnutChart();
            this.uiLabel = new Sunny.UI.UILabel();
            this.reuidata();
            this.uiDoughnutChartShow();

        }
        public void logint()
        {
            logview.Dock = DockStyle.Fill;
            return;
        }
        public void imgint()
        {
            imageControl.Dock = DockStyle.Fill;
            return;
        }        

        public void BarChartShow()
        {
            uiBarChart.Dock = DockStyle.Fill;
            UIBarOption option = new UIBarOption();

            // 标题配置
            option.Title = new UITitle { Text = " " };

            // 图例配置
            option.Legend = new UILegend
            {
                Orient = UIOrient.Horizontal,
                Top = UITopAlignment.Top,
                Left = UILeftAlignment.Left
            };

            // 数据系列配置
            var series = new UIBarSeries { Name = "尺寸参数", DecimalPlaces = 3 };

            // 定义数据项
            var dataItems = new[]
            {
                new { Key = "粒径", Label = "直径", Color = Color.IndianRed },
                new { Key = "长轴", Label = "长轴", Color = Color.Green },
                new { Key = "短轴", Label = "短轴", Color = Color.CadetBlue },
                new { Key = "圆度值", Label = "圆度", Color = Color.SandyBrown }
            };

            foreach (var item in dataItems)
            {
                if (xop1080model.vars.TryGetValue(item.Key, out var valueStr) &&
                    double.TryParse(valueStr, out var value))
                {
                    series.AddData(value, item.Color);
                    option.Legend.AddData(item.Label, item.Color);
                    option.XAxis.Data.Add(item.Label);
                }
                else
                {
                    // 处理数据无效的情况
                    series.AddData(0, item.Color);
                    option.Legend.AddData(item.Label, item.Color);
                    option.XAxis.Data.Add(item.Label);
                }
            }

            option.Series.Add(series);

            // 其他配置
            option.ToolTip.Visible = true;
            option.YAxis.Scale = true;
            option.YAxis.AxisLabel.DecimalPlaces = 2;
            option.XAxis.AxisLabel.Angle = 60;
            option.ToolTip.AxisPointer.Type = UIAxisPointerType.Shadow;
            option.ShowValue = true;

            uiBarChart.SetOption(option);
        }
        public void redatashow()
        {
            xop1080model.getvars();
            this.BarChartShow();
            this.reuiDoughnutChartShow();
            this.reuidata();
        }

        public void PieChartShow()
        {
            uiPieChart.Dock = DockStyle.Fill;
            var option = new UIPieOption();

            //设置Title
            option.Title = new UITitle();
            option.Title.Text = "缺陷统计";
            //option.Title.SubText = "%";
            option.Title.Left = UILeftAlignment.Center;

            //设置ToolTip
            option.ToolTip.Visible = true;

            //设置Legend
            option.Legend = new UILegend();
            option.Legend.Orient = UIOrient.Vertical;
            option.Legend.Top = UITopAlignment.Top;
            option.Legend.Left = UILeftAlignment.Left;

            option.Legend.AddData("皮帽");
            option.Legend.AddData("脏污");
            option.Legend.AddData("异色");
            option.Legend.AddData("凹陷");
            option.Legend.AddData("凸点");
            option.Legend.AddData("气泡");
            option.Legend.AddData("实心");
            option.Legend.AddData("异形");
            option.Legend.AddData("尺寸");
            option.Legend.AddData("圆度");
            option.Legend.AddData("色差");

            //设置Series
            var series = new UIPieSeries();
            series.Name = "缺陷";
            series.Center = new UICenter(50, 55);
            series.Radius = 70;
            //series.Label.Show = true;

            //增加数据
            series.AddData("带皮帽", 10.000);
            series.AddData("脏污", 10.000);
            series.AddData("异色", 10);
            series.AddData("凹陷", 10);
            series.AddData("凸点", 10);
            series.AddData("气泡", 10);
            series.AddData("实心", 10);
            series.AddData("异形", 10);
            series.AddData("尺寸", 10);
            series.AddData("圆度", 5);
            series.AddData("色差", 5);

            //增加Series
            option.Series.Clear();
            option.Series.Add(series);

            //显示数据小数位数
            option.DecimalPlaces = 3;

            //设置Option
            uiPieChart.SetOption(option);
            return;
        }
        public void uiDoughnutChartShow()
        {
            uiDoughnutChart.Dock = DockStyle.Fill;
            var option = new UIDoughnutOption();

            //设置Title
            option.Title = new UITitle();
            option.Title.Text = "缺陷统计";
            //option.Title.SubText = "%";
            option.Title.Left = UILeftAlignment.Center;

            //设置ToolTip
            option.ToolTip.Visible = true;

            //设置Legend
            option.Legend = new UILegend();
            option.Legend.Orient = UIOrient.Vertical;
            option.Legend.Top = UITopAlignment.Top;
            option.Legend.Left = UILeftAlignment.Left;

            option.Legend.AddData("皮帽");
            option.Legend.AddData("脏污");
            option.Legend.AddData("异色");
            option.Legend.AddData("凹陷");
            option.Legend.AddData("凸点");
            option.Legend.AddData("气泡");
            option.Legend.AddData("实心");
            option.Legend.AddData("异形");
            option.Legend.AddData("尺寸");
            option.Legend.AddData("圆度");
            option.Legend.AddData("色差");

            //设置Series
            var series = new UIDoughnutSeries();
            series.Name = "缺陷";
            series.Center = new UICenter(50, 55);
            series.Radius.Inner = 40;
            series.Radius.Outer = 70;
            series.Label.Show = true;
            series.Label.Position = UIPieSeriesLabelPosition.Center;

            //增加数据
            //Math.Round((float)ExtHandler.GetGlobalVar<Int32>("r_001") / (float)ExtHandler.GetGlobalVar<Int32>("r_ng") * 100, 3)
                
            //series.AddData("带皮帽",Math.Round((float.Parse(xop1080model.vars["带皮帽"]) / float.Parse(xop1080model.vars["NG"])) * 100, 3));
            series.AddData("带皮帽", 10.000);
            series.AddData("脏污", 10.000);
            series.AddData("异色", 10);
            series.AddData("凹陷", 10);
            series.AddData("凸点", 10);
            series.AddData("气泡", 10);
            series.AddData("实心", 10);
            series.AddData("异形", 10);
            series.AddData("尺寸", 10);
            series.AddData("圆度", 5);
            series.AddData("色差", 5);

            //增加Series
            option.Series.Clear();
            option.Series.Add(series);

            //显示数据小数位数
            option.DecimalPlaces = 3;

            //设置Option
            uiDoughnutChart.SetOption(option);
            return;
        }
        public void reuiDoughnutChartShow()
        {
            uiDoughnutChart.Dock = DockStyle.Fill;
            var option = new UIDoughnutOption();

            //设置Title
            option.Title = new UITitle();
            option.Title.Text = "缺陷统计";
            //option.Title.SubText = "%";
            option.Title.Left = UILeftAlignment.Center;

            //设置ToolTip
            option.ToolTip.Visible = true;

            //设置Legend
            option.Legend = new UILegend();
            option.Legend.Orient = UIOrient.Vertical;
            option.Legend.Top = UITopAlignment.Top;
            option.Legend.Left = UILeftAlignment.Left;

            option.Legend.AddData("皮帽");
            option.Legend.AddData("脏污");
            option.Legend.AddData("异色");
            option.Legend.AddData("凹陷");
            option.Legend.AddData("凸点");
            option.Legend.AddData("气泡");
            option.Legend.AddData("实心");
            option.Legend.AddData("异形");
            option.Legend.AddData("尺寸");
            option.Legend.AddData("圆度");
            option.Legend.AddData("色差");

            //设置Series
            var series = new UIDoughnutSeries();
            series.Name = "缺陷";
            series.Center = new UICenter(50, 55);
            series.Radius.Inner = 40;
            series.Radius.Outer = 70;
            series.Label.Show = true;
            series.Label.Position = UIPieSeriesLabelPosition.Center;            

            series.AddData("带皮帽",Math.Round(float.Parse(xop1080model.vars["皮帽"])));
            series.AddData("脏污", Math.Round(float.Parse(xop1080model.vars["脏污"])));
            series.AddData("异色", Math.Round(float.Parse(xop1080model.vars["异色"])));
            series.AddData("凹陷", Math.Round(float.Parse(xop1080model.vars["凹陷"])));
            series.AddData("凸点", Math.Round(float.Parse(xop1080model.vars["凸点"])));
            series.AddData("气泡", Math.Round(float.Parse(xop1080model.vars["气泡"])));
            series.AddData("实心", Math.Round(float.Parse(xop1080model.vars["实心"])));
            series.AddData("异形", Math.Round(float.Parse(xop1080model.vars["异形"])));
            series.AddData("尺寸", Math.Round(float.Parse(xop1080model.vars["尺寸"])));
            series.AddData("圆度", Math.Round(float.Parse(xop1080model.vars["圆度"])));
            series.AddData("色差", Math.Round(float.Parse(xop1080model.vars["色差"])));

            //增加Series
            option.Series.Clear();
            option.Series.Add(series);

            //显示数据小数位数
            option.DecimalPlaces = 3;

            //设置Option
            uiDoughnutChart.SetOption(option);
            return;
        }
        //检测参数显示
        public void reuidata()
        {
            uiLabel.Dock = DockStyle.Fill;
            uiLabel.Width = 250;
            uiLabel.Height = 40;
            FontStyle currentStyle = uiLabel.Font.Style; // 获取当前字体样式
            uiLabel.Font = new Font(uiLabel.Font.FontFamily, 14, currentStyle);
            this.uiLabel.Text = "直径："
                + xop1080model.vars["粒径标准"]
                + "±" + xop1080model.vars["公差"]
                + " 圆度：" + xop1080model.vars["圆度标准"]
                + " 色差" + xop1080model.vars["色差标准"];
        }
        //电机启动公用方法
        public void runmt(ushort mtid, bool isStart,bool way)
        {
            ushort wayid = 0;
            if (way)
            {
                wayid = (ushort)(mtid + 1);
            }
            else
            {
                wayid=mtid; 
            }
            if (isStart)
            {
                xop1080sv.MCSetM(wayid, 1, 1);
            }
            else if (!isStart)
            {
                xop1080sv.MCSetM(wayid, 1, 0);
            }
        }
    }
}
