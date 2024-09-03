param (
    [switch]$delete
)

# Function to check if the database exists
function Test-DatabaseExists {
    param (
        [string]$server,
        [string]$username,
        [string]$password,
        [string]$database
    )

    $query = "IF DB_ID('$database') IS NOT NULL PRINT 'EXISTS'"
    $result = & sqlcmd -S $server -U $username -P $password -Q $query
    return $result -like '*EXISTS*'
}

# Function to delete the database
function Delete-Database {
    param (
        [string]$server,
        [string]$username,
        [string]$password,
        [string]$database
    )

    $query = @"
    ALTER DATABASE [$database] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [$database];
"@
    & sqlcmd -S $server -U $username -P $password -Q $query
    Write-Output "Database $database deleted."
}

# Database connection details
$server = "localhost"
$username = "SA"
$password = "YourStrong!Passw0rd"
$database = "NorthWinds"

if ($delete) {
    if (Test-DatabaseExists -server $server -username $username -password $password -database $database) {
        Delete-Database -server $server -username $username -password $password -database $database
    } else {
        Write-Output "Database $database does not exist. Nothing to delete."
    }
} else {
    # Check if the database already exists
    if (Test-DatabaseExists -server $server -username $username -password $password -database $database) {
        Write-Output "Database already exists. Skipping creation."
    } else {
        # Create the database
        $createDbQuery = "CREATE DATABASE [$database]"
        & sqlcmd -S $server -U $username -P $password -Q $createDbQuery

        # Run the SQL scripts to create the schema and insert data
        & sqlcmd -S $server -U $username -P $password -d $database -i "./instnwnd.sql"
        Write-Output "Schema created."
        & sqlcmd -S $server -U $username -P $password -d $database -i "./InsertNorthwindsDefaultData.sql"
        Write-Output "Data inserted."
        & sqlcmd -S $server -U $username -P $password -d $database -i "./CreateUsers.sql"
        Write-Output "Created asp user."
        & sqlcmd -S $server -U $username -P $password -d $database -i "./CreateReadOnlyUser.sql"
        Write-Output "Created readonly user."
        & sqlcmd -S $server -U $username -P $password -d $database -i "./CreateMigrationUsers.sql"
        Write-Output "Created migration user."

        Write-Output "Database created."
    }
}