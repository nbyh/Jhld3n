using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AnonManagementSystem
{
    public partial class ImageListViewer : UserControl, IShowImageList
    {
        public ImageListViewer()
        {
            InitializeComponent();
        }

        public string DeleteImgKey { get; set; }
        private Dictionary<string, Image> _imgDic = new Dictionary<string, Image>();

        public Dictionary<string, Image> ImgDictionary
        {
            get { return _imgDic; }
            set { _imgDic = value; }
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
        }

        public void AddImages(string key, Image img)
        {
            FileInfo fi = new FileInfo(key);
            imgList.Images.Add(key, img);
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
                lsvImages.EndUpdate();
            }
        }

        private void lsvImages_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (lsvImages.SelectedItems.Count == 0)
                    return;
                string picname = lsvImages.SelectedItems[0].Text;
                picBox.Image = _imgDic[picname];
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}