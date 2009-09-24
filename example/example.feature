Feature: This is a test feature
I want to test some stuff here
More text goes here

Background:
Given ProviderLocation "Laurens Office" exists
And "Laurens Office" has a kiosk called "Kiosk 1"

Scenario: I search for dogs
Given I am on the google homepage
When I type "dogs" in the "search" field
And I click the "Google Search" button
And I wait for the page to load
Then I should be on the "dogs - Google Search" page
When I click the "Search settings" link
And I wait for the page to load
Then I should be on the "Preferences" page

Scenario: I search for cars
Given I am on the google homepage
When I type "cars" in the "search" field
And I click the "Google Search" button
And I wait for the page to load
Then I should be on the "cars - Google Search" page
When I click the "Search settings" link
And I wait for the page to load
Then I should be on the "Preferences" page

Scenario Outline: I am having fun
#Given My Name is "billy bob"
Given I live at "<CITY>"
Given My city is "Roswell" and my state is "<VALUE>"

Examples:
| CITY	    | VALUE	|
| Roswell	| GA	|
| Wheaton	| IL	|
| Auburn	| AL	|

Scenario: I am doing good
Given My Name is "Billy Bob"
Given I live at "123 Roswell Rd"
Given My city is "Roswell" and my state is "Georgia"
