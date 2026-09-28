using Demo.Tests.SelfHealing;
using SauceDemo.Tests.Utils;
using Microsoft.Playwright;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Appium;

namespace Demo.Tests.Pages;

public abstract class BasePage
{
    protected readonly SelfHealingDriver Driver;

    protected BasePage(SelfHealingDriver driver)
    {
        Driver = driver;
    }

    // =========================
    // BASIC ACTIONS
    // =========================

    protected async Task Click(
        string keyword,
        string? css = null,
        string? xpath = null,
        string? id = null,
        string? name = null,
        string? accessibilityId = null,
        string? resourceId = null,
        string? text = null,
        params string[] aliases)
    {
        await SelfHealingEngine.Click(
            Driver,
            new LocatorHint
            {
                Keyword = keyword,
                Css = css,
                XPath = xpath,
                Id = id,
                Name = name,
                AccessibilityId = accessibilityId,
                ResourceId = resourceId,
                Text = text,

                // IMPORTANT
                Type = ElementType.Button,

                Aliases = aliases
            });
    }

    protected async Task Type(
        string keyword,
        string value,
        string? css = null,
        string? xpath = null,
        string? id = null,
        string? name = null,
        string? accessibilityId = null,
        string? resourceId = null,
        params string[] aliases)
    {
        await SelfHealingEngine.Type(
            Driver,
            new LocatorHint
            {
                Keyword = keyword,
                Value = value,
                Css = css,
                XPath = xpath,
                Id = id,
                Name = name,
                AccessibilityId = accessibilityId,
                ResourceId = resourceId,
                Type = ElementType.Input,
                Aliases = aliases
            });
    }

    protected async Task PressKey(string key)
    {
        await SelfHealingEngine.PressKey(
            Driver,
            key);
    }

    protected async Task UploadFile(
        string keyword,
        string filePath,
        string? css = null,
        string? xpath = null,
        string? id = null,
        params string[] aliases)
    {
        await SelfHealingEngine.UploadFile(
            Driver,
            new LocatorHint
            {
                Keyword = keyword,
                Value = filePath,
                Css = css,
                XPath = xpath,
                Id = id,
                Type = ElementType.Input,
                Aliases = aliases
            });
    }

    protected async Task DragDrop(
        string sourceKeyword,
        string targetKeyword,
        string? sourceCss = null,
        string? targetCss = null)
    {
        await SelfHealingEngine.DragDrop(
            Driver,

            new LocatorHint
            {
                Keyword = sourceKeyword,
                Css = sourceCss
            },

            new LocatorHint
            {
                Keyword = targetKeyword,
                Css = targetCss
            });
    }

    protected async Task SelectDropdown(
        string keyword,
        string value,
        string? css = null,
        params string[] aliases)
    {
        await SelfHealingEngine.SelectDropdown(
            Driver,
            new LocatorHint
            {
                Keyword = keyword,
                Css = css,
                Value = value,
                Aliases = aliases
            });
    }

    protected async Task Select(
        string keyword,
        string? css = null,
        params string[] aliases)
    {
        await SelfHealingEngine.Select(
            Driver,
            new LocatorHint
            {
                Keyword = keyword,
                Css = css,
                Aliases = aliases,
                Type = ElementType.Select
            });
    }

    // =========================
    // WAITING
    // =========================

    protected async Task Wait(int milliseconds)
    {
        await Task.Delay(milliseconds);
    }

    protected async Task WaitForPageLoad()
    {
        if (Driver.IsWeb)
        {
            await Driver.PlaywrightPage!
                .WaitForLoadStateAsync(
                    LoadState.DOMContentLoaded);
        }
    }

    protected async Task WaitForNetworkIdle()
    {
        if (Driver.IsWeb)
        {
            await Driver.PlaywrightPage!
                .WaitForLoadStateAsync(
                    LoadState.NetworkIdle);
        }
    }

    protected async Task WaitForUrlContains(
        string keyword,
        int timeout = 10000)
    {
        if (Driver.IsWeb)
        {
            await Driver.PlaywrightPage!
                .WaitForURLAsync(
                    url => url.Contains(keyword),
                    new()
                    {
                        Timeout = timeout
                    });
        }
        else
        {
            var wait =
                new WebDriverWait(
                    Driver.SeleniumDriver!,
                    TimeSpan.FromMilliseconds(timeout));

            wait.Until(d => d.Url.Contains(keyword));
        }
    }

