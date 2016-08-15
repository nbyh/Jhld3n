using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using EquipmentInformationData;

namespace AnonManagementSystem
{
    public partial class SparePartsForm : Form, IMdiFunction
    {
        private readonly SynchronizationContext _synchContext;
        private bool _enableedit = false;
        private SparePartManagementEntities _sparePartEntities = new SparePartManagementEntities();
        private int _pageSize = 20, _curPage = 1, _lastPage = 1;
        private DbRawSqlQuery<SpareParts> _sparePart;

        public SparePartsForm()
        {
            InitializeComponent();
            _synchContext = SynchronizationContext.Current;
        }

        private void SaveDataSuccess()
        {
            DataRefresh();
            btnLast_Click(null, null);
        }

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
                        dgvSparePart.Rows.RemoveAt(selectRowIndex);
                        string id = dgvSparePart.Rows[selectRowIndex].Cells["SerialNo"].Value.ToString();
                        var eq = (from eqt in _sparePartEntities.SpareParts
                            where eqt.SerialNo == id
                            select eqt).First();
                        _sparePartEntities.SpareParts.Remove(eq);
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
                _sparePartEntities = new SparePartManagementEntities();
                LoadData();
                CommonLogHelper.GetInstance("LogInfo").Info(@"刷新备件数据成功");
            }
            catch (Exception e)
            {
                MessageBox.Show(this, @"刷新备件数据失败" + e.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CommonLogHelper.GetInstance("LogError").Error(@"刷新备件数据失败", e);
            }
        }
        
        public void LoadData()
        {
            string cmds = "select * from SpareParts";
            _sparePart = _sparePartEntities.Database.SqlQuery<SpareParts>(cmds);

            List<string> equipNameList = (from s in _sparePart where !string.IsNullOrEmpty(s.Name) select s.Name).Distinct().ToList();
            cmbName.DataSource = equipNameList;
            List<string> equipModelList = (from s in _sparePart where !string.IsNullOrEmpty(s.Model) select s.Model).Distinct().ToList();
            cmbModel.DataSource = equipModelList;
            List<string> equipUseconList = (from s in _sparePart where !string.IsNullOrEmpty(s.Statue) select s.Statue).Distinct().ToList();
            cmbUseCondition.DataSource = equipUseconList;
            List<string> equipFactList = (from s in _sparePart where !string.IsNullOrEmpty(s.Factory) select s.Factory).Distinct().ToList();
            cmbFactory.DataSource = equipFactList;
            List<string> equipSubdepartList = (from s in _sparePart where !string.IsNullOrEmpty(s.StoreSpot) select s.StoreSpot).Distinct().ToList();
            cmbSpot.DataSource = equipSubdepartList;
            List<string> equipMajcatList = (from s in _sparePart where !string.IsNullOrEmpty(s.UseType) select s.UseType).Distinct().ToList();
            cmbUseType.DataSource = equipMajcatList;
            
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

        private void DataRefresh(int pagesize, int curpage, DbRawSqlQuery<SpareParts> iquery)
        {
            int all = iquery.Count();
            _lastPage = (int)Math.Ceiling((double)all / _pageSize);
            var equippage = QueryByPage(pagesize, curpage, iquery);
            _synchContext.Post(a =>
            {
                lbPageInfo.Text = $"总共{all}条记录，当前第{curpage}页，每页{pagesize}条，共{_lastPage}页";
                dgvSparePart.DataSource = equippage.ToList();
                for (int i = 0; i < dgvSparePart.RowCount; i++)
                {
                    dgvSparePart[0, i].Value = i + 1;
                    dgvSparePart.Rows[i].Cells["MoreInfo"].Value = "查看图片";
                }
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

        private void SparePartsForm_Load(object sender, EventArgs e)
        {
        }

        private void btnQueryInfo_Click(object sender, EventArgs e)
        {
            dgvSparePart.Rows.Clear();
            IEnumerable<SpareParts> appointsp = new List<SpareParts>();
            if (!string.IsNullOrEmpty(cmbName.Text))
            {
                appointsp = from sp in _sparePart
                            where sp.Name == cmbName.Text
                            select sp;
            }
            //todo:其他条件
            if (appointsp.Any())
            {
                dgvSparePart.DataSource = appointsp;
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
        
        private void SparePartsForm_Shown(object sender, EventArgs e)
        {
            Thread loadDataThread = new Thread((ThreadStart)delegate
            {
                try
                {
                    LoadData();
                    CommonLogHelper.GetInstance("LogInfo").Info(@"加载备件数据成功");
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

        private IList<SpareParts> QueryByPage(int pageSize, int curPage, DbRawSqlQuery<SpareParts> dbRaw)
        {
            return dbRaw.OrderBy(s => s.SerialNo).Take(pageSize * curPage).Skip(pageSize * (curPage - 1)).ToList();
        }
    }
}