using System;
using EquipmentInformationData;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;

namespace AnonManagementSystem
{
    public static class ExportData2Excel
    {
        public static void ExportData(string filepath, EquipExcelDataStruct eeds)
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
                for (int i = 0; i < eeds.VhList.Count; i++)
                {
                    List<string> vhContentList = DataConvert(eeds.VhList[i]);
                    for (int j = 0; j < vhContentList.Count; j++)
                    {
                        worksheetvh.Cells[i + 1, j + 1].Value = vhContentList[i];
                    }
                }
                #endregion

                #region 添加油机信息表

                if (eeds.Oe != null)
                {
                    ExcelWorksheet worksheetoe = package.Workbook.Worksheets.Add("油机信息"); //创建workshee
                    worksheetoe.Cells.Style.ShrinkToFit = true; //单元格自动适应大小
                    OeTitleCollection oetitle = new OeTitleCollection();
                    for (int i = 0; i < oetitle.Length; i++)
                    {
                        worksheetoe.Cells[1, i + 1].Value = oetitle[i];
                    }
                    List<string> oeContentList = DataConvert(eeds.Oe);
                    for (int i = 0; i < oeContentList.Count; i++)
                    {
                        worksheetoe.Cells[2, i + 1].Value = oeContentList[i];
                    }
                }
                #endregion

                #region 添加活动信息表

                ExcelWorksheet worksheetee = package.Workbook.Worksheets.Add("活动信息"); //创建workshee
                worksheetee.Cells.Style.ShrinkToFit = true; //单元格自动适应大小
                EventTitleCollection eetitle = new EventTitleCollection();
                for (int i = 0; i < eetitle.Length; i++)
                {
                    worksheetee.Cells[1, i + 1].Value = eetitle[i];
                }
                for (int i = 0; i < eeds.Events.Count; i++)
                {
                    List<string> eeContentList = DataConvert(eeds.Events[i]);
                    for (int j = 0; j < eeContentList.Count; j++)
                    {
                        worksheetee.Cells[i + 1, j + 1].Value = eeContentList[i];
                    }
                }
                #endregion

                #region 添加材料信息表

