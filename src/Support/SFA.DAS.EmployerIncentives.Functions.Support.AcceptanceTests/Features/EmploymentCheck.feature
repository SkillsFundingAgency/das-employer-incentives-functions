@messageBus
@employerIncentivesApi
Feature: EmploymentCheck
	When an employment check refresh is required
	Then the request is forwarded to the Employer Incentives system

Scenario: An employment check refresh is requested by support
	When an <EmploymentCheckType> employment check refresh request is received
	Then the request is forwarded to the Employer Incentives system
Examples: 
| EmploymentCheckType     |
| InitialEmploymentChecks |
| EmployedAt365DaysCheck  |