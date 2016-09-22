//---------------------------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated by T4Model template for T4 (https://github.com/linq2db/t4models).
//    Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//---------------------------------------------------------------------------------------------------
using System;
using System.Linq;

using LinqToDB;
using LinqToDB.DataProvider;
using LinqToDB.Mapping;

namespace EquipmentInformationData
{
	/// <summary>
	/// Database       : SystemManager
	/// Data Source    : SystemManager
	/// Server Version : 3.9.2
	/// </summary>
	public partial class SystemManagerDB : LinqToDB.Data.DataConnection
	{
		public ITable<UserManage> UserManages { get { return this.GetTable<UserManage>(); } }

		public SystemManagerDB()
		{
			InitDataContext();
		}

		public SystemManagerDB(string configuration)
			: base(configuration)
		{
			InitDataContext();
		}

		public SystemManagerDB(IDataProvider dataProvider, string configuration)
			: base(dataProvider, configuration)
		{
			InitDataContext();
		}

		partial void InitDataContext();
	}

	[Table("UserManage")]
	public partial class UserManage
	{
		[PrimaryKey, Identity] public long   ID       { get; set; } // integer
		[Column,     NotNull ] public string User     { get; set; } // nvarchar(12)
		[Column,     NotNull ] public string Password { get; set; } // nvarchar(12)
		[Column,     NotNull ] public bool   Edit     { get; set; } // bool
	}

	public static partial class TableExtensions
	{
		public static UserManage Find(this ITable<UserManage> table, long ID)
		{
			return table.FirstOrDefault(t =>
				t.ID == ID);
		}
	}
}
