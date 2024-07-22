--create database dev-careblock
--use dev-careblock;

-- Tổ chức/Bệnh viện
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Organizations')
BEGIN
    CREATE TABLE Organizations (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        Code VARCHAR(MAX) not null, -- mã tổ chức
        [Name] NVARCHAR(MAX) not null,
        [Location] NVARCHAR(MAX) null,
        Avatar VARCHAR(MAX) null,
		[Description] TEXT null,
		CreatedBy VARCHAR(MAX) null,
		ModifiedBy VARCHAR(MAX) null,
		CreatedDate DateTime null,
		ModifiedDate DateTime null
    );
END


-- Tài khoản (nhiều role)
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Accounts')
BEGIN
    CREATE TABLE Accounts (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        OrganizationId UNIQUEIDENTIFIER null,
        StakeId VARCHAR(MAX) not null,
        IdentityId VARCHAR(MAX) null, -- CCCD,...
        Firstname NVARCHAR(MAX) not null,
        Lastname NVARCHAR(MAX) not null,
        Age TINYINT null,
        Phone VARCHAR(MAX) null,
        Race VARCHAR(MAX) null,
        Avatar VARCHAR(MAX) null,
        Gender TINYINT not null, -- {1: Male, 2: Female, 3: Other}
        BloodType TINYINT null, -- {1: A, 2: B, 3: AB, 4: O}
        [Role] TINYINT not null, -- {1: Patient, 2: Doctor, 3: Doctor Manager, 4: Admin}
		Seniority TINYINT null, -- thâm niên

		CreatedBy VARCHAR(MAX) null,
		ModifiedBy VARCHAR(MAX) null,
		CreatedDate DateTime null,
		ModifiedDate DateTime null,
		IsDeleted BIT,
        FOREIGN KEY (OrganizationId) REFERENCES Organizations(Id)
    );
END

--Refresh Token
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'RefreshTokens')
CREATE TABLE RefreshTokens(
	Id UNIQUEIDENTIFIER PRIMARY KEY,
	AccountId UNIQUEIDENTIFIER NOT NULL,
	Token VARCHAR(max) NOT NULL,
	Expires DATETIME NOT NULL,
	Created DATETIME NOT NULL,
	CreatedByIp VARCHAR(max) NOT NULL,
	Revoked DATETIME NULL,
	RevokedByIp VARCHAR(max),
	ReasonRevoked VARCHAR(max),
	FOREIGN KEY (AccountId) REFERENCES Accounts(Id)
)

-- Thuốc
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Medicines')
BEGIN
    CREATE TABLE Medicines (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        OrganizationId UNIQUEIDENTIFIER not null,
        [Name] NVARCHAR(MAX) not null,
        Price MONEY not null,
        Note TEXT null,
        Avatar VARCHAR(MAX) null,
		IsDeleted BIT,
		CreatedBy VARCHAR(MAX) null,
		ModifiedBy VARCHAR(MAX) null,
		CreatedDate DateTime null,
		ModifiedDate DateTime null,
        FOREIGN KEY (OrganizationId) REFERENCES Organizations(Id)
    );
END


-- Dịch vụ
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Services')
BEGIN
    CREATE TABLE Services (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        OrganizationId UNIQUEIDENTIFIER not null,
        [Name] NVARCHAR(MAX) not null,
        Price MONEY not null,
        Avatar VARCHAR(MAX) null,
        Note TEXT null,
		IsDeleted BIT,
        FOREIGN KEY (OrganizationId) REFERENCES Organizations(Id)
    );
END


-- Lịch hẹn
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Appointments')
BEGIN
    CREATE TABLE Appointments ( 
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        DoctorId UNIQUEIDENTIFIER not null,
        PatientId UNIQUEIDENTIFIER not null,
        [Status] TINYINT not null, -- {1: Pending, 2: Accept, 3: Reject, 4: Done}
        Reason NVARCHAR(MAX) null,
		StartTime Datetime, -- thời gian bắt đầu khám (tạo range ở fe)
		EndTime Datetime, -- thời gian kết thúc khám
        Note TEXT null,
		CreatedDate DateTime null,
		ModifiedDate DateTime null,
        FOREIGN KEY (DoctorId) REFERENCES Accounts(Id),
        FOREIGN KEY (PatientId) REFERENCES Accounts(Id)
    );
END


-- Chuẩn đoán
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Diagnostics')
BEGIN
    CREATE TABLE Diagnostics (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        PatientId UNIQUEIDENTIFIER not null,
        DoctorId UNIQUEIDENTIFIER not null,
        Disease NVARCHAR(MAX),
        [Time] DATETIME null,
        [Weight] FLOAT null,
        Height FLOAT null,
		HeartRate FLOAT null,
		BodyTemperature FLOAT null,
		DiastolicBloodPressure FLOAT null, -- Huyết áp tâm trương
		SystolicBloodPressure FLOAT null, -- Huyết áp tâm thu
        Note TEXT null,
        [Status] TINYINT not null, -- {1: Healthy/Khoẻ mạnh, 2: Weak/Yếu, 3: Critical/Nguy cấp, 4: Normal/Bình thường, 5: Pathological/Bệnh lý}
        FOREIGN KEY (PatientId) REFERENCES Accounts(Id),
        FOREIGN KEY (DoctorId) REFERENCES Accounts(Id)
    );
END


-- Hoá đơn
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Invoices')
BEGIN
    CREATE TABLE Invoices (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        AppointmentId UNIQUEIDENTIFIER not null,
        Price MONEY null,
		CreatedDate DateTime null,
		ModifiedDate DateTime null,
        FOREIGN KEY (AppointmentId) REFERENCES Appointments(Id)
    );
END


-- Chi tiết hoá đơn/Bảng n-n hoá đơn và dịch vụ/thuốc
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'InvoiceDetails')
BEGIN
    CREATE TABLE InvoiceDetails (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        ServiceId UNIQUEIDENTIFIER null,
        MedicineId UNIQUEIDENTIFIER null,
        InvoiceId UNIQUEIDENTIFIER not null,
        Quantity INT not null,
        FOREIGN KEY (ServiceId) REFERENCES Services(Id),
        FOREIGN KEY (MedicineId) REFERENCES Medicines(Id),
        FOREIGN KEY (InvoiceId) REFERENCES Invoices(Id)
    );
END