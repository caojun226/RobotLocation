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
using VisionCore.Log;

namespace RobotLocation.UI
{
    public partial class UcLogin : UIForm
    {
        public UcLogin()
        {
            InitializeComponent();
            this.Load += UcLogin_Load;
        }

        private void UcLogin_Load(object sender, EventArgs e)
        {
            uiNumPadTextBox9.PasswordChar = '*';
            cmbUser.SelectedIndex = 0;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            AppMangerTool.curIndex = -1;
            if (uiNumPadTextBox9.Text.IsNullOrEmpty()) {
                MessageBox.Show("请输入密码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            AppMangerTool.curIndex = cmbUser.SelectedIndex;
            if (uiNumPadTextBox9.Text == AppMangerTool.mSysUses[AppMangerTool.curIndex].userPWD)
            {
                this.DialogResult = DialogResult.OK;
            }
            else {
                AppMangerTool.curIndex = -1;
                MessageBox.Show("登陆失败,密码输入错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
