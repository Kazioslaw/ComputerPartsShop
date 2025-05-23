IF NOT EXISTS (Select Name from dbo.sysdatabases WHERE name = 'ComputerPartsShop')
BEGIN 
CREATE DATABASE [ComputerPartsShop]
END;
GO

BEGIN TRANSACTION;
USE [ComputerPartsShop]

	CREATE TABLE [Category] (
		[ID] INT PRIMARY KEY,
		[Name] NVARCHAR(50) UNIQUE NOT NULL,
		[Description] NVARCHAR(4000) NOT NULL,		
	);

	CREATE TABLE [Country] (
		[ID] INT PRIMARY KEY,
		[Alpha2] CHAR(2) UNIQUE NOT NULL,
		[Alpha3] CHAR(3) UNIQUE NOT NULL,
		[Name] VARCHAR(100) UNIQUE NOT NULL		
	);

	CREATE TABLE [PaymentProvider] (
		[ID] INT PRIMARY KEY,
		[Name] NVARCHAR(100) UNIQUE NOT NULL
	);

	CREATE TABLE [Address] (
		[ID] UNIQUEIDENTIFIER PRIMARY KEY,
		[Street] NVARCHAR(100) NOT NULL,
		[City] NVARCHAR(50) NOT NULL,
		[Region] NVARCHAR(50) NOT NULL,
		[ZipCode] VARCHAR(10) NOT NULL,
		[CountryID] INT NOT NULL
		FOREIGN KEY ([CountryID]) REFERENCES [Country] ([ID])
	);
	CREATE INDEX IX_Address_City ON [Address] ([City]);
	CREATE INDEX IX_Address_ZipCode ON [Address] ([ZipCode]);
	CREATE INDEX IX_Address_CountryID ON [Address] ([CountryID]);

	CREATE TABLE [Customer] (
		[ID] UNIQUEIDENTIFIER PRIMARY KEY,
		[FirstName] NVARCHAR(100) NOT NULL,
		[LastName] NVARCHAR(100) NOT NULL,
		[Username] VARCHAR(50) UNIQUE NOT NULL,
		[Email] VARCHAR(50) UNIQUE NOT NULL,
		[PhoneNumber] VARCHAR(20)
	);
	CREATE INDEX IX_Customer_PhoneNumber ON [Customer] ([PhoneNumber]);

	CREATE TABLE [CustomerPaymentSystem] (
		[ID] UNIQUEIDENTIFIER PRIMARY KEY,
		[CustomerID] UNIQUEIDENTIFIER NOT NULL,
		[ProviderID] INT NOT NULL,
		[PaymentReference] VARCHAR(50) NOT NULL,
		FOREIGN KEY ([CustomerID]) REFERENCES [Customer] ([ID]),
		FOREIGN KEY ([ProviderID]) REFERENCES [PaymentProvider] ([ID])
	);
	CREATE INDEX IX_CustomerPaymentSystem_CustomerID ON [CustomerPaymentSystem] ([CustomerID]);
	CREATE INDEX IX_CustomerPaymentSystem_ProviderID ON [CustomerPaymentSystem] ([ProviderID]);


	CREATE TABLE [CustomerAddress] (
		[CustomerID] UNIQUEIDENTIFIER,
		[AddressID] UNIQUEIDENTIFIER,
		PRIMARY KEY ([CustomerID], [AddressID]),
		FOREIGN KEY ([CustomerID]) REFERENCES [Customer] ([ID]),
		FOREIGN KEY ([AddressID]) REFERENCES [Address] ([ID])
	);	
	
	CREATE TABLE [Product] (
		[ID] INT PRIMARY KEY,
		[Name] NVARCHAR(100) NOT NULL,
		[Description] NVARCHAR(4000) NOT NULL,
		[UnitPrice] DECIMAL(18,2) NOT NULL,
		[Stock] INT NOT NULL,
		[CategoryID] INT NOT NULL,
		[InternalCode] NVARCHAR(100) NOT NULL
		FOREIGN KEY ([CategoryID]) REFERENCES [Category] ([ID])
	);
	CREATE INDEX IX_Product_Name ON [Product] ([Name]);
	CREATE INDEX IX_Product_CategoryID ON [Product] ([CategoryID]);
	CREATE INDEX IX_Product_InternalCode ON [Product] ([InternalCode]);


	CREATE TABLE [Order] (
		[ID] INT PRIMARY KEY,
		[CustomerID] UNIQUEIDENTIFIER NOT NULL,
		[Total] DECIMAL(18,2) NOT NULL,
		[DeliveryAddressID] UNIQUEIDENTIFIER NOT NULL,
		[Status] nvarchar(255) NOT NULL CHECK ([Status] IN ('Pending', 'Processing', 'Shipped', 'Delivered', 'Returned', 'Cancelled')),
		[OrderedAt] DATETIME2(3) NOT NULL,
		[SendAt] DATETIME2(3)
		FOREIGN KEY ([CustomerID]) REFERENCES [Customer] ([ID]),
		FOREIGN KEY ([DeliveryAddressID]) REFERENCES [Address] ([ID])
	);
	CREATE INDEX IX_Order_CustomerID ON [Order] ([CustomerID])
	CREATE INDEX IX_Order_DeliveryAddressID ON [Order] ([DeliveryAddressID])
	CREATE INDEX IX_Order_Status ON [Order] ([Status])
	CREATE INDEX IX_Order_Status_OrderedAt ON [Order] ([Status], [OrderedAt])
	CREATE INDEX IX_Order_SendAt ON [Order] ([SendAt])

	CREATE TABLE [OrderProduct] (
		[OrderID] INT,
		[ProductID] INT,
		[Quantity] INT NOT NULL,
		PRIMARY KEY ([OrderID], [ProductID]),
		FOREIGN KEY ([OrderID]) REFERENCES [Order] ([ID]),
		FOREIGN KEY ([ProductID]) REFERENCES [Product] ([ID])
	);	

	CREATE TABLE [Payment] (
		[ID] UNIQUEIDENTIFIER PRIMARY KEY,
		[CustomerPaymentSystemID] UNIQUEIDENTIFIER NOT NULL,
		[OrderID] INT NOT NULL,
		[Total] DECIMAL(18,2) NOT NULL,
		[Method] nvarchar(255) NOT NULL CHECK ([Method] IN ('Cash', 'CreditCard', 'BLIK', 'BankTransfer', 'PayPal')),
		[Status] nvarchar(255) NOT NULL CHECK ([Status] IN ('Pending', 'Authorized', 'Completed', 'Failed', 'Cancelled', 'Refunded')),
		[PaymentStartAt] DATETIME2(3) NOT NULL,
		[PaidAt] DATETIME2(3),
		FOREIGN KEY ([OrderID]) REFERENCES [Order] ([ID]),
		FOREIGN KEY ([CustomerPaymentSystemID]) REFERENCES [CustomerPaymentSystem] ([ID])
	);
	CREATE INDEX IX_Payment_OrderID ON [Payment] ([OrderID])
	CREATE INDEX IX_Payment_CustomerPaymentSystemID ON [Payment] ([CustomerPaymentSystemID])
	CREATE INDEX IX_Payment_Status ON [Payment] ([Status])
	CREATE INDEX IX_Payment_PaymentStartAt ON [Payment] ([PaymentStartAt])
	CREATE INDEX IX_Payment_PaidAt ON [Payment] ([PaidAt])

	CREATE TABLE [Review] (
		[ID] INT PRIMARY KEY,
		[CustomerID] UNIQUEIDENTIFIER,
		[ProductID] INT NOT NULL,
		[Rating] TINYINT NOT NULL,
		[Description] NVARCHAR(4000),
		FOREIGN KEY ([ProductID]) REFERENCES [Product] ([ID]),
		FOREIGN KEY ([CustomerID]) REFERENCES [Customer] ([ID])		
	);
	CREATE INDEX IX_Review_CustomerID ON [Review] ([CustomerID])
	CREATE INDEX IX_Review_ProductID ON [Review] ([ProductID])
COMMIT;