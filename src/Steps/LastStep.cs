#pragma warning disable SKEXP0080

using Microsoft.SemanticKernel;

public sealed class LastStep : KernelProcessStep
{
    [KernelFunction("ExecuteAsyncApproved")]
    public async ValueTask ExecuteAsyncApproved(KernelProcessStepContext context)
    {
        Console.WriteLine("=== Purchase Order Processing Result ===");
        Console.WriteLine("Status: Approved");
        Console.WriteLine("The purchase order has been successfully processed.");
    }

    [KernelFunction("ExecuteAsyncNotApproved")]
    public async ValueTask ExecuteAsyncNotApproved(KernelProcessStepContext context)
    {
        Console.WriteLine("=== Purchase Order Processing Result ===");
        Console.WriteLine("Status: Not Approved");
        Console.WriteLine("The purchase order could not be approved. Please review and resubmit.");
    }
}
