CREATE DATABASE [InventoryManagement];
GO

CREATE TABLE [InventoryManagement].[dbo].[Users] (
    [Id] INT IDENTITY (1, 1) PRIMARY KEY,
    [Name] VARCHAR(255) NOT NULL,
    [Password] VARCHAR(255) NOT NULL,
    [Role] VARCHAR(10) NOT NULL CHECK([Role] IN ('user', 'admin'))
);

CREATE TABLE [InventoryManagement].[dbo].[Categories] (
    [Id] INT IDENTITY (1, 1) PRIMARY KEY,
    [Name] VARCHAR(255) NOT NULL,
);

CREATE TABLE [InventoryManagement].[dbo].[Suppliers] (
    [Id] INT IDENTITY (1, 1) PRIMARY KEY,
    [Name] VARCHAR(255) NOT NULL,
);

CREATE TABLE [InventoryManagement].[dbo].[Products] (
    [Id] INT IDENTITY (1, 1) PRIMARY KEY,
    [Title] VARCHAR(255) NOT NULL,
    [Amount] INT NOT NULL CHECK([Amount] >= 0),
    [Price] DECIMAL(19, 4) NOT NULL CHECK([Price] > 0),
    [Description] TEXT NULL,
    [CategoryId] INT FOREIGN KEY REFERENCES [Categories]([Id]),
    [SupplierId] INT FOREIGN KEY REFERENCES [Suppliers]([Id]),
    [LastUpdated] TIMESTAMP NOT NULL
);