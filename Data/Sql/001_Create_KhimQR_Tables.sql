CREATE TABLE IF NOT EXISTS Autonumber (
    pfx TEXT NOT NULL PRIMARY KEY,
    NewNumber TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Course (
    Code TEXT NOT NULL PRIMARY KEY,
    Name TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS StudentMasterLists (
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

CREATE INDEX IF NOT EXISTS IX_StudentMasterLists_StudentID ON StudentMasterLists(StudentID);

CREATE TABLE IF NOT EXISTS Attendance (
    RecNumber INTEGER PRIMARY KEY AUTOINCREMENT,
    StudentID TEXT NOT NULL,
    Date_STAMP TEXT NOT NULL,
    TimeIN TEXT NOT NULL
);

CREATE INDEX IF NOT EXISTS IX_Attendance_StudentID_Date_STAMP ON Attendance(StudentID, Date_STAMP);