using Demo.Tests.SelfHealing;
using OpenQA.Selenium;
using Demo.Tests.Pages;

namespace Demo.Tests.Pages;

public class InventoryPage : BasePage
{
    public InventoryPage(SelfHealingDriver driver) : base(driver)
    {
    }

    public async Task<bool> IsInventoryPageLoaded()
    {
        try
        {
            if (Driver.IsWeb)
            {
                // WAIT URL CHANGE
                await Driver.PlaywrightPage!
                    .WaitForURLAsync("**/inventory.html", new() { Timeout = 10000 });

                // WAIT ELEMENT
                await Driver.PlaywrightPage!
                    .Locator(".inventory_list")
                    .First
                    .WaitForAsync(new() { Timeout = 10000 });

                Console.WriteLine("Inventory page loaded (WEB)");

                await Driver.PlaywrightPage!.WaitForTimeoutAsync(1000);

                return true;
            }
            else
            {
                var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(
                    Driver.SeleniumDriver!,
                    TimeSpan.FromSeconds(10));

                return wait.Until(d =>
                    d.Url.Contains("inventory") ||
                    d.FindElements(By.ClassName("inventory_list")).Count > 0
                );
            }
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> IsCartVisible()
    {
        if (Driver.IsWeb)
        {
            return await Driver.PlaywrightPage!
                .Locator(".shopping_cart_link")
                .IsVisibleAsync();
        }
        else
        {
            return Driver.SeleniumDriver!
                .FindElements(By.ClassName("shopping_cart_link"))
                .Count > 0;
        }
    }

    public async Task<bool> IsErrorVisible()
    {
        if (Driver.IsWeb)
        {
            return await Driver.PlaywrightPage!
                .Locator("[data-test='error']")
                .IsVisibleAsync();
        }
        else
        {
            return Driver.SeleniumDriver!
                .FindElements(By.XPath("//*[contains(@data-test,'error')]"))
                .Count > 0;
        }
    }

    public async Task<string> GetPageText()
    {
        if (Driver.IsWeb)
        {
            return await Driver.PlaywrightPage!
                .InnerTextAsync("body");
        }
        else
        {
            return Driver.SeleniumDriver!
                .FindElement(By.TagName("body"))
                .Text;
        }
    }
}