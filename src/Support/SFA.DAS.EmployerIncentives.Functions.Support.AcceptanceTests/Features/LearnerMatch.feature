@messageBus
@employerIncentivesApi
Feature: LearnerMatch
	When a refresh of the Learner Match is requested
	Then it is forwarded to the EmployerIncentives system

Scenario: A job to refresh learner match is requested
	When a request to refresh learner match is received	
	Then the request is forwarded to the Employer Incentives system


