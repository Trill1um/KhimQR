-- Seed Professors (6)
INSERT OR IGNORE INTO Professor (Professor_ID, FirstName, MiddleName, LastName, Password, IsAdmin) VALUES (1, 'Robert', 'Alexander', 'Johnson', 'Robert123', 0);
INSERT OR IGNORE INTO Professor (Professor_ID, FirstName, MiddleName, LastName, Password, IsAdmin) VALUES (2, 'Maria', 'Carter', 'Santos', 'Maria123', 0);
INSERT OR IGNORE INTO Professor (Professor_ID, FirstName, MiddleName, LastName, Password, IsAdmin) VALUES (3, 'Daniel', 'Miguel', 'Reyes', 'Daniel123', 0);
INSERT OR IGNORE INTO Professor (Professor_ID, FirstName, MiddleName, LastName, Password, IsAdmin) VALUES (4, 'Angela', 'Patricia', 'Garcia', 'Angela123', 0);
INSERT OR IGNORE INTO Professor (Professor_ID, FirstName, MiddleName, LastName, Password, IsAdmin) VALUES (5, 'Victor', 'Luis', 'Cruz', 'Victor123', 0);
INSERT OR IGNORE INTO Professor (Professor_ID, FirstName, MiddleName, LastName, Password, IsAdmin) VALUES (6, 'Admin', NULL, 'Account', 'Admin123', 1);

-- Seed Courses (10 total)
INSERT OR IGNORE INTO Course (Course_ID, Code, Name) VALUES (1, 'CS101', 'Introduction to Programming');
INSERT OR IGNORE INTO Course (Course_ID, Code, Name) VALUES (2, 'CS102', 'Object-Oriented Programming');
INSERT OR IGNORE INTO Course (Course_ID, Code, Name) VALUES (3, 'CS201', 'Data Structures');
INSERT OR IGNORE INTO Course (Course_ID, Code, Name) VALUES (4, 'CS202', 'Database Systems');
INSERT OR IGNORE INTO Course (Course_ID, Code, Name) VALUES (5, 'CS203', 'Computer Networks');
INSERT OR IGNORE INTO Course (Course_ID, Code, Name) VALUES (6, 'CS204', 'Operating Systems');
INSERT OR IGNORE INTO Course (Course_ID, Code, Name) VALUES (7, 'CS301', 'Software Engineering');
INSERT OR IGNORE INTO Course (Course_ID, Code, Name) VALUES (8, 'CS302', 'Web Development');
INSERT OR IGNORE INTO Course (Course_ID, Code, Name) VALUES (9, 'CS303', 'Mobile Development');
INSERT OR IGNORE INTO Course (Course_ID, Code, Name) VALUES (10, 'CS304', 'Machine Learning Fundamentals');

-- Seed Students (100)
WITH RECURSIVE seq(n) AS (
    SELECT 1
    UNION ALL
    SELECT n + 1 FROM seq WHERE n < 100
)
INSERT OR IGNORE INTO Student (ID, Student_Code, FirstName, MiddleName, LastName)
SELECT
    n,
    printf('STU%04d', n),
    CASE (n % 10)
        WHEN 0 THEN 'Liam'
        WHEN 1 THEN 'Noah'
        WHEN 2 THEN 'Olivia'
        WHEN 3 THEN 'Emma'
        WHEN 4 THEN 'Lucas'
        WHEN 5 THEN 'Sophia'
        WHEN 6 THEN 'Mason'
        WHEN 7 THEN 'Mia'
        WHEN 8 THEN 'Ethan'
        ELSE 'Ava'
    END,
    CASE (n % 5)
        WHEN 0 THEN 'Anderson'
        WHEN 1 THEN 'Bernard'
        WHEN 2 THEN 'Carter'
        WHEN 3 THEN 'Daniel'
        ELSE 'Edward'
    END,
    CASE (n % 12)
        WHEN 0 THEN 'Lopez'
        WHEN 1 THEN 'Gonzales'
        WHEN 2 THEN 'Fernandez'
        WHEN 3 THEN 'Ramos'
        WHEN 4 THEN 'Torres'
        WHEN 5 THEN 'Mendoza'
        WHEN 6 THEN 'Navarro'
        WHEN 7 THEN 'Villanueva'
        WHEN 8 THEN 'Delacruz'
        WHEN 9 THEN 'Castro'
        WHEN 10 THEN 'Rivera'
        ELSE 'Aquino'
    END
FROM seq;

