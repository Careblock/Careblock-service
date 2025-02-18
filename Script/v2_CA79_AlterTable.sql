drop table ExaminationPackageSpecialist;
drop table ExaminationPackageOption;
drop table ExaminationType;
drop table ExaminationPackageOption;

CREATE TABLE [ExaminationPackageSpecialist] (
  [Id] int PRIMARY KEY,
  [SpecialistId] uniqueidentifier NOT NULL,
  [ExaminationPackageId] uniqueidentifier NOT NULL
)
GO

CREATE TABLE [ExaminationPackageOption] (
  [Id] int PRIMARY KEY,
  [ExaminationOptionId] uniqueidentifier,
  [ExaminationPackageId] uniqueidentifier,
  [Priority] tinyint,
  [Sequency] int,
)
GO

Alter Table ExaminationPackage
Drop Constraint FK__Examinati__Exami__1BC821DD;

CREATE TABLE [ExaminationType] (
  [Id] int IDENTITY(1,1) PRIMARY KEY,
  [Name] nvarchar(255),
  Thumbnail varchar(max),
)
GO

ALTER TABLE [ExaminationPackage] ADD FOREIGN KEY ([ExaminationTypeId]) REFERENCES [ExaminationType] ([Id])
GO

CREATE TABLE [ExaminationPackageOption] (
  [Id] uniqueidentifier PRIMARY KEY,
  [ExaminationOptionId] uniqueidentifier,
  [ExaminationPackageId] uniqueidentifier,
  [Priority] tinyint,
  [Sequency] int,
)
GO

Alter Table [TimeSlot]
Alter Column StartTime time;

Alter Table [TimeSlot]
Alter Column EndTime time;

ALter Table [Appointment]
Add OrganizationId uniqueidentifier,
Constraint FK_Appointments_Organizations
Foreign Key (OrganizationId)
References Organization(Id);

ALter Table [Appointment]
Add ExaminationPackageId uniqueidentifier,
Constraint FK_Appointments_ExaminationPackages
Foreign Key (ExaminationPackageId)
References ExaminationPackage(Id);