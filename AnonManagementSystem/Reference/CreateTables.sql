CREATE TABLE [EquipImage] (
	[ID] integer NOT NULL PRIMARY KEY AUTOINCREMENT, 
	[ImageFront] image, 
	[ImageSize1] image, 
	[ImageSize2] image
)
GO

CREATE TABLE [OilEngineImage] (
	[ID] integer NOT NULL PRIMARY KEY AUTOINCREMENT, 
	[ImageFront] image, 
	[ImageSize1] image, 
	[ImageSize2] image, 
	[ImageTop] image
)
GO

CREATE TABLE [OilEngine] (
	[ID] integer NOT NULL PRIMARY KEY AUTOINCREMENT, 
	[OeNo] nvarchar(254), 
	[OeModel] nvarchar(254), 
	[OutPower] nvarchar(254), 
	[OutSize] nvarchar(254), 
	[WorkHour] nvarchar(254), 
	[OeFactory] nvarchar(254), 
	[OeDate] nvarchar(254), 
	[OeOemNo] nvarchar(254), 
	[MotorNo] nvarchar(254), 
	[MotorModel] nvarchar(254), 
	[MotorPower] nvarchar(254), 
	[MotorFuel] nvarchar(254), 
	[MotorTankage] nvarchar(254), 
	[MotorFactory] nvarchar(254), 
	[MotorDate] nvarchar(254), 
	[MotorOemNo] nvarchar(254), 
	[FaultDescri] nvarchar(254), 
	[Image] integer, 
	FOREIGN KEY ([Image])
		REFERENCES [OilEngineImage] ([ID])
		ON UPDATE NO ACTION ON DELETE NO ACTION
)
GO
CREATE UNIQUE INDEX [index_OilEngine_1]
	ON [OilEngine] ([OeNo])
GO

CREATE TABLE [CombatEquipment] (
	[ID] integer NOT NULL PRIMARY KEY AUTOINCREMENT, 
	[Name] text NOT NULL, 
	[SerialNo] nvarchar(50) NOT NULL, 
	[MajorCategory] nvarchar(254), 
	[Model] text NOT NULL, 
	[SubDepartment] text, 
	[UseType] nvarchar(254), 
	[TechCondition] nvarchar(254), 
	[UseCondition] nvarchar(254), 
	[Factory] nvarchar(254), 
	[FactoryTime] date, 
	[ServiceLife] integer, 
	[InventorySpot] text, 
	[MajorComp] nvarchar(254), 
	[MainUsage] nvarchar(254), 
	[PerformIndex] nvarchar(254), 
	[UseMethod] nvarchar(254), 
	[Manager] nvarchar(254), 
	[User] nvarchar(254), 
	[Maintainer] nvarchar(254), 
	[Technician] nvarchar(254), 
	[Image] integer, 
	[OilEngine] integer, 
	FOREIGN KEY ([Image])
		REFERENCES [EquipImage] ([ID])
		ON UPDATE NO ACTION ON DELETE NO ACTION, 
	FOREIGN KEY ([OilEngine])
		REFERENCES [OilEngine] ([ID])
		ON UPDATE NO ACTION ON DELETE NO ACTION
)
GO
CREATE UNIQUE INDEX [index_CombatEquipment_1]
	ON [CombatEquipment] ([SerialNo])
GO

CREATE TABLE [VehiclesImage] (
	[ID] integer NOT NULL PRIMARY KEY AUTOINCREMENT, 
	[ImageFront] image, 
	[ImageSize1] image, 
	[ImageSize2] image
)
GO

