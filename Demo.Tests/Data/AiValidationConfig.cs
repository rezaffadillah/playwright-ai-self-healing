namespace SauceDemo.Tests.Data;

public class AiValidationConfig
{
    public List<string> PositiveKeywords { get; set; } = new();
    public List<string> NegativeKeywords { get; set; } = new();
    public int PassThreshold { get; set; } = 2;
}