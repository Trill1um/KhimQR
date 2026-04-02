IF OBJECT_ID('dbo.Attendance', 'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.Attendance;
END;
GO

IF OBJECT_ID('dbo.StudentMasterLists', 'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.StudentMasterLists;
END;
GO

IF OBJECT_ID('dbo.Course', 'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.Course;
END;
GO

IF OBJECT_ID('dbo.Autonumber', 'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.Autonumber;
END;
GO

CREATE TABLE dbo.Autonumber
(
    pfx NVARCHAR(20) NOT NULL PRIMARY KEY,
    NewNumber NVARCHAR(50) NOT NULL
);
GO

CREATE TABLE dbo.Course
(
    Code NVARCHAR(20) NOT NULL PRIMARY KEY,
    Name NVARCHAR(150) NOT NULL
);
GO

CREATE TABLE dbo.StudentMasterLists
(
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    StudentID NVARCHAR(50) NOT NULL,
    Firstname NVARCHAR(100) NOT NULL,
    Middlename NVARCHAR(100) NULL,
    Lastname NVARCHAR(100) NOT NULL,
    Course NVARCHAR(100) NOT NULL,
    Section NVARCHAR(50) NOT NULL,
    QRCode VARBINARY(MAX) NOT NULL,
    CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_StudentMasterLists_CreatedAt DEFAULT SYSUTCDATETIME()
);
GO

CREATE INDEX IX_StudentMasterLists_StudentID ON dbo.StudentMasterLists(StudentID);
GO

CREATE TABLE dbo.Attendance
(
    RecNumber INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    StudentID NVARCHAR(50) NOT NULL,
    Date_STAMP DATE NOT NULL,
    TimeIN TIME(0) NOT NULL
);
GO

CREATE INDEX IX_Attendance_StudentID_Date_STAMP ON dbo.Attendance(StudentID, Date_STAMP);
GO