    protected async Task WaitForUrl(
        string url,
        int timeout = 10000)
    {
        if (Driver.IsWeb)
        {
            await Driver.PlaywrightPage!
                .WaitForURLAsync(
                    u => u == url,
                    new()
                    {
                        Timeout = timeout
                    });
        }
        else
        {
            var wait =
                new WebDriverWait(
                    Driver.SeleniumDriver!,
                    TimeSpan.FromMilliseconds(timeout));

            wait.Until(d => d.Url == url);
        }
    }

    protected void WaitForElementVisible(
        string? keyword = null,
        string? css = null,
        string? xpath = null,
        string? id = null,
        string? accessibilityId = null,
        string? resourceId = null,
        int timeout = 10000)
    {
        // Web (Playwright)
        if (Driver.IsWeb)
        {
            return;
        }

        var wait = new WebDriverWait(
            Driver.SeleniumDriver!,
            TimeSpan.FromMilliseconds(timeout));

        wait.Until(_ =>
        {
            try
            {
                IWebElement el;

                // Native AccessibilityId
                if (!string.IsNullOrWhiteSpace(accessibilityId))
                {
                    el = Driver.SeleniumDriver!
                        .FindElement(
                            MobileBy.AccessibilityId(
                                accessibilityId));
                }

                // Native ResourceId
                else if (!string.IsNullOrWhiteSpace(resourceId))
                {
                    el = Driver.SeleniumDriver!
                        .FindElement(
                            By.Id(resourceId));
                }

                // XPath
                else if (!string.IsNullOrWhiteSpace(xpath))
                {
                    el = Driver.SeleniumDriver!
                        .FindElement(
                            By.XPath(xpath));
                }

                // HTML ID
                else if (!string.IsNullOrWhiteSpace(id))
                {
                    el = Driver.SeleniumDriver!
                        .FindElement(
                            By.Id(id));
                }

                // CSS
                else if (!string.IsNullOrWhiteSpace(css))
                {
                    el = Driver.SeleniumDriver!
                        .FindElement(
                            By.CssSelector(css));
                }

                // Fallback keyword
                else
                {
                    el = Driver.SeleniumDriver!
                        .FindElement(
                            By.XPath(
                                $"//*[contains(@text,'{keyword}')]"));
                }

                return el.Displayed;
            }
            catch
            {
                return false;
            }
        });
    }

    protected async Task WaitForElementHidden(
        string keyword,
        string? css = null,
        int timeout = 10000)
    {
        if (Driver.IsWeb)
        {
            var el =
                !string.IsNullOrEmpty(css)
                ? Driver.PlaywrightPage!.Locator(css).First
                : Driver.PlaywrightPage!
                    .Locator($"text={keyword}")
                    .First;

            await el.WaitForAsync(new()
            {
                State = WaitForSelectorState.Hidden,
                Timeout = timeout
            });
        }
        else
        {
            var wait =
                new WebDriverWait(
                    Driver.SeleniumDriver!,
                    TimeSpan.FromMilliseconds(timeout));

            wait.Until(d =>
            {
                try
                {
                    IWebElement el =
                        !string.IsNullOrEmpty(css)
                        ? d.FindElement(By.CssSelector(css))
                        : d.FindElement(
                            By.XPath(
                                $"//*[contains(text(), '{keyword}')]"));

                    return !el.Displayed;
                }
                catch
                {
                    return true;
                }
            });
        }
    }

    protected async Task WaitForText(
        string expectedText,
        int timeout = 10000)
    {
        if (Driver.IsWeb)
        {
            await Driver.PlaywrightPage!
                .WaitForFunctionAsync(
                    @"text => document.body.innerText.includes(text)",
                    expectedText,
                    new()
                    {
                        Timeout = timeout
                    });
        }
        else
        {
            var wait =
                new WebDriverWait(
                    Driver.SeleniumDriver!,
                    TimeSpan.FromMilliseconds(timeout));

            wait.Until(d =>
                d.PageSource.Contains(expectedText));
        }
    }

    // =========================
    // NAVIGATION
    // =========================

    protected async Task Reload()
    {
        if (Driver.IsWeb)
        {
            await Driver.PlaywrightPage!
                .ReloadAsync();
        }
        else
        {
            Driver.SeleniumDriver!
                .Navigate()
                .Refresh();
        }
    }

    protected string GetUrl()
    {
        return Driver.GetUrl();
    }

