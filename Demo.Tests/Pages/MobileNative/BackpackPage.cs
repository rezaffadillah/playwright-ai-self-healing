using Demo.Tests.SelfHealing;

namespace Demo.Tests.Pages;

public class BackpackPage : BasePage
{
    public BackpackPage(SelfHealingDriver driver)
        : base(driver)
    {
    }

    private const string CartButtonId = "com.saucelabs.mydemoapp.android:id/cartBt";

    public async Task BackpackIsDisplayed()
    {
        Assert.That(await IsDisplayed(resourceId: CartButtonId), Is.True);
    }
}