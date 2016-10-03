﻿using EquipmentInformationData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using LinqToDB;
using LinqToDB.DataProvider.SQLite;

namespace AnonManagementSystem
{
    public partial class EquipmentDetailForm : Form, IAddModify
    {
        private readonly EquipmentImagesDB _equipImageDb = new EquipmentImagesDB(new SQLiteDataProvider(), DbPublicFunction.ReturnDbConnectionString(@"\ZBDataBase\EquipmentImages.db"));
        private readonly EventsImagesDB _eventsImageDb = new EventsImagesDB(new SQLiteDataProvider(), DbPublicFunction.ReturnDbConnectionString(@"\ZBDataBase\EventsImages.db"));
        private readonly OilEngineImagesDB _oilImageDb = new OilEngineImagesDB(new SQLiteDataProvider(), DbPublicFunction.ReturnDbConnectionString(@"\ZBDataBase\OilEngineImages.db"));
        private readonly SynchronizationContext _synchContext;
        private readonly VehiclesImagesDB _vehiclesImageDb = new VehiclesImagesDB(new SQLiteDataProvider(), DbPublicFunction.ReturnDbConnectionString(@"\ZBDataBase\VehiclesImages.db"));
        private List<CombatVehicle> _comVehList = new List<CombatVehicle>();
        private bool _enableedit;
        private EquipmentManagementDB _equipDb = new EquipmentManagementDB(new SQLiteDataProvider(), DbPublicFunction.ReturnDbConnectionString(@"\ZBDataBase\EquipmentManagement.db"));
        private List<EquipmentImage> _equipImageList = new List<EquipmentImage>();
        private BindingList<Event> _eveBindingList = new BindingList<Event>();
        private List<EventData> _eventDataList = new List<EventData>();
        private List<EventsImage> _eventsImgList = new List<EventsImage>();
        private List<Event> _eventsList = new List<Event>();
        private string _id;
        private BindingList<Material> _materBindingList = new BindingList<Material>();
        private List<Material> _materList = new List<Material>();
        private OilEngine _oilEngines;
        private List<OilEngineImage> _oilImgList = new List<OilEngineImage>();
        private List<VehiclesImage> _vehImgList = new List<VehiclesImage>();
        private BindingList<CombatVehicle> _vhBindingList = new BindingList<CombatVehicle>();

        public EquipmentDetailForm()
        {
            InitializeComponent();
            _synchContext = SynchronizationContext.Current;
        }

        public delegate void SaveChangeSuccess();

        public event SaveChangeSuccess SaveSuccess;

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

        private void AddModifyEventsSucess(bool add, int index, Event events, List<EventData> eventdatalist,
            List<EventsImage> eventimglist)
        {
            if (add)
            {
                _eventsList.Add(events);
                _eveBindingList = new BindingList<Event>(_eventsList);
                dgvEvents.DataSource = _eveBindingList;
                _eventDataList.AddRange(eventdatalist);
                _eventsImgList.AddRange(eventimglist);
                DgvSetup();
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

                if (Add)
                {
                    #region 活动事件临时数据更新

                    var pointevent = _eventsList.First(ev => ev.No == events.No);
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

                    _eventDataList = eventdatalist;
                    _eventsImgList = eventimglist;

                    #endregion 活动事件更新
                }
                else
                {
                    #region 活动事件更新

                    var pointevent = _equipDb.Events.First(ev => ev.No == events.No);
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

                    #region 事件数据更新

                    _equipDb.InsertOrReplace(eventdatalist);
                    _eventDataList = eventdatalist;

                    #endregion 事件数据更新

                    #region 活动事件图片更新

                    List<string> deinoList = eventimglist.Select(ed => ed.Name).ToList();
                    var apointei = _eventsImageDb.EventsImages.Where(ed => ed.SerialNo == events.No);
                    List<string> seinoList = apointei.Select(ed => ed.Name).ToList();
                    if (eventimglist.Any())
                    {
                        foreach (var data in eventimglist)
                        {
                            if (!seinoList.Contains(data.Name))
                            {
                                _eventsImageDb.InsertOrReplace(data);
                            }
                        }
                    }
                    if (apointei.Any())
                    {
                        foreach (var data in apointei)
                        {
                            if (!deinoList.Contains(data.Name))
                            {
                                _eventsImageDb.EventsImages.Where(d => d.Name == data.Name).Delete();
                            }
                        }
                    }

                    #endregion 活动事件图片更新
                }
            }
        }

