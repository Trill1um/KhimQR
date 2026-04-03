DROP TABLE IF EXISTS Attendance;
DROP TABLE IF EXISTS StudentMasterLists;
DROP TABLE IF EXISTS Course;
DROP TABLE IF EXISTS Autonumber;

CREATE TABLE Autonumber (
    pfx TEXT NOT NULL PRIMARY KEY,
    NewNumber TEXT NOT NULL
);

CREATE TABLE Course (
    Code TEXT NOT NULL PRIMARY KEY,
    Name TEXT NOT NULL
);

CREATE TABLE StudentMasterLists (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    StudentID TEXT NOT NULL,
    Firstname TEXT NOT NULL,
    Middlename TEXT,
    Lastname TEXT NOT NULL,
    Course TEXT NOT NULL,
    Section TEXT NOT NULL,
    QRCode BLOB NOT NULL,
    CreatedAt TEXT NOT NULL DEFAULT (datetime('now'))
);

CREATE INDEX IX_StudentMasterLists_StudentID ON StudentMasterLists(StudentID);

CREATE TABLE Attendance (
    RecNumber INTEGER PRIMARY KEY AUTOINCREMENT,
    StudentID TEXT NOT NULL,
    Date_STAMP TEXT NOT NULL,
    TimeIN TEXT NOT NULL
);

CREATE INDEX IX_Attendance_StudentID_Date_STAMP ON Attendance(StudentID, Date_STAMP);