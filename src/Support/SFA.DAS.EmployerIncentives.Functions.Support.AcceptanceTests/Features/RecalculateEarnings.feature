@messageBus
@employerIncentivesApi
Feature: RecalculateEarnings
	When recalculate earnings job is triggered
	Then the earnings for affected apprenticeships are recalculated in the Employer Incentives system

Scenario: A recalculate earnings request is triggered
	When a recalculate earnings request is triggered
	Then the Employer Incentives API is called to recalculate earnings for the specified learners

