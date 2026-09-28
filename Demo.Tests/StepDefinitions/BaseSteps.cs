using Demo.Tests.Context;
using Demo.Tests.SelfHealing;

namespace Demo.Tests.StepDefinitions;

public abstract class BaseSteps
{
    protected readonly TestSession Context;
    protected readonly SelfHealingDriver Driver;

    protected BaseSteps(TestSession context)
    {
        Context = context;
        Driver = context.Driver 
            ?? throw new Exception("Driver is NULL");
    }
}