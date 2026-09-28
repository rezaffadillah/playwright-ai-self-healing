using OpenQA.Selenium;
using SauceDemo.Tests.Utils;

namespace Demo.Tests.SelfHealing;

internal static class MobileWebHealingEngine
{
    public static IWebElement Find(
        IWebDriver driver,
        LocatorHint hint)
    {
        Console.WriteLine(
            $"SEARCHING : {hint.Keyword}");

        Console.WriteLine(
            $"CSS       : {hint.Css}");

        Console.WriteLine(
            $"ID        : {hint.Id}");

        Console.WriteLine(
            $"NAME      : {hint.Name}");

        Console.WriteLine(
            $"TYPE      : {hint.Type}");

        // ==========================================
        // STRICT CSS
        // ==========================================

        if (!string.IsNullOrWhiteSpace(hint.Css))
        {
            try
            {
                var el = driver.FindElement(
                    By.CssSelector(hint.Css));

                Console.WriteLine(
                    $"[STRICT CSS] {hint.Css}");

                return el;
            }
            catch
            {
                Console.WriteLine(
                    "[STRICT CSS FAILED]");
            }
        }

        // ==========================================
        // STRICT ID
        // ==========================================

        if (!string.IsNullOrWhiteSpace(hint.Id))
        {
            try
            {
                var el = driver.FindElement(
                    By.Id(hint.Id));

                Console.WriteLine(
                    $"[STRICT ID] {hint.Id}");

                return el;
            }
            catch
            {
                Console.WriteLine(
                    "[STRICT ID FAILED]");
            }
        }

        // ==========================================
        // STRICT NAME
        // ==========================================

        if (!string.IsNullOrWhiteSpace(hint.Name))
        {
            try
            {
                var el = driver.FindElement(
                    By.Name(hint.Name));

                Console.WriteLine(
                    $"[STRICT NAME] {hint.Name}");

                return el;
            }
            catch
            {
                Console.WriteLine(
                    "[STRICT NAME FAILED]");
            }
        }

        // ==========================================
        // STRICT XPATH
        // ==========================================

        if (!string.IsNullOrWhiteSpace(hint.XPath))
        {
            try
            {
                var el = driver.FindElement(
                    By.XPath(hint.XPath));

                Console.WriteLine(
                    $"[STRICT XPATH] {hint.XPath}");

                return el;
            }
            catch
            {
                Console.WriteLine(
                    "[STRICT XPATH FAILED]");
            }
        }

        // ==========================================
        // SMART HEALING
        // ==========================================

        var keyword =
            hint.Keyword
                .Trim()
                .ToLower();

        var candidates =
            GetCandidates(
                driver,
                hint.Type);

        var scored =
            new List<(IWebElement Element, int Score)>();

        foreach (var el in candidates)
        {
            try
            {
                if (!el.Displayed)
                    continue;

                if (!el.Enabled)
                    continue;

                int score =
                    ScoreElement(
                        el,
                        keyword);

                if (score > 0)
                {
                    scored.Add(
                        (el, score));
                }
            }
            catch
            {
            }
        }

        if (!scored.Any())
        {
            throw new Exception(
                $"Element not found (Mobile Web): {keyword}");
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
            $"ID          : {winner.Element.GetAttribute("id")}");

        Console.WriteLine(
            $"NAME        : {winner.Element.GetAttribute("name")}");

        Console.WriteLine(
            $"TYPE        : {winner.Element.GetAttribute("type")}");

        Console.WriteLine(
            $"DATA-TEST   : {winner.Element.GetAttribute("data-test")}");

        Console.WriteLine(
            "================================");

        return winner.Element;
    }

    // ====================================================
    // BUILD CANDIDATES
    // ====================================================

    private static IReadOnlyCollection<IWebElement>
        GetCandidates(
            IWebDriver driver,
            ElementType type)
    {
        string css =
            type switch
            {
                ElementType.Button =>
                    @"button,
                      input[type='submit'],
                      input[type='button'],
                      input[type='image'],
                      [role='button'],
                      a.button,
                      .btn",

                ElementType.Input =>
                    @"input,
                      textarea",

                ElementType.Checkbox =>
                    @"input[type='checkbox']",

                ElementType.Radio =>
                    @"input[type='radio']",

                ElementType.Select =>
                    @"select",

                ElementType.Link =>
                    @"a",

                _ =>
                    @"input,
                      textarea,
                      button,
                      select,
                      a,
                      [role='button']"
            };

        return driver.FindElements(
            By.CssSelector(css));
    }

    // ====================================================
    // SMART SCORING
    // ====================================================

    private static int ScoreElement(
        IWebElement el,
        string keyword)
    {
        int score = 0;

        string id =
            Get(el, "id");

        string name =
            Get(el, "name");

        string dataTest =
            Get(el, "data-test");

        string ariaLabel =
            Get(el, "aria-label");

        string placeholder =
            Get(el, "placeholder");

        string title =
            Get(el, "title");

        string value =
            Get(el, "value");

        string type =
            Get(el, "type");

        string role =
            Get(el, "role");

        string text =
            (el.Text ?? "")
            .Trim()
            .ToLower();

        //-----------------------------------------
        // EXACT
        //-----------------------------------------

        if (id == keyword)
            score += 500;

        if (name == keyword)
            score += 500;

        if (dataTest == keyword)
            score += 500;

        if (placeholder == keyword)
            score += 450;

        if (ariaLabel == keyword)
            score += 450;

        if (value == keyword)
            score += 450;

        if (title == keyword)
            score += 400;

        //-----------------------------------------
        // CONTAINS
        //-----------------------------------------

        if (id.Contains(keyword))
            score += 200;

        if (name.Contains(keyword))
            score += 200;

        if (dataTest.Contains(keyword))
            score += 200;

        if (placeholder.Contains(keyword))
            score += 180;

        if (ariaLabel.Contains(keyword))
            score += 180;

        if (title.Contains(keyword))
            score += 180;

        if (value.Contains(keyword))
            score += 150;

        if (text.Contains(keyword))
            score += 120;

        //-----------------------------------------
        // BUTTON BONUS
        //-----------------------------------------

        if (keyword.Contains("login"))
        {
            if (type == "submit")
                score += 300;

            if (role == "button")
                score += 250;
        }

        //-----------------------------------------
        // USERNAME BONUS
        //-----------------------------------------

        if (keyword.Contains("user"))
        {
            if (placeholder.Contains("user"))
                score += 250;

            if (name.Contains("user"))
                score += 250;
        }

        //-----------------------------------------
        // PASSWORD BONUS
        //-----------------------------------------

        if (keyword.Contains("password"))
        {
            if (type == "password")
                score += 350;
        }

        return score;
    }

    // ====================================================
    // ATTRIBUTE
    // ====================================================

    private static string Get(
        IWebElement el,
        string attr)
    {
        return
            el.GetAttribute(attr)?
            .Trim()
            .ToLower()
            ?? "";
    }
}