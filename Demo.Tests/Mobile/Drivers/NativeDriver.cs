using Demo.Tests.Config;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

namespace Demo.Tests.Mobile.Drivers;

public static class NativeDriver
{
    private static AndroidDriver? _driver;

    public static AndroidDriver Driver =>
        _driver ??
        throw new Exception(
            "Native Driver has not been initialized.");

    public static bool IsStarted =>
        _driver != null;

    public static void Start()
    {
        if (_driver != null)
            return;

        Console.WriteLine("====================================");
        Console.WriteLine(" STARTING NATIVE DRIVER");
        Console.WriteLine("====================================");

        var apkPath =
            Path.GetFullPath(
                TestConfigNative.AppPath);

        if (!File.Exists(apkPath))
        {
            throw new FileNotFoundException(
                $"APK not found : {apkPath}");
        }

        var options = new AppiumOptions
        {
            PlatformName = TestConfigNative.PlatformName,
            AutomationName = TestConfigNative.AutomationName,
            DeviceName = TestConfigNative.DeviceName,
            App = apkPath
        };

        options.AddAdditionalAppiumOption(
            "appWaitActivity",
            "*");

        options.AddAdditionalAppiumOption(
            "disableWindowAnimation",
            true);

        options.AddAdditionalAppiumOption(
            "ignoreUnimportantViews",
            true);

        options.AddAdditionalAppiumOption(
            "waitForIdleTimeout", 0);

        options.AddAdditionalAppiumOption(
            "waitForSelectorTimeout", 0);

        options.AddAdditionalAppiumOption(
            "disableWindowAnimation", true);

        options.AddAdditionalAppiumOption(
            "ignoreUnimportantViews", true);

        if (!string.IsNullOrWhiteSpace(
            TestConfigNative.AppPackage))
        {
            options.AddAdditionalAppiumOption(
                "appPackage",
                TestConfigNative.AppPackage);
        }

        if (!string.IsNullOrWhiteSpace(
            TestConfigNative.AppActivity))
        {
            options.AddAdditionalAppiumOption(
                "appActivity",
                TestConfigNative.AppActivity);
        }

        //--------------------------------
        // Reset
        //--------------------------------

        options.AddAdditionalAppiumOption(
            "noReset",
            TestConfigNative.NoReset);

        options.AddAdditionalAppiumOption(
            "fullReset",
            TestConfigNative.FullReset);

        options.AddAdditionalAppiumOption(
            "autoGrantPermissions",
            TestConfigNative.AutoGrantPermissions);

        //--------------------------------
        // Timeout
        //--------------------------------

        options.AddAdditionalAppiumOption(
            "newCommandTimeout",
            TestConfigNative.NewCommandTimeout);

        //--------------------------------
        // Package
        //--------------------------------

        if (!string.IsNullOrWhiteSpace(
            TestConfigNative.AppPackage))
        {
            options.AddAdditionalAppiumOption(
                "appPackage",
                TestConfigNative.AppPackage);
        }

        if (!string.IsNullOrWhiteSpace(
            TestConfigNative.AppActivity))
        {
            options.AddAdditionalAppiumOption(
                "appActivity",
                TestConfigNative.AppActivity);
        }

        //--------------------------------
        // Driver
        //--------------------------------

        Console.WriteLine($"APK           : {apkPath}");
        Console.WriteLine($"Server        : {TestConfigNative.AppiumServer}");
        Console.WriteLine($"Device        : {TestConfigNative.DeviceName}");
        Console.WriteLine($"Package       : {TestConfigNative.AppPackage}");
        Console.WriteLine($"Activity      : {TestConfigNative.AppActivity}");

        _driver =
            new AndroidDriver(
                new Uri(TestConfigNative.AppiumServer),
                options,
                TimeSpan.FromSeconds(180));

        _driver
            .Manage()
            .Timeouts()
            .ImplicitWait =
            TimeSpan.FromMilliseconds(500);
        Console.WriteLine($"IMPLICIT : {_driver.Manage().Timeouts()}");

        Console.WriteLine("Native Driver Started.");
    }

    public static void Quit()
    {
        if (_driver == null)
            return;

        try
        {
            Console.WriteLine(
                "Stopping Native Driver...");

            //--------------------------------
            // Close application only
            //--------------------------------

            _driver.TerminateApp(
                "com.saucelabs.mydemoapp.android");

            //--------------------------------
            // Close Appium session
            //--------------------------------

            _driver.Quit();

            _driver.Dispose();
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Quit Error : {ex.Message}");
        }
        finally
        {
            _driver = null;
        }
    }
}