Feature: SqlConfigurationInterface
	In order to avoid silly mistakes
	As a SQL idiot
	I want to be told how to configure API

Background: 
	Given the 'Test' database is created
	And the 'Monkey' database is created
	And WebApiHost has started
	And Monkey was installed in 'Test' database

Scenario Outline: I want to map renamed procedure according to REST conventions
	Given I executed a script against 'Test' database:
	| SqlLine                                                                        |
	| CREATE OR ALTER PROC Ping <ParamName> <ParamType>, <ParamName2> <ParamType2>   |
	| AS                                                                             |
	| BEGIN                                                                          |
	| SELECT <ParamName> as <ResultColumnName>, <ParamName2> as <ResultColumnName2>; |
	| END                                                                            |

	And I expose the procedure with sql statement on 'Test' database:
	| SqlLine                                                         |
	| EXEC webapi_BindStoredProc 'Ping','Test','dbo','<HandlerName>'; |
	
	When I publish WebApi on 'Test' database with sql statement:
	| SqlLine              |
	| EXEC webapi_Publish; |

	And I invoke WebApi with '<HttpMethod>' request on '<Url>' with data '<RequestPayload>'
	Then I expect a response from url '<Url>' with data '<ResponsePayload>'
	
Examples: 
	| HandlerName   | ParamType     | ParamName | ParamName2 | ParamType2 | ResultColumnName | ResultColumnName2 | HttpMethod | Url            | RequestPayload             | ResponsePayload            |
	| AddProduct    | nvarchar(255) | @name     | @number    | int        | Name             | Number            | POST       | api/Product    | {"name":"pc","number":123} | {"name":"pc","number":123} |
	| CreateProduct | nvarchar(255) | @name     | @number    | int        | Name             | Number            | POST       | api/Product    | {"name":"pc","number":123} | {"name":"pc","number":123} |
	| InsertProduct | nvarchar(255) | @name     | @number    | int        | Name             | Number            | POST       | api/Product    | {"name":"pc","number":123} | {"name":"pc","number":123} |
	| ModifyProduct | nvarchar(255) | @id       | @number    | int        | Name             | Number            | PUT        | api/Product/pc | {"number":123}             | {"name":"pc","number":123} |
	| EditProduct   | nvarchar(255) | @id       | @number    | int        | Name             | Number            | PUT        | api/Product/pc | {"number":123}             | {"name":"pc","number":123} |
	| UpdateProduct | nvarchar(255) | @id       | @number    | int        | Name             | Number            | PUT        | api/Product/pc | {"number":123}             | {"name":"pc","number":123} |

Scenario Outline: I want to map and rename procedure according to REST conventions
	Given I executed a script against 'Test' database:
	| SqlLine                                                                        |
	| CREATE OR ALTER PROC Ping <ParamName> <ParamType>, <ParamName2> <ParamType2>   |
	| AS                                                                             |
	| BEGIN                                                                          |
	| SELECT <ParamName> as <ResultColumnName>, <ParamName2> as <ResultColumnName2>; |
	| END                                                                            |

	And I expose the procedure with sql statement on 'Test' database:
	| SqlLine                                     |
	| EXEC webapi_BindStoredProc 'Ping','Test' |

	And I rename the binding with sql statement on 'Test' database:
	| SqlLine                                     |
	| EXEC webapi_Rename 'Ping','Test','dbo','handler','<HandlerName>' |
	
	When I publish WebApi on 'Test' database with sql statement:
	| SqlLine              |
	| EXEC webapi_Publish; |

	And I invoke WebApi with '<HttpMethod>' request on '<Url>' with data '<RequestPayload>'
	Then I expect a response from url '<Url>' with data '<ResponsePayload>'
	
Examples: 
	| HandlerName   | ParamType     | ParamName | ParamName2 | ParamType2 | ResultColumnName | ResultColumnName2 | HttpMethod | Url            | RequestPayload             | ResponsePayload            |
	| AddProduct    | nvarchar(255) | @name     | @number    | int        | Name             | Number            | POST       | api/Product    | {"name":"pc","number":123} | {"name":"pc","number":123} |
	| CreateProduct | nvarchar(255) | @name     | @number    | int        | Name             | Number            | POST       | api/Product    | {"name":"pc","number":123} | {"name":"pc","number":123} |
	| InsertProduct | nvarchar(255) | @name     | @number    | int        | Name             | Number            | POST       | api/Product    | {"name":"pc","number":123} | {"name":"pc","number":123} |
	| ModifyProduct | nvarchar(255) | @id       | @number    | int        | Name             | Number            | PUT        | api/Product/pc | {"number":123}             | {"name":"pc","number":123} |
	| EditProduct   | nvarchar(255) | @id       | @number    | int        | Name             | Number            | PUT        | api/Product/pc | {"number":123}             | {"name":"pc","number":123} |
	| UpdateProduct | nvarchar(255) | @id       | @number    | int        | Name             | Number            | PUT        | api/Product/pc | {"number":123}             | {"name":"pc","number":123} |
