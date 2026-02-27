using RobotLocation.Service;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RobotLocation.UI
{
    public partial class FrmBranLst : UIForm
    {

        public BrandData mBrandData { get; set; }
        public FrmBranLst()
        {
            InitializeComponent();
            this.Load += FrmBranLst_Load;
        }

        private void FrmBranLst_Load(object sender, EventArgs e)
        {
            foreach (var item in AppMangerTool.mBrandLst)
            {
                rBtnLst.Items.Add(item.BrandName);
            }
            rBtnLst.SelectedIndex = 0;
        }

        private void BtnApp_Click(object sender, EventArgs e)
        {
            int index = rBtnLst.SelectedIndex;
            AppMangerTool.mBrandLst[index].IsCurent = 1;
            (new SVBrandData()).switchBrandData();
            (new SVBrandData()).updData(AppMangerTool.mBrandLst[index]);
            mBrandData = AppMangerTool.mBrandLst[index];
            this.DialogResult = DialogResult.OK;

        }
    }
}
