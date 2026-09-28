# Playwright AI Self-Healing Automation Framework

A Playwright-based test automation framework focused on custom AI-driven self-healing for web, mobile web, and native Android automation.

The framework is designed to improve test resilience by detecting invalid or changed locators, evaluating alternative element candidates, and recovering interactions through custom element analysis, locator-resolution, and scoring logic.

> **Note:** The AI component in this project is implemented as custom framework logic and decision-making mechanisms. It does not depend on external LLM services such as OpenAI, ChatGPT, or Gemini.

---

## Why Self-Healing?

Traditional UI automation can become fragile when application locators change.

For example, a test may originally use:

```text
#login-button
```

If the application's locator changes, a conventional automation test may fail immediately.

This framework introduces a custom AI self-healing layer that attempts to recover the interaction instead of immediately failing the test.

The general recovery process is:

1. Attempt the original locator.
2. Detect when the locator cannot be resolved.
3. Discover alternative element candidates.
4. Analyze candidate attributes and semantic hints.
5. Evaluate candidate relevance using custom scoring logic.
6. Select the most relevant candidate.
7. Retry the interaction using the recovered element.

---

## Self-Healing Flow

```text
Test Scenario
      │
      ▼
SelfHealingDriver
      │
      ▼
Locator Hint
      │
      ▼
Try Original Locator
      │
      ├── Found ───────────────► Execute Action
      │
      └── Not Found
              │
              ▼
       AI Healing Triggered
              │
              ▼
      Candidate Discovery
              │
              ▼
        Element Analysis
              │
              ▼
        Candidate Scoring
              │
              ▼
       Best Candidate Selected
              │
              ▼
        Retry Interaction
```

---

## Example: Web Self-Healing

The framework intentionally uses invalid locators in the sample login page to exercise the self-healing mechanism.

Example:

```csharp
private const string InpUsername = "#Reza-project-username";
private const string InpPassword = "#Reza-project-password";
private const string BtnLogin = "#login-button-wrong";
```

The test interaction is still driven by semantic hints:

```csharp
await Type("username", user, InpUsername);
await Type("password", pass, InpPassword);
await Click("login", BtnLogin);
```

When the original locator cannot be resolved, the self-healing engine searches for alternative candidates and evaluates them using the framework's custom analysis and scoring logic.

This allows the automation flow to recover from certain locator or UI changes without requiring the original selector to be manually updated immediately.

---

## AI Self-Healing Approach

The framework uses custom decision logic to evaluate potential replacement elements.

Candidate evaluation can consider attributes and semantic information such as:

- `id`
- `name`
- `data-test`
- `aria-label`
- `placeholder`
- `title`
- element text
- element type
- role
- keyword relevance

The candidate evaluation process is designed to identify the element that most closely matches the intended interaction.

Conceptually:

```text
Original Locator
      │
      ▼
Locator Validation
      │
      ├── Valid
      │    └── Execute
      │
      └── Invalid
           │
           ▼
    Find Candidates
           │
           ▼
    Analyze Elements
           │
           ▼
      Score Matches
           │
           ▼
    Select Best Match
           │
           ▼
     Execute Action
```

The approach is intended to make UI automation more resilient while keeping the self-healing logic inside the automation framework itself.

---

## Self-Healing Components

The main self-healing implementation is organized under the `SelfHealing` folder.

```text
Demo.Tests/
└── SelfHealing/
    ├── ElementValidator.cs
    ├── MobileWebHealingEngine.cs
    ├── NativeHealingEngine.cs
    ├── PlaywrightEngine.cs
    ├── RetryEngine.cs
    ├── SeleniumMobileEngine.cs
    ├── SelfHealingDriver.cs
    ├── SelfHealingEngine.cs
    └── WebHealingEngine.cs
```

### Web Self-Healing

`WebHealingEngine` provides the self-healing layer for Playwright-based web automation.

The recovery process includes:

- Original locator validation
- Alternative candidate discovery
- Element analysis
- Candidate scoring
- Best-match selection
- Interaction recovery

### Mobile Web Self-Healing

`MobileWebHealingEngine` provides self-healing support for mobile web automation.

The engine follows a similar candidate discovery and scoring approach while operating within the mobile web automation flow.

### Native Android Self-Healing

`NativeHealingEngine` provides self-healing support for native Android automation using Appium.

Native Android recovery is handled separately from web and mobile-web automation so that platform-specific interaction strategies can remain independent.

---

## Retry and Recovery

The framework also contains a dedicated retry layer.

The `RetryEngine` works together with the self-healing mechanism to allow failed interactions to be retried after recovery attempts.

The framework includes handling for common automation failures such as:

- `NoSuchElementException`
- `StaleElementReferenceException`
- timeout-related failures
- WebDriver-related exceptions
- invalid session-related failures

