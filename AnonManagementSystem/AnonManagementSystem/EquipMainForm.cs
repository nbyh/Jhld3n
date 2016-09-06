using EquipmentInformationData;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
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
        private DbRawSqlQuery<CombatEquipment> _equipDbRaw;

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
                        var vh = from comvh in _equipEntities.CombatVehicles
                                 where comvh.Equipment == id
                                 select comvh;
                        _equipEntities.CombatVehicles.RemoveRange(vh);
                        _equipEntities.SaveChanges();
                        var ev = from eve in _equipEntities.Events
                                 where eve.Equipment == id
                                 select eve;
                        if (ev.Any())
                        {
                            foreach (var eventse in ev)
                            {
                                var ed = from evd in _equipEntities.EventData
                                         where evd.EventsNo == eventse.No
                                         select evd;
                                _equipEntities.EventData.RemoveRange(ed);
                                _equipEntities.SaveChanges();
                            }
                        }
                        _equipEntities.Events.RemoveRange(ev);
                        _equipEntities.SaveChanges();
                        var eq = (from eqt in _equipEntities.CombatEquipment
                                  where eqt.SerialNo == id
                                  select eqt).First();
                        _equipEntities.CombatEquipment.Remove(eq);
                        _equipEntities.SaveChanges();
                        DataRefresh();
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
            Thread refreshThread = new Thread((ThreadStart)delegate
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
           })
            { IsBackground = true };
            refreshThread.Start();
        }

        public void Export2Excel()
        {
            try
            {
                int? r = dgvEquip.CurrentRow?.Index;
                if (r != null && r >= 0)
                {
                    sfdExcel.ShowDialog();
                    string fn = sfdExcel.FileName;
                    string excelid = dgvEquip.Rows[r.Value].Cells["SerialNo"].Value.ToString();
                    var firsteq = (from eq in _equipEntities.CombatEquipment
                                   where eq.SerialNo == excelid
                                   select eq).First();
                    var vehicles = (from vh in _equipEntities.CombatVehicles
                                    where vh.Equipment == excelid
                                    select vh).ToList();
                    var comboevhid = vehicles.FirstOrDefault(a => a.CombineOe);

                    OilEngine oe = null;
                    if (comboevhid != null)
                    {
                        oe = (from o in _equipEntities.OilEngine
                              where o.Vehicle == comboevhid.SerialNo
                              select o).FirstOrDefault();
                    }

                    var events = (from ev in _equipEntities.Events
                                  where ev.Equipment == excelid
                                  select ev).ToList();
                    //var eventsd = new Dictionary<string, List<EventData>>();
                    //foreach (var ee in events)
                    //{
                    //    var ed = (from d in _equipEntities.EventData
                    //              where d.EventsNo == ee.No
                    //              select d).ToList();
                    //    eventsd.Add(ee.No, ed);
                    //}

                    EquipImageEntities eqImgEntities = new EquipImageEntities();
                    List<EquipmentImage> eqimgList = (from img in eqImgEntities.EquipmentImage
                                                      where img.SerialNo == excelid
                                                      select img).Take(3).ToList();
                    EquipExcelDataStruct eeds=new EquipExcelDataStruct()
                    {
                        Equip = firsteq,
                        VhList = vehicles,
                        Oe = oe,
                        Events = events,
                        //EventDic = eventsd,
                        EqImg = eqimgList
                    };
                    if (ExportData2Excel.ExportData(fn, eeds))
                    {
                        CommonLogHelper.GetInstance("LogInfo").Info($"导出设备数据{excelid}成功");
                        MessageBox.Show(this, @"导出设备数据成功", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception exception)
            {
                CommonLogHelper.GetInstance("LogError").Error(@"导出设备数据失败", exception);
                MessageBox.Show(this, @"导出设备数据失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadData()
        {
            string cmds = "select * from CombatEquipment";
            _equipDbRaw = _equipEntities.Database.SqlQuery<CombatEquipment>(cmds);

            List<string> equipNameList = (from s in _equipDbRaw where !string.IsNullOrEmpty(s.Name) select s.Name).Distinct().ToList();
            cmbName.DataSource = equipNameList;
            List<string> equipSubdepartList = (from s in _equipDbRaw where !string.IsNullOrEmpty(s.SubDepartment) select s.SubDepartment).Distinct().ToList();
            cmbSubDepart.DataSource = equipSubdepartList;
            List<string> equipMajcatList = (from s in _equipDbRaw where !string.IsNullOrEmpty(s.MajorCategory) select s.MajorCategory).Distinct().ToList();
            cmbMajorCategory.DataSource = equipMajcatList;
            List<string> equipModelList = (from s in _equipDbRaw where !string.IsNullOrEmpty(s.Model) select s.Model).Distinct().ToList();
            cmbModel.DataSource = equipModelList;
            List<string> equipTechcanList = (from s in _equipDbRaw where !string.IsNullOrEmpty(s.Technician) select s.Technician).Distinct().ToList();
            cmbTechnician.DataSource = equipTechcanList;
            List<string> equipManagerList = (from s in _equipDbRaw where !string.IsNullOrEmpty(s.Manager) select s.Manager).Distinct().ToList();
            cmbManager.DataSource = equipManagerList;
            List<string> equipTechconList = (from s in _equipDbRaw where !string.IsNullOrEmpty(s.TechCondition) select s.TechCondition).Distinct().ToList();
            cmbTechCondition.DataSource = equipTechconList;
            List<string> equipUseconList = (from s in _equipDbRaw where !string.IsNullOrEmpty(s.UseCondition) select s.UseCondition).Distinct().ToList();
            cmbUseCondition.DataSource = equipUseconList;
            List<string> equipFactList = (from s in _equipDbRaw where !string.IsNullOrEmpty(s.Factory) select s.Factory).Distinct().ToList();
            cmbFactory.DataSource = equipFactList;

            _pageSize = 20;
            _curPage = 1;
            DataRefresh(_pageSize, _curPage, _equipDbRaw);
        }

        private void btnFront_Click(object sender, EventArgs e)
        {
            _curPage = 1;
            try
            {
                DataRefresh(_pageSize, _curPage, _equipDbRaw);
                CommonLogHelper.GetInstance("LogInfo").Info(@"加载首页设备数据成功");
            }
            catch (Exception exception)
            {
                CommonLogHelper.GetInstance("LogError").Error(@"加载首页设备数据失败", exception);
                MessageBox.Show(this, @"加载首页设备数据失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                DataRefresh(_pageSize, gopage, _equipDbRaw);
                CommonLogHelper.GetInstance("LogInfo").Info(@"跳转设备数据页面成功");
            }
            catch (Exception exception)
            {
                CommonLogHelper.GetInstance("LogError").Error(@"跳转设备数据页面失败", exception);
                MessageBox.Show(this, @"加载设备数据页面失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            _curPage = _lastPage;
            try
            {
                DataRefresh(_pageSize, _lastPage, _equipDbRaw);
                CommonLogHelper.GetInstance("LogInfo").Info(@"加载末页设备数据成功");
            }
            catch (Exception exception)
            {
                CommonLogHelper.GetInstance("LogError").Error(@"加载末页设备数据失败", exception);
                MessageBox.Show(this, @"加载末页设备数据失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    DataRefresh(_pageSize, _curPage, _equipDbRaw);
                    CommonLogHelper.GetInstance("LogInfo").Info(@"加载下一页设备数据成功");
                }
                catch (Exception exception)
                {
                    CommonLogHelper.GetInstance("LogError").Error(@"加载下一页设备数据失败", exception);
                    MessageBox.Show(this, @"加载下一页设备数据失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    DataRefresh(_pageSize, _curPage, _equipDbRaw);
                    CommonLogHelper.GetInstance("LogInfo").Info(@"加载上一页设备数据成功");
                }
                catch (Exception exception)
                {
                    CommonLogHelper.GetInstance("LogError").Error(@"加载上一页设备数据失败", exception);
                    MessageBox.Show(this, @"加载上一页设备数据失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    DataRefresh(_pageSize, _curPage, _equipDbRaw);
                    CommonLogHelper.GetInstance("LogInfo").Info(@"切换页数后加载设备数据成功");
                }
                catch (Exception exception)
                {
                    CommonLogHelper.GetInstance("LogError").Error(@"切换页数后加载设备数据失败", exception);
                    MessageBox.Show(this, @"加载设备数据失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void dgvEquip_CellContentClick(object sender, DataGridViewCellEventArgs e)
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
            var appointeq = from equipment in _equipDbRaw
                            select equipment;
            if (!string.IsNullOrEmpty(cmbName.Text))
            {
                appointeq = appointeq.Where(a => a.Name == cmbName.Text);
            }
            if (!string.IsNullOrEmpty(cmbMajorCategory.Text))
            {
                appointeq = appointeq.Where(a => a.MajorCategory == cmbMajorCategory.Text);
            }
            if (!string.IsNullOrEmpty(cmbModel.Text))
            {
                appointeq = appointeq.Where(a => a.Model == cmbModel.Text);
            }
            if (!string.IsNullOrEmpty(cmbManager.Text))
            {
                appointeq = appointeq.Where(a => a.Manager == cmbManager.Text);
            }
            if (!string.IsNullOrEmpty(cmbTechnician.Text))
            {
                appointeq = appointeq.Where(a => a.Technician == cmbTechnician.Text);
            }
            if (!string.IsNullOrEmpty(cmbUseCondition.Text))
            {
                appointeq = appointeq.Where(a => a.UseCondition == cmbUseCondition.Text);
            }
            if (!string.IsNullOrEmpty(cmbTechCondition.Text))
            {
                appointeq = appointeq.Where(a => a.TechCondition == cmbTechCondition.Text);
            }
            if (!string.IsNullOrEmpty(cmbSubDepart.Text))
            {
                appointeq = appointeq.Where(a => a.SubDepartment == cmbSubDepart.Text);
            }
            if (!string.IsNullOrEmpty(cmbFactory.Text))
            {
                appointeq = appointeq.Where(a => a.Factory == cmbFactory.Text);
            }
            if (!string.IsNullOrEmpty(cmbTimeTerm1.Text))
            {
                appointeq = appointeq.Where(a => PublicFunction.CompareTime(cmbTimeTerm1.Text, a.ProductionDate, dtpTime1.Value.Date));
            }
            if (!string.IsNullOrEmpty(cmbTimeTerm2.Text))
            {
                appointeq = appointeq.Where(a => PublicFunction.CompareTime(cmbTimeTerm2.Text, a.ProductionDate, dtpTime2.Value.Date));
            }
            if (appointeq.Any())
            {
                dgvEquip.DataSource = appointeq;
            }
            else
            {
                MessageBox.Show(this, @"没有筛选到相关条件的设备信息，请修改筛选条件", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            var appointee = from ee in _equipEntities.Events
                            select ee;
            if (!string.IsNullOrEmpty(cmbEventName.Text))
            {
                appointee = appointee.Where(a => a.Name == cmbEventName.Text);
            }
            if (!string.IsNullOrEmpty(cmbEventAddress.Text))
            {
                appointee = appointee.Where(a => a.Address == cmbEventAddress.Text);
            }
            if (!string.IsNullOrEmpty(cmbPublishUnit.Text))
            {
                appointee = appointee.Where(a => a.PublishUnit == cmbPublishUnit.Text);
            }
            if (!string.IsNullOrEmpty(cmbPublisher.Text))
            {
                appointee = appointee.Where(a => a.Publisher == cmbPublisher.Text);
            }
            if (!string.IsNullOrEmpty(cmbEventType.Text))
            {
                appointee = appointee.Where(a => a.EventType == cmbEventType.Text);
            }
            if (!string.IsNullOrEmpty(cmbSpecificType.Text))
            {
                appointee = appointee.Where(a => a.SpecificType == cmbSpecificType.Text);
            }
            if (!string.IsNullOrEmpty(cmbPublishDtTerm.Text))
            {
                appointee = appointee.Where(a => PublicFunction.CompareTime(cmbPublishDtTerm.Text, a.PublishDate, dtpPublish.Value.Date));
            }
            if (!string.IsNullOrEmpty(cmbEventDtTerm1.Text))
            {
                appointee = appointee.Where(a => PublicFunction.CompareTime(cmbEventDtTerm1.Text, a.StartTime, dtpEventDt1.Value.Date));
            }
            if (!string.IsNullOrEmpty(cmbEventDtTerm2.Text))
            {
                appointee = appointee.Where(a => PublicFunction.CompareTime(cmbEventDtTerm2.Text, a.EndTime, dtpEventDt2.Value.Date));
            }
            if (appointee.Any())
            {
                var appointeq = from equipment in _equipDbRaw
                                where equipment.SerialNo == appointee.First().Equipment
                                select equipment;
                dgvEquip.DataSource = appointeq;
            }
            else
            {
                MessageBox.Show(this, @"没有筛选到相关条件的设备信息，请修改筛选条件", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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