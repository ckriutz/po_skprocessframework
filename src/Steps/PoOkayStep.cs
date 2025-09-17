#pragma warning disable SKEXP0080
using Microsoft.SemanticKernel;

public sealed class PoOkayStep : KernelProcessStep
{
    [KernelFunction]
    public async ValueTask ExecuteAsync(KernelProcessStepContext context, PurchaseOrder purchaseOrder)
    {
        Console.WriteLine($"Purchase Order for PO {purchaseOrder.PoNumber} Approved for amount of: {purchaseOrder.Amount}");
    }
}