The objective is to distinguish between a transient interaction failure and a locator-related problem that may be recoverable through the self-healing layer.

---

## Cross-Platform Automation

The framework contains dedicated automation components for web, mobile web, and native Android.

```text
                 Self-Healing Framework
                         │
          ┌──────────────┼──────────────┐
          │              │              │
          ▼              ▼              ▼
        Web         Mobile Web     Native Android
     Playwright       Appium          Appium
```

This separation allows platform-specific automation logic to remain independent while applying the same overall self-healing concept across different application types.

---

## Test Coverage

### Web

- Login
- Add to Cart
- Self-healing login scenario
- Element validation and recovery

### Mobile Web

- Login
- Self-healing mobile web scenario
- Locator recovery

### Native Android

- Application launch
- Product search
- Product interaction
- Native element recovery

---

## BDD Test Automation

The project uses **Reqnroll** with Gherkin-style feature files to describe test scenarios.

Example:

```gherkin
Scenario Outline: Login validation
    Given user opens SauceDemo login page
    When user login with username "<username>" and password "<password>"
    Then login result should be "<result>"
```

This allows business-level test scenarios to remain separated from implementation details while the underlying step definitions use the automation framework.

---

## API Validation

API validation is included as a supporting capability of the framework.

The repository contains a lightweight ASP.NET Core Mock API that can be used to support API-based validation within the automation flow.

```text
Playwright / Appium Tests
          │
          ├──────── UI Validation
          │
          └──────── API Validation
                       │
                       ▼
                    MockApi
```

The Mock API is a **supporting component** of the project.

The primary focus of this repository is the custom AI-driven self-healing automation framework.

---

## Technology Stack

- C#
- .NET 8
- Playwright
- Selenium WebDriver
- Appium
- Reqnroll
- NUnit
- ASP.NET Core
- REST API
- Git

---

## Project Structure

```text
playwright-ai-self-healing/
│
├── Demo.Tests/
│   ├── Apps/
│   │   └── Android/
│   │       └── MyDemoApp.apk
│   │
│   ├── Config/
│   ├── Context/
│   ├── Data/
│   ├── Features/
│   │   ├── Mobile/
│   │   ├── MobileNative/
│   │   └── Web/
│   │
│   ├── Hooks/
│   ├── Mobile/
│   ├── MobileTests/
│   ├── Pages/
│   ├── SelfHealing/
│   ├── StepDefinitions/
│   └── Utils/
│
├── MockApi/
│   └── Supporting API service for validation
│
├── playwright-concept.sln
├── .gitignore
└── README.md
```

---

## Application Under Test

The framework uses publicly available demo applications for automation scenarios.

### Web

```text
https://www.saucedemo.com/
```

### Native Android

The native Android automation uses the included demo application:

```text
Demo.Tests/Apps/Android/MyDemoApp.apk
```

---

## Getting Started

### Prerequisites

- .NET 8 SDK
- Node.js
- Android Emulator
- Appium Server
- Playwright
- Git

### Clone the Repository

```bash
git clone https://github.com/rezaffadillah/playwright-ai-self-healing.git
```

```bash
cd playwright-ai-self-healing
```

### Restore Dependencies

```bash
dotnet restore
```

### Install Playwright Browsers

After restoring the project, install the Playwright browsers required by the test project.

```bash
pwsh bin/Debug/net8.0/playwright.ps1 install
```

### Run Tests

```bash
dotnet test
```

---

## Mobile Automation Setup

Native Android automation requires:

- Android Emulator or physical Android device
- Appium Server
- UiAutomator2 driver
- Appropriate Android SDK configuration

The project configuration contains the required Appium and Android automation settings under:

```text
Demo.Tests/Config/
```

The native application package used by the framework is included in:

```text
Demo.Tests/Apps/Android/MyDemoApp.apk
```

---

## Project Goal

This project was created as a technical demonstration of designing a maintainable test automation framework with custom AI-driven self-healing capabilities across:

- Web automation
- Mobile web automation
- Native Android automation
- Element validation
- Locator recovery
- Candidate evaluation
- Retry and recovery
- Supporting API validation

The primary goal is to improve automation resilience by allowing the framework to identify alternative elements and recover interactions when the original locator is no longer valid.

---

## Key Takeaway

The core concept of this project is:

```text
Traditional Automation

Locator Changes
      │
      ▼
Test Failure


Self-Healing Automation

Locator Changes
      │
      ▼
Detect Failure
      │
      ▼
Find Alternative Candidates
      │
      ▼
Analyze & Score
      │
      ▼
Recover Locator
      │
      ▼
Retry Interaction
      │
      ▼
Continue Test
```

This repository demonstrates how custom AI-driven decision logic can be integrated directly into a test automation framework to improve resilience against UI and locator changes.