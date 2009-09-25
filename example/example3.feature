Feature: Authenticate Provider Portal User login

Background:
Given the Provider Portal User "sgpc.admin" exists
And the Provider Portal User "sgpc.admin" has a password equal to "qa" 

Scenario: Enter an active login to the provider portal
Given the Provider Portal User "sgpc.admin" is active
And I am on the "Login" Screen
When I enter "sgpc.admin" in the "Username" field
And I enter "qa" in the "Password" field
And I push "Login" button
Then the "Available Locations List" is enabled

Scenario: Enter an inactive login to the provider portal ten times
Given the Provider Portal User "sgpc.admin" is not active
And I am on the "Login" Screen
When I enter "sgpc.admin" in the "Username" field
And I enter "abadpassword" in the "Password" field
And I push "Login" button
And an alert saying "Authentication failed. Please enter a valid Username and Password and try again." is displayed each attempt
Then the "Available Locations List" is not enabled

Scenario: Enter a login that is not in the system ten times
Given I am on the "Login" Screen
When I enter "sgpc.junk" in the "Username" field
And I enter "blah" in the "Password" field
And I push "Login" button
And an alert saying "Authentication failed. Please enter a valid Username and Password and try again." is displayed each attempt
Then the "Available Locations List" is not enabled
