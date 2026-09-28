@mobileNative @selfhealingnativemobile
Feature: Launch My Demo App

Scenario: Launch and Search application successfully

Given My Demo App is installed
When user opens My Demo App
Then Product page should be displayed

When User tap one of the product
Then Backpack detail page should be displayed