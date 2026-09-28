using Demo.Tests.SelfHealing;

namespace Demo.Tests.Pages;

public class LoginPage : BasePage
{
    public LoginPage(SelfHealingDriver driver)
        : base(driver)
    {
    }

    private const string InpUsername = "#Reza-project-username";
    private const string InpPassword = "#Reza-project-password";
    private const string BtnLogin = "#login-button-wrong";
    public async Task Navigate(string url)
    {
        if (Driver.IsWeb)
        {
            await Driver.PlaywrightPage!
                .GotoAsync(url);
        }
        else
        {
            Driver.SeleniumDriver!
                .Navigate()
                .GoToUrl(url);
        }
    }

    public async Task Login(string user, string pass)
    {
        await Type("username", user, InpUsername);
        await Type("password", pass, InpPassword);
        await Click("login", BtnLogin);
        await WaitForPageLoad();
        await Wait(1000);
    }
}