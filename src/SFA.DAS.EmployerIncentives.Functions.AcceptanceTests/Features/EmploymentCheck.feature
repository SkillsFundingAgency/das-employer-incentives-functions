@messageBus
@employerIncentivesApi
Feature: EmploymentCheck
	When am employment check state changes
	Then is is forwarded to the EmployerIncentives system

Scenario Outline: An employment check result is received
	When an employment check result is received with <EmploymentResult> and <ErrorType> 
	Then the event is forwarded to the Employer Incentives system

Examples:
    | EmploymentResult | ErrorType           |
    | true             | null                |
    | false            | null                |
    |                  | NinoNotFound        |
    |                  | NinoFailure         |
    |                  | NinoInvalid         |
    |                  | PAYENotFound        |
    |                  | PAYEFailure         |
    |                  | NinoAndPAYENotFound |
    |                  | HmrcFailure         |

Scenario: An employment check result is received with an unhandled result
	When an employment check result is received with an invalid error type
	Then the event is not forwarded to the Employer Incentives system

Scenario: An employment check refresh is requested by support
	When an employment check refresh request is received
	Then the request is forwarded to the Employer Incentives system