@messageBus
@employerIncentivesApi
Feature: LegalEntities
	When a legal entity state changes
	Then is is forwarded to the EmployerIncentives system

Scenario: A legal entity is added to an account
	When a legal entity is added to an account
	Then the event is forwarded to the Employer Incentives system

Scenario: A legal entity is removed from an account
	When a legal entity is removed from an account
	Then the event is forwarded to the Employer Incentives system

Scenario: A job to load all legal entities is requested
	When a request to refresh legal entities is received	
	Then the request is forwarded to the Employer Incentives system

Scenario: A job to load a page of legal entities is requested
	When a request to refresh a page of legal entities is received
	Then the event is forwarded to the Employer Incentives system

Scenario: A job to load a legal entity is requested
	When a request to refresh a legal entity is received
	Then the event is forwarded to the Employer Incentives system