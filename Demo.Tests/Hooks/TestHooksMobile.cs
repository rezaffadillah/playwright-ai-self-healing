using Reqnroll;
using Demo.Tests.Context;
using Demo.Tests.SelfHealing;
using Demo.Tests.Mobile.Drivers;

namespace Demo.Tests.Hooks;

[Binding]
[Scope(Tag = "mobile")]
public class TestHooksMobile
{
    private readonly TestSession _context;

    public TestHooksMobile(TestSession context)
    {
        _context = context;
    }

    [BeforeScenario]
    public void Setup()
    {
        MobileDriver.Start();

        _context.Driver =
            new SelfHealingDriver(
                MobileDriver.Driver,
                DriverPlatform.MobileWeb
            );
    }

    [AfterScenario]
    public async Task TearDown()
    {
        await Task.Delay(2000);
        MobileDriver.Driver?.Quit();
    }
}