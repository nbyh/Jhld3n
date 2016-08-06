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
        private bool _enableedit = false;
        private string _id;
        private DbRawSqlQuery<Events> _events;
        private DbRawSqlQuery<Material> _materials;
        private DbRawSqlQuery<CombatVehicles> _vehicleses;
        private List<CombatVehicles> _comVehList = new List<CombatVehicles>();
        private List<EquipmentImage> _equipImageList = new List<EquipmentImage>();
        private Dictionary<string, List<EventData>> _eventDataDictionary = new Dictionary<string, List<EventData>>();
        private Dictionary<string, List<EventsImage>> _eventsImgDictionary = new Dictionary<string, List<EventsImage>>();
        private List<Events> _eventsList = new List<Events>();
        private List<Material> _materList = new List<Material>();
        private Dictionary<string, OilEngine> _oilEnginesDictionary = new Dictionary<string, OilEngine>();
        private Dictionary<string, List<OilEngineImage>> _oilImgDictionary = new Dictionary<string, List<OilEngineImage>>();
        private Dictionary<string, List<VehiclesImage>> _vehImgDictionary = new Dictionary<string, List<VehiclesImage>>();
        private EquipmentManagementEntities _equipEntities = new EquipmentManagementEntities();
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
            //todo:界面更新
            if (add)
            {
                _eventsList.Add(events);
                dgvEvents.DataSource = _eventsList;
                _eventDataDictionary.Add(events.No, eventdatalist);
                _eventsImgDictionary.Add(events.No, eventimglist);

            }
            else
            {
                dgvEvents.Rows[index].Cells["Name"].Value = events.Name;
                dgvEvents.Rows[index].Cells["Name"].Value = events.StartTime;
                dgvEvents.Rows[index].Cells["Name"].Value = events.Address;
                dgvEvents.Rows[index].Cells["Name"].Value = events.EndTime;
                dgvEvents.Rows[index].Cells["Name"].Value = events.EventType;
                dgvEvents.Rows[index].Cells["Name"].Value = events.SpecificType;
                dgvEvents.Rows[index].Cells["Name"].Value = events.Code;
                dgvEvents.Rows[index].Cells["Name"].Value = events.PublishUnit;
                dgvEvents.Rows[index].Cells["Name"].Value = events.PublishDate;
                dgvEvents.Rows[index].Cells["Name"].Value = events.Publisher;
                dgvEvents.Rows[index].Cells["Name"].Value = events.According;
                dgvEvents.Rows[index].Cells["Name"].Value = events.PeopleDescri;
                dgvEvents.Rows[index].Cells["Name"].Value = events.ProcessDescri;
                dgvEvents.Rows[index].Cells["Name"].Value = events.HandleStep;
                dgvEvents.Rows[index].Cells["Name"].Value = events.Problem;
                dgvEvents.Rows[index].Cells["Name"].Value = events.Remarks;
                dgvEvents.Rows[index].Cells["Name"].Value = events.Equipment;

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

                foreach (var eventData in eventdatalist)
                {
                    var apointed = from ed in _equipEntities.EventData
                        where ed.ID == eventData.ID && ed.EventsNo == events.No
                        select ed;
                    if (apointed.Any())
                    {
                        
                    }
                    else
                    {
                        _equipEntities.EventData.Remove();
                    }
                }



            }
        }

        private void AddMaterialSucess(bool add, int index, Material material)
        {
            if (add)
            {
                _materList.Add(material);
                dgvMaterial.DataSource = _materList;
            }
            else
            {

            }
            //todo:界面增加
        }

        private void AddVehicleSucess(bool add, int index, CombatVehicles combatVehicles, List<VehiclesImage> vehiclesImgList, OilEngine oilEngine, List<OilEngineImage> oilImgList)
        {
            if (add)
            {
                _comVehList.Add(combatVehicles);
                dGvCombatVehicles.DataSource = _comVehList;
                _vehImgDictionary.Add(combatVehicles.SerialNo, vehiclesImgList);
                _oilEnginesDictionary.Add(combatVehicles.SerialNo, oilEngine);
                _oilImgDictionary.Add(combatVehicles.SerialNo, oilImgList);
            }
            else
            {

            }
            //todo:界面增加
        }

        private void cmsPicture_Opening(object sender, CancelEventArgs e)
        {
        }

        private void dGvCombatVehicles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dGvCombatVehicles.Columns[e.ColumnIndex].Name.Equals("VehcileMoreInfo"))
            {
                VehicleDetailForm vehicleDetailForm = new VehicleDetailForm()
                {
                    Enableedit = _enableedit,
                    Index = e.RowIndex,
                    Add = false,
                    Id = dGvCombatVehicles.Rows[e.RowIndex].Cells["SerialNo"].Value.ToString()
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
                dGvCombatVehicles.DataSource = vechiledata;

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
            EquipmentManagementEntities eqEntities = new EquipmentManagementEntities();
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
                eqEntities.CombatEquipment.Add(ce);
            }
            else
            {
                var equipfirst = (from eq in eqEntities.CombatEquipment
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
            eqEntities.SaveChanges();
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
