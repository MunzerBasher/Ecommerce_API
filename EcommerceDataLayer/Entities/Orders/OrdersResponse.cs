namespace EcommerceDataLayer.Entities.Orders
{
    public class OrdersResponse
    {
        public int OrderID { get; set; }
        public required int UserID { get; set; }
        public DateTime OrderDate { get; set; }
        public required decimal TotalAmount { get; set; }
        public int Status { get; set; }

    }
}
