using EquipmentInformationData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace AnonManagementSystem
{
    public partial class VehicleDetailForm : Form, IAddModify
    {
        private readonly SynchronizationContext _synchContext;

        public delegate void SaveVehicle(bool add, int index, CombatVehicles combatVehicle, List<VehiclesImage> viList, OilEngine oilEngine, List<OilEngineImage> oiList);

        public event SaveVehicle SaveVehicleSucess;

        private List<VehiclesImage> _vehiclesImagesList = new List<VehiclesImage>();
        private List<OilEngineImage> _oilImagesList = new List<OilEngineImage>();

        private string _id;
        private bool _enableedit = false;

        public VehicleDetailForm()
        {
            InitializeComponent();
            _synchContext = SynchronizationContext.Current;
        }

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

        private void tsbSave_Click(object sender, EventArgs e)
        {
            try
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
                        WorkHour = nudWorkHour.Value.ToString(CultureInfo.InvariantCulture),
                        OeFactory = cmbOeFactory.Text,
                        OeDate = dtpOeTime.Value.Date,
                        OeOemNo = tbOeOemNo.Text,
                        MotorModel = cmbMotorModel.Text,
                        MotorPower = tbMotorPower.Text,
                        MotorFuel = cmbMotorFuelType.Text,
                        MotorTankage = tbMotorTankage.Text,
                        MotorFactory = cmbMotorFactory.Text,
                        MotorDate = dtpMotorTime.Value.Date,
                        MotorOemNo = tbMotorOemNo.Text,
                        FaultDescri = tbOeFailDetail.Text,
                        Vehicle = tbVehiclesNo.Text
                    };
                    SaveVehicleSucess?.Invoke(Add, Index, cv, _vehiclesImagesList, oe, _oilImagesList);
                }
                else
                {
                    SaveVehicleSucess?.Invoke(Add, Index, cv, _vehiclesImagesList, null, null);
                }
                CommonLogHelper.GetInstance("LogInfo").Info(@"车辆数据保存成功");
                MessageBox.Show(this, @"车辆数据保存成功", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, @"车辆数据保存失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CommonLogHelper.GetInstance("LogError").Error(@"车辆数据保存失败", exception);
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
                using (MemoryStream ms = new MemoryStream(imgBytes))
                {
                    Image img = Image.FromStream(ms);
                    ilvOe.ImgDictionary.Add(oiImg.Name, img);
                    ilvOe.AddImages(oiImg.Name, img);
                }
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
                    Name = imgpath,
                    SerialNo = tbSerialNo.Text
                };
                _vehiclesImagesList.Add(cvImag);
                using (MemoryStream ms = new MemoryStream(imgBytes))
                {
                    Image img = Image.FromStream(ms);
                    ilvVehicle.ImgDictionary.Add(cvImag.Name, img);
                    ilvVehicle.AddImages(cvImag.Name, img);
                }
            }
        }

        private void tsbDeleteOeImages_Click(object sender, EventArgs e)
        {
            ilvOe.DeleteImages();
            if (!string.IsNullOrEmpty(ilvOe.DeleteImgKey))
            {
                string key = ilvOe.DeleteImgKey;
                foreach (var ei in _oilImagesList.Where(d => d.Name == key))
                {
                    _oilImagesList.Remove(ei);
                    MessageBox.Show(this, @"图片删除成功", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
        }

        private void tsbDeleteImages_Click(object sender, EventArgs e)
        {
            ilvVehicle.DeleteImages();
            if (!string.IsNullOrEmpty(ilvVehicle.DeleteImgKey))
            {
                string key = ilvVehicle.DeleteImgKey;
                foreach (var ei in _vehiclesImagesList.Where(d => d.Name == key))
                {
                    _vehiclesImagesList.Remove(ei);
                    MessageBox.Show(this, @"图片删除成功", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
        }

        private void VehicleDetailForm_Shown(object sender, EventArgs e)
        {
            _synchContext.Post(a =>
            {
                try
                {
                    if (!Add)
                    {
                        EquipmentManagementEntities eme = new EquipmentManagementEntities();
                        VehiclesImagesEntities vie = new VehiclesImagesEntities();
                        OilEngineImagesEntities oeie = new OilEngineImagesEntities();
                        var vh = (from eh in eme.CombatVehicles
                                  where eh.SerialNo == _id
                                  select eh).First();

                        cmbName.Text = vh.Name;
                        tbSerialNo.Text = vh.SerialNo;
                        cmbVehiclesModel.Text = vh.Model;
                        tbVehiclesNo.Text = vh.VehiclesNo;
                        cmbAutoModel.Text = vh.MotorModel;
                        cmbTechCondition.Text = vh.TechCondition;
                        cmbFactory.Text = vh.Factory;
                        dtpTime.Value = vh.ProductionDate;
                        tbMass.Text = vh.Mass;
                        tbTankAge.Text = vh.Tankage;
                        tbSize.Text = vh.OverallSize;
                        cmbFuelType.Text = vh.FuelType;
                        cmbDriveModel.Text = vh.DrivingModel;
                        tbMileAge.Text = vh.Mileage;
                        tbOutput.Text = vh.Output;
                        tbLicenseCarry.Text = vh.LicenseCarry;
                        cmbCharger.Text = vh.VehicleChargers;
                        cmbSpot.Text = vh.VehicleSpotNo;
                        tbVehiclesDescri.Text = vh.VehicleDescri;
                        chkCombineOe.Checked = vh.CombineOe;
                        _vehiclesImagesList = (from vhimg in vie.VehiclesImage
                                               where vhimg.SerialNo == _id
                                               select vhimg).ToList();
                        if (vh.CombineOe)
                        {
                            var vhoe = (from oe in eme.OilEngine
                                        where oe.Vehicle == _id
                                        select oe).First();

                            tbOilEngineNo.Text = vhoe.OeNo;
                            cmbOeModel.Text = vhoe.OeModel;
                            tbOePower.Text = vhoe.OutPower;
                            cmbTechCondition.Text = vhoe.TechCondition;
                            nudWorkHour.Value = int.Parse(vhoe.WorkHour);
                            cmbOeFactory.Text = vhoe.OeFactory;
                            dtpOeTime.Value = vhoe.OeDate;
                            tbOeOemNo.Text = vhoe.OeOemNo;
                            cmbMotorModel.Text = vhoe.MotorModel;
                            tbMotorPower.Text = vhoe.MotorPower;
                            cmbMotorFuelType.Text = vhoe.MotorFuel;
                            tbMotorTankage.Text = vhoe.MotorTankage;
                            cmbMotorFactory.Text = vhoe.MotorFactory;
                            dtpMotorTime.Value = vhoe.MotorDate;
                            tbMotorOemNo.Text = vhoe.MotorOemNo;
                            tbOeFailDetail.Text = vhoe.FaultDescri;
                            tbVehiclesNo.Text = vhoe.Vehicle;

                            _oilImagesList = (from oeimg in oeie.OilEngineImage
                                              where oeimg.SerialNo == _id
                                              select oeimg).ToList();
                        }
                    }
                    CommonLogHelper.GetInstance("LogInfo").Info($"加载车辆数据{_id}成功");
                }
                catch (Exception exception)
                {
                    if (Add)
                    {
                        MessageBox.Show(this, @"打开添加车辆数据失败" + exception.Message, @"错误", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        CommonLogHelper.GetInstance("LogError").Error(@"打开添加车辆数据失败", exception);
                    }
                    else
                    {
                        MessageBox.Show(this, $"加载车辆数据{_id}失败" + exception.Message, @"错误", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        CommonLogHelper.GetInstance("LogError").Error($"加载车辆数据{_id}失败", exception);
                    }
                }
            }, null);
        }
    }
}