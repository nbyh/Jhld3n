CREATE TABLE [SpareParts] (
	[SerialNo] nvarchar(20) NOT NULL PRIMARY KEY, 
	[Name] nvarchar(20), 
	[Model] nvarchar(20), 
	[Factory] nvarchar(20), 
	[ProductionDate] date NOT NULL, 
	[StoreSpot] nvarchar(5), 
	[StoreDate] date NOT NULL, 
	[Amount] nvarchar(3), 
	[UseType] nvarchar(15), 
	[Status] nvarchar(6)
);