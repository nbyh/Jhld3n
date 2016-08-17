using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using EquipmentInformationData;

namespace AnonManagementSystem
{
    public class PublicFunction
    {
        internal const double MAX_FILE_SIZE = 1048576;
        internal const double MAX_IMAGE_HEIGHT = 768;
        internal const double MAX_IMAGE_WIDTH = 1024;

        public static bool CheckFileSize(string filepath)
        {
            var fi = new FileInfo(filepath);
            double fz = fi.Length;
            return fz <= MAX_FILE_SIZE;
        }

        public static bool CheckImageSize(Image img)
        {
            return img.Width <= MAX_IMAGE_WIDTH && img.Height <= MAX_IMAGE_HEIGHT;
        }

        public static byte[] ReturnImgBytes(string imgpath)
        {
            if (!CheckFileSize(imgpath))
            {
                MessageBox.Show(@"文件尺寸太大，请选择小于1MB的图片", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
            FileStream fs = new FileStream(imgpath, FileMode.Open, FileAccess.Read);
            Image img = Image.FromStream(fs);
            if (!CheckImageSize(img))
            {
                MessageBox.Show(@"图片尺寸太大，请选择宽小于1024、高小于768像素的图片", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
            BinaryReader br = new BinaryReader(fs);
            byte[] imgBytes = br.ReadBytes((int)fs.Length);
            fs.Close();
            return imgBytes;
        }

        public static bool Export2Excel(string filepath, CombatEquipment equip, List<CombatVehicles> vhList, OilEngine oe, List<Events> events, Dictionary<string, List<EventData>> eventDic, List<EquipmentImage> eqImg)
        {
            using (ExcelPackage package = new ExcelPackage(new FileInfo(filepath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("设备信息");//创建worksheet




                //todo：填充数据
                package.Save();//保存excel
            }
            return true;
        }

        public static bool CompareTime(string express, DateTime dt1, DateTime dt2)
        {
            switch (express)
            {
                case ">":
                    return dt1 > dt2;

                case "<":
                    return dt1 < dt2;

                case "≥":
                    return dt1 >= dt2;

                case "≤":
                    return dt1 <= dt2;

                default:
                    return dt1 == dt2;
            }
        }
    }
}