IF NOT EXISTS (Select Name from dbo.sysdatabases WHERE name = 'ComputerPartsShop')
BEGIN 
CREATE DATABASE [ComputerPartsShop]
END;
GO

BEGIN TRY
BEGIN TRANSACTION;
USE [ComputerPartsShop];
IF NOT EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_NAME = N'Category')
BEGIN
	CREATE TABLE [Category] (
		[Id] INT IDENTITY(1,1) PRIMARY KEY,
		[Name] NVARCHAR(50) UNIQUE NOT NULL,
		[Description] NVARCHAR(4000) NOT NULL,		
	);
END;
IF NOT EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_NAME = N'Country')
BEGIN
	CREATE TABLE [Country] (
		[Id] INT IDENTITY(1,1) PRIMARY KEY,
		[Alpha2] CHAR(2) UNIQUE NOT NULL,
		[Alpha3] CHAR(3) UNIQUE NOT NULL,
		[Name] VARCHAR(100) UNIQUE NOT NULL		
	);
END;

IF NOT EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_NAME = N'PaymentProvider')
BEGIN
	CREATE TABLE [PaymentProvider] (
		[Id] INT IDENTITY(1,1) PRIMARY KEY,
		[Name] NVARCHAR(100) UNIQUE NOT NULL
	);
END;

IF NOT EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_NAME = N'Address')
BEGIN
	CREATE TABLE [Address] (
		[Id] UNIQUEIDENTIFIER PRIMARY KEY,
		[Street] NVARCHAR(100) NOT NULL,
		[City] NVARCHAR(50) NOT NULL,
		[Region] NVARCHAR(50) NOT NULL,
		[ZipCode] VARCHAR(10) NOT NULL,
		[CountryId] INT NOT NULL
		FOREIGN KEY ([CountryId]) REFERENCES [Country] ([Id])
	);
	CREATE INDEX IX_Address_City ON [Address] ([City]);
	CREATE INDEX IX_Address_ZipCode ON [Address] ([ZipCode]);
	CREATE INDEX IX_Address_CountryId ON [Address] ([CountryId]);
END;

IF NOT EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_NAME = N'ShopUser')
BEGIN
	CREATE TABLE [ShopUser] (
		[Id] UNIQUEIDENTIFIER PRIMARY KEY,
		[FirstName] NVARCHAR(100) NOT NULL,
		[LastName] NVARCHAR(100) NOT NULL,
		[Username] VARCHAR(50) UNIQUE NOT NULL,
		[Email] VARCHAR(50) UNIQUE NOT NULL,
		[PhoneNumber] VARCHAR(20),
		[PasswordHash] VARCHAR(256) NOT NULL,
		[RefreshToken] VARCHAR(256),
		[RefreshTokenExpiresAtUtc] DATETIME2(3)
	);
	CREATE INDEX IX_User_PhoneNumber ON [ShopUser] ([PhoneNumber]);
END;

IF NOT EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_NAME = N'UserPaymentSystem')
BEGIN
	CREATE TABLE [UserPaymentSystem] (
		[Id] UNIQUEIDENTIFIER PRIMARY KEY,
		[UserId] UNIQUEIDENTIFIER NOT NULL,
		[ProviderId] INT NOT NULL,
		[PaymentReference] VARCHAR(50) NOT NULL,
		FOREIGN KEY ([UserId]) REFERENCES [ShopUser] ([Id]),
		FOREIGN KEY ([ProviderId]) REFERENCES [PaymentProvider] ([Id])
	);
	CREATE INDEX IX_UserPaymentSystem_UserId ON [UserPaymentSystem] ([UserId]);
	CREATE INDEX IX_UserPaymentSystem_ProviderId ON [UserPaymentSystem] ([ProviderId]);
END;

IF NOT EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_NAME = N'UserAddress')
BEGIN
	CREATE TABLE [UserAddress] (
		[UserId] UNIQUEIDENTIFIER,
		[AddressId] UNIQUEIDENTIFIER,
		PRIMARY KEY ([UserId], [AddressId]),
		FOREIGN KEY ([UserId]) REFERENCES [ShopUser] ([Id]),
		FOREIGN KEY ([AddressId]) REFERENCES [Address] ([Id])
	);	
END;

IF NOT EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_NAME = N'Product')
BEGIN
	CREATE TABLE [Product] (
		[Id] INT IDENTITY(1,1) PRIMARY KEY,
		[Name] NVARCHAR(100) NOT NULL,
		[Description] NVARCHAR(4000) NOT NULL,
		[UnitPrice] DECIMAL(18,2) NOT NULL,
		[Stock] INT NOT NULL,
		[CategoryId] INT NOT NULL,
		[InternalCode] NVARCHAR(100) NOT NULL
		FOREIGN KEY ([CategoryId]) REFERENCES [Category] ([Id])
	);
	CREATE INDEX IX_Product_Name ON [Product] ([Name]);
	CREATE INDEX IX_Product_CategoryId ON [Product] ([CategoryId]);
	CREATE INDEX IX_Product_InternalCode ON [Product] ([InternalCode]);
