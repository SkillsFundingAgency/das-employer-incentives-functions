@messageBus
@employerIncentivesApi
Feature: ReinstatePayments
	When a support user wants to reinstate an archived payment for an apprenticeship incentive
	Then the request is forwarded to the EmployerIncentives system

#Scenario: a Reinstate Payments request is requested
#	When a reinstate payments request is received
#	Then the reinstate payments request is forwarded to the Employer Incentives API
#
#Scenario: a Reinstate Payments request is requested but the payment is not found
#	When a reinstate request is received but no matching payment is found
#	Then the user receives a Bad Request response
