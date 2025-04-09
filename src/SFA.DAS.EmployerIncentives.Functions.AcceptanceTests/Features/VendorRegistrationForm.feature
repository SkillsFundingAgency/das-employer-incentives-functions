@messageBus
@employerIncentivesApi
Feature: VendorRegistrationForm
	When VendorRegistrationForm (VRF) status update job is triggered
	Then affected legal entities are updated in the Employer Incentives system

Scenario: A VRF case status update job is triggered
	Given the VRF case status update job is not paused
	When a VRF case status update job is triggered
	Then the Employer Incentives API is called to update Legal Entities
	And last job run date time is updated

Scenario: A VRF case status update job is triggered when paused
	Given the VRF case status update job is paused
	When a VRF case status update job is triggered
	Then the Employer Incentives API is not called to update Legal Entities
	And last job run date time is not updated
	And last job run is paused

Scenario: A paused VRF case status update job is resumed
	Given the VRF case status update job is paused
	When the VRF case status update job is resumed
	And a VRF case status update job is triggered
	Then the Employer Incentives API is called to update Legal Entities
	And last job run date time is not updated
	And last job run is not paused