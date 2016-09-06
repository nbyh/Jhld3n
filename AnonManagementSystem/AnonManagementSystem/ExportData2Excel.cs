using EquipmentInformationData;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;

namespace AnonManagementSystem
{
    public static class ExportData2Excel
    {
        private static List<string> DataConvert(CombatEquipment eq)
        {
            List<string> eqStrList = new List<string>
            {
                eq.SerialNo,
                eq.Name,
                eq.Model,
                eq.Factory,
                eq.OemNo,
                eq.ProductionDate.ToString("yyyy/MM/dd"),
                eq.MajorCategory,
                eq.SubDepartment,
                eq.TechCondition,
                eq.UseCondition,
                eq.Manager,
                eq.Technician,
                eq.MajorComp,
                eq.PerformIndex,
                eq.MainUsage,
                eq.UseMethod,
                eq.TechRemould,
                eq.SetupVideo
            };
            return eqStrList;
        }
        private static List<string> DataConvert(CombatVehicles vh)
        {
            List<string> eqStrList = new List<string>
            {
                vh.SerialNo,
                vh.Name,
                vh.Model,
                vh.Factory,
                vh.OemNo,
                vh.ProductionDate.ToString("yyyy/MM/dd"),
                vh.MajorCategory,
                vh.SubDepartment,
                vh.TechCondition,
                vh.UseCondition,
                vh.Manager,
                vh.Technician,
                vh.MajorComp,
                vh.PerformIndex,
                vh.MainUsage,
                vh.UseMethod,
                vh.TechRemould,
                vh.SetupVideo
            };
            return eqStrList;
        }
        public static bool ExportData(string filepath, EquipExcelDataStruct eeds)
        {
            using (ExcelPackage package = new ExcelPackage(new FileInfo(filepath)))
            {

                #region 添加设备信息表
                ExcelWorksheet worksheeteq = package.Workbook.Worksheets.Add("设备信息");//创建worksheet
                worksheeteq.Cells.Style.ShrinkToFit = true;//单元格自动适应大小
                EquipTitleCollection eqtitle = new EquipTitleCollection();
                for (int i = 0; i < eqtitle.Length; i++)
                {
                    worksheeteq.Cells[1, i + 1].Value = eqtitle[i];
                }
                List<string> eqContentList = DataConvert(eeds.Equip);
                for (int i = 0; i < eqContentList.Count; i++)
                {
                    worksheeteq.Cells[2, i + 1].Value = eqContentList[i];
                }

                #endregion

                #region 添加车辆信息表

                ExcelWorksheet worksheetvh = package.Workbook.Worksheets.Add("车辆信息"); //创建workshee
                worksheetvh.Cells.Style.ShrinkToFit = true; //单元格自动适应大小
                VehicleTitleCollection vhtitle = new VehicleTitleCollection();
                for (int i = 0; i < vhtitle.Length; i++)
                {
                    worksheetvh.Cells[1, i + 1].Value = vhtitle[i];
                }

                #endregion


                //todo：填充数据
                package.Save();//保存excel
            }
            return true;
        }

        public static bool ExportData(string filepath, SpareExcelDataStruct seds)
        {
            using (ExcelPackage package = new ExcelPackage(new FileInfo(filepath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("备件信息");//创建worksheet
                worksheet.Cells.Style.ShrinkToFit = true;//单元格自动适应大小
                List<string> sptitlesList = new List<string>()
                {
                    "备件编号","备件名称","型号","用于型号","数量","状态","生产厂家","出厂日期","库存位置","入库时间"
                };
                for (int i = 0; i < sptitlesList.Count; i++)
                {
                    worksheet.Cells[1, i + 1].Value = sptitlesList[i];
                }

                //todo：填充数据
                package.Save();//保存excel
            }
            return true;
        }
    }

    public class EquipExcelDataStruct
    {
        public List<EquipmentImage> EqImg { get; set; }
        public CombatEquipment Equip { get; set; }
        public Dictionary<string, List<EventData>> EventDic { get; set; }
        public List<Events> Events { get; set; }
        public OilEngine Oe { get; set; }
        public List<CombatVehicles> VhList { get; set; }
    }

    public class SpareExcelDataStruct
    {
        public SpareParts SparePart { get; set; }
        public List<SparePartImage> SpImgList { get; set; }
    }

    public class EquipTitleCollection
    {
        private readonly string[] _titles = { "设备编号", "设备名称", "型号", "生产厂家", "出厂编号", "出厂日期", "专业分类", "隶属单位", "技术状态", "使用状态", "负责人", "技术员", "主要组成", "性能指标", "主要用途", "运用方法", "技术改造情况", "架设视频链接" };

        public int Length => _titles.Length;

        public string this[int i] => _titles[i];
    }

    public class VehicleTitleCollection
    {
        private readonly string[] _titles = { "车辆编号", "车辆名称", "型号", "发动机型号", "车牌号", "生产厂家", "出厂日期", "技术状态", "车辆负责人", "整备质量", "油箱容量", "静态外廓尺寸", "车库号", "燃料类型", "驱动方式", "里程读数", "排量", "核载人数", "车辆描述", "是否包括油机" };

        public int Length => _titles.Length;

        public string this[int i] => _titles[i];
    }
}
