#!/bin/bash

# Wait for SQL Server to be ready
echo "Waiting for SQL Server to be ready..."
sleep 30s

# Run the SQL script to create the database
/opt/mssql-tools18/bin/sqlcmd -S localhost -U SA -P "YourStrong!Passw0rd" -i /sql-test/instnwnd.sql
/opt/mssql-tools18/bin/sqlcmd -S localhost -U SA -P "YourStrong!Passw0rd" -i /sql-test/InsertNorthwindsDefaultData.sql
/opt/mssql-tools18/bin/sqlcmd -S localhost -U SA -P "YourStrong!Passw0rd" -i /sql-test/CreateUsers.sql
/opt/mssql-tools18/bin/sqlcmd -S localhost -U SA -P "YourStrong!Passw0rd" -i /sql-test/CreateReadOnlyUsers.sql
/opt/mssql-tools18/bin/sqlcmd -S localhost -U SA -P "YourStrong!Passw0rd" -i /sql-test/CreateMigrationUsers.sql


echo "Database created."