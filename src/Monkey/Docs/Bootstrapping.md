#Bootstrapping

##I want to restart app and load assembly from database

_Given _ the 'Test' database is created
_And _ the 'Monkey' database is created
_And _ WebApiHost has started
_And _ Monkey was installed in 'Test' database
_And _ I executed a script against 'Test' database:
```Sql
CREATE OR ALTER PROC AddProduct @name nvarchar(255)
AS
BEGIN
SELECT @name as [Name];
END
```
_And _ I expose the procedure with sql statement on 'Test' database:
```Sql
EXEC webapi_BindStoredProc 'AddProduct','Test';
```
_And _ I publish WebApi on 'Test' database with sql statement:
```Sql
EXEC webapi_Publish;
```
_When _ I restart WebApiHost
_And _ I invoke WebApi with 'POST' request on 'api/Product' with data '{"name":"iPhone"}'
_Then _ I expect a response from url 'api/Product' with data '{"name":"iPhone"}'
##I want to publish changes while app is running

