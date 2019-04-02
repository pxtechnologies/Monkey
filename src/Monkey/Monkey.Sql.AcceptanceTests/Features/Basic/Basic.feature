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
	And I have mapped parameters to command 'AddUserCommand'
	| SqlParameterName | SqlType  | PropertyName | PropertyType |
	| @id              | int      | Id           | int          |
	| @name            | nvarchar | Name         | string       |
	| @birthdate       | datetime | BirthDate    | DateTime     |

	And I have mapped resultset 'UserEntity'
	| SqlColumnName | SqlColumnType | PropertyName | PropertyType |
	| Id            | int           | Id           | int          |
	| Name          | nvarchar      | Name         | string       |
	| BirthDate     | datetime      | BirthDate    | DateTime     |

	When a commandhandler is generated
	And It is executed with command '[ "Id": 1, "Name": "John", "BirthDate": "2019-04-01" ]'