using OpenQA.Selenium;

namespace Demo.Tests.SelfHealing;

public static class ElementValidator
{
    public static void Validate(IWebElement el)
    {
        if (el == null)
            throw new Exception("Element is null");

        if (!el.Displayed)
            throw new Exception("Element not visible");

        if (!el.Enabled)
            throw new Exception("Element not enabled");
    }
}