using Reqnroll;
using Demo.Tests.Context;
using Demo.Tests.Pages;

namespace Demo.Tests.StepDefinitions;

[Binding]
public class MyDemoAppSteps
{
    private readonly ProductPage _productPage;
    private readonly BackpackPage _backpackPage;

    public MyDemoAppSteps(TestSession context)
    {
        _productPage = new ProductPage(context.Driver!);
        _backpackPage = new BackpackPage(context.Driver!);
    }

    [Given(@"My Demo App is installed")]
    public void Installed()
    {
        Console.WriteLine("APK will be installed by Appium");
    }

    [When(@"user opens My Demo App")]
    public void Open()
    {
        Console.WriteLine("App launched successfully");
    }

    [Then(@"Product page should be displayed")]
    public async Task VerifyProduct()
    {
        await _productPage.ProductIsDisplayed();
    }

    [When(@"User tap one of the product")]
    public async Task TapProduct()
    {
        Console.WriteLine(
    $"{DateTime.Now:HH:mm:ss.fff} - A");
        await _productPage.OpenBackpack();
        Console.WriteLine(
    $"{DateTime.Now:HH:mm:ss.fff} - B");
    }

    [Then(@"Backpack detail page should be displayed")]
    public async Task VerifyBackpack()
    {
        Console.WriteLine(
    $"{DateTime.Now:HH:mm:ss.fff} - VERIFY START");
        await _backpackPage.BackpackIsDisplayed();
        Console.WriteLine(
    $"{DateTime.Now:HH:mm:ss.fff} - VERIFY END");
    }
}