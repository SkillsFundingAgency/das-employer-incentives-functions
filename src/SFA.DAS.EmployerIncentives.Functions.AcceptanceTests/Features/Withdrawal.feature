@messageBus
@employerIncentivesApi
Feature: Withdrawal
	When a support user wants to withdraw an apprenticeship incentive
	Then the request is forwarded to the EmployerIncentives system

Scenario: an EmployerWithdrawal request is requested
	When an employer withdrawal request is received
	Then the withdrawal request is forwarded to the Employer Incentives API

Scenario: a ComplianceWithdrawal request for a single application is requested
	When a compliance withdrawal request for a single application is received
	Then the withdrawal request is forwarded to the Employer Incentives API

	Scenario: a ComplianceWithdrawal request for multiple applications is requested
	When a compliance withdrawal request with multiple applications is received
	Then the withdrawal request is forwarded to the Employer Incentives API

Scenario: a ReinstateApplication request is requested
	When a reinstate application request is received
	Then the reinstate application request is forwarded to the Employer Incentives API