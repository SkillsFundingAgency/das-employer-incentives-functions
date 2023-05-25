@messageBus
@employerIncentivesApi
Feature: PaymentApproval
	When a trigger of Payment Approval is requested
	Then it is forwarded to the EmployerIncentives system

Scenario: A job to trigger payment approval is requested
	When a request to trigger payment approval is received	
	Then the request is forwarded to the Employer Incentives system