                ExcelWorksheet worksheetmt = package.Workbook.Worksheets.Add("材料信息"); //创建workshee
                worksheetmt.Cells.Style.ShrinkToFit = true; //单元格自动适应大小
                MaterialTitleCollection mttitle = new MaterialTitleCollection();
                for (int i = 0; i < mttitle.Length; i++)
                {
                    worksheetmt.Cells[1, i + 1].Value = mttitle[i];
                }
                for (int i = 0; i < eeds.MaterialList.Count; i++)
                {
                    List<string> mtContentList = DataConvert(eeds.MaterialList[i]);
                    for (int j = 0; j < mtContentList.Count; j++)
                    {
                        worksheetmt.Cells[i + 1, j + 1].Value = mtContentList[i];
                    }
                }
                #endregion
                //todo：填充数据
                package.Save();//保存excel
            }
        }

        public static void ExportData(string filepath, SpareExcelDataStruct seds)
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
                List<string> eqContentList = DataConvert(seds.SparePart);
                for (int i = 0; i < eqContentList.Count; i++)
                {
                    worksheet.Cells[2, i + 1].Value = eqContentList[i];
                }
                //todo：填充数据
                package.Save();//保存excel
            }
        }

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
                vh.MotorModel,
                vh.VehiclesNo,
                vh.Factory,
                vh.ProductionDate.ToString("yyyy/MM/dd"),
                vh.TechCondition,
                vh.VehicleChargers,
                vh.Mass,
                vh.Tankage,
                vh.OverallSize,
                vh.VehicleSpotNo,
                vh.FuelType,
                vh.DrivingModel,
                vh.Mileage,
                vh.Output,
                vh.LicenseCarry,
                vh.VehicleDescri,
                vh.CombineOe?"是":"否"
            };
            return eqStrList;
        }

        private static List<string> DataConvert(OilEngine oe)
        {
            List<string> oeStrList = new List<string>
            {
                oe.OeNo,
                oe.OeModel,
                oe.OutPower,
                oe.TechCondition,
                oe.WorkHour,
                oe.OeFactory,
                oe.OeDate.ToString("yyyy/MM/dd"),
                oe.OeOemNo,
                oe.MotorModel,
                oe.MotorPower,
                oe.MotorFuel,
                oe.MotorTankage,
                oe.MotorFactory,
                oe.MotorDate.ToString("yyyy/MM/dd"),
                oe.MotorOemNo,
                oe.FaultDescri,
            };
            return oeStrList;
        }

        private static List<string> DataConvert(Events eve)
        {
            List<string> eqEveList = new List<string>
            {
                eve.No ,
                eve.Name ,
                eve.StartTime .ToString("yyyy/MM/dd"),
                eve.EndTime.ToString("yyyy/MM/dd") ,
                eve.Address ,
                eve.EventType ,
                eve.SpecificType ,
                eve.Code ,
                eve.PublishUnit ,
                eve.PublishDate.ToString("yyyy/MM/dd") ,
                eve.Publisher ,
                eve.According ,
                eve.HigherUnit ,
                eve.Executor ,
                eve.NoInEvents ,
                eve.PeopleDescri ,
                eve.ProcessDescri ,
                eve.HandleStep ,
                eve.Problem ,
                eve.Remarks ,
            };
            return eqEveList;
        }

        private static List<string> DataConvert(Material mt)
        {
            List<string> eqMtList = new List<string>
            {
                mt.No ,
                mt.Name ,
                mt.Volume ,
                mt.PaperSize ,
                mt.Edition ,
                mt.Pagination ,
                mt.Date.ToString("yyyy/MM/dd") ,
                mt.StoreSpot ,
                mt.DocumentLink ,
                mt.Content
            };
            return eqMtList;
        }

        private static List<string> DataConvert(SpareParts sp)
        {
            List<string> eqSpList = new List<string>
            {
                sp.SerialNo ,
                sp.Name ,
                sp.Model ,
                sp.UseType ,
                sp.Amount ,
                sp.Status ,
                sp.Factory ,
                sp.ProductionDate.ToString("yyyy/MM/dd") ,
                sp.StoreSpot ,
                sp.StoreDate.ToString("yyyy/MM/dd") ,
            };
            return eqSpList;
        }
    }

    public class EquipExcelDataStruct
    {
        public List<EquipmentImage> EqImg { get; set; }
        public CombatEquipment Equip { get; set; }
        public Dictionary<string, List<EventData>> EventDic { get; set; }
        public List<Events> Events { get; set; }
        public List<Material> MaterialList { get; set; }
        public OilEngine Oe { get; set; }
        public List<CombatVehicles> VhList { get; set; }
    }

    public class EquipTitleCollection
    {
        private readonly string[] _titles = { "设备编号", "设备名称", "型号", "生产厂家", "出厂编号", "出厂日期", "专业分类", "隶属单位", "技术状态", "使用状态", "负责人", "技术员", "主要组成", "性能指标", "主要用途", "运用方法", "技术改造情况", "架设视频链接" };

        public int Length => _titles.Length;

        public string this[int i] => _titles[i];
    }

    public class EventTitleCollection
    {
        private readonly string[] _titles = { "活动编号", "活动名称", "开始时间", "结束时间", "地点", "活动类型", "具体类型", "活动代码", "发文单位", "发文时间", "发文人", "依据", "上级单位", "本级单位", "活动中本套编号", "参加单位及人员描述", "过程描述", "处理措施", "遗留问题", "备注" };

        public int Length => _titles.Length;

        public string this[int i] => _titles[i];
    }

    public class MaterialTitleCollection
    {
        private readonly string[] _titles = { "资料编号", "资料名称", "册数", "形态", "版本", "页数", "日期", "存放位置", "文档链接", "内容简介" };

        public int Length => _titles.Length;

        public string this[int i] => _titles[i];
    }

    public class OeTitleCollection
    {
        private readonly string[] _titles = { "机组编号", "型号", "输出功率", "技术状态", "工作时数", "生产厂家", "出厂日期", "出厂编号", "发动机型号", "额定功率", "燃料类型", "油箱容量", "生产厂家", "出厂日期", "出厂编号", "故障描述" };

        public int Length => _titles.Length;

        public string this[int i] => _titles[i];
    }

    public class SpareExcelDataStruct
    {
        public SpareParts SparePart { get; set; }
        public List<SparePartImage> SpImgList { get; set; }
    }

    public class VehicleTitleCollection
    {
        private readonly string[] _titles = { "车辆编号", "车辆名称", "型号", "发动机型号", "车牌号", "生产厂家", "出厂日期", "技术状态", "车辆负责人", "整备质量", "油箱容量", "静态外廓尺寸", "车库号", "燃料类型", "驱动方式", "里程读数", "排量", "核载人数", "车辆描述", "是否包括油机" };

        public int Length => _titles.Length;

        public string this[int i] => _titles[i];
    }
}