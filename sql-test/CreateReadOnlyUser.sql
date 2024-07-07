-- Create the login
CREATE LOGIN aspnetReadOnly
WITH PASSWORD = 'devP@ssword!',
    DEFAULT_DATABASE = NorthWinds,
    CHECK_POLICY = ON,
    CHECK_EXPIRATION = OFF;

-- Create the user for the NorthWindsContext database
USE NorthWinds;
CREATE USER aspnetReadOnly FOR LOGIN aspnetReadOnly;
GO

-- Assign necessary roles for the user
ALTER ROLE [db_datareader] ADD MEMBER aspnetReadOnly;
GO

--USE NorthWinds;
--EXEC sp_MSforeachtable 'GRANT SELECT ON ? TO [your_username]';
