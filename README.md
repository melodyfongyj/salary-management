# Introduction 
This project was developed to test my .NET and Angular knowledge. Salary Management system consists of 2 applications.

Back-end: salary-management (.NET Core)

Front-end: salary-management-frontend (Angular)


Project Description: Employees will be able to log in and view their personal, work, bank and salary information. Employees are able to update their personal and bank details.
There will also be an administrator who will be in charge of creating employee accounts, updating salaries as well as modifying other information.


# Installation process
1) This system uses SQL Server 2019. If you are using SQL Express, you may change the database server type to `Server=.\\SQLEXPRESS` inside `appsettings.json` under DefaultConnection.
2) Delete the migration files under `Migrations` folder. Via the NuGet Package Console, `add-migration InitialCreate` then `update-database`.
3) You must host this application in IIS, set the physical path to `..SalaryManagement\bin\Release\net5.0\publish` and set the port number to 90. If you use a port number other than 90, you must update `..src\app\constant\constant.ts` inside the front-end application.

Note: If you host your front-end application on IIS other than port number 91, you must update the port number inside `Startup.cs` > `builder.WithOrigins("http://localhost:91", "http://localhost:4200")` for CORS access.


# API references
You may check the API via Swagger: http://localhost:91/swagger


# Video demo

https://user-images.githubusercontent.com/93577252/151668965-c41279be-0bb2-40d0-a416-07bf04b7baa2.mp4

