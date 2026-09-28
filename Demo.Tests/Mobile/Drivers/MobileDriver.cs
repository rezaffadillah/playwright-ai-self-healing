using Demo.Tests.Config;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Support.UI;

namespace Demo.Tests.Mobile.Drivers;

public static class MobileDriver
{
    private static AndroidDriver? _driver;

    public static AndroidDriver Driver =>
        _driver ??
        throw new Exception(
            "Mobile driver not initialized");

    public static void Start()
    {
        if (_driver != null)
            return;

        var options = new AppiumOptions
        {
            PlatformName = TestConfigMobile.PlatformName,
            AutomationName = TestConfigMobile.AutomationName,
            BrowserName = TestConfigMobile.BrowserName,
            DeviceName = TestConfigMobile.DeviceName
        };

        options.AddAdditionalAppiumOption(
            "chromedriverAutodownload",
            true);

        options.AddAdditionalAppiumOption(
            "newCommandTimeout",
            TestConfigMobile.NewCommandTimeout);

        options.AddAdditionalAppiumOption(
            "noReset",
            false);

        options.AddAdditionalAppiumOption(
            "autoGrantPermissions",
            true);

        options.AddAdditionalAppiumOption(
            "chromeOptions",
            new Dictionary<string, object>
            {
                ["args"] = new[]
                {
                    "--disable-notifications",
                    "--no-first-run",
                    "--disable-fre"
                }
            });

        _driver = new AndroidDriver(
            new Uri(TestConfigMobile.AppiumServer),
            options,
            TimeSpan.FromSeconds(180));

        _driver.Manage().Timeouts().ImplicitWait =
            TimeSpan.FromSeconds(10);

        Thread.Sleep(3000);

        DismissChromePopup();
    }

    private static void DismissChromePopup()
    {
        if (_driver == null)
            return;

        try
        {
            _driver.Context = "NATIVE_APP";

            for (int i = 0; i < 5; i++)
            {
                var buttons =
                    _driver.FindElements(
                        By.XPath("//android.widget.Button"));

                if (buttons.Count == 0)
                    break;

                foreach (var btn in buttons)
                {
                    var text =
                        btn.Text?.ToLower() ?? "";

                    Console.WriteLine(
                        $"[POPUP] Found: {text}");

                    if (text.Contains("no") ||
                        text.Contains("continue") ||
                        text.Contains("accept") ||
                        text.Contains("skip") ||
                        text.Contains("got it"))
                    {
                        btn.Click();

                        Thread.Sleep(1500);

                        break;
                    }
                }
            }

            Console.WriteLine("Popup handled");
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Popup handler error: {ex.Message}");
        }
    }

    public static void SwitchToWebView()
    {
        if (_driver == null)
            throw new Exception("Driver null");

        for (int i = 0; i < 10; i++)
        {
            var contexts = _driver.Contexts;

            Console.WriteLine(
                "Contexts: " +
                string.Join(", ", contexts));

            var webContext =
                contexts.FirstOrDefault(
                    c => c.Contains("CHROMIUM"));

            if (webContext != null)
            {
                _driver.Context = webContext;

                Console.WriteLine(
                    $"Switched to: {webContext}");

                return;
            }

            Thread.Sleep(1000);
        }

        throw new Exception(
            "Web context not found!");
    }

    public static void WaitForDomStable(
        IWebDriver driver)
    {
        var wait =
            new WebDriverWait(
                driver,
                TimeSpan.FromSeconds(30));

        wait.Until(d =>
        {
            try
            {
                if (d is not IJavaScriptExecutor js)
                    return false;

                var readyState =
                    js.ExecuteScript(
                        "return document.readyState")
                    ?.ToString();

                var hasInput =
                    d.FindElements(
                        By.Id("user-name")).Count > 0;

                Console.WriteLine(
                    $"readyState={readyState}, input={hasInput}");

                return readyState == "complete"
                    && hasInput;
            }
            catch
            {
                return false;
            }
        });

        Console.WriteLine("Login page ready");
    }

    public static void Quit()
    {
        try
        {
            _driver?.Quit();
        }
        finally
        {
            _driver = null;
        }
    }
}