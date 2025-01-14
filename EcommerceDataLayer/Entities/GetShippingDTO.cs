public class GetShippingDTO
{
    public int ShippingID { get; set; }
    public int OrderID { get; set; }
    public string CarrierName { get; set; }
    public string TrackingNumber { get; set; }
    public string ShippingStatus { get; set; }
    public DateTime? EstimatedDeliveryDate { get; set; }
    public DateTime? ActualDeliveryDate { get; set; }
}
