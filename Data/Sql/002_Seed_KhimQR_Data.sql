-- Seed Professors (5)
INSERT OR IGNORE INTO Professor (Professor_ID, FirstName, MiddleName, LastName) VALUES (1, 'Robert', 'A.', 'Johnson');
INSERT OR IGNORE INTO Professor (Professor_ID, FirstName, MiddleName, LastName) VALUES (2, 'Maria', 'C.', 'Santos');
INSERT OR IGNORE INTO Professor (Professor_ID, FirstName, MiddleName, LastName) VALUES (3, 'Daniel', 'M.', 'Reyes');
INSERT OR IGNORE INTO Professor (Professor_ID, FirstName, MiddleName, LastName) VALUES (4, 'Angela', 'P.', 'Garcia');
INSERT OR IGNORE INTO Professor (Professor_ID, FirstName, MiddleName, LastName) VALUES (5, 'Victor', 'L.', 'Cruz');

-- Seed Courses (10 total, 2 intentionally without sections: Course_ID 9 and 10)
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
        WHEN 0 THEN 'A.'
        WHEN 1 THEN 'B.'
        WHEN 2 THEN 'C.'
        WHEN 3 THEN 'D.'
        ELSE 'E.'
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

-- Seed Class Sections (8 total, assigned to 8 courses so only 2 courses have no section)
INSERT OR IGNORE INTO ClassSection (ClassSection_ID, Course_ID, Professor_ID, SectionName, GracePeriodMinutes) VALUES (1, 1, 1, 'BSCS-1A', 15);
INSERT OR IGNORE INTO ClassSection (ClassSection_ID, Course_ID, Professor_ID, SectionName, GracePeriodMinutes) VALUES (2, 2, 2, 'BSCS-1B', 15);
INSERT OR IGNORE INTO ClassSection (ClassSection_ID, Course_ID, Professor_ID, SectionName, GracePeriodMinutes) VALUES (3, 3, 3, 'BSCS-2A', 15);
INSERT OR IGNORE INTO ClassSection (ClassSection_ID, Course_ID, Professor_ID, SectionName, GracePeriodMinutes) VALUES (4, 4, 4, 'BSCS-2B', 15);
INSERT OR IGNORE INTO ClassSection (ClassSection_ID, Course_ID, Professor_ID, SectionName, GracePeriodMinutes) VALUES (5, 5, 5, 'BSCS-3A', 15);
INSERT OR IGNORE INTO ClassSection (ClassSection_ID, Course_ID, Professor_ID, SectionName, GracePeriodMinutes) VALUES (6, 6, 1, 'BSCS-3B', 15);
INSERT OR IGNORE INTO ClassSection (ClassSection_ID, Course_ID, Professor_ID, SectionName, GracePeriodMinutes) VALUES (7, 7, 2, 'BSCS-4A', 15);
INSERT OR IGNORE INTO ClassSection (ClassSection_ID, Course_ID, Professor_ID, SectionName, GracePeriodMinutes) VALUES (8, 8, 3, 'BSCS-4B', 15);

-- Seed Class Sessions (per section: 3-6 days/week, 3-5 sessions/day)
WITH section_rules(ClassSection_ID, DayCount, SessionCount) AS (
    SELECT 1, 3, 3 UNION ALL
    SELECT 2, 4, 4 UNION ALL
    SELECT 3, 5, 5 UNION ALL
    SELECT 4, 6, 3 UNION ALL
    SELECT 5, 3, 4 UNION ALL
    SELECT 6, 4, 5 UNION ALL
    SELECT 7, 5, 3 UNION ALL
    SELECT 8, 6, 4
),
days(d) AS (
    SELECT 1
    UNION ALL
    SELECT d + 1 FROM days WHERE d < 6
),
slots(s) AS (
    SELECT 1
    UNION ALL
    SELECT s + 1 FROM slots WHERE s < 5
)
INSERT OR IGNORE INTO ClassSession (ClassSession_ID, ClassSection_ID, DayOfWeek, StartTime, EndTime)
SELECT
    ((sr.ClassSection_ID - 1) * 30) + ((d.d - 1) * 5) + s.s AS ClassSession_ID,
    sr.ClassSection_ID,
    d.d AS DayOfWeek,
    substr(time('08:00', printf('+%d minutes', (s.s - 1) * 120)), 1, 5) AS StartTime,
    substr(time('08:00', printf('+%d minutes', ((s.s - 1) * 120) + 90)), 1, 5) AS EndTime
FROM section_rules sr
JOIN days d ON d.d <= sr.DayCount
JOIN slots s ON s.s <= sr.SessionCount;

-- Seed Enrollment (100 students distributed across 8 sections)
WITH RECURSIVE seq(n) AS (
    SELECT 1
    UNION ALL
    SELECT n + 1 FROM seq WHERE n < 100
)
INSERT OR IGNORE INTO Enrollment (Enrollment_ID, ClassSection_ID, Student_ID)
SELECT
    n,
    ((n - 1) % 8) + 1 AS ClassSection_ID,
    n AS Student_ID
FROM seq;