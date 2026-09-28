@web @addtocartweb
Feature: Add To Cart Web

Scenario Outline: Add item to cart

Given user opens SauceDemo login page
When user login with username "<username>" and password "<password>"
Then login result should be "<result>"

When user adds "<product>" to cart
Then the cart badge should show "1"

When user opens cart page
Then "<product>" should be visible in cart

Examples:
| username      | password      | result  | product               |
| standard_user | secret_sauce  | success | Sauce Labs Backpack   |