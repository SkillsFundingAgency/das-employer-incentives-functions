@messageBus
@employerIncentivesApi
Feature: EmploymentCheck
	When am employment check state changes
	Then is is forwarded to the EmployerIncentives system

Scenario Outline: An employment check result is received
	When an employment check result is received with result <checkresult>
	Then the event is forwarded to the Employer Incentives system

Examples:
    | checkresult      |
    | Employed         |
    | Not Employed     |
    | HMRC Unknown     |
    | No NINO Found    |
    | No Account Found |


Scenario: An employment check result is received with an unhandled result
	When an employment check result is received with result Invalid
	Then the event is not forwarded to the Employer Incentives system