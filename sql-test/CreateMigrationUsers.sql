-- Create the login
CREATE LOGIN aspmigration
WITH PASSWORD = 'devP@ssword!',
    DEFAULT_DATABASE = NorthWinds,
    CHECK_POLICY = ON,
    CHECK_EXPIRATION = OFF;

-- Create the user for the NorthWindsContext database
USE NorthWinds;
CREATE USER aspmigration FOR LOGIN aspmigration;
GO

-- Assign necessary roles for the user
ALTER ROLE [db_datareader] ADD MEMBER aspmigration;
ALTER ROLE [db_datawriter] ADD MEMBER aspmigration;
ALTER ROLE [db_owner] DROP MEMBER aspmigration
GO
