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
    public partial class AddEventsForm : Form, IAddModify
    {
        private bool _add = false;
        private bool _enableedit = false;
        private string _id;
        public delegate void SaveEvents(bool add, int index, Events events, List<EventData> eventDataList, List<EventsImage> eventImgList);
        public event SaveEvents SaveEventsSucess;

        public bool Enableedit { get; set; }
        public int Index { get; set; }

        public string Id
        {
            set { _id = value; }
        }

        public bool Add { get; set; }

        public AddEventsForm()
        {
            InitializeComponent();
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            EquipmentManagementEntities eqEntities = new EquipmentManagementEntities();
            List<EventData> eventdataList=new List<EventData>();
            for (int i = 0; i < dgvEvents.RowCount; i++)
            {
                EventData ed=new EventData()
                {
                    ID = dgvEvents[1,i].Value.ToString(),
                    Name = dgvEvents[2, i].Value.ToString(),
                    Spot = dgvEvents[3, i].Value.ToString(),
                    EventsNo = tbSerialNo.Text
                };
                eventdataList.Add(ed);
            }
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
                for (int i = 0; i < dgvEvents.RowCount; i++)
                {
                    EventData ed = new EventData()
                    {
                        ID = dgvEvents[1, i].Value.ToString(),
                        Name = dgvEvents[2, i].Value.ToString(),
                        Spot = dgvEvents[3, i].Value.ToString(),
                        EventsNo = tbSerialNo.Text
                    };
                    eventdataList.Add(ed);
                }

                SaveEventsSucess(Add, Index, eve, eventdataList, null);
            }
            else
            {
                var eventfirst = (from eq in eqEntities.Events
                                  where eq.Equipment == _id
                                  select eq).First();

                for (int i = 0; i < dgvEvents.RowCount; i++)
                {
                    string no = dgvEvents[1, i].Value.ToString();
                    var eventsdata = (from ed in eqEntities.EventData
                                      where ed.EventsNo == no
                                      select ed).First();
                    eventsdata.Name = dgvEvents[2, i].Value.ToString();
                    eventsdata.Spot = dgvEvents[3, i].Value.ToString();
                    eventdataList.Add(eventsdata);
                }

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
                SaveEventsSucess(Add, Index, eventfirst, eventdataList, null);

            }
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
