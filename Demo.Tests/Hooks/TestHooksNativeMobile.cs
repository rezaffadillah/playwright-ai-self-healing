using Reqnroll;
using Demo.Tests.Context;
using Demo.Tests.SelfHealing;
using Demo.Tests.Mobile.Drivers;

namespace Demo.Tests.Hooks;

[Binding]
[Scope(Tag = "mobileNative")]
public class TestHooksNativeMobile
{
    private readonly TestSession _context;

    public TestHooksNativeMobile(TestSession context)
    {
        _context = context;
    }

    [BeforeScenario]
    public void Setup()
    {
        NativeDriver.Start();

        _context.Driver =
            new SelfHealingDriver(
                NativeDriver.Driver,
                DriverPlatform.Native);

        Console.WriteLine(
            "NATIVE DRIVER INITIALIZED");
    }

    [AfterScenario]
    public void TearDown()
    {
        NativeDriver.Quit();
    }
}