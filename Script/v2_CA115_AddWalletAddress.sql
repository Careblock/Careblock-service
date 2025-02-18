alter table Accounts
add WalletAddress nvarchar(255);

alter table Accounts
add AssetToken nvarchar(255);

alter table PermissionRoles
add AccountId uniqueidentifier not null;
ALTER TABLE PermissionRoles ADD FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id]);