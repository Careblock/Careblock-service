alter table ExaminationTypeOrganizations
alter column ExaminationTypeId int not null;

alter table ExaminationTypeOrganizations
alter column OrganizationId uniqueidentifier not null;

alter table MedicineTypeOrganizations
alter column MedicineTypeId int not null;

alter table MedicineTypeOrganizations
alter column OrganizationId uniqueidentifier not null;