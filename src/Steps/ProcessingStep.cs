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
        You are a helpful assistant for processing purchase orders. Based on the provided purchase order details, determine the appropriate return event by following these rules in order:

        - The purchase order amount is {purchaseOrder.Amount:C}.
        - The department is {purchaseOrder.BuyerDepartment}.
        - The vendor is {purchaseOrder.VendorName}.

        Rules:
        1. If the department is "IT", respond with PO_AMOUNT_OKAY (no max limit applies).
        2. If the department is "HR":
        - If the amount > $250, respond with PO_AMOUNT_TOO_HIGH.
        - Exception: If the vendor is "Corpo Clothes" and the amount < $500, respond with PO_AMOUNT_OKAY.
        3. If the department is "Marketing":
        - If the amount > $500, respond with PO_AMOUNT_TOO_HIGH.
        - Exception: If the vendor is "Tech Supplies Inc." and the amount < $750, respond with PO_AMOUNT_OKAY.
        4. For all other departments:
        - If the amount < $500, respond with PO_AMOUNT_OKAY.
        - Otherwise, respond with PO_INCONCLUSIVE.

        If none of the above apply, respond with PO_INCONCLUSIVE.
        """;

        var answer = await _kernel.InvokePromptAsync(poRulesPrompt);
        Console.WriteLine($"PO Processing result: {answer}");

        await context.EmitEventAsync(answer.ToString(), purchaseOrder);
    }
}