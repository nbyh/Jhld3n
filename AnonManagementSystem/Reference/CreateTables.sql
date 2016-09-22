CREATE TABLE [CombatEquipment] (
	[SerialNo] nvarchar(16) NOT NULL PRIMARY KEY, 
	[Name] nvarchar(20) NOT NULL, 
	[MajorCategory] nvarchar(10), 
	[Model] nvarchar(10), 
	[SubDepartment] nvarchar(5),  
	[TechCondition] nvarchar(6), 
	[UseCondition] nvarchar(6), 
	[Factory] nvarchar(20), 
	[OemNo] nvarchar(10), 
	[ProductionDate] date NOT NULL, 
	[InventorySpot] text, 
	[MajorComp] text, 
	[MainUsage] text, 
	[PerformIndex] text, 
	[UseMethod] text, 
	[Manager] nvarchar(4), 
	[Technician] nvarchar(4), 
	[TechRemould] text, 
	[SetupVideo] text
)
GO
CREATE UNIQUE INDEX [index_CombatEquipment_1]
	ON [CombatEquipment] ([SerialNo])
GO

CREATE TABLE [CombatVehicles] (
	[SerialNo] nvarchar(16) NOT NULL PRIMARY KEY, 
	[Name] nvarchar(20) NOT NULL, 
	[Model] nvarchar(10), 
	[VehiclesNo] nvarchar(7), 
	[MotorModel] nvarchar(10), 
	[TechCondition] nvarchar(4), 
	[Factory] nvarchar(10), 
	[ProductionDate] date NOT NULL, 
	[Mass] nvarchar(5), 
	[Tankage] nvarchar(3), 
	[OverallSize] nvarchar(16), 
	[FuelType] nvarchar(2), 
	[DrivingModel] nvarchar(6), 
	[Mileage] nvarchar(6), 
	[Output] nvarchar(3), 
	[LicenseCarry] nvarchar(1), 
	[VehicleChargers] nvarchar(4), 
	[VehicleSpotNo] nvarchar(3), 
	[VehicleDescri] text, 
	[CombineOe] bool NOT NULL DEFAULT false, 
	[Equipment] nvarchar(16), 
	FOREIGN KEY ([Equipment])
		REFERENCES [CombatEquipment] ([SerialNo])
		ON UPDATE NO ACTION ON DELETE NO ACTION
)
GO
CREATE UNIQUE INDEX [index_CombatVehicles_1]
	ON [CombatVehicles] ([SerialNo])
GO

CREATE TABLE [Events] (
	[No] nvarchar(16) NOT NULL PRIMARY KEY, 
	[Name] nvarchar(10), 
	[StartTime] date NOT NULL, 
	[EndTime] date NOT NULL, 
	[Address] nvarchar(5), 
	[EventType] nvarchar(4), 
	[SpecificType] nvarchar(4), 
	[Code] nvarchar(5), 
	[NoInEvents] nvarchar(4), 
	[HigherUnit] nvarchar(10), 
	[Executor] nvarchar(4), 
	[PublishUnit] nvarchar(5), 
	[PublishDate] date NOT NULL, 
	[Publisher] nvarchar(4), 
	[According] text, 
	[PeopleDescri] nvarchar(50), 
	[ProcessDescri] text, 
	[HandleStep] text, 
	[Problem] text, 
	[Remarks] text, 
	[Equipment] nvarchar(16), 
	FOREIGN KEY ([Equipment])
		REFERENCES [CombatEquipment] ([SerialNo])
		ON UPDATE NO ACTION ON DELETE NO ACTION
)
GO
CREATE UNIQUE INDEX [index_Events_1]
	ON [Events] ([No])
GO

CREATE TABLE [EventData] (
[ID] integer NOT NULL PRIMARY KEY AUTOINCREMENT, 
	[Name] nvarchar(100), 
	[Spot] nvarchar(10), 
	[EventsNo] nvarchar(16), 
	FOREIGN KEY ([EventsNo])
		REFERENCES [Events] ([No])
		ON UPDATE NO ACTION ON DELETE NO ACTION
)
GO

CREATE TABLE [Material] (
	[No] nvarchar(16) NOT NULL PRIMARY KEY, 
	[Name] nvarchar(20), 
	[Type] nvarchar(6), 
	[PaperSize] nvarchar(4), 
	[Pagination] nvarchar(3), 
	[Edition] nvarchar(3), 
	[Volume] nvarchar(2), 
	[Date] date NOT NULL, 
	[DocumentLink] text, 
	[StoreSpot] nvarchar(5), 
	[Content] text, 
	[Equipment] nvarchar(16), 
	FOREIGN KEY ([Equipment])
		REFERENCES [CombatEquipment] ([SerialNo])
		ON UPDATE NO ACTION ON DELETE NO ACTION
)
GO

CREATE TABLE [OilEngine] (
	[OeNo] nvarchar(16) NOT NULL PRIMARY KEY, 
	[OeModel] nvarchar(10), 
	[OutPower] nvarchar(3), 
	[TechCondition] nvarchar(6), 
	[WorkHour] nvarchar(4), 
	[OeFactory] nvarchar(10), 
	[OeDate] date NOT NULL, 
	[OeOemNo] nvarchar(10), 
	[MotorModel] nvarchar(10) NOT NULL, 
	[MotorPower] nvarchar(3), 
	[MotorFuel] nvarchar(2), 
	[MotorTankage] nvarchar(3), 
	[MotorFactory] nvarchar(10), 
	[MotorDate] date NOT NULL, 
	[MotorOemNo] nvarchar(10), 
	[FaultDescri] text, 
	[Vehicle] nvarchar(16), 
	FOREIGN KEY ([Vehicle])
		REFERENCES [CombatVehicles] ([SerialNo])
		ON UPDATE NO ACTION ON DELETE NO ACTION
)
GO
CREATE UNIQUE INDEX [index_OilEngine_1]
	ON [OilEngine] ([OeNo])
GO