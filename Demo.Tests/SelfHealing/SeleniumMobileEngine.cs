namespace Demo.Tests.SelfHealing;
using System.Diagnostics;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

internal static class SeleniumMobileEngine
{
    public static void Click(
        IWebDriver driver,
        IWebElement el,
        bool isNative = false)
    {
        Retry(() =>
        {
            Console.WriteLine("WAIT START");
            WaitUntilClickable(driver, el);
            Console.WriteLine("WAIT END");
            Console.WriteLine(
                $"CLICKING : {el.Text}");

            Console.WriteLine(
                $"CLICKABLE : {el.GetAttribute("clickable")}");

            // if (isNative)
            // {
            //     if (el.GetAttribute("clickable") == "true")
            //     {
            //         Console.WriteLine(
            //         "DIRECT CLICK");
            //         el.Click();
            //     }
            //     else
            //     {
            //         Console.WriteLine(
            //             "NATIVE TAP USING PARENT");

            //         var parent =
            //             el.FindElement(
            //                 By.XPath(".."));

            //         Console.WriteLine(
            //         "FIND PARENT END");

            //         Console.WriteLine(
            //             "PARENT CLICK");

            //         parent.Click();
            //         Console.WriteLine(
            //         "PARENT CLICK DONE");
            //     }

            //     return;
            // }
            if (isNative)
{
    var sw = Stopwatch.StartNew();

    if (el.GetAttribute("clickable") == "true")
    {
        el.Click();
    }
    else
    {
        Console.WriteLine(
            "NATIVE TAP USING PARENT");

        var parent =
            el.FindElement(
                By.XPath(".."));

        parent.Click();
    }

    sw.Stop();

    Console.WriteLine(
        $"CLICK TIME : {sw.ElapsedMilliseconds} ms");

    return;
}

            //---------------------------------
            // MOBILE WEB
            //---------------------------------

            try
            {
                el.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)driver)
                    .ExecuteScript(
                        "arguments[0].click();",
                        el);
            }
        });
    }

    public static void Type(
        IWebDriver driver,
        IWebElement el,
        string value)
    {
        Retry(() =>
        {
            WaitUntilVisible(driver, el);

            el.Clear();
            el.SendKeys(value);

            var actual = el.GetAttribute("value") ?? "";

            if (actual != value)
                throw new Exception("Typing failed");
        });
    }

    // private static void Retry(Action action, int retry = 2)
    // {
    //     Exception? last = null;

    //     for (int i = 0; i < retry; i++)
    //     {
    //         try
    //         {
    //             action();
    //             return;
    //         }
    //         catch (Exception ex)
    //         {
    //             last = ex;
    //             Thread.Sleep(500);
    //         }
    //     }

    //     throw last!;
    // }

    private static void Retry(
    Action action,
    int retry = 3)
{
    for (int i = 1; i <= retry; i++)
    {
        try
        {
            action();
            return;
        }
        catch
        {
            if (i == retry)
                throw;

            Thread.Sleep(500);
        }
    }
}

    private static void WaitUntilClickable(
        IWebDriver driver,
        IWebElement el)
    {
        var wait = new WebDriverWait(
            driver,
            TimeSpan.FromSeconds(10));

        wait.Until(_ =>
        {
            try
            {
                return el.Displayed && el.Enabled;
            }
            catch
            {
                return false;
            }
        });
    }

    private static void WaitUntilVisible(
        IWebDriver driver,
        IWebElement el)
    {
        var wait = new WebDriverWait(
            driver,
            TimeSpan.FromSeconds(10));

        wait.Until(_ =>
        {
            try
            {
                return el.Displayed;
            }
            catch
            {
                return false;
            }
        });
    }
}