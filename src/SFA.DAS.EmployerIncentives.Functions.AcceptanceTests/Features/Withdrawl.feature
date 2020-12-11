@messageBus
@employerIncentivesApi
Feature: Withdrawl
	When a support user wants to withdraw an apprenticeship incentive
	Then the request is forwarded to the EmployerIncentives system

Scenario: an EmployerWithdrawl request is requested
	When a withdrawl request is received
	Then the withdrawl request is forwarded to the Employer Incentives API