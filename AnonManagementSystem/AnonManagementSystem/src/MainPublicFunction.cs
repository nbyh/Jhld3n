using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AnonManagementSystem
{
    public class MainPublicFunction
    {
        internal const double MaxFileSize = 1048576;
        internal const double MaxImageHeight = 768;
        internal const double MaxImageWidth = 1024;

        public static bool CheckFileSize(string filepath)
        {
            var fi = new FileInfo(filepath);
            double fz = fi.Length;
            return fz <= MaxFileSize;
        }

        public static bool CheckImageSize(Image img)
        {
            return img.Width <= MaxImageWidth && img.Height <= MaxImageHeight;
        }

        public static bool CheckImgCondition(string imgpath)
        {
            if (!CheckFileSize(imgpath))
            {
                MessageBox.Show(@"文件尺寸太大，请选择小于1MB的图片", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            Image img = Image.FromFile(imgpath);
            if (!CheckImageSize(img))
            {
                MessageBox.Show(@"图片尺寸太大，请选择宽小于1024、高小于768像素的图片", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        public static bool JudgeNumCharKeys(char key)
        {
            int keyValue = key;
            return (keyValue < 48 || keyValue > 57) && keyValue != 45 && keyValue != 8 && keyValue != 127 && keyValue != 1 &&
                   keyValue != 3 && keyValue != 22 && keyValue != 24 && keyValue != 25 && keyValue != 26;
        }

        public static bool JudgeNumDigKeys(char key)
        {
            int keyValue = key;
            return (keyValue < 48 || keyValue > 57) && keyValue != 45 && keyValue != 8 && keyValue != 127 && keyValue != 1 &&
                   keyValue != 3 && keyValue != 22 && keyValue != 24 && keyValue != 25 && keyValue != 26;
        }
    }
}