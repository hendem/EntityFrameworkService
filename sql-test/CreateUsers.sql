-- Create the login
CREATE LOGIN aspnet
WITH PASSWORD = 'devP@ssword!',
    DEFAULT_DATABASE = NorthWinds,
    CHECK_POLICY = ON,
    CHECK_EXPIRATION = OFF;

-- Create the user for the NorthWindsContext database
USE NorthWinds;
CREATE USER aspnet FOR LOGIN aspnet;
GO

-- Assign necessary roles for the user
ALTER ROLE [db_datareader] ADD MEMBER aspnet;
ALTER ROLE [db_datawriter] ADD MEMBER aspnet;
--ALTER ROLE [db_owner] DROP MEMBER aspnet
GO
