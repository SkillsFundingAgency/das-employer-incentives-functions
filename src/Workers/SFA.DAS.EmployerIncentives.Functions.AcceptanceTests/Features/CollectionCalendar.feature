@messageBus
@employerIncentivesApi
Feature: CollectionCalendar
	When collection calendar period update job is triggered
	Then the active period is changed to the new period

Scenario: A collection calendar period update is triggered
	When a collection calendar period update is triggered
	Then the Employer Incentives API is called to update the active period

