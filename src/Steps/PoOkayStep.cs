#pragma warning disable SKEXP0080
using Microsoft.SemanticKernel;
using System;
using System.IO;

public sealed class PoOkayStep : KernelProcessStep
{
    [KernelFunction]
    public async ValueTask ExecuteAsync(KernelProcessStepContext context, PurchaseOrder purchaseOrder)
    {
        // If we got here, the PO is approved, so lets update the Purchase Order status.
        purchaseOrder.IsApproved = true;

        // Append to CSV
        Console.WriteLine("Writing approved PO to CSV");
        string csvLine = $"{purchaseOrder.PoNumber},{purchaseOrder.GrandTotal},{purchaseOrder.SupplierName},{purchaseOrder.BuyerDepartment},{purchaseOrder.Notes},{purchaseOrder.IsApproved}";
        File.AppendAllText("../Data/Orders.csv", csvLine + Environment.NewLine);
    }
}