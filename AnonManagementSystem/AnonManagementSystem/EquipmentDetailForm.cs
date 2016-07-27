using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EquipmentInformationData;

namespace AnonManagementSystem
{
    public partial class EquipmentDetailForm : Form
    {
        private bool _add = false;
        private bool _enableedit = false;
        private string _id;

        public EquipmentDetailForm()
        {
            InitializeComponent();
        }

        public bool Add
        {
            set { _add = value; }
        }

        public bool Enableedit
        {
            set { _enableedit = value; }
        }

        public string Id
        {
            set { _id = value; }
        }

        private void cmsPicture_Opening(object sender, CancelEventArgs e)
        {
        }

        private void EquipmentDetailForm_Load(object sender, EventArgs e)
        {
            tsbRestore.Visible = !_add;
            EquipmentManagementEntities equipEntities = new EquipmentManagementEntities();
            var equip = from eq in equipEntities.CombatEquipment
                        select eq;
            List<string> equipnameList = (from n in equip select n.Name).Distinct().ToList();
            //cmbeqName.DataSource = equipnameList;
            List<string> equipsubdepartList = (from d in equip select d.SubDepartment).Distinct().ToList();
            cmbSubDepart.DataSource = equipsubdepartList;
            List<string> equipModelList = (from s in equip select s.Model).Distinct().ToList();
            cmbModel.DataSource = equipModelList;
            List<string> equipSpotList = (from s in equip select s.InventorySpot).Distinct().ToList();
            if (_add)
            {
                //cmbeqName.SelectedIndex = -1;
                cmbSubDepart.SelectedIndex = -1;
                cmbModel.SelectedIndex = -1;
            }
            else
            {
                var appointeq = from eq in equip
                                where eq.SerialNo == _id
                                select eq;
                //cmbeqName.SelectedItem = appointeq.First().Name;
                cmbSubDepart.SelectedItem = appointeq.First().SubDepartment;
                cmbModel.SelectedItem = appointeq.First().Model;
                //tbSerialNo.Text = appointeq.First().SerialNo;
                //dtpEnableTime.Value = (DateTime)appointeq.First().EnableTime;
                //nudServiceLift.Value = (decimal)appointeq.First().ServiceLife;
            }
            tsDetail.Enabled = gbBaseInfo.Enabled = 更新图片ToolStripMenuItem.Enabled = _enableedit;
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            CombatEquipment ce = new CombatEquipment()
            {
                Name = cmbName.Text,
                SerialNo = tbSerialNo.Text,
                MajorCategory = cmbMajorCategory.Text,
                Model = cmbModel.Text,
                SubDepartment = cmbSubDepart.Text,
                TechCondition = cmbTechCondition.Text,
                UseCondition = cmbUseCondition.Text,
                Factory = cmbFactory.Text,
                FactoryTime = dtpTime.Value.Date,

                MajorComp = tbMajorComp.Text,
                MainUsage = tbMainUsage.Text,
                PerformIndex = tbPerformIndex.Text,
                UseMethod = tbUseMethod.Text,
                Manager = cmbCharger.Text,
                Technician = cmbTechnician.Text,
                TechRemould = tbTechRemould.Text,
                SetupVideo = tbSetupVideo.Text
            };
            EquipmentManagementEntities eqEntities = new EquipmentManagementEntities();
            eqEntities.CombatEquipment.Add(ce);
            eqEntities.SaveChanges();
        }

        private void tsbAddVehicle_Click(object sender, EventArgs e)
        {
            VehicleDetailForm vdForm = new VehicleDetailForm()
            {
                Eqserialno = tbSerialNo.Text
            };
            vdForm.ShowDialog();
        }

        private void tsbAddTrain_Click(object sender, EventArgs e)
        {

        }

        private void 随机资料tsmAdd_Click(object sender, EventArgs e)
        {
            AddMaterialForm mmForm = new AddMaterialForm();
            mmForm.ShowDialog();
        }

        private void tsbAddEvents_Click(object sender, EventArgs e)
        {
            EventsForm eFrom = new EventsForm();
            eFrom.ShowDialog();
        }

        private void dgvVideo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void toolStrip5_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void tsbAddMaterial_Click(object sender, EventArgs e)
        {

        }

        private void tsbDeleteMaterial_Click(object sender, EventArgs e)
        {

        }

        private void tsbDeleteEvents_Click(object sender, EventArgs e)
        {

        }

        private void tsbAddImages_Click(object sender, EventArgs e)
        {

        }

        private void tsbDeleteImage_Click(object sender, EventArgs e)
        {

        }
    }
}
