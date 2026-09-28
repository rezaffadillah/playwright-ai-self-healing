using NUnit.Framework;
using Reqnroll;
using Demo.Tests.Context;
using Demo.Tests.Pages;
using Demo.Tests.Config;

namespace Demo.Tests.StepDefinitions;

[Binding]
public class LoginSteps : BaseSteps
{
    private readonly LoginPage _loginPage;
    private readonly InventoryPage _inventoryPage;
    private readonly TestSession _context;
    private string _username = "";

    public LoginSteps(TestSession context) : base(context)
    {
        _context = context;

        var driver = _context.Driver 
            ?? throw new Exception("Driver is NULL");

        _loginPage = new LoginPage(driver);
        _inventoryPage = new InventoryPage(_context.Driver!);
    }

    [Given(@"user opens SauceDemo login page")]
    public async Task GivenOpenLogin()
        => await _loginPage.Navigate(TestConfig.BaseUrl);

    [When(@"user login with username ""(.*)"" and password ""(.*)""")]
    public async Task WhenLogin(string username, string password)
    {
        _username = username;
        await _loginPage.Login(username, password);
    }

    [Then(@"login result should be ""(.*)""")]
    public async Task ThenLoginResult(string result)
    {
        if (result == "success")
        {
            Assert.That(
                await _inventoryPage.IsInventoryPageLoaded(),
                Is.True,
                "User NOT redirected to inventory page"
            );

            Assert.That(
                _context.Driver!.GetUrl().Contains("inventory"),
                Is.True,
                "URL does not contain 'inventory'"
            );
        }
        else
        {
            Assert.That(
                await _inventoryPage.IsErrorVisible(),
                Is.True,
                "Error message not visible"
            );
        }
    }
}