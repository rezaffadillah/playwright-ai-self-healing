using Microsoft.Extensions.Configuration;

namespace Demo.Tests.Config;

public static class TestConfigNative
{
    private static readonly IConfiguration Root =
        new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

    private static string Read(string key)
    {
        return Root[key]
            ?? throw new Exception(
                $"Missing configuration: {key}");
    }

    // =========================
    // PLATFORM
    // =========================

    public static string PlatformName =>
        Read("TestConfigNative:PlatformName");

    public static string AutomationName =>
        Read("TestConfigNative:AutomationName");

    public static string DeviceName =>
        Read("TestConfigNative:DeviceName");

    public static string AppiumServer =>
        Read("TestConfigNative:AppiumServer");

    public static string Udid =>
        Read("TestConfigNative:Udid");

    // =========================
    // APK
    // =========================

    public static string AppPath =>
        Read("TestConfigNative:AppPath");

    public static string AppPackage =>
        Read("TestConfigNative:AppPackage");

    public static string AppActivity =>
        Read("TestConfigNative:AppActivity");

    // =========================
    // SESSION
    // =========================

    public static bool AutoGrantPermissions =>
        bool.Parse(Read("TestConfigNative:AutoGrantPermissions"));

    public static bool NoReset =>
        bool.Parse(Read("TestConfigNative:NoReset"));

    public static bool FullReset =>
        bool.Parse(Read("TestConfigNative:FullReset"));

    public static int NewCommandTimeout =>
        int.Parse(Read("TestConfigNative:NewCommandTimeout"));

    // =========================
    // WAIT
    // =========================

    public static int ImplicitWait =>
        int.Parse(Read("TestConfigNative:ImplicitWait"));

    public static int LaunchTimeout =>
        int.Parse(Read("TestConfigNative:LaunchTimeout"));
}