using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace AnonManagementSystem
{
    public partial class ImageListViewer : UserControl
    {
        private Dictionary<string, Image> _imgDic = new Dictionary<string, Image>();

        public ImageListViewer()
        {
            InitializeComponent();
        }

        public string DeleteImgKey { get; set; }

        public Dictionary<string, Image> ImgDictionary
        {
            get { return _imgDic; }
            set { _imgDic = value; }
        }

        public void AddImages(string key, Image img)
        {
            if (_imgDic.ContainsKey(key))
            {
                MessageBox.Show(@"已经包含相同名称的图片，请选择不同名称的图片！", @"警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            FileInfo fi = new FileInfo(key);
            imgList.Images.Add(key, img);
            _imgDic.Add(key, img);
            //lsvImages.LargeImageList = imgList;
            lsvImages.BeginUpdate();
            ListViewItem lvi = new ListViewItem
            {
                ImageKey = key,
                Text = fi.Name
            };
            lsvImages.Items.Add(lvi);
            lsvImages.EndUpdate();
            lvi.Selected = true;
            picBox.Image = img;
        }

        public void DeleteImages()
        {
            if (lsvImages.SelectedItems.Count > 0)
            {
                lsvImages.BeginUpdate();
                foreach (ListViewItem lvi in lsvImages.SelectedItems)
                {
                    lsvImages.Items.Remove(lvi);
                    DeleteImgKey = lvi.ImageKey;
                    _imgDic.Remove(DeleteImgKey);
                }
                if (lsvImages.Items.Count - 1 >= 0)
                {
                    int selectindex = lsvImages.Items.Count - 1;
                    lsvImages.Items[selectindex].Selected = true;
                    picBox.Image = _imgDic[lsvImages.Items[selectindex].ImageKey];
                }
                else
                {
                    picBox.Image = null;
                }
                lsvImages.EndUpdate();
            }
        }

        public void SaveImages()
        {
            try
            {
                if (picBox.Image != null)
                {
                    if (sfdImg.ShowDialog() == DialogResult.OK)
                    {
                        Bitmap bmp = new Bitmap(picBox.Image);
                        bmp.Save(sfdImg.FileName, ImageFormat.Jpeg);
                        bmp.Dispose();
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"图片保存失败！{exception.Message}", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ShowImages()
        {
            foreach (var imgdic in _imgDic)
            {
                imgList.Images.Add(imgdic.Key, imgdic.Value);
            }
            lsvImages.Items.Clear();
            lsvImages.LargeImageList = imgList;
            lsvImages.BeginUpdate();
            for (int i = 0; i < imgList.Images.Count; i++)
            {
                FileInfo fi = new FileInfo(imgList.Images.Keys[i]);
                ListViewItem lvi = new ListViewItem
                {
                    ImageKey = imgList.Images.Keys[i],
                    Text = fi.Name
                };
                lsvImages.Items.Add(lvi);
            }
            lsvImages.EndUpdate();
            if (lsvImages.Items.Count > 0)
            {
                string picname = lsvImages.Items[0].ImageKey;
                picBox.Image = _imgDic[picname];
            }
        }

        private void lsvImages_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (lsvImages.SelectedItems.Count == 0)
                    return;
                string picname = lsvImages.SelectedItems[0].ImageKey;
                picBox.Image = _imgDic[picname];
            }
            catch (Exception exception)
            {
                MessageBox.Show($"图片显示失败！{exception.Message}", @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}