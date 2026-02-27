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
    public partial class FrmBrandSet : UIForm
    {
        private List<BrandData> mLst;
        public BrandData mBrandData;
        public FrmBrandSet(List<BrandData> lst, BrandData dataItem )
        {
            mLst = lst;
            mBrandData = dataItem;
            InitializeComponent();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            string brandName= txtBrandName.Text;
            if (string.IsNullOrEmpty(brandName))
            {
                this.ShowWarningDialog("品牌名称不能为空");
                return;
            }
            int count= mLst.Where(p => p.BrandName == brandName).Count();
            if (count>0) {
                this.ShowWarningDialog("品牌名称已经存在");
                return;
            }
            mBrandData.BrandName = brandName;
            if (mBrandData.ID > 0)
            {
                (new SVBrandData()).updData(mBrandData);
            }
            else {
                mBrandData.ID=(new SVBrandData()).insertData(mBrandData);
            }
            this.DialogResult = DialogResult.OK;
        }
    }
}
