using Microsoft.Playwright;
using OpenQA.Selenium;

namespace Demo.Tests.SelfHealing;

public enum DriverPlatform
{
    Web,
    MobileWeb,
    Native
}

public class SelfHealingDriver
{
    public IPage? PlaywrightPage { get; }

    public IWebDriver? SeleniumDriver { get; }

    public DriverPlatform Platform { get; }

    public bool IsWeb =>
        Platform == DriverPlatform.Web;

    public bool IsMobileWeb =>
        Platform == DriverPlatform.MobileWeb;

    public bool IsNative =>
        Platform == DriverPlatform.Native;

    //--------------------------------
    // WEB (PLAYWRIGHT)
    //--------------------------------

    public SelfHealingDriver(
        IPage page)
    {
        PlaywrightPage = page;
        Platform = DriverPlatform.Web;
    }

    //--------------------------------
    // SELENIUM BASED
    //--------------------------------

    public SelfHealingDriver(
        IWebDriver driver,
        DriverPlatform platform)
    {
        SeleniumDriver = driver;
        Platform = platform;

        Console.WriteLine(
            $"USING PLATFORM : {platform}");
    }

    //--------------------------------
    // URL
    //--------------------------------

    public string GetUrl()
    {
        if (IsWeb)
        {
            return PlaywrightPage?.Url
                ?? string.Empty;
        }

        if (IsMobileWeb)
        {
            return SeleniumDriver?.Url
                ?? string.Empty;
        }

        // Native app tidak punya URL
        return string.Empty;
    }
}