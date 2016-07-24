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
    public partial class EquipMainForm : Form, IMdiFunction
    {
        public delegate void ChangeUser();
        public event ChangeUser ChangeCurrentuser;
        private bool _exitapp = true;
        private bool _enableedit = false;
        public EquipMainForm()
        {
            InitializeComponent();
        }

        public bool Enableedit
        {
            set { _enableedit = value; }
        }

        private void dGvEquip_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dGvEquip.Columns[e.ColumnIndex].Name.Equals("MoreInfo"))
            {
                EquipmentDetailForm equipDetailForm = new EquipmentDetailForm()
                {
                    Enableedit = _enableedit,
                    Add = false,
                    Id = dGvEquip.Rows[e.RowIndex].Cells["SerialNo"].Value.ToString()
                };
                equipDetailForm.Show();
            }
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            Entities equipEntities = new Entities();
            var equip = from eq in equipEntities.CombatEquipment
                        select eq;
            dGvEquip.DataSource = equip.ToList();
            for (int i = 0; i < dGvEquip.RowCount; i++)
            {
                dGvEquip[0, i].Value = i + 1;
                dGvEquip.Rows[i].Cells["MoreInfo"].Value = "详细信息";
            }
            List<string> equipnameList = (from n in equip select n.Name).Distinct().ToList();
            cmbName.DataSource = equipnameList;
            List<string> equipsubdepartList = (from d in equip select d.SubDepartment).Distinct().ToList();
            cmbSubDepart.DataSource = equipsubdepartList;
            List<string> equipModelList = (from s in equip select s.Model).Distinct().ToList();
            cmbModel.DataSource = equipModelList;
            List<string> equipSpotList = (from s in equip select s.InventorySpot).Distinct().ToList();
            cmbSpot.DataSource = equipSpotList;
            cmbName.SelectedIndex = -1;
            cmbSubDepart.SelectedIndex = -1;
            cmbModel.SelectedIndex = -1;
            cmbSpot.SelectedIndex = -1;
            //tsbAdd.Enabled = tsbDelete.Enabled = _enableedit;
        }
        
        private void cmb_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }
            e.DrawBackground();
            e.DrawFocusRectangle();
            e.Graphics.DrawString(((ComboBox)sender).Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds.X, e.Bounds.Y + 3);
        }
        
        public void DataRefresh()
        {
            Entities equipEntities = new Entities();
            var equip = from eq in equipEntities.CombatEquipment
                        select eq;
            dGvEquip.DataSource = equip.ToList();
            for (int i = 0; i < dGvEquip.RowCount; i++)
            {
                dGvEquip[0, i].Value = i + 1;
                dGvEquip.Rows[i].Cells["MoreInfo"].Value = "详细信息";
            }
        }

        public void DataAdd()
        {
            EquipmentDetailForm equipDetailForm = new EquipmentDetailForm()
            {
                Enableedit = true,
                Add = true
            };
            equipDetailForm.Show();
        }

        public void DataDelete()
        {
            throw new NotImplementedException();
        }
    }
}
