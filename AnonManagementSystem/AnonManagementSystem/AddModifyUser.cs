using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AnonManagementSystem
{
    public partial class AddModifyUser : Form
    {
        private bool _add;
        private List<string> _alluserList = new List<string>();

        private bool _enableedit;

        private int _id = -1;
        private string _pwd;
        private int _rowindex = -1;
        private string _user;

        public AddModifyUser()
        {
            InitializeComponent();
        }

        public delegate void AddModifySysUser(bool modify, int rowindex, int id, string user, string pwd, bool enableedit);

        public event AddModifySysUser AddOrModifyUser;

        public bool Add
        {
            set { _add = value; }
        }

        public List<string> AlluserList
        {
            set { _alluserList = value; }
        }

        public bool Enableedit
        {
            set { _enableedit = value; }
        }

        public int Id
        {
            set { _id = value; }
        }

        public string Pwd
        {
            set { _pwd = value; }
        }

        public int RowIndex
        {
            set { _rowindex = value; }
        }
        public string User
        {
            set { _user = value; }
        }

        private void AddModifyUser_Load(object sender, EventArgs e)
        {
            if (_add)
            {
                if (!string.IsNullOrEmpty(_user))
                {
                    tbUser.ReadOnly = true;
                    tbUser.Text = _user;
                    tbPwdSure.Text = tbPwd.Text = _pwd;
                    cmbAuthority.SelectedItem = _enableedit ? "可写" : "只读";
                }
            }
            else
            {
                tbUser.ReadOnly = true;
                tbUser.Text = _user;
                tbPwdSure.Text = tbPwd.Text = _pwd;
                cmbAuthority.SelectedItem = _enableedit ? "可写" : "只读";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            if (tbPwd.Text != tbPwdSure.Text)
            {
                MessageBox.Show(@"两次密码不一致!");
                return;
            }
            if (!_add && _alluserList.Contains(tbUser.Text))
            {
                MessageBox.Show(@"已经包含相同用户，请勿重复添加!");
                return;
            }
            if (cmbAuthority.SelectedIndex < 0)
            {
                MessageBox.Show(@"请选择权限!");
                return;
            }
            bool enableedit = cmbAuthority.SelectedItem.ToString().Equals("可写");
            AddOrModifyUser?.Invoke(_add, _rowindex, _id, tbUser.Text, tbPwdSure.Text, enableedit);
            Close();
        }

        private void cmbAuthority_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }
            e.DrawBackground();
            e.DrawFocusRectangle();
            e.Graphics.DrawString(((ComboBox)sender).Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds.X, e.Bounds.Y + 3);
        }
    }
}