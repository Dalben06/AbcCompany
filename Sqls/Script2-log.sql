-- Criar a base de dados
CREATE DATABASE AbcCompanyLog;
GO

-- Usar a base de dados recém-criada
USE AbcCompanyLog;
GO

-- Criar a tabela para armazenar os logs do Serilog
CREATE TABLE Logs
(
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Message] NVARCHAR(MAX),
    [MessageTemplate] NVARCHAR(MAX),
    [Level] NVARCHAR(MAX),
    [TimeStamp] DATETIME,
    [Exception] NVARCHAR(MAX),
    [Properties] NVARCHAR(MAX)
);
GO