public class OrderItemDTO
{
    public  int OrderItemID { get; set; }
    public required int OrderID { get; set; }
    public required int ProductID { get; set; }
    public required int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal TotalItemsPrice { get; set; }
}
