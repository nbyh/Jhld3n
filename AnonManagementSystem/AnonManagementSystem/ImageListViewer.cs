using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AnonManagementSystem
{
    public partial class ImageListViewer : UserControl, IShowImageList
    {
        public ImageListViewer()
        {
            InitializeComponent();
        }

        public Dictionary<string, Image> ImgDictionary { get; set; }

        public void ShowImages()
        {
            foreach (var imgdic in ImgDictionary)
            {
                imgList.Images.Add(imgdic.Key, imgdic.Value);
            }
            lsvImages.Items.Clear();
            lsvImages.LargeImageList = imgList;
            lsvImages.BeginUpdate();
            for (int i = 0; i < imgList.Images.Count; i++)
            {
                ListViewItem lvi = new ListViewItem
                {
                    ImageIndex = i,
                    Text = imgList.Images.Keys[i]
                };
                lsvImages.Items.Add(lvi);
            }
            lsvImages.EndUpdate();
        }

        private void lsvImages_DoubleClick(object sender, EventArgs e)
        {
            if (lsvImages.SelectedItems.Count == 0)
                return;
            string picname = lsvImages.SelectedItems[0].Text;
            picBox.Image = ImgDictionary[picname];
        }
    }
}
