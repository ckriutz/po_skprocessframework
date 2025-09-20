#pragma warning disable SKEXP0080
using Microsoft.SemanticKernel;

public sealed class PoOkayStep : KernelProcessStep
{
    [KernelFunction]
    public async ValueTask ExecuteAsync(KernelProcessStepContext context, PurchaseOrder purchaseOrder)
    {
        // If we got here, the PO is approved, so lets update the Purchase Order status.
        purchaseOrder.IsApproved = true;
    }
}