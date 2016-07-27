using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using EquipmentInformationData;

namespace AnonManagementSystem
{
    public partial class EquipMainForm : Form, IMdiFunction
    {
        private int _pageSize = 10, _curPage = 1, _lastPage = 1;
        public delegate void ChangeUser();
        public event ChangeUser ChangeCurrentuser;
        private bool _exitapp = true;
        private bool _enableedit = false;
        public EquipMainForm()
        {
            InitializeComponent();
        }

        public bool Enableedit
        {
            set { _enableedit = value; }
        }

        private void dGvEquip_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dGvEquip.Columns[e.ColumnIndex].Name.Equals("MoreInfo"))
            {
                EquipmentDetailForm equipDetailForm = new EquipmentDetailForm()
                {
                    Enableedit = _enableedit,
                    Add = false,
                    Id = dGvEquip.Rows[e.RowIndex].Cells["SerialNo"].Value.ToString()
                };
                equipDetailForm.Show();
            }
        }


        private void EquipMainForm_Load(object sender, EventArgs e)
        {
        }

        private void cmb_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }
            e.DrawBackground();
            e.DrawFocusRectangle();
            e.Graphics.DrawString(((ComboBox)sender).Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds.X, e.Bounds.Y + 3);
        }

        private EquipmentManagementEntities _equipEntities = new EquipmentManagementEntities();
        private IQueryable<CombatEquipment> equip;
        public void LoadData()
        {
            equip = from eq in _equipEntities.CombatEquipment
                    select eq;

            List<string> equipnameList = (from n in equip select n.Name).Distinct().ToList();
            cmbName.DataSource = equipnameList;
            List<string> equipsubdepartList = (from d in equip select d.SubDepartment).Distinct().ToList();
            cmbSubDepart.DataSource = equipsubdepartList;
            List<string> equipModelList = (from s in equip select s.Model).Distinct().ToList();
            cmbModel.DataSource = equipModelList;
            List<string> equipSpotList = (from s in equip select s.InventorySpot).Distinct().ToList();
            cmbSpot.DataSource = equipSpotList;
            cmbName.SelectedIndex = -1;
            cmbSubDepart.SelectedIndex = -1;
            cmbModel.SelectedIndex = -1;
            cmbSpot.SelectedIndex = -1;

            _pageSize = 10;
            _curPage = 1;
            DataRefresh(_pageSize, _curPage, equip);
        }

        private void DataRefresh(int pagesize, int curpage, IQueryable<CombatEquipment> iquery)
        {
            int all = iquery.Count();
            _lastPage = all / _pageSize;
            lbPageInfo.Text = $"总共{all}条记录，当前第{curpage}页，每页{pagesize}条，共{_lastPage}页";
            var equippage = QueryByPage(pagesize, curpage, iquery);
            dGvEquip.DataSource = equippage.ToList();
            for (int i = 0; i < dGvEquip.RowCount; i++)
            {
                dGvEquip[0, i].Value = i + 1;
                dGvEquip.Rows[i].Cells["MoreInfo"].Value = "详细信息";
            }
        }

        public void DataAdd()
        {
            EquipmentDetailForm equipDetailForm = new EquipmentDetailForm()
            {
                Enableedit = true,
                Add = true
            };
            equipDetailForm.Show();
        }

        public void DataDelete()
        {
            throw new NotImplementedException();
        }

        public void DataRefresh()
        {
            _equipEntities = new EquipmentManagementEntities();
            LoadData();
        }

        private void EquipMainForm_Shown(object sender, EventArgs e)
        {
            LoadData();
            cmbPageSize.SelectedIndex = 0;
        }

        private void btnFront_Click(object sender, EventArgs e)
        {
            _curPage = 1;
            DataRefresh(_pageSize, _curPage, equip);
        }

        private void cmbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPageSize.SelectedIndex > -1)
            {
                _pageSize = int.Parse(cmbPageSize.SelectedItem.ToString());
                DataRefresh(_pageSize, _curPage, equip);
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            DataRefresh(_pageSize, _lastPage, equip);
        }

        private void btnPre_Click(object sender, EventArgs e)
        {
            if (_curPage - 1 < 1)
            {
                MessageBox.Show("已经是首页了");
            }
            else
            {
                _curPage--;
                DataRefresh(_pageSize, _curPage, equip);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_curPage + 1 > _lastPage)
            {
                MessageBox.Show("已经是尾页了");
            }
            else
            {
                _curPage++;
                DataRefresh(_pageSize, _curPage, equip);
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            DataRefresh(_pageSize, int.Parse(tbPage.Text), equip);
        }

        private IList<CombatEquipment> QueryByPage(int pageSize, int curPage, IQueryable<CombatEquipment> query)
        {
            //return query.OrderBy(s => s.SerialNo).Take(pageSize).Skip(pageSize * (curPage - 1)).ToList();
            int offset = pageSize * (curPage - 1);
            string cmds = $"select * from  CombatEquipment Limit {pageSize} Offset {offset}";
            var cs = _equipEntities.Database.SqlQuery<CombatEquipment>(cmds);
            return cs.ToList();
        }
    }
}
