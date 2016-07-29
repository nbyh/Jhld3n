using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using EquipmentInformationData;

namespace AnonManagementSystem
{
    public partial class SystemSetting : Form
    {
        readonly SystemManagerEntities _sysManagerEntities = new SystemManagerEntities();
        private List<string> _alluser = new List<string>();

        public SystemSetting()
        {
            InitializeComponent();
        }

        private void AddModify(bool modify, int rowindex, int id, string user, string pwd, bool enableedit)
        {
            int j = dgvUserManage.RowCount;
            if (modify)
            {
                dgvUserManage[1, rowindex].Value = user;
                dgvUserManage[2, rowindex].Value = enableedit ? "可写" : "只读";
                dgvUserManage[3, rowindex].Value = pwd;

                var sms = from u in _sysManagerEntities.UserManage
                          where u.ID == id
                          select u;
                sms.First().User = user;
                sms.First().Password = pwd;
                sms.First().Edit = enableedit;
                //修改
            }
            else
            {
                if (rowindex != -1)
                {//增加
                    dgvUserManage.Rows.RemoveAt(rowindex);
                    var sms = from u in _sysManagerEntities.UserManage
                              select u;
                    _sysManagerEntities.UserManage.Remove(sms.First());
                }
                dgvUserManage.Rows.Add(1);
                int i = dgvUserManage.RowCount - 1;
                dgvUserManage[0, i].Value = i + 1;
                dgvUserManage[1, i].Value = user;
                dgvUserManage[2, i].Value = enableedit ? "可写" : "只读";
                dgvUserManage[3, i].Value = pwd;
                UserManage um = new UserManage() { Edit = enableedit, Password = pwd, User = user };
                _sysManagerEntities.UserManage.Add(um);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _sysManagerEntities.SaveChanges();
            Close();
        }

        private void SystemSetting_Load(object sender, EventArgs e)
        {
            var sms = (from u in _sysManagerEntities.UserManage
                       select u).Distinct().ToList();
            _alluser = sms.Select(n => n.User).ToList();
            dgvUserManage.RowCount = sms.Count;
            for (int i = 0; i < sms.Count; i++)
            {
                dgvUserManage[0, i].Value = i + 1;
                dgvUserManage[1, i].Value = sms[i].User;
                dgvUserManage[2, i].Value = sms[i].Edit ? "可写" : "只读";
                dgvUserManage[3, i].Value = sms[i].Password;
                dgvUserManage[4, i].Value = sms[i].ID;
            }
        }

        private void tsbAdd_Click(object sender, EventArgs e)
        {
            AddModifyUser addModifyUser = new AddModifyUser { AlluserList = _alluser };
            addModifyUser.Add_ModifyUser += AddModify;
            addModifyUser.ShowDialog();
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (dgvUserManage.CurrentRow != null)
            {
                int rowindex = dgvUserManage.CurrentRow.Index;
                if (dgvUserManage[4, rowindex].Value != null)
                {
                    if (MessageBox.Show(@"确定是否删除", @"提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        int id = int.Parse(dgvUserManage[3, rowindex].Value.ToString());
                        var sms = from u in _sysManagerEntities.UserManage
                                  where u.ID == id
                                  select u;
                        _sysManagerEntities.UserManage.Remove(sms.First());
                        dgvUserManage.Rows.RemoveAt(rowindex);
                    }
                    else
                    {
                        dgvUserManage.Rows.RemoveAt(rowindex);
                    }
                }
            }
        }

        private void tsbModify_Click(object sender, EventArgs e)
        {
            if (dgvUserManage.CurrentRow != null)
            {
                int rowindex = dgvUserManage.CurrentRow.Index;
                AddModifyUser addModifyUser = new AddModifyUser();
                if (dgvUserManage[4, rowindex].Value != null)
                {
                    addModifyUser.Modify = true;
                    addModifyUser.RowIndex = rowindex;
                    addModifyUser.User = dgvUserManage[1, rowindex].Value.ToString();
                    addModifyUser.Enableedit = dgvUserManage[2, rowindex].Value.ToString().Equals("可写");
                    addModifyUser.Pwd = dgvUserManage[3, rowindex].Value.ToString();
                    addModifyUser.Id = int.Parse(dgvUserManage[4, rowindex].Value.ToString());
                }
                else
                {
                    addModifyUser.Modify = false;
                    addModifyUser.RowIndex = rowindex;
                    addModifyUser.User = dgvUserManage[1, rowindex].Value.ToString();
                    addModifyUser.Enableedit = dgvUserManage[2, rowindex].Value.ToString().Equals("可写");
                    addModifyUser.Pwd = dgvUserManage[3, rowindex].Value.ToString();
                }
                addModifyUser.Add_ModifyUser += AddModify;
                addModifyUser.ShowDialog();
            }
        }
    }
}
