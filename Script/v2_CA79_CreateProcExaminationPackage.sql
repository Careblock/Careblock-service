select 
	ep.Id, 
	ep.OrganizationId, 
	ep.ExaminationTypeId, 
	ep.Name, 
	ep.Thumbnail, 
	org.Name as OrganizationName, 
	org.Address as OrganizationLocation,
	(select SUM(Price) from ExaminationOption as eo 
	join ExaminationPackageOption as epo on eo.Id = epo.ExaminationOptionId
	join ExaminationPackage as ep1 on ep1.Id = epo.ExaminationPackageId
	where ep1.Id = ep.Id) as Price
from ExaminationPackage as ep 
join Organization as org on ep.OrganizationId = org.Id
where ep.ExaminationTypeId = 3;