﻿using EquipmentInformationData;
using LinqToDB;
using LinqToDB.DataProvider.SQLite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace AnonManagementSystem
{
    public partial class EquipMainForm : Form, IMdiFunction
    {
        private readonly SynchronizationContext _synchContext;

        private bool _enableedit = false;

        private IEnumerable<CombatEquipment> _equip;

        private EquipmentManagementDB _equipDb = new EquipmentManagementDB(new SQLiteDataProvider(), DbPublicFunction.ReturnDbConnectionString(@"\ZBDataBase\EquipmentManagement.db"));

        private int _pageSize = 20, _curPage = 1, _lastPage = 1;

        public EquipMainForm()
        {
            InitializeComponent();
            _synchContext = SynchronizationContext.Current;
        }

        public delegate void StatusSet(string info);

        public delegate void VisibleTools();

        public event StatusSet SetStatusInfo;

        public event VisibleTools SetToolStripVisible;

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
                        //dgvEquip.Rows.RemoveAt(selectRowIndex);
                        string id = dgvEquip.Rows[selectRowIndex].Cells["SerialNo"].Value.ToString();
                        _equipDb.CombatVehicles.Where(comvh => comvh.Equipment == id).Delete();
                        //_equipDB.SaveChanges();
                        var ev = _equipDb.Events.Where(eve => eve.Equipment == id);
                        if (ev.Any())
                        {
                            foreach (var eventse in ev)
                            {
                                _equipDb.EventData.Where(a => a.EventsNo == eventse.No).Delete();
                            }
                        }
                        ev.Delete();
                        //_equipDB.SaveChanges();
                        _equipDb.CombatEquipments.Where(eqt => eqt.SerialNo == id).Delete();
                        //_equipDB.SaveChanges();
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
                   _equipDb = new EquipmentManagementDB(new SQLiteDataProvider(), DbPublicFunction.ReturnDbConnectionString(@"\ZBDataBase\EquipmentManagement.db")); ;
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

        public void ExportAll2Excel()
        {
            try
            {
                if (sfdExcel.ShowDialog() == DialogResult.OK)
                {
                    string fn = sfdExcel.FileName;
                    if (File.Exists(fn))
                    {
                        try
                        {
                            File.Delete(fn);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this, @"文件被占用无法删除！" + ex.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    var eqlist = (_equipDb.CombatEquipments.Select(eq => eq)).ToList();
                    var vehicles = (_equipDb.CombatVehicles.Select(vh => vh)).ToList();

                    var oe = (_equipDb.OilEngines.Select(o => o)).ToList();

                    var events = (_equipDb.Events.Select(ev => ev)).ToList();

                    EquipAllExcelDataStruct eads = new EquipAllExcelDataStruct()
                    {
                        EquipList = eqlist,
                        VhList = vehicles,
                        OeList = oe,
                        Events = events,
                        //EventDic = eventsd,
                        //MaterialList = material,
                        //EqImgList = eqimgList
                    };
                    ExportData2Excel.ExportAllData(fn, eads);
                    CommonLogHelper.GetInstance("LogInfo").Info($"导出全部设备数据成功");
                    MessageBox.Show(this, @"导出设备数据成功", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception exception)
            {
                CommonLogHelper.GetInstance("LogError").Error(@"导出设备数据失败", exception);
                MessageBox.Show(this, @"导出设备数据失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ExportOne2Excel()
        {
            try
            {
                int? r = dgvEquip.CurrentRow?.Index;
                if (r != null && r >= 0)
                {
                    if (sfdExcel.ShowDialog() == DialogResult.OK)
                    {
                        string fn = sfdExcel.FileName;
                        if (File.Exists(fn))
                        {
                            try
                            {
                                File.Delete(fn);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(this, @"文件被占用无法删除！" + ex.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        string excelid = dgvEquip.Rows[r.Value].Cells["SerialNo"].Value.ToString();
                        var firsteq = (_equipDb.CombatEquipments.Where(eq => eq.SerialNo == excelid)).First();
                        var vehicles = (_equipDb.CombatVehicles.Where(vh => vh.Equipment == excelid)).ToList();
                        var comboevhid = vehicles.FirstOrDefault(a => a.CombineOe);

                        OilEngine oe = null;
                        if (comboevhid != null)
                        {
                            oe = (_equipDb.OilEngines.Where(o => o.Vehicle == comboevhid.SerialNo)).FirstOrDefault();
                        }

                        var events = (_equipDb.Events.Where(ev => ev.Equipment == excelid)).ToList();
                        var material = (_equipDb.Materials.Where(mt => mt.Equipment == excelid)).ToList();
                        //var eventsd = new Dictionary<string, List<EventData>>();
                        //foreach (var ee in events)
                        //{
                        //    var ed = (from d in _equipDB.EventData
                        //              where d.EventsNo == ee.No
                        //              select d).ToList();
                        //    eventsd.Add(ee.No, ed);
                        //}

                        //EquipImageDB eqImgDB = new EquipImageDB();
                        //List<EquipmentImage> eqimgList = (from img in eqImgDB.EquipmentImage
                        //                                  where img.SerialNo == excelid
                        //                                  select img).Take(3).ToList();
                        EquipOneExcelDataStruct eeds = new EquipOneExcelDataStruct()
                        {
                            Equip = firsteq,
                            VhList = vehicles,
                            Oe = oe,
                            Events = events,
                            //EventDic = eventsd,
                            MaterialList = material,
                            //EqImgList = eqimgList
                        };
                        ExportData2Excel.ExportOneData(fn, eeds);
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
            _equip = _equipDb.CombatEquipments.Select(eq => eq);
            FillSelectionData();

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
                DataRefresh(_pageSize, gopage, _equip);
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
                DataRefresh(_pageSize, _lastPage, _equip);
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
                    DataRefresh(_pageSize, _curPage, _equip);
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
                    DataRefresh(_pageSize, _curPage, _equip);
                    CommonLogHelper.GetInstance("LogInfo").Info(@"加载上一页设备数据成功");
                }
                catch (Exception exception)
                {
                    CommonLogHelper.GetInstance("LogError").Error(@"加载上一页设备数据失败", exception);
                    MessageBox.Show(this, @"加载上一页设备数据失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnQueryEvent_Click(object sender, EventArgs e)
        {
            try
            {
                dgvEquip.DataSource = null;
                dgvEquip.Rows.Clear();
                var appointee = _equipDb.Events.Select(ee => ee).AsEnumerable();
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
                    appointee = DbPublicFunction.CompareTimeResult(appointee, "PublishDate", cmbPublishDtTerm.Text, dtpPublish.Value.Date);
                }
                if (!string.IsNullOrEmpty(cmbEventDtTerm1.Text))
                {
                    appointee = DbPublicFunction.CompareTimeResult(appointee, "StartTime", cmbEventDtTerm1.Text, dtpEventDt1.Value.Date);
                }
                if (!string.IsNullOrEmpty(cmbEventDtTerm2.Text))
                {
                    appointee = DbPublicFunction.CompareTimeResult(appointee, "EndTime", cmbEventDtTerm2.Text, dtpEventDt2.Value.Date);
                }
                if (appointee.Any())
                {
                    string eeid = appointee.First().Equipment;
                    var appointeq = _equipDb.CombatEquipments.Where(equipment => equipment.SerialNo == eeid);
                    dgvEquip.DataSource = appointeq.ToList();
                }
                else
                {
                    MessageBox.Show(this, @"没有筛选到相关条件的设备信息，请修改筛选条件", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                CommonLogHelper.GetInstance("LogInfo").Info(@"根据事件条件筛选匹配设备成功");
            }
            catch (Exception exception)
            {
                CommonLogHelper.GetInstance("LogError").Error(@"根据事件条件筛选匹配设备失败", exception);
                MessageBox.Show(this, @"根据事件条件筛选匹配设备失败,请将截图和日志发给程序猿排查问题" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnQueryInfo_Click(object sender, EventArgs e)
        {
            try
            {
                dgvEquip.DataSource = null;
                dgvEquip.Rows.Clear();
                var appointeq = _equip.Select(equipment => equipment);
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
                    appointeq = DbPublicFunction.CompareTimeResult(appointeq, "ProductionDate", cmbTimeTerm1.Text, dtpTime1.Value.Date);
                }
                if (!string.IsNullOrEmpty(cmbTimeTerm2.Text))
                {
                    appointeq = DbPublicFunction.CompareTimeResult(appointeq, "ProductionDate", cmbTimeTerm2.Text, dtpTime2.Value.Date);
                }
                if (appointeq.Any())
                {
                    dgvEquip.DataSource = appointeq.ToList();
                }
                else
                {
                    MessageBox.Show(this, @"没有筛选到相关条件的设备信息，请修改筛选条件", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                CommonLogHelper.GetInstance("LogInfo").Info(@"根据设备条件筛选匹配设备成功");
            }
            catch (Exception exception)
            {
                CommonLogHelper.GetInstance("LogError").Error(@"根据设备条件筛选匹配设备失败", exception);
                MessageBox.Show(this, @"根据设备条件筛选匹配设备失败,请将截图和日志发给程序猿排查问题" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRestEvent_Click(object sender, EventArgs e)
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
                    CommonLogHelper.GetInstance("LogError").Error(@"切换页数后加载设备数据失败", exception);
                    MessageBox.Show(this, @"加载设备数据失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DataRefresh(int pagesize, int curpage, IEnumerable<CombatEquipment> iquery)
        {
            int all = iquery.Count();
            _lastPage = (int)Math.Ceiling((double)all / _pageSize);
            var equippage = QueryByPage(pagesize, curpage, iquery);
            _synchContext.Post(a =>
            {
                lbPageInfo.Text = $"总共{all}条记录，当前第{curpage}页，每页{pagesize}条，共{_lastPage}页";
                dgvEquip.DataSource = equippage;
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

        private void EquipMainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SetToolStripVisible?.Invoke();
        }

        private void EquipMainForm_Load(object sender, EventArgs e)
        {
        }

        private void EquipMainForm_Shown(object sender, EventArgs e)
        {
            Thread loadDataThread = new Thread((ThreadStart)delegate
           {
               try
               {
                   SetStatusInfo?.Invoke("正在加载数据");
                   LoadData();
                   CommonLogHelper.GetInstance("LogInfo").Info(@"加载设备数据成功");
                   SetStatusInfo?.Invoke("加载数据成功");
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

        private void FillSelectionData()
        {
            List<string> equipNameList = (_equip.Where(s => !string.IsNullOrEmpty(s.Name)).Select(s => s.Name)).Distinct().ToList();
            List<string> equipSubdepartList = (_equip.Where(s => !string.IsNullOrEmpty(s.SubDepartment))
                .Select(s => s.SubDepartment)).Distinct().ToList();
            List<string> equipMajcatList = (_equip.Where(s => !string.IsNullOrEmpty(s.MajorCategory))
                .Select(s => s.MajorCategory)).Distinct().ToList();
            List<string> equipModelList = (_equip.Where(s => !string.IsNullOrEmpty(s.Model)).Select(s => s.Model)).Distinct().ToList();
            List<string> equipTechcanList = (_equip.Where(s => !string.IsNullOrEmpty(s.Technician))
                .Select(s => s.Technician)).Distinct().ToList();
            List<string> equipManagerList = (_equip.Where(s => !string.IsNullOrEmpty(s.Manager)).Select(s => s.Manager)).Distinct().ToList();
            List<string> equipTechconList = (_equip.Where(s => !string.IsNullOrEmpty(s.TechCondition))
                .Select(s => s.TechCondition)).Distinct().ToList();
            List<string> equipUseconList = (_equip.Where(s => !string.IsNullOrEmpty(s.UseCondition))
                .Select(s => s.UseCondition)).Distinct().ToList();
            List<string> equipFactList = (_equip.Where(s => !string.IsNullOrEmpty(s.Factory)).Select(s => s.Factory)).Distinct().ToList();

            List<string> eventNameList = (_equipDb.Events.Where(s => !string.IsNullOrEmpty(s.Name)).Select(s => s.Name)).Distinct().ToList();
            List<string> eventSpecificList = (_equipDb.Events.Where(s => !string.IsNullOrEmpty(s.SpecificType))
                .Select(s => s.SpecificType)).Distinct().ToList();
            List<string> eventAddressList = (_equipDb.Events.Where(s => !string.IsNullOrEmpty(s.Address))
                .Select(s => s.Address)).Distinct().ToList();
            List<string> eventPublishUnitList = (_equipDb.Events.Where(s => !string.IsNullOrEmpty(s.PublishUnit))
                .Select(s => s.PublishUnit)).Distinct().ToList();
            List<string> eventPublisherList = (_equipDb.Events.Where(s => !string.IsNullOrEmpty(s.Publisher))
                .Select(s => s.Publisher)).Distinct().ToList();

            _synchContext.Post(a =>
            {
                cmbName.DataSource = equipNameList;
                cmbSubDepart.DataSource = equipSubdepartList;
                cmbMajorCategory.DataSource = equipMajcatList;
                cmbModel.DataSource = equipModelList;
                cmbTechnician.DataSource = equipTechcanList;
                cmbManager.DataSource = equipManagerList;
                cmbTechCondition.DataSource = equipTechconList;
                cmbUseCondition.DataSource = equipUseconList;
                cmbFactory.DataSource = equipFactList;

                cmbEventName.DataSource = eventNameList;
                cmbSpecificType.DataSource = eventSpecificList;
                cmbEventAddress.DataSource = eventAddressList;
                cmbPublishUnit.DataSource = eventPublishUnitList;
                cmbPublisher.DataSource = eventPublisherList;

                cmbName.SelectedIndex = -1;
                cmbSubDepart.SelectedIndex = -1;
                cmbMajorCategory.SelectedIndex = -1;
                cmbModel.SelectedIndex = -1;
                cmbTechnician.SelectedIndex = -1;
                cmbManager.SelectedIndex = -1;
                cmbTechCondition.SelectedIndex = -1;
                cmbUseCondition.SelectedIndex = -1;
                cmbFactory.SelectedIndex = -1;

                cmbEventName.SelectedIndex = -1;
                cmbSpecificType.SelectedIndex = -1;
                cmbEventAddress.SelectedIndex = -1;
                cmbPublishUnit.SelectedIndex = -1;
                cmbPublisher.SelectedIndex = -1;
            }, null);
        }

        private void dgvEquip_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            for (int i = 0; i < dgvEquip.RowCount; i++)
            {
                dgvEquip[0, i].Value = i + 1;
                dgvEquip.Rows[i].Cells["MoreInfo"].Value = "详细信息";
            }
        }

        private IList<CombatEquipment> QueryByPage(int pageSize, int curPage, IEnumerable<CombatEquipment> dbRaw)
        {
            List<CombatEquipment> lstEquip = dbRaw.ToList();
            lstEquip.Sort(new AlphanumComparatorFast());
            return lstEquip.Take(pageSize * curPage).Skip(pageSize * (curPage - 1)).ToList();
        }

        private void SaveDataSuccess()
        {
            DataRefresh();
            btnLast_Click(null, null);
        }
    }
}