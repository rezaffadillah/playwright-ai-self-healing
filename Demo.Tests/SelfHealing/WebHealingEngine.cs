namespace Demo.Tests.SelfHealing;

using Microsoft.Playwright;
using SauceDemo.Tests.Utils;

internal static class WebHealingEngine
{
    public static async Task<ILocator> Find(
        IPage page,
        LocatorHint hint)
    {
        var keyword = hint.Keyword.ToLower();

        // STRICT
        if (!string.IsNullOrEmpty(hint.Css))
        {
            var strict = page.Locator(hint.Css).First;

            if (await strict.CountAsync() > 0 &&
                await strict.IsVisibleAsync())
            {
                Console.WriteLine($"[STRICT WEB] {hint.Css}");
                return strict;
            }

            Console.WriteLine("[HEALING TRIGGERED - WEB]");
        }

        // EXACT
        var exactSelectors = new List<string>
        {
            $"[data-test='{keyword}']",
            $"[data-test='{keyword}-button']",
            $"#{keyword}",
            $"[name='{keyword}']"
        };

        foreach (var s in exactSelectors)
        {
            var el = page.Locator(s).First;

            if (await el.CountAsync() > 0 &&
                await el.IsVisibleAsync() &&
                await el.IsEnabledAsync())
            {
                Console.WriteLine($"[HEALED EXACT] {s}");
                return el;
            }
        }

        // FILTER
        var selectors = hint.Type == ElementType.Button
            ? new[] { "button", "input[type='submit']" }
            : new[] { "input", "textarea" };

        var candidates = new List<(ILocator el, int score)>();

        foreach (var sel in selectors)
        {
            var elements = page.Locator(sel);
            var count = await elements.CountAsync();

            for (int i = 0; i < count; i++)
            {
                var el = elements.Nth(i);

                if (!await el.IsVisibleAsync() ||
                    !await el.IsEnabledAsync())
                    continue;

                var tag = await el.EvaluateAsync<string>(
                    "e => e.tagName.toLowerCase()");

                var type = (
                    await el.GetAttributeAsync("type")
                    ?? "").ToLower();

                if (hint.Type == ElementType.Button &&
                    tag != "button" &&
                    type != "submit")
                    continue;

                if (hint.Type == ElementType.Input &&
                    tag != "input" &&
                    tag != "textarea")
                    continue;

                var dataTest =
                    (await el.GetAttributeAsync("data-test")
                    ?? "").ToLower();

                var id =
                    (await el.GetAttributeAsync("id")
                    ?? "").ToLower();

                var name =
                    (await el.GetAttributeAsync("name")
                    ?? "").ToLower();

                var text =
                    (await el.InnerTextAsync()
                    ?? "").ToLower();

                int score = 0;

                if (dataTest == keyword) score += 200;
                if (dataTest == $"{keyword}-button") score += 300;
                if (id == keyword) score += 150;
                if (name == keyword) score += 100;

                if (dataTest.Contains(keyword)) score += 50;
                if (text.Contains(keyword)) score += 20;

                if (score > 0)
                    candidates.Add((el, score));
            }
        }

        if (!candidates.Any())
            throw new Exception($"Element not found (WEB): {keyword}");

        return candidates
            .OrderByDescending(c => c.score)
            .First()
            .el;
    }
}