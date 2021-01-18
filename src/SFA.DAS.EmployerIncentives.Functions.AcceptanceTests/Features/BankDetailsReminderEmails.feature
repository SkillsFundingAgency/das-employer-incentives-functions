@messageBus
@employerIncentivesApi
Feature: BankDetailsReminderEmails
	When bank details reminder emails job is triggered
	Then accounts with applications without bank details are sent a reminder email

Scenario: A bank details reminder emails job is triggered
	When a bank details reminder emails job is triggered
	Then the Employer Incentives API is called to check for applications where the account has no bank details