-- Seed Class Sections (20 total, two sections per course)
-- A section is defined by Course + SectionName + Professor.
-- Section names repeat across different courses, but a single section is owned by one professor only.
INSERT OR IGNORE INTO ClassSection (ClassSection_ID, Course_ID, Professor_ID, SectionName, GracePeriodMinutes) VALUES (1, 1, 1, 'Block-A', 15);
INSERT OR IGNORE INTO ClassSection (ClassSection_ID, Course_ID, Professor_ID, SectionName, GracePeriodMinutes) VALUES (2, 1, 2, 'Block-B', 15);
INSERT OR IGNORE INTO ClassSection (ClassSection_ID, Course_ID, Professor_ID, SectionName, GracePeriodMinutes) VALUES (3, 2, 2, 'Block-A', 15);
INSERT OR IGNORE INTO ClassSection (ClassSection_ID, Course_ID, Professor_ID, SectionName, GracePeriodMinutes) VALUES (4, 2, 3, 'Block-B', 15);
INSERT OR IGNORE INTO ClassSection (ClassSection_ID, Course_ID, Professor_ID, SectionName, GracePeriodMinutes) VALUES (5, 3, 3, 'Block-A', 15);
INSERT OR IGNORE INTO ClassSection (ClassSection_ID, Course_ID, Professor_ID, SectionName, GracePeriodMinutes) VALUES (6, 3, 4, 'Block-B', 15);
INSERT OR IGNORE INTO ClassSection (ClassSection_ID, Course_ID, Professor_ID, SectionName, GracePeriodMinutes) VALUES (7, 4, 4, 'Block-A', 15);
INSERT OR IGNORE INTO ClassSection (ClassSection_ID, Course_ID, Professor_ID, SectionName, GracePeriodMinutes) VALUES (8, 4, 5, 'Block-B', 15);
INSERT OR IGNORE INTO ClassSection (ClassSection_ID, Course_ID, Professor_ID, SectionName, GracePeriodMinutes) VALUES (9, 5, 5, 'Block-A', 15);
INSERT OR IGNORE INTO ClassSection (ClassSection_ID, Course_ID, Professor_ID, SectionName, GracePeriodMinutes) VALUES (10, 5, 1, 'Block-B', 15);
INSERT OR IGNORE INTO ClassSection (ClassSection_ID, Course_ID, Professor_ID, SectionName, GracePeriodMinutes) VALUES (11, 6, 1, 'Block-A', 15);
INSERT OR IGNORE INTO ClassSection (ClassSection_ID, Course_ID, Professor_ID, SectionName, GracePeriodMinutes) VALUES (12, 6, 3, 'Block-B', 15);
INSERT OR IGNORE INTO ClassSection (ClassSection_ID, Course_ID, Professor_ID, SectionName, GracePeriodMinutes) VALUES (13, 7, 2, 'Block-A', 15);
INSERT OR IGNORE INTO ClassSection (ClassSection_ID, Course_ID, Professor_ID, SectionName, GracePeriodMinutes) VALUES (14, 7, 4, 'Block-B', 15);
INSERT OR IGNORE INTO ClassSection (ClassSection_ID, Course_ID, Professor_ID, SectionName, GracePeriodMinutes) VALUES (15, 8, 3, 'Block-A', 15);
INSERT OR IGNORE INTO ClassSection (ClassSection_ID, Course_ID, Professor_ID, SectionName, GracePeriodMinutes) VALUES (16, 8, 5, 'Block-B', 15);
INSERT OR IGNORE INTO ClassSection (ClassSection_ID, Course_ID, Professor_ID, SectionName, GracePeriodMinutes) VALUES (17, 9, 4, 'Block-A', 15);
INSERT OR IGNORE INTO ClassSection (ClassSection_ID, Course_ID, Professor_ID, SectionName, GracePeriodMinutes) VALUES (18, 9, 1, 'Block-B', 15);
INSERT OR IGNORE INTO ClassSection (ClassSection_ID, Course_ID, Professor_ID, SectionName, GracePeriodMinutes) VALUES (19, 10, 5, 'Block-A', 15);
INSERT OR IGNORE INTO ClassSection (ClassSection_ID, Course_ID, Professor_ID, SectionName, GracePeriodMinutes) VALUES (20, 10, 2, 'Block-B', 15);

-- Seed Class Sessions (non-overlapping schedules per professor and per student's multi-section load)
WITH section_ids(ClassSection_ID) AS (
    SELECT 1 UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4 UNION ALL SELECT 5
    UNION ALL SELECT 6 UNION ALL SELECT 7 UNION ALL SELECT 8 UNION ALL SELECT 9 UNION ALL SELECT 10
    UNION ALL SELECT 11 UNION ALL SELECT 12 UNION ALL SELECT 13 UNION ALL SELECT 14 UNION ALL SELECT 15
    UNION ALL SELECT 16 UNION ALL SELECT 17 UNION ALL SELECT 18 UNION ALL SELECT 19 UNION ALL SELECT 20
),
session_seed AS (
    SELECT
        ClassSection_ID,
        ((ClassSection_ID - 1) / 4) + 1 AS DayOfWeek,
        CASE ((ClassSection_ID - 1) % 4)
            WHEN 0 THEN '08:00'
            WHEN 1 THEN '09:30'
            WHEN 2 THEN '11:00'
            ELSE '13:00'
        END AS StartTime,
        CASE ((ClassSection_ID - 1) % 4)
            WHEN 0 THEN '09:15'
            WHEN 1 THEN '10:45'
            WHEN 2 THEN '12:15'
            ELSE '14:15'
        END AS EndTime
    FROM section_ids
)
INSERT OR IGNORE INTO ClassSession (ClassSection_ID, DayOfWeek, StartTime, EndTime)
SELECT ClassSection_ID, DayOfWeek, StartTime, EndTime
FROM session_seed;

-- Seed Enrollment (each student is enrolled in multiple sections across different courses)
WITH RECURSIVE seq(n) AS (
    SELECT 1
    UNION ALL
    SELECT n + 1 FROM seq WHERE n < 100
),
student_sections AS (
    -- Day block 1 (ClassSection_ID 1-4)
    SELECT n AS Student_ID, ((n - 1) % 4) + 1 AS ClassSection_ID FROM seq
    UNION ALL
    -- Day block 2 (ClassSection_ID 5-8)
    SELECT n AS Student_ID, 4 + ((n - 1) % 4) + 1 AS ClassSection_ID FROM seq
    UNION ALL
    -- Day block 3 (ClassSection_ID 9-12)
    SELECT n AS Student_ID, 8 + ((n - 1) % 4) + 1 AS ClassSection_ID FROM seq
)
INSERT OR IGNORE INTO Enrollment (ClassSection_ID, Student_ID, EnrollmentDate)
SELECT
    ClassSection_ID,
    Student_ID,
    date('2026-04-16') AS EnrollmentDate
FROM student_sections;