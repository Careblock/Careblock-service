EXEC sp_rename 'Accounts.Race' , 'Region', 'COLUMN'

ALTER TABLE Accounts
ADD DateOfBirth Date;