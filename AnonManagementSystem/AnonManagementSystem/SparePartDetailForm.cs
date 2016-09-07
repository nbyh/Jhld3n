using EquipmentInformationData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace AnonManagementSystem
{
    public partial class SparePartDetailForm : Form, IAddModify
    {
        public delegate void SaveChangeSuccess();
        public event SaveChangeSuccess SaveSuccess;
        private readonly SynchronizationContext _synchContext;
        private readonly SparePartImagesEntities _partsImageEntities = new SparePartImagesEntities();
        private List<SparePartImage> _spImgList = new List<SparePartImage>();
        private bool _enableedit;
        private string _id;
        private SparePartManagementEntities _sparePartEntities = new SparePartManagementEntities();
        public SparePartDetailForm()
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

        private void cmsPicture_Opening(object sender, CancelEventArgs e)
        {
        }

        private void LoadSparePartData(IQueryable<SpareParts> sparePart)
        {
            var appointsp = from eq in sparePart
                            where eq.SerialNo == _id
                            select eq;
            var spfirst = appointsp.First();
            cmbName.SelectedItem = spfirst.Name;
            cmbModel.SelectedItem = spfirst.Model;
            cmbStoreSpot.SelectedItem = spfirst.StoreSpot;
            tbSerialNo.Text = spfirst.SerialNo;
            cmbStatus.Text = spfirst.Status;
            dtpStoreDate.Value = spfirst.StoreDate;
            cmbUseType.SelectedItem = spfirst.UseType;
            nUdAmount.Value = int.Parse(spfirst.Amount);
            cmbFactory.SelectedItem = spfirst.Factory;
            dtpOemDate.Value = spfirst.ProductionDate;


            var imgs = (from img in _partsImageEntities.SparePartImage
                        where img.SerialNo == _id
                        select img);
            _spImgList = imgs.ToList();
            Dictionary<string, Image> imgdic = new Dictionary<string, Image>();
            foreach (var spmentImage in imgs)
            {
                using (MemoryStream ms = new MemoryStream(spmentImage.Images))
                {
                    Image img = Image.FromStream(ms);
                    imgdic.Add(spmentImage.Name, img);
                }
            }
            ilvEquipment.ImgDictionary = imgdic;
            ilvEquipment.ShowImages();
        }

        private void SparePartDetailForm_Load(object sender, EventArgs e)
        {
            tsbRestore.Visible = !Add;
            tsDetail.Enabled = gbBaseInfo.Enabled = 更新图片ToolStripMenuItem.Enabled = _enableedit;
        }

        private void SparePartDetailForm_Shown(object sender, EventArgs e)
        {
            _synchContext.Post(a =>
            {
                try
                {
                    var sp = from eq in _sparePartEntities.SpareParts
                             select eq;

                    #region 下拉列表内容

                    List<string> spNameList =
                        (from s in sp where !string.IsNullOrEmpty(s.Name) select s.Name).Distinct().ToList();
                    cmbName.DataSource = spNameList;
                    List<string> spModelList =
                        (from s in sp where !string.IsNullOrEmpty(s.Model) select s.Model).Distinct().ToList();
                    cmbModel.DataSource = spModelList;
                    List<string> spFactList =
                        (from s in sp where !string.IsNullOrEmpty(s.Factory) select s.Factory).Distinct().ToList();
                    cmbFactory.DataSource = spFactList;
                    List<string> spSubdepartList =
                        (from s in sp where !string.IsNullOrEmpty(s.StoreSpot) select s.StoreSpot).Distinct()
                            .ToList();
                    cmbStoreSpot.DataSource = spSubdepartList;
                    List<string> spTechcanList =
                        (from s in sp where !string.IsNullOrEmpty(s.UseType) select s.UseType).Distinct().ToList();
                    cmbUseType.DataSource = spTechcanList;
                    List<string> spManagerList =
                        (from s in sp where !string.IsNullOrEmpty(s.Status) select s.Status).Distinct().ToList();
                    cmbStatus.DataSource = spManagerList;

                    #endregion 下拉列表内容

                    if (Add)
                    {
                        cmbName.SelectedIndex = -1;
                        cmbStoreSpot.SelectedIndex = -1;
                        cmbModel.SelectedIndex = -1;
                        cmbStatus.SelectedIndex = -1;
                        cmbUseType.SelectedIndex = -1;
                        cmbFactory.SelectedIndex = -1;
                        tsbRestore.Enabled = false;
                    }
                    else
                    {
                        LoadSparePartData(sp);
                    }
                    CommonLogHelper.GetInstance("LogInfo").Info($"加载设备数据{_id}成功");
                }
                catch (Exception exception)
                {
                    if (Add)
                    {
                        MessageBox.Show(this, @"打开添加设备数据失败" + exception.Message, @"错误", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        CommonLogHelper.GetInstance("LogError").Error(@"打开添加设备数据失败", exception);
                    }
                    else
                    {
                        CommonLogHelper.GetInstance("LogError").Error($"加载设备数据{_id}失败", exception);
                        MessageBox.Show(this, $"加载设备数据{_id}失败" + exception.Message, @"错误", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }, null);
        }
        private void tsbAddImages_Click(object sender, EventArgs e)
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
                    SparePartImage eqImg = new SparePartImage
                    {
                        Name = imgpath,
                        Images = imgBytes,
                        SerialNo = tbSerialNo.Text
                    };
                    _spImgList.Add(eqImg);
                    using (MemoryStream ms = new MemoryStream(imgBytes))
                    {
                        Image img = Image.FromStream(ms);
                        ilvEquipment.AddImages(eqImg.Name, img);
                    }
                }
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
                    foreach (var spImg in _spImgList.Where(spimg => spimg.Name == key))
                    {
                        _spImgList.Remove(spImg);
                    }
                    var eqimg = _partsImageEntities.SparePartImage.Where(img => img.Name == key);
                    if (eqimg.Any())
                    {
                        _partsImageEntities.SparePartImage.Remove(eqimg.First());
                    }
                    CommonLogHelper.GetInstance("LogInfo").Info($"删除图片{key}成功");
                    MessageBox.Show(this, @"删除图片成功", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception exception)
                {
                    CommonLogHelper.GetInstance("LogError").Error(@"删除图片失败", exception);
                    MessageBox.Show(this, @"删除图片失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tsbRestore_Click(object sender, EventArgs e)
        {
            _synchContext.Post(a =>
            {
                try
                {
                    _sparePartEntities = new SparePartManagementEntities();
                    var sp = from eq in _sparePartEntities.SpareParts
                             select eq;
                    LoadSparePartData(sp);
                    CommonLogHelper.GetInstance("LogInfo").Info(@"恢复原始备件数据成功");
                    MessageBox.Show(this, @"恢复原始备件数据成功", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception exception)
                {
                    CommonLogHelper.GetInstance("LogError").Error(@"恢复原始备件数据失败", exception);
                    MessageBox.Show(this, @"恢复原始备件数据失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }, null);
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Add)
                {
                    SpareParts ce = new SpareParts()
                    {
                        SerialNo = tbSerialNo.Text,
                        Name = cmbName.Text,
                        Model = cmbModel.Text,
                        Factory = cmbFactory.Text,
                        ProductionDate = dtpOemDate.Value.Date,
                        StoreSpot = cmbStoreSpot.Text,
                        StoreDate = dtpStoreDate.Value.Date,
                        Amount = nUdAmount.Value.ToString(CultureInfo.InvariantCulture),
                        UseType = cmbUseType.Text,
                        Status = cmbStatus.Text,
                    };
                    _sparePartEntities.SpareParts.Add(ce);
                    _partsImageEntities.SparePartImage.AddRange(_spImgList);
                }
                else
                {
                    var spfirst = (from eq in _sparePartEntities.SpareParts
                                   where eq.SerialNo == _id
                                   select eq).First();

                    spfirst.SerialNo = tbSerialNo.Text;
                    spfirst.Name = cmbName.Text;
                    spfirst.Model = cmbModel.Text;
                    spfirst.Factory = cmbFactory.Text;
                    spfirst.ProductionDate = dtpOemDate.Value.Date;
                    spfirst.StoreSpot = cmbStoreSpot.Text;
                    spfirst.StoreDate = dtpStoreDate.Value.Date;
                    spfirst.Amount = nUdAmount.Value.ToString(CultureInfo.InvariantCulture);
                    spfirst.UseType = cmbUseType.Text;
                    spfirst.Status = cmbStatus.Text;
                    //var spImginDb = from img in _partsImageEntities.SparePartImage
                    //                 where img.SerialNo == spfirst.SerialNo
                    //                 select img;
                    //foreach (var sp in spImginDb)
                    //{
                    //    var noimg = from n in _spImgList
                    //                where n.Name == sp.Name
                    //                select n;
                    //    if (!noimg.Any())
                    //    {
                    //        _partsImageEntities.SparePartImage.Remove(sp);
                    //    }
                    //}
                    foreach (var sparePartImage in _spImgList)
                    {
                        var spimg = from img in _partsImageEntities.SparePartImage
                                    where img.Name == sparePartImage.Name
                                    select img;
                        if (!spimg.Any())
                        {
                            _partsImageEntities.SparePartImage.Add(sparePartImage);
                        }
                    }
                }
                _sparePartEntities.SaveChanges();
                _partsImageEntities.SaveChanges();
                SaveSuccess?.Invoke();
                CommonLogHelper.GetInstance("LogInfo").Info(@"保存备件数据成功");
                MessageBox.Show(this, @"保存备件数据成功", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception exception)
            {
                CommonLogHelper.GetInstance("LogError").Error(@"保存备件数据失败", exception);
                MessageBox.Show(this, @"保存备件数据失败" + exception.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tbSerialNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = PublicFunction.JudgeKeyPress(e.KeyChar);
        }
    }
}