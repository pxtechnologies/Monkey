# Monkey
The purpose of the project is to automate API Development based on Handler Design Pattern. You might think of Handler Design Pattern as a general purpose function that accept some parameters and returns some objects/records. Thus if:
* You are developing **CQRS** Handlers and want to automatically expose them by conventions with REST API 
* You are developing **T-SQL stored procedures** in database and you want to automatically expose procedures by conventions with REST API <br/>
Then you are at right place :)

## SQL-Dev Quick glance:
* You have your sql procedure named **"AddProduct"**
* You execute:
```sql
	EXEC webapi_BindStoredProc 'AddProduct','Test';
	EXEC webapi_Publish;
```
* You have your REST api available under: http://localhost:8080/api/Product and you can invoke it with parameters and receive record(s).
* Swagger is available under: http://localhost:8080/swagger

## Running on docker:
1. Open PowerShell or back and run:
```cmd
docker run -p 8080:80 -e "ConnectionStrings__Monkey=Server=<Server>;Database=Monkey;User=<User>;Password=<YourPassword>" \
					  -e "ConnectionStrings__Test=Server=<Server>;Database=Test;User=<User>;Password=<YourPassword>" \
					  -e "ASPNETCORE_URLS=http://+:80" pxtechnologies/monkey
```
   \<**Server**\> is the name of SQL server <br/>
   \<**User**\> is SQL user<br/>
   \<**YourPassword**\> is password to SQL Server<br/><br/>
	
2. Open brower and navigate to http://localhost:8080/swagger
3. If you want to install utils procedures to your database make sure to add environment variable with connection string and then invoke Install action though swagger
4. The name of the connection string is the sufix of envrionment variable name - in this example we have 'Monkey' and 'Test' connection strings.

## Running standalone:
1. Download [Monkey.zip](https://github.com/pxtechnologies/Monkey/blob/master/bundle/Monkey.Sql.WebApiHost.zip)
2. Install [dotnet-core runtime 2.2 or sdk](https://dotnet.microsoft.com/download)
3. Edit configuration file: **appsettings.json** - change connection-strings if needed.
4. Run the app from command-line:
```cmd
dotnet Monkey.Sql.WebApiHost
```

## Installing SQL API stored procedures on your database.
1. Add appropriate connection string to your database. You have 3 options to do it:
	- Edit configuration file: appsettings.json
	- Edit/Add environment variable: ConnectrionStrings__\<**Name_Of_Your_Connection**\>=\<**Connection_String**\>
	- Add command line arguments:  ConnectrionStrings__\<**Name_Of_Your_Connection**\>=\<**Connection_String**\>
2. Run WebApiHost
3. Make HTTP **POST** to [http://localhost:8080/Database/Install](http://localhost:8080/Database/Install) with payload: **{"name":"Name_Of_Your_Connection"}**. For example:
	1. Though swagger:
		- Open swagger page: [http://localhost:8080/swagger](http://localhost:8080/swagger) 
		- Invoke **Install** action on **Database** controller with appropriate payload.
	2. Though curl: 
	```cmd
	curl -X POST "http://localhost:8080/api/Install?connectionStringName=<Name_Of_Your_Connection>" -H "accept: application/json"
	```
#### Note
Installing SQL API stored procedures, creates synonyms, few stored procedures and functions. It does not create any tables, except for script migrations. All metadata is stored in 'Monkey' database.

## Documentation by scenarios
[Hosting Scenarios](https://github.com/pxtechnologies/Monkey/blob/master/src/Monkey/Docs/Bootstrapping.md)<br/>
[SQL Configuration Interface Scenarios](https://github.com/pxtechnologies/Monkey/blob/master/src/Monkey/Docs/SqlConfigurationInterface.md)<br/>
[Stored Procedure Primitive Type Bindings Scenarios](https://github.com/pxtechnologies/Monkey/blob/master/src/Monkey/Docs/PrimitiveInvocations.md)<br/>
[Error handling](https://github.com/pxtechnologies/Monkey/blob/master/src/Monkey/Docs/ErrorHandling.md)<br/>

## Documentation in SQL Database
[Retriving help information](https://github.com/pxtechnologies/Monkey/blob/master/src/Monkey/Docs/SqlHelp.md)

## Project TODOs:
* Documentation
* XML support in stored procedures
* Handling Kafka events
