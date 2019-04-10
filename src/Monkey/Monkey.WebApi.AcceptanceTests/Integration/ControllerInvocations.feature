Feature: ControllerInvocations
	In order to avoid silly mistakes
	As a web-api developer
	I want to be be sure that controllers invoke command/query handlers

Background: 
	Given The applications container is configured
	

Scenario: I can invoke create command handler on dynamic controller
	Given I have written command 'CreateUser' and result as:
	| Line | Code                                                                                     |
	| 1    | public class CreateUser { public string Name { get;set; }}                               |
	| 2    | public class UserEntity { public Guid Id { get; set; } public string Name { get; set; }} |
	And I have written command-handler that accepts 'CreateUser' and returns 'UserEntity'
	When I invoke 'UserController' with 'Post' method and 'CreateUserRequest' argument:
	| Line | Json               |
	| 1    | { "Name": "John" } |
	Then CommandHandler is invoked with corresponding 'CreateUser' argument
	And 'UserEntityResponse' that corresponds to 'UserEntity' is returned

	
Scenario: I can invoke update command handler on dynamic controller
	Given I have written command 'UpdateUser' and result as:
	| Line | Code                                                                                     |
	| 1    | public class UpdateUser { public string Name { get;set; } public Guid Id {get; set; }}   |
	| 2    | public class UserEntity { public Guid Id { get; set; } public string Name { get; set; }} |
	And I have written command-handler that accepts 'UpdateUser' and returns 'UserEntity'
	When I found record with id to update
	And I invoke 'UserController' with 'Put' method and 'UpdateUserRequest' argument:
	| Line | Json               |
	| 1    | { "Name": "John" } |
	Then CommandHandler is invoked with corresponding 'UpdateUser' argument
	And 'UserEntityResponse' that corresponds to 'UserEntity' is returned