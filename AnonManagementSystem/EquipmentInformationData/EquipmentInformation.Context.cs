﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class EquipmentManagementEntities : DbContext
    {
        public EquipmentManagementEntities()
            : base("name=EquipmentManagementEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CombatEquipment> CombatEquipment { get; set; }
        public virtual DbSet<CombatVehicles> CombatVehicles { get; set; }
        public virtual DbSet<EventData> EventData { get; set; }
        public virtual DbSet<Events> Events { get; set; }
        public virtual DbSet<Material> Material { get; set; }
        public virtual DbSet<OilEngine> OilEngine { get; set; }
    }
}
