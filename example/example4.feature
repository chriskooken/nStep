@aTag
Feature: This is a test feature
I want to test some stuff here
More text goes here
And Adam is cool

Background:
	Given user "a@aol.com" is logged in

Scenario: When I search for dogs
	Given I am on the google homepage
	When I type "dogs" in the "search" field:
		| CITY	    | VALUE	|
		| Roswell	| GA	|
		| Wheaton	| IL	|
		| Auburn	| AL	|

	Then all is well
	And that is all

