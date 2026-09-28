using NUnit.Framework;
using Demo.Tests.Mobile.Drivers;

namespace Demo.Tests.MobileTests;

public class AndroidSmokeTest
{
    [SetUp]
    public void Setup()
    {
        MobileDriver.Start();
    }

    [TearDown]
    public void Cleanup()
    {
        MobileDriver.Quit();
    }

    [Test]
    public void OpenAndroidSettings()
    {
        var source =
            MobileDriver.Driver.PageSource;

        Assert.That(
            source.Contains("Settings"),
            Is.True
        );
    }
}