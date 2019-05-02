Feature: ErrorHandling
	In order to avoid silly mistakes
	As a SQL dev
	I want to be told how to API handles SQL Exceptions

Background: 
	Given the 'Test' database is created
	And the 'Monkey' database is created
	And WebApiHost has started
	And Monkey was installed in 'Test' database

Scenario Outline: I want to return error response according to HTTP conventions
	Given I executed a script against 'Test' database:
	| Sql                                                 |
	| CREATE OR ALTER PROC AddProduct @name nvarchar(255) |
	| AS                                                  |
	| BEGIN                                               |
	| THROW <ErrorCode>, '<ErrorMessage>', <ErrorState>;  |
	| END                                                 |

	And I expose the procedure with sql statement on 'Test' database:
	| Sql                                                         |
	| EXEC webapi_BindStoredProc 'AddProduct','Test'; |
	
	When I publish WebApi on 'Test' database with sql statement:
	| Sql              |
	| EXEC webapi_Publish; |

	And I invoke WebApi with 'POST' request on 'api/Product' with data '{"name":"iPhone"}'
	Then I expect a response from url 'api/Product' with http-code '<HttpErrorCode>' and data '<PayloadResponse>'
	
Examples: 
	| ErrorCode | ErrorMessage           | ErrorState | PayloadResponse                        | HttpErrorCode |
	| 50401     | Funny message          | 2          | {"code":2,"message":"Funny message"}   | 401           |
	| 50402     | Another message        | 1          | {"code":1,"message":"Another message"} | 402           |
	| 50401     | <custom>Hello</custom> | 255        | {"custom":"Hello"}                     | 401           |
	
