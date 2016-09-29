﻿using System;
using System.Windows.Forms;

namespace AnonManagementSystem
{
    public partial class MdiParent : Form
    {
        public MdiParent()
        {
            InitializeComponent();
        }

        private void LoginOnSucess(bool enableedit)
        {
            _islogin = true;
            tsbAdd.Enabled = _enableedit = enableedit;
            MessageBox.Show(@"登陆成功", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool _enableedit;
        private bool _islogin;
        
        private void EquipMenu_Click(object sender, EventArgs e)
        {
            if (_islogin)
            {
                foreach (var childForm in MdiChildren)
                {
                    if (childForm.Tag.ToString() == "Equipment")
                    {
                        childForm.Activate();
                        return;
                    }
                }
                EquipMainForm subMainForm = new EquipMainForm { MdiParent = this, Enableedit = _enableedit, Tag = "Equipment" };
                if (WindowState == FormWindowState.Maximized)
                {
                    subMainForm.WindowState = FormWindowState.Maximized;
                }
                subMainForm.Show();
            }
            else
            {
                LoginForm loginForm = new LoginForm();
                loginForm.ShowDialog();
                _islogin = loginForm.DialogResult == DialogResult.OK;
            }
        }

        private void MDIParent_Shown(object sender, EventArgs e)
        {
#if DEBUG
            {
                LoginOnSucess(true);
            }
#else
            {
                LoginForm loginForm = new LoginForm();
                loginForm.LoginSucess += LoginOnSucess;
                loginForm.ShowDialog();
            }
#endif
        }

        private void ChangeUserMenu_Click(object sender, EventArgs e)
        {
            _islogin = false;
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
            LoginForm loginForm = new LoginForm();
            loginForm.LoginSucess += LoginOnSucess;
            loginForm.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AmsAboutBox amsAboutBox = new AmsAboutBox();
            amsAboutBox.ShowDialog();
        }

        private void tsbAdd_Click(object sender, EventArgs e)
        {
            Form form = ActiveMdiChild;
            var mdiFunction = (IMdiFunction)form;
            mdiFunction?.DataAdd();
        }

        private void SparePartsMenu_Click(object sender, EventArgs e)
        {
            if (_islogin)
            {
                foreach (var childForm in MdiChildren)
                {
                    if (childForm.Tag.ToString() == "SpareParts")
                    {
                        childForm.Activate();
                        return;
                    }
                }
                SparePartsForm sparePartsForm = new SparePartsForm { MdiParent = this, Enableedit = _enableedit, Tag = "SpareParts" };
                if (WindowState == FormWindowState.Maximized)
                {
                    sparePartsForm.WindowState = FormWindowState.Maximized;
                }
                sparePartsForm.Show();
            }
            else
            {
                LoginForm loginForm = new LoginForm();
                loginForm.ShowDialog();
                _islogin = loginForm.DialogResult == DialogResult.OK;
            }
        }

        private void SettingMenu_Click(object sender, EventArgs e)
        {
            SystemSetting sysSetForm = new SystemSetting();
            sysSetForm.ShowDialog();
        }

        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            Form form = ActiveMdiChild;
            var mdiFunction = (IMdiFunction)form;
            mdiFunction?.DataRefresh();
        }

        private void MDIParent_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                foreach (var childForm in MdiChildren)
                {
                    childForm.WindowState = FormWindowState.Maximized;
                }
            }
            BackgroundImageLayout = ImageLayout.Zoom;
            OnBackgroundImageLayoutChanged(e);
        }

        private void tsbExportExcel_Click(object sender, EventArgs e)
        {
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            Form form = ActiveMdiChild;
            var mdiFunction = (IMdiFunction)form;
            mdiFunction?.DataDelete();
        }

        private void 导入数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DataHandle dataProcessHandle = new DataHandle();
                    dataProcessHandle.Dirpath = fbd.SelectedPath;
                    dataProcessHandle.ImportData();
                }
                catch (Exception exception)
                {
                    CommonLogHelper.GetInstance("LogError").Error(@"失败", exception);
                    MessageBox.Show(this, @"失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void 单条数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = ActiveMdiChild;
            var mdiFunction = (IMdiFunction)form;
            mdiFunction?.ExportOne2Excel();

        }

        private void 全部数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = ActiveMdiChild;
            var mdiFunction = (IMdiFunction)form;
            mdiFunction?.ExportAll2Excel();
        }

        private void 数据备份ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}