#pragma warning disable SKEXP0080

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Process.Runtime;

public class ProcessingStep : KernelProcessStep
{

    [KernelFunction]
    public async Task CreateNumberEventAsync(KernelProcessStepContext context, Kernel _kernel, PurchaseOrder purchaseOrder)
    {
        Console.WriteLine("Processing Purchase Order...");
        string poRulesPrompt = $"""
        You are a helpful assistant for processing purchase orders. Based on the provided purchase order details, you will need to indicate a return event.
        The purchase order amount is {purchaseOrder.Amount:C}. If that amount is less than $500, respond with PO_AMOUNT_OKAY.
        After that, we need to review the purchase order details and determine the appropriate next steps.
        The department is {purchaseOrder.BuyerDepartment}, and the vendor is {purchaseOrder.VendorName}.
        If the department is "IT" They don't have a max limit, so you can respond with PO_AMOUNT_OKAY regardless of the amount, or the vendor.
        If the department is "HR" and the amount is greater than $250, respond with PO_AMOUNT_TOO_HIGH unless the vendor is "Corpo Clothes", and the purchase amount is less than $500, then respond with PO_AMOUNT_OKAY.
        If the department is "Marketing" and the amount is greater than $500, respond with PO_AMOUNT_TOO_HIGH unless the vendor is "Tech Supplies Inc.", and the purchase amount is less than $750, then respond with PO_AMOUNT_OKAY.
        If none of these conditions are met, respond with PO_INCONCLUSIVE.
        """;

        var answer = await _kernel.InvokePromptAsync(poRulesPrompt);
        Console.WriteLine($"PO Processing result: {answer}");

        await context.EmitEventAsync(answer.ToString(), purchaseOrder);
    }
}