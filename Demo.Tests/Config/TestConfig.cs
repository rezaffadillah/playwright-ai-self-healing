using Microsoft.Extensions.Configuration;

namespace Demo.Tests.Config;

public static class TestConfig
{
    public static string BaseUrl { get; }
    public static bool Headless { get; }
    public static string Browser { get; }

    static TestConfig()
    {
        var config =
            new ConfigurationBuilder()
                .SetBasePath(
                    Directory.GetCurrentDirectory())
                .AddJsonFile(
                    "appsettings.json",
                    optional:false)
                .Build();

        BaseUrl =
            Environment.GetEnvironmentVariable("BASE_URL")
            ?? config["TestSettings:BaseUrl"]
            ?? throw new Exception("BaseUrl missing");

        Browser =
            Environment.GetEnvironmentVariable("BROWSER")
            ?? config["TestSettings:Browser"]
            ?? "Chromium";

        Headless =
            bool.Parse(
                Environment.GetEnvironmentVariable("HEADLESS")
                ?? config["TestSettings:Headless"]
                ?? "false");
    }
}