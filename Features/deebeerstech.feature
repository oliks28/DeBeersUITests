@RegressionTest
Feature: deebeers technologies functionality 

A short summary of the feature

@deebeers
@testcase:1234
Scenario: Verify that the Search functionality on Deebeers site
	Given the user navigates to the homepage
	When the user hover on about us and click on the technology
	Then Verify the presence of "World-leading Technology" text
	When the user click on the search box and search "DiamondProof"
	Then the user verifies that the result contains 5 items and titles are displayed
	When the user click on the search box and search for "Jwaneng"
	Then the user verifies that the predicted search are listed as follows
		| Suggestions       | Values |
		|-------------------|--------|
		| jwaneng           |    130 |
		| jwaneng mine      |     60 |
		| jwaneng cut       |     28 |
		| jwaneng mines     |     23 |
		| jwaneng orapa     |     12 |
		| jwaneng production|     10 |
		| jwaneng 1982      |      6 |
		| jwaneng hospital  |      5 |
		| jwaneng botswana  |      4 |
		| jwaneng community |      4 |
	
	
