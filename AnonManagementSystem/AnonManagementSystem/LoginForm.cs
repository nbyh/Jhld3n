using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EquipmentInformationData;

namespace AnonManagementSystem
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            string user = tbUser.Text;
            string pwd = tbPwd.Text;
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pwd))
            {
                MessageBox.Show(@"用户名和密码均不能为空！");
                return;
            }
            //string conn = "data source=" + AppDomain.CurrentDomain.BaseDirectory + "SystemManager.db";
            //string filename = AppDomain.CurrentDomain.BaseDirectory + "SystemManager.db";filename

            SystemManagerEntities sysManagerEntities = new SystemManagerEntities();
            var u = from s in sysManagerEntities.UserManage
                    where s.User == user
                    select s;
            if (u.ToList().Count < 0)
            {
                MessageBox.Show(@"用户不存在！");
                return;
            }
            UserManage loginuser = u.First();
            if (loginuser.Password != pwd)
            {
                MessageBox.Show(@"密码不正确！");
                return;
            }
            //SubMainForm form = new SubMainForm()
            //{
            //    Enableedit = loginuser.Edit
            //};
            //form.ChangeCurrentuser += ShowForm;
            this.Close();
        }

        //private void ShowForm()
        //{
        //    this.Show();
        //}

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
