using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EquipmentInformationData;

namespace AnonManagementSystem
{
    public partial class EquipmentDetailForm : Form
    {
        private bool _add = false;
        private bool _enableedit = false;
        private string _id;
        private int _pageSize = 20, _curPage = 1, _lastPage = 1;
        private DbRawSqlQuery<CombatVehicles> _vehicleses;
        private List<CombatVehicles> comVehList = new List<CombatVehicles>();
        private List<EquipmentImage> equipImageList = new List<EquipmentImage>();
        private List<Events> eventsList = new List<Events>();
        private List<Material> materList = new List<Material>();

        public EquipmentDetailForm()
        {
            InitializeComponent();
        }

        public bool Add
        {
            set { _add = value; }
        }

        public bool Enableedit
        {
            set { _enableedit = value; }
        }

        public string Id
        {
            set { _id = value; }
        }

        private void cmsPicture_Opening(object sender, CancelEventArgs e)
        {
        }

        private void DataRefresh(int pagesize, int curpage, DbRawSqlQuery<CombatVehicles> iquery)
        {
            int all = iquery.Count();
            _lastPage = (int)Math.Ceiling((double)all / _pageSize);
            var vehcilepage = QueryByPage(pagesize, curpage, iquery);
            dGvCombatVehicles.DataSource = vehcilepage.ToList();
            for (int i = 0; i < dGvCombatVehicles.RowCount; i++)
            {
                dGvCombatVehicles[0, i].Value = i + 1;
                dGvCombatVehicles.Rows[i].Cells["MoreInfo"].Value = "详细信息";
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
            List<string> equipNameList = (from s in equip where !string.IsNullOrEmpty(s.Name) select s.Name).Distinct().ToList();
            cmbName.DataSource = equipNameList;
            List<string> equipSubdepartList = (from s in equip where !string.IsNullOrEmpty(s.SubDepartment) select s.SubDepartment).Distinct().ToList();
            cmbSubDepart.DataSource = equipSubdepartList;
            List<string> equipModelList = (from s in equip where !string.IsNullOrEmpty(s.Model) select s.Model).Distinct().ToList();
            cmbModel.DataSource = equipModelList;
            List<string> equipTechcanList = (from s in equip where !string.IsNullOrEmpty(s.Technician) select s.Technician).Distinct().ToList();
            cmbTechnician.DataSource = equipTechcanList;
            List<string> equipManagerList = (from s in equip where !string.IsNullOrEmpty(s.Manager) select s.Manager).Distinct().ToList();
            cmbCharger.DataSource = equipManagerList;
            List<string> equipTechconList = (from s in equip where !string.IsNullOrEmpty(s.TechCondition) select s.TechCondition).Distinct().ToList();
            cmbTechCondition.DataSource = equipTechconList;
            List<string> equipUseconList = (from s in equip where !string.IsNullOrEmpty(s.UseCondition) select s.UseCondition).Distinct().ToList();
            cmbUseCondition.DataSource = equipUseconList;
            List<string> equipMajcatList = (from s in equip where !string.IsNullOrEmpty(s.MajorCategory) select s.MajorCategory).Distinct().ToList();
            cmbMajorCategory.DataSource = equipMajcatList;
            List<string> equipFactList = (from s in equip where !string.IsNullOrEmpty(s.Factory) select s.Factory).Distinct().ToList();
            cmbFactory.DataSource = equipFactList;


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

            }
            tsDetail.Enabled = gbBaseInfo.Enabled = 更新图片ToolStripMenuItem.Enabled = _enableedit;

            string cmds = $"select *from CombatVehicles where Equipment='{_id}' ";
            _vehicleses = equipEntities.Database.SqlQuery<CombatVehicles>(cmds);

            _pageSize = 20;
            _curPage = 1;
            DataRefresh(_pageSize, _curPage, _vehicleses);
        }

        private IList<CombatVehicles> QueryByPage(int pageSize, int curPage, DbRawSqlQuery<CombatVehicles> dbRaw)
        {
            return dbRaw.OrderBy(s => s.SerialNo).Take(pageSize * curPage).Skip(pageSize * (curPage - 1)).ToList();
        }

        private void AddVehicleSucess(CombatVehicles combatVehicles)
        {
            comVehList.Add(combatVehicles);
            //界面增加
        }

        private void tsbAddEvents_Click(object sender, EventArgs e)
        {
            AddEventsForm eFrom = new AddEventsForm()
            {
                Id = tbSerialNo.Text
            };
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
                equipImageList.Add(eqImg);
                //界面增加
            }
        }

        private void tsbAddMaterial_Click(object sender, EventArgs e)
        {
            AddMaterialForm addMaterialForm = new AddMaterialForm()
            {
                Id = tbSerialNo.Text
            };
            addMaterialForm.ShowDialog();
        }

        private void tsbAddVehicle_Click(object sender, EventArgs e)
        {
            VehicleDetailForm vdForm = new VehicleDetailForm()
            {
                Id = tbSerialNo.Text
            };
            vdForm.SaveVehicleSucess += AddVehicleSucess;
            vdForm.ShowDialog();
        }

        private void tsbDeleteEvents_Click(object sender, EventArgs e)
        {

        }

        private void tsbDeleteImage_Click(object sender, EventArgs e)
        {

        }

        private void tsbDeleteMaterial_Click(object sender, EventArgs e)
        {

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
                ;
                equipfirst.MajorComp = tbMajorComp.Text;
                equipfirst.MainUsage = tbMainUsage.Text;
                equipfirst.UseMethod = tbUseMethod.Text;
                equipfirst.PerformIndex = tbPerformIndex.Text;
            }
            eqEntities.SaveChanges();
        }
    }
}
