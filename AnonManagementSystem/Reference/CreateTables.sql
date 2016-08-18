CREATE TABLE [CombatEquipment] (
	[SerialNo] nvarchar(254) NOT NULL PRIMARY KEY, 
	[Name] nvarchar(254) NOT NULL, 
	[MajorCategory] nvarchar(254), 
	[Model] nvarchar(254), 
	[SubDepartment] text, 
	[TechCondition] nvarchar(254), 
	[UseCondition] nvarchar(254), 
	[Factory] nvarchar(254), 
	[OemNo] nvarchar(254), 
	[ProductionDate] date NOT NULL, 
	[InventorySpot] text, 
	[MajorComp] text, 
	[MainUsage] text, 
	[PerformIndex] text, 
	[UseMethod] text, 
	[Manager] nvarchar(254), 
	[Technician] nvarchar(254), 
	[TechRemould] text, 
	[SetupVideo] text
)
GO
CREATE UNIQUE INDEX [index_CombatEquipment_1]
	ON [CombatEquipment] ([SerialNo])
GO

CREATE TABLE [CombatVehicles] (
	[SerialNo] nvarchar(254) NOT NULL PRIMARY KEY, 
	[Name] text NOT NULL, 
	[Model] nvarchar(254), 
	[VehiclesNo] nvarchar(254), 
	[MotorModel] nvarchar(254), 
	[TechCondition] nvarchar(254), 
	[Factory] nvarchar(254), 
	[ProductionDate] date NOT NULL, 
	[Mass] nvarchar(254), 
	[Tankage] nvarchar(254), 
	[OverallSize] nvarchar(254), 
	[FuelType] nvarchar(254), 
	[DrivingModel] nvarchar(254), 
	[Mileage] nvarchar(254), 
	[Output] nvarchar(254), 
	[LicenseCarry] nvarchar(254), 
	[VehicleChargers] nvarchar(254), 
	[VehicleSpotNo] nvarchar(254), 
	[VehicleDescri] nvarchar(254), 
	[CombineOe] bool NOT NULL DEFAULT false, 
	[Equipment] nvarchar(254), 
	FOREIGN KEY ([Equipment])
		REFERENCES [CombatEquipment] ([SerialNo])
		ON UPDATE NO ACTION ON DELETE NO ACTION
)
GO
CREATE UNIQUE INDEX [index_CombatVehicles_1]
	ON [CombatVehicles] ([SerialNo])
GO

CREATE TABLE [Events] (
	[No] nvarchar(254) NOT NULL PRIMARY KEY, 
	[Name] text, 
	[StartTime] date NOT NULL, 
	[EndTime] date NOT NULL, 
	[Address] text, 
	[EventType] nvarchar(254), 
	[SpecificType] nvarchar(254), 
	[Code] nvarchar(254), 
	[NoInEvents] nvarchar(254), 
	[HigherUnit] nvarchar(254), 
	[Executor] nvarchar(254), 
	[PublishUnit] nvarchar(254), 
	[PublishDate] date NOT NULL, 
	[Publisher] nvarchar(254), 
	[According] text, 
	[PeopleDescri] nvarchar(254), 
	[ProcessDescri] text, 
	[HandleStep] text, 
	[Problem] text, 
	[Remarks] text, 
	[Equipment] nvarchar(254), 
	FOREIGN KEY ([Equipment])
		REFERENCES [CombatEquipment] ([SerialNo])
		ON UPDATE NO ACTION ON DELETE NO ACTION
)
GO
CREATE UNIQUE INDEX [index_Events_1]
	ON [Events] ([No])
GO

CREATE TABLE [EventData] (
	[ID] varchar(254) NOT NULL PRIMARY KEY, 
	[Name] nvarchar(254), 
	[Spot] nvarchar(254), 
	[EventsNo] nvarchar(254), 
	FOREIGN KEY ([EventsNo])
		REFERENCES [Events] ([No])
		ON UPDATE NO ACTION ON DELETE NO ACTION
)
GO

CREATE TABLE [Material] (
	[No] nvarchar(254) NOT NULL PRIMARY KEY, 
	[Name] nvarchar(254), 
	[Type] nvarchar(254), 
	[PaperSize] nvarchar(254), 
	[Pagination] nvarchar(254), 
	[Edition] nvarchar(254), 
	[Volume] nvarchar(254), 
	[Date] date NOT NULL, 
	[DocumentLink] nvarchar(254), 
	[StoreSpot] nvarchar(254), 
	[Content] nvarchar(254), 
	[Equipment] nvarchar(254), 
	FOREIGN KEY ([Equipment])
		REFERENCES [CombatEquipment] ([SerialNo])
		ON UPDATE NO ACTION ON DELETE NO ACTION
)
GO

CREATE TABLE [OilEngine] (
	[OeNo] nvarchar(254) NOT NULL PRIMARY KEY, 
	[OeModel] nvarchar(254), 
	[OutPower] nvarchar(254), 
	[TechCondition] nvarchar(254), 
	[WorkHour] nvarchar(254), 
	[OeFactory] nvarchar(254), 
	[OeDate] date NOT NULL, 
	[OeOemNo] nvarchar(254), 
	[MotorModel] nvarchar(254) NOT NULL, 
	[MotorPower] nvarchar(254), 
	[MotorFuel] nvarchar(254), 
	[MotorTankage] nvarchar(254), 
	[MotorFactory] nvarchar(254), 
	[MotorDate] date NOT NULL, 
	[MotorOemNo] nvarchar(254), 
	[FaultDescri] nvarchar(254), 
	[Vehicle] nvarchar(254), 
	FOREIGN KEY ([Vehicle])
		REFERENCES [CombatVehicles] ([SerialNo])
		ON UPDATE NO ACTION ON DELETE NO ACTION
)
GO
CREATE UNIQUE INDEX [index_OilEngine_1]
	ON [OilEngine] ([OeNo])
GO