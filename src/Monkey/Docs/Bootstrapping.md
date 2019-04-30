# Bootstrapping

## Background: 
## 
**_Given_** the 'Test' database is created<br />
**_And_** the 'Monkey' database is created<br />
**_And_** WebApiHost has started<br />
**_And_** Monkey was installed in 'Test' database<br />
## I want to restart app and load assembly from database
**_Given_** I executed a script against 'Test' database:<br />
```Sql
CREATE OR ALTER PROC AddProduct @name nvarchar(255)
AS
BEGIN
SELECT @name as [Name];
END
```
**_And_** I expose the procedure with sql statement on 'Test' database:<br />
```Sql
EXEC webapi_BindStoredProc 'AddProduct','Test';
```
**_And_** I publish WebApi on 'Test' database with sql statement:<br />
```Sql
EXEC webapi_Publish;
```
**_When_** I restart WebApiHost<br />
**_And_** I invoke WebApi with 'POST' request on 'api/Product' with data '{"name":"iPhone"}'<br />
**_Then_** I expect a response from url 'api/Product' with data '{"name":"iPhone"}'<br />
## I want to publish changes while app is running
**_Given_** I executed a script against 'Test' database:<br />
```Sql
CREATE OR ALTER PROC AddProduct @name nvarchar(255)
AS
BEGIN
SELECT @name as [Name];
END
```
**_And_** I expose the procedure with sql statement on 'Test' database:<br />
```Sql
EXEC webapi_BindStoredProc 'AddProduct','Test';
```
**_And_** I publish WebApi on 'Test' database with sql statement:<br />
```Sql
EXEC webapi_Publish;
```
**_And_** I executed a script against 'Test' database:<br />
```Sql
CREATE OR ALTER PROC EditProduct @id int, @name nvarchar(255)
AS
BEGIN
SELECT @name as [Name];
END
```
**_And_** I expose the procedure with sql statement on 'Test' database:<br />
```Sql
EXEC webapi_BindStoredProc 'EditProduct','Test';
```
**_When_** I publish WebApi on 'Test' database with sql statement:<br />
```Sql
EXEC webapi_Publish;
```
**_And_** I invoke WebApi with 'PUT' request on 'api/Product/123' with data '{"name":"iPhone"}'<br />
**_Then_** I expect a response from url 'api/Product/123' with data '{"name":"iPhone"}'<br />
