using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EquipmentInformationData;

namespace AnonManagementSystem
{
    public partial class VehicleDetailForm : Form
    {
        public delegate void SaveVehicle(
            CombatVehicles combatVehicle, List<VehiclesImage> viList, OilEngine oilEngine, List<OilEngineImage> oiList);
        private readonly List<VehiclesImage> _vehiclesImagesList = new List<VehiclesImage>();
        private readonly List<OilEngineImage> _oilImagesList = new List<OilEngineImage>();
        public event SaveVehicle SaveVehicleSucess;

        private string _id;

        public VehicleDetailForm()
        {
            InitializeComponent();
        }

        public string Id
        {
            set { _id = value; }
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            EquipmentManagementEntities eqEntities = new EquipmentManagementEntities();
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
                Equipment = _id
            };
            if (chkCombineOe.Checked)
            {
                OilEngine oe = new OilEngine()
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
                //eqEntities.OilEngine.Add(oe);
                SaveVehicleSucess(cv, _vehiclesImagesList, oe, _oilImagesList);

            }
            else
            {
                SaveVehicleSucess(cv, _vehiclesImagesList, null, null);
            }
        }

        private void chkCombineOe_CheckedChanged(object sender, EventArgs e)
        {
            tabOilEngine.Parent = chkCombineOe.Checked ? tabCtrlVehicle : null;
        }

        private void VehicleDetailForm_Load(object sender, EventArgs e)
        {
            tabOilEngine.Parent = null;
        }

        private void tsbAddOeImages_Click(object sender, EventArgs e)
        {
            if (ofdImage.ShowDialog() == DialogResult.OK)
            {
                string imgpath = ofdImage.FileName;
                FileStream fs = new FileStream(imgpath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                byte[] imgBytes = br.ReadBytes((int)fs.Length);
                fs.Close();
                OilEngineImage oiImg = new OilEngineImage
                {
                    Images = imgBytes,
                    SerialNo = tbSerialNo.Text
                };
                _oilImagesList.Add(oiImg);
            }
        }

        private void tsbAddImages_Click(object sender, EventArgs e)
        {

            if (ofdImage.ShowDialog() == DialogResult.OK)
            {
                string imgpath = ofdImage.FileName;
                FileStream fs = new FileStream(imgpath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                byte[] imgBytes = br.ReadBytes((int)fs.Length);
                fs.Close();
                VehiclesImage cvImag = new VehiclesImage
                {
                    Images = imgBytes,
                    SerialNo = tbSerialNo.Text
                };
                _vehiclesImagesList.Add(cvImag);
            }
        }

        private void tsbDeleteOeImages_Click(object sender, EventArgs e)
        {
            foreach (var oilEngineImage in _oilImagesList)
            {
                if (VScroll)
                {
                    _oilImagesList.Remove(oilEngineImage);
                    return;
                }
                //界面修改
            }
        }

        private void tsbDeleteImages_Click(object sender, EventArgs e)
        {
            foreach (var vehiclesImages in _vehiclesImagesList)
            {
                if (VScroll)
                {
                    _vehiclesImagesList.Remove(vehiclesImages);
                    return;
                }
                //界面修改
            }
        }
    }
}