        private void AddModifyMaterialSucess(bool add, int index, Material material)
        {
            if (add)
            {
                _materList.Add(material);
                _materBindingList = new BindingList<Material>(_materList);
                dgvMaterial.DataSource = _materBindingList;
                DgvSetup();
            }
            else
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

                if (Add)
                {
                    #region 材料临时数据更新

                    var pointmaterial = _materList.First(m => m.No == material.No);
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
                else
                {
                    #region 材料数据更新

                    var pointmaterial = _equipDb.Materials.First(ma => ma.No == material.No);
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
        }

        private void AddModifyVehicleSucess(bool add, int index, CombatVehicle combatVehicles,
            List<VehiclesImage> vehiclesImgList, OilEngine oilEngine, List<OilEngineImage> oilImgList)
        {
            if (add)
            {
                #region 增加

                _comVehList.Add(combatVehicles);
                _vhBindingList = new BindingList<CombatVehicle>(_comVehList);
                dgvCombatVehicles.DataSource = _vhBindingList;
                _vehImgList.AddRange(vehiclesImgList);
                if (combatVehicles.CombineOe)
                {
                    _oilEngines = oilEngine;
                    _oilImgList.AddRange(oilImgList);
                }
                DgvSetup();

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

                if (Add)
                {
                    #region 车辆信息更新

                    var vh = _comVehList.First(v => v.SerialNo == combatVehicles.SerialNo);
                    vh.Name = combatVehicles.Name;
                    vh.Model = combatVehicles.Model;
                    vh.VehiclesNo = combatVehicles.VehiclesNo;
                    vh.MotorModel = combatVehicles.MotorModel;
                    vh.TechCondition = combatVehicles.TechCondition;
                    vh.Factory = combatVehicles.Factory;
                    vh.ProductionDate = combatVehicles.ProductionDate;
                    vh.Mass = combatVehicles.Mass;
                    vh.Tankage = combatVehicles.Tankage;
                    vh.OverallSize = combatVehicles.OverallSize;
                    vh.FuelType = combatVehicles.FuelType;
                    vh.DrivingModel = combatVehicles.DrivingModel;
                    vh.Mileage = combatVehicles.Mileage;
                    vh.Output = combatVehicles.Output;
                    vh.LicenseCarry = combatVehicles.LicenseCarry;
                    vh.VehicleChargers = combatVehicles.VehicleChargers;
                    vh.VehicleSpotNo = combatVehicles.VehicleSpotNo;
                    vh.VehicleDescri = combatVehicles.VehicleDescri;
                    vh.CombineOe = combatVehicles.CombineOe;
                    vh.Equipment = combatVehicles.Equipment;
                    _vehImgList = vehiclesImgList;
                    _oilEngines = oilEngine;
                    _oilImgList = oilImgList;

                    #endregion
                }
                else
                {
                    #region 车辆更新

                    var pointcv = (from cv in _equipDb.CombatVehicles
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
                    var apointed = from ed in _vehiclesImageDb.VehiclesImages
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
                                _vehiclesImageDb.InsertOrReplace(data);
                            }
                        }
                    }
                    if (apointed.Any())
                    {
                        foreach (var data in apointed)
                        {
                            if (!devdnoList.Contains(data.Name))
                            {
                                _vehiclesImageDb.VehiclesImages.Where(d => d.Name == data.Name).Delete();
                            }
                        }
                    }

                    #endregion 车辆图片更新

                    if (combatVehicles.CombineOe)
                    {
                        #region 油机更新

                        var pointoe = (from oe in _equipDb.OilEngines
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
                        var apointoi = from ed in _oilImageDb.OilEngineImages
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
                                    _oilImageDb.InsertOrReplace(data);
                                }
                            }
                        }
                        if (apointoi.Any())
                        {
                            foreach (var data in apointoi)
                            {
                                if (!doilnoList.Contains(data.Name))
                                {
                                    _oilImageDb.OilEngineImages.Where(d => d.Name == data.Name).Delete();
                                }
                            }
                        }

                        #endregion 油机图片更新
                    }

                }
            }
        }

        private void dgvCombatVehicles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvCombatVehicles.Columns[e.ColumnIndex].Name.Equals("VehicleMoreInfo"))
            {
                string id = dgvCombatVehicles.Rows[e.RowIndex].Cells["SerialNo"].Value.ToString();
                OilEngine oe = null;
                List<OilEngineImage> oeImgList = new List<OilEngineImage>();
                CombatVehicle vh = Add ? _comVehList.First(mm => mm.SerialNo == id) : _equipDb.CombatVehicles.First(mm => mm.SerialNo == id);
                if (vh.CombineOe)
                {
                    oe = Add ? _oilEngines : _equipDb.OilEngines.First(mm => mm.Vehicle == id);
                    oeImgList = Add ? _oilImgList.Where(mm => mm.SerialNo == id).ToList() : _oilImageDb.OilEngineImages.Where(mm => mm.SerialNo == id).ToList();
                }
                List<VehiclesImage> vilist = Add ? _vehImgList.Where(mm => mm.SerialNo == id).ToList() : _vehiclesImageDb.VehiclesImages.Where(mm => mm.SerialNo == id).ToList();
                VehicleDetailForm vehicleDetailForm = new VehicleDetailForm()
                {
                    Enableedit = _enableedit,
                    Index = e.RowIndex,
                    Add = false,
                    Id = id,
                    Comvh = vh,
                    VehiclesImagesList = vilist,
                    Oe = oe,
                    OilImagesList = oeImgList
                };
                vehicleDetailForm.SaveVehicleSucess += AddModifyVehicleSucess;
                vehicleDetailForm.Show();
            }
        }

        private void dgvEvents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvEvents.Columns[e.ColumnIndex].Name.Equals("EventMoreInfo"))
            {
                string id = dgvEvents.Rows[e.RowIndex].Cells["No"].Value.ToString();
                Event ee = Add ? _eventsList.First(mm => mm.No == id) : _equipDb.Events.First(mm => mm.No == id);
                List<EventData> edlist = Add ? _eventDataList.Where(mm => mm.EventsNo == id).ToList() : _equipDb.EventData.Where(mm => mm.EventsNo == id).ToList();
                List<EventsImage> eilist = Add ? _eventsImgList.Where(mm => mm.SerialNo == id).ToList() : _eventsImageDb.EventsImages.Where(mm => mm.SerialNo == id).ToList();
                AddEventsForm eventDetailForm = new AddEventsForm()
                {
                    Enableedit = _enableedit,
                    Index = e.RowIndex,
                    Add = false,
                    Id = id,
                    Eqevents = ee,
                    EventdataList = edlist,
                    EventsImgList = eilist
                };
                eventDetailForm.SaveEventsSucess += AddModifyEventsSucess;
                eventDetailForm.Show();
            }
        }

        private void dgvMaterial_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvMaterial.Columns[e.ColumnIndex].Name.Equals("MaterialMoreInfo"))
            {
                string id = dgvMaterial.Rows[e.RowIndex].Cells["No"].Value.ToString();
                Material m = Add ? _materList.First(mm => mm.No == id) : _equipDb.Materials.First(mm => mm.No == id);
                AddMaterialForm materialDetailForm = new AddMaterialForm()
                {
                    Enableedit = _enableedit,
                    Index = e.RowIndex,
                    Add = false,
                    Id = id,
                    Material1 = m
                };
                materialDetailForm.SaveMaterialSucess += AddModifyMaterialSucess;
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

        private void DgvSetup()
        {
            for (int i = 0; i < dgvCombatVehicles.RowCount; i++)
            {
                dgvCombatVehicles[0, i].Value = i + 1;
                dgvCombatVehicles.Rows[i].Cells["VehcileMoreInfo"].Value = "详细信息";
            }
            for (int i = 0; i < dgvEvents.RowCount; i++)
            {
                dgvEvents[0, i].Value = i + 1;
                dgvEvents[7, i].Value = "详细信息";
            }
            for (int i = 0; i < dgvMaterial.RowCount; i++)
            {
                dgvMaterial[0, i].Value = i + 1;
                dgvMaterial[8, i].Value = "详细信息";
            }
        }
        private void EquipmentDetailForm_Load(object sender, EventArgs e)
        {
            tsbRestore.Visible = !Add;
            tsDetail.Enabled = gbBaseInfo.Enabled = _enableedit;
        }

        private void EquipmentDetailForm_Shown(object sender, EventArgs e)
        {
            Thread initThread = new Thread((ThreadStart)delegate
           {
               try
               {
                   var equip = from eq in _equipDb.CombatEquipments
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
            _comVehList = (from v in _equipDb.CombatVehicles
                           where v.Equipment == _id
                           select v).ToList();
            _eventsList = (from ev in _equipDb.Events
                           where ev.Equipment == _id
                           select ev).ToList();
            _materList = (from m in _equipDb.Materials
                          where m.Equipment == _id
                          select m).ToList();
            _equipImageList = (from img in _equipImageDb.EquipmentImages
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
            _vhBindingList = new BindingList<CombatVehicle>(_comVehList);
            _eveBindingList = new BindingList<Event>(_eventsList);
            _materBindingList = new BindingList<Material>(_materList);
            _synchContext.Send(a =>
            {
                cmbName.SelectedText = equipfirst.Name;
                cmbModel.SelectedText = equipfirst.Model;
                cmbSubDepart.SelectedText = equipfirst.SubDepartment;
                tbSerialNo.Text = equipfirst.SerialNo;
                tbTechRemould.Text = equipfirst.TechRemould;
                tbOemNo.Text = equipfirst.OemNo;
                cmbTechnician.SelectedText = equipfirst.Technician;
                cmbCharger.SelectedText = equipfirst.Manager;
                cmbTechCondition.SelectedText = equipfirst.TechCondition;
                cmbUseCondition.SelectedText = equipfirst.UseCondition;
                cmbMajorCategory.SelectedText = equipfirst.MajorCategory;
                cmbFactory.SelectedText = equipfirst.Factory;
                dtpTime.Value = equipfirst.ProductionDate;
                tbMajorComp.Text = equipfirst.MajorComp;
                tbMainUsage.Text = equipfirst.MainUsage;
                tbUseMethod.Text = equipfirst.UseMethod;
                tbPerformIndex.Text = equipfirst.PerformIndex;

                dgvCombatVehicles.DataSource = _vhBindingList;
                dgvEvents.DataSource = _eveBindingList;
                dgvMaterial.DataSource = _materBindingList;
                DgvSetup();

                ilvEquipment.ImgDictionary = imgdic;
                ilvEquipment.ShowImages();
            }, null);
        }

        private void SerialNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = MainPublicFunction.JudgeKeyPress(e.KeyChar);
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
            eFrom.SaveEventsSucess += AddModifyEventsSucess;
            eFrom.ShowDialog();
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
            addMaterialForm.SaveMaterialSucess += AddModifyMaterialSucess;
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
            vdForm.SaveVehicleSucess += AddModifyVehicleSucess;
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

                foreach (var es in _eventsList.Where(eve => eve.No == id))
                {
                    _eventsList.Remove(es);
                    break;
                }
                foreach (var eed in _eventDataList.Where(eve => eve.EventsNo == id))
                {
                    _eventDataList.Remove(eed);
                    return;
                }
                foreach (var eei in _eventsImgList.Where(eve => eve.SerialNo == id))
                {
                    _eventsImgList.Remove(eei);
                    return;
                }
                _equipDb.EventData.Where(eqed => eqed.EventsNo == id).Delete();
                _equipDb.Events.Where(eqee => eqee.No == id).Delete();
                _eventsImageDb.EventsImages.Where(eqei => eqei.SerialNo == id).Delete();
                _eveBindingList = new BindingList<Event>(_eventsList);
                dgvEvents.DataSource = _eveBindingList;
                CommonLogHelper.GetInstance("LogInfo").Info($"删除事件{id}成功");
                DgvSetup();
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
                        break;
                    }
                    _equipImageDb.EquipmentImages.Where(img => img.Name == key).Delete();
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

                foreach (var mm in _materList.Where(mm => mm.No == id))
                {
                    _materList.Remove(mm);
                    break;
                }
                _equipDb.Materials.Where(eqm => eqm.No == id).Delete();
                _materBindingList = new BindingList<Material>(_materList);
                dgvMaterial.DataSource = _materBindingList;
                DgvSetup();
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

                bool isContainsOe = false;
                foreach (var es in _comVehList.Where(vh => vh.SerialNo == id))
                {
                    isContainsOe = es.CombineOe;
                    _comVehList.Remove(es);
                    break;
                }
                foreach (var vhi in _vehImgList.Where(vh => vh.SerialNo == id))
                {
                    _vehImgList.Remove(vhi);
                    return;
                }
                if (isContainsOe)
                {
                    _oilEngines = null;
                    foreach (var ed in _oilImgList.Where(vh => vh.SerialNo == id))
                    {
                        _oilImgList.Remove(ed);
                        return;
                    }
                }
                var ee = _equipDb.CombatVehicles.Where(eqee => eqee.SerialNo == id);
                if (ee.Any())
                {
                    if (ee.First().CombineOe)
                    {
                        _equipDb.OilEngines.Where(eqed => eqed.Vehicle == id).Delete();
                        _oilImageDb.OilEngineImages.Where(eqed => eqed.SerialNo == id).Delete();
                    }
                    ee.Delete();
                    _vehiclesImageDb.VehiclesImages.Where(eqei => eqei.SerialNo == id).Delete();
                }
                //dgvCombatVehicles.Rows.RemoveAt(selectRowIndex);
                _vhBindingList = new BindingList<CombatVehicle>(_comVehList);
                dgvCombatVehicles.DataSource = _vhBindingList;
                DgvSetup();
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
                    _equipDb = new EquipmentManagementDB(new SQLiteDataProvider(), DbPublicFunction.ReturnDbConnectionString(@"\ZBDataBase\EquipmentManagement.db"));
                    var equip = from eq in _equipDb.CombatEquipments
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
                    _equipImageDb.InsertOrReplace(_equipImageList);
                    if (Add)
                    {
                        #region 添加设备及相关信息

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
                        _equipDb.InsertOrReplace(ce);
                        //_equipDB.SaveChanges();
                        _equipDb.InsertOrReplace(_eventsList);
                        //_equipDB.SaveChanges();
                        _equipDb.InsertOrReplace(_comVehList);
                        //_equipDB.SaveChanges();
                        if (_oilEngines != null)
                        {
                            _equipDb.InsertOrReplace(_oilEngines);
                            _oilImageDb.InsertOrReplace(_oilImgList);
                            //_oilImageDB.SaveChanges();
                        }
                        _equipDb.InsertOrReplace(_materList);
                        _equipDb.InsertOrReplace(_eventDataList);

                        _vehiclesImageDb.InsertOrReplace(_vehImgList);
                        _eventsImageDb.InsertOrReplace(_eventsImgList);
                        //_equipDB.SaveChanges();

                        #endregion
                    }
                    else
                    {
                        #region 修改设备信息

                        var equipfirst = (from eq in _equipDb.CombatEquipments
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

                        #endregion

                        //_equipDB.SaveChanges();

                        _equipDb.InsertOrReplace(_eventsList);
                        //_equipDB.SaveChanges();
                        _equipDb.InsertOrReplace(_comVehList);
                        //_equipDB.SaveChanges();
                        if (_oilEngines != null)
                        {
                            _equipDb.InsertOrReplace(_oilEngines);
                            _oilImageDb.InsertOrReplace(_oilImgList);
                            //_oilImageDB.SaveChanges();
                        }
                        _equipDb.InsertOrReplace(_materList);
                        _equipDb.InsertOrReplace(_eventDataList);

                        _vehiclesImageDb.InsertOrReplace(_vehImgList);
                        _eventsImageDb.InsertOrReplace(_eventsImgList);
                        //_equipDB.SaveChanges();

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

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                tbSetupVideo.Text = ofd.FileName;
            }
        }

        private void btnDeleteSetuoVideo_Click(object sender, EventArgs e)
        {
            tbSetupVideo.Clear();
        }
    }
}