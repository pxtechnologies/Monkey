Feature: PrimitiveInvocations
	In order to avoid silly mistakes
	As a SQL developer
	I want to be told how to expose simple stored procedures as web-api

Background: 
	Given the 'Test' database is created
	And the 'Monkey' database is created
	And WebApiHost has started
	And Monkey was installed in 'Test' database

Scenario Outline: I want to invoke stored procedure with different primitive parameters
	Given I executed a script against 'Test' database:
	| SqlLine                                                      |
	| CREATE OR ALTER PROC <ProcedureName> <ParamName> <ParamType> |
	| AS                                                           |
	| BEGIN                                                        |
	| SELECT <ParamName> as <ResultColumnName>;                    |
	| END                                                          |

	And I expose the procedure with sql statement on 'Test' database:
	| SqlLine                                                |
	| EXEC sp_WebApi_AddStoredProc '<ProcedureName>','Test'; |

	When I publish WebApi on 'Monkey' database with sql statement:
	| SqlLine                 |
	| EXEC sp_WebApi_Publish; |

	And I invoke WebApi with '<HttpMethod>' request on '<Url>' with data '<RequestPayload>'
	Then I expect a response from url '<Url>' with data '<ResponsePayload>'
	
Examples: 
	| ProcedureName | ParamType        | ParamName | ResultColumnName | HttpMethod | Url          | RequestPayload                                 | ResponsePayload                                     |
	| AddProduct    | tinyint          | @number   | ResultNumber     | POST       | api/Product/ | {"number":123}                                 | {"resultNumber":123}                                |
	| AddProduct    | smallint         | @number   | ResultNumber     | POST       | api/Product/ | {"number":123}                                 | {"resultNumber":123}                                |
	| AddProduct    | int              | @number   | ResultNumber     | POST       | api/Product/ | {"number":123}                                 | {"resultNumber":123}                                |
	| AddProduct    | bigint           | @number   | ResultNumber     | POST       | api/Product/ | {"number":123}                                 | {"resultNumber":123}                                |
	| AddProduct    | numeric(10,2)    | @number   | ResultNumber     | POST       | api/Product/ | {"number":123.5}                               | {"resultNumber":123.50}                             |
	| AddProduct    | money            | @number   | ResultNumber     | POST       | api/Product/ | {"number":123.5}                               | {"resultNumber":123.5000}                           |
	| AddProduct    | smallmoney       | @number   | ResultNumber     | POST       | api/Product/ | {"number":123.5}                               | {"resultNumber":123.5000}                           |
	| AddProduct    | real             | @number   | ResultNumber     | POST       | api/Product/ | {"number":123.1}                               | {"resultNumber":123.1}                              |
	| AddProduct    | float            | @number   | ResultNumber     | POST       | api/Product/ | {"number":123.1}                               | {"resultNumber":123.1}                              |
	| AddProduct    | nvarchar(255)    | @name     | ResultName       | POST       | api/Product/ | {"name":"John"}                                | {"resultName":"John"}                               |
	| AddProduct    | varchar(255)     | @name     | ResultName       | POST       | api/Product/ | {"name":"John"}                                | {"resultName":"John"}                               |
	| AddProduct    | char(255)        | @name     | ResultName       | POST       | api/Product/ | {"name":"John"}                                | {"resultName":"John"}                               |
	| AddProduct    | nchar(255)       | @name     | ResultName       | POST       | api/Product/ | {"name":"John"}                                | {"resultName":"John"}                               |
	| AddProduct    | text             | @name     | ResultName       | POST       | api/Product/ | {"name":"John"}                                | {"resultName":"John"}                               |
	| AddProduct    | ntext            | @name     | ResultName       | POST       | api/Product/ | {"name":"John"}                                | {"resultName":"John"}                               |
	| AddProduct    | decimal          | @value    | ResultValue      | POST       | api/Product/ | {"value":"1.0"}                                | {"resultValue":1.0}                                 |
	| AddProduct    | uniqueidentifier | @sku      | ResutSku         | POST       | api/Product/ | {"sku":"B915B92A-8E13-4763-8F4B-2DDF5CE09076"} | {"resutSku":"b915b92a-8e13-4763-8f4b-2ddf5ce09076"} |
	| AddProduct    | time             | @time     | ResultTime       | POST       | api/Product/ | {"time":"11:22"}                               | {"resultTime":"11:22:00"}                           |
	| AddProduct    | datetime         | @date     | ResultDate       | POST       | api/Product/ | {"date":"2019-04-01 11:22"}                    | {"resultDate":"2019-04-01T11:22:00"}                |
	| AddProduct    | datetime2        | @date     | ResultDate       | POST       | api/Product/ | {"date":"2019-04-01 11:22"}                    | {"resultDate":"2019-04-01T11:22:00"}                |
	| AddProduct    | datetimeoffset   | @date     | ResultDate       | POST       | api/Product/ | {"date":"2019-04-01 11:22"}                    | {"resultDate":"2019-04-01T11:22:00+00:00"}                |

