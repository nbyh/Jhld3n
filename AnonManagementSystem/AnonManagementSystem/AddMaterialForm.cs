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

    public partial class AddMaterialForm : Form
    {
        public delegate void SaveMaterial(Material material);
        public event SaveMaterial SaveMaterialSucess;

        private string _id;
        public string Id
        {
            set { _id = value; }
        }
        
        private bool _add = false;
        public bool Add
        {
            set { _add = value; }
        }

        EquipmentManagementEntities eqEntities = new EquipmentManagementEntities();
        public AddMaterialForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void AddMaterialForm_Load(object sender, EventArgs e)
        {

        }

        private void AddMaterialForm_Shown(object sender, EventArgs e)
        {
            var equip = from eq in eqEntities.Material
                        select eq;
            if (_add)
            {
                cmbShape.SelectedIndex = -1;
            }
            else
            {
                //var appointeq = from eq in equip
                //                where eq.No == _id
                //                select eq;
                //var equipfirst = appointeq.First();
                //cmbName.SelectedItem = equipfirst.Name;
                //cmbModel.SelectedItem = equipfirst.Model;
                //cmbSubDepart.SelectedItem = equipfirst.SubDepartment;
                //tbSerialNo.Text = equipfirst.SerialNo;
                //tbTechRemould.Text = equipfirst.TechRemould;
                //tbOemNo.Text = equipfirst.InventorySpot;
                //cmbTechnician.SelectedItem = equipfirst.Technician;
                //cmbCharger.SelectedItem = equipfirst.Manager;
                //cmbTechCondition.SelectedItem = equipfirst.TechCondition;
                //cmbUseCondition.SelectedItem = equipfirst.UseCondition;
                //cmbMajorCategory.SelectedItem = equipfirst.MajorCategory;
                //cmbFactory.SelectedItem = equipfirst.Factory;
                //dtpTime.Value = equipfirst.FactoryTime.Value;

                //tbMajorComp.Text = equipfirst.MajorComp;
                //tbMainUsage.Text = equipfirst.MainUsage;
                //tbUseMethod.Text = equipfirst.UseMethod;
                //tbPerformIndex.Text = equipfirst.PerformIndex;

            }
        }
    }
}
