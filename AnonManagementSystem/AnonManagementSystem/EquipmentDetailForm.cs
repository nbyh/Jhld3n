using EquipmentInformationData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace AnonManagementSystem
{
    public partial class EquipmentDetailForm : Form, IAddModify
    {
        public delegate void SaveChangeSuccess();

        public event SaveChangeSuccess SaveSuccess;

        private readonly SynchronizationContext _synchContext;
        private List<CombatVehicles> _comVehList = new List<CombatVehicles>();
        private readonly EquipImageEntities _equipImageEntities = new EquipImageEntities();
        private List<EquipmentImage> _equipImageList = new List<EquipmentImage>();
        private readonly List<EventData> _eventDataList = new List<EventData>();
        private readonly EventsImagesEntities _eventsImageEntities = new EventsImagesEntities();
        private readonly List<EventsImage> _eventsImgList = new List<EventsImage>();
        private List<Events> _eventsList = new List<Events>();
        private List<Material> _materList = new List<Material>();
        private readonly OilEngineImagesEntities _oilImageEntities = new OilEngineImagesEntities();
        private readonly List<OilEngineImage> _oilImgList = new List<OilEngineImage>();
        private readonly VehiclesImagesEntities _vehiclesImageEntities = new VehiclesImagesEntities();
        private readonly List<VehiclesImage> _vehImgList = new List<VehiclesImage>();
        private bool _enableedit;
        private EquipmentManagementEntities _equipEntities = new EquipmentManagementEntities();
        private string _id;
        private OilEngine _oilEngines;

        public EquipmentDetailForm()
        {
            InitializeComponent();
            _synchContext = SynchronizationContext.Current;
        }

        public bool Add { get; set; }

        public bool Enableedit
        {
            set { _enableedit = value; }
        }

        public string Id
        {
            set { _id = value; }
        }

        public int Index { get; set; }

        private void AddEventsSucess(bool add, int index, Events events, List<EventData> eventdatalist,
            List<EventsImage> eventimglist)
        {
            if (add)
            {
                _eventsList.Add(events);
                dgvEvents.DataSource = _eventsList;
                _eventDataList.AddRange(eventdatalist);
                _eventsImgList.AddRange(eventimglist);
                for (int i = 0; i < dgvEvents.RowCount; i++)
                {
                    dgvEvents[0, i].Value = i + 1;
                    dgvEvents[7, i].Value = "详细信息";
                }
            }
            else
            {
                #region 界面更新

                dgvEvents.Rows[index].Cells["No"].Value = events.No;
                dgvEvents.Rows[index].Cells["Name"].Value = events.Name;
                dgvEvents.Rows[index].Cells["StartTime"].Value = events.StartTime;
                dgvEvents.Rows[index].Cells["Address"].Value = events.Address;
                dgvEvents.Rows[index].Cells["EndTime"].Value = events.EndTime;
                dgvEvents.Rows[index].Cells["SpecificType"].Value = events.SpecificType;

                #endregion 界面更新

                #region 活动事件更新

                var pointevent = (from ev in _equipEntities.Events
                                  where ev.No == events.No
                                  select ev).First();
                pointevent.Name = events.Name;
                pointevent.StartTime = events.StartTime;
                pointevent.Address = events.Address;
                pointevent.EndTime = events.EndTime;
                pointevent.EventType = events.EventType;
                pointevent.SpecificType = events.SpecificType;
                pointevent.Code = events.Code;
                pointevent.PublishUnit = events.PublishUnit;
                pointevent.PublishDate = events.PublishDate;
                pointevent.Publisher = events.Publisher;
                pointevent.According = events.According;
                pointevent.PeopleDescri = events.PeopleDescri;
                pointevent.ProcessDescri = events.ProcessDescri;
                pointevent.HandleStep = events.HandleStep;
                pointevent.Problem = events.Problem;
                pointevent.Remarks = events.Remarks;
                pointevent.Equipment = events.Equipment;

                #endregion 活动事件更新

                #region 活动事件图片更新

                List<string> deinoList = eventimglist.Select(ed => ed.Name).ToList();
                var apointei = from ed in _eventsImageEntities.EventsImage
                               where ed.SerialNo == events.No
                               select ed;
                List<string> seinoList = apointei.Select(ed => ed.Name).ToList();
                if (eventimglist.Any())
                {
                    foreach (var data in eventimglist)
                    {
                        if (!seinoList.Contains(data.Name))
                        {
                            //todo：增加
                            _eventsImageEntities.EventsImage.Add(data);
                        }
                    }
                }
                if (apointei.Any())
                {
                    foreach (var data in apointei)
                    {
                        if (!deinoList.Contains(data.Name))
                        {
                            _eventsImageEntities.EventsImage.Remove(data);
                        }
                    }
                }

                #endregion 活动事件图片更新

                #region 事件数据更新

                List<string> devdnoList = eventdatalist.Select(ed => ed.ID).ToList();
                var apointed = from ed in _equipEntities.EventData
                               where ed.EventsNo == events.No
                               select ed;
                List<string> sevdnoList = apointed.Select(ed => ed.ID).ToList();
                if (eventdatalist.Any())
                {
                    foreach (var data in eventdatalist)
                    {
                        if (!sevdnoList.Contains(data.ID))
                        {
                            //todo：增加
                            _equipEntities.EventData.Add(data);
                        }
                        else
                        {
                            //todo：修改
                            var evd = (from ed in _equipEntities.EventData
                                       where ed.EventsNo == events.No && ed.ID == data.ID
                                       select ed).First();
                            evd.Name = data.Name;
                            evd.Spot = data.Spot;
                            evd.EventsNo = data.EventsNo;
                        }
                    }
                }
                if (apointed.Any())
                {
                    foreach (var data in apointed)
                    {
                        if (!devdnoList.Contains(data.ID))
                        {
                            //todo:删除
                            _equipEntities.EventData.Remove(data);
                        }
                    }
                }

                #endregion 事件数据更新
            }
        }

        private void AddMaterialSucess(bool add, int index, Material material)
        {
            if (add)
            {
                _materList.Add(material);
                dgvMaterial.DataSource = _materList;
                for (int i = 0; i < dgvMaterial.RowCount; i++)
                {
                    dgvMaterial[0, i].Value = i + 1;
                    dgvMaterial[8, i].Value = "详细信息";
                }
            }
            {
                #region 界面更新

                dgvMaterial.Rows[index].Cells["MaterialNo"].Value = material.No;
                dgvMaterial.Rows[index].Cells["MaterialName"].Value = material.Name;
                dgvMaterial.Rows[index].Cells["MaterialEdition"].Value = material.Edition;
                dgvMaterial.Rows[index].Cells["MaterialPagination"].Value = material.Pagination;
                dgvMaterial.Rows[index].Cells["PaginationDate"].Value = material.Date;
                dgvMaterial.Rows[index].Cells["PaginationSpot"].Value = material.StoreSpot;
                dgvMaterial.Rows[index].Cells["DocumentLink"].Value = material.DocumentLink;

                #endregion 界面更新

                #region 材料数据更新

                var pointmaterial = (from ma in _equipEntities.Material
                                     where ma.No == material.No
                                     select ma).First();
                pointmaterial.Name = material.Name;
                pointmaterial.Type = material.Type;
                pointmaterial.PaperSize = material.PaperSize;
                pointmaterial.Pagination = material.Pagination;
                pointmaterial.Edition = material.Edition;
                pointmaterial.Volume = material.Volume;
                pointmaterial.Date = material.Date;
                pointmaterial.DocumentLink = material.DocumentLink;
                pointmaterial.StoreSpot = material.StoreSpot;
                pointmaterial.Content = material.Content;
                pointmaterial.Equipment = material.Equipment;

                #endregion 材料数据更新
            }
        }

        private void AddVehicleSucess(bool add, int index, CombatVehicles combatVehicles,
            List<VehiclesImage> vehiclesImgList, OilEngine oilEngine, List<OilEngineImage> oilImgList)
        {
            if (add)
            {
                #region 增加

                _comVehList.Add(combatVehicles);
                dgvCombatVehicles.DataSource = _comVehList;
                _vehImgList.AddRange(vehiclesImgList);
                if (combatVehicles.CombineOe)
                {
                    _oilEngines = oilEngine;
                    _oilImgList.AddRange(oilImgList);
                }
                for (int i = 0; i < dgvCombatVehicles.RowCount; i++)
                {
                    dgvCombatVehicles[0, i].Value = i + 1;
                    dgvCombatVehicles[9, i].Value = "详细信息";
                }

                #endregion 增加
            }
            else
            {
                #region 界面更新

                dgvCombatVehicles.Rows[index].Cells["SerialNo"].Value = combatVehicles.SerialNo;
                dgvCombatVehicles.Rows[index].Cells["cvName"].Value = combatVehicles.Name;
                dgvCombatVehicles.Rows[index].Cells["cvNo"].Value = combatVehicles.VehiclesNo;
                dgvCombatVehicles.Rows[index].Cells["cvModel"].Value = combatVehicles.Model;
                dgvCombatVehicles.Rows[index].Cells["cvFactory"].Value = combatVehicles.Factory;
                dgvCombatVehicles.Rows[index].Cells["cvProductionDate"].Value = combatVehicles.ProductionDate;
                dgvCombatVehicles.Rows[index].Cells["cvMotorModel"].Value = combatVehicles.MotorModel;
                dgvCombatVehicles.Rows[index].Cells["cvTechCondition"].Value = combatVehicles.TechCondition;

                #endregion 界面更新

                #region 车辆更新

                var pointcv = (from cv in _equipEntities.CombatVehicles
                               where cv.SerialNo == combatVehicles.SerialNo
                               select cv).First();
                pointcv.Name = combatVehicles.Name;
                pointcv.Model = combatVehicles.Model;
                pointcv.VehiclesNo = combatVehicles.VehiclesNo;
                pointcv.MotorModel = combatVehicles.MotorModel;
                pointcv.TechCondition = combatVehicles.TechCondition;
                pointcv.Factory = combatVehicles.Factory;
                pointcv.ProductionDate = combatVehicles.ProductionDate;
                pointcv.Mass = combatVehicles.Mass;
                pointcv.Tankage = combatVehicles.Tankage;
                pointcv.OverallSize = combatVehicles.OverallSize;
                pointcv.FuelType = combatVehicles.FuelType;
                pointcv.DrivingModel = combatVehicles.DrivingModel;
                pointcv.Mileage = combatVehicles.Mileage;
                pointcv.Output = combatVehicles.Output;
                pointcv.LicenseCarry = combatVehicles.LicenseCarry;
                pointcv.VehicleChargers = combatVehicles.VehicleChargers;
                pointcv.VehicleSpotNo = combatVehicles.VehicleSpotNo;
                pointcv.VehicleDescri = combatVehicles.VehicleDescri;
                pointcv.CombineOe = combatVehicles.CombineOe;
                pointcv.Equipment = combatVehicles.Equipment;

                #endregion 车辆更新

                #region 车辆图片更新

                List<string> devdnoList = vehiclesImgList.Select(ed => ed.Name).ToList();
                var apointed = from ed in _vehiclesImageEntities.VehiclesImage
                               where ed.SerialNo == combatVehicles.SerialNo
                               select ed;
                List<string> sevdnoList = apointed.Select(ed => ed.Name).ToList();
                if (vehiclesImgList.Any())
                {
                    foreach (var data in vehiclesImgList)
                    {
                        if (!sevdnoList.Contains(data.Name))
                        {
                            //todo：增加
                            _vehiclesImageEntities.VehiclesImage.Add(data);
                        }
                    }
                }
                if (apointed.Any())
                {
                    foreach (var data in apointed)
                    {
                        if (!devdnoList.Contains(data.Name))
                        {
                            _vehiclesImageEntities.VehiclesImage.Remove(data);
                        }
                    }
                }

                #endregion 车辆图片更新

                if (combatVehicles.CombineOe)
                {
                    #region 油机更新

                    var pointoe = (from oe in _equipEntities.OilEngine
                                   where oe.OeNo == oilEngine.OeNo
                                   select oe).First();
                    pointoe.OeModel = oilEngine.OeModel;
                    pointoe.OutPower = oilEngine.OutPower;
                    pointoe.TechCondition = oilEngine.TechCondition;
                    pointoe.WorkHour = oilEngine.WorkHour;
                    pointoe.OeFactory = oilEngine.OeFactory;
                    pointoe.OeDate = oilEngine.OeDate;
                    pointoe.OeOemNo = oilEngine.OeOemNo;
                    pointoe.MotorModel = oilEngine.MotorModel;
                    pointoe.MotorPower = oilEngine.MotorPower;
                    pointoe.MotorFuel = oilEngine.MotorFuel;
                    pointoe.MotorTankage = oilEngine.MotorTankage;
                    pointoe.MotorFactory = oilEngine.MotorFactory;
                    pointoe.MotorDate = oilEngine.MotorDate;
                    pointoe.MotorOemNo = oilEngine.MotorOemNo;
                    pointoe.FaultDescri = oilEngine.FaultDescri;
                    pointoe.Vehicle = oilEngine.Vehicle;

                    #endregion 油机更新

                    #region 油机图片更新

                    List<string> doilnoList = oilImgList.Select(ed => ed.Name).ToList();
                    var apointoi = from ed in _oilImageEntities.OilEngineImage
                                   where ed.SerialNo == combatVehicles.SerialNo
                                   select ed;
                    List<string> soilnoList = apointoi.Select(ed => ed.Name).ToList();
                    if (oilImgList.Any())
                    {
                        foreach (var data in oilImgList)
                        {
                            if (!soilnoList.Contains(data.Name))
                            {
                                //todo：增加
                                _oilImageEntities.OilEngineImage.Add(data);
                            }
                        }
                    }
                    if (apointoi.Any())
                    {
                        foreach (var data in apointoi)
                        {
                            if (!doilnoList.Contains(data.Name))
                            {
                                _oilImageEntities.OilEngineImage.Remove(data);
                            }
                        }
                    }

                    #endregion 油机图片更新
                }
            }
        }

        private void cmsPicture_Opening(object sender, CancelEventArgs e)
        {
        }

        private void dGvCombatVehicles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvCombatVehicles.Columns[e.ColumnIndex].Name.Equals("VehcileMoreInfo"))
            {
                VehicleDetailForm vehicleDetailForm = new VehicleDetailForm()
                {
                    Enableedit = _enableedit,
                    Index = e.RowIndex,
                    Add = false,
                    Id = dgvCombatVehicles.Rows[e.RowIndex].Cells["SerialNo"].Value.ToString()
                };
                vehicleDetailForm.SaveVehicleSucess += AddVehicleSucess;
                vehicleDetailForm.Show();
            }
        }

        private void dgvEvents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvEvents.Columns[e.ColumnIndex].Name.Equals("EventMoreInfo"))
            {
                AddEventsForm eventDetailForm = new AddEventsForm()
                {
                    Enableedit = _enableedit,
                    Index = e.RowIndex,
                    Add = false,
                    Id = dgvEvents.Rows[e.RowIndex].Cells["No"].Value.ToString()
                };
                eventDetailForm.SaveEventsSucess += AddEventsSucess;
                eventDetailForm.Show();
            }
        }

        private void dgvMaterial_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvMaterial.Columns[e.ColumnIndex].Name.Equals("MaterialMoreInfo"))
            {
                AddMaterialForm materialDetailForm = new AddMaterialForm()
                {
                    Enableedit = _enableedit,
                    Index = e.RowIndex,
                    Add = false,
                    Id = dgvMaterial.Rows[e.RowIndex].Cells["No"].Value.ToString()
                };
                materialDetailForm.SaveMaterialSucess += AddMaterialSucess;
                materialDetailForm.Show();
            }
            if (e.ColumnIndex >= 0 && dgvMaterial.Columns[e.ColumnIndex].Name.Equals("DocumentLink"))
            {
                string path = dgvMaterial[e.ColumnIndex, e.RowIndex].Value.ToString();
                FileInfo fInfo = new FileInfo(path);
                string dir = fInfo.DirectoryName;
                if (dir == null) return;
                ProcessStartInfo psi = new ProcessStartInfo("Explorer.exe")
                {
                    Arguments = dir
                };
                Process.Start(psi);
            }
        }

        private void EquipmentDetailForm_Load(object sender, EventArgs e)
        {
            tsbRestore.Visible = !Add;
            tsDetail.Enabled = gbBaseInfo.Enabled = 更新图片ToolStripMenuItem.Enabled = _enableedit;
        }

        private void EquipmentDetailForm_Shown(object sender, EventArgs e)
        {
            Thread initThread = new Thread((ThreadStart)delegate
           {
               try
               {
                   var equip = from eq in _equipEntities.CombatEquipment
                               select eq;

                   #region 下拉列表内容

                   List<string> equipNameList =
                       (from s in equip where !string.IsNullOrEmpty(s.Name) select s.Name).Distinct().ToList();
                   List<string> equipSubdepartList =
                        (from s in equip where !string.IsNullOrEmpty(s.SubDepartment) select s.SubDepartment).Distinct()
                            .ToList();
                   List<string> equipModelList =
                        (from s in equip where !string.IsNullOrEmpty(s.Model) select s.Model).Distinct().ToList();
                   List<string> equipTechcanList =
                        (from s in equip where !string.IsNullOrEmpty(s.Technician) select s.Technician).Distinct().ToList();
                   List<string> equipManagerList =
                        (from s in equip where !string.IsNullOrEmpty(s.Manager) select s.Manager).Distinct().ToList();
                   List<string> equipTechconList =
                        (from s in equip where !string.IsNullOrEmpty(s.TechCondition) select s.TechCondition).Distinct()
                            .ToList();
                   List<string> equipUseconList =
                        (from s in equip where !string.IsNullOrEmpty(s.UseCondition) select s.UseCondition).Distinct().ToList();
                   List<string> equipMajcatList =
                        (from s in equip where !string.IsNullOrEmpty(s.MajorCategory) select s.MajorCategory).Distinct()
                            .ToList();
                   List<string> equipFactList =
                        (from s in equip where !string.IsNullOrEmpty(s.Factory) select s.Factory).Distinct().ToList();

                   _synchContext.Post(a =>
                   {
                       cmbName.DataSource = equipNameList;
                       cmbSubDepart.DataSource = equipSubdepartList;
                       cmbModel.DataSource = equipModelList;
                       cmbTechnician.DataSource = equipTechcanList;
                       cmbCharger.DataSource = equipManagerList;
                       cmbTechCondition.DataSource = equipTechconList;
                       cmbUseCondition.DataSource = equipUseconList;
                       cmbMajorCategory.DataSource = equipMajcatList;
                       cmbFactory.DataSource = equipFactList;
                       cmbName.SelectedIndex = -1;
                       cmbSubDepart.SelectedIndex = -1;
                       cmbModel.SelectedIndex = -1;
                       cmbTechnician.SelectedIndex = -1;
                       cmbCharger.SelectedIndex = -1;
                       cmbTechCondition.SelectedIndex = -1;
                       cmbUseCondition.SelectedIndex = -1;
                       cmbMajorCategory.SelectedIndex = -1;
                       cmbFactory.SelectedIndex = -1;

                       tsbRestore.Enabled = !Add;
                   }, null);

                   #endregion 下拉列表内容

                   if (!Add)
                   {
                       LoadEquipData(equip);
                       CommonLogHelper.GetInstance("LogInfo").Info($"加载设备数据{_id}成功");
                   }
               }
               catch (Exception exception)
               {
                   _synchContext.Post(a =>
                    {
                        if (Add)
                        {
                            CommonLogHelper.GetInstance("LogError").Error(@"打开添加设备数据失败", exception);
                            MessageBox.Show(this, @"打开添加设备数据失败" + exception.Message, @"错误", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                        else
                        {
                            CommonLogHelper.GetInstance("LogError").Error($"加载设备数据{_id}失败", exception);
                            MessageBox.Show(this, $"加载设备数据{_id}失败" + exception.Message, @"错误", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    }, null);
               }
           })
            { IsBackground = true };
            initThread.Start();
        }

        private void LoadEquipData(IQueryable<CombatEquipment> equip)
        {
            var appointeq = from eq in equip
                            where eq.SerialNo == _id
                            select eq;
            var equipfirst = appointeq.First();
            _comVehList = (from v in _equipEntities.CombatVehicles
                           where v.Equipment == _id
                           select v).ToList();
            _eventsList = (from ev in _equipEntities.Events
                           where ev.Equipment == _id
                           select ev).ToList();
            _materList = (from m in _equipEntities.Material
                          where m.Equipment == _id
                          select m).ToList();
            _equipImageList = (from img in _equipImageEntities.EquipmentImage
                               where img.SerialNo == _id
                               select img).ToList();
            Dictionary<string, Image> imgdic = new Dictionary<string, Image>();
            foreach (var equipmentImage in _equipImageList)
            {
                using (MemoryStream ms = new MemoryStream(equipmentImage.Images))
                {
                    Image img = Image.FromStream(ms);
                    imgdic.Add(equipmentImage.Name, img);
                }
            }
            _synchContext.Post(a =>
            {
                cmbName.SelectedItem = equipfirst.Name;
                cmbModel.SelectedItem = equipfirst.Model;
                cmbSubDepart.SelectedItem = equipfirst.SubDepartment;
                tbSerialNo.Text = equipfirst.SerialNo;
                tbTechRemould.Text = equipfirst.TechRemould;
                tbOemNo.Text = equipfirst.OemNo;
                cmbTechnician.SelectedItem = equipfirst.Technician;
                cmbCharger.SelectedItem = equipfirst.Manager;
                cmbTechCondition.SelectedItem = equipfirst.TechCondition;
                cmbUseCondition.SelectedItem = equipfirst.UseCondition;
                cmbMajorCategory.SelectedItem = equipfirst.MajorCategory;
                cmbFactory.SelectedItem = equipfirst.Factory;
                dtpTime.Value = equipfirst.ProductionDate;
                tbMajorComp.Text = equipfirst.MajorComp;
                tbMainUsage.Text = equipfirst.MainUsage;
                tbUseMethod.Text = equipfirst.UseMethod;
                tbPerformIndex.Text = equipfirst.PerformIndex;

                dgvCombatVehicles.DataSource = _comVehList;
                for (int i = 0; i < dgvCombatVehicles.RowCount; i++)
                {
                    dgvCombatVehicles[0, i].Value = i + 1;
                    dgvCombatVehicles[9, i].Value = "详细信息";
                }
                dgvEvents.DataSource = _eventsList;
                for (int i = 0; i < dgvEvents.RowCount; i++)
                {
                    dgvEvents[0, i].Value = i + 1;
                    dgvEvents[7, i].Value = "详细信息";
                }
                dgvMaterial.DataSource = _materList;
                for (int i = 0; i < dgvMaterial.RowCount; i++)
                {
                    dgvMaterial[0, i].Value = i + 1;
                    dgvMaterial[8, i].Value = "详细信息";
                }
                ilvEquipment.ImgDictionary = imgdic;
                ilvEquipment.ShowImages();
            }, null);
        }

        private void tsbAddEvents_Click(object sender, EventArgs e)
        {
            AddEventsForm eFrom = new AddEventsForm()
            {
                Add = true,
                Index = -1,
                Enabled = _enableedit,
                Id = tbSerialNo.Text
            };
            eFrom.SaveEventsSucess += AddEventsSucess;
            eFrom.ShowDialog();
        }

        private void tsbAddImages_Click(object sender, EventArgs e)
        {
            if (ofdImage.ShowDialog() == DialogResult.OK)
            {
                string imgpath = ofdImage.FileName;
                //byte[] imgBytes = PublicFunction.ReturnImgBytes(imgpath);
                FileStream fs = new FileStream(imgpath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                byte[] imgBytes = br.ReadBytes((int)fs.Length);
                fs.Close();
                if (imgBytes != null)
                {
                    EquipmentImage eqImg = new EquipmentImage
                    {
                        Name = imgpath,
                        Images = imgBytes,
                        SerialNo = tbSerialNo.Text
                    };
                    _equipImageList.Add(eqImg);

                    using (MemoryStream ms = new MemoryStream(imgBytes))
                    {
                        Image img = Image.FromStream(ms);
                        ilvEquipment.AddImages(eqImg.Name, img);
                    }
                }
            }
        }

        private void tsbAddMaterial_Click(object sender, EventArgs e)
        {
            AddMaterialForm addMaterialForm = new AddMaterialForm()
            {
                Add = true,
                Index = -1,
                Enabled = _enableedit,
                Id = tbSerialNo.Text
            };
            addMaterialForm.SaveMaterialSucess += AddMaterialSucess;
            addMaterialForm.ShowDialog();
        }

        private void tsbAddVehicle_Click(object sender, EventArgs e)
        {
            VehicleDetailForm vdForm = new VehicleDetailForm()
            {
                Add = true,
                Index = -1,
                Enabled = _enableedit,
                Id = tbSerialNo.Text
            };
            vdForm.SaveVehicleSucess += AddVehicleSucess;
            vdForm.ShowDialog();
        }

        private void tsbDeleteEvents_Click(object sender, EventArgs e)
        {
            if (dgvEvents.CurrentRow == null) return;
            if (MessageBox.Show(@"确定要删除该事件及相关数据？删除后将无法找回！", @"警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question) !=
                DialogResult.Yes) return;
            try
            {
                int selectRowIndex = dgvEvents.CurrentRow.Index;
                string id = dgvEvents.Rows[selectRowIndex].Cells["No"].Value.ToString();
                //dgvEvents.Rows.RemoveAt(selectRowIndex);
                if (Add)
                {
                    foreach (var es in _eventsList.Where(ee => ee.No == id))
                    {
                        _eventsList.Remove(es);
                        break;
                    }
                    foreach (var ed in _eventDataList.Where(ee => ee.EventsNo == id))
                    {
                        _eventDataList.Remove(ed);
                        return;
                    }
                    foreach (var ei in _eventsImgList.Where(ei => ei.SerialNo == id))
                    {
                        _eventsImgList.Remove(ei);
                        return;
                    }
                }
                else
                {
                    var ee = (from eqee in _equipEntities.Events
                              where eqee.No == id
                              select eqee).First();
                    _equipEntities.Events.Remove(ee);
                    var ed = (from eqed in _equipEntities.EventData
                              where eqed.EventsNo == id
                              select eqed).First();
                    _equipEntities.EventData.Remove(ed);
                    var ei = (from eqei in _eventsImageEntities.EventsImage
                              where eqei.SerialNo == id
                              select eqei).First();
                    _eventsImageEntities.EventsImage.Remove(ei);
                }
                dgvEvents.DataSource = _eventsList;
                CommonLogHelper.GetInstance("LogInfo").Info($"删除事件{id}成功");
                MessageBox.Show(this, @"删除事件成功", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                CommonLogHelper.GetInstance("LogError").Error(@"删除事件失败", exception);
                MessageBox.Show(this, @"删除事件失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsbDeleteImage_Click(object sender, EventArgs e)
        {
            ilvEquipment.DeleteImages();
            if (!string.IsNullOrEmpty(ilvEquipment.DeleteImgKey))
            {
                try
                {
                    string key = ilvEquipment.DeleteImgKey;
                    foreach (var equipmentImage in _equipImageList.Where(equipmentImage => equipmentImage.Name == key))
                    {
                        _equipImageList.Remove(equipmentImage);
                    }
                    var eqimg = from img in _equipImageEntities.EquipmentImage
                                where img.Name == key
                                select img;
                    if (eqimg.Any())
                    {
                        _equipImageEntities.EquipmentImage.Remove(eqimg.First());
                    }
                    CommonLogHelper.GetInstance("LogInfo").Info($"删除图片{key}成功");
                    MessageBox.Show(this, $"删除图片成功", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception exception)
                {
                    CommonLogHelper.GetInstance("LogError").Error(@"删除图片失败", exception);
                    MessageBox.Show(this, @"删除图片失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tsbDeleteMaterial_Click(object sender, EventArgs e)
        {
            if (dgvMaterial.CurrentRow == null) return;
            if (MessageBox.Show(@"确定要删除该材料？删除后将无法找回！", @"警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question) !=
                DialogResult.Yes) return;
            try
            {
                int selectRowIndex = dgvMaterial.CurrentRow.Index;
                string id = dgvMaterial.Rows[selectRowIndex].Cells["No"].Value.ToString();
                //dgvMaterial.Rows.RemoveAt(selectRowIndex);
                if (Add)
                {
                    foreach (var mm in _materList.Where(mm => mm.No == id))
                    {
                        _materList.Remove(mm);
                        break;
                    }
                }
                else
                {
                    var mm = from eqm in _equipEntities.Material
                             where eqm.No == id
                             select eqm;
                    if (mm.Any())
                    {
                        _equipEntities.Material.Remove(mm.First());
                    }
                }
                dgvMaterial.DataSource = _materList;
                CommonLogHelper.GetInstance("LogInfo").Info($"删除材料{id}成功");
                MessageBox.Show(this, @"删除材料成功", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                CommonLogHelper.GetInstance("LogError").Error(@"删除材料失败", exception);
                MessageBox.Show(this, @"删除材料失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsbDeleteVehicle_Click(object sender, EventArgs e)
        {
            if (dgvCombatVehicles.CurrentRow == null) return;
            if (MessageBox.Show(@"确定要删除该车辆及相关数据？删除后将无法找回！", @"警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question) !=
                DialogResult.Yes) return;
            try
            {
                int selectRowIndex = dgvCombatVehicles.CurrentRow.Index;
                string id = dgvCombatVehicles.Rows[selectRowIndex].Cells["SerialNo"].Value.ToString();
                if (Add)
                {
                    bool isContainsOe = false;
                    foreach (var es in _comVehList.Where(ee => ee.SerialNo == id))
                    {
                        isContainsOe = es.CombineOe;
                        _comVehList.Remove(es);
                        break;
                    }
                    foreach (var ei in _vehImgList.Where(ei => ei.SerialNo == id))
                    {
                        _vehImgList.Remove(ei);
                        return;
                    }
                    if (isContainsOe)
                    {
                        _oilEngines = null;
                        foreach (var ed in _oilImgList.Where(ee => ee.SerialNo == id))
                        {
                            _oilImgList.Remove(ed);
                            return;
                        }
                    }
                }
                else
                {
                    var ee = (from eqee in _equipEntities.CombatVehicles
                              where eqee.SerialNo == id
                              select eqee).First();
                    _equipEntities.CombatVehicles.Remove(ee);
                    var ei = (from eqei in _vehiclesImageEntities.VehiclesImage
                              where eqei.SerialNo == id
                              select eqei).First();
                    _vehiclesImageEntities.VehiclesImage.Remove(ei);
                    if (ee.CombineOe)
                    {
                        var oe = (from eqed in _equipEntities.OilEngine
                                  where eqed.Vehicle == id
                                  select eqed).First();
                        _equipEntities.OilEngine.Remove(oe);
                        var ed = (from eqed in _oilImageEntities.OilEngineImage
                                  where eqed.SerialNo == id
                                  select eqed).First();
                        _oilImageEntities.OilEngineImage.Remove(ed);
                    }
                }
                //dgvCombatVehicles.Rows.RemoveAt(selectRowIndex);
                dgvCombatVehicles.DataSource = _comVehList;
                CommonLogHelper.GetInstance("LogInfo").Info($"删除车辆{id}成功");
                MessageBox.Show(this, @"删除车辆成功", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                CommonLogHelper.GetInstance("LogError").Error(@"删除车辆失败", exception);
                MessageBox.Show(this, @"删除车辆失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsbRestore_Click(object sender, EventArgs e)
        {
            _synchContext.Post(a =>
            {
                try
                {
                    _equipEntities = new EquipmentManagementEntities();
                    var equip = from eq in _equipEntities.CombatEquipment
                                select eq;
                    LoadEquipData(equip);
                    CommonLogHelper.GetInstance("LogInfo").Info(@"恢复原始设备数据成功");
                    MessageBox.Show(this, @"恢复原始设备数据成功", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception exception)
                {
                    CommonLogHelper.GetInstance("LogError").Error(@"恢复原始设备数据失败", exception);
                    MessageBox.Show(this, @"恢复原始设备数据失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }, null);
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            _synchContext.Post(a =>
            {
                try
                {
                    _equipImageEntities.EquipmentImage.AddRange(_equipImageList);
                    if (Add)
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
                            ProductionDate = dtpTime.Value.Date,
                            OemNo = tbOemNo.Text,
                            MajorComp = tbMajorComp.Text,
                            MainUsage = tbMainUsage.Text,
                            PerformIndex = tbPerformIndex.Text,
                            UseMethod = tbUseMethod.Text,
                            Manager = cmbCharger.Text,
                            Technician = cmbTechnician.Text,
                            TechRemould = tbTechRemould.Text,
                            SetupVideo = tbSetupVideo.Text
                        };
                        _equipEntities.CombatEquipment.Add(ce);
                        _equipEntities.SaveChanges();
                        _equipEntities.Events.AddRange(_eventsList);
                        _equipEntities.SaveChanges();
                        _equipEntities.CombatVehicles.AddRange(_comVehList);
                        _equipEntities.SaveChanges();
                        if (_oilEngines != null)
                        {
                            _equipEntities.OilEngine.Add(_oilEngines);
                            _oilImageEntities.OilEngineImage.AddRange(_oilImgList);
                        }
                        _equipEntities.Material.AddRange(_materList);
                        _equipEntities.EventData.AddRange(_eventDataList);

                        _vehiclesImageEntities.VehiclesImage.AddRange(_vehImgList);
                        _eventsImageEntities.EventsImage.AddRange(_eventsImgList);
                        _equipEntities.SaveChanges();
                    }
                    else
                    {
                        var equipfirst = (from eq in _equipEntities.CombatEquipment
                                          where eq.SerialNo == _id
                                          select eq).First();

                        equipfirst.OemNo = tbOemNo.Text;
                        equipfirst.Technician = cmbTechnician.Text;
                        equipfirst.SubDepartment = cmbSubDepart.Text;
                        equipfirst.Manager = cmbCharger.Text;
                        equipfirst.TechCondition = cmbTechCondition.Text;
                        equipfirst.UseCondition = cmbUseCondition.Text;
                        equipfirst.MajorCategory = cmbMajorCategory.Text;
                        equipfirst.Factory = cmbFactory.Text;
                        equipfirst.OemNo = tbOemNo.Text;
                        equipfirst.ProductionDate = dtpTime.Value.Date;
                        equipfirst.Model = cmbModel.Text;
                        equipfirst.Name = cmbName.Text;
                        equipfirst.SubDepartment = cmbSubDepart.Text;
                        equipfirst.Model = cmbModel.Text;
                        equipfirst.SerialNo = tbSerialNo.Text;
                        equipfirst.TechRemould = tbTechRemould.Text;
                        equipfirst.MajorComp = tbMajorComp.Text;
                        equipfirst.MainUsage = tbMainUsage.Text;
                        equipfirst.UseMethod = tbUseMethod.Text;
                        equipfirst.PerformIndex = tbPerformIndex.Text;
                        _equipEntities.SaveChanges();
                    }
                    SaveSuccess?.Invoke();
                    CommonLogHelper.GetInstance("LogInfo").Info(@"保存设备数据成功");
                    MessageBox.Show(this, @"保存设备数据成功", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                catch (Exception exception)
                {
                    CommonLogHelper.GetInstance("LogError").Error(@"保存设备数据失败", exception);
                    MessageBox.Show(this, @"保存设备数据失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }, null);
        }
    }
}