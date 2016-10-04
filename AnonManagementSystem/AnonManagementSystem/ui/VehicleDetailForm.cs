﻿using EquipmentInformationData;
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

        public delegate void SaveVehicle(bool add, int index, CombatVehicle combatVehicle, List<VehiclesImage> viList, OilEngine oilEngine, List<OilEngineImage> oiList);

        public event SaveVehicle SaveVehicleSucess;

        private List<VehiclesImage> _vehiclesImagesList = new List<VehiclesImage>();
        private List<OilEngineImage> _oilImagesList = new List<OilEngineImage>();
        private CombatVehicle _comvh;
        private OilEngine _oe;
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

        public List<VehiclesImage> VehiclesImagesList
        {
            get { return _vehiclesImagesList; }
            set { _vehiclesImagesList = value; }
        }

        public List<OilEngineImage> OilImagesList
        {
            get { return _oilImagesList; }
            set { _oilImagesList = value; }
        }

        public CombatVehicle Comvh
        {
            get { return _comvh; }
            set { _comvh = value; }
        }

        public OilEngine Oe
        {
            get { return _oe; }
            set { _oe = value; }
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            try
            {
                _comvh = new CombatVehicle()
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
                    OverallSize = masktbSize.Text,
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
                    _oe = new OilEngine()
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
                    SaveVehicleSucess?.Invoke(Add, Index, _comvh, _vehiclesImagesList,_oe, _oilImagesList);
                }
                else
                {
                    SaveVehicleSucess?.Invoke(Add, Index, _comvh, _vehiclesImagesList, null, null);
                }
                CommonLogHelper.GetInstance("LogInfo").Info(@"车辆数据保存成功");
                MessageBox.Show(this, @"车辆数据保存成功", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception exception)
            {
                CommonLogHelper.GetInstance("LogError").Error(@"车辆数据保存失败", exception);
                MessageBox.Show(this, @"车辆数据保存失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkCombineOe_CheckedChanged(object sender, EventArgs e)
        {
            tabOilEngine.Parent = chkCombineOe.Checked ? tabCtrlVehicle : null;
        }

        private void VehicleDetailForm_Load(object sender, EventArgs e)
        {
            tbSerialNo.Enabled = Add;
            tabOilEngine.Parent = null;
        }

        private void tsbAddOeImages_Click(object sender, EventArgs e)
        {
            if (ofdImage.ShowDialog() == DialogResult.OK)
            {
                string imgpath = ofdImage.FileName;
                if (MainPublicFunction.CheckImgCondition(imgpath))
                {
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
                        ilvOe.AddImages(oiImg.Name, img);
                    }
                }
            }
        }

        private void tsbAddImages_Click(object sender, EventArgs e)
        {
            if (ofdImage.ShowDialog() == DialogResult.OK)
            {
                string imgpath = ofdImage.FileName;
                if (MainPublicFunction.CheckImgCondition(imgpath))
                {
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
                        ilvVehicle.AddImages(cvImag.Name, img);
                    }
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
            Thread loadVhDataThread = new Thread((ThreadStart)delegate
           {
               try
               {
                   if (!Add)
                   {
                       Dictionary<string, Image> vhimgdic = new Dictionary<string, Image>();
                       foreach (var equipmentImage in _vehiclesImagesList)
                       {
                           using (MemoryStream ms = new MemoryStream(equipmentImage.Images))
                           {
                               Image img = Image.FromStream(ms);
                               vhimgdic.Add(equipmentImage.Name, img);
                           }
                       }
                       _synchContext.Post(a =>
                       {
                           cmbName.Text = _comvh.Name;
                           tbSerialNo.Text = _comvh.SerialNo;
                           cmbVehiclesModel.Text = _comvh.Model;
                           tbVehiclesNo.Text = _comvh.VehiclesNo;
                           cmbAutoModel.Text = _comvh.MotorModel;
                           cmbTechCondition.Text = _comvh.TechCondition;
                           cmbFactory.Text = _comvh.Factory;
                           dtpTime.Value = _comvh.ProductionDate;
                           tbMass.Text = _comvh.Mass;
                           tbTankAge.Text = _comvh.Tankage;
                           masktbSize.Text = _comvh.OverallSize;
                           cmbFuelType.Text = _comvh.FuelType;
                           cmbDriveModel.Text = _comvh.DrivingModel;
                           tbMileAge.Text = _comvh.Mileage;
                           tbOutput.Text = _comvh.Output;
                           tbLicenseCarry.Text = _comvh.LicenseCarry;
                           cmbCharger.Text = _comvh.VehicleChargers;
                           cmbSpot.Text = _comvh.VehicleSpotNo;
                           tbVehiclesDescri.Text = _comvh.VehicleDescri;
                           chkCombineOe.Checked = _comvh.CombineOe;
                           ilvVehicle.ImgDictionary = vhimgdic;
                           ilvVehicle.ShowImages();
                       }, null);
                       if (_comvh.CombineOe)
                       {

                           Dictionary<string, Image> oeimgdic = new Dictionary<string, Image>();
                           foreach (var equipmentImage in _oilImagesList)
                           {
                               using (MemoryStream ms = new MemoryStream(equipmentImage.Images))
                               {
                                   Image img = Image.FromStream(ms);
                                   oeimgdic.Add(equipmentImage.Name, img);
                               }
                           }
                           _synchContext.Post(a =>
                           {
                               tbOilEngineNo.Text = _oe.OeNo;
                               cmbOeModel.Text = _oe.OeModel;
                               tbOePower.Text = _oe.OutPower;
                               cmbTechCondition.Text = _oe.TechCondition;
                               nudWorkHour.Value = int.Parse(_oe.WorkHour);
                               cmbOeFactory.Text = _oe.OeFactory;
                               dtpOeTime.Value = _oe.OeDate;
                               tbOeOemNo.Text = _oe.OeOemNo;
                               cmbMotorModel.Text = _oe.MotorModel;
                               tbMotorPower.Text = _oe.MotorPower;
                               cmbMotorFuelType.Text = _oe.MotorFuel;
                               tbMotorTankage.Text = _oe.MotorTankage;
                               cmbMotorFactory.Text = _oe.MotorFactory;
                               dtpMotorTime.Value = _oe.MotorDate;
                               tbMotorOemNo.Text = _oe.MotorOemNo;
                               tbOeFailDetail.Text = _oe.FaultDescri;
                               tbVehiclesNo.Text = _oe.Vehicle;
                               ilvOe.ImgDictionary = oeimgdic;
                               ilvOe.ShowImages();
                           }, null);
                       }
                   }
                   CommonLogHelper.GetInstance("LogInfo").Info($"加载车辆数据{_id}成功");
               }
               catch (Exception exception)
               {
                   _synchContext.Post(a =>
                   {
                       if (Add)
                       {
                           CommonLogHelper.GetInstance("LogError").Error(@"打开添加车辆数据失败", exception);
                           MessageBox.Show(this, @"打开添加车辆数据失败" + exception.Message, @"错误", MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
                       }
                       else
                       {
                           CommonLogHelper.GetInstance("LogError").Error($"加载车辆数据{_id}失败", exception);
                           MessageBox.Show(this, $"加载车辆数据{_id}失败" + exception.Message, @"错误", MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
                       }
                   }, null);
               }
           })
            { IsBackground = true };
            loadVhDataThread.Start();
        }

        private void Num_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = MainPublicFunction.JudgeNumCharKeys(e.KeyChar);
        }
    }
}