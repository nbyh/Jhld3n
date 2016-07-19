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
    public partial class SystemSetting : Form
    {

        private List<UserManage> adduser = new List<UserManage>();
        private List<UserManage> modifyuser = new List<UserManage>();
        public SystemSetting()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SystemManagerEntities sysManagerEntities = new SystemManagerEntities();
            sysManagerEntities.UserManage.AddRange(adduser);
            foreach (var um in modifyuser)
            {
                var umtemp = (from u in sysManagerEntities.UserManage
                              where u.ID == um.ID
                              select u).First();
                umtemp.User = um.User;
                umtemp.Password = um.Password;
                umtemp.Edit = um.Edit;
            }
            sysManagerEntities.SaveChanges();
            this.Close();
        }

        private void tsbModify_Click(object sender, EventArgs e)
        {
            if (dgvUserManage.CurrentRow != null)
            {
                int rowindex = dgvUserManage.CurrentRow.Index;
                AddModifyUser addModifyUser = new AddModifyUser()
                {
                    Modify = true,
                    Id = (int)dgvUserManage[3, rowindex].Value,
                    User = dgvUserManage[1, rowindex].Value.ToString(),
                    Pwd = dgvUserManage[4, rowindex].Value.ToString(),
                    Enableedit = dgvUserManage[2, rowindex].Value.ToString().Equals("可写")
                };
                addModifyUser.ShowDialog();
            }
        }

        private void tsbAdd_Click(object sender, EventArgs e)
        {
            AddModifyUser addModifyUser = new AddModifyUser { AlluserList = alluser };
            addModifyUser.Add_ModifyUser += AddModify;
            addModifyUser.ShowDialog();
        }

        private void AddModify(bool modify, int id, string user, string pwd, bool enableedit)
        {
            int j = dgvUserManage.RowCount;
            if (modify)
            {
                UserManage um = new UserManage() { ID = id, Edit = enableedit, Password = pwd, User = user };
                modifyuser.Add(um);
                for (int i = 0; i < j; i++)
                {
                    if (dgvUserManage.Rows[i].Cells["ID"].Value.ToString() == id.ToString())
                    {
                        dgvUserManage.Rows[i].Cells[0].Value = i + 1;
                        dgvUserManage.Rows[i].Cells[1].Value = user;
                        dgvUserManage.Rows[i].Cells[2].Value = enableedit ? "可写" : "只读";
                    }
                }
                //修改
            }
            else
            {
                UserManage um = new UserManage() { Edit = enableedit, Password = pwd, User = user };
                adduser.Add(um);
                dgvUserManage.RowCount = j + 1;
                dgvUserManage.Rows[j].Cells[0].Value = j + 1;
                dgvUserManage.Rows[j].Cells[1].Value = user;
                dgvUserManage.Rows[j].Cells[2].Value = enableedit ? "可写" : "只读";
                //增加
            }
        }
        private List<string> alluser = new List<string>();
        private void SystemSetting_Load(object sender, EventArgs e)
        {
            SystemManagerEntities sysManagerEntities = new SystemManagerEntities();
            var sms = (from u in sysManagerEntities.UserManage
                       select u).Distinct().ToList();
            alluser = sms.Select(n => n.User).ToList();
            dgvUserManage.RowCount = sms.Count;
            for (int i = 0; i < sms.Count; i++)
            {
                dgvUserManage[0, i].Value = i + 1;
                dgvUserManage[1, i].Value = sms[i].User;
                dgvUserManage[2, i].Value = sms[i].Edit ? "可写" : "只读";
                dgvUserManage[3, i].Value = sms[i].ID;
                dgvUserManage[4, i].Value = sms[i].Password;
            }
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (dgvUserManage.CurrentRow != null)
            {
                if (MessageBox.Show(@"确定是否删除", @"提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    int rowindex = dgvUserManage.CurrentRow.Index;
                    UserManage um = new UserManage()
                    {
                        ID = (int)dgvUserManage[3, rowindex].Value,
                        User = dgvUserManage[1, rowindex].Value.ToString(),
                        Password = dgvUserManage[4, rowindex].Value.ToString(),
                        Edit = dgvUserManage[2, rowindex].Value.ToString().Equals("可写")
                    };
                    SystemManagerEntities sysManagerEntities = new SystemManagerEntities();
                    sysManagerEntities.UserManage.Remove(um);
                    sysManagerEntities.SaveChanges();
                }
            }
        }
    }
}
