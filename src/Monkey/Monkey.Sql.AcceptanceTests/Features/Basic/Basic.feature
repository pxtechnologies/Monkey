Feature: Basic
	In order to avoid silly mistakes
	As a backend-developer
	I want to be told how to get started with Monkey.WebApi
	And expose my backend as webapi easily

Background: 
	Given I have my system configured with SqlServer
	And I configured basic WebApi features with swagger
	

Scenario: I can expose stored procedure to WebApi
	Given I have a stored procedure with name 'AddUser' in 'Test' database
	| Sql Line                                     |
	| CREATE PROCEDURE AddUser @name nvarchar(255) |
	| AS                                           |
	| BEGIN                                        |
	| SELECT @name + '!' as Name                   |
	| END                                          |

	And I have mapped 'AddUser' procedure from 'Test' database in apidatabase in schema 'dbo'
	And I have mapped parameter '@name' of type 'nvarchar' to property 'Name' of type 'string' in object command 'AddUserCommand'
	And I have mapped resultset to object 'UserEntity':
	| SqlColumnName | SqlColumnType | ObjectPath | ParentObjectName | ObjectType |
	| Name          | nvarchar      | Name       | UserEntity       | string     |