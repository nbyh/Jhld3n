using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AnonManagementSystem
{
    public partial class MDIParent : Form
    {
        public MDIParent()
        {
            InitializeComponent();
        }

        private void LoginOnSucess(bool enableedit)
        {
            _enableedit = enableedit;
            _islogin = true;
            MessageBox.Show(@"登陆成功");
        }

        private bool _enableedit = false;
        private bool _islogin = false;

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

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
            Form form = this.ActiveMdiChild;
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
                SparePartsForm sparePartsForm = new SparePartsForm { MdiParent = this, Tag = "SpareParts" };
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
            Form form = this.ActiveMdiChild;
            var mdiFunction = (IMdiFunction)form;
            mdiFunction?.DataRefresh();
        }
    }
}
