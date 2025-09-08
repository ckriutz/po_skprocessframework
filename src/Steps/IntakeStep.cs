#pragma warning disable SKEXP0080

using Microsoft.SemanticKernel;

// In this initial step, we can do some pre-processing. In this case, we're just going to log the details.
// However we can imagine maybe reading in a PO, or parsing some JSON, reading an email, or something similar.
public sealed class IntakeStep : KernelProcessStep
{
    // Since this is a simple step and there is only one KernelFunction, we can use the [KernelFunction]
    // attribute without giving it a name. Also the method name can be called anything, but ExecuteAsync
    // seems to make the most sense.
    [KernelFunction]
    public async Task<PurchaseOrder> ExecuteAsync(KernelProcessStepContext context, PurchaseOrder purchaseOrder)
    {
        // This is the simplest step, emitting any events, or storing any state. Just returning the purchase order.
        Console.WriteLine($"Vendor: {purchaseOrder.VendorName}");
        Console.WriteLine($"Department: {purchaseOrder.BuyerDepartment}");
        Console.WriteLine($"Amount: ${purchaseOrder.Amount}");

        return purchaseOrder;
    }
}