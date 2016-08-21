using EquipmentInformationData;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

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

        public static bool Export2Excel(string filepath, CombatEquipment equip, List<CombatVehicles> vhList, OilEngine oe, List<Events> events, Dictionary<string, List<EventData>> eventDic, List<EquipmentImage> eqImg)
        {
            using (ExcelPackage package = new ExcelPackage(new FileInfo(filepath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("设备信息");//创建worksheet
                List<string> titlesList = new List<string>()
                {
                    "设备编号","设备名称","型号","生产厂家","出厂编号","出厂日期","专业分类","隶属单位","技术状态","使用状态","负责人","技术员","主要组成","性能指标","主要用途","运用方法","技术改造情况","架设视频链接"
                 };
                worksheet.Cells.LoadFromCollection(titlesList, true);

                //todo：填充数据
                package.Save();//保存excel
            }
            return true;
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

        internal static bool Export2Excel(string filepath, SpareParts firstsp, List<SparePartImage> spimgList)
        {
            using (ExcelPackage package = new ExcelPackage(new FileInfo(filepath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("备件信息");//创建worksheet
                List<string> titlesList = new List<string>()
                {
                    "备件编号","备件名称","型号","用于型号","数量","状态","生产厂家","出厂日期","库存位置","入库时间"
                };
                worksheet.Cells.LoadFromCollection(titlesList, true);

                //todo：填充数据
                package.Save();//保存excel
            }
            return true;
        }
    }
}