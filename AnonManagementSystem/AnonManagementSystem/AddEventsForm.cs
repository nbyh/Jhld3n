using EquipmentInformationData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace AnonManagementSystem
{
    public partial class AddEventsForm : Form, IAddModify
    {
        private readonly EquipmentManagementEntities _eqEntities = new EquipmentManagementEntities();
        private readonly SynchronizationContext _synchContext;
        private bool _add = false;
        private bool _enableedit = false;
        private List<EventData> _eventdataList = new List<EventData>();
        private List<EventsImage> _eventsImgList = new List<EventsImage>();
        private string _id;

        public AddEventsForm()
        {
            InitializeComponent();
            _synchContext = SynchronizationContext.Current;
        }

        public delegate void SaveEvents(bool add, int index, Events events, List<EventData> eventDataList, List<EventsImage> eventImgList);

        public event SaveEvents SaveEventsSucess;

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

        private void AddEventsForm_Load(object sender, EventArgs e)
        {
        }

        private void AddEventsForm_Shown(object sender, EventArgs e)
        {
            _synchContext.Post(a =>
            {
                try
                {
                    if (!Add)
                    {
                        EventsImagesEntities vie = new EventsImagesEntities();
                        var vh = (from eh in _eqEntities.Events
                                  where eh.No == _id
                                  select eh).First();

                        tbSerialNo.Text = vh.No;
                        cmbEventName.Text = vh.Name;
                        cmbAddress.Text = vh.Address;
                        dtpEventDt1.Value = vh.StartTime;
                        dtpEventDt2.Value = vh.EndTime;
                        cmbEventType.Text = vh.EventType;
                        cmbSpecificType.Text = vh.SpecificType;
                        tbCode.Text = vh.Code;
                        cmbPulishUnit.Text = vh.PublishUnit;
                        dtpPulishDt.Value = vh.PublishDate;
                        cmbPulisher.Text = vh.Publisher;
                        tbAccording.Text = vh.According;
                        tbDesci.Text = vh.PeopleDescri;
                        tbProcessDescri.Text = vh.ProcessDescri;
                        tbHandleStep.Text = vh.HandleStep;
                        tbProblems.Text = vh.Problem;
                        tbRemark.Text = vh.Remarks;

                        var ed = from d in _eqEntities.EventData
                                 where d.EventsNo == _id
                                 select d;
                        _eventdataList = ed.ToList();
                        dgvEvents.DataSource = _eventdataList;

                        _eventsImgList = (from vhimg in vie.EventsImage
                                          where vhimg.SerialNo == _id
                                          select vhimg).ToList();
                        CommonLogHelper.GetInstance("LogInfo").Info($"加载事件数据{_id}成功");
                    }
                }
                catch (Exception exception)
                {
                    if (Add)
                    {
                        MessageBox.Show(this, @"打开添加事件数据失败" + exception.Message, @"错误", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        CommonLogHelper.GetInstance("LogError").Error(@"打开添加事件数据失败", exception);
                    }
                    else
                    {
                        MessageBox.Show(this, $"加载事件数据{_id}失败" + exception.Message, @"错误", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        CommonLogHelper.GetInstance("LogError").Error($"加载事件数据{_id}失败", exception);
                    }
                }
            }, null);
        }

        private void tsbAddEventsData_Click(object sender, EventArgs e)
        {
            int x = dgvEvents.RowCount;
            dgvEvents.Rows.Add(1);
            dgvEvents[0, x].Value = x + 1;
        }

        private void tsbAddImg_Click(object sender, EventArgs e)
        {
            if (ofdImage.ShowDialog() == DialogResult.OK)
            {
                string imgpath = ofdImage.FileName;
                byte[] imgBytes = PublicFunction.ReturnImgBytes(imgpath);
                if (imgBytes != null)
                {
                    EventsImage eveImg = new EventsImage
                    {
                        Name = imgpath,
                        Images = imgBytes,
                        SerialNo = tbSerialNo.Text
                    };
                    _eventsImgList.Add(eveImg);

                    using (MemoryStream ms = new MemoryStream(imgBytes))
                    {
                        Image img = Image.FromStream(ms);
                        ilvEvents.ImgDictionary.Add(eveImg.Name, img);
                        ilvEvents.AddImages(eveImg.Name, img);
                    }
                }
            }
        }

        private void tsbDeleteEventsData_Click(object sender, EventArgs e)
        {
            if (dgvEvents.CurrentRow != null)
            {
                int rowindex = dgvEvents.CurrentRow.Index;
                if (MessageBox.Show(@"确定是否删除", @"提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    if (dgvEvents[1, rowindex].Value != null)
                    {
                        string id = dgvEvents[1, rowindex].Value.ToString();
                        foreach (var ed in _eventdataList.Where(d => d.Name == id))
                        {
                            _eventdataList.Remove(ed);
                        }
                    }
                    dgvEvents.Rows.RemoveAt(rowindex);
                    MessageBox.Show(this, @"活动资料删除成功", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void tsbDeleteImg_Click(object sender, EventArgs e)
        {
            ilvEvents.DeleteImages();
            if (!string.IsNullOrEmpty(ilvEvents.DeleteImgKey))
            {
                string key = ilvEvents.DeleteImgKey;
                foreach (var ei in _eventsImgList.Where(d => d.Name == key))
                {
                    _eventsImgList.Remove(ei);
                    MessageBox.Show(this, @"图片删除成功", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < dgvEvents.RowCount; i++)
                {
                    EventData ed = new EventData()
                    {
                        ID = dgvEvents[1, i].Value.ToString(),
                        Name = dgvEvents[2, i].Value.ToString(),
                        Spot = dgvEvents[3, i].Value.ToString(),
                        EventsNo = tbSerialNo.Text
                    };
                    _eventdataList.Add(ed);
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
                        _eventdataList.Add(ed);
                    }

                    SaveEventsSucess?.Invoke(Add, Index, eve, _eventdataList, _eventsImgList);
                }
                else
                {
                    var eventfirst = (from eq in _eqEntities.Events
                                      where eq.Equipment == _id
                                      select eq).First();

                    for (int i = 0; i < dgvEvents.RowCount; i++)
                    {
                        string no = dgvEvents[1, i].Value.ToString();
                        var eventsdata = (from ed in _eqEntities.EventData
                                          where ed.EventsNo == no
                                          select ed).First();
                        eventsdata.Name = dgvEvents[2, i].Value.ToString();
                        eventsdata.Spot = dgvEvents[3, i].Value.ToString();
                        _eventdataList.Add(eventsdata);
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
                    SaveEventsSucess?.Invoke(Add, Index, eventfirst, _eventdataList, _eventsImgList);
                }
                CommonLogHelper.GetInstance("LogInfo").Info(@"活动事件保存成功");
                MessageBox.Show(this, @"活动事件保存成功", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, @"活动事件保存失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CommonLogHelper.GetInstance("LogError").Error(@"活动事件保存失败", exception);
            }
        }
    }
}