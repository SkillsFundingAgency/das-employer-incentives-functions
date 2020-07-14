Feature: EmployerIncentiveClaimSubmitted
	When an employer incentive claim is submitted
	Then a request to calculate the payment is triggered

Scenario: A claim is submitted successfully
	When a claim is successfully submitted
	Then the account and claim id should be included in the calculation request

Scenario: A claim is submitted with a failure response
	When a claim is unsuccessfully submitted
	Then an error response is returned

Scenario: A claim is submitted with a failure and then a successful response
	When a claim submission fails then is successful
	Then the claim should submit on the second attempt

