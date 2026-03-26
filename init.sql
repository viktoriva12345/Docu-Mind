IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'DocuMind')
BEGIN
    CREATE DATABASE DocuMind;
END
GO

USE DocuMind;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Documents')
BEGIN
    CREATE TABLE Documents (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        FileName NVARCHAR(255) NOT NULL,
        FileExtension NVARCHAR(10) NOT NULL,
        ExtractedText NVARCHAR(MAX) NOT NULL,
        Status NVARCHAR(50) NOT NULL DEFAULT 'Ready',
        FileSizeBytes BIGINT NOT NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END
GO
