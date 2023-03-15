@messageBus
@employerIncentivesApi
Feature: EarningsResilienceCheck
	When earnings resilience check job is triggered
	Then affected apprenticeships are updated in the Employer Incentives system

#Scenario: An earnings resilience check is triggered
#	When an earnings resilience check is triggered
#	Then the Employer Incentives API is called to update apprenticeships
#
