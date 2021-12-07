@messageBus
@employerIncentivesApi
Feature: RefreshEmploymentChecks
	When the refresh employment checks job is triggered
	Then affected apprenticeships are updated in the Employer Incentives system

Scenario: An employment checks refresh is triggered
	When an employment checks refresh is triggered
	Then the Employer Incentives API is called to update employment checks for apprenticeships

