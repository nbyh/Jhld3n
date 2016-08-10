using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EquipmentInformationData;

namespace AnonManagementSystem
{
    public partial class EquipmentDetailForm : Form, IAddModify
    {
        private bool _add = false;
        private List<CombatVehicles> _comVehList = new List<CombatVehicles>();
        private bool _enableedit = false;
        private EquipmentManagementEntities _equipEntities = new EquipmentManagementEntities();
        private EquipImageEntities _equipImageEntities = new EquipImageEntities();
        private List<EquipmentImage> _equipImageList = new List<EquipmentImage>();
        private List<EventData> _eventDataList = new List<EventData>();
        private EventsImagesEntities _eventsImageEntities = new EventsImagesEntities();
        private List<EventsImage> _eventsImgList = new List<EventsImage>();
        private List<Events> _eventsList = new List<Events>();
        private string _id;
        private List<Material> _materList = new List<Material>();
        private OilEngine _oilEngines;
        private OilEngineImagesEntities _oilImageEntities = new OilEngineImagesEntities();
        private List<OilEngineImage> _oilImgList = new List<OilEngineImage>();
        private VehiclesImagesEntities _vehiclesImageEntities = new VehiclesImagesEntities();
        private List<VehiclesImage> _vehImgList = new List<VehiclesImage>();

