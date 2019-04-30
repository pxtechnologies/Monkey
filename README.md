# Monkey
The purpose of the project is to automate API Development based on Handler Design Pattern. You might think of Handler Design Pattern as a general purpose function that accept some parameters and returns some objects/records. Thus if:
* You are developing **CQRS** Handlers and want to automatically expose them by conventions with REST API 
* You are developing **T-SQL stored procedures** in database and you want to automatically expose procedures by conventions with REST API <br/>
Then you are at right place :)

# Testing on docker:
1. Open PowerShell or back and run:
```cmd
docker run -p 8080:80 -e "ConnectionStrings__Monkey=Server=<Server>;Database=Monkey;User=<User>;Password=<YourPassword>" \
					  -e "ConnectionStrings__Test=Server=<Server>;Database=Test;User=<User>;Password=<YourPassword>" \
					  -e "ASPNETCORE_URLS=http://+:80" pxtechnologies/monkey
```
<Server> is the name of SQL server <br/>
<User> is SQL user<br/>
<YourPassword> is password to SQL Server<br/>
2. Open brower and navigate to http://localhost:8080/swagger
3. If you want to install utils procedures to your database make sure to add environment variable with connection string and then invoke Install action though swagger
4. The name of the connection string is the sufix of envrionment variable name - in this example we have 'Monkey' and 'Test' connection strings.

## Documentation by scenarios
[Hosting Scenarios](https://github.com/pxtechnologies/Monkey/blob/master/src/Monkey/Docs/Bootstrapping.md)<br/>
[SQL Configuration Interface Scenarios](https://github.com/pxtechnologies/Monkey/blob/master/src/Monkey/Docs/SqlConfigurationInterface.md)<br/>
[Stored Procedure Primitive Type Bindings Scenarios](https://github.com/pxtechnologies/Monkey/blob/master/src/Monkey/Docs/PrimitiveInvocations.md)<br/>

## Project TODOs:
* Documentation
* XML support in stored procedures
* Handling Kafka events
