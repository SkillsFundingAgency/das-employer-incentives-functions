@messageBus
@employerIncentivesApi
Feature: BlockPayments
	When a support user wants to block account legal entities for payments
	Then the request is forwarded to the EmployerIncentives system

Scenario: a Block Payments request is requested
	When a block payments request is received
	Then the block payments request is forwarded to the Employer Incentives API
	And the user receives an OK response