using Microsoft.Playwright;

namespace Demo.Tests.SelfHealing;

internal static class PlaywrightEngine
{
    private const int DefaultTimeout = 10000;
    private const int RetryCount = 3;

    // =========================
    // CLICK
    // =========================
    public static async Task Click(
        IPage page,
        ILocator el)
    {
        Exception? lastError = null;

        for (int attempt = 1; attempt <= RetryCount; attempt++)
        {
            try
            {
                Console.WriteLine($"[CLICK] Attempt {attempt}");

                await WaitUntilVisible(el);
                await WaitUntilClickable(el);

                var beforeUrl = page.Url;

                await el.ScrollIntoViewIfNeededAsync();

                // NORMAL CLICK
                await el.ClickAsync(new()
                {
                    Timeout = DefaultTimeout
                });

                // WAIT NAVIGATION
                try
                {
                    await page.WaitForURLAsync(
                        url => url != beforeUrl,
                        new()
                        {
                            Timeout = 3000
                        });

                    Console.WriteLine("[NAVIGATION SUCCESS]");
                }
                catch
                {
                    Console.WriteLine("[NO NAVIGATION]");
                }

                return;
            }
            catch (Exception ex)
            {
                lastError = ex;

                Console.WriteLine(
                    $"[CLICK FAILED] Attempt {attempt}: {ex.Message}");

                // JS FALLBACK LAST RETRY
                if (attempt == RetryCount)
                {
                    try
                    {
                        Console.WriteLine("[JS CLICK FALLBACK]");

                        await el.EvaluateAsync(
                            "e => e.click()");

                        return;
                    }
                    catch (Exception jsEx)
                    {
                        lastError = jsEx;
                    }
                }

                await Task.Delay(1000);
            }
        }

        throw new Exception(
            $"Playwright click failed: {lastError?.Message}");
    }

    // =========================
    // TYPE
    // =========================
    public static async Task Type(
        IPage page,
        ILocator el,
        string value)
    {
        Exception? lastError = null;

        for (int attempt = 1; attempt <= RetryCount; attempt++)
        {
            try
            {
                Console.WriteLine($"[TYPE] Attempt {attempt}");

                await WaitUntilVisible(el);
                await WaitUntilClickable(el);

                await el.ScrollIntoViewIfNeededAsync();

                // CLICK FIRST
                await el.ClickAsync();

                // CLEAR
                await el.FillAsync("");

                // FILL
                await el.FillAsync(value);

                // VALIDATE
                var actual =
                    await el.InputValueAsync();

                if (actual == value)
                {
                    Console.WriteLine(
                        $"[INPUT OK] {actual}");

                    return;
                }

                Console.WriteLine(
                    $"[FILL FAILED] actual={actual}");

                // FALLBACK TYPE
                await el.FillAsync("");

                await el.PressSequentiallyAsync(value);

                actual =
                    await el.InputValueAsync();

                if (actual == value)
                {
                    Console.WriteLine(
                        "[SEQUENTIAL TYPE SUCCESS]");

                    return;
                }

                throw new Exception(
                    $"Typing validation failed. Expected={value}, Actual={actual}");
            }
            catch (Exception ex)
            {
                lastError = ex;

                Console.WriteLine(
                    $"[TYPE FAILED] Attempt {attempt}: {ex.Message}");

                await Task.Delay(1000);
            }
        }

        throw new Exception(
            $"Playwright typing failed: {lastError?.Message}");
    }

    // =========================
    // WAIT UNTIL VISIBLE
    // =========================
    public static async Task WaitUntilVisible(
        ILocator el,
        int timeout = DefaultTimeout)
    {
        await el.WaitForAsync(new()
        {
            State = WaitForSelectorState.Visible,
            Timeout = timeout
        });
    }

    // =========================
    // WAIT UNTIL CLICKABLE
    // =========================
    public static async Task WaitUntilClickable(
        ILocator el,
        int timeout = DefaultTimeout)
    {
        var start = DateTime.Now;

        while ((DateTime.Now - start).TotalMilliseconds < timeout)
        {
            try
            {
                if (await el.IsVisibleAsync() &&
                    await el.IsEnabledAsync())
                {
                    return;
                }
            }
            catch
            {
                // ignore transient errors
            }

            await Task.Delay(300);
        }

        throw new Exception(
            "Element not clickable");
    }
}