        public EquipmentDetailForm()
        {
            InitializeComponent();
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

        private void AddEventsSucess(bool add, int index, Events events, List<EventData> eventdatalist, List<EventsImage> eventimglist)
        {
            if (add)
            {
                _eventsList.Add(events);
                dgvEvents.DataSource = _eventsList;
                _eventDataList.AddRange(eventdatalist);
                _eventsImgList.AddRange(eventimglist);
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

                #endregion

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

                #endregion

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

                #endregion

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

                #endregion
            }
        }

        private void AddMaterialSucess(bool add, int index, Material material)
        {
            if (add)
            {
                _materList.Add(material);
                dgvMaterial.DataSource = _materList;
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

                #endregion

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

                #endregion

            }
        }

        private void AddVehicleSucess(bool add, int index, CombatVehicles combatVehicles, List<VehiclesImage> vehiclesImgList, OilEngine oilEngine, List<OilEngineImage> oilImgList)
        {
            if (add)
            {
                #region 增加

                _comVehList.Add(combatVehicles);
                dgvCombatVehicles.DataSource = _comVehList;
                _vehImgList.AddRange(vehiclesImgList);
                _oilEngines = oilEngine;
                _oilImgList.AddRange(oilImgList);

                #endregion
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

                #endregion

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

                #endregion

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
                pointoe.MotorNo = oilEngine.MotorNo;
                pointoe.MotorPower = oilEngine.MotorPower;
                pointoe.MotorFuel = oilEngine.MotorFuel;
                pointoe.MotorTankage = oilEngine.MotorTankage;
                pointoe.MotorFactory = oilEngine.MotorFactory;
                pointoe.MotorDate = oilEngine.MotorDate;
                pointoe.MotorOemNo = oilEngine.MotorOemNo;
                pointoe.FaultDescri = oilEngine.FaultDescri;
                pointoe.Vehicle = oilEngine.Vehicle;

                #endregion

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

                #endregion

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

                #endregion
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
                ProcessStartInfo psi = new ProcessStartInfo("Explorer.exe")
                {
                    Arguments = dir
                };
                Process.Start(psi);
            }
        }

        private void EquipmentDetailForm_Load(object sender, EventArgs e)
        {
            tsbRestore.Visible = !_add;
        }

        private void EquipmentDetailForm_Shown(object sender, EventArgs e)
        {
            EquipmentManagementEntities equipEntities = new EquipmentManagementEntities();
            var equip = from eq in equipEntities.CombatEquipment
                        select eq;

            #region 下拉列表内容

            List<string> equipNameList =
                (from s in equip where !string.IsNullOrEmpty(s.Name) select s.Name).Distinct().ToList();
            cmbName.DataSource = equipNameList;
            List<string> equipSubdepartList =
                (from s in equip where !string.IsNullOrEmpty(s.SubDepartment) select s.SubDepartment).Distinct()
                    .ToList();
            cmbSubDepart.DataSource = equipSubdepartList;
            List<string> equipModelList =
                (from s in equip where !string.IsNullOrEmpty(s.Model) select s.Model).Distinct().ToList();
            cmbModel.DataSource = equipModelList;
            List<string> equipTechcanList =
                (from s in equip where !string.IsNullOrEmpty(s.Technician) select s.Technician).Distinct().ToList();
            cmbTechnician.DataSource = equipTechcanList;
            List<string> equipManagerList =
                (from s in equip where !string.IsNullOrEmpty(s.Manager) select s.Manager).Distinct().ToList();
            cmbCharger.DataSource = equipManagerList;
            List<string> equipTechconList =
                (from s in equip where !string.IsNullOrEmpty(s.TechCondition) select s.TechCondition).Distinct()
                    .ToList();
            cmbTechCondition.DataSource = equipTechconList;
            List<string> equipUseconList =
                (from s in equip where !string.IsNullOrEmpty(s.UseCondition) select s.UseCondition).Distinct().ToList();
            cmbUseCondition.DataSource = equipUseconList;
            List<string> equipMajcatList =
                (from s in equip where !string.IsNullOrEmpty(s.MajorCategory) select s.MajorCategory).Distinct()
                    .ToList();
            cmbMajorCategory.DataSource = equipMajcatList;
            List<string> equipFactList =
                (from s in equip where !string.IsNullOrEmpty(s.Factory) select s.Factory).Distinct().ToList();
            cmbFactory.DataSource = equipFactList;

            #endregion


            if (_add)
            {
                cmbName.SelectedIndex = -1;
                cmbSubDepart.SelectedIndex = -1;
                cmbModel.SelectedIndex = -1;
                cmbTechnician.SelectedIndex = -1;
                cmbCharger.SelectedIndex = -1;
                cmbTechCondition.SelectedIndex = -1;
                cmbUseCondition.SelectedIndex = -1;
                cmbMajorCategory.SelectedIndex = -1;
                cmbFactory.SelectedIndex = -1;
            }
            else
            {
                var appointeq = from eq in equip
                                where eq.SerialNo == _id
                                select eq;
                var equipfirst = appointeq.First();
                cmbName.SelectedItem = equipfirst.Name;
                cmbModel.SelectedItem = equipfirst.Model;
                cmbSubDepart.SelectedItem = equipfirst.SubDepartment;
                tbSerialNo.Text = equipfirst.SerialNo;
                tbTechRemould.Text = equipfirst.TechRemould;
                tbOemNo.Text = equipfirst.InventorySpot;
                cmbTechnician.SelectedItem = equipfirst.Technician;
                cmbCharger.SelectedItem = equipfirst.Manager;
                cmbTechCondition.SelectedItem = equipfirst.TechCondition;
                cmbUseCondition.SelectedItem = equipfirst.UseCondition;
                cmbMajorCategory.SelectedItem = equipfirst.MajorCategory;
                cmbFactory.SelectedItem = equipfirst.Factory;
                dtpTime.Value = equipfirst.FactoryTime.Value;

                tbMajorComp.Text = equipfirst.MajorComp;
                tbMainUsage.Text = equipfirst.MainUsage;
                tbUseMethod.Text = equipfirst.UseMethod;
                tbPerformIndex.Text = equipfirst.PerformIndex;

                var vechiledata = (from v in equipEntities.CombatVehicles
                                   select v).ToList();
                dgvCombatVehicles.DataSource = vechiledata;

                var events = (from ev in equipEntities.Events select ev).ToList();
                dgvEvents.DataSource = events;

                var material = (from m in equipEntities.Material select m).ToList();
                dgvMaterial.DataSource = material;

            }
            tsDetail.Enabled = gbBaseInfo.Enabled = 更新图片ToolStripMenuItem.Enabled = _enableedit;

        }

        //private void EventsDataRefresh(int pagesize, int curpage, DbRawSqlQuery<Events> iquery)
        //{
        //    int all = iquery.Count();
        //    _eventsLastPage = (int)Math.Ceiling((double)all / _eventsPageSize);
        //    var eventsParts = QueryByPage(pagesize, curpage, iquery);
        //    dgvEvents.DataSource = eventsParts.ToList();
        //    for (int i = 0; i < dgvEvents.RowCount; i++)
        //    {
        //        dgvEvents[0, i].Value = i + 1;
        //        dgvEvents.Rows[i].Cells["EventMoreInfo"].Value = "详细信息";
        //    }
        //}

        //private void MaterialDataRefresh(int pagesize, int curpage, DbRawSqlQuery<Material> iquery)
        //{
        //    int all = iquery.Count();
        //    _materialLastPage = (int)Math.Ceiling((double)all / _materialPageSize);
        //    var materialParts = QueryByPage(pagesize, curpage, iquery);
        //    dgvMaterial.DataSource = materialParts.ToList();
        //    for (int i = 0; i < dgvMaterial.RowCount; i++)
        //    {
        //        dgvMaterial[0, i].Value = i + 1;
        //        dgvMaterial.Rows[i].Cells["MaterialMoreInfo"].Value = "详细信息";
        //    }
        //}

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
                //todo：界面增加
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
            //todo:界面修改

        }

