CREATE TABLE IF NOT EXISTS Professor (
    Professor_ID INTEGER PRIMARY KEY AUTOINCREMENT,
    FirstName TEXT NOT NULL,
    MiddleName TEXT,
    LastName TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Course (
    Course_ID INTEGER PRIMARY KEY AUTOINCREMENT,
    Code TEXT NOT NULL UNIQUE,
    Name TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Student (
    ID INTEGER PRIMARY KEY AUTOINCREMENT,
    Student_Code TEXT NOT NULL UNIQUE,
    FirstName TEXT NOT NULL,
    MiddleName TEXT,
    LastName TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS ClassSection (
    ClassSection_ID INTEGER PRIMARY KEY AUTOINCREMENT,
    Course_ID INTEGER NOT NULL,
    Section_ID INTEGER NOT NULL,
    Professor_ID INTEGER NOT NULL,
    SectionName TEXT NOT NULL,
    GracePeriodMinutes INTEGER NOT NULL DEFAULT 15,
    FOREIGN KEY (Course_ID) REFERENCES Course(Course_ID) ON DELETE CASCADE,
    FOREIGN KEY (Professor_ID) REFERENCES Professor(Professor_ID) ON DELETE CASCADE,
    UNIQUE(Course_ID, Section_ID)
);

CREATE TABLE IF NOT EXISTS ClassSession (
    ClassSession_ID INTEGER PRIMARY KEY AUTOINCREMENT,
    ClassSection_ID INTEGER NOT NULL,
    DayOfWeek INTEGER NOT NULL, -- 0=Sunday, 1=Monday, etc.
    StartTime TEXT NOT NULL,    -- Format: HH:MM
    EndTime TEXT NOT NULL,      -- Format: HH:MM
    FOREIGN KEY (ClassSection_ID) REFERENCES ClassSection(ClassSection_ID) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Enrollment (
    Enrollment_ID INTEGER PRIMARY KEY AUTOINCREMENT,
    ClassSection_ID INTEGER NOT NULL,
    Student_ID INTEGER NOT NULL,
    FOREIGN KEY (ClassSection_ID) REFERENCES ClassSection(ClassSection_ID) ON DELETE CASCADE,
    FOREIGN KEY (Student_ID) REFERENCES Student(ID) ON DELETE CASCADE,
    UNIQUE(ClassSection_ID, Student_ID)
);

CREATE TABLE IF NOT EXISTS Attendance (
    Attendance_ID INTEGER PRIMARY KEY AUTOINCREMENT,
    ClassSession_ID INTEGER NOT NULL,
    Enrollment_ID INTEGER NOT NULL,
    Date_Stamp TEXT NOT NULL, -- Format: YYYY-MM-DD
    TimeIn TEXT,              -- Format: HH:MM:SS
    Status TEXT NOT NULL,     -- Present, Late, Excused, Absent
    FOREIGN KEY (ClassSession_ID) REFERENCES ClassSession(ClassSession_ID) ON DELETE CASCADE,
    FOREIGN KEY (Enrollment_ID) REFERENCES Enrollment(Enrollment_ID) ON DELETE CASCADE,
    UNIQUE(ClassSession_ID, Enrollment_ID, Date_Stamp)
);

INSERT OR IGNORE INTO Professor (Professor_ID, FirstName, MiddleName, LastName) VALUES (1, 'John', 'D.', 'Smith');
INSERT OR IGNORE INTO Professor (Professor_ID, FirstName, MiddleName, LastName) VALUES (2, 'Jane', 'A.', 'Doe');

INSERT OR IGNORE INTO Course (Course_ID, Code, Name) VALUES (1, 'CS101', 'Intro to Programming');
INSERT OR IGNORE INTO Course (Course_ID, Code, Name) VALUES (2, 'CS222', 'Advanced Computer Programming');
INSERT OR IGNORE INTO Course (Course_ID, Code, Name) VALUES (3, 'MTH101', 'Calculus I');
INSERT OR IGNORE INTO Course (Course_ID, Code, Name) VALUES (4, 'MTH202', 'Linear Algebra');
INSERT OR IGNORE INTO Course (Course_ID, Code, Name) VALUES (5, 'ENG101', 'English Composition');

-- Sample Class Sections
INSERT OR IGNORE INTO ClassSection (ClassSection_ID, Course_ID, Section_ID, Professor_ID, SectionName, GracePeriodMinutes) VALUES (1, 1, 1, 1, 'IT-1A', 15);
INSERT OR IGNORE INTO ClassSection (ClassSection_ID, Course_ID, Section_ID, Professor_ID, SectionName, GracePeriodMinutes) VALUES (2, 2, 2, 2, 'CS-2B', 15);

-- Sample Class Sessions
INSERT OR IGNORE INTO ClassSession (ClassSession_ID, ClassSection_ID, DayOfWeek, StartTime, EndTime) VALUES (1, 1, 1, '08:00', '10:00');
INSERT OR IGNORE INTO ClassSession (ClassSession_ID, ClassSection_ID, DayOfWeek, StartTime, EndTime) VALUES (2, 1, 3, '08:00', '10:00');
INSERT OR IGNORE INTO ClassSession (ClassSession_ID, ClassSection_ID, DayOfWeek, StartTime, EndTime) VALUES (3, 2, 2, '13:00', '15:00');
INSERT OR IGNORE INTO ClassSession (ClassSession_ID, ClassSection_ID, DayOfWeek, StartTime, EndTime) VALUES (4, 2, 4, '13:00', '15:00');