# Sql configuration interface

## I want to map renamed procedure according to REST conventions

_Given_ I executed a script against 'Test' database:
```SqlLine
CREATE OR ALTER PROC Ping <ParamName> <ParamType>, <ParamName2> <ParamType2>
AS
BEGIN
SELECT <ParamName> as <ResultColumnName>, <ParamName2> as <ResultColumnName2>;
END
```
_And_ I expose the procedure with sql statement on 'Test' database:
```SqlLine
EXEC webapi_BindStoredProc 'Ping','Test','dbo','<HandlerName>';
```
_When_ I publish WebApi on 'Test' database with sql statement:
```SqlLine
EXEC webapi_Publish;
```
_And_ I invoke WebApi with **"HttpMethod"** request on **"Url"** with data **"RequestPayload"**
_Then_ I expect a response from url **"Url"** with data **"ResponsePayload"**
### Examples:
| Handler name | Param type | Param name | Param name 2 | Param type 2 | Result column name | Result column name 2 | Http method | Url | Request payload | Response payload| 
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | ---| 
| AddProduct | nvarchar(255) | @name | @number | int | Name | Number | POST | api/Product | {"name":"pc","number":123} | {"name":"pc","number":123}| 
| CreateProduct | nvarchar(255) | @name | @number | int | Name | Number | POST | api/Product | {"name":"pc","number":123} | {"name":"pc","number":123}| 
| InsertProduct | nvarchar(255) | @name | @number | int | Name | Number | POST | api/Product | {"name":"pc","number":123} | {"name":"pc","number":123}| 
| ModifyProduct | nvarchar(255) | @id | @number | int | Name | Number | PUT | api/Product/pc | {"number":123} | {"name":"pc","number":123}| 
| EditProduct | nvarchar(255) | @id | @number | int | Name | Number | PUT | api/Product/pc | {"number":123} | {"name":"pc","number":123}| 
| UpdateProduct | nvarchar(255) | @id | @number | int | Name | Number | PUT | api/Product/pc | {"number":123} | {"name":"pc","number":123}| 
## I want to map and rename procedure according to REST conventions

_Given_ I executed a script against 'Test' database:
```SqlLine
CREATE OR ALTER PROC Ping <ParamName> <ParamType>, <ParamName2> <ParamType2>
AS
BEGIN
SELECT <ParamName> as <ResultColumnName>, <ParamName2> as <ResultColumnName2>;
END
```
_And_ I expose the procedure with sql statement on 'Test' database:
```SqlLine
EXEC webapi_BindStoredProc 'Ping','Test'
```
_And_ I rename the binding with sql statement on 'Test' database:
```SqlLine
EXEC webapi_Rename 'Ping','Test','dbo','handler','<HandlerName>'
```
_When_ I publish WebApi on 'Test' database with sql statement:
```SqlLine
EXEC webapi_Publish;
```
_And_ I invoke WebApi with **"HttpMethod"** request on **"Url"** with data **"RequestPayload"**
_Then_ I expect a response from url **"Url"** with data **"ResponsePayload"**
### Examples:
| Handler name | Param type | Param name | Param name 2 | Param type 2 | Result column name | Result column name 2 | Http method | Url | Request payload | Response payload| 
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | ---| 
| AddProduct | nvarchar(255) | @name | @number | int | Name | Number | POST | api/Product | {"name":"pc","number":123} | {"name":"pc","number":123}| 
| CreateProduct | nvarchar(255) | @name | @number | int | Name | Number | POST | api/Product | {"name":"pc","number":123} | {"name":"pc","number":123}| 
| InsertProduct | nvarchar(255) | @name | @number | int | Name | Number | POST | api/Product | {"name":"pc","number":123} | {"name":"pc","number":123}| 
| ModifyProduct | nvarchar(255) | @id | @number | int | Name | Number | PUT | api/Product/pc | {"number":123} | {"name":"pc","number":123}| 
| EditProduct | nvarchar(255) | @id | @number | int | Name | Number | PUT | api/Product/pc | {"number":123} | {"name":"pc","number":123}| 
| UpdateProduct | nvarchar(255) | @id | @number | int | Name | Number | PUT | api/Product/pc | {"number":123} | {"name":"pc","number":123}| 
