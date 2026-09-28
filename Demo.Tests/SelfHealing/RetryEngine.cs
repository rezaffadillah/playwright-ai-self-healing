using OpenQA.Selenium;

namespace Demo.Tests.SelfHealing;

internal static class RetryEngine
{
    private const int DefaultRetry = 3;
    private const int DefaultDelay = 1000;

    // ==========================================
    // RETRYABLE EXCEPTIONS
    // ==========================================

    private static readonly Type[] RetryableExceptions =
    {
        typeof(NoSuchElementException),
        typeof(StaleElementReferenceException),
        typeof(WebDriverTimeoutException),
        typeof(TimeoutException),
        typeof(WebDriverException)
    };

    // ==========================================
    // SHOULD RETRY?
    // ==========================================

    private static bool ShouldRetry(Exception ex)
    {
        if (RetryableExceptions.Any(
            x => x.IsAssignableFrom(ex.GetType())))
        {
            return true;
        }

        var message = ex.Message.ToLowerInvariant();

        return
            message.Contains("invalid session") ||
            message.Contains("stale element") ||
            message.Contains("timeout") ||
            message.Contains("no such element") ||
            message.Contains("session deleted");
    }

    // ==========================================
    // SYNC
    // ==========================================

    public static void Execute(
        Action action,
        int retry = DefaultRetry,
        int delay = DefaultDelay)
    {
        Exception? last = null;

        for (int attempt = 1; attempt <= retry; attempt++)
        {
            try
            {
                Console.WriteLine(
                    $"[RETRY] Attempt {attempt}/{retry}");

                action();

                return;
            }
            catch (Exception ex)
            {
                last = ex;

                Console.WriteLine(
                    $"[{ex.GetType().Name}] {ex.Message}");

                if (!ShouldRetry(ex))
                {
                    Console.WriteLine(
                        "[NON RETRYABLE]");

                    throw;
                }

                if (attempt < retry)
                {
                    Console.WriteLine(
                        $"Retry after {delay} ms...");

                    Thread.Sleep(delay);
                }
            }
        }

        throw new Exception(
            $"Retry failed after {retry} attempts.",
            last);
    }

    // ==========================================
    // ASYNC
    // ==========================================

    public static async Task ExecuteAsync(
        Func<Task> action,
        int retry = DefaultRetry,
        int delay = DefaultDelay)
    {
        Exception? last = null;

        for (int attempt = 1; attempt <= retry; attempt++)
        {
            try
            {
                Console.WriteLine(
                    $"[RETRY] Attempt {attempt}/{retry}");

                await action();

                return;
            }
            catch (Exception ex)
            {
                last = ex;

                Console.WriteLine(
                    $"[{ex.GetType().Name}] {ex.Message}");

                if (!ShouldRetry(ex))
                {
                    Console.WriteLine(
                        "[NON RETRYABLE]");

                    throw;
                }

                if (attempt < retry)
                {
                    Console.WriteLine(
                        $"Retry after {delay} ms...");

                    await Task.Delay(delay);
                }
            }
        }

        throw new Exception(
            $"Retry failed after {retry} attempts.",
            last);
    }

    // ==========================================
    // RETURN VALUE
    // ==========================================

    public static T Execute<T>(
        Func<T> action,
        int retry = DefaultRetry,
        int delay = DefaultDelay)
    {
        Exception? last = null;

        for (int attempt = 1; attempt <= retry; attempt++)
        {
            try
            {
                Console.WriteLine(
                    $"[RETRY] Attempt {attempt}/{retry}");

                return action();
            }
            catch (Exception ex)
            {
                last = ex;

                Console.WriteLine(
                    $"[{ex.GetType().Name}] {ex.Message}");

                if (!ShouldRetry(ex))
                {
                    Console.WriteLine(
                        "[NON RETRYABLE]");

                    throw;
                }

                if (attempt < retry)
                {
                    Console.WriteLine(
                        $"Retry after {delay} ms...");

                    Thread.Sleep(delay);
                }
            }
        }

        throw new Exception(
            $"Retry failed after {retry} attempts.",
            last);
    }

    // ==========================================
    // RETURN VALUE ASYNC
    // ==========================================

    public static async Task<T> ExecuteAsync<T>(
        Func<Task<T>> action,
        int retry = DefaultRetry,
        int delay = DefaultDelay)
    {
        Exception? last = null;

        for (int attempt = 1; attempt <= retry; attempt++)
        {
            try
            {
                Console.WriteLine(
                    $"[RETRY] Attempt {attempt}/{retry}");

                return await action();
            }
            catch (Exception ex)
            {
                last = ex;

                Console.WriteLine(
                    $"[{ex.GetType().Name}] {ex.Message}");

                if (!ShouldRetry(ex))
                {
                    Console.WriteLine(
                        "[NON RETRYABLE]");

                    throw;
                }

                if (attempt < retry)
                {
                    Console.WriteLine(
                        $"Retry after {delay} ms...");

                    await Task.Delay(delay);
                }
            }
        }

        throw new Exception(
            $"Retry failed after {retry} attempts.",
            last);
    }
}