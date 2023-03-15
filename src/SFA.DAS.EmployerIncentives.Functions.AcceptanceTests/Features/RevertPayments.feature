@messageBus
@employerIncentivesApi
Feature: RevertPayments
	When a support user wants to revert a failed payment for an apprenticeship incentive
	Then the request is forwarded to the EmployerIncentives system

#Scenario: a Revert Payments request is requested
#	When a revert payments request is received
#	Then the revert payments request is forwarded to the Employer Incentives API
#
#Scenario: a Revert Payments request is requested but the payment is not found
#	When a revert request is received but no matching payment is found
#	Then the user receives a Bad Request response