        private void tsbDeleteImage_Click(object sender, EventArgs e)
        {
            //todo:界面修改

        }

        private void tsbDeleteMaterial_Click(object sender, EventArgs e)
        {
            //todo:界面修改

        }

        private void tsbDeleteVehicle_Click(object sender, EventArgs e)
        {
            //todo:界面修改

        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (_add)
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
                _equipEntities.CombatEquipment.Add(ce);
                _equipEntities.CombatVehicles.AddRange(_comVehList);
                if (_oilEngines != null)
                {
                    _equipEntities.OilEngine.Add(_oilEngines);
                }
                _equipEntities.Material.AddRange(_materList);
                _equipEntities.Events.AddRange(_eventsList);
                _equipEntities.EventData.AddRange(_eventDataList);

                _vehiclesImageEntities.VehiclesImage.AddRange(_vehImgList);
                _oilImageEntities.OilEngineImage.AddRange(_oilImgList);
                _eventsImageEntities.EventsImage.AddRange(_eventsImgList);
            }
            else
            {
                var equipfirst = (from eq in _equipEntities.CombatEquipment
                                  where eq.SerialNo == _id
                                  select eq).First();

                equipfirst.InventorySpot = tbOemNo.Text;
                equipfirst.Technician = cmbTechnician.Text;
                equipfirst.SubDepartment = cmbSubDepart.Text;
                equipfirst.Manager = cmbCharger.Text;
                equipfirst.TechCondition = cmbTechCondition.Text;
                equipfirst.UseCondition = cmbUseCondition.Text;
                equipfirst.MajorCategory = cmbMajorCategory.Text;
                equipfirst.Factory = cmbFactory.Text;
                equipfirst.FactoryTime = dtpTime.Value.Date;
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
            }
            _equipEntities.SaveChanges();
        }


        //private void VehicleDataRefresh(int pagesize, int curpage, DbRawSqlQuery<CombatVehicles> iquery)
        //{
        //    int all = iquery.Count();
        //    _vehicleLastPage = (int)Math.Ceiling((double)all / _vehiclePageSize);
        //    var vehcileParts = QueryByPage(pagesize, curpage, iquery);
        //    dGvCombatVehicles.DataSource = vehcileParts.ToList();
        //    for (int i = 0; i < dGvCombatVehicles.RowCount; i++)
        //    {
        //        dGvCombatVehicles[0, i].Value = i + 1;
        //        dGvCombatVehicles.Rows[i].Cells["EventMoreInfo"].Value = "详细信息";
        //    }
        //}
    }
}
