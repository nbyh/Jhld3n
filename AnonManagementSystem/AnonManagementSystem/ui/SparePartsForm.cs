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
    public partial class SparePartsForm : Form, IMdiFunction
    {
        private readonly SynchronizationContext _synchContext;

        private bool _enableedit = false;

        private int _pageSize = 20, _curPage = 1, _lastPage = 1;

        private IQueryable<SparePart> _sparePart;

        private SparePartManagementDB _sparePartDb = new SparePartManagementDB(new SQLiteDataProvider(), DbPublicFunction.ReturnDbConnectionString(@"\ZBDataBase\SparePartManagement.db"));

        public SparePartsForm()
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
            SparePartDetailForm spDetailForm = new SparePartDetailForm()
            {
                Enableedit = _enableedit,
                Add = true
            };
            spDetailForm.SaveSuccess += SaveDataSuccess;
            spDetailForm.ShowDialog();
        }

        public void DataDelete()
        {
            if (dgvSparePart.CurrentRow != null)
            {
                if (MessageBox.Show(@"确定要删除该备件？删除后将无法找回！", @"警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        int selectRowIndex = dgvSparePart.CurrentRow.Index;
                        string id = dgvSparePart.Rows[selectRowIndex].Cells["SerialNo"].Value.ToString();
                        _sparePartDb.SpareParts.Where(eqt => eqt.SerialNo == id).Delete();
                        DataRefresh();
                        CommonLogHelper.GetInstance("LogInfo").Info($"删除备件{id}成功");
                        MessageBox.Show(this, @"删除备件成功", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(this, @"删除备件失败" + e.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        CommonLogHelper.GetInstance("LogError").Error(@"删除备件失败", e);
                    }
                }
            }
        }

        public void DataRefresh()
        {
            try
            {
                _sparePartDb = new SparePartManagementDB(new SQLiteDataProvider(), DbPublicFunction.ReturnDbConnectionString(@"\ZBDataBase\SparePartManagement.db"));
                LoadData();
                CommonLogHelper.GetInstance("LogInfo").Info(@"刷新备件数据成功");
            }
            catch (Exception e)
            {
                MessageBox.Show(this, @"刷新备件数据失败" + e.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CommonLogHelper.GetInstance("LogError").Error(@"刷新备件数据失败", e);
            }
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
                            MessageBox.Show(this, @"文件被占用无法删除！" + ex.Message, @"错误", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    }
                    var splist = (from sp in _sparePartDb.SpareParts
                                  select sp).ToList();

                    SpareAllExcelDataStruct sads = new SpareAllExcelDataStruct()
                    {
                        SparePartList = splist,
                    };
                    ExportData2Excel.ExportAllData(fn, sads);
                    CommonLogHelper.GetInstance("LogInfo").Info(@"导出所有备件数据成功");
                    MessageBox.Show(this, @"导出备件数据成功", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception exception)
            {
                CommonLogHelper.GetInstance("LogError").Error(@"导出备件数据失败", exception);
                MessageBox.Show(this, @"导出备件数据失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ExportOne2Excel()
        {
            try
            {
                int? r = dgvSparePart.CurrentRow?.Index;
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
                                MessageBox.Show(this, @"文件被占用无法删除！" + ex.Message, @"错误", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                            }
                        }
                        string excelid = dgvSparePart.Rows[r.Value].Cells["SerialNo"].Value.ToString();
                        var firstsp = (from sp in _sparePartDb.SpareParts
                                       where sp.SerialNo == excelid
                                       select sp).First();
                        //SparePartImagesDB spImgDB = new SparePartImagesDB();
                        //List<SparePartImage> spimgList = (from img in spImgDB.SparePartImage
                        //                                  where img.SerialNo == excelid
                        //                                  select img).Take(3).ToList();
                        SpareOneExcelDataStruct seds = new SpareOneExcelDataStruct()
                        {
                            SparePart = firstsp,
                            //SpImgList = spimgList
                        };
                        ExportData2Excel.ExportOneData(fn, seds);
                        CommonLogHelper.GetInstance("LogInfo").Info($"导出备件数据{excelid}成功");
                        MessageBox.Show(this, @"导出备件数据成功", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception exception)
            {
                CommonLogHelper.GetInstance("LogError").Error(@"导出备件数据失败", exception);
                MessageBox.Show(this, @"导出备件数据失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadData()
        {
            _sparePart = from entity in _sparePartDb.SpareParts
                         select entity;

            FillSelectionData();

            _pageSize = 20;
            _curPage = 1;
            DataRefresh(_pageSize, _curPage, _sparePart);
        }

        private void btnFront_Click(object sender, EventArgs e)
        {
            _curPage = 1;
            try
            {
                DataRefresh(_pageSize, _curPage, _sparePart);
                CommonLogHelper.GetInstance("LogInfo").Info(@"加载首页备件数据成功");
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, @"加载首页备件数据失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CommonLogHelper.GetInstance("LogError").Error(@"加载首页备件数据失败", exception);
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                int gopage = int.Parse(tbPage.Text);
                if (gopage > _lastPage)
                {
                    gopage = _lastPage;
                    tbPage.Text = _lastPage.ToString();
                }
                _curPage = gopage;
                DataRefresh(_pageSize, gopage, _sparePart);
                CommonLogHelper.GetInstance("LogInfo").Info(@"加载备件数据页面成功");
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, @"加载备件数据页面失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CommonLogHelper.GetInstance("LogError").Error(@"加载备件数据页面失败", exception);
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            _curPage = _lastPage;
            try
            {
                DataRefresh(_pageSize, _lastPage, _sparePart);
                CommonLogHelper.GetInstance("LogInfo").Info(@"加载末页备件数据成功");
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, @"加载末页备件数据失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CommonLogHelper.GetInstance("LogError").Error(@"加载末页备件数据失败", exception);
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
                    DataRefresh(_pageSize, _curPage, _sparePart);
                    CommonLogHelper.GetInstance("LogInfo").Info(@"加载下一页备件数据成功");
                }
                catch (Exception exception)
                {
                    MessageBox.Show(this, @"加载下一页备件数据失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    CommonLogHelper.GetInstance("LogError").Error(@"加载下一页备件数据失败", exception);
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
                    DataRefresh(_pageSize, _curPage, _sparePart);
                    CommonLogHelper.GetInstance("LogInfo").Info(@"加载上一页备件数据成功");
                }
                catch (Exception exception)
                {
                    MessageBox.Show(this, @"加载上一页备件数据失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    CommonLogHelper.GetInstance("LogError").Error(@"加载上一页备件数据失败", exception);
                }
            }
        }

        private void btnQueryInfo_Click(object sender, EventArgs e)
        {
            try
            {
                dgvSparePart.DataSource = null;
                dgvSparePart.Rows.Clear();
                var appointsp = _sparePartDb.SpareParts.Select(ee => ee).AsEnumerable();
                if (!string.IsNullOrEmpty(cmbName.Text))
                {
                    appointsp = appointsp.Where(a => a.Name == cmbName.Text);
                }
                if (!string.IsNullOrEmpty(cmbUseType.Text))
                {
                    appointsp = appointsp.Where(a => a.UseType == cmbUseType.Text);
                }
                if (!string.IsNullOrEmpty(cmbModel.Text))
                {
                    appointsp = appointsp.Where(a => a.Model == cmbModel.Text);
                }
                if (!string.IsNullOrEmpty(cmbUseCondition.Text))
                {
                    appointsp = appointsp.Where(a => a.Status == cmbUseCondition.Text);
                }
                if (!string.IsNullOrEmpty(cmbSpot.Text))
                {
                    appointsp = appointsp.Where(a => a.StoreSpot == cmbSpot.Text);
                }
                if (!string.IsNullOrEmpty(cmbFactory.Text))
                {
                    appointsp = appointsp.Where(a => a.Factory == cmbFactory.Text);
                }
                if (!string.IsNullOrEmpty(cmbProDate1.Text))
                {
                    appointsp = DbPublicFunction.CompareTimeResult(appointsp, "ProductionDate", cmbProDate1.Text, dtpProTime1.Value.Date);
                }
                if (!string.IsNullOrEmpty(cmbProDate2.Text))
                {
                    appointsp = DbPublicFunction.CompareTimeResult(appointsp, "ProductionDate", cmbProDate2.Text, dtpProTime2.Value.Date);
                }
                if (!string.IsNullOrEmpty(cmbStore1.Text))
                {
                    appointsp = DbPublicFunction.CompareTimeResult(appointsp, "StoreDate", cmbStore1.Text, dtpStore1.Value.Date);
                }
                if (!string.IsNullOrEmpty(cmbStore2.Text))
                {
                    appointsp = DbPublicFunction.CompareTimeResult(appointsp, "StoreDate", cmbStore2.Text, dtpStore2.Value.Date);
                }
                if (appointsp.Any())
                {
                    dgvSparePart.DataSource = appointsp.ToList();
                }
                else
                {
                    MessageBox.Show(this, @"没有筛选到相关条件的备件信息，请修改筛选条件", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                CommonLogHelper.GetInstance("LogInfo").Info(@"根据备件条件筛选匹配备件成功");
            }
            catch (Exception exception)
            {
                CommonLogHelper.GetInstance("LogError").Error(@"根据备件条件筛选匹配备件失败", exception);
                MessageBox.Show(this, @"根据备件条件筛选匹配备件失败,请将截图和日志发给程序猿排查问题" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRestInfo_Click(object sender, EventArgs e)
        {
            cmbName.SelectedIndex = -1;
            cmbModel.SelectedIndex = -1;
            cmbUseCondition.SelectedIndex = -1;
            cmbFactory.SelectedIndex = -1;
            cmbSpot.SelectedIndex = -1;
            cmbUseType.SelectedIndex = -1;
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
            try
            {
                if (cmbPageSize.SelectedIndex > -1)
                {
                    _pageSize = int.Parse(cmbPageSize.SelectedItem.ToString());
                    DataRefresh(_pageSize, _curPage, _sparePart);
                }
                CommonLogHelper.GetInstance("LogInfo").Info(@"切换页数后加载备件数据成功");
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, @"切换页数后加载备件数据失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CommonLogHelper.GetInstance("LogError").Error(@"切换页数后加载备件数据失败", exception);
            }
        }

        private void DataRefresh(int pagesize, int curpage, IQueryable<SparePart> iquery)
        {
            int all = iquery.Count();
            _lastPage = (int)Math.Ceiling((double)all / _pageSize);
            var equippage = QueryByPage(pagesize, curpage, iquery);
            _synchContext.Post(a =>
            {
                lbPageInfo.Text = $"总共{all}条记录，当前第{curpage}页，每页{pagesize}条，共{_lastPage}页";
                dgvSparePart.DataSource = equippage.ToList();
            }, null);
        }

        private void dgvSparePart_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvSparePart.Columns[e.ColumnIndex].Name.Equals("MoreInfo"))
            {
                SparePartDetailForm spDetailForm = new SparePartDetailForm()
                {
                    Enableedit = _enableedit,
                    Add = false,
                    Id = dgvSparePart.Rows[e.RowIndex].Cells["SerialNo"].Value.ToString()
                };
                spDetailForm.SaveSuccess += SaveDataSuccess;
                spDetailForm.Show();
            }
        }

        private void FillSelectionData()
        {
            List<string> equipNameList = (_sparePart.Where(s => !string.IsNullOrEmpty(s.Name)).Select(s => s.Name)).Distinct().ToList();
            cmbName.DataSource = equipNameList;
            List<string> equipModelList = (_sparePart.Where(s => !string.IsNullOrEmpty(s.Model)).Select(s => s.Model)).Distinct().ToList();
            cmbModel.DataSource = equipModelList;
            List<string> equipUseconList = (_sparePart.Where(s => !string.IsNullOrEmpty(s.Status)).Select(s => s.Status)).Distinct().ToList();
            cmbUseCondition.DataSource = equipUseconList;
            List<string> equipFactList = (_sparePart.Where(s => !string.IsNullOrEmpty(s.Factory)).Select(s => s.Factory)).Distinct().ToList();
            cmbFactory.DataSource = equipFactList;
            List<string> equipSubdepartList = (_sparePart.Where(s => !string.IsNullOrEmpty(s.StoreSpot))
                .Select(s => s.StoreSpot)).Distinct().ToList();
            cmbSpot.DataSource = equipSubdepartList;
            List<string> equipMajcatList = (_sparePart.Where(s => !string.IsNullOrEmpty(s.UseType))
                .Select(s => s.UseType)).Distinct().ToList();
            cmbUseType.DataSource = equipMajcatList;
            _synchContext.Post(a =>
            {
                cmbName.SelectedIndex = -1;
                cmbModel.SelectedIndex = -1;
                cmbUseCondition.SelectedIndex = -1;
                cmbFactory.SelectedIndex = -1;
                cmbSpot.SelectedIndex = -1;
                cmbUseType.SelectedIndex = -1;
            }, null);
        }

        private IList<SparePart> QueryByPage(int pageSize, int curPage, IQueryable<SparePart> dbRaw)
        {
            List<SparePart> lstEquip = dbRaw.ToList();
            lstEquip.Sort(new AlphanumComparatorFast());
            return lstEquip.Take(pageSize * curPage).Skip(pageSize * (curPage - 1)).ToList();
        }

        private void SaveDataSuccess()
        {
            DataRefresh();
            btnLast_Click(null, null);
        }

        private void SparePartsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SetToolStripVisible?.Invoke();
        }

        private void dgvSparePart_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            for (int i = 0; i < dgvSparePart.RowCount; i++)
            {
                dgvSparePart[0, i].Value = i + 1;
                dgvSparePart.Rows[i].Cells["MoreInfo"].Value = "详细信息";
            }
        }

        private void SparePartsForm_Load(object sender, EventArgs e)
        {
        }

        private void SparePartsForm_Shown(object sender, EventArgs e)
        {
            Thread loadDataThread = new Thread((ThreadStart)delegate
            {
                try
                {
                    SetStatusInfo?.Invoke("正在加载数据");
                    LoadData();
                    CommonLogHelper.GetInstance("LogInfo").Info(@"加载备件数据成功");
                    SetStatusInfo?.Invoke("加载数据成功");
                }
                catch (Exception exception)
                {
                    _synchContext.Post(
                        a =>
                        {
                            MessageBox.Show(this, @"加载备件数据失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }, null);
                    CommonLogHelper.GetInstance("LogError").Error(@"加载备件数据失败", exception);
                }
            })
            { IsBackground = true };
            loadDataThread.Start();
        }
    }
}