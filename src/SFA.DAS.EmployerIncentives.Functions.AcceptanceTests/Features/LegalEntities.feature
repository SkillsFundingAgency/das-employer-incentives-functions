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

Scenario: An agreement is signed for a legal entity
	When an agreement is signed
	Then the event is forwarded to the Employer Incentives system

Scenario: An application is submitted for a legal entity
	When an application has been submitted for a legal entity
	Then a request is made to the Employer Incentives system

Scenario: A job to update vrf details for a legal entity is requested
	When a request to update legal entity vrf case details is received
	Then the event is forwarded to the Employer Incentives system

Scenario: A job to update vrf case statuses for incomplete applications is requested
	When a request to update vrf case statuses for incomplete applications is received
	Then a request is made to the Employer Incentives system