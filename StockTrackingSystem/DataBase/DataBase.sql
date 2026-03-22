/* =========================
   DROP EXISTING DATABASE
========================= */
IF DB_ID('StockTrackingDb') IS NOT NULL
BEGIN
    ALTER DATABASE StockTrackingDb SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE StockTrackingDb;
END
GO

/* =========================
   CREATE DATABASE
========================= */
CREATE DATABASE StockTrackingDb;
GO

/* =========================
   USE DATABASE
========================= */
USE StockTrackingDb;
GO

/* =========================
   CATEGORIES TABLE
========================= */
CREATE TABLE Categories
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedDate DATETIME NULL
);

/* =========================
   SUPPLIERS TABLE
========================= */
CREATE TABLE Suppliers
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CompanyName NVARCHAR(150) NOT NULL,
    ContactName NVARCHAR(100) NULL,
    Phone NVARCHAR(20) NULL,
    Email NVARCHAR(100) NULL,
    Address NVARCHAR(255) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedDate DATETIME NULL
);

/* =========================
   AUDIT LOGS TABLE
========================= */
CREATE TABLE AuditLogs
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ActionType NVARCHAR(50) NOT NULL,
    EntityName NVARCHAR(100) NULL,
    EntityId INT NULL,
    Description NVARCHAR(255) NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
);

/* =========================
   PRODUCTS TABLE
========================= */
CREATE TABLE Products
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(150) NOT NULL,
    ProductCode NVARCHAR(50) NULL,
    Description NVARCHAR(255) NULL,
    CategoryId INT NOT NULL,
    SupplierId INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    StockQuantity INT NOT NULL DEFAULT 0,
    CriticalStockLevel INT NOT NULL DEFAULT 0,
    UnitType NVARCHAR(50) NULL,
    ImageUrl NVARCHAR(255) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedDate DATETIME NULL,
    CONSTRAINT FK_Products_Categories FOREIGN KEY (CategoryId) REFERENCES Categories(Id),
    CONSTRAINT FK_Products_Suppliers FOREIGN KEY (SupplierId) REFERENCES Suppliers(Id)
);

/* =========================
   STOCK MOVEMENTS TABLE
========================= */
CREATE TABLE StockMovements
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProductId INT NOT NULL,
    MovementType NVARCHAR(50) NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2) NULL,
    ReferenceCode NVARCHAR(100) NULL,
    Description NVARCHAR(255) NULL,
    MovementDate DATETIME NOT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_StockMovements_Products FOREIGN KEY (ProductId) REFERENCES Products(Id)
);

/* =========================
   INDEXES
========================= */
CREATE INDEX IX_Products_CategoryId ON Products(CategoryId);
CREATE INDEX IX_Products_SupplierId ON Products(SupplierId);
CREATE INDEX IX_StockMovements_ProductId ON StockMovements(ProductId);
GO