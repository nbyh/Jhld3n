using EquipmentInformationData;
using LinqToDB;
using LinqToDB.DataProvider.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AnonManagementSystem
{
    public partial class SystemSetting : Form
    {
        private static readonly string Conn = $"data source = {AppDomain.CurrentDomain.BaseDirectory}SystemManager.db;";
        private readonly SystemManagerDB _sysManagerDb = new SystemManagerDB(new SQLiteDataProvider(), Conn);
        private readonly List<UserManage> _userList = new List<UserManage>();
        private List<string> _alluser = new List<string>();
        //private UserManage um;

        public SystemSetting()
        {
            InitializeComponent();
        }

        private void AddModify(bool add, int rowindex, int id, string user, string pwd, bool enableedit)
        {
            int j = dgvUserManage.RowCount;

            if (add)
            {
                if (id != -1)
                {
                    var s = from us in _userList
                            where us.User == user
                            select us;
                    _userList.Remove(s.First());
                    dgvUserManage.Rows.RemoveAt(rowindex);
                }
                dgvUserManage.Rows.Add(1);
                int i = dgvUserManage.RowCount - 1;
                dgvUserManage[0, i].Value = i + 1;
                dgvUserManage[1, i].Value = user;
                dgvUserManage[2, i].Value = enableedit ? "可写" : "只读";
                dgvUserManage[3, i].Value = pwd;
                UserManage um = new UserManage() { Edit = enableedit, Password = pwd, User = user };
                _userList.Add(um);
            }
            else
            {
                var sms = from u in _sysManagerDb.UserManages
                          where u.ID == id
                          select u;
                sms.First().User = user;
                sms.First().Password = pwd;
                sms.First().Edit = enableedit;

                dgvUserManage[1, rowindex].Value = user;
                dgvUserManage[2, rowindex].Value = enableedit ? "可写" : "只读";
                dgvUserManage[3, rowindex].Value = pwd;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_userList.Count > 0)
            {
                _sysManagerDb.InsertOrReplace(_userList);
            }
            Close();
        }

        private void SystemSetting_Load(object sender, EventArgs e)
        {
            var sms = (from u in _sysManagerDb.UserManages
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
            AddModifyUser addModifyUser = new AddModifyUser { AlluserList = _alluser, Add = true };
            addModifyUser.AddOrModifyUser += AddModify;
            addModifyUser.ShowDialog();
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (dgvUserManage.CurrentRow != null)
            {
                int rowindex = dgvUserManage.CurrentRow.Index;
                if (MessageBox.Show(@"确定是否删除", @"提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    if (dgvUserManage[4, rowindex].Value != null)
                    {
                        int id = int.Parse(dgvUserManage[4, rowindex].Value.ToString());
                        _sysManagerDb.UserManages.Where(u => u.ID == id).Delete();
                    }
                    dgvUserManage.Rows.RemoveAt(rowindex);
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
                    addModifyUser.Add = false;
                    addModifyUser.RowIndex = rowindex;
                    addModifyUser.User = dgvUserManage[1, rowindex].Value.ToString();
                    addModifyUser.Enableedit = dgvUserManage[2, rowindex].Value.ToString().Equals("可写");
                    addModifyUser.Pwd = dgvUserManage[3, rowindex].Value.ToString();
                    addModifyUser.Id = int.Parse(dgvUserManage[4, rowindex].Value.ToString());
                }
                else
                {
                    addModifyUser.Add = true;
                    addModifyUser.RowIndex = rowindex;
                    addModifyUser.User = dgvUserManage[1, rowindex].Value.ToString();
                    addModifyUser.Enableedit = dgvUserManage[2, rowindex].Value.ToString().Equals("可写");
                    addModifyUser.Pwd = dgvUserManage[3, rowindex].Value.ToString();
                }
                addModifyUser.AddOrModifyUser += AddModify;
                addModifyUser.ShowDialog();
            }
        }
    }
}