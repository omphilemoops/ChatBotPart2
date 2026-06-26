CREATE TABLE Tasks
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title VARCHAR(100) NOT NULL,
    Description VARCHAR(500),
    ReminderDate DATETIME,
    Completed BIT NOT NULL DEFAULT 0
);