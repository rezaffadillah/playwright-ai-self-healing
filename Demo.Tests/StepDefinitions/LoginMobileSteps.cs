using OpenQA.Selenium;
using Reqnroll;
using Demo.Tests.Mobile.Drivers;
using Demo.Tests.Config;
using OpenQA.Selenium.Support.UI;
using Demo.Tests.Context;
using Demo.Tests.Pages;

namespace Demo.Tests.StepDefinitions;

[Binding]
public class LoginMobileSteps
{
    private readonly LoginPageMobile _loginPage;

    public LoginMobileSteps(TestSession context)
    {
        _loginPage = new LoginPageMobile(context.Driver!);
    }

    [Given(@"user opens SauceDemo website in mobile chrome")]
    public async Task OpenSauceDemo()
    {
        MobileDriver.SwitchToWebView();
        await _loginPage.Navigate(TestConfigMobile.ApplicationUrl);
    }

    [When(@"user logs in on mobile with username ""(.*)"" and password ""(.*)""")]
    public async Task Login(string username, string password)
    {
        await _loginPage.Login(username, password);
    }

    [Then(@"mobile login should be ""(.*)""")]
    public async Task Verify(string result)
    {
        Assert.That(await _loginPage.IsLoginSuccess(), Is.True);
    }
}