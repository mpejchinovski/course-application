## How to run
Application currently running on Heroku. Link in About.

To run yourself, a running MySQL server instance is required for which a connection string needs to be provided in appsettings.json. The connectionString variable in Startup.cs also needs to be changed to acquire the connection string from appsettings.json.
Namely: 
```
string connectionString = Configuration.GetConnectionString("name_of_connection_string");
```
After, run command:
```
dotnet run
```
