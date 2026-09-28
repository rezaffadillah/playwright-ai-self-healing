using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SauceDemo.Tests.Utils;

namespace Demo.Tests.SelfHealing;

public static class SelfHealingEngine
{
    // =====================================================
    // CLICK
    // =====================================================

    public static async Task Click(
        SelfHealingDriver driver,
        LocatorHint hint)
    {
        if (driver.IsWeb)
        {
            var element =
                await WebHealingEngine.Find(
                    driver.PlaywrightPage!,
                    hint);

            await PlaywrightEngine.Click(
                driver.PlaywrightPage!,
                element);

            return;
        }

        if (driver.IsMobileWeb)
        {
            var element =
                MobileWebHealingEngine.Find(
                    driver.SeleniumDriver!,
                    hint);

            ElementValidator.Validate(element);

            SeleniumMobileEngine.Click(
                driver.SeleniumDriver!,
                element, false);

            return;
        }

        if (driver.IsNative)
        {
            var element =
                NativeHealingEngine.Find(
                    driver.SeleniumDriver!,
                    hint);

            ElementValidator.Validate(element);

            SeleniumMobileEngine.Click(
                driver.SeleniumDriver!,
                element, true);

            return;
        }

        throw new Exception("Unknown driver platform.");
    }

    // =====================================================
    // TYPE
    // =====================================================

    public static async Task Type(
        SelfHealingDriver driver,
        LocatorHint hint)
    {
        if (driver.IsWeb)
        {
            var element =
                await WebHealingEngine.Find(
                    driver.PlaywrightPage!,
                    hint);

            await PlaywrightEngine.Type(
                driver.PlaywrightPage!,
                element,
                hint.Value ?? "");

            return;
        }

        if (driver.IsMobileWeb)
        {
            var element =
                MobileWebHealingEngine.Find(
                    driver.SeleniumDriver!,
                    hint);

            ElementValidator.Validate(element);

            SeleniumMobileEngine.Type(
                driver.SeleniumDriver!,
                element,
                hint.Value ?? "");

            return;
        }

        if (driver.IsNative)
        {
            var element =
                NativeHealingEngine.Find(
                    driver.SeleniumDriver!,
                    hint);

            ElementValidator.Validate(element);

            SeleniumMobileEngine.Type(
                driver.SeleniumDriver!,
                element,
                hint.Value ?? "");

            return;
        }

        throw new Exception("Unknown driver platform.");
    }

    // =====================================================
    // UPLOAD FILE
    // =====================================================

    public static async Task UploadFile(
        SelfHealingDriver driver,
        LocatorHint hint)
    {
        if (driver.IsWeb)
        {
            var element =
                await WebHealingEngine.Find(
                    driver.PlaywrightPage!,
                    hint);

            await element.SetInputFilesAsync(
                hint.Value!);

            return;
        }

        if (driver.IsMobileWeb)
        {
            var element =
                MobileWebHealingEngine.Find(
                    driver.SeleniumDriver!,
                    hint);

            element.SendKeys(hint.Value!);

            return;
        }

        if (driver.IsNative)
        {
            var element =
                NativeHealingEngine.Find(
                    driver.SeleniumDriver!,
                    hint);

            element.SendKeys(hint.Value!);

            return;
        }

        throw new Exception("Unknown driver platform.");
    }

    // =====================================================
    // DRAG DROP
    // =====================================================

    public static async Task DragDrop(
        SelfHealingDriver driver,
        LocatorHint sourceHint,
        LocatorHint targetHint)
    {
        if (driver.IsWeb)
        {
            var source =
                await WebHealingEngine.Find(
                    driver.PlaywrightPage!,
                    sourceHint);

            var target =
                await WebHealingEngine.Find(
                    driver.PlaywrightPage!,
                    targetHint);

            await source.DragToAsync(target);

            return;
        }

        IWebElement sourceElement;
        IWebElement targetElement;

        if (driver.IsMobileWeb)
        {
            sourceElement =
                MobileWebHealingEngine.Find(
                    driver.SeleniumDriver!,
                    sourceHint);

            targetElement =
                MobileWebHealingEngine.Find(
                    driver.SeleniumDriver!,
                    targetHint);
        }
        else
        {
            sourceElement =
                NativeHealingEngine.Find(
                    driver.SeleniumDriver!,
                    sourceHint);

            targetElement =
                NativeHealingEngine.Find(
                    driver.SeleniumDriver!,
                    targetHint);
        }

        new Actions(driver.SeleniumDriver!)
            .DragAndDrop(sourceElement, targetElement)
            .Perform();
    }

    // =====================================================
    // SELECT
    // =====================================================

    public static async Task SelectDropdown(
        SelfHealingDriver driver,
        LocatorHint hint)
    {
        if (driver.IsWeb)
        {
            var element =
                await WebHealingEngine.Find(
                    driver.PlaywrightPage!,
                    hint);

            await element.SelectOptionAsync(
                new[] { hint.Value! });

            return;
        }

        IWebElement dropdown;

        if (driver.IsMobileWeb)
        {
            dropdown =
                MobileWebHealingEngine.Find(
                    driver.SeleniumDriver!,
                    hint);
        }
        else
        {
            dropdown =
                NativeHealingEngine.Find(
                    driver.SeleniumDriver!,
                    hint);
        }

        dropdown.Click();

        var option =
            driver.SeleniumDriver!
                .FindElement(
                    By.XPath(
                        $"//*[contains(.,'{hint.Value}')]"));

        option.Click();
    }

    // =====================================================
    // PRESS KEY
    // =====================================================

    public static async Task PressKey(
        SelfHealingDriver driver,
        string key)
    {
        if (driver.IsWeb)
        {
            await driver.PlaywrightPage!
                .Keyboard
                .PressAsync(key);

            return;
        }

        string seleniumKey =
            key.ToLower() switch
            {
                "enter" => Keys.Enter,
                "tab" => Keys.Tab,
                "escape" => Keys.Escape,
                "backspace" => Keys.Backspace,
                "delete" => Keys.Delete,
                "arrowdown" => Keys.ArrowDown,
                "arrowup" => Keys.ArrowUp,
                "arrowleft" => Keys.ArrowLeft,
                "arrowright" => Keys.ArrowRight,
                _ => key
            };

        driver.SeleniumDriver!
            .SwitchTo()
            .ActiveElement()
            .SendKeys(seleniumKey);
    }

    // =====================================================
    // SELECT
    // =====================================================

    public static async Task Select(
        SelfHealingDriver driver,
        LocatorHint hint)
    {
        await Click(driver, hint);
    }
}