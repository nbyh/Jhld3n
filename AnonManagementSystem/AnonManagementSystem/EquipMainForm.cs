using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Infrastructure;
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
        private bool _enableedit = false;
        private EquipmentManagementEntities _equipEntities = new EquipmentManagementEntities();
        private int _pageSize = 20, _curPage = 1, _lastPage = 1;
        private DbRawSqlQuery<CombatEquipment> equip;

        public EquipMainForm()
        {
            InitializeComponent();
        }

        public delegate void ChangeUser();
        public event ChangeUser ChangeCurrentuser;

        public bool Enableedit
        {
            set { _enableedit = value; }
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

        public void LoadData()
        {
            //equip = from eq in _equipEntities.CombatEquipment
            //        select eq;

            string cmds = "select * from CombatEquipment";
            equip = _equipEntities.Database.SqlQuery<CombatEquipment>(cmds);

            List<string> equipNameList = (from s in equip where !string.IsNullOrEmpty(s.Name) select s.Name).Distinct().ToList();
            cmbName.DataSource = equipNameList;
            List<string> equipSubdepartList = (from s in equip where !string.IsNullOrEmpty(s.SubDepartment) select s.SubDepartment).Distinct().ToList();
            cmbSubDepart.DataSource = equipSubdepartList;
            List<string> equipMajcatList = (from s in equip where !string.IsNullOrEmpty(s.MajorCategory) select s.MajorCategory).Distinct().ToList();
            cmbMajorCategory.DataSource = equipMajcatList;
            List<string> equipModelList = (from s in equip where !string.IsNullOrEmpty(s.Model) select s.Model).Distinct().ToList();
            cmbModel.DataSource = equipModelList;
            List<string> equipTechcanList = (from s in equip where !string.IsNullOrEmpty(s.Technician) select s.Technician).Distinct().ToList();
            cmbTechnician.DataSource = equipTechcanList;
            List<string> equipManagerList = (from s in equip where !string.IsNullOrEmpty(s.Manager) select s.Manager).Distinct().ToList();
            cmbManager.DataSource = equipManagerList;
            List<string> equipTechconList = (from s in equip where !string.IsNullOrEmpty(s.TechCondition) select s.TechCondition).Distinct().ToList();
            cmbTechCondition.DataSource = equipTechconList;
            List<string> equipUseconList = (from s in equip where !string.IsNullOrEmpty(s.UseCondition) select s.UseCondition).Distinct().ToList();
            cmbUseCondition.DataSource = equipUseconList;
            List<string> equipFactList = (from s in equip where !string.IsNullOrEmpty(s.Factory) select s.Factory).Distinct().ToList();
            cmbFactory.DataSource = equipFactList;
            
            cmbName.SelectedIndex = -1;
            cmbSubDepart.SelectedIndex = -1;
            cmbModel.SelectedIndex = -1;
            cmbMajorCategory.SelectedIndex = -1;
            cmbTechnician.SelectedIndex = -1;
            cmbManager.SelectedIndex = -1;
            cmbTechCondition.SelectedIndex = -1;
            cmbUseCondition.SelectedIndex = -1;
            cmbFactory.SelectedIndex = -1;
            cmbTimeTerm1.SelectedIndex = -1;
            cmbTimeTerm2.SelectedIndex = -1;
            
            _pageSize = 20;
            _curPage = 1;
            DataRefresh(_pageSize, _curPage, equip);
        }

        private void btnFront_Click(object sender, EventArgs e)
        {
            _curPage = 1;
            DataRefresh(_pageSize, _curPage, equip);
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            int gopage = int.Parse(tbPage.Text);
            if (gopage > _lastPage)
            {
                gopage = _lastPage;
                tbPage.Text = _lastPage.ToString();
            }
            _curPage = gopage;
            DataRefresh(_pageSize, gopage, equip);
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            DataRefresh(_pageSize, _lastPage, equip);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_curPage + 1 > _lastPage)
            {
                MessageBox.Show(@"已经是尾页了");
            }
            else
            {
                _curPage++;
                DataRefresh(_pageSize, _curPage, equip);
            }
        }

        private void btnPre_Click(object sender, EventArgs e)
        {
            if (_curPage - 1 < 1)
            {
                MessageBox.Show(@"已经是首页了");
            }
            else
            {
                _curPage--;
                DataRefresh(_pageSize, _curPage, equip);
            }
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

        private void cmbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPageSize.SelectedIndex > -1)
            {
                _pageSize = int.Parse(cmbPageSize.SelectedItem.ToString());
                DataRefresh(_pageSize, _curPage, equip);
            }
        }

        private void DataRefresh(int pagesize, int curpage, DbRawSqlQuery<CombatEquipment> iquery)
        {
            int all = iquery.Count();
            _lastPage = (int)Math.Ceiling((double)all / _pageSize);
            lbPageInfo.Text = $"总共{all}条记录，当前第{curpage}页，每页{pagesize}条，共{_lastPage}页";
            var equippage = QueryByPage(pagesize, curpage, iquery);
            dGvEquip.DataSource = equippage.ToList();
            for (int i = 0; i < dGvEquip.RowCount; i++)
            {
                dGvEquip[0, i].Value = i + 1;
                dGvEquip.Rows[i].Cells["MoreInfo"].Value = "详细信息";
            }
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

        private void btnQueryInfo_Click(object sender, EventArgs e)
        {

        }

        private void btnRestInfo_Click(object sender, EventArgs e)
        {
            cmbName.SelectedIndex = -1;
            cmbSubDepart.SelectedIndex = -1;
            cmbModel.SelectedIndex = -1;
            cmbMajorCategory.SelectedIndex = -1;
            cmbTechnician.SelectedIndex = -1;
            cmbManager.SelectedIndex = -1;
            cmbTechCondition.SelectedIndex = -1;
            cmbUseCondition.SelectedIndex = -1;
            cmbFactory.SelectedIndex = -1;
            cmbTimeTerm1.SelectedIndex = -1;
            cmbTimeTerm2.SelectedIndex = -1;
        }

        private void btnQueryEvent_Click(object sender, EventArgs e)
        {

        }

        private void btnRestEvent_Click(object sender, EventArgs e)
        {

        }
        
        private void EquipMainForm_Shown(object sender, EventArgs e)
        {
            LoadData();
            cmbPageSize.SelectedIndex = 0;
        }

        private IList<CombatEquipment> QueryByPage(int pageSize, int curPage, DbRawSqlQuery<CombatEquipment> dbRaw)
        {
            return dbRaw.OrderBy(s => s.SerialNo).Take(pageSize * curPage).Skip(pageSize * (curPage - 1)).ToList();
        }
    }
}
