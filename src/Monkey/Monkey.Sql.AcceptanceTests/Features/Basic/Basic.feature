﻿Feature: Basic
	In order to avoid silly mistakes
	As a sql-developer
	I want to be told how to get started with Monkey.Sql
	And generate command and query handlers easily

Background: 
	Given I have my system configured with SqlServer
	
Scenario: I can invoke stored procedure though query-handler
	Given I have a stored procedure with name 'GetUsers' in 'Test' database
	| Sql Line                                                                       |
	| CREATE PROCEDURE GetUsers @id int, @name nvarchar(255), @birthdate datetime |
	| AS                                                                             |
	| BEGIN                                                                          |
	| SELECT 'One' as Name,  @id as Id, DATEADD(dd,1, @birthdate) as BirthDate        |
	| UNION ALL SELECT 'Two',2, DATEADD(dd,2, @birthdate)                                             |
	| END                                                                            |

	And I have mapped 'GetUsers' procedure from 'Test' database in apidatabase in schema 'dbo'
	And I have mapped parameters to query 'GetUsers'
	| SqlParameterName | SqlType  | PropertyName | PropertyType |
	| @id              | int      | Id           | int          |
	| @name            | nvarchar | Name         | string       |
	| @birthdate       | datetime | BirthDate    | DateTime     |

	And I have mapped resultset 'UserEntity'
	| SqlColumnName | SqlColumnType | PropertyName | PropertyType |
	| Id            | int           | Id           | int          |
	| Name          | nvarchar      | Name         | string       |
	| BirthDate     | datetime      | BirthDate    | DateTime     |

	And I bind that read procedure
	When a queryhandler is generated as 'GetUsersQueryHandler'
	And It is executed with query '{ "Id": 1, "Name": "John", "BirthDate": "2019-04-01" }'
	Then result is: '[{ "Id": 1, "Name": "One", "BirthDate": "2019-04-02" },{ "Id": 2, "Name": "Two", "BirthDate": "2019-04-03" }]'

Scenario: I can invoke stored procedure though command-handler
	Given I have a stored procedure with name 'AddUser' in 'Test' database
	| Sql Line                                                                   |
	| CREATE PROCEDURE AddUser @id int, @name nvarchar(255), @birthdate datetime |
	| AS                                                                         |
	| BEGIN                                                                      |
	| SELECT @name + '!' as Name, @id+1 as Id, DATEADD(dd,1, @birthdate) as BirthDate                                               |
	| END                                                                        |

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

	And I bind that write procedure
	When a commandhandler is generated as 'AddUserCommandHandler'
	And It is executed with command '{ "Id": 1, "Name": "John", "BirthDate": "2019-04-01" }'
	Then result is: '{ "Id": 2, "Name": "John!", "BirthDate": "2019-04-02" }'