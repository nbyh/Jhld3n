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
    
    public partial class Events
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Events()
        {
            this.EventData = new HashSet<EventData>();
        }
    
        public string No { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public string Address { get; set; }
        public string EventType { get; set; }
        public string SpecificType { get; set; }
        public string Code { get; set; }
        public string PublishUnit { get; set; }
        public Nullable<System.DateTime> PublishDate { get; set; }
        public string Publisher { get; set; }
        public string According { get; set; }
        public string PeopleDescri { get; set; }
        public string ProcessDescri { get; set; }
        public string HandleStep { get; set; }
        public string Problem { get; set; }
        public string Remarks { get; set; }
        public string Equipment { get; set; }
    
        public virtual CombatEquipment CombatEquipment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EventData> EventData { get; set; }
    }
}
