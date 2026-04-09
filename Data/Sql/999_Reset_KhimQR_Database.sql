DROP TABLE IF EXISTS Attendance;
DROP TABLE IF EXISTS Enrollment;
DROP TABLE IF EXISTS ClassSession;
DROP TABLE IF EXISTS ClassSection;
DROP TABLE IF EXISTS Student;
DROP TABLE IF EXISTS Course;
DROP TABLE IF EXISTS Professor;

CREATE TABLE Professor (
    Professor_ID INTEGER PRIMARY KEY AUTOINCREMENT,
    FirstName TEXT NOT NULL,
    MiddleName TEXT,
    LastName TEXT NOT NULL
);

CREATE TABLE Course (
    Course_ID INTEGER PRIMARY KEY AUTOINCREMENT,
    Code TEXT NOT NULL UNIQUE,
    Name TEXT NOT NULL
);

CREATE TABLE Student (
    ID INTEGER PRIMARY KEY AUTOINCREMENT,
    Student_Code TEXT NOT NULL UNIQUE,
    FirstName TEXT NOT NULL,
    MiddleName TEXT,
    LastName TEXT NOT NULL
);

CREATE TABLE ClassSection (
    ClassSection_ID INTEGER PRIMARY KEY AUTOINCREMENT,
    Course_ID INTEGER NOT NULL,
    Professor_ID INTEGER NOT NULL,
    SectionName TEXT NOT NULL,
    GracePeriodMinutes INTEGER NOT NULL DEFAULT 15,
    FOREIGN KEY (Course_ID) REFERENCES Course(Course_ID) ON DELETE CASCADE,
    FOREIGN KEY (Professor_ID) REFERENCES Professor(Professor_ID) ON DELETE CASCADE
);

CREATE TABLE ClassSession (
    ClassSession_ID INTEGER PRIMARY KEY AUTOINCREMENT,
    ClassSection_ID INTEGER NOT NULL,
    DayOfWeek INTEGER NOT NULL, -- 0=Sunday, 1=Monday, etc.
    StartTime TEXT NOT NULL,    -- Format: HH:MM
    EndTime TEXT NOT NULL,      -- Format: HH:MM
    FOREIGN KEY (ClassSection_ID) REFERENCES ClassSection(ClassSection_ID) ON DELETE CASCADE
);

CREATE TABLE Enrollment (
    Enrollment_ID INTEGER PRIMARY KEY AUTOINCREMENT,
    ClassSection_ID INTEGER NOT NULL,
    Student_ID INTEGER NOT NULL,
    FOREIGN KEY (ClassSection_ID) REFERENCES ClassSection(ClassSection_ID) ON DELETE CASCADE,
    FOREIGN KEY (Student_ID) REFERENCES Student(ID) ON DELETE CASCADE,
    UNIQUE(ClassSection_ID, Student_ID)
);

CREATE TABLE Attendance (
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

INSERT INTO Professor (FirstName, MiddleName, LastName) VALUES ('John', 'D.', 'Smith');
INSERT INTO Professor (FirstName, MiddleName, LastName) VALUES ('Jane', 'A.', 'Doe');
INSERT INTO Professor (FirstName, MiddleName, LastName) VALUES ('Robert', '', 'Johnson');

INSERT INTO Course (Code, Name) VALUES ('CS101', 'Intro to Programming');
INSERT INTO Course (Code, Name) VALUES ('CS222', 'Advanced Computer Programming');
INSERT INTO Course (Code, Name) VALUES ('MTH101', 'Calculus I');
INSERT INTO Course (Code, Name) VALUES ('MTH202', 'Linear Algebra');
INSERT INTO Course (Code, Name) VALUES ('ENG101', 'English Composition');

-- Sample Class Sections
INSERT INTO ClassSection (Course_ID, Professor_ID, SectionName, GracePeriodMinutes) VALUES (1, 1, 'IT-1A', 15);
INSERT INTO ClassSection (Course_ID, Professor_ID, SectionName, GracePeriodMinutes) VALUES (2, 2, 'CS-2B', 15);

-- Sample Class Sessions (Realistic Times)
-- Moday & Wednesday (DayOfWeek 1 and 3)
INSERT INTO ClassSession (ClassSection_ID, DayOfWeek, StartTime, EndTime) VALUES (1, 1, '08:00', '10:00');
INSERT INTO ClassSession (ClassSection_ID, DayOfWeek, StartTime, EndTime) VALUES (1, 3, '08:00', '10:00');

-- Tuesday & Thursday (DayOfWeek 2 and 4)
INSERT INTO ClassSession (ClassSection_ID, DayOfWeek, StartTime, EndTime) VALUES (2, 2, '13:00', '15:00');
INSERT INTO ClassSession (ClassSection_ID, DayOfWeek, StartTime, EndTime) VALUES (2, 4, '13:00', '15:00');
