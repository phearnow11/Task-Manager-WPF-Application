create database TaskManager
use TaskManager


CREATE TABLE Users (
	UserId INT IDENTITY (1,1) PRIMARY KEY,
	username varchar (100) not null, 
	password varchar(100) not null, 
	name nvarchar(100),
    CreatedAt DATETIME DEFAULT GETDATE(),
)

insert into Users (username, password, name) values (N'Phien', N'password', N'Phien')
insert into Tasks(Title, UserId) values (N'Test', 1)
insert into Tasks(Title, UserId, DueDateTime, Priority) values (N'Testt', 1, GETDATE(), N'Low')
select * from Tasks
-- Create Tasks table
CREATE TABLE Tasks (
    TaskId INT IDENTITY(1,1) PRIMARY KEY,
	UserId INT not null,
    Title NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX),
    DueDateTime DATETIME,
    Priority NVARCHAR(20),
    Status NVARCHAR(20),
    RecurrenceType NVARCHAR(20),
    RecurrenceInterval INT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
	FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

-- Create TaskDependencies table
CREATE TABLE TaskDependencies (
    DependencyId INT IDENTITY(1,1) PRIMARY KEY,
    TaskId INT NOT NULL,
    DependsOnTaskId INT NOT NULL,
    FOREIGN KEY (TaskId) REFERENCES Tasks(TaskId),
    FOREIGN KEY (DependsOnTaskId) REFERENCES Tasks(TaskId)
);

-- Create Reminders table
CREATE TABLE Reminders (
    ReminderId INT IDENTITY(1,1) PRIMARY KEY,
    TaskId INT NOT NULL,
    ReminderDateTime DATETIME NOT NULL,
    FOREIGN KEY (TaskId) REFERENCES Tasks(TaskId)
);

-- Create indexes for faster querying
CREATE INDEX IX_TaskDependencies_TaskId ON TaskDependencies(TaskId);
CREATE INDEX IX_TaskDependencies_DependsOnTaskId ON TaskDependencies(DependsOnTaskId);
CREATE INDEX IX_Reminders_TaskId ON Reminders(TaskId);
CREATE INDEX IX_Reminders_ReminderDateTime ON Reminders(ReminderDateTime);

CREATE PROCEDURE CreateTask
    @Title NVARCHAR(100),
    @Description NVARCHAR(MAX),
    @DueDateTime DATETIME,
    @Priority NVARCHAR(20),
    @Status NVARCHAR(20),
    @RecurrenceType NVARCHAR(20),
    @RecurrenceInterval INT,
    @DependencyIds NVARCHAR(MAX) = NULL,
    @ReminderDateTime DATETIME = NULL
AS
BEGIN
    DECLARE @TaskId INT;

    INSERT INTO Tasks (Title, Description, DueDateTime, Priority, Status, RecurrenceType, RecurrenceInterval)
    VALUES (@Title, @Description, @DueDateTime, @Priority, @Status, @RecurrenceType, @RecurrenceInterval);

    SET @TaskId = SCOPE_IDENTITY();

    IF @DependencyIds IS NOT NULL
    BEGIN
        INSERT INTO TaskDependencies (TaskId, DependsOnTaskId)
        SELECT @TaskId, value
        FROM STRING_SPLIT(@DependencyIds, ',');
    END

    IF @ReminderDateTime IS NOT NULL
    BEGIN
        INSERT INTO Reminders (TaskId, ReminderDateTime)
        VALUES (@TaskId, @ReminderDateTime);
    END

    SELECT @TaskId AS NewTaskId;
END
GO

-- Get all tasks with their dependencies and reminders
CREATE PROCEDURE GetAllTasks
AS
BEGIN
    SELECT 
        T.TaskId, T.Title, T.Description, T.DueDateTime, T.Priority, T.Status,
        T.RecurrenceType, T.RecurrenceInterval,
        STUFF((
            SELECT ',' + CAST(TD.DependsOnTaskId AS NVARCHAR(10))
            FROM TaskDependencies TD
            WHERE TD.TaskId = T.TaskId
            FOR XML PATH('')
        ), 1, 1, '') AS DependencyIds,
        R.ReminderDateTime
    FROM Tasks T
    LEFT JOIN Reminders R ON T.TaskId = R.TaskId
END
GO

-- Update a task
CREATE PROCEDURE UpdateTask
    @TaskId INT,
    @Title NVARCHAR(100),
    @Description NVARCHAR(MAX),
    @DueDateTime DATETIME,
    @Priority NVARCHAR(20),
    @Status NVARCHAR(20),
    @RecurrenceType NVARCHAR(20),
    @RecurrenceInterval INT,
    @DependencyIds NVARCHAR(MAX) = NULL,
    @ReminderDateTime DATETIME = NULL
AS
BEGIN
    UPDATE Tasks
    SET Title = @Title,
        Description = @Description,
        DueDateTime = @DueDateTime,
        Priority = @Priority,
        Status = @Status,
        RecurrenceType = @RecurrenceType,
        RecurrenceInterval = @RecurrenceInterval,
        UpdatedAt = GETDATE()
    WHERE TaskId = @TaskId;

    DELETE FROM TaskDependencies WHERE TaskId = @TaskId;
    DELETE FROM Reminders WHERE TaskId = @TaskId;

    IF @DependencyIds IS NOT NULL
    BEGIN
        INSERT INTO TaskDependencies (TaskId, DependsOnTaskId)
        SELECT @TaskId, value
        FROM STRING_SPLIT(@DependencyIds, ',');
    END

    IF @ReminderDateTime IS NOT NULL
    BEGIN
        INSERT INTO Reminders (TaskId, ReminderDateTime)
        VALUES (@TaskId, @ReminderDateTime);
    END
END
GO

-- Delete a task
CREATE PROCEDURE DeleteTask
    @TaskId INT
AS
BEGIN
    DELETE FROM TaskDependencies WHERE TaskId = @TaskId OR DependsOnTaskId = @TaskId;
    DELETE FROM Reminders WHERE TaskId = @TaskId;
    DELETE FROM Tasks WHERE TaskId = @TaskId;
END
GO

-- Get due recurring tasks
CREATE PROCEDURE GetDueRecurringTasks
AS
BEGIN
    SELECT *
    FROM Tasks
    WHERE RecurrenceType IS NOT NULL
    AND DueDateTime <= GETDATE()
END
GO

-- Get due reminders
CREATE PROCEDURE GetDueReminders
AS
BEGIN
    SELECT T.*, R.ReminderDateTime
    FROM Tasks T
    INNER JOIN Reminders R ON T.TaskId = R.TaskId
    WHERE R.ReminderDateTime <= GETDATE()
END
GO
