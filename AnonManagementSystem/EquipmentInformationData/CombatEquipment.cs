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
    
    public partial class CombatEquipment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CombatEquipment()
        {
            this.CombatVehicles = new HashSet<CombatVehicles>();
            this.Material = new HashSet<Material>();
            this.Train = new HashSet<Train>();
            this.Events = new HashSet<Events>();
        }
    
        public long ID { get; set; }
        public string Name { get; set; }
        public string SerialNo { get; set; }
        public string MajorCategory { get; set; }
        public string Model { get; set; }
        public string SubDepartment { get; set; }
        public string UseType { get; set; }
        public string TechCondition { get; set; }
        public string UseCondition { get; set; }
        public string Factory { get; set; }
        public Nullable<System.DateTime> FactoryTime { get; set; }
        public Nullable<long> ServiceLife { get; set; }
        public string InventorySpot { get; set; }
        public string MajorComp { get; set; }
        public string MainUsage { get; set; }
        public string PerformIndex { get; set; }
        public string UseMethod { get; set; }
        public string Manager { get; set; }
        public string User { get; set; }
        public string Maintainer { get; set; }
        public string Technician { get; set; }
        public Nullable<long> Image { get; set; }
        public Nullable<long> OilEngine { get; set; }
    
        public virtual OilEngine OilEngine1 { get; set; }
        public virtual EquipImage EquipImage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CombatVehicles> CombatVehicles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Material> Material { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Train> Train { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Events> Events { get; set; }
    }
}
