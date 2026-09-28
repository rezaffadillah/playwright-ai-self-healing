@web @selfhealingloginweb
Feature: Self Healing Login Web

Scenario Outline: Login validation
 Given user opens SauceDemo login page
 When user login with username "<username>" and password "<password>"
 Then login result should be "<result>"

Examples:
| username | password | result |
| standard_user | secret_sauce | success |