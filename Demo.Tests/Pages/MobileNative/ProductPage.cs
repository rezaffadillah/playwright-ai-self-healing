using Demo.Tests.SelfHealing;

namespace Demo.Tests.Pages;

public class ProductPage : BasePage
{
    public ProductPage(SelfHealingDriver driver) : base(driver)
    {
    }

    private const string ProductId = "com.saucelabs.mydemoapp.android:id/productTV-Reza"; //For testing purposes, this is not the correct resourceId for the product name. The correct one is: com.saucelabs.mydemoapp.android:id/productTV-Backpack
    private const string ItemBackpack = "com.saucelabs.mydemoapp.android:id/productIV-Reza"; //For testing purposes, this is not the correct resourceId for the backpack item. The correct one is: com.saucelabs.mydemoapp.android:id/productIV-Backpack

    public async Task ProductIsDisplayed()
    {
        Assert.That(await IsDisplayed(keyword: "product", resourceId: ProductId), Is.True);
    }

    public async Task OpenBackpack()
    {
        Console.WriteLine("1");
        await Click(keyword: "backpack", resourceId: ItemBackpack);
        Console.WriteLine("2");
    }
}