END;

IF NOT EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_NAME = N'Order')
BEGIN
	CREATE TABLE [Order] (
		[Id] INT IDENTITY(1,1) PRIMARY KEY,
		[UserId] UNIQUEIDENTIFIER NOT NULL,
		[Total] DECIMAL(18,2) NOT NULL,
		[DeliveryAddressId] UNIQUEIDENTIFIER NOT NULL,
		[Status] nvarchar(255) NOT NULL CHECK ([Status] IN ('Pending', 'Processing', 'Shipped', 'Delivered', 'Returned', 'Cancelled')),
		[OrderedAt] DATETIME2(3) NOT NULL,
		[SendAt] DATETIME2(3)
		FOREIGN KEY ([UserId]) REFERENCES [ShopUser] ([Id]),
		FOREIGN KEY ([DeliveryAddressId]) REFERENCES [Address] ([Id])
	);
	CREATE INDEX IX_Order_UserId ON [Order] ([UserId])
	CREATE INDEX IX_Order_DeliveryAddressId ON [Order] ([DeliveryAddressId])
	CREATE INDEX IX_Order_Status ON [Order] ([Status])
	CREATE INDEX IX_Order_Status_OrderedAt ON [Order] ([Status], [OrderedAt])
	CREATE INDEX IX_Order_SendAt ON [Order] ([SendAt])
END;

IF NOT EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_NAME = N'OrderProduct')
BEGIN
	CREATE TABLE [OrderProduct] (
		[OrderId] INT,
		[ProductId] INT,
		[Quantity] INT NOT NULL,
		PRIMARY KEY ([OrderId], [ProductId]),
		FOREIGN KEY ([OrderId]) REFERENCES [Order] ([Id]) ON DELETE CASCADE,
		FOREIGN KEY ([ProductId]) REFERENCES [Product] ([Id]) ON DELETE CASCADE
	);	
END;

IF NOT EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_NAME = N'Payment')
BEGIN
	CREATE TABLE [Payment] (
		[Id] UNIQUEIDENTIFIER PRIMARY KEY,
		[UserPaymentSystemId] UNIQUEIDENTIFIER NOT NULL,
		[OrderId] INT NOT NULL,
		[Total] DECIMAL(18,2) NOT NULL,
		[Method] nvarchar(255) NOT NULL CHECK ([Method] IN ('Cash', 'CreditCard', 'BLIK', 'BankTransfer', 'PayPal')),
		[Status] nvarchar(255) NOT NULL CHECK ([Status] IN ('Pending', 'Authorized', 'Completed', 'Failed', 'Cancelled', 'Refunded')),
		[PaymentStartAt] DATETIME2(3) NOT NULL,
		[PaidAt] DATETIME2(3),
		FOREIGN KEY ([OrderId]) REFERENCES [Order] ([Id]),
		FOREIGN KEY ([UserPaymentSystemId]) REFERENCES [UserPaymentSystem] ([Id])
	);
	CREATE INDEX IX_Payment_OrderId ON [Payment] ([OrderId])
	CREATE INDEX IX_Payment_UserPaymentSystemId ON [Payment] ([UserPaymentSystemId])
	CREATE INDEX IX_Payment_Status ON [Payment] ([Status])
	CREATE INDEX IX_Payment_PaymentStartAt ON [Payment] ([PaymentStartAt])
	CREATE INDEX IX_Payment_PaidAt ON [Payment] ([PaidAt])
END;

IF NOT EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_NAME = N'Review')
BEGIN
	CREATE TABLE [Review] (
		[Id] INT IDENTITY(1,1) PRIMARY KEY,
		[UserId] UNIQUEIDENTIFIER,
		[ProductId] INT NOT NULL,
		[Rating] TINYINT NOT NULL,
		[Description] NVARCHAR(4000),
		FOREIGN KEY ([ProductId]) REFERENCES [Product] ([Id]),
		FOREIGN KEY ([UserId]) REFERENCES [ShopUser] ([Id])		
	);
	CREATE INDEX IX_Review_UserId ON [Review] ([UserId])
	CREATE INDEX IX_Review_ProductId ON [Review] ([ProductId])
END;

COMMIT;
END TRY
BEGIN CATCH
	ROLLBACK;
	THROW;
END CATCH