@messageBus
@employerIncentivesApi
Feature: CollectionCalendar
	When collection calendar period activation job is triggered
	Then the active period is changed to the new period

Scenario: A collection calendar period activation is triggered
	When a collection calendar period activation is triggered
	Then the Employer Incentives API is called to update the active period

