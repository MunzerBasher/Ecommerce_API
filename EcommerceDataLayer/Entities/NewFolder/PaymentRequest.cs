namespace EcommerceDataLayer.Entities.NewFolder
{

    public class PaymentRequest
    {
      
        public long? Amount { get; set; }

        public DateTime Date { get; set; }

        public string UserId { get; set; }  = string.Empty;

    }


}
