//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace EquipmentInformationData
{
    using System;
    using System.Collections.Generic;
    
    public partial class Material
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string No { get; set; }
        public string Type { get; set; }
        public string PaperSize { get; set; }
        public string Pagination { get; set; }
        public string Edition { get; set; }
        public string Volume { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string DocumentLink { get; set; }
        public string Content { get; set; }
        public Nullable<long> Equipment { get; set; }
    
        public virtual CombatEquipment CombatEquipment { get; set; }
    }
}