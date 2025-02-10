

namespace EcommerceLogicalLayer.Helpers
{
    public class Erorr
    {
        public Erorr(string message, int? statusCode = null)
        {
            this.Message = message;
            this.statusCode = statusCode;
        }
        public string Message { get; }
        public int? statusCode { get; }


    }
}