CREATE TABLE [CombatVehicles] (
	[ID] integer NOT NULL PRIMARY KEY AUTOINCREMENT, 
	[Name] text, 
	[SerialNo] varchar(25), 
	[Model] nvarchar(254), 
	[VehiclesNo] nvarchar(254), 
	[MotorModel] nvarchar(254), 
	[MotorNo] nvarchar(254), 
	[Factory] nvarchar(254), 
	[ProductionDate] nvarchar(254), 
	[OemNo] nvarchar(254), 
	[Mass] nvarchar(254), 
	[Tankage] nvarchar(254), 
	[OverallSize] nvarchar(254), 
	[ExpandedSize] nvarchar(254), 
	[WheelBase] nvarchar(254), 
	[FuelType] nvarchar(254), 
	[DrivingModel] nvarchar(254), 
	[Mileage] nvarchar(254), 
	[Output] nvarchar(254), 
	[LicenseCarry] nvarchar(254), 
	[SubDepartment] nvarchar(254), 
	[VehicleChargers] nvarchar(254), 
	[UseType] nvarchar(254), 
	[VehicleSpotNo] nvarchar(254), 
	[FaultDescri] nvarchar(254), 
	[Image] integer, 
	[Equipment] integer, 
	FOREIGN KEY ([Image])
		REFERENCES [VehiclesImage] ([ID])
		ON UPDATE NO ACTION ON DELETE NO ACTION, 
	FOREIGN KEY ([Equipment])
		REFERENCES [CombatEquipment] ([ID])
		ON UPDATE NO ACTION ON DELETE NO ACTION
)
GO
CREATE UNIQUE INDEX [index_CombatVehicles_1]
	ON [CombatVehicles] ([SerialNo])
GO

CREATE TABLE [EventData] (
	[ID] integer NOT NULL PRIMARY KEY AUTOINCREMENT, 
	[Name] nvarchar(254), 
	[Spot] nvarchar(254) NOT NULL
)
GO

CREATE TABLE [Events] (
	[ID] integer NOT NULL PRIMARY KEY AUTOINCREMENT, 
	[Name] text, 
	[StartTime] date, 
	[Address] text, 
	[EndTime] date, 
	[No] nvarchar(254), 
	[PartUnit] nvarchar(254), 
	[PulishUnit] nvarchar(254), 
	[PublishDate] date, 
	[EventType] nvarchar(254), 
	[SpecificType] nvarchar(254), 
	[Code] nvarchar(254), 
	[According] nvarchar(254), 
	[Document] nvarchar(254), 
	[PeopleDescri] nvarchar(254), 
	[ProcessDescri] nvarchar(254), 
	[HandleStep] nvarchar(254), 
	[Problem] nvarchar(254), 
	[Remarks] nvarchar(254), 
	[Equipment] integer, 
	[Data] integer, 
	FOREIGN KEY ([Equipment])
		REFERENCES [CombatEquipment] ([ID])
		ON UPDATE NO ACTION ON DELETE NO ACTION, 
	FOREIGN KEY ([Data])
		REFERENCES [EventData] ([ID])
		ON UPDATE NO ACTION ON DELETE NO ACTION
)
GO
CREATE UNIQUE INDEX [index_Events_1]
	ON [Events] ([No])
GO

CREATE TABLE [Material] (
	[ID] integer NOT NULL PRIMARY KEY AUTOINCREMENT, 
	[Name] nvarchar(254), 
	[No] nvarchar(254), 
	[Type] nvarchar(254), 
	[PaperSize] nvarchar(254), 
	[Pagination] nvarchar(254), 
	[Edition] nvarchar(254), 
	[Volume] nvarchar(254), 
	[Date] datetime, 
	[DocumentLink] nvarchar(254), 
	[Content] nvarchar(254), 
	[Equipment] integer, 
	FOREIGN KEY ([Equipment])
		REFERENCES [CombatEquipment] ([ID])
		ON UPDATE NO ACTION ON DELETE NO ACTION
)
GO
CREATE UNIQUE INDEX [index_Material_1]
	ON [Material] ([No])
GO

CREATE TABLE [Train] (
	[ID] integer NOT NULL PRIMARY KEY AUTOINCREMENT, 
	[Name] text, 
	[Trainee] text, 
	[StartTime] date, 
	[Address] text, 
	[Content] text, 
	[Equipment] integer, 
	FOREIGN KEY ([Equipment])
		REFERENCES [CombatEquipment] ([ID])
		ON UPDATE NO ACTION ON DELETE NO ACTION
)
GO