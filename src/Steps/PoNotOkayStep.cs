#pragma warning disable SKEXP0080
using Microsoft.SemanticKernel;
using System;
using System.IO;

public sealed class PoNotOkayStep : KernelProcessStep
{
    [KernelFunction]
    public async ValueTask ExecuteAsync(KernelProcessStepContext context, PurchaseOrder purchaseOrder)
    {
        // If we got here, the PO is not approved, so lets update the Purchase Order status.
        purchaseOrder.IsApproved = false;
        purchaseOrder.RejectionReason = "Purchase Order exceeds departmental spending limits.";

        // Append to CSV
        string csvLine = $"{purchaseOrder.PoNumber},{purchaseOrder.GrandTotal},{purchaseOrder.SupplierName},{purchaseOrder.BuyerDepartment}";
        File.AppendAllText("../Data/Orders.csv", csvLine + Environment.NewLine);
    }
}