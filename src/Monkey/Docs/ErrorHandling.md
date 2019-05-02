# Error handling

	In order to avoid silly mistakes
	As a SQL dev
	I want to be told how to API handles SQL Exceptions
## Background: 
## 
**_Given_** the 'Test' database is created<br />
**_And_** the 'Monkey' database is created<br />
**_And_** WebApiHost has started<br />
**_And_** Monkey was installed in 'Test' database<br />
## I want to return error response according to HTTP conventions
**_Given_** I executed a script against 'Test' database:<br />
```Sql
CREATE OR ALTER PROC AddProduct @name nvarchar(255)
AS
BEGIN
THROW <ErrorCode>, '<ErrorMessage>', <ErrorState>;
END
```
**_And_** I expose the procedure with sql statement on 'Test' database:<br />
```Sql
EXEC webapi_BindStoredProc 'AddProduct','Test';
```
**_When_** I publish WebApi on 'Test' database with sql statement:<br />
```Sql
EXEC webapi_Publish;
```
**_And_** I invoke WebApi with 'POST' request on 'api/Product' with data '{"name":"iPhone"}'<br />
**_Then_** I expect a response from url 'api/Product' with http-code '**\<HttpErrorCode\>**' and data '**\<PayloadResponse\>**'<br />
### Examples:
| Error code | Error message | Error state | Payload response | Http error code| 
| --- | --- | --- | --- | ---| 
| 50401 | Funny message | 2 | {"code":2,"message":"Funny message"} | 401| 
| 50402 | Another message | 1 | {"code":1,"message":"Another message"} | 402| 
| 50401 | **\<custom\>**Hello**\</custom\>** | 255 | {"custom":"Hello"} | 401| 
