--CREATE DATABASE [dev-careblock-v2];
--USE [dev-careblock-v2];

--drop database [dev-careblock-v2];

CREATE TABLE [Organizations] (
  [Id] uniqueidentifier PRIMARY KEY,
  [Code] nvarchar(255),
  [Name] nvarchar(255),
  [City] nvarchar(255),
  [District] nvarchar(255),
  [Address] nvarchar(255),
  [MapUrl] nvarchar(max),
  [Thumbnail] nvarchar(max)
)
GO

CREATE TABLE [Departments] (
  [Id] uniqueidentifier PRIMARY KEY,
  [OrganizationId] uniqueidentifier,
  [Name] nvarchar(255),
  [Location] nvarchar(255),
)
GO

CREATE TABLE [Accounts] (
  [Id] uniqueidentifier PRIMARY KEY,
  [DepartmentId] uniqueidentifier,
  [StakeId] nvarchar(max),
  [Firstname] nvarchar(100),
  [Lastname] nvarchar(100),
  [DateOfBirth] date,
  [Gender] tinyint,
  [Avatar] nvarchar(max),
  [IdentityId] nvarchar(255),
  [Phone] nvarchar(255),
  [Email] nvarchar(255),
  [Description] nvarchar(max),
  [Seniority] tinyint,
  [CreatedDate] datetime,
  [ModifiedDate] datetime,
  [IsDisable] bit,
)
GO

CREATE TABLE [RefreshTokens] (
  [Id] uniqueidentifier PRIMARY KEY,
  [AccountId] uniqueidentifier,
  [Token] nvarchar(max),
  [Expires] date,
  [CreatedByIp] nvarchar(100),
  [Created] datetime,
  [Revoked] datetime,
  [RevokedByIp] datetime,
  [ReasonRevoked] nvarchar(100),
)
GO

CREATE TABLE [Roles] (
  [Id] int PRIMARY KEY,
  [Name] nvarchar(255)
)
GO

CREATE TABLE [AccountRoles] (
  [Id] uniqueidentifier PRIMARY KEY,
  [AccountId] uniqueidentifier NOT NULL,
  [RoleId] int NOT NULL,
)
GO

CREATE TABLE [Permissions] (
  [Id] int PRIMARY KEY,
  [Name] nvarchar(255)
)
GO

CREATE TABLE [PermissionRoles] (
  [Id] uniqueidentifier PRIMARY KEY,
  [PermissionId] int NOT NULL,
  [RoleId] int NOT NULL,
)
GO

CREATE TABLE [Notifications] (
  [Id] uniqueidentifier PRIMARY KEY,
  [AccountId] uniqueidentifier NOT NULL,
  [NotificationTypeId] int,
  [Message] nvarchar(max),
  [Link] nvarchar(max),
  [IsRead] bit,
  [CreatedDate] datetime,
)
GO

CREATE TABLE [NotificationTypes] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [Name] nvarchar(max)
)
GO

CREATE TABLE [Specialists] (
  [Id] uniqueidentifier PRIMARY KEY,
  [OrganizationId] uniqueidentifier,
  [Name] nvarchar(255),
  [Thumbnail] nvarchar(255),
  [Description] nvarchar(max),
  [IsHidden] bit,
)
GO

CREATE TABLE [DoctorSpecialists] (
  [Id] uniqueidentifier PRIMARY KEY,
  [AccountId] uniqueidentifier NOT NULL,
  [SpecialistId] uniqueidentifier NOT NULL,
  [StartDate] date,
)
GO

CREATE TABLE [ExaminationTypes] (
  [Id] int PRIMARY KEY,
  [Name] nvarchar(255)
)
GO

CREATE TABLE [TimeSlots] (
  [Id] uniqueidentifier PRIMARY KEY,
  [ExaminationPackageId] uniqueidentifier NOT NULL,
  [StartTime] time,
  [EndTime] time,
  [Period] int,
  [CreatedDate] datetime,
  [ModifiedDate] datetime,
)
GO

CREATE TABLE [ExaminationPackages] (
  [Id] uniqueidentifier PRIMARY KEY,
  [OrganizationId] uniqueidentifier,
  [ExaminationTypeId] int NOT NULL,
  [Name] nvarchar(255),
  [Thumbnail] nvarchar(max),
  [CreatedDate] datetime,
  [ModifiedDate] datetime,
  [IsDeleted] bit,
)
GO

