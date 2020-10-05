@messageBus
@employerIncentivesApi
Feature: EmployerVendorId
	When AddEmployerVendorId job is triggered
	Then affected legal entities are updated in the Employer Incentives system

Scenario: the AddEmployerVendorId job is triggered
	When the AddEmployerEmployerVendorId command triggered
	Then the Employer Incentives API is called to update the Legal Entities Employer Vendor Id