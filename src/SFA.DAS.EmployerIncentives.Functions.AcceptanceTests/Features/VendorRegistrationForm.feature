@employerIncentivesApi
@messageBus
Feature: VendorRegistrationForm
	When VendorRegistrationForm (VRF) status update job is triggered
	Then affected legal entities are updated in the Employer Incentives system

Scenario: A VRF case status update job is triggered
	When a VRF case status update job is triggered
	Then the Employer Incentives API is called to update Legal Entities

