Feature: Bootstrapping
	In order to avoid silly mistakes
	As a admin idiot
	I want to be told how the deployment works


Scenario: I want to restart app and load assembly from database
	Given the 'Test' database is created
	And the 'Monkey' database is created
	And WebApiHost has started
	And Monkey was installed in 'Test' database

	And I executed a script against 'Test' database:
	| Sql                                             |
	| CREATE OR ALTER PROC AddProduct @name nvarchar(255) |
	| AS                                                  |
	| BEGIN                                               |
	| SELECT @name as [Name];                             |
	| END                                                 |

	And I expose the procedure with sql statement on 'Test' database:
	| Sql                                         |
	| EXEC webapi_BindStoredProc 'AddProduct','Test'; |

	And I publish WebApi on 'Test' database with sql statement:
	| Sql              |
	| EXEC webapi_Publish; |

	When I restart WebApiHost
	And I invoke WebApi with 'POST' request on 'api/Product' with data '{"name":"iPhone"}'

	Then I expect a response from url 'api/Product' with data '{"name":"iPhone"}'

Scenario: I want to publish changes while app is running
