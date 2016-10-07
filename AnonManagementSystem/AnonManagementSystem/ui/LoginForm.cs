using EquipmentInformationData;
using LinqToDB.DataProvider.SQLite;
using System;
using System.Linq;
using System.Windows.Forms;

namespace AnonManagementSystem
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        public delegate void LoginOn(bool enableedit);

        public event LoginOn LoginSucess;

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            string user = tbUser.Text;
            string pwd = tbPwd.Text;
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pwd))
            {
                MessageBox.Show(@"用户名和密码均不能为空！", @"警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string conn = $"data source = {AppDomain.CurrentDomain.BaseDirectory}SystemManager.db;";
            SystemManagerDB sysManagerDb = new SystemManagerDB(new SQLiteDataProvider(), conn);
            var u = sysManagerDb.UserManages.Where(s => s.User == user).Take(1);
            if (!u.Any())
            {
                MessageBox.Show(@"用户不存在！", @"警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            UserManage loginuser = u.First();
            if (loginuser.Password != pwd)
            {
                MessageBox.Show(@"密码不正确！", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //SubMainForm form = new SubMainForm()
            //{
            //    Enableedit = loginuser.Edit
            //};
            //form.ChangeCurrentuser += ShowForm;
            LoginSucess?.Invoke(loginuser.Edit);
            CommonLogHelper.GetInstance("LogInfo").Info($"用户{loginuser.User}登陆成功");
            this.Close();
        }
    }
}