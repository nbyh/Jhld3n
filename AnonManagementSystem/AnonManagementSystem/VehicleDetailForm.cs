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
        public delegate void SaveVehicle(CombatVehicles combatVehicle);
        public event SaveVehicle SaveVehicleSucess;

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
            EquipmentManagementEntities eqEntities = new EquipmentManagementEntities();
            if (chkCombineOe.Checked)
            {
                OilEngine oe=new OilEngine()
                {
                    OeNo = tbOilEngineNo.Text,
                    OeModel = cmbOeModel.Text,
                    OutPower = tbOePower.Text,
                    TechCondition = cmbTechCondition.Text,
                    WorkHour = nudWorkHour.Value.ToString(),
                    OeFactory = cmbOeFactory.Text,
                    OeDate = dtpOeTime.Value.Date.ToString(),
                    OeOemNo = tbOeOemNo.Text,
                    MotorNo = cmbMotorModel.Text,
                    MotorPower = tbMotorPower.Text,
                    MotorFuel = cmbMotorFuelType.Text,
                    MotorTankage = tbMotorTankage.Text,
                    MotorFactory = cmbMotorFactory.Text,
                    MotorDate = dtpMotorTime.Value.Date.ToString(),
                    MotorOemNo = tbMotorOemNo.Text,
                    FaultDescri = tbOeFailDetail.Text,
                    Vehicle = tbVehiclesNo.Text
                };
                eqEntities.OilEngine.Add(oe);
            }
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
                Tankage = tbTankAge.Text,
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
            eqEntities.CombatVehicles.Add(cv);
            eqEntities.SaveChanges();
            SaveVehicleSucess(cv);
        }

        private void chkCombineOe_CheckedChanged(object sender, EventArgs e)
        {
            tabOilEngine.Parent = chkCombineOe.Checked ? tabCtrlVehicle : null;
        }

        private void VehicleDetailForm_Load(object sender, EventArgs e)
        {
            tabOilEngine.Parent = null;
        }
    }
}
