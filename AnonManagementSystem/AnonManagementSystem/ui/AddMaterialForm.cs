using EquipmentInformationData;
using LinqToDB.DataProvider.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace AnonManagementSystem
{
    public partial class AddMaterialForm : Form, IAddModify
    {
        private readonly EquipmentManagementDB _equipDb = new EquipmentManagementDB(new SQLiteDataProvider(), DbPublicFunction.ReturnDbConnectionString(@"\ZBDataBase\EquipmentManagement.db"));

        private readonly SynchronizationContext _synchContext;

        private bool _enableedit = false;

        private string _id;

        private Material _material;

        public AddMaterialForm()
        {
            InitializeComponent();
            _synchContext = SynchronizationContext.Current;
        }

        public delegate void SaveMaterial(bool add, int index, Material material);

        public event SaveMaterial SaveMaterialSucess;

        public bool Add { get; set; }

        public bool Enableedit
        {
            set { _enableedit = value; }
        }

        public string Id
        {
            set { _id = value; }
        }

        public int Index { get; set; }

        public Material Material1
        {
            get { return _material; }
            set { _material = value; }
        }

        private void AddMaterialForm_Load(object sender, EventArgs e)
        {
            tbDocNo.Enabled = Add;
            cmbShape.SelectedIndex = -1;
        }

        private void AddMaterialForm_Shown(object sender, EventArgs e)
        {
            try
            {
                var qmaterial = _equipDb.Materials.Select(eq => eq);
                List<string> sharpList = (qmaterial.Where(s => !string.IsNullOrEmpty(s.PaperSize))
                    .Select(s => s.PaperSize)).Distinct().ToList();
                _synchContext.Post(a =>
                {
                    cmbShape.DataSource = sharpList;
                    if (!Add)
                    {
                        tbDocNo.Text = _material.No.ToString();
                        tbDocName.Text = _material.Name;
                        cmbShape.SelectedText = _material.PaperSize;
                        nUdPages.Value = decimal.Parse(_material.Pagination);
                        //tbEdition.Text = _material.Edition;
                        nUdCopybook.Value = decimal.Parse(_material.Volume);
                        dtpDate.Value = _material.Date;
                        tbDocumentLink.Text = _material.DocumentLink;
                        tbStoreSpot.Text = _material.StoreSpot;
                        tbBrief.Text = _material.Content;
                    }
                }, null);
                CommonLogHelper.GetInstance("LogInfo").Info($"加载随机材料{_id}成功");
            }
            catch (Exception exception)
            {
                if (Add)
                {
                    CommonLogHelper.GetInstance("LogError").Error(@"打开添加随机材料失败", exception);
                    MessageBox.Show(this, @"打开添加随机材料失败" + exception.Message, @"错误", MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                }
                else
                {
                    CommonLogHelper.GetInstance("LogError").Error($"加载随机材料{_id}失败", exception);
                    MessageBox.Show(this, $"加载随机材料{_id}失败" + exception.Message, @"错误", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            if (ofdMaterial.ShowDialog() == DialogResult.OK)
            {
                tbDocumentLink.Text = ofdMaterial.FileName;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                _material = new Material()
                {
                    No = tbDocNo.Text,
                    Name = tbDocName.Text,
                    PaperSize = cmbShape.Text,
                    Pagination = nUdPages.Value.ToString(),
                    //Edition = tbEdition.Text,
                    Volume = nUdCopybook.Value.ToString(),
                    Date = dtpDate.Value.Date,
                    DocumentLink = tbDocumentLink.Text,
                    StoreSpot = tbStoreSpot.Text,
                    Content = tbBrief.Text,
                    Equipment = _id
                };
                SaveMaterialSucess?.Invoke(Add, Index, _material);
                CommonLogHelper.GetInstance("LogInfo").Info(@"随机材料保存成功");
                MessageBox.Show(this, @"随机材料保存成功", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, @"随机材料保存成功" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CommonLogHelper.GetInstance("LogError").Error(@"随机材料保存成功", exception);
            }
        }

        private void tbDocNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = MainPublicFunction.JudgeNumCharKeys(e.KeyChar);
        }
    }
}