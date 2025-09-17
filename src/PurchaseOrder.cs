public class PurchaseOrder
{
    public decimal Amount { get; set; }
    public string PoNumber { get; set; }
    public string VendorName { get; set; }
    public string BuyerDepartment { get; set; }

    private static readonly string[] VendorNames = { "The Bookish Attic", "Tech Supplies Inc.", "Corpo Clothes" };
    private static readonly string[] BuyerDepartments = { "IT", "Marketing", "HR" };

    public static PurchaseOrder CreateRandomPurchaseOrder()
    {
        var po = new PurchaseOrder();
        po.PoNumber = $"PO-{Random.Shared.Next(1000, 9999)}";
        po.Amount = Random.Shared.Next(100, 1000);
        po.VendorName = VendorNames[Random.Shared.Next(VendorNames.Length)];
        po.BuyerDepartment = BuyerDepartments[Random.Shared.Next(BuyerDepartments.Length)];
        return po;
    }
}