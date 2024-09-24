-- Criação do banco de dados

CREATE DATABASE AbcCompany;
GO

USE AbcCompany;
GO

-- Tabela Order
CREATE TABLE Orders (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderNumber INT NOT NULL,
    Date DATETIME,
    ClientId INT NOT NULL,
    ClientName NVARCHAR(100),
    BranchId INT,
    BranchName NVARCHAR(100),
    OrderStatusId INT,
    OrderStatusName NVARCHAR(100),
    Total DECIMAL(18, 2),
    DiscountTotal DECIMAL(18, 2),
);
GO

-- Tabela OrderProducts

CREATE TABLE OrderProducts (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT FOREIGN KEY REFERENCES [Orders](Id),
    ProductId INT NOT NULL,
    ProductName NVARCHAR(100),
    ProductUnitValue DECIMAL(18, 2),
    OrderProductStatusId INT NOT NULL,
    OrderProductStatusName NVARCHAR(100),
    Quantity INT,
    Discount DECIMAL(18, 2),
    Total DECIMAL(18, 2)
);
GO

-- Tabela OrderPayments
CREATE TABLE OrderPayments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT FOREIGN KEY REFERENCES [Orders](Id),
    PaymentId INT NOT NULL,
    PaymentName NVARCHAR(50),
    OrderPaymentStatusId INT NOT NULL,
    OrderPaymentStatusName NVARCHAR(100),
    Value DECIMAL(18, 2)
);
GO