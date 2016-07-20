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
                SubMainForm subMainForm = new SubMainForm { MdiParent = this, Tag = "Equipment" };
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
            LoginForm loginForm = new LoginForm();
            loginForm.ShowDialog();
            _islogin = loginForm.DialogResult == DialogResult.OK;

        }

        private void ChangeUserMenu_Click(object sender, EventArgs e)
        {
            _islogin = false;
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
            LoginForm loginForm = new LoginForm();
            loginForm.ShowDialog();
            _islogin = loginForm.DialogResult == DialogResult.OK;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AmsAboutBox amsAboutBox = new AmsAboutBox();
            amsAboutBox.ShowDialog();
        }

        private void tsbAdd_Click(object sender, EventArgs e)
        {
            Form form = this.ActiveMdiChild;
            var mdiFunction = (IMdiFunction) form;
            mdiFunction?.DataAdd();
        }
    }
}
