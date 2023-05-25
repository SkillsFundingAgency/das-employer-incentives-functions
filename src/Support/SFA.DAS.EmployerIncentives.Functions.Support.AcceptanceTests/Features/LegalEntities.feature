@messageBus
@employerIncentivesApi
Feature: LegalEntities
	When a legal entity state changes
	Then is is forwarded to the EmployerIncentives system

Scenario: A job to load all legal entities is requested
	When a request to refresh legal entities is received	
	Then the request is forwarded to the Employer Incentives system