    protected async Task<string> GetTitle()
    {
        if (Driver.IsWeb)
        {
            return await Driver.PlaywrightPage!
                .TitleAsync();
        }

        return Driver.SeleniumDriver!.Title;
    }

    // =========================
    // SCROLL
    // =========================

    protected async Task ScrollTo(
        string keyword)
    {
        if (Driver.IsWeb)
        {
            var el =
                Driver.PlaywrightPage!
                    .Locator($"text={keyword}");

            await el
                .First
                .ScrollIntoViewIfNeededAsync();
        }
    }

    protected async Task ScrollToBottom()
    {
        if (Driver.IsWeb)
        {
            await Driver.PlaywrightPage!
                .EvaluateAsync(
                    "window.scrollTo(0, document.body.scrollHeight)");
        }
    }

    protected async Task ScrollToTop()
    {
        if (Driver.IsWeb)
        {
            await Driver.PlaywrightPage!
                .EvaluateAsync(
                    "window.scrollTo(0, 0)");
        }
    }

    // =========================
    // GET VALUE / TEXT
    // =========================

    protected async Task<string> GetText(
        string? keyword = null,
        string? css = null,
        string? xpath = null,
        string? id = null,
        string? accessibilityId = null,
        string? resourceId = null)
    {
        if (Driver.IsWeb)
        {
            if (!string.IsNullOrEmpty(css))
                return await Driver.PlaywrightPage!
                    .Locator(css)
                    .InnerTextAsync();

            if (!string.IsNullOrEmpty(xpath))
                return await Driver.PlaywrightPage!
                    .Locator($"xpath={xpath}")
                    .InnerTextAsync();

            if (!string.IsNullOrEmpty(id))
                return await Driver.PlaywrightPage!
                    .Locator($"#{id}")
                    .InnerTextAsync();

            return "";
        }

        IWebElement element;

        if (!string.IsNullOrEmpty(accessibilityId))
        {
            element = Driver.SeleniumDriver!
                .FindElement(
                    MobileBy.AccessibilityId(
                        accessibilityId));
        }
        else if (!string.IsNullOrEmpty(resourceId))
        {
            element = Driver.SeleniumDriver!
                .FindElement(
                    By.Id(resourceId));
        }
        else if (!string.IsNullOrEmpty(xpath))
        {
            element = Driver.SeleniumDriver!
                .FindElement(
                    By.XPath(xpath));
        }
        else if (!string.IsNullOrEmpty(id))
        {
            element = Driver.SeleniumDriver!
                .FindElement(
                    By.Id(id));
        }
        else
        {
            element = Driver.SeleniumDriver!
                .FindElement(
                    By.XPath(
                        $"//*[contains(@text,'{keyword}')]"));
        }

        return element.Text ?? "";
    }

    protected async Task<string> GetValue(
        string? keyword = null,
        string? css = null,
        string? xpath = null,
        string? id = null,
        string? accessibilityId = null,
        string? resourceId = null)
    {
        if (Driver.IsWeb)
        {
            if (!string.IsNullOrEmpty(css))
                return await Driver.PlaywrightPage!
                    .Locator(css)
                    .InputValueAsync();

            if (!string.IsNullOrEmpty(xpath))
                return await Driver.PlaywrightPage!
                    .Locator($"xpath={xpath}")
                    .InputValueAsync();

            if (!string.IsNullOrEmpty(id))
                return await Driver.PlaywrightPage!
                    .Locator($"#{id}")
                    .InputValueAsync();

            return "";
        }

        IWebElement element;

        if (!string.IsNullOrEmpty(accessibilityId))
        {
            element = Driver.SeleniumDriver!
                .FindElement(
                    MobileBy.AccessibilityId(
                        accessibilityId));
        }
        else if (!string.IsNullOrEmpty(resourceId))
        {
            element = Driver.SeleniumDriver!
                .FindElement(
                    By.Id(resourceId));
        }
        else if (!string.IsNullOrEmpty(xpath))
        {
            element = Driver.SeleniumDriver!
                .FindElement(
                    By.XPath(xpath));
        }
        else if (!string.IsNullOrEmpty(id))
        {
            element = Driver.SeleniumDriver!
                .FindElement(
                    By.Id(id));
        }
        else
        {
            element = Driver.SeleniumDriver!
                .FindElement(
                    By.XPath(
                        $"//*[contains(@text,'{keyword}')]"));
        }

        return
            element.GetAttribute("value")
            ?? element.Text
            ?? "";
    }

