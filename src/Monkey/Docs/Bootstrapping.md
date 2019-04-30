# Bootstrapping

## I want to restart app and load assembly from database

_Given_ the 'Test' database is created
_And_ the 'Monkey' database is created
_And_ WebApiHost has started
_And_ Monkey was installed in 'Test' database
_And_ I executed a script against 'Test' database:
```Sql
CREATE OR ALTER PROC AddProduct @name nvarchar(255)
AS
BEGIN
SELECT @name as [Name];
END
```
_And_ I expose the procedure with sql statement on 'Test' database:
```Sql
EXEC webapi_BindStoredProc 'AddProduct','Test';
```
_And_ I publish WebApi on 'Test' database with sql statement:
```Sql
EXEC webapi_Publish;
```
_When_ I restart WebApiHost
_And_ I invoke WebApi with 'POST' request on 'api/Product' with data '{"name":"iPhone"}'
_Then_ I expect a response from url 'api/Product' with data '{"name":"iPhone"}'
## I want to publish changes while app is running

