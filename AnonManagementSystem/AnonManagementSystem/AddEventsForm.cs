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
    public partial class AddEventsForm : Form
    {
        private bool _add = false;
        private bool _enableedit = false;
        private string _id;
        public delegate void SaveEvents(Events events,List<EventData> eventDataList,List<EventsImage> eventImgList);
        public event SaveEvents SaveEventsSucess;

        public string Id
        {
            set { _id = value; }
        }

        public AddEventsForm()
        {
            InitializeComponent();
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            EquipmentManagementEntities eqEntities = new EquipmentManagementEntities();
            if (_add)
            {
                Events eve = new Events()
                {
                    No = tbSerialNo.Text,
                    Name = cmbEventName.Text,
                    Address = cmbAddress.Text,
                    StartTime = dtpEventDt1.Value.Date,
                    EndTime = dtpEventDt2.Value.Date,
                    EventType = cmbEventType.Text,
                    SpecificType = cmbSpecificType.Text,
                    Code = tbCode.Text,

                    PublishUnit = cmbPulishUnit.Text,
                    PublishDate = dtpPulishDt.Value.Date,
                    Publisher = cmbPulisher.Text,
                    According = tbAccording.Text,
                    PeopleDescri = tbDesci.Text,
                    ProcessDescri = tbProcessDescri.Text,
                    HandleStep = tbHandleStep.Text,
                    Problem = tbProblems.Text,
                    Remarks = tbRemark.Text,
                    Equipment = _id
                };
                eqEntities.Events.Add(eve);
            }
            else
            {
                var eventfirst = (from eq in eqEntities.Events
                                  where eq.Equipment == _id
                                  select eq).First();

                eventfirst.No = tbSerialNo.Text;
                eventfirst.Name = cmbEventName.Text;
                eventfirst.Address = cmbAddress.Text;
                eventfirst.StartTime = dtpEventDt1.Value.Date;
                eventfirst.EndTime = dtpEventDt2.Value.Date;
                eventfirst.EventType = cmbEventType.Text;
                eventfirst.SpecificType = cmbSpecificType.Text;
                eventfirst.Code = tbCode.Text;
                eventfirst.PublishUnit = cmbPulishUnit.Text;
                eventfirst.PublishDate = dtpPulishDt.Value.Date;
                eventfirst.Publisher = cmbPulisher.Text;
                eventfirst.According = tbAccording.Text;
                eventfirst.PeopleDescri = tbDesci.Text;
                eventfirst.ProcessDescri = tbProcessDescri.Text;
                eventfirst.HandleStep = tbHandleStep.Text;
                eventfirst.Problem = tbProblems.Text;
                eventfirst.Remarks = tbRemark.Text;
            }
            eqEntities.SaveChanges();
        }

        private void tsbAddEventsData_Click(object sender, EventArgs e)
        {

        }

        private void tsbDeleteEventsData_Click(object sender, EventArgs e)
        {

        }

        private void tsbAddImg_Click(object sender, EventArgs e)
        {

        }

        private void tsbDeleteImg_Click(object sender, EventArgs e)
        {

        }
    }
}
