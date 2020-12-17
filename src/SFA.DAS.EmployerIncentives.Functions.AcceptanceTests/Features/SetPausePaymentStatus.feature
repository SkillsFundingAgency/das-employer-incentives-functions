@messageBus
@employerIncentivesApi
Feature: SetPausePaymentsStatus
	When a support user wants to set the pause status of an apprenticeship incentive
	Then the request is forwarded to the EmployerIncentives system

Scenario: a Pause Payments request is requested
	When a pause payments request is received
	Then the set pause payments status request is forwarded to the Employer Incentives API

Scenario: a Resume Payments request is requested
	When a resume payments request is received
	Then the set pause payments status request is forwarded to the Employer Incentives API
