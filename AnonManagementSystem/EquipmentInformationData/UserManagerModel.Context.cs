﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

using System.Data.SQLite;

namespace EquipmentInformationData
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class SystemManagerEntities : DbContext
    {
        public SystemManagerEntities()
            : base("name=SystemManagerEntities")
        {
        }

        //public SystemManagerEntities(string connectionString)
        //    : base(new SQLiteConnection() { ConnectionString = connectionString }, true)
        //{
        //}

        public SystemManagerEntities(string filename)
            : base(new SQLiteConnection()
            {
                ConnectionString = new SQLiteConnectionStringBuilder()
                {
                    DataSource = filename,
                    ForeignKeys = true
                }
            .ConnectionString
            }, true)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<UserManage> UserManage { get; set; }
    }
}