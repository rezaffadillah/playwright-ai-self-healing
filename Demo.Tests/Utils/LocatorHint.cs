namespace SauceDemo.Tests.Utils;

public enum ElementType
{
    Any,
    Button,
    Input,
    Select,
    Checkbox,
    Link,
    Radio
}

public class LocatorHint
{
    // Generic
    public string Keyword { get; set; } = "";
    public ElementType Type { get; set; } = ElementType.Any;

    public string? Label { get; set; }
    public string? Placeholder { get; set; }
    public string? Value { get; set; }
    public string[]? Aliases { get; set; }

    // Web
    public string? Css { get; set; }
    public string? XPath { get; set; }
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? ParentCss { get; set; }

    // Native
    public string? ResourceId { get; set; }
    public string? AccessibilityId { get; set; }
    public string? ClassName { get; set; }
    public string? UiAutomator { get; set; }
    public string? Text { get; set; }
    public string? Package { get; set; }
}