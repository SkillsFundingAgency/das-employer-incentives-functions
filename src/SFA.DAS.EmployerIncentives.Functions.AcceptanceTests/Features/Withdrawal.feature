@messageBus
@employerIncentivesApi
Feature: Withdrawal
	When a support user wants to withdraw an apprenticeship incentive
	Then the request is forwarded to the EmployerIncentives system

Scenario: an EmployerWithdrawl request is requested
	When a withdrawal request is received
	Then the withdrawal request is forwarded to the Employer Incentives API