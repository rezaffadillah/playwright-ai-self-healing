@mobile @selfhealingloginmobile
Feature: Sauce Demo Login on Android Chrome

Scenario Outline: User login successfully on mobile chrome
 Given user opens SauceDemo website in mobile chrome
 When user logs in on mobile with username "<username>" and password "<password>"
 Then mobile login should be "<result>"

Examples:
| username      | password     | result  |
| standard_user | secret_sauce | success |