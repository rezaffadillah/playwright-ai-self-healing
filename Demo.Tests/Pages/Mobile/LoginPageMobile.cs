using Demo.Tests.SelfHealing;

namespace Demo.Tests.Pages;

public class LoginPageMobile : BasePage
{
    public LoginPageMobile(SelfHealingDriver driver)
        : base(driver)
    {
    }

    private const string InpUsername = "input[data-test='username-wrong']"; //input[data-test='username']
    private const string InpPassword = "#password -wrong"; // #password
    private const string BtnLogin = "#login-button-wrong"; // #login-button

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

        await WaitForPageLoad();
    }

    public async Task Login(
        string username,
        string password)
    {
        await Type(
            "username",
            username,
            InpUsername);

        await Type(
            "password",
            password,
            InpPassword);

        await Click(
            "login",
            BtnLogin);

        await WaitForPageLoad();
        await Wait(1000);
    }

    public Task<bool> IsLoginSuccess()
    {
        if (Driver.IsWeb)
        {
            return Task.FromResult(
                Driver.PlaywrightPage!
                    .Url
                    .Contains("inventory"));
        }

        return Task.FromResult(
            Driver.SeleniumDriver!
                .Url
                .Contains("inventory"));
    }
}