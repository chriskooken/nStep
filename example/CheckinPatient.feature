Feature: Provider Portal Check In Screen Function
  In order to Check in a Patient
  As a Portal User
  I want to Enter Patient Data
  And I want to Submit the Data

Background:
  	Given I am on the "Patient Check-In" page

Scenario Outline: Required Patient Info Data Missing
  	Given "<FIELD>" is blank
  	When I press "Check in Now"
  	Then the page should display a validation message for "<FIELD>"

Examples: Required Fields
  		|FIELD			|
  		|First Name		|
  		|Last Name		|
  		|Date of Birth	|
  		|Gender			|

Scenario: Gender Dropdown Default
    Then "Gender" should be "Please Choose"

Scenario: Gender Dropdown List Order
	Then the "Gender" dropdown should contain:
 		|ITEMS              |
 		|Please Choose		|
 		|Male				|
		|Female				|

Scenario: Primary Relationship to Subscriber Dropdown Default
  	Then "Primary Relationship to Subscriber" should be "Please Choose"

Scenario: Secondary Relationship to Subscriber Dropdown Default
  	Then "Secondary Relationship to Subscriber" should be "Please Choose"

Scenario: Primary Relationship to Subscriber Dropdown List Order
  	Then the "Primary Relationship to Subscriber" dropdown should contain:
 		|RELATIONSHIP    |
 		|Please Choose   |
 		|Subscriber/Self |
 		|Spouse		     |
		|Child		     |
		|Other Adult     |

Scenario: Secondary Relationship to Subscriber Dropdown List Order
  	Then the "Secondary Relationship to Subscriber" dropdown should contain:
 		|RELATIONSHIP    |
 		|Please Choose   |
 		|Subscriber/Self |
 		|Spouse		     |
		|Child		     |
		|Other Adult     |

Scenario Outline: Required Insurance Info Data Missing
  	Given "Self-Pay" is not checked
  	And "<INSURANCE_FIELD>" is blank
	When I press "Check in Now"
  	Then the page should display a validation message for "<INSURANCE_FIELD>"

Examples: Required INSURANCE_FIELDS
  		|INSURANCE_FIELD	|
  		|PrimaryPayer		|
  		|PrimaryPayerID		|

Scenario: Self-Pay is Checked
   	Given "Self-Pay" is not checked
   	When I check "Self-Pay"
   	Then "PrimaryPayer" should be grayed out
   	And "PrimaryPayerID" should be grayed out
   	And "SecondaryPayer" should be grayed out
   	And "SecondaryPayerID" should be grayed out
   	And "Primary Relationship to Subscriber" should be grayed out
   	And "Secondary Relationship to Subscriber" should be grayed out

Scenario: Self-Pay is un Checked
   	Given "Self-Pay" is checked
   	When I uncheck "Self-Pay"
   	Then "PrimaryPayer" should not be grayed out
   	And "PrimaryPayerID" should not be grayed out
   	And "SecondaryPayer" should not be grayed out
   	And "SecondaryPayerID" should not be grayed out
   	And "Primary Relationship to Subscriber" should not be grayed out
   	And "Secondary Relationship to Subscriber" should not be grayed out

Scenario: Self-Pay UnChecked as Default
    Then "Self-Pay" should be unchecked

Scenario: Self-Pay is checked & Check in Now is Pressed
    Given "Self-Pay" is checked
    When I click "Check in Now"
    Then the page should not submit any insurance information with the check in request

Scenario: Home phone only allows phone numbers and auto formats them
	When I type "678-2213112" into "Home Phone"
	Then "Home Phone" should be "(678) 221-3112"

Scenario: Cell phone only allows phone numbers and auto formats them
	When I type "6782213112" into "Cell Phone"
	Then "Cell Phone" should be "(678) 221-3112"

Scenario: Social Security only allows numbers and auto formats them
	When I type "123456789" into "Social Security"
	Then "Social Security" should be "123-45-6789"

Scenario: Zip only allows numbers and auto formats them
	When I type "a30d324a" into "Zip"
	Then "Zip" should be "30324"

Scenario: State only allows letters and formats to 2 chars long
	When I type "GA2G" into "State"
	Then "State" should be "GA"

Scenario: Date of Birth only allows dates and auto formats them
	When I type "05261984" into "Date of Birth"
	Then "Date of Birth" should be "05/26/1984"

Scenario Outline: Date of Birth does not allow age to be greater than 110
	Given CurrentDate is "<DATE>"
	When I type "<DATA>" into "Date of Birth"
	Then the page should display a validation message for "Date of Birth"

Examples: Dates
	|DATE		|DOB		|
	|08/31/2009	|08/30/1898	|
	|01/01/2010	|01/01/1899	|

Scenario Outline: Secondary Insurance Information Entered
	Given "<SECONDARY_FIELD1>" is not blank
    And "<SECONDARY_FIELD2>" is blank
    When I press "Check in Now"
  	Then the page should not submit a check in request
  	And the page should display a validation message for "<SECONDARY_FIELD2>"