Scenario Outline: I want to map stored procedure according to REST conventions
	Given I executed a script against 'Test' database:
	| SqlLine                                                                                 |
	| CREATE OR ALTER PROC <ProcedureName> <ParamName> <ParamType>, <ParamName2> <ParamType2> |
	| AS                                                                                      |
	| BEGIN                                                                                   |
	| SELECT <ParamName> as <ResultColumnName>, <ParamName2> as <ResultColumnName2>;          |
	| END                                                                                     |

	And I expose the procedure with sql statement on 'Test' database:
	| SqlLine                                                |
	| EXEC sp_WebApi_AddStoredProc '<ProcedureName>','Test'; |

	When I publish WebApi on 'Monkey' database with sql statement:
	| SqlLine                 |
	| EXEC sp_WebApi_Publish; |

	And I invoke WebApi with '<HttpMethod>' request on '<Url>' with data '<RequestPayload>'
	Then I expect a response from url '<Url>' with data '<ResponsePayload>'
	
Examples: 
	| ProcedureName | ParamType     | ParamName | ParamName2 | ParamType2 | ResultColumnName | ResultColumnName2 | HttpMethod | Url            | RequestPayload             | ResponsePayload            |
	| AddProduct    | nvarchar(255) | @name     | @number    | int        | Name             | Number            | POST       | api/Product    | {"name":"pc","number":123} | {"name":"pc","number":123} |
	| CreateProduct | nvarchar(255) | @name     | @number    | int        | Name             | Number            | POST       | api/Product    | {"name":"pc","number":123} | {"name":"pc","number":123} |
	| InsertProduct | nvarchar(255) | @name     | @number    | int        | Name             | Number            | POST       | api/Product    | {"name":"pc","number":123} | {"name":"pc","number":123} |
	| ModifyProduct | nvarchar(255) | @id       | @number    | int        | Name             | Number            | PUT        | api/Product/pc | {"number":123}             | {"name":"pc","number":123} |
	| EditProduct   | nvarchar(255) | @id       | @number    | int        | Name             | Number            | PUT        | api/Product/pc | {"number":123}             | {"name":"pc","number":123} |
	| UpdateProduct | nvarchar(255) | @id       | @number    | int        | Name             | Number            | PUT        | api/Product/pc | {"number":123}             | {"name":"pc","number":123} |


Scenario: I want to pass nulls and retrive nulls
	Given I executed a script against 'Test' database:
	| SqlLine                                                 |
	| CREATE OR ALTER PROC AddProduct @name nvarchar(255)     |
	| AS                                                      |
	| BEGIN                                                   |
	| if @name is not null throw 51000, 'name is not null',1; |
	| SELECT 'Tv' as Name, null as Company;                   |
	| END                                                     |

	And I expose the procedure with sql statement on 'Test' database:
	| SqlLine                                           |
	| EXEC sp_WebApi_AddStoredProc 'AddProduct','Test'; |

	When I publish WebApi on 'Monkey' database with sql statement:
	| SqlLine                 |
	| EXEC sp_WebApi_Publish; |

	And I invoke WebApi with 'POST' request on 'api/Product' with data '{}'
	Then I expect a response from url 'api/Product' with data '{"name":"Tv","company":null}'

Scenario: I want to retrive many records from procedure execution
Given I executed a script against 'Test' database:
	| SqlLine                                              |
	| CREATE OR ALTER PROC GetProducts @name nvarchar(255) |
	| AS                                                   |
	| BEGIN                                                |
	| SELECT @name as Name                                 |
	| UNION ALL SELECT 'Two'                               |
	| END                                                  |

	And I expose the procedure with sql statement on 'Test' database:
	| SqlLine                                            |
	| EXEC sp_WebApi_AddStoredProc 'GetProducts','Test'; |

	When I publish WebApi on 'Monkey' database with sql statement:
	| SqlLine                 |
	| EXEC sp_WebApi_Publish; |

	And I invoke WebApi with 'GET' request on 'api/Product' with data '{"name":"tv"}'
	Then I expect a response from url 'api/Product' with data '[{"name":"tv"},{"name":"Two"}]'