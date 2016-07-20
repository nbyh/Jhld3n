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
            更新图片ToolStripMenuItem.Text = pbEquipFront.Image == null ? "添加图片" : "更新图片";
        }

        private void EquipmentDetailForm_Load(object sender, EventArgs e)
        {
            tsbRestore.Visible = !_add;
            Entities equipEntities = new Entities();
            var equip = from eq in equipEntities.CombatEquipment
                        select eq;
            List<string> equipnameList = (from n in equip select n.Name).Distinct().ToList();
            //cmbeqName.DataSource = equipnameList;
            List<string> equipsubdepartList = (from d in equip select d.SubDepartment).Distinct().ToList();
            cmbSubDepart.DataSource = equipsubdepartList;
            List<string> equipModelList = (from s in equip select s.Model).Distinct().ToList();
            cmbModel.DataSource = equipModelList;
            List<string> equipSpotList = (from s in equip select s.InventorySpot).Distinct().ToList();
            cmbSpot.DataSource = equipSpotList;
            if (_add)
            {
                //cmbeqName.SelectedIndex = -1;
                cmbSubDepart.SelectedIndex = -1;
                cmbModel.SelectedIndex = -1;
                cmbSpot.SelectedIndex = -1;
            }
            else
            {
                var appointeq = from eq in equip
                                where eq.SerialNo == _id
                                select eq;
                //cmbeqName.SelectedItem = appointeq.First().Name;
                cmbSubDepart.SelectedItem = appointeq.First().SubDepartment;
                cmbModel.SelectedItem = appointeq.First().Model;
                cmbSpot.SelectedItem = appointeq.First().InventorySpot;
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
                //Name = cmbeqName.Text,
                //SerialNo = tbSerialNo.Text,
                Model = cmbModel.Text,
                //EnableTime = dtpEnableTime.Value.Date,
                SubDepartment = cmbSubDepart.Text,
                //ServiceLife = (long)nudServiceLift.Value,
                InventorySpot = cmbSpot.Text
            };
            Entities eqEntities = new Entities();
            eqEntities.CombatEquipment.Add(ce);
            eqEntities.SaveChanges();
        }

        private void tsbAddVehicle_Click(object sender, EventArgs e)
        {
            VehicleDetailForm vdForm=new VehicleDetailForm();
            vdForm.ShowDialog();
        }

        private void tsbAddTrain_Click(object sender, EventArgs e)
        {
            
        }

        private void 随机资料tsmAdd_Click(object sender, EventArgs e)
        {
            MachineMaterialForm mmForm = new MachineMaterialForm();
            mmForm.ShowDialog();
        }

        private void tsbAddEvents_Click(object sender, EventArgs e)
        {
            EventsForm eFrom=new EventsForm();
            eFrom.ShowDialog();
        }

        private void dgvVideo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
