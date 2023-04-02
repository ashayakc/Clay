Feature: Lock
For managing doors and access them remotely

Scenario: Open door when user has access
	Given we have a valid user
	And a door created for an office
	And the user is authorized to access the door
	When the user tries to open the door
	Then the result should be 200
	And the door should be opened successfully

Scenario: Open door when user donot have access
	Given we have a valid user
	And a door created for an office
	And the user is not authorized to access the door
	When the user tries to open the door
	Then the result should be 401