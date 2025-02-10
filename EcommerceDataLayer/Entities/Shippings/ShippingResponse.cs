public class ShippingResponse
{
    public int ShippingID { get; set; }
    public int OrderID { get; set; }
    public string CarrierName { get; set; } = string.Empty;
    public string TrackingNumber { get; set; } = string.Empty;
    public string ShippingStatus { get; set; } = string.Empty;
    public DateTime EstimatedDeliveryDate { get; set; }
    public DateTime? ActualDeliveryDate { get; set; }

}
