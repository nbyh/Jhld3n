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
    public partial class VehicleDetailForm : Form
    {
        private string _eqserialno;
        public VehicleDetailForm()
        {
            InitializeComponent();
        }

        public string Eqserialno
        {
            get { return _eqserialno; }
            set { _eqserialno = value; }
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            CombatVehicles cv = new CombatVehicles()
            {
                Name = cmbName.Text,
                SerialNo = tbSerialNo.Text,
                Model = cmbVehiclesModel.Text,
                VehiclesNo = tbVehiclesNo.Text,
                MotorModel = cmbAutoModel.Text,
                TechCondition = cmbTechCondition.Text,
                Factory = cmbFactory.Text,
                ProductionDate = dtpTime.Value.Date,
                Mass = tbMass.Text,
                Tankage =   tbTankAge.Text,
                OverallSize = tbSize.Text,
                FuelType = cmbFuelType.Text,
                DrivingModel = cmbDriveModel.Text,
                Mileage = tbMileAge.Text,
                Output = tbOutput.Text,
                LicenseCarry = tbLicenseCarry.Text,
                VehicleChargers = cmbCharger.Text,
                VehicleSpotNo = cmbSpot.Text,
                VehicleDescri = tbVehiclesDescri.Text,
                CombineOe = chkCombineOe.Checked,
                Equipment = _eqserialno
            };
            EquipmentManagementEntities eqEntities = new EquipmentManagementEntities();
            eqEntities.CombatVehicles.Add(cv);
            eqEntities.SaveChanges();
        }
    }
}