CREATE TABLE [ExaminationPackageSpecialists] (
  [Id] int PRIMARY KEY,
  [SpecialistId] uniqueidentifier NOT NULL,
  [ExaminationPackageId] uniqueidentifier NOT NULL,
  PRIMARY KEY ([SpecialistId], [ExaminationPackageId])
)
GO

CREATE TABLE [ExaminationOptions] (
  [Id] uniqueidentifier PRIMARY KEY,
  [SpecialistId] uniqueidentifier NOT NULL,
  [Name] nvarchar(255),
  [Description] nvarchar(max),
  [Price] float,
  [TimeEstimation] int,
  [ExaminationForm] nvarchar(max),
)
GO

CREATE TABLE [ExaminationPackageOptions] (
  [Id] int PRIMARY KEY,
  [ExaminationOptionId] uniqueidentifier,
  [ExaminationPackageId] uniqueidentifier,
  [Priority] tinyint,
  [Sequency] int,
  PRIMARY KEY ([ExaminationOptionId], [ExaminationPackageId])
)
GO

CREATE TABLE [Appointments] (
  [Id] uniqueidentifier PRIMARY KEY,
  [PatientId] uniqueidentifier NOT NULL,
  [DoctorId] uniqueidentifier,
  [Status] tinyint,
  [Name] nvarchar(255),
  [Gender] tinyint,
  [Phone] nvarchar(100),
  [Email] nvarchar(100),
  [Address] nvarchar(255),
  [Symptom] nvarchar(255),
  [Note] nvarchar(max),
  [Reason] nvarchar(max),
  [StartDateExpectation] datetime,
  [EndDateExpectation] datetime,
  [StartDateReality] datetime,
  [EndDateReality] datetime,
  [CreatedDate] datetime,
  [ModifiedDate] datetime,
)
GO

CREATE TABLE [AppointmentDetails] (
  [Id] uniqueidentifier PRIMARY KEY,
  [ExaminationOptionId] uniqueidentifier NOT NULL,
  [AppointmentId] uniqueidentifier NOT NULL,
  [DoctorId] uniqueidentifier NOT NULL,
  [Diagnostic] nvarchar(max),
  [Price] float,
)
GO

CREATE TABLE [AppointmentResponses] (
  [Id] uniqueidentifier PRIMARY KEY,
  [AppointmentId] uniqueidentifier NOT NULL,
  [FileUrl] nvarchar(max),
  [Message] nvarchar(max),
)
GO

CREATE TABLE [Results] (
  [Id] uniqueidentifier PRIMARY KEY,
  [AppointmentId] uniqueidentifier NOT NULL,
  [DiagnosticUrl] nvarchar(max),
  [CreatedDate] datetime,
  [ModifiedDate] datetime,
)
GO

CREATE TABLE [MedicineTypes] (
  [Id] int PRIMARY KEY,
  [Name] nvarchar(255)
)
GO

CREATE TABLE [Medicines] (
  [Id] uniqueidentifier PRIMARY KEY,
  [MedicineTypeId] int NOT NULL,
  [Name] nvarchar(255),
  [Price] float,
  [UnitPrice] tinyint,
  [Description] nvarchar(max),
  [Thumbnail] nvarchar(max),
  [CreatedDate] datetime,
  [ModifiedDate] datetime,
  [IsDeleted] bit,
)
GO

CREATE TABLE [MedicineResults] (
  [Id] uniqueidentifier PRIMARY KEY,
  [MedicineId] uniqueidentifier NOT NULL,
  [ResultId] uniqueidentifier NOT NULL,
  [Quantity] int,
  [Price] float,
)
GO

CREATE TABLE [PaymentMethods] (
  [Id] int PRIMARY KEY,
  [Name] nvarchar(255)
)
GO

CREATE TABLE [Payments] (
  [Id] uniqueidentifier PRIMARY KEY,
  [AppointmentId] uniqueidentifier NOT NULL,
  [PaymentMethodId] int NOT NULL,
  [Status] nvarchar(100),
  [Total] float,
  [CreatedDate] datetime,
  [ModifiedDate] datetime,
)
GO

ALTER TABLE [Departments] ADD FOREIGN KEY ([OrganizationId]) REFERENCES [Organizations] ([Id])
GO

