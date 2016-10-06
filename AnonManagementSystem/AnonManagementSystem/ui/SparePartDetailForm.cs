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
using LinqToDB;
using LinqToDB.DataProvider.SQLite;

namespace AnonManagementSystem
{
    public partial class SparePartDetailForm : Form, IAddModify
    {
        public delegate void SaveChangeSuccess();
        public event SaveChangeSuccess SaveSuccess;
        private readonly SynchronizationContext _synchContext;
        private readonly SparePartImagesDB _partsImageDb = new SparePartImagesDB(new SQLiteDataProvider(), DbPublicFunction.ReturnDbConnectionString(@"\ZBDataBase\SparePartImages.db"));
        private List<SparePartImage> _spImgList = new List<SparePartImage>();
        private bool _enableedit;
        private string _id;
        private SparePartManagementDB _sparePartDb = new SparePartManagementDB(new SQLiteDataProvider(), DbPublicFunction.ReturnDbConnectionString(@"\ZBDataBase\SparePartManagement.db"));
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

        private void LoadSparePartData(IQueryable<SparePart> sparePart)
        {
            var appointsp = from eq in sparePart
                            where eq.SerialNo == _id
                            select eq;
            var spfirst = appointsp.First();
            cmbName.SelectedText = spfirst.Name;
            cmbModel.SelectedText = spfirst.Model;
            ssbStoreSpot.Text = spfirst.StoreSpot;
            tbSerialNo.Text = spfirst.SerialNo;
            cmbStatus.Text = spfirst.Status;
            dtpStoreDate.Value = spfirst.StoreDate;
            tbUseType.Text = spfirst.UseType;
            nUdAmount.Value = int.Parse(spfirst.Amount);
            cmbFactory.SelectedText = spfirst.Factory;
            dtpOemDate.Value = spfirst.ProductionDate;
            
            var imgs = (from img in _partsImageDb.SparePartImages
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
            tbSerialNo.Enabled = Add;
            tsbRestore.Visible = !Add;
            tsDetail.Enabled = gbBaseInfo.Enabled = 更新图片ToolStripMenuItem.Enabled = _enableedit;
        }

        private void SparePartDetailForm_Shown(object sender, EventArgs e)
        {
            _synchContext.Post(a =>
            {
                try
                {
                    var sp = from eq in _sparePartDb.SpareParts
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
                    IEnumerable<string> spStoreSpotList =
                        (from s in sp where !string.IsNullOrEmpty(s.StoreSpot) select s.StoreSpot).Distinct().ToList();
                    List<string> spSsSub1 = spStoreSpotList.Select(s => s.Substring(0, 1)).Distinct().ToList();
                    List<string> spSsSub2 = spStoreSpotList.Select(s => s.Substring(1, 1)).Distinct().ToList();
                    List<string> spSsSub3 = spStoreSpotList.Select(s => s.Substring(2, 1)).Distinct().ToList();
                    ssbStoreSpot.DataSource1 = spSsSub1;
                    ssbStoreSpot.DataSource2 = spSsSub2;
                    ssbStoreSpot.DataSource3 = spSsSub3;
                    //List<string> spTechcanList =
                    //    (from s in sp where !string.IsNullOrEmpty(s.UseType) select s.UseType).Distinct();
                    //tbUseType.DataSource = spTechcanList;
                    List<string> spManagerList =
                        (from s in sp where !string.IsNullOrEmpty(s.Status) select s.Status).Distinct().ToList();
                    cmbStatus.DataSource = spManagerList;

                    #endregion 下拉列表内容

                    if (Add)
                    {
                        cmbName.SelectedIndex = -1;
                        cmbModel.SelectedIndex = -1;
                        cmbStatus.SelectedIndex = -1;
                        cmbFactory.SelectedIndex = -1;
                        ssbStoreSpot.Clear();
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
                if (MainPublicFunction.CheckImgCondition(imgpath))
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
                    _partsImageDb.SparePartImages.Where(img => img.Name == key).Delete();
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
                    _sparePartDb = new SparePartManagementDB(new SQLiteDataProvider(), DbPublicFunction.ReturnDbConnectionString(@"\ZBDataBase\SparePartManagement.db"));
                    var sp = from eq in _sparePartDb.SpareParts
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
                if (ssbStoreSpot.Text.Length < 3)
                {
                    MessageBox.Show(@"库存位置请至少填到层为止", @"警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ssbStoreSpot.Focus();
                    return;
                }
                if (Add)
                {
                    SparePart ce = new SparePart()
                    {
                        SerialNo = tbSerialNo.Text,
                        Name = cmbName.Text,
                        Model = cmbModel.Text,
                        Factory = cmbFactory.Text,
                        ProductionDate = dtpOemDate.Value.Date,
                        StoreSpot = ssbStoreSpot.Text,
                        StoreDate = dtpStoreDate.Value.Date,
                        Amount = nUdAmount.Value.ToString(CultureInfo.InvariantCulture),
                        UseType = tbUseType.Text,
                        Status = cmbStatus.Text,
                    };
                    _sparePartDb.InsertOrReplace(ce);
                    _partsImageDb.InsertOrReplace(_spImgList);
                }
                else
                {
                    var spfirst = (from eq in _sparePartDb.SpareParts
                                   where eq.SerialNo == _id
                                   select eq).First();

                    spfirst.SerialNo = tbSerialNo.Text;
                    spfirst.Name = cmbName.Text;
                    spfirst.Model = cmbModel.Text;
                    spfirst.Factory = cmbFactory.Text;
                    spfirst.ProductionDate = dtpOemDate.Value.Date;
                    spfirst.StoreSpot = ssbStoreSpot.Text;
                    spfirst.StoreDate = dtpStoreDate.Value.Date;
                    spfirst.Amount = nUdAmount.Value.ToString(CultureInfo.InvariantCulture);
                    spfirst.UseType = tbUseType.Text;
                    spfirst.Status = cmbStatus.Text;
                    //var spImginDb = from img in _partsImageDB.SparePartImage
                    //                 where img.SerialNo == spfirst.SerialNo
                    //                 select img;
                    //foreach (var sp in spImginDb)
                    //{
                    //    var noimg = from n in _spImgList
                    //                where n.Name == sp.Name
                    //                select n;
                    //    if (!noimg.Any())
                    //    {
                    //        _partsImageDB.SparePartImage.Remove(sp);
                    //    }
                    //}
                    foreach (var sparePartImage in _spImgList)
                    {
                        var spimg = from img in _partsImageDb.SparePartImages
                                    where img.Name == sparePartImage.Name
                                    select img;
                        if (!spimg.Any())
                        {
                            _partsImageDb.InsertOrReplace(sparePartImage);
                        }
                    }
                }
                //_sparePartDB;
                //_partsImageDB.SaveChanges();
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

        private void tbNumDig_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = MainPublicFunction.JudgeNumCharKeys(e.KeyChar);
        }

        private void tsbSaveImage_Click(object sender, EventArgs e)
        {
            ilvEquipment.SaveImages();
        }
    }
}