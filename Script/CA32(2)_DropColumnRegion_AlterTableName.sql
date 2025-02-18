ALTER TABLE Accounts
DROP COLUMN Region;

EXEC sp_rename Services, MedicalServices;