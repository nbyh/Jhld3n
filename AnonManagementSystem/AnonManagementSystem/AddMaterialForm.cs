using EquipmentInformationData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace AnonManagementSystem
{
    public partial class AddMaterialForm : Form, IAddModify
    {
        public delegate void SaveMaterial(bool add, int index, Material material);

        public event SaveMaterial SaveMaterialSucess;

        private bool _enableedit = false;
        private string _id;
        public int Index { get; set; }

        public string Id
        {
            set { _id = value; }
        }

        public bool Add { get; set; }

        public bool Enableedit
        {
            set { _enableedit = value; }
        }

        private readonly EquipmentManagementEntities _eqEntities = new EquipmentManagementEntities();
        private readonly SynchronizationContext _synchContext;

        public AddMaterialForm()
        {
            InitializeComponent();
            _synchContext = SynchronizationContext.Current;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Material ma = new Material()
                {
                    No = tbDocNo.Text,
                    Name = tbDocName.Text,
                    PaperSize = cmbShape.Text,
                    Pagination = nUdPages.Value.ToString(),
                    Edition = tbEdition.Text,
                    Volume = nUdCopybook.Value.ToString(),
                    Date = dtpDate.Value.Date,
                    DocumentLink = tbDocumentLink.Text,
                    StoreSpot = tbStoreSpot.Text,
                    Content = tbBrief.Text,
                    Equipment = _id
                };
                SaveMaterialSucess?.Invoke(Add, Index, ma);
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

        private void AddMaterialForm_Load(object sender, EventArgs e)
        {
            cmbShape.SelectedIndex = -1;
        }

        private void AddMaterialForm_Shown(object sender, EventArgs e)
        {
            _synchContext.Post(a =>
            {
                try
                {
                    var material = from eq in _eqEntities.Material
                                   select eq;
                    List<string> sharpList = (from s in material where !string.IsNullOrEmpty(s.PaperSize) select s.PaperSize).Distinct().ToList();
                    cmbShape.DataSource = sharpList;
                    if (!Add)
                    {
                        var appointma = from ma in material
                                        where ma.No.ToString() == _id
                                        select ma;
                        var mafirst = appointma.First();
                        tbDocNo.Text = mafirst.No.ToString();
                        tbDocName.Text = mafirst.Name;
                        cmbShape.Text = mafirst.PaperSize;
                        nUdPages.Value = decimal.Parse(mafirst.Pagination);
                        tbEdition.Text = mafirst.Edition;
                        nUdCopybook.Value = decimal.Parse(mafirst.Volume);
                        dtpDate.Value = mafirst.Date;
                        tbDocumentLink.Text = mafirst.DocumentLink;
                        tbStoreSpot.Text = mafirst.StoreSpot;
                        tbBrief.Text = mafirst.Content;
                        CommonLogHelper.GetInstance("LogInfo").Info($"加载随机材料{_id}成功");
                    }
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
            }, null);
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
    }
}