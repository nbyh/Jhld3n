using EquipmentInformationData;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
        private bool _enableedit = false;
        private Events _events;
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

        public List<EventData> EventdataList
        {
            get { return _eventdataList; }
            set { _eventdataList = value; }
        }

        public List<EventsImage> EventsImgList
        {
            get { return _eventsImgList; }
            set { _eventsImgList = value; }
        }

        public Events Eqevents
        {
            get { return _events; }
            set { _events = value; }
        }

        private void AddEventsForm_Load(object sender, EventArgs e)
        {
        }

        private void AddEventsForm_Shown(object sender, EventArgs e)
        {
            try
            {
                if (!Add)
                {
                    Dictionary<string, Image> imgdic = new Dictionary<string, Image>();
                    foreach (var equipmentImage in _eventsImgList)
                    {
                        using (MemoryStream ms = new MemoryStream(equipmentImage.Images))
                        {
                            Image img = Image.FromStream(ms);
                            imgdic.Add(equipmentImage.Name, img);
                        }
                    }
                    _synchContext.Post(a =>
                    {
                        cmbEventNo.Text = _events.No;
                        cmbEventName.Text = _events.Name;
                        cmbAddress.Text = _events.Address;
                        dtpEventDt1.Value = _events.StartTime;
                        dtpEventDt2.Value = _events.EndTime;
                        cmbEventType.Text = _events.EventType;
                        cmbSpecificType.Text = _events.SpecificType;
                        tbCode.Text = _events.Code;
                        cmbHigherUnit.Text = _events.HigherUnit;
                        cmbExecutor.Text = _events.Executor;
                        tbNoInEvents.Text = _events.NoInEvents;
                        cmbPulishUnit.Text = _events.PublishUnit;
                        dtpPulishDt.Value = _events.PublishDate;
                        cmbPulisher.Text = _events.Publisher;
                        tbAccording.Text = _events.According;
                        tbDesci.Text = _events.PeopleDescri;
                        tbProcessDescri.Text = _events.ProcessDescri;
                        tbHandleStep.Text = _events.HandleStep;
                        tbProblems.Text = _events.Problem;
                        tbRemark.Text = _events.Remarks;

                        dgvEvents.DataSource = _eventdataList;

                        ilvEvents.ImgDictionary = imgdic;
                        ilvEvents.ShowImages();

                    }, null);
                    CommonLogHelper.GetInstance("LogInfo").Info($"加载事件数据{_id}成功");
                }
            }
            catch (Exception exception)
            {
                if (Add)
                {
                    CommonLogHelper.GetInstance("LogError").Error(@"打开添加事件数据失败", exception);
                    MessageBox.Show(this, @"打开添加事件数据失败" + exception.Message, @"错误", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                else
                {
                    CommonLogHelper.GetInstance("LogError").Error($"加载事件数据{_id}失败", exception);
                    MessageBox.Show(this, $"加载事件数据{_id}失败" + exception.Message, @"错误", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
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
                if (PublicFunction.CheckImgCondition(imgpath))
                {
                    FileStream fs = new FileStream(imgpath, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    byte[] imgBytes = br.ReadBytes((int)fs.Length);
                    fs.Close();
                    EventsImage cvImag = new EventsImage
                    {
                        Images = imgBytes,
                        Name = imgpath,
                        SerialNo = cmbEventNo.Text
                    };
                    _eventsImgList.Add(cvImag);
                    using (MemoryStream ms = new MemoryStream(imgBytes))
                    {
                        Image img = Image.FromStream(ms);
                        ilvEvents.AddImages(cvImag.Name, img);
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
                    if (dgvEvents[2, rowindex].Value != null)
                    {
                        string id = dgvEvents[2, rowindex].Value.ToString();
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
                _eventdataList.Clear();
                for (int i = 0; i < dgvEvents.RowCount; i++)
                {
                    if (dgvEvents[2, i].Value != null)
                    {
                        EventData ed = new EventData()
                        {
                            //ID = dgvEvents[1, i].Value.ToString(),
                            Name = dgvEvents[2, i].Value.ToString(),
                            //Spot = dgvEvents[3, i].Value.ToString(),
                            EventsNo = cmbEventNo.Text
                        };
                        _eventdataList.Add(ed);
                    }
                }
                if (Add)
                {
                    _events = new Events()
                    {
                        No = cmbEventNo.Text,
                        Name = cmbEventName.Text,
                        Address = cmbAddress.Text,
                        StartTime = dtpEventDt1.Value.Date,
                        EndTime = dtpEventDt2.Value.Date,
                        EventType = cmbEventType.Text,
                        SpecificType = cmbSpecificType.Text,
                        Code = tbCode.Text,

                        HigherUnit = cmbHigherUnit.Text,
                        Executor = cmbExecutor.Text,
                        NoInEvents = tbNoInEvents.Text,
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

                    SaveEventsSucess?.Invoke(Add, Index, _events, _eventdataList, _eventsImgList);
                }
                else
                {
                    var eventfirst = _eqEntities.Events.First(eq => eq.Equipment == _id);
                    _eventdataList.Clear();

                    eventfirst.No = cmbEventNo.Text;
                    eventfirst.Name = cmbEventName.Text;
                    eventfirst.Address = cmbAddress.Text;
                    eventfirst.StartTime = dtpEventDt1.Value.Date;
                    eventfirst.EndTime = dtpEventDt2.Value.Date;
                    eventfirst.EventType = cmbEventType.Text;
                    eventfirst.SpecificType = cmbSpecificType.Text;
                    eventfirst.Code = tbCode.Text;
                    eventfirst.HigherUnit = cmbHigherUnit.Text;
                    eventfirst.Executor = cmbExecutor.Text;
                    eventfirst.NoInEvents = tbNoInEvents.Text;
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
                Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, @"活动事件保存失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CommonLogHelper.GetInstance("LogError").Error(@"活动事件保存失败", exception);
            }
        }

        private void Num_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = PublicFunction.JudgeKeyPress(e.KeyChar);
        }

        private void Combox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }
            e.DrawBackground();
            e.DrawFocusRectangle();
            e.Graphics.DrawString(((ComboBox)sender).Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds.X, e.Bounds.Y + 3);
        }

        private void cmbEventType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string s = cmbEventType.SelectedText;
            switch (s)
            {
                case "调整类":
                    cmbSpecificType.DataSource = new List<string>()
                    {
                        "交接活动","接装培训","封存启用","退役报废"
                    };
                    break;
                case "动用类":
                    cmbSpecificType.DataSource = new List<string>()
                    {
                        "任务","试验改造","借用","大中修"
                    };
                    break;
                case "使用维护":
                    cmbSpecificType.DataSource = new List<string>()
                    {
                        "维护","点检","性能测试","车辆记录"
                    };
                    break;
                case "测试检修":
                    cmbSpecificType.DataSource = new List<string>()
                    {
                        "故障确认","故障处理","厂所修理"
                    };
                    break;
            }
        }
    }
}