Examples: Secondary Insurance Fields
  	|<SECONDARY_FIELD1>	|<SECONDARY_FIELD2>		|
  	|Secondary			|ID						|
  	|ID					|Secondary				|

Scenario Outline: Email entered incorrectly
  	Given "Email" is "<EMAIL>"
  	When I click outside of "Email"
  	Then the page should display a validation message for "Email"

Examples: Email Formats
	|EMAIL			|
	|abc@123		|
	|abc123.com		|
	|@123.com		|

Scenario: Check in Now Pressed - Happy Path
  	Given "First Name" is "John"
  	Given "Last Name" is "Smith"
  	Given "Date of Birth" is "01/01/2000"
  	Given "Gender" is "Male"
  	Given "PrimaryPayer" is "Aetna"
  	Given "PrimaryPayerID" is "80125467"
  	When I click "Check in Now"
  	Then I should be on the patient list
  	And I should see a row like this in the patient list:
  		| Patient    | In  |
  		| John Smith | NOW |


Scenario Outline: Autocomplete the Primary Insurance Fields
    Given the "PrimaryInsurance"? dropdown contains:
   		| Insurance		|
    	|Aetna			|
    	|BCBS of Florida|
    	|BCBS of Georgia|
    	|Century		|
    	|United			|

    When I type "<INPUT>"
    Then the list should autocomplete to "<RESULT>"
Examples: Input & Result
    	|INPUT				|RESULT				|
    	|A					|Aetna				|
    	|Auto				|Auto				|
    	|BCB				|BCBS of Florida	|
		|BCBS of N			|BCBS of N			|
		|BCBS of G			|BCBS of Georgia	|
		|Cen				|Century			|
		|cen				|Century			|
		|CEN				|Century			|
		|cEN				|Century			|
		|Humana				|Humana				|
		|United				|United				|

Scenario Outline: Autocomplete the Secondary Insurance Fields
    Given the "SecondaryInsurance"? dropdown contains:
    	| Insurance		|
    	|Aetna			|
    	|BCBS of Florida|
    	|BCBS of Georgia|
    	|Century		|
    	|United			|

    When I type "<INPUT>"
    Then the list should autocomplete to "<RESULT>"
Examples: Input & Result
   		|INPUT				|RESULT				|
    	|A					|Aetna				|
    	|Auto				|Auto				|
    	|BCB				|BCBS of Florida	|
		|BCBS of N			|BCBS of N			|
		|BCBS of G			|BCBS of Georgia	|
		|Cen				|Century			|
		|cen				|Century			|
		|CEN				|Century			|
		|cEN				|Century			|
		|Humana				|Humana				|
		|United				|United				|

Scenario Outline: Required Insurance Info Data Missing
  	Given "Self-Pay" is not checked
  	And "<INSURANCE_FIELD>" is "<INPUT>"
	When I press "Check in Now"
  	Then the page should display a validation message for "<FIELD>"
Examples: Required INSURANCE_FIELDS
  		|INSURANCE_FIELD			|INPUT			|
  		|Primary					|dummy			|
  		|ID							|dummy			|
 		|Relationship to Subscriber	|Please Choose	|

Scenario: Self-Pay is Checked
   	Given "Self-Pay" is unchecked
   	When I check "Self-Pay"
   	Then "Primary" should be grayed out
   	And "ID" should be grayed out
   	And "Secondary" should be grayed out

Scenario Outline: Secondary Insurance Information Entered
	Given "<SECONDARY_FIELD1>" is "<INPUT>"
    And "<SECONDARY_FIELD2>" is "<INPUT2>"
    When I press "Check in Now"
  	Then the page should display a validation message for "<SECONDARY_FIELD2>"
Examples: Secondary Insurance Fields
  		|<SECONDARY_FIELD1>			|<SECONDARY_FIELD2>			|INPUT			|INPUT2			|
  		|Secondary					|ID							|Aetna			|dummy			|
  		|ID							|Secondary					|123456			|dummy			|
  		|Relationship to Subscriber	|ID							|Male			|dummy			|
  		|Secondary 					|Relationship to Subscriber	|Aetna			|Please Choose	|
  		|ID							|Relationship to Subscriber	|123456			|Please Choose	|
  		|Relationship to Subscriber	|Secondary					|Male			|dummy			|

Scenario: Check in Now Pressed - Happy Path
  	Given "First Name" is "John"
  	Given "Last Name" is "Smith"
  	Given "Date of Birth" is "01/01/2000"
  	Given "Gender" is "Male"
  	Given "Primary" is "Aetna"
  	Given "ID" is "80125467"
  	When I click "Check in Now"
  	Then the system should submit a check in request
  	And navigate the display to the Patient List page
  	And display the patient at the top the Patient List

Scenario: Insight User - for integration
    Given I am logged into an Insight Location
    When I click "CheckInNow"
    Then the system should submit a check in request
    And the system should submit an ADT804 Message with Patient Information and Insurance Information to Isight via JCAPS integration
