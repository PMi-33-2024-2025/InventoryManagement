CREATE DATABASE [InventoryManagement];
GO

CREATE TABLE [InventoryManagement].[dbo].[Categories] (
    [Id] INT IDENTITY (1, 1) PRIMARY KEY,
    [Name] NVARCHAR(255) NOT NULL,
);

CREATE TABLE [InventoryManagement].[dbo].[Suppliers] (
    [Id] INT IDENTITY (1, 1) PRIMARY KEY,
    [Name] NVARCHAR(255) NOT NULL,
);

CREATE TABLE [InventoryManagement].[dbo].[Products] (
    [Id] INT IDENTITY (1, 1) PRIMARY KEY,
    [Title] NVARCHAR(255) NOT NULL,
    [Amount] INT NOT NULL CHECK([Amount] >= 0),
    [Price] DECIMAL(19, 4) NOT NULL CHECK([Price] > 0),
    [Description] NVARCHAR(MAX) NULL,
    [CategoryId] INT FOREIGN KEY REFERENCES [Categories]([Id]),
    [SupplierId] INT FOREIGN KEY REFERENCES [Suppliers]([Id]),
    [LastUpdated] DATETIME NOT NULL
);