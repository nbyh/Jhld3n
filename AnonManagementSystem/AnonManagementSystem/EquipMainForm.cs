using EquipmentInformationData;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace AnonManagementSystem
{
    public partial class EquipMainForm : Form, IMdiFunction
    {
        private readonly SynchronizationContext _synchContext;
        private bool _enableedit = false;
        private EquipmentManagementEntities _equipEntities = new EquipmentManagementEntities();
        private int _pageSize = 20, _curPage = 1, _lastPage = 1;
        private DbRawSqlQuery<CombatEquipment> _equip;

        public EquipMainForm()
        {
            InitializeComponent();
            _synchContext = SynchronizationContext.Current;
        }

        public bool Enableedit
        {
            set { _enableedit = value; }
        }

        public void DataAdd()
        {
            EquipmentDetailForm equipDetailForm = new EquipmentDetailForm()
            {
                Enableedit = _enableedit,
                Add = true
            };
            equipDetailForm.SaveSuccess += SaveDataSuccess;
            equipDetailForm.ShowDialog();
        }

        public void DataDelete()
        {
            if (dgvEquip.CurrentRow != null)
            {
                if (MessageBox.Show(@"确定要删除该装备？删除后将无法找回！", @"警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        int selectRowIndex = dgvEquip.CurrentRow.Index;
                        dgvEquip.Rows.RemoveAt(selectRowIndex);
                        string id = dgvEquip.Rows[selectRowIndex].Cells["SerialNo"].Value.ToString();
                        var eq = (from eqt in _equipEntities.CombatEquipment
                                  where eqt.SerialNo == id
                                  select eqt).First();
                        _equipEntities.CombatEquipment.Remove(eq);
                        _equipEntities.SaveChanges();
                        CommonLogHelper.GetInstance("LogInfo").Info($"删除设备数据{id}成功");
                        MessageBox.Show(this, @"删除设备数据成功", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(this, @"删除设备数据失败" + e.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        CommonLogHelper.GetInstance("LogError").Error(@"删除设备数据失败", e);
                    }
                }
            }
        }

        public void DataRefresh()
        {
            Thread refreshThread = new Thread((ThreadStart) delegate
            {
                try
                {
                    _equipEntities = new EquipmentManagementEntities();
                    LoadData();
                    CommonLogHelper.GetInstance("LogInfo").Info(@"刷新设备数据成功");
                }
                catch (Exception e)
                {
                    MessageBox.Show(this, @"刷新设备数据失败" + e.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    CommonLogHelper.GetInstance("LogError").Error(@"刷新设备数据失败", e);
                }
            }) {IsBackground = true};
            refreshThread.Start();
        }

        public void LoadData()
        {
            string cmds = "select * from CombatEquipment";
            _equip = _equipEntities.Database.SqlQuery<CombatEquipment>(cmds);

            List<string> equipNameList = (from s in _equip where !string.IsNullOrEmpty(s.Name) select s.Name).Distinct().ToList();
            cmbName.DataSource = equipNameList;
            List<string> equipSubdepartList = (from s in _equip where !string.IsNullOrEmpty(s.SubDepartment) select s.SubDepartment).Distinct().ToList();
            cmbSubDepart.DataSource = equipSubdepartList;
            List<string> equipMajcatList = (from s in _equip where !string.IsNullOrEmpty(s.MajorCategory) select s.MajorCategory).Distinct().ToList();
            cmbMajorCategory.DataSource = equipMajcatList;
            List<string> equipModelList = (from s in _equip where !string.IsNullOrEmpty(s.Model) select s.Model).Distinct().ToList();
            cmbModel.DataSource = equipModelList;
            List<string> equipTechcanList = (from s in _equip where !string.IsNullOrEmpty(s.Technician) select s.Technician).Distinct().ToList();
            cmbTechnician.DataSource = equipTechcanList;
            List<string> equipManagerList = (from s in _equip where !string.IsNullOrEmpty(s.Manager) select s.Manager).Distinct().ToList();
            cmbManager.DataSource = equipManagerList;
            List<string> equipTechconList = (from s in _equip where !string.IsNullOrEmpty(s.TechCondition) select s.TechCondition).Distinct().ToList();
            cmbTechCondition.DataSource = equipTechconList;
            List<string> equipUseconList = (from s in _equip where !string.IsNullOrEmpty(s.UseCondition) select s.UseCondition).Distinct().ToList();
            cmbUseCondition.DataSource = equipUseconList;
            List<string> equipFactList = (from s in _equip where !string.IsNullOrEmpty(s.Factory) select s.Factory).Distinct().ToList();
            cmbFactory.DataSource = equipFactList;

            _pageSize = 20;
            _curPage = 1;
            DataRefresh(_pageSize, _curPage, _equip);
        }

        private void btnFront_Click(object sender, EventArgs e)
        {
            _curPage = 1;
            try
            {
                DataRefresh(_pageSize, _curPage, _equip);
                CommonLogHelper.GetInstance("LogInfo").Info(@"加载首页设备数据成功");
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, @"加载首页设备数据失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CommonLogHelper.GetInstance("LogError").Error(@"加载首页设备数据失败", exception);
            }
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
            try
            {
                DataRefresh(_pageSize, gopage, _equip);
                CommonLogHelper.GetInstance("LogInfo").Info(@"跳转设备数据页面成功");
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, @"加载设备数据页面失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CommonLogHelper.GetInstance("LogError").Error(@"跳转设备数据页面失败", exception);
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            _curPage = _lastPage;
            try
            {
                DataRefresh(_pageSize, _lastPage, _equip);
                CommonLogHelper.GetInstance("LogInfo").Info(@"加载末页设备数据成功");
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, @"加载末页设备数据失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CommonLogHelper.GetInstance("LogError").Error(@"加载末页设备数据失败", exception);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_curPage + 1 > _lastPage)
            {
                MessageBox.Show(@"已经是尾页了", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                _curPage++;
                try
                {
                    DataRefresh(_pageSize, _curPage, _equip);
                    CommonLogHelper.GetInstance("LogInfo").Info(@"加载下一页设备数据成功");
                }
                catch (Exception exception)
                {
                    MessageBox.Show(this, @"加载下一页设备数据失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    CommonLogHelper.GetInstance("LogError").Error(@"加载下一页设备数据失败", exception);
                }
            }
        }

        private void btnPre_Click(object sender, EventArgs e)
        {
            if (_curPage - 1 < 1)
            {
                MessageBox.Show(@"已经是首页了", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                _curPage--;
                try
                {
                    DataRefresh(_pageSize, _curPage, _equip);
                    CommonLogHelper.GetInstance("LogInfo").Info(@"加载上一页设备数据成功");
                }
                catch (Exception exception)
                {
                    MessageBox.Show(this, @"加载上一页设备数据失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    CommonLogHelper.GetInstance("LogError").Error(@"加载上一页设备数据失败", exception);
                }
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
                try
                {
                    DataRefresh(_pageSize, _curPage, _equip);
                    CommonLogHelper.GetInstance("LogInfo").Info(@"切换页数后加载设备数据成功");
                }
                catch (Exception exception)
                {
                    MessageBox.Show(this, @"加载设备数据失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    CommonLogHelper.GetInstance("LogError").Error(@"切换页数后加载设备数据失败", exception);
                }
            }
        }

        private void DataRefresh(int pagesize, int curpage, DbRawSqlQuery<CombatEquipment> iquery)
        {
            int all = iquery.Count();
            _lastPage = (int)Math.Ceiling((double)all / _pageSize);
            var equippage = QueryByPage(pagesize, curpage, iquery);
            _synchContext.Post(a =>
            {
                lbPageInfo.Text = $"总共{all}条记录，当前第{curpage}页，每页{pagesize}条，共{_lastPage}页";
                dgvEquip.DataSource = equippage.ToList();
                for (int i = 0; i < dgvEquip.RowCount; i++)
                {
                    dgvEquip[0, i].Value = i + 1;
                    dgvEquip.Rows[i].Cells["MoreInfo"].Value = "详细信息";
                }
            }, null);
        }

        private void dGvEquip_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvEquip.Columns[e.ColumnIndex].Name.Equals("MoreInfo"))
            {
                EquipmentDetailForm equipDetailForm = new EquipmentDetailForm()
                {
                    Enableedit = _enableedit,
                    Add = false,
                    Id = dgvEquip.Rows[e.RowIndex].Cells["SerialNo"].Value.ToString()
                };
                equipDetailForm.SaveSuccess += SaveDataSuccess;
                equipDetailForm.Show();
            }
        }

        private void SaveDataSuccess()
        {
            DataRefresh();
            btnLast_Click(null, null);
        }

        private void EquipMainForm_Load(object sender, EventArgs e)
        {
        }

        private void btnQueryInfo_Click(object sender, EventArgs e)
        {
            dgvEquip.Rows.Clear();
            IEnumerable<CombatEquipment> appointeq = new List<CombatEquipment>();
            if (!string.IsNullOrEmpty(cmbName.Text))
            {
                appointeq = from equipment in _equip
                            where equipment.Name == cmbName.Text
                            select equipment;
            }
            //todo:其他条件
            if (appointeq.Any())
            {
                dgvEquip.DataSource = appointeq;
            }
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
            dgvEquip.Rows.Clear();
            IEnumerable<Events> appointee = new List<Events>();
            if (!string.IsNullOrEmpty(cmbEventName.Text))
            {
                appointee = from ee in _equipEntities.Events
                            where ee.Name == cmbEventName.Text
                            select ee;
            }
            //todo:其他条件
            if (appointee.Any())
            {
                var appointeq = from equipment in _equip
                                where equipment.SerialNo == appointee.First().Equipment
                                select equipment;
                dgvEquip.DataSource = appointeq;
            }
        }

        private void btnRestEvent_Click(object sender, EventArgs e)
        {
        }

        private void EquipMainForm_Shown(object sender, EventArgs e)
        {
            Thread loadDataThread = new Thread((ThreadStart)delegate
           {
               try
               {
                   LoadData();
                   CommonLogHelper.GetInstance("LogInfo").Info(@"加载设备数据成功");
               }
               catch (Exception exception)
               {
                   _synchContext.Post(
                       a =>
                       {
                           MessageBox.Show(this, @"加载设备数据失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                       }, null);
                   CommonLogHelper.GetInstance("LogError").Error(@"加载设备数据失败", exception);
               }
           })
            { IsBackground = true };
            loadDataThread.Start();
        }

        private IList<CombatEquipment> QueryByPage(int pageSize, int curPage, DbRawSqlQuery<CombatEquipment> dbRaw)
        {
            return dbRaw.OrderBy(s => s.SerialNo).Take(pageSize * curPage).Skip(pageSize * (curPage - 1)).ToList();
        }
    }
}