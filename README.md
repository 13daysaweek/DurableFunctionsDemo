## Database Setup
This project needs a SQL Database to read sales history data and to store results.  Do the following to set this up:
1. Create a new SQL Database (either Azure SQL or local SQL Server)
2. In the root of the repository, find the file `tables.sql` and run this script in your new database
3. Create a new login/user with `db_datareader` and `db_datawriter` access to the database created in step 1

## Environment Setup
Before running this project, create a `local.settings.json` file in the project directory.  This file needs to have the following entries under the `values` section:
  
| Key                                 | Value                                    |
|-------------------------------------|------------------------------------------|
| AzureWebJobsStorage                 | The connection string to the storage account used by the Functions runtime.  To use the storage emulator, set the value to UseDevelopmentStorage=true |
| FUNCTIONS_WORKER_RUNTIME            | Set this value to `dotnet` as this is a .Net Core Function App |
| sales-history-sql-connection-string | A SQL connection string pointing to the database created above.  Note this connection string should use the database credentials created above. |
| input-storage-connection-string     | Connection string to a storage account that will be used by the Function to read the input blob containing regions and divisions |
