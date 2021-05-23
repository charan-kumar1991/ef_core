# EF Core reference application
___

Postman collection [`here`](https://www.getpostman.com/collections/1a0201ab950da1134ee7)

1. To Scaffold the app layout
```
mkdir src
cd src

-- Create projects
dotnet new classlib -o Core
dotnet new classlib -o Infrastructure
dotnet new webapi -o Api

cd ..
-- Create fresh solution
dotnet new sln

-- Link projects to solution
dotnet sln add .\src\Api
dotnet sln add .\src\Core
dotnet sln add .\src\Infrastructure
```

2. Create appropriate models with constraints (Data annotations / Fluent Validations)

3. Install Nuget packages for 
```
Microsoft.EntityFrameworkCore.SqlServer -> EF Core for SQL Server (Add in infrastructure)
Microsoft.EntityFrameworkCore.Tools -> Add in the API project (Needed for Migration commands)
EntityFrameworkCore.Exceptions.SqlServer -> Library for catching common SQL Server exceptions (Install in Core)

```

4. Add migrations and flush the changes to database
```
Add-Migration <MIGRATION_NAME> (no spaces)
Update-Database
```

5. Construct repositories
6. Add the services and appropriate business logic
7. Make controllers and inject appropriate services
8. Register all the dependencies in `Startup.cs`