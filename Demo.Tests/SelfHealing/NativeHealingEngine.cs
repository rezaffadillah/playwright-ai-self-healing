using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using SauceDemo.Tests.Utils;
using System.Diagnostics;

namespace Demo.Tests.SelfHealing;

internal static class NativeHealingEngine
{
    public static IWebElement Find(
        IWebDriver driver,
        LocatorHint hint)
    {
        var keyword =
            hint.Keyword?.Trim().ToLower() ?? "";

        Console.WriteLine(
            $"SEARCHING : {hint.Keyword}");

        Console.WriteLine(
            $"RESOURCE  : {hint.ResourceId}");

        Console.WriteLine(
            $"ACCESS ID : {hint.AccessibilityId}");

        Console.WriteLine(
            $"TEXT      : {hint.Text}");

        Console.WriteLine(
            $"TYPE      : {hint.Type}");

        // =====================================================
        // RESOURCE ID (HIGHEST PRIORITY)
        // =====================================================

        if (!string.IsNullOrWhiteSpace(
                hint.ResourceId))
        {
            try
            {
                var el = driver.FindElement(
                    By.Id(
                        hint.ResourceId));

                Console.WriteLine(
                    $"[RESOURCE ID] {hint.ResourceId}");

                return el;
            }
            catch
            {
                Console.WriteLine(
                    $"[RESOURCE ID FAILED] {hint.ResourceId}");
            }
        }

        // =====================================================
        // ACCESSIBILITY ID
        // =====================================================

        if (!string.IsNullOrWhiteSpace(
                hint.AccessibilityId))
        {
            try
            {
                var el = driver.FindElement(
                    MobileBy.AccessibilityId(
                        hint.AccessibilityId));

                Console.WriteLine(
                    $"[ACCESSIBILITY ID] {hint.AccessibilityId}");

                return el;
            }
            catch
            {
                Console.WriteLine(
                    $"[ACCESSIBILITY ID FAILED] {hint.AccessibilityId}");
            }
        }

        // =====================================================
        // TEXT
        // =====================================================

        if (!string.IsNullOrWhiteSpace(
                hint.Text))
        {
            try
            {
                var el = driver.FindElement(
                    By.XPath(
                        $"//*[@text='{hint.Text}']"));

                Console.WriteLine(
                    $"[TEXT] {hint.Text}");

                return el;
            }
            catch
            {
                Console.WriteLine(
                    $"[TEXT FAILED] {hint.Text}");
            }
        }

        // =====================================================
        // STRICT (CSS USED AS ID)
        // =====================================================

        if (!string.IsNullOrWhiteSpace(
                hint.Css))
        {
            try
            {
                var el = driver.FindElement(
                    By.Id(hint.Css));

                Console.WriteLine(
                    $"[STRICT] {hint.Css}");

                return el;
            }
            catch
            {
                Console.WriteLine(
                    "[STRICT FAILED]");
            }
        }

        // =====================================================
        // ACCESSIBILITY BY KEYWORD
        // =====================================================

        try
        {
            var el = driver.FindElement(
                MobileBy.AccessibilityId(
                    keyword));

            Console.WriteLine(
                "[HEALED ACCESSIBILITY ID]");

            return el;
        }
        catch
        {
        }

        // =====================================================
        // RESOURCE ID EXACT
        // =====================================================

        try
        {
            var els = driver.FindElements(
                By.XPath(
                    $"//*[@resource-id='{keyword}']"));

            if (els.Count > 0)
            {
                Console.WriteLine(
                    "[HEALED RESOURCE EXACT]");

                return els.First();
            }
        }
        catch
        {
        }

        // =====================================================
        // RESOURCE ID CONTAINS
        // =====================================================

        try
        {
            var els = driver.FindElements(
                By.XPath(
                    $"//*[contains(@resource-id,'{keyword}')]"));

            if (els.Count > 0)
            {
                Console.WriteLine(
                    "[HEALED RESOURCE]");

                return els.First();
            }
        }
        catch
        {
        }

        // =====================================================
        // TEXT EXACT
        // =====================================================

        try
        {
            var els = driver.FindElements(
                By.XPath(
                    $"//*[@text='{keyword}']"));

            if (els.Count > 0)
            {
                Console.WriteLine(
                    "[HEALED TEXT EXACT]");

                return els.First();
            }
        }
        catch
        {
        }

        // =====================================================
        // TEXT CONTAINS
        // =====================================================

        try
        {
            var els = driver.FindElements(
                By.XPath(
                    $"//*[contains(@text,'{keyword}')]"));

            if (els.Count > 0)
            {
                Console.WriteLine(
                    "[HEALED TEXT]");

                return els.First();
            }
        }
        catch
        {
        }

        // =====================================================
        // CONTENT DESC EXACT
        // =====================================================

        try
        {
            var els = driver.FindElements(
                By.XPath(
                    $"//*[@content-desc='{keyword}']"));

            if (els.Count > 0)
            {
                Console.WriteLine(
                    "[HEALED CONTENT DESC EXACT]");

                return els.First();
            }
        }
        catch
        {
        }

        // =====================================================
        // CONTENT DESC CONTAINS
        // =====================================================

        try
        {
            var els = driver.FindElements(
                By.XPath(
                    $"//*[contains(@content-desc,'{keyword}')]"));

            if (els.Count > 0)
            {
                Console.WriteLine(
                    "[HEALED CONTENT DESC]");

                return els.First();
            }
        }
        catch
        {
        }

        // =====================================================
        // SMART SCAN
        // =====================================================
        var sw = Stopwatch.StartNew();

        var candidates =
            driver.FindElements(
                By.XPath("//*"));

        Console.WriteLine(
            $"TOTAL ELEMENTS : {candidates.Count}");

        Console.WriteLine(
            $"SCAN TIME : {sw.ElapsedMilliseconds} ms");

        var scored =
            new List<(IWebElement Element, int Score)>();

        foreach (var el in candidates)
        {
            try
            {
                int score = 0;

                var text =
                    (el.Text ?? "")
                    .Trim()
                    .ToLower();

                var resourceId =
                    el.GetAttribute("resource-id")?
                    .ToLower() ?? "";

                var contentDesc =
                    el.GetAttribute("content-desc")?
                    .ToLower() ?? "";

                var className =
                    el.GetAttribute("class")?
                    .ToLower() ?? "";

                var clickable =
                    el.GetAttribute("clickable")?
                    .ToLower() ?? "";

                var focusable =
                    el.GetAttribute("focusable")?
                    .ToLower() ?? "";

                //--------------------------------
                // RESOURCE ID
                //--------------------------------

                if (resourceId == keyword)
                    score += 1500;

                if (resourceId.Contains(keyword))
                    score += 1000;

                //--------------------------------
                // TEXT
                //--------------------------------

                if (text == keyword)
                    score += 500;

                if (text.Contains(keyword))
                    score += 250;

                //--------------------------------
                // CONTENT DESC
                //--------------------------------

                if (contentDesc == keyword)
                    score += 500;

                if (contentDesc.Contains(keyword))
                    score += 250;

                //--------------------------------
                // DISPLAY
                //--------------------------------

                if (el.Displayed)
                    score += 50;

                if (el.Enabled)
                    score += 50;

                //--------------------------------
                // BUTTON BONUS
                //--------------------------------

                if (hint.Type == ElementType.Button)
                {
                    if (clickable == "true")
                        score += 1000;

                    if (focusable == "true")
                        score += 300;

                    if (className.Contains("button"))
                        score += 500;

                    if (className.Contains("imageview"))
                        score += 500;

                    if (resourceId.Contains("product"))
                        score += 500;
                }

                //--------------------------------
                // INPUT BONUS
                //--------------------------------

                if (hint.Type == ElementType.Input)
                {
                    if (className.Contains("edittext"))
                        score += 1500;
                }

                if (score > 0)
                {
                    scored.Add((el, score));
                }
            }
            catch
            {
            }
        }

        if (!scored.Any())
        {
            throw new Exception(
                $"Native element not found : {hint.Keyword}");
        }

        var winner =
            scored
                .OrderByDescending(
                    x => x.Score)
                .First();

        Console.WriteLine(
            "================================");

        Console.WriteLine(
            $"SMART SCORE : {winner.Score}");

        Console.WriteLine(
            $"TEXT        : {winner.Element.Text}");

        Console.WriteLine(
            $"RESOURCE ID : {winner.Element.GetAttribute("resource-id")}");

        Console.WriteLine(
            $"CONTENT DESC: {winner.Element.GetAttribute("content-desc")}");

        Console.WriteLine(
            $"CLASS       : {winner.Element.GetAttribute("class")}");

        Console.WriteLine(
            $"CLICKABLE   : {winner.Element.GetAttribute("clickable")}");

        Console.WriteLine(
            $"FOCUSABLE   : {winner.Element.GetAttribute("focusable")}");

        Console.WriteLine(
            "================================");

        return winner.Element;
    }
}