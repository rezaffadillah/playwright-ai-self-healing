using Microsoft.Extensions.Configuration;

namespace Demo.Tests.Config;

public static class TestConfigMobile
{
    private static readonly IConfiguration Root =
        new ConfigurationBuilder()
        .AddJsonFile(
            "appsettings.json",
            optional: false,
            reloadOnChange: true)
        .Build();

    public static string ApplicationUrl =>
        Root["TestConfigMobile:ApplicationUrl"]
        ?? throw new Exception(
            "TestConfigMobile:ApplicationUrl missing");

    public static string AppiumServer =>
        Root["TestConfigMobile:AppiumServer"]
        ?? throw new Exception(
            "TestConfigMobile:AppiumServer missing");

    public static string DeviceName =>
        Root["TestConfigMobile:DeviceName"]
        ?? throw new Exception(
            "TestConfigMobile:DeviceName missing");

    public static string PlatformName =>
        Root["TestConfigMobile:PlatformName"]
        ?? throw new Exception(
            "TestConfigMobile:PlatformName missing");

    public static string AutomationName =>
        Root["TestConfigMobile:AutomationName"]
        ?? throw new Exception(
            "TestConfigMobile:AutomationName missing");

    public static string BrowserName =>
        Root["TestConfigMobile:BrowserName"]
        ?? throw new Exception(
            "TestConfigMobile:BrowserName missing");

    public static int NewCommandTimeout =>
        int.TryParse(
            Root["TestConfigMobile:NewCommandTimeout"],
            out var timeout)
        ? timeout
        : throw new Exception(
            "TestConfigMobile:NewCommandTimeout invalid or missing");
}