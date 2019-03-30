Feature: Basic
	In order to avoid silly mistakes
	As a backend-developer
	I want to be told how to get started with Monkey.WebApi
	And expose my backend as webapi easily

Background: 
	Given I have writen my model with IRequestHandler pattern
	And I configured basic WebApi features with swagger
	

Scenario: I can expose add operation in WebApi
	Given I add dynamic api to mvc 
	
	And I have written 'AddUserCommandHandler' that accepts 'AddUserCommand' and returns 'UserEntity' in 'Users' namespace
	And I have written 'AddUserCommand' with properties
	| PropertyName | PropertyType |
	| Name         | string       |
	And I have written 'UserEntity' 
	| PropertyName | PropertyType |
	| Name         | string       |

	When I run the application

	Then Controller 'UsersController' is exposed
	And Simple 'Post' action is explosed 

	When I make a 'Post' request to 'api/Users' with payload:
	| PropertyName | PropertyType | PropertyValue |
	| Name         | string       | John          |

	Then RequestHandler 'AddUserCommandHandler' is invoked


Scenario: I can expose update operation in WebApi
	Given I add dynamic api to mvc 
	
	And I have written 'UpdateUserCommandHandler' that accepts 'id','UpdateUserCommand' and returns 'UserEntity' in 'Users' namespace
	And I have written 'UpdateUserCommand' with properties
	| PropertyName | PropertyType |
	| Name         | string       |
	And I have written 'UserEntity' 
	| PropertyName | PropertyType |
	| Name         | string       |

	When I run the application

	Then Controller 'UsersController' is exposed
	And Simple 'Put' action is explosed 

	When I make a 'Put' request to 'api/Users/{id}' with payload:
	| PropertyName | PropertyType | PropertyValue |
	| Name         | string       | John          |

	Then RequestHandler 'UpdateUserCommandHandler' is invoked

Scenario: I can expose custom operation in WebApi
	Given I add dynamic api to mvc 
	
	And I have written 'AcceptUserCommandHandler' that accepts 'id','AcceptUserCommand' and returns 'UserEntity' in 'Users' namespace
	And I have written 'AcceptUserCommand' with properties
	| PropertyName | PropertyType |
	| Name         | string       |
	And I have written 'UserEntity' 
	| PropertyName | PropertyType |
	| Name         | string       |

	When I run the application

	Then Controller 'UsersController' is exposed
	And Extended 'Post' action is explosed under 'api/Users/{id}/Accept'

	When I make a 'Post' request to 'api/Users/{id}/Accept' with payload:
	| PropertyName | PropertyType | PropertyValue |
	| Name         | string       | John          |

	Then RequestHandler 'AcceptUserCommandHandler' is invoked