create table ExaminationTypeOrganization (
    Id uniqueidentifier PRIMARY KEY,
	ExaminationTypeId int not null,
	OrganizationId uniqueidentifier not null,
)
go
ALTER TABLE ExaminationTypeOrganization ADD FOREIGN KEY ([OrganizationId]) REFERENCES [Organizations] ([Id])
go
ALTER TABLE ExaminationTypeOrganization ADD FOREIGN KEY ([ExaminationTypeId]) REFERENCES [ExaminationTypes] ([Id])

create table MedicineTypeOrganization (
    Id uniqueidentifier PRIMARY KEY,
	MedicineTypeId int not null,
	OrganizationId uniqueidentifier not null,
)
go
ALTER TABLE MedicineTypeOrganization ADD FOREIGN KEY ([OrganizationId]) REFERENCES [Organizations] ([Id])
go
ALTER TABLE MedicineTypeOrganization ADD FOREIGN KEY ([MedicineTypeId]) REFERENCES [MedicineTypes] ([Id])
go
ALTER TABLE Appointments ADD IsDeleted bit;

Alter table specialists
add CreatedDate datetime;
Alter table specialists
add ModifiedDate datetime;

Alter table departments
add CreatedDate datetime;
Alter table departments
add ModifiedDate datetime;

Alter table examinationTypes
add CreatedDate datetime;
Alter table examinationTypes
add ModifiedDate datetime;

Alter table medicineTypes
add CreatedDate datetime;
Alter table medicineTypes
add ModifiedDate datetime;

Alter table results
add [Message] nvarchar(max);