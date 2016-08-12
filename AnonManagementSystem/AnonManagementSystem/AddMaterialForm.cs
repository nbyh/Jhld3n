using EquipmentInformationData;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private bool _add = false;
        public bool Add { get; set; }

        public bool Enableedit
        {
            set { _enableedit = value; }
        }

        private EquipmentManagementEntities eqEntities = new EquipmentManagementEntities();

        public AddMaterialForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
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
        }

        private void AddMaterialForm_Load(object sender, EventArgs e)
        {
        }

        private void AddMaterialForm_Shown(object sender, EventArgs e)
        {
            var material = from eq in eqEntities.Material
                           select eq;
            List<string> sharpList = (from s in material where !string.IsNullOrEmpty(s.PaperSize) select s.PaperSize).Distinct().ToList();
            cmbShape.DataSource = sharpList;
            if (_add)
            {
                cmbShape.SelectedIndex = -1;
            }
            else
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
                dtpDate.Value = DateTime.Now;//todo:null属性要去掉
                tbDocumentLink.Text = mafirst.DocumentLink;
                tbStoreSpot.Text = mafirst.StoreSpot;
                tbBrief.Text = mafirst.Content;
            }
        }

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            if (ofdMaterial.ShowDialog() == DialogResult.OK)
            {
                tbDocumentLink.Text = ofdMaterial.FileName;
            }
        }
    }
}