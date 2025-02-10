

namespace EcommerceLogicalLayer.IServices
{
    public interface IEmailService
    {

        public  Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
