using Microsoft.Playwright;
using Reqnroll;
using Demo.Tests.Context;
using Demo.Tests.SelfHealing;

namespace SauceDemo.Tests.Hooks;

[Binding]
[Scope(Tag = "web")]
public class TestHooks
{
    private readonly TestSession _context;

    private IPlaywright? _playwright;
    private IBrowser? _browser;

    public TestHooks(TestSession context)
    {
        _context = context;
    }

    [BeforeScenario]
    public async Task Setup()
    {
        _playwright = await Playwright.CreateAsync();

        _browser = await _playwright.Chromium.LaunchAsync(new()
        {
            Headless = false,
            SlowMo = 300
        });

        var browserContext = await _browser.NewContextAsync();
        var page = await browserContext.NewPageAsync();

        _context.Driver = new SelfHealingDriver(page);
    }

    [AfterScenario]
    public async Task TearDown()
    {
        await Task.Delay(2000);
        if (_browser != null)
            await _browser.CloseAsync();

        _playwright?.Dispose();
    }
}