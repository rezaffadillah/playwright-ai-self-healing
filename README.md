# Playwright AI Self-Healing Automation Framework

A Playwright-based test automation framework focused on AI-assisted self-healing automation across web, mobile web, and native mobile application testing.

The framework is designed to improve test resilience by validating elements, detecting locator or UI changes, and applying recovery strategies during automated test execution.

## Key Features

- AI-assisted self-healing automation
- Web automation using Playwright
- Mobile web automation using Appium
- Native Android application automation
- Element validation and locator recovery
- Retry and recovery mechanisms
- BDD-style test scenarios using Reqnroll
- API validation support
- Supporting Mock API service for test validation
- Reusable page object and driver architecture

## Technology Stack

- C#
- .NET 8
- Playwright
- Appium
- Reqnroll
- NUnit
- REST API
- Git

## Project Structure

```text
playwright-ai-self-healing/
│
├── Demo.Tests/
│   ├── Apps/
│   ├── Config/
│   ├── Context/
│   ├── Data/
│   ├── Features/
│   ├── Hooks/
│   ├── Mobile/
│   ├── MobileTests/
│   ├── Pages/
│   ├── SelfHealing/
│   ├── StepDefinitions/
│   └── Utils/
│
├── MockApi/
│   └── Supporting API service for test validation
│
├── playwright-concept.sln
├── .gitignore
└── README.md