ALTER TABLE [Accounts] ADD FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([Id])
GO

ALTER TABLE [RefreshTokens] ADD FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id])
GO

ALTER TABLE [AccountRoles] ADD FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id])
GO

ALTER TABLE [AccountRoles] ADD FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id])
GO

ALTER TABLE [PermissionRoles] ADD FOREIGN KEY ([PermissionId]) REFERENCES [Permissions] ([Id])
GO

ALTER TABLE [PermissionRoles] ADD FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id])
GO

ALTER TABLE [Notifications] ADD FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id])
GO

ALTER TABLE [Notifications] ADD FOREIGN KEY ([NotificationTypeId]) REFERENCES [NotificationTypes] ([Id])
GO

ALTER TABLE [Specialists] ADD FOREIGN KEY ([OrganizationId]) REFERENCES [Organizations] ([Id])
GO

ALTER TABLE [DoctorSpecialists] ADD FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id])
GO

ALTER TABLE [DoctorSpecialists] ADD FOREIGN KEY ([SpecialistId]) REFERENCES [Specialists] ([Id])
GO

ALTER TABLE [TimeSlots] ADD FOREIGN KEY ([ExaminationPackageId]) REFERENCES [ExaminationPackages] ([Id])
GO

ALTER TABLE [ExaminationPackages] ADD FOREIGN KEY ([OrganizationId]) REFERENCES [Organizations] ([Id])
GO

ALTER TABLE [ExaminationPackages] ADD FOREIGN KEY ([ExaminationTypeId]) REFERENCES [ExaminationTypes] ([Id])
GO

ALTER TABLE [ExaminationPackageSpecialists] ADD FOREIGN KEY ([SpecialistId]) REFERENCES [Specialists] ([Id])
GO

ALTER TABLE [ExaminationPackageSpecialists] ADD FOREIGN KEY ([ExaminationPackageId]) REFERENCES [ExaminationPackages] ([Id])
GO

ALTER TABLE [ExaminationOptions] ADD FOREIGN KEY ([SpecialistId]) REFERENCES [Specialists] ([Id])
GO

ALTER TABLE [ExaminationPackageOptions] ADD FOREIGN KEY ([ExaminationOptionId]) REFERENCES [ExaminationOptions] ([Id])
GO

ALTER TABLE [ExaminationPackageOptions] ADD FOREIGN KEY ([ExaminationPackageId]) REFERENCES [ExaminationPackages] ([Id])
GO

ALTER TABLE [Appointments] ADD FOREIGN KEY ([ParientId]) REFERENCES [Accounts] ([Id])
GO

ALTER TABLE [Appointments] ADD FOREIGN KEY ([DoctorId]) REFERENCES [Accounts] ([Id])
GO

ALTER TABLE [AppointmentDetails] ADD FOREIGN KEY ([ExaminationOptionId]) REFERENCES [ExaminationOptions] ([Id])
GO

ALTER TABLE [AppointmentDetails] ADD FOREIGN KEY ([AppointmentId]) REFERENCES [Appointments] ([Id])
GO

ALTER TABLE [AppointmentDetails] ADD FOREIGN KEY ([DoctorId]) REFERENCES [Accounts] ([Id])
GO

ALTER TABLE [AppointmentResponses] ADD FOREIGN KEY ([AppointmentId]) REFERENCES [Appointments] ([Id])
GO

ALTER TABLE [Results] ADD FOREIGN KEY ([AppointmentId]) REFERENCES [Appointments] ([Id])
GO

ALTER TABLE [Medicines] ADD FOREIGN KEY ([MedicineTypeId]) REFERENCES [MedicineTypes] ([Id])
GO

ALTER TABLE [MedicineResults] ADD FOREIGN KEY ([MedicineId]) REFERENCES [Medicines] ([Id])
GO

ALTER TABLE [MedicineResults] ADD FOREIGN KEY ([ResultId]) REFERENCES [Results] ([Id])
GO

ALTER TABLE [Payments] ADD FOREIGN KEY ([AppointmentId]) REFERENCES [Appointments] ([Id])
GO

ALTER TABLE [Payments] ADD FOREIGN KEY ([PaymentMethodId]) REFERENCES [PaymentMethods] ([Id])
GO
