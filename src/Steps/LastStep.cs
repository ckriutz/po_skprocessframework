#pragma warning disable SKEXP0080

using Microsoft.SemanticKernel;

public sealed class LastStep : KernelProcessStep
{
    [KernelFunction("ExecuteAsyncApproved")]
    public async ValueTask ExecuteAsyncApproved(KernelProcessStepContext context)
    {
        Console.WriteLine("Last Step - Purchase Order Approved!");
    }

    [KernelFunction("ExecuteAsyncNotApproved")]
    public async ValueTask ExecuteAsyncNotApproved(KernelProcessStepContext context)
    {
        Console.WriteLine("Last Step - Purchase Order Not Approved!");
    }
}
