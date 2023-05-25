@messageBus
@employerIncentivesApi
Feature: PaymentValidation
	When a trigger of Payment Validation is requested
	Then it is forwarded to the EmployerIncentives system

Scenario: A job to trigger payment validation is requested
	When a request to trigger payment validation is received	
	Then the request is forwarded to the Employer Incentives system


