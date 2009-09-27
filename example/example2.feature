Feature: This is a _test_ feature
I want to test some stuff here
Seriously, more shit is happening

#Background:
#Given ProviderLocation "Laurens Office" exists
#And "Laurens Office" has a kiosk called "Kiosk 1"

Scenario: I search for cats
Given I am on the google homepage
When I type "cats" in the "search" field
And I click the "Google Search" button
And I wait for the page to load
Then I should be on the "cats - Google Search" page
When I click the "Search settings" link
And I wait for the page to load
Then I should be on the "Preferences" page