    // =========================
    // VALIDATION
    // =========================

    protected async Task<bool> IsDisplayed(
        string? keyword = null,
        string? css = null,
        string? xpath = null,
        string? id = null,
        string? name = null,
        string? accessibilityId = null,
        string? resourceId = null)
    {
        try
        {
            //------------------------------------
            // WEB (PLAYWRIGHT)
            //------------------------------------
            if (Driver.IsWeb)
            {
                if (!string.IsNullOrEmpty(css))
                    return await Driver.PlaywrightPage!
                        .Locator(css)
                        .First
                        .IsVisibleAsync();

                if (!string.IsNullOrEmpty(xpath))
                    return await Driver.PlaywrightPage!
                        .Locator($"xpath={xpath}")
                        .First
                        .IsVisibleAsync();

                if (!string.IsNullOrEmpty(id))
                    return await Driver.PlaywrightPage!
                        .Locator($"#{id}")
                        .First
                        .IsVisibleAsync();

                if (!string.IsNullOrEmpty(name))
                    return await Driver.PlaywrightPage!
                        .Locator($"[name='{name}']")
                        .First
                        .IsVisibleAsync();

                if (!string.IsNullOrEmpty(keyword))
                    return await Driver.PlaywrightPage!
                        .Locator($"text={keyword}")
                        .First
                        .IsVisibleAsync();

                return false;
            }

            //------------------------------------
            // MOBILE WEB / NATIVE
            //------------------------------------

            IWebElement element;

            if (Driver.IsMobileWeb)
            {
                element =
                    MobileWebHealingEngine.Find(
                        Driver.SeleniumDriver!,
                        new LocatorHint
                        {
                            Keyword = keyword ?? "",
                            Css = css,
                            XPath = xpath,
                            Id = id,
                            Name = name,
                            Type = ElementType.Any
                        });
            }
            else
            {
                element =
                    NativeHealingEngine.Find(
                        Driver.SeleniumDriver!,
                        new LocatorHint
                        {
                            Keyword = keyword ?? "",
                            XPath = xpath,
                            ResourceId = resourceId,
                            AccessibilityId = accessibilityId,
                            Type = ElementType.Any
                        });
            }

            Console.WriteLine($"FOUND ELEMENT : {element.Text}");

            return element.Displayed;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ISDISPLAYED ERROR : {ex.Message}");
            return false;
        }
    }

    protected async Task<bool> IsElementVisible(
        string keyword,
        string? css = null)
    {
        try
        {
            WaitForElementVisible(
                keyword: keyword,
                css: css,
                timeout: 1000);

            return true;
        }
        catch
        {
            return false;
        }
    }

    protected async Task<bool> IsElementExists(
        string keyword,
        string? css = null)
    {
        return await IsElementVisible(
            keyword,
            css);
    }

    // =========================
    // FRAME
    // =========================

    protected void SwitchToFrame(
        string frameNameOrId)
    {
        if (!Driver.IsWeb)
        {
            Driver.SeleniumDriver!
                .SwitchTo()
                .Frame(frameNameOrId);
        }
    }

    protected void SwitchToDefault()
    {
        if (!Driver.IsWeb)
        {
            Driver.SeleniumDriver!
                .SwitchTo()
                .DefaultContent();
        }
    }

    // =========================
    // MOUSE
    // =========================

    protected async Task Hover(
        string keyword)
    {
        if (Driver.IsWeb)
        {
            var el =
                Driver.PlaywrightPage!
                    .Locator($"text={keyword}");

            await el
                .First
                .HoverAsync();
        }
    }

    // =========================
    // SCREENSHOT
    // =========================

    protected async Task Screenshot(
        string fileName)
    {
        Directory.CreateDirectory("Screenshots");

        var path =
            $"Screenshots/{fileName}_{DateTime.Now:yyyyMMdd_HHmmss}.png";

        if (Driver.IsWeb)
        {
            await Driver.PlaywrightPage!
                .ScreenshotAsync(new()
                {
                    Path = path,
                    FullPage = true
                });
        }
        else
        {
            var ss =
                ((ITakesScreenshot)Driver.SeleniumDriver!)
                .GetScreenshot();

            ss.SaveAsFile(path);
        }

        Console.WriteLine($"[SCREENSHOT] {path}